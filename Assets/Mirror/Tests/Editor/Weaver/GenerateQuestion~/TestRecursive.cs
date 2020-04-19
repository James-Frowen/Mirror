using System;
using Mirror;

namespace Mirror.Weaver.Tests
{
    public class TestRecursive : NetworkBehaviour
    {
        [ClientRpc]
        public void RpcSendRecursive(SomeDataUsingRecursiveReference recursive)
        {
            // do
        }

        public struct SomeDataUsingRecursiveReference
        {
            public int id;
            public Recursive recursive;
        }
        public class Recursive
        {
            public int id;
            public Recursive recursive;
        }
    }
}
