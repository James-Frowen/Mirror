using System;
using System.Collections;
using UnityEngine;
using Mirror;

namespace MirrorTest
{
    class MirrorTestPlayer : MonoBehaviour
    {
        [Server]
        void ServerOnlyMethod() {}
    }
    class MirrorTestPlayerExtra : NetworkBehaviour
    {
        [Server]
        void ServerOnlyMethod() {}
    }
}
