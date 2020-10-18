using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mirror
{
    public interface IMessageWithNetId
    {
        uint NetId { get; }
    }
    /// <summary>
    /// Manager scenes over the network
    /// <para>Keeps track of: which scenes are loaded on clients, message to scene objects on clients which may not be loaded yet</para>
    /// </summary>
    public class NetworkSceneManager
    {
        Dictionary<NetworkConnection, SceneData> connectioScenes = new Dictionary<NetworkConnection, SceneData>();
        SceneData serverScenes;
        int _OperationIndex;
        int NextOperationIndex => _OperationIndex++;

        Dictionary<int, Operation> operations = new Dictionary<int, Operation>();
        private bool isLoading;
        private Queue<IMessageWithNetId> messageQueue = new Queue<IMessageWithNetId>();

        public NetworkSceneManager()
        {
            NetworkClient.RegisterHandler<LoadSceneMessage>(LoadSceneHandler);
            NetworkClient.RegisterHandler<UnloadSceneMessage>(UnloadSceneHandler);

            NetworkServer.RegisterHandler<LoadSceneFinishedMessage>(LoadSceneFinishedHandler);
            NetworkServer.RegisterHandler<UnloadSceneFinishedMessage>(UnloadSceneFinishedHandler);

            // todo find a way to get existing handler when replacing
            NetworkClient.ReplaceHandler<SpawnMessage>(SpawnMessageHandler);
            NetworkClient.ReplaceHandler<UpdateVarsMessage>(UpdateVarsMessageHandler);
            NetworkClient.ReplaceHandler<ObjectHideMessage>(ObjectHideMessageHandler);
            NetworkClient.ReplaceHandler<ObjectDestroyMessage>(ObjectDestroyMessageHandler);
        }

        private void ObjectDestroyMessageHandler(ObjectDestroyMessage msg)
        {
            if (isLoading)
            {
                messageQueue.Enqueue(msg);
            }
            else
            {
                ClientScene.OnSpawn(msg);
            }
        }

        private void ObjectHideMessageHandler(ObjectHideMessage msg)
        {
            if (isLoading)
            {
                messageQueue.Enqueue(msg);
            }
            else
            {
                ClientScene.OnSpawn(msg);
            }
        }

        private void UpdateVarsMessageHandler(UpdateVarsMessage msg)
        {
            if (isLoading)
            {
                messageQueue.Enqueue(msg);
            }
            else
            {
                ClientScene.OnSpawn(msg);
            }
        }

        void SpawnMessageHandler(SpawnMessage msg)
        {
            if (isLoading)
            {
                messageQueue.Enqueue(msg);
            }
            else
            {
                ClientScene.OnSpawn(msg);
            }
        }

        private void LoadSceneFinishedHandler(NetworkConnection conn, LoadSceneFinishedMessage msg)
        {

        }
        private void UnloadSceneFinishedHandler(NetworkConnection conn, UnloadSceneFinishedMessage msg)
        {
            throw new NotImplementedException();
        }


        private void LoadSceneHandler(NetworkConnection conn, LoadSceneMessage msg)
        {
            isLoading = true;
            // load
            AsyncOperation asyncOp = SceneManager.LoadSceneAsync(msg.nameOrPath, msg.mode);
            // on finish send message
            asyncOp.completed += _ =>
            {
                isLoading = false;
                conn.Send(new LoadSceneFinishedMessage { operationIndex = msg.operationIndex });
                //ClientScene.PrepareToSpawnSceneObjects();
                // invoke message locally
                NetworkClient.connection.InvokeHandler(new ObjectSpawnStartedMessage(), -1);
                // spawn all objects
                while (spawnQueue.Count > 0)
                {
                    ClientScene.OnSpawn(spawnQueue.Dequeue());
                }
                // invoke locally
                NetworkClient.connection.InvokeHandler(new ObjectSpawnFinishedMessage(), -1);
            };
        }
        private void UnloadSceneHandler(NetworkConnection conn, UnloadSceneMessage msg)
        {
            throw new NotImplementedException();
        }


        void OnConnect(NetworkConnection conn)
        {
            connectioScenes.Add(conn, new SceneData());
        }
        void OnDisconnect(NetworkConnection conn)
        {
            connectioScenes.Remove(conn);
        }

        public void LoadSceneForAll(string nameOrPath, LoadSceneMode loadSceneMode = default)
        {
            AsyncOperation asyncOp = SceneManager.LoadSceneAsync(nameOrPath, loadSceneMode);
            Operation op = new Operation(asyncOp, NextOperationIndex);
            operations.Add(op.index, op);
            NetworkServer.SendToAll(new LoadSceneMessage
            {
                nameOrPath = nameOrPath,
                mode = loadSceneMode,
                operationIndex = op.index,
            });
            asyncOp.completed += _ =>
            {
                //Scene scene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
                // todo tell NetworkServer to only prepare objects in scene
                NetworkServer.SpawnObjects();
            };
        }
        public void UnLoadSceneForAll(string nameOrPath, UnloadSceneOptions unloadSceneOptions = default)
        {
            AsyncOperation asyncOp = SceneManager.UnloadSceneAsync(nameOrPath, unloadSceneOptions);
            Operation op = new Operation(asyncOp, NextOperationIndex);
            operations.Add(op.index, op);
            NetworkServer.SendToAll(new UnloadSceneMessage
            {
                nameOrPath = nameOrPath,
                options = unloadSceneOptions,
                operationIndex = op.index,
            });
        }

    }
    public class Operation
    {
        public readonly AsyncOperation op;
        public readonly int index;

        public Operation(AsyncOperation op, int index)
        {
            this.op = op ?? throw new ArgumentNullException(nameof(op));
            this.index = index;
        }
    }
    public class SceneData
    {

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
