using NUnit.Framework;

namespace Mirror.Weaver.Tests
{
    public class WeaverClientRpcTests : BatchedWeaverTests
    {
        [BatchedTest(true)]
        public void ClientRpcValid()
        {
            Assert.Pass();
        }

        [BatchedTest(false)]
        public void ClientRpcCantBeStatic()
        {
            AssertHasError("RpcCantBeStatic must not be static (at System.Void WeaverClientRpcTests.ClientRpcCantBeStatic.ClientRpcCantBeStatic::RpcCantBeStatic())");
        }

        [BatchedTest(true)]
        public void VirtualClientRpc()
        {
            Assert.Pass();
        }

        [BatchedTest(true)]
        public void OverrideVirtualClientRpc()
        {
            Assert.Pass();
        }

        [BatchedTest(false)]
        public void AbstractClientRpc()
        {
            AssertHasError("Abstract ClientRpc are currently not supported, use virtual method instead (at System.Void WeaverClientRpcTests.AbstractClientRpc.AbstractClientRpc::RpcDoSomething())");
        }

        [BatchedTest(false)]
        public void OverrideAbstractClientRpc()
        {
            AssertHasError("Abstract ClientRpc are currently not supported, use virtual method instead (at System.Void WeaverClientRpcTests.OverrideAbstractClientRpc.BaseBehaviour::RpcDoSomething())");
        }

        [BatchedTest(true)]
        public void ClientRpcThatExcludesOwner()
        {
            Assert.Pass();
        }
    }
}
