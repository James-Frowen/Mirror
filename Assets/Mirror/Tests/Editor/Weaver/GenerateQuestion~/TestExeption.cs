using System;
using Mirror;

namespace Mirror.Weaver.Tests
{
    public class TestExeption : NetworkBehaviour
    {
        [ClientRpc]
        public void RpcSendExeption(ArgumentException exception)
        {
            // do
        }
    }
}
