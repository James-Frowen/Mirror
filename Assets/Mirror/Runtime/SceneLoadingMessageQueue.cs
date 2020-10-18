using System.Collections.Generic;

namespace Mirror
{
    public interface IMessageWithNetId
    {
        uint NetId { get; }
    }
    /// <summary>
    /// Stores message that are for a new scene that has not loaded yet
    /// </summary>
    public class SceneLoadingMessageQueue
    {
        Queue<IMessageWithNetId> messageQueue = new Queue<IMessageWithNetId>();
        bool isLoading;

        // todo use this class to get message that are for not loaded sceens
        public SceneLoadingMessageQueue()
        {
            // todo find a way to get existing handler when replacing
            //NetworkClient.ReplaceHandler<SpawnMessage>(SpawnMessageHandler);
            //NetworkClient.ReplaceHandler<UpdateVarsMessage>(UpdateVarsMessageHandler);
            //NetworkClient.ReplaceHandler<ObjectHideMessage>(ObjectHideMessageHandler);
            //NetworkClient.ReplaceHandler<ObjectDestroyMessage>(ObjectDestroyMessageHandler);
        }

        //private void ObjectDestroyMessageHandler(ObjectDestroyMessage msg)
        //{
        //    if (isLoading)
        //    {
        //        messageQueue.Enqueue(msg);
        //    }
        //    else
        //    {
        //        ClientScene.OnSpawn(msg);
        //    }
        //}

        //private void ObjectHideMessageHandler(ObjectHideMessage msg)
        //{
        //    if (isLoading)
        //    {
        //        messageQueue.Enqueue(msg);
        //    }
        //    else
        //    {
        //        ClientScene.OnSpawn(msg);
        //    }
        //}

        //private void UpdateVarsMessageHandler(UpdateVarsMessage msg)
        //{
        //    if (isLoading)
        //    {
        //        messageQueue.Enqueue(msg);
        //    }
        //    else
        //    {
        //        ClientScene.OnSpawn(msg);
        //    }
        //}

        //void SpawnMessageHandler(SpawnMessage msg)
        //{
        //    if (isLoading)
        //    {
        //        messageQueue.Enqueue(msg);
        //    }
        //    else
        //    {
        //        ClientScene.OnSpawn(msg);
        //    }
        //}
    }
}
