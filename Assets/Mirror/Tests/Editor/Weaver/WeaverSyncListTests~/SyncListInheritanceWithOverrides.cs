using Mirror;
using UnityEngine;

namespace MirrorTest
{
    class SyncListInheritanceWithOverrides : NetworkBehaviour
    {
        readonly SomeExtraList superSyncListString = new SomeExtraList();
    }

    // Type that cant have custom writer
    public class MyBehaviourWithValue : NetworkBehaviour
    {
        public Vector3 target;
    }

    public class SomeBaseList : SyncList<MyBehaviourWithValue>
    {
        protected override void SerializeItem(NetworkWriter writer, MyBehaviourWithValue item)
        {
            writer.WriteUInt32(item.netId);
            writer.WriteVector3(item.target);
        }
        protected override MyBehaviourWithValue DeserializeItem(NetworkReader reader)
        {
            NetworkIdentity item = NetworkIdentity.spawned[reader.ReadUInt32()];
            MyBehaviourWithValue behaviour = item.GetComponent<MyBehaviourWithValue>();

            behaviour.target = reader.ReadVector3();

            return behaviour;
        }
    }

    public class SomeExtraList : SomeBaseList
    {
        // do extra stuff here
    }
}
