using NUnit.Framework;

namespace Mirror.Weaver.Tests
{
    public class WeaverCommandTests : BatchedWeaverTests
    {
        [BatchedTest(true)]
        public void CommandValid()
        {
            Assert.Pass();
        }

        [BatchedTest(false)]
        public void CommandCantBeStatic()
        {
            AssertHasError("CmdCantBeStatic cannot be static (at System.Void WeaverCommandTests.CommandCantBeStatic.CommandCantBeStatic::CmdCantBeStatic())");
        }

        [BatchedTest(true)]
        public void CommandThatIgnoresAuthority()
        {
            Assert.Pass();
        }

        [BatchedTest(true)]
        public void CommandWithArguments()
        {
            Assert.Pass();
        }

        [BatchedTest(true)]
        public void CommandThatIgnoresAuthorityWithSenderConnection()
        {
            Assert.Pass();
        }

        [BatchedTest(true)]
        public void CommandWithSenderConnectionAndOtherArgs()
        {
            Assert.Pass();
        }

        [BatchedTest(false)]
        public void ErrorForOptionalNetworkConnectionThatIsNotSenderConnection()
        {
            AssertHasError("CmdFunction has invalid parameter connection, Cannot pass NetworkConnections. Instead use 'NetworkConnectionToClient conn = null' to get the sender's connection on the server (at System.Void WeaverCommandTests.ErrorForOptionalNetworkConnectionThatIsNotSenderConnection.ErrorForOptionalNetworkConnectionThatIsNotSenderConnection::CmdFunction(Mirror.NetworkConnection))");
        }

        [BatchedTest(false)]
        public void ErrorForNetworkConnectionThatIsNotSenderConnection()
        {
            AssertHasError("CmdFunction has invalid parameter connection, Cannot pass NetworkConnections. Instead use 'NetworkConnectionToClient conn = null' to get the sender's connection on the server (at System.Void WeaverCommandTests.ErrorForNetworkConnectionThatIsNotSenderConnection.ErrorForNetworkConnectionThatIsNotSenderConnection::CmdFunction(Mirror.NetworkConnection))");
        }

        [BatchedTest(true)]
        public void VirtualCommand()
        {
            Assert.Pass();
        }

        [BatchedTest(true)]
        public void OverrideVirtualCommand()
        {
            Assert.Pass();
        }

        [BatchedTest(true)]
        public void OverrideVirtualCallBaseCommand()
        {
            Assert.Pass();
        }

        [BatchedTest(true)]
        public void OverrideVirtualCallsBaseCommandWithMultipleBaseClasses()
        {
            Assert.Pass();
        }

        [BatchedTest(true)]
        public void OverrideVirtualCallsBaseCommandWithOverride()
        {
            Assert.Pass();
        }

        [BatchedTest(false)]
        public void AbstractCommand()
        {
            AssertHasError("Abstract Commands are currently not supported, use virtual method instead (at System.Void WeaverCommandTests.AbstractCommand.AbstractCommand::CmdDoSomething())");
        }

        [BatchedTest(false)]
        public void OverrideAbstractCommand()
        {
            AssertHasError("Abstract Commands are currently not supported, use virtual method instead (at System.Void WeaverCommandTests.OverrideAbstractCommand.BaseBehaviour::CmdDoSomething())");
        }
    }
}
