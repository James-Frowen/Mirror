using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror
{
    public class NetworkConnectionToClient : NetworkConnection
    {
        static readonly ILogger logger = LogFactory.GetLogger<NetworkConnectionToClient>();


        /// <summary>
        /// A list of the NetworkIdentity objects owned by this connection. This list is read-only.
        /// <para>This includes the player object for the connection - if it has localPlayerAutority set, and any objects spawned with local authority or set with AssignLocalAuthority.</para>
        /// <para>This list can be used to validate messages from clients, to ensure that clients are only trying to control objects that they own.</para>
        /// </summary>
        // IMPORTANT: this needs to be <NetworkIdentity>, not <uint netId>. fixes a bug where DestroyOwnedObjects wouldn't find
        //            the netId anymore: https://github.com/vis2k/Mirror/issues/1380 . Works fine with NetworkIdentity pointers though.
        readonly HashSet<NetworkIdentity> clientOwnedObjects = new HashSet<NetworkIdentity>();
        public IReadOnlyCollection<NetworkIdentity> ClientOwnedObjects => clientOwnedObjects;

        internal readonly HashSet<NetworkIdentity> visList = new HashSet<NetworkIdentity>();
        public readonly INetworkServer server;
        public readonly float disconnectInactiveTimeout;

        public NetworkConnectionToClient(INetworkServer server, int networkConnectionId, float disconnectInactiveTimeout) : base(networkConnectionId)
        {
            this.server = server ?? throw new ArgumentNullException(nameof(server));
            this.disconnectInactiveTimeout = disconnectInactiveTimeout;
        }

        public override string address => Transport.activeTransport.ServerGetClientAddress(connectionId);


        // internal because no one except Mirror should send bytes directly to
        // the client. they would be detected as a message. send messages instead.
        readonly List<int> singleConnectionId = new List<int> { -1 };

        // Failsafe to kick clients that have stopped sending anything to the server.
        // Clients ping the server every 2 seconds but transports are unreliable
        // when it comes to properly generating Disconnect messages to the server.
        internal override bool IsClientAlive() => Time.time - lastMessageTime < disconnectInactiveTimeout;

        internal override bool Send(ArraySegment<byte> segment, int channelId = Channels.DefaultReliable)
        {
            if (logger.LogEnabled()) logger.Log("ConnectionSend " + this + " bytes:" + BitConverter.ToString(segment.Array, segment.Offset, segment.Count));

            // validate packet size first.
            if (ValidatePacketSize(segment, channelId))
            {
                singleConnectionId[0] = connectionId;
                return Transport.activeTransport.ServerSend(singleConnectionId, channelId, segment);
            }
            return false;
        }

        // Send to many. basically Transport.Send(connections) + checks.
        internal static bool Send(List<int> connectionIds, ArraySegment<byte> segment, int channelId = Channels.DefaultReliable)
        {
            // validate packet size first.
            if (ValidatePacketSize(segment, channelId))
            {
                // only the server sends to many, we don't have that function on
                // a client.
                if (Transport.activeTransport.ServerActive())
                {
                    return Transport.activeTransport.ServerSend(connectionIds, channelId, segment);
                }
            }
            return false;
        }

        /// <summary>
        /// Disconnects this connection.
        /// </summary>
        public override void Disconnect()
        {
            // set not ready and handle clientscene disconnect in any case
            // (might be client or host mode here)
            isReady = false;
            Transport.activeTransport.ServerDisconnect(connectionId);
            RemoveObservers();
        }



        internal void AddToVisList(NetworkIdentity identity)
        {
            visList.Add(identity);

            // spawn identity for this conn
            if (isReady)
                identity.SendSpawnMessage(this);
        }

        internal void RemoveFromVisList(NetworkIdentity identity, bool isDestroyed)
        {
            visList.Remove(identity);

            if (!isDestroyed)
            {
                // hide identity for this conn
                Send(new ObjectHideMessage
                {
                    netId = identity.netId
                });
            }
        }

        internal void RemoveObservers()
        {
            foreach (NetworkIdentity identity in visList)
            {
                identity.RemoveObserverInternal(this);
            }
            visList.Clear();
        }



        internal void DestroyOwnedObjects()
        {
            // create a copy because the list might be modified when destroying
            HashSet<NetworkIdentity> tmp = new HashSet<NetworkIdentity>(clientOwnedObjects);
            foreach (NetworkIdentity netIdentity in tmp)
            {
                if (netIdentity != null)
                {
                    server.Destroy(netIdentity.gameObject);
                }
            }

            // clear the hashset because we destroyed them all
            clientOwnedObjects.Clear();
        }



        internal void AddOwnedObject(NetworkIdentity obj)
        {
            clientOwnedObjects.Add(obj);
        }

        internal void RemoveOwnedObject(NetworkIdentity obj)
        {
            clientOwnedObjects.Remove(obj);
        }
    }
}
