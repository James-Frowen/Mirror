using System;
using Mirror;
using UnityEngine;

namespace Mirror.Weaver.Tests
{
    public class TestCLip : NetworkBehaviour
    {
        [ClientRpc]
        public void RpcSendClip(AnimationClip clip)
        {
            // do
        }
    }
}
