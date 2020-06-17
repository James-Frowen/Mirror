// wraps Telepathy for use as HLAPI TransportLayer
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using Telepathy;
using UnityEngine;
using UnityEngine.Serialization;

namespace Mirror
{
    public class TelepathyTransportWithLag : Transport
    {
        struct Frame
        {
            public readonly int readFrame;
            public readonly Queue<Message> client;
            public readonly Queue<Message> server;

            public Frame(int readFrame) : this()
            {
                this.readFrame = readFrame;
                client = new Queue<Message>();
                server = new Queue<Message>();
            }
        }

        readonly Queue<Frame> lagFrames = new Queue<Frame>();
        public int framesOfLag;
        public float chanceToSkip;
        int _frame;

        private void GetReadFrame(out int writeFrame, out int readFrame)
        {
            // read message in lagFrames that have readFrame == readFrame
            readFrame = _frame;
            // write message to lagFrames with readFrame = writeFrame
            writeFrame = _frame + framesOfLag;
            while (UnityEngine.Random.value < chanceToSkip)
            {
                // max 2 times greater
                if (writeFrame >= _frame + framesOfLag * 10)
                {
                    break;
                }

                writeFrame++;
            }

            _frame++;
        }

        // scheme used by this transport
        // "tcp4" means tcp with 4 bytes header, network byte order
        public const string Scheme = "tcp4";

        public ushort port = 7777;

        [Tooltip("Nagle Algorithm can be disabled by enabling NoDelay")]
        public bool NoDelay = true;

        // Deprecated 04/08/2019
        [EditorBrowsable(EditorBrowsableState.Never), Obsolete("Use MaxMessageSizeFromClient or MaxMessageSizeFromServer instead.")]
        public int MaxMessageSize
        {
            get => serverMaxMessageSize;
            set => serverMaxMessageSize = clientMaxMessageSize = value;
        }

        [Header("Server")]
        [Tooltip("Protect against allocation attacks by keeping the max message size small. Otherwise an attacker might send multiple fake packets with 2GB headers, causing the server to run out of memory after allocating multiple large packets.")]
        [FormerlySerializedAs("MaxMessageSize")] public int serverMaxMessageSize = 16 * 1024;

        [Tooltip("Server processes a limit amount of messages per tick to avoid a deadlock where it might end up processing forever if messages come in faster than we can process them.")]
        public int serverMaxReceivesPerTick = 10000;

        [Header("Client")]
        [Tooltip("Protect against allocation attacks by keeping the max message size small. Otherwise an attacker host might send multiple fake packets with 2GB headers, causing the connected clients to run out of memory after allocating multiple large packets.")]
        [FormerlySerializedAs("MaxMessageSize")] public int clientMaxMessageSize = 16 * 1024;

        [Tooltip("Client processes a limit amount of messages per tick to avoid a deadlock where it might end up processing forever if messages come in faster than we can process them.")]
        public int clientMaxReceivesPerTick = 1000;


        protected Telepathy.Client client = new Telepathy.Client();
        protected Telepathy.Server server = new Telepathy.Server();

        void Awake()
        {
            // tell Telepathy to use Unity's Debug.Log
            Telepathy.Logger.Log = Debug.Log;
            Telepathy.Logger.LogWarning = Debug.LogWarning;
            Telepathy.Logger.LogError = Debug.LogError;

            // configure
            client.NoDelay = NoDelay;
            client.MaxMessageSize = clientMaxMessageSize;
            server.NoDelay = NoDelay;
            server.MaxMessageSize = serverMaxMessageSize;

            Debug.Log("TelepathyTransport initialized!");
        }

        public override bool Available()
        {
            // C#'s built in TCP sockets run everywhere except on WebGL
            return Application.platform != RuntimePlatform.WebGLPlayer;
        }

        // client
        public override bool ClientConnected() => client.Connected;
        public override void ClientConnect(string address) => client.Connect(address, port);
        public override void ClientConnect(Uri uri)
        {
            if (uri.Scheme != Scheme)
                throw new ArgumentException($"Invalid url {uri}, use {Scheme}://host:port instead", nameof(uri));

            int serverPort = uri.IsDefaultPort ? port : uri.Port;
            client.Connect(uri.Host, serverPort);
        }
        public override bool ClientSend(int channelId, ArraySegment<byte> segment)
        {
            // telepathy doesn't support allocation-free sends yet.
            // previously we allocated in Mirror. now we do it here.
            byte[] data = new byte[segment.Count];
            Array.Copy(segment.Array, segment.Offset, data, 0, segment.Count);
            return client.Send(data);
        }
        bool ProcessClientMessageFromLag(Queue<Message> messages)
        {
            if (messages.Count > 0)
            {
                Message message = messages.Dequeue();
                OnClientDataReceived.Invoke(new ArraySegment<byte>(message.data), Channels.DefaultReliable);
                return true;
            }
            return false;
        }
        void ProcessClientMessageIntoLag(Queue<Message> messages)
        {
            while (client.GetNextMessage(out Message message))
            {
                switch (message.eventType)
                {
                    case Telepathy.EventType.Connected:
                        OnClientConnected.Invoke();
                        break;
                    case Telepathy.EventType.Data:
                        messages.Enqueue(message);
                        break;
                    case Telepathy.EventType.Disconnected:
                        OnClientDisconnected.Invoke();
                        break;
                    default:
                        // TODO:  Telepathy does not report errors at all
                        // it just disconnects,  should be fixed
                        OnClientDisconnected.Invoke();
                        break;
                }
            }
        }
        public override void ClientDisconnect() => client.Disconnect();



        // IMPORTANT: set script execution order to >1000 to call Transport's
        //            LateUpdate after all others. Fixes race condition where
        //            e.g. in uSurvival Transport would apply Cmds before
        //            ShoulderRotation.LateUpdate, resulting in projectile
        //            spawns at the point before shoulder rotation.
        public void LateUpdate()
        {
            // note: we need to check enabled in case we set it to false
            // when LateUpdate already started.
            // (https://github.com/vis2k/Mirror/pull/379)
            if (!enabled)
                return;

            GetReadFrame(out int writeFrame, out int readFrame);

            Frame lagFrame = new Frame(writeFrame);
            ProcessClientMessageIntoLag(lagFrame.client);
            ProcessServerMessageIntoLag(lagFrame.server);
            lagFrames.Enqueue(lagFrame);

            // if empty, skip read
            // if next readFrame is later than current read frame, skip read
            while (lagFrames.Count != 0 && lagFrames.Peek().readFrame <= readFrame)
            {
                Frame frame = lagFrames.Dequeue();

                // process a maximum amount of client messages per tick
                // process a maximum amount of client messages per tick
                for (int i = 0; i < clientMaxReceivesPerTick; ++i)
                {
                    // stop when there is no more message
                    if (!ProcessClientMessageFromLag(frame.client))
                    {
                        break;
                    }

                    // Some messages can disable transport
                    // If this is disabled stop processing message in queue
                    if (!enabled)
                    {
                        break;
                    }
                }

                // process a maximum amount of server messages per tick
                for (int i = 0; i < serverMaxReceivesPerTick; ++i)
                {
                    // stop when there is no more message
                    if (!ProcessServerMessageFromLag(frame.server))
                    {
                        break;
                    }

                    // Some messages can disable transport
                    // If this is disabled stop processing message in queue
                    if (!enabled)
                    {
                        break;
                    }
                }
            }
        }

        public override Uri ServerUri()
        {
            UriBuilder builder = new UriBuilder();
            builder.Scheme = Scheme;
            builder.Host = Dns.GetHostName();
            builder.Port = port;
            return builder.Uri;
        }

        // server
        public override bool ServerActive() => server.Active;
        public override void ServerStart() => server.Start(port);
        public override bool ServerSend(List<int> connectionIds, int channelId, ArraySegment<byte> segment)
        {
            // telepathy doesn't support allocation-free sends yet.
            // previously we allocated in Mirror. now we do it here.
            byte[] data = new byte[segment.Count];
            Array.Copy(segment.Array, segment.Offset, data, 0, segment.Count);

            // send to all
            bool result = true;
            foreach (int connectionId in connectionIds)
                result &= server.Send(connectionId, data);
            return result;
        }

        bool ProcessServerMessageFromLag(Queue<Message> messages)
        {
            if (messages.Count > 0)
            {
                Message message = messages.Dequeue();
                OnServerDataReceived.Invoke(message.connectionId, new ArraySegment<byte>(message.data), Channels.DefaultReliable);
                return true;
            }
            return false;
        }
        void ProcessServerMessageIntoLag(Queue<Message> messages)
        {
            while (server.GetNextMessage(out Message message))
            {
                switch (message.eventType)
                {
                    case Telepathy.EventType.Connected:
                        OnServerConnected.Invoke(message.connectionId);
                        break;
                    case Telepathy.EventType.Data:
                        messages.Enqueue(message);
                        break;
                    case Telepathy.EventType.Disconnected:
                        OnServerDisconnected.Invoke(message.connectionId);
                        break;
                    default:
                        // TODO handle errors from Telepathy when telepathy can report errors
                        OnServerDisconnected.Invoke(message.connectionId);
                        break;
                }
            }
        }
        public override bool ServerDisconnect(int connectionId) => server.Disconnect(connectionId);
        public override string ServerGetClientAddress(int connectionId)
        {
            try
            {
                return server.GetClientAddress(connectionId);
            }
            catch (SocketException)
            {
                // using server.listener.LocalEndpoint causes an Exception
                // in UWP + Unity 2019:
                //   Exception thrown at 0x00007FF9755DA388 in UWF.exe:
                //   Microsoft C++ exception: Il2CppExceptionWrapper at memory
                //   location 0x000000E15A0FCDD0. SocketException: An address
                //   incompatible with the requested protocol was used at
                //   System.Net.Sockets.Socket.get_LocalEndPoint ()
                // so let's at least catch it and recover
                return "unknown";
            }
        }
        public override void ServerStop() => server.Stop();

        // common
        public override void Shutdown()
        {
            Debug.Log("TelepathyTransport Shutdown()");
            client.Disconnect();
            server.Stop();
        }

        public override int GetMaxPacketSize(int channelId)
        {
            return serverMaxMessageSize;
        }

        public override string ToString()
        {
            if (server.Active && server.listener != null)
            {
                // printing server.listener.LocalEndpoint causes an Exception
                // in UWP + Unity 2019:
                //   Exception thrown at 0x00007FF9755DA388 in UWF.exe:
                //   Microsoft C++ exception: Il2CppExceptionWrapper at memory
                //   location 0x000000E15A0FCDD0. SocketException: An address
                //   incompatible with the requested protocol was used at
                //   System.Net.Sockets.Socket.get_LocalEndPoint ()
                // so let's use the regular port instead.
                return "Telepathy Server port: " + port;
            }
            else if (client.Connecting || client.Connected)
            {
                return "Telepathy Client ip: " + client.client.Client.RemoteEndPoint;
            }
            return "Telepathy (inactive/disconnected)";
        }

    }
}
