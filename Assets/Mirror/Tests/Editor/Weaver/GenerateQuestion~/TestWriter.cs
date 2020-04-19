using System;
using Mirror;

namespace Mirror.Weaver.Tests
{
    public class TestWriter : NetworkBehaviour
    {
        [ClientRpc]
        public void RpcSendWriter(NetworkWriter writer)
        {
            // do
        }
    }
}
