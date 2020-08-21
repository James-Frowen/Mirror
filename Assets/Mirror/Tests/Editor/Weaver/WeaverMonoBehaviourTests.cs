using NUnit.Framework;

namespace Mirror.Weaver.Tests
{
    public class WeaverMonoBehaviourTests : BatchedWeaverTests
    {
        [BatchedTest(true)]
        public void MonoBehaviourValid()
        {
            Assert.Pass();
        }

        [BatchedTest(false)]
        public void MonoBehaviourSyncVar()
        {
            AssertHasError("SyncVar potato must be inside a NetworkBehaviour.  MonoBehaviourSyncVar is not a NetworkBehaviour (at System.Int32 WeaverMonoBehaviourTests.MonoBehaviourSyncVar.MonoBehaviourSyncVar::potato)");
        }

        [BatchedTest(false)]
        public void MonoBehaviourSyncList()
        {
            AssertHasError("potato is a SyncObject and must be inside a NetworkBehaviour.  MonoBehaviourSyncList is not a NetworkBehaviour (at Mirror.SyncListInt WeaverMonoBehaviourTests.MonoBehaviourSyncList.MonoBehaviourSyncList::potato)");
        }

        [BatchedTest(false)]
        public void MonoBehaviourCommand()
        {
            AssertHasError("Command CmdThisCantBeOutsideNetworkBehaviour must be declared inside a NetworkBehaviour (at System.Void WeaverMonoBehaviourTests.MonoBehaviourCommand.MonoBehaviourCommand::CmdThisCantBeOutsideNetworkBehaviour())");
        }

        [BatchedTest(false)]
        public void MonoBehaviourClientRpc()
        {
            AssertHasError("ClientRpc RpcThisCantBeOutsideNetworkBehaviour must be declared inside a NetworkBehaviour (at System.Void WeaverMonoBehaviourTests.MonoBehaviourClientRpc.MonoBehaviourClientRpc::RpcThisCantBeOutsideNetworkBehaviour())");
        }

        [BatchedTest(false)]
        public void MonoBehaviourTargetRpc()
        {
            AssertHasError("TargetRpc TargetThisCantBeOutsideNetworkBehaviour must be declared inside a NetworkBehaviour (at System.Void WeaverMonoBehaviourTests.MonoBehaviourTargetRpc.MonoBehaviourTargetRpc::TargetThisCantBeOutsideNetworkBehaviour(Mirror.NetworkConnection))");
        }

        [BatchedTest(true)]
        public void MonoBehaviourServer()
        {
            Assert.Pass();
        }

        [BatchedTest(true)]
        public void MonoBehaviourServerCallback()
        {
            Assert.Pass();
        }

        [BatchedTest(true)]
        public void MonoBehaviourClient()
        {
            Assert.Pass();
        }

        [BatchedTest(true)]
        public void MonoBehaviourClientCallback()
        {
            Assert.Pass();
        }
    }
}
