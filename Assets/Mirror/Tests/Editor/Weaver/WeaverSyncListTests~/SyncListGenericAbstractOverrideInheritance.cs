using Mirror;

namespace MirrorTest
{
    class SyncListGenericAbstractOverrideInheritance : NetworkBehaviour
    {
        readonly SomeListInt superSyncListString = new SomeListInt();
    }
    
    public abstract class SomeList<T> : SyncList<T>
    {
        protected abstract override T DeserializeItem(NetworkReader reader);
        protected abstract override void SerializeItem(NetworkWriter writer, T item);
    }
    public class SomeListInt : SomeList<int>
    {
        protected override int DeserializeItem(NetworkReader reader)
        {
            // do something
            return reader.ReadInt32();
        }

        protected override void SerializeItem(NetworkWriter writer, int item)
        {
            // do something 
            writer.WriteInt32(item);
        }
    }
}
