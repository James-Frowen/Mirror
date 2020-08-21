using NUnit.Framework;

namespace Mirror.Weaver.Tests
{
    public class WeaverTargetRpcTests : BatchedWeaverTests
    {
        [BatchedTest(true)]
        public void TargetRpcValid()
        {
            Assert.Pass();
        }

        [BatchedTest(false)]
        public void ErrorWhenTargetRpcIsStatic()
        {
            AssertHasError("TargetCantBeStatic must not be static " +
                "(at System.Void WeaverTargetRpcTests.ErrorWhenTargetRpcIsStatic.ErrorWhenTargetRpcIsStatic::TargetCantBeStatic(Mirror.NetworkConnection))");
        }

        [BatchedTest(true)]
        public void TargetRpcCanSkipNetworkConnection()
        {
            Assert.Pass();
        }

        [BatchedTest(true)]
        public void TargetRpcCanHaveOtherParametersWhileSkipingNetworkConnection()
        {
            Assert.Pass();
        }

        [BatchedTest(false)]
        public void ErrorWhenNetworkConnectionIsNotTheFirstParameter()
        {
            AssertHasError($"TargetRpcMethod has invalid parameter nc. Cannot pass NetworkConnections " +
                "(at System.Void WeaverTargetRpcTests.ErrorWhenNetworkConnectionIsNotTheFirstParameter.ErrorWhenNetworkConnectionIsNotTheFirstParameter::TargetRpcMethod(System.Int32,Mirror.NetworkConnection))");
        }

        [BatchedTest(true)]
        public void VirtualTargetRpc()
        {
            Assert.Pass();
        }

        [BatchedTest(true)]
        public void OverrideVirtualTargetRpc()
        {
            Assert.Pass();
        }

        [BatchedTest(false)]
        public void AbstractTargetRpc()
        {
            AssertHasError("Abstract TargetRpc are currently not supported, use virtual method instead (at System.Void WeaverTargetRpcTests.AbstractTargetRpc.AbstractTargetRpc::TargetDoSomething())");
        }

        [BatchedTest(false)]
        public void OverrideAbstractTargetRpc()
        {
            AssertHasError("Abstract TargetRpc are currently not supported, use virtual method instead (at System.Void WeaverTargetRpcTests.OverrideAbstractTargetRpc.BaseBehaviour::TargetDoSomething())");
        }
    }
}
