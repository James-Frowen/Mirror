using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mirror
{
    public interface INetworkSceneManager
    {
        event Action<LoadSceneMessage, AsyncOperation> onLoadStarted;
        event Action<LoadSceneMessage, AsyncOperation> onLoadFinished;
        event Action<UnloadSceneMessage, AsyncOperation> onUnloadStarted;
        event Action<UnloadSceneMessage, AsyncOperation> onUnloadFinished;
    }
    public class ClientSceneManager : INetworkSceneManager
    {
        static readonly ILogger logger = LogFactory.GetLogger<ClientSceneManager>();

        public event Action<LoadSceneMessage, AsyncOperation> onLoadStarted;
        public event Action<LoadSceneMessage, AsyncOperation> onLoadFinished;
        public event Action<UnloadSceneMessage, AsyncOperation> onUnloadStarted;
        public event Action<UnloadSceneMessage, AsyncOperation> onUnloadFinished;

        public ClientSceneManager()
        {
            NetworkClient.RegisterHandler<LoadSceneMessage>(LoadSceneHandler);
            NetworkClient.RegisterHandler<UnloadSceneMessage>(UnloadSceneHandler);
        }

        private void LoadSceneHandler(NetworkConnection conn, LoadSceneMessage msg)
        {
            PauseTransport();

            AsyncOperation asyncOp = SceneManager.LoadSceneAsync(msg.nameOrPath, msg.mode);
            onLoadStarted?.Invoke(msg, asyncOp);

            // send message on finish
            asyncOp.completed += _ =>
            {
                ResumeTransport();

                conn.Send(new LoadSceneFinishedMessage { operationIndex = msg.operationIndex });

                onLoadFinished?.Invoke(msg, asyncOp);

                // todo finish this method
                handleNewSceneObjects();
            };
        }

        private void handleNewSceneObjects()
        {
            // todo manage 
            ////ClientScene.PrepareToSpawnSceneObjects();
            //// invoke message locally
            //NetworkClient.connection.InvokeHandler(new ObjectSpawnStartedMessage(), -1);
            //// spawn all objects
            //while (spawnQueue.Count > 0)
            //{
            //    ClientScene.OnSpawn(spawnQueue.Dequeue());
            //}
            //// invoke locally
            //NetworkClient.connection.InvokeHandler(new ObjectSpawnFinishedMessage(), -1);
        }

        private static void PauseTransport()
        {
            // vis2k: pause message handling while loading scene. otherwise we will process messages and then lose all
            // the state as soon as the load is finishing, causing all kinds of bugs because of missing state.
            // (client may be null after StopClient etc.)
            if (logger.LogEnabled()) logger.Log("ClientChangeScene: pausing handlers while scene is loading to avoid data loss after scene was loaded.");
            Transport.activeTransport.enabled = false;
        }
        private static void ResumeTransport()
        {
            // process queued messages that we received while loading the scene
            logger.Log("FinishLoadScene: resuming handlers after scene was loading.");
            Transport.activeTransport.enabled = true;
        }

        private void UnloadSceneHandler(NetworkConnection conn, UnloadSceneMessage msg)
        {
            PauseTransport();

            AsyncOperation asyncOp = SceneManager.UnloadSceneAsync(msg.nameOrPath, msg.options);
            onUnloadStarted?.Invoke(msg, asyncOp);

            // send message on finish
            asyncOp.completed += _ =>
            {
                if (logger.LogEnabled()) logger.Log("ClientChangeScene done readyCon:" + NetworkClient.connection);

                ResumeTransport();
                conn.Send(new LoadSceneFinishedMessage { operationIndex = msg.operationIndex });
                onUnloadFinished?.Invoke(msg, asyncOp);

                handleNewSceneObjects();
            };
        }
    }

    /// <summary>
    /// Manager scenes over the network
    /// <para>Keeps track of: which scenes are loaded on clients, message to scene objects on clients which may not be loaded yet</para>
    /// </summary>
    public class ServerSceneManager : INetworkSceneManager
    {
        static readonly ILogger logger = LogFactory.GetLogger<ServerSceneManager>();
        Dictionary<int, Scene> localScenes;

        int _loadId;
        int GetGetNextLoadId() => _loadId++;

        public event Action<LoadSceneMessage, AsyncOperation> onLoadStarted;
        public event Action<LoadSceneMessage, AsyncOperation> onLoadFinished;
        public event Action<UnloadSceneMessage, AsyncOperation> onUnloadStarted;
        public event Action<UnloadSceneMessage, AsyncOperation> onUnloadFinished;

        public event Action<LoadSceneFinishedMessage, NetworkConnection> onClientLoadFinished;
        public event Action<UnloadSceneFinishedMessage, NetworkConnection> onClientUnloadFinished;


        // todo keep track of what scenes are loaded on each connection
        //Dictionary<NetworkConnection, SceneData> connectioScenes = new Dictionary<NetworkConnection, SceneData>();
        //SceneData serverScenes;
        //void OnConnect(NetworkConnection conn)
        //{
        //    //connectioScenes.Add(conn, new SceneData());
        //}
        //void OnDisconnect(NetworkConnection conn)
        //{
        //    //connectioScenes.Remove(conn);
        //}

        public ServerSceneManager()
        {
            NetworkServer.RegisterHandler<LoadSceneFinishedMessage>(LoadSceneFinishedHandler);
            NetworkServer.RegisterHandler<UnloadSceneFinishedMessage>(UnloadSceneFinishedHandler);
        }

        public void LoadSceneForAll(string nameOrPath, LoadSceneMode loadSceneMode = default)
        {
            AsyncOperation asyncOp = SceneManager.LoadSceneAsync(nameOrPath, loadSceneMode);
            int nextId = GetGetNextLoadId();
            LoadSceneMessage msg = new LoadSceneMessage
            {
                nameOrPath = nameOrPath,
                mode = loadSceneMode,
                operationIndex = nextId,
            };
            NetworkServer.SendToAll(msg);
            onLoadStarted?.Invoke(msg, asyncOp);
            asyncOp.completed += _ =>
            {
                onLoadStarted?.Invoke(msg, asyncOp);
            };
        }

        private void LoadSceneFinishedHandler(NetworkConnection conn, LoadSceneFinishedMessage msg)
        {
            onClientLoadFinished?.Invoke(msg, conn);
        }

        public void UnLoadSceneForAll(string nameOrPath, UnloadSceneOptions unloadSceneOptions = default)
        {
            AsyncOperation asyncOp = SceneManager.UnloadSceneAsync(nameOrPath, unloadSceneOptions);

            int nextId = GetGetNextLoadId();
            UnloadSceneMessage msg = new UnloadSceneMessage
            {
                nameOrPath = nameOrPath,
                options = unloadSceneOptions,
                operationIndex = nextId,
            };
            NetworkServer.SendToAll(msg);
            onUnloadStarted?.Invoke(msg, asyncOp);
            asyncOp.completed += _ =>
            {
                onUnloadFinished?.Invoke(msg, asyncOp);
            };
        }

        private void UnloadSceneFinishedHandler(NetworkConnection conn, UnloadSceneFinishedMessage msg)
        {
            onClientUnloadFinished?.Invoke(msg, conn);
        }
    }

    public struct LoadSceneMessage : NetworkMessage
    {
        public string nameOrPath;
        // todo use byte instead of LoadSceneMode
        public LoadSceneMode mode;
        public int operationIndex;
    }
    public struct LoadSceneFinishedMessage : NetworkMessage
    {
        public int operationIndex;
    }
    public struct UnloadSceneMessage : NetworkMessage
    {
        public string nameOrPath;
        // todo use byte instead of UnloadSceneOptions
        public UnloadSceneOptions options;
        public int operationIndex;
    }
    public struct UnloadSceneFinishedMessage : NetworkMessage
    {
        public int operationIndex;
    }
}
