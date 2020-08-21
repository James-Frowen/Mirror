using NUnit.Framework;

namespace Mirror.Weaver.Tests
{
    public class WeaverNetworkBehaviourTests : BatchedWeaverTests
    {
        [BatchedTest(true)]
        public void NetworkBehaviourValid()
        {
            Assert.Pass();
        }

        [BatchedTest(true)]
        public void NetworkBehaviourAbstractBaseValid()
        {
            Assert.Pass();
        }

        [BatchedTest(false)]
        public void NetworkBehaviourGeneric()
        {
            AssertHasError("NetworkBehaviourGeneric`1 cannot have generic parameters (at WeaverNetworkBehaviourTests.NetworkBehaviourGeneric.NetworkBehaviourGeneric`1)");
        }

        [BatchedTest(false)]
        public void NetworkBehaviourCmdGenericParam()
        {
            AssertHasError("CmdCantHaveGeneric cannot have generic parameters (at System.Void WeaverNetworkBehaviourTests.NetworkBehaviourCmdGenericParam.NetworkBehaviourCmdGenericParam::CmdCantHaveGeneric())");
        }

        [BatchedTest(false)]
        public void NetworkBehaviourCmdCoroutine()
        {
            AssertHasError("CmdCantHaveCoroutine cannot be a coroutine (at System.Collections.IEnumerator WeaverNetworkBehaviourTests.NetworkBehaviourCmdCoroutine.NetworkBehaviourCmdCoroutine::CmdCantHaveCoroutine())");
        }

        [BatchedTest(false)]
        public void NetworkBehaviourCmdVoidReturn()
        {
            AssertHasError("CmdCantHaveNonVoidReturn cannot return a value.  Make it void instead (at System.Int32 WeaverNetworkBehaviourTests.NetworkBehaviourCmdVoidReturn.NetworkBehaviourCmdVoidReturn::CmdCantHaveNonVoidReturn())");
        }

        [BatchedTest(false)]
        public void NetworkBehaviourTargetRpcGenericParam()
        {
            AssertHasError("TargetRpcCantHaveGeneric cannot have generic parameters (at System.Void WeaverNetworkBehaviourTests.NetworkBehaviourTargetRpcGenericParam.NetworkBehaviourTargetRpcGenericParam::TargetRpcCantHaveGeneric())");
        }

        [BatchedTest(false)]
        public void NetworkBehaviourTargetRpcCoroutine()
        {
            AssertHasError("TargetRpcCantHaveCoroutine cannot be a coroutine (at System.Collections.IEnumerator WeaverNetworkBehaviourTests.NetworkBehaviourTargetRpcCoroutine.NetworkBehaviourTargetRpcCoroutine::TargetRpcCantHaveCoroutine())");
        }

        [BatchedTest(false)]
        public void NetworkBehaviourTargetRpcVoidReturn()
        {
            AssertHasError("TargetRpcCantHaveNonVoidReturn cannot return a value.  Make it void instead (at System.Int32 WeaverNetworkBehaviourTests.NetworkBehaviourTargetRpcVoidReturn.NetworkBehaviourTargetRpcVoidReturn::TargetRpcCantHaveNonVoidReturn())");
        }

        [BatchedTest(false)]
        public void NetworkBehaviourTargetRpcParamOut()
        {
            AssertHasError("TargetRpcCantHaveParamOut cannot have out parameters (at System.Void WeaverNetworkBehaviourTests.NetworkBehaviourTargetRpcParamOut.NetworkBehaviourTargetRpcParamOut::TargetRpcCantHaveParamOut(Mirror.NetworkConnection,System.Int32&))");
        }

        [BatchedTest(false)]
        public void NetworkBehaviourTargetRpcParamOptional()
        {
            AssertHasError("TargetRpcCantHaveParamOptional cannot have optional parameters (at System.Void WeaverNetworkBehaviourTests.NetworkBehaviourTargetRpcParamOptional.NetworkBehaviourTargetRpcParamOptional::TargetRpcCantHaveParamOptional(Mirror.NetworkConnection,System.Int32))");
        }

        [BatchedTest(false)]
        public void NetworkBehaviourTargetRpcParamRef()
        {
            AssertHasError("Cannot pass Int32& by reference (at System.Int32&)");
            AssertHasError("TargetRpcCantHaveParamRef has invalid parameter monkeys (at System.Void WeaverNetworkBehaviourTests.NetworkBehaviourTargetRpcParamRef.NetworkBehaviourTargetRpcParamRef::TargetRpcCantHaveParamRef(Mirror.NetworkConnection,System.Int32&))");
            AssertHasError("Cannot pass type Int32& by reference (at System.Int32&)");
            AssertHasError("TargetRpcCantHaveParamRef has invalid parameter monkeys.  Unsupported type System.Int32&,  use a supported Mirror type instead (at System.Void WeaverNetworkBehaviourTests.NetworkBehaviourTargetRpcParamRef.NetworkBehaviourTargetRpcParamRef::TargetRpcCantHaveParamRef(Mirror.NetworkConnection,System.Int32&))");
        }

        [BatchedTest(false)]
        public void NetworkBehaviourTargetRpcParamAbstract()
        {
            AssertHasError("AbstractClass can't be deserialized because it has no default constructor (at WeaverNetworkBehaviourTests.NetworkBehaviourTargetRpcParamAbstract.NetworkBehaviourTargetRpcParamAbstract/AbstractClass)");
        }

        [BatchedTest(false)]
        public void NetworkBehaviourTargetRpcParamComponent()
        {
            AssertHasError("Cannot generate writer for component type ComponentClass. Use a supported type or provide a custom writer (at WeaverNetworkBehaviourTests.NetworkBehaviourTargetRpcParamComponent.NetworkBehaviourTargetRpcParamComponent/ComponentClass)");
            AssertHasError("TargetRpcCantHaveParamComponent has invalid parameter monkeyComp (at System.Void WeaverNetworkBehaviourTests.NetworkBehaviourTargetRpcParamComponent.NetworkBehaviourTargetRpcParamComponent::TargetRpcCantHaveParamComponent(Mirror.NetworkConnection,WeaverNetworkBehaviourTests.NetworkBehaviourTargetRpcParamComponent.NetworkBehaviourTargetRpcParamComponent/ComponentClass))");
            AssertHasError("Cannot generate reader for component type ComponentClass. Use a supported type or provide a custom reader (at WeaverNetworkBehaviourTests.NetworkBehaviourTargetRpcParamComponent.NetworkBehaviourTargetRpcParamComponent/ComponentClass)");
            AssertHasError("TargetRpcCantHaveParamComponent has invalid parameter monkeyComp.  Unsupported type WeaverNetworkBehaviourTests.NetworkBehaviourTargetRpcParamComponent.NetworkBehaviourTargetRpcParamComponent/ComponentClass,  use a supported Mirror type instead (at System.Void WeaverNetworkBehaviourTests.NetworkBehaviourTargetRpcParamComponent.NetworkBehaviourTargetRpcParamComponent::TargetRpcCantHaveParamComponent(Mirror.NetworkConnection,WeaverNetworkBehaviourTests.NetworkBehaviourTargetRpcParamComponent.NetworkBehaviourTargetRpcParamComponent/ComponentClass))");
        }

        [BatchedTest(true)]
        public void NetworkBehaviourTargetRpcParamNetworkConnection()
        {
            Assert.Pass();
        }

        [BatchedTest(false)]
        public void NetworkBehaviourTargetRpcDuplicateName()
        {
            AssertHasError("Duplicate Target Rpc name TargetRpcCantHaveSameName (at System.Void WeaverNetworkBehaviourTests.NetworkBehaviourTargetRpcDuplicateName.NetworkBehaviourTargetRpcDuplicateName::TargetRpcCantHaveSameName(Mirror.NetworkConnection,System.Int32,System.Int32))");
        }

        [BatchedTest(false)]
        public void NetworkBehaviourClientRpcGenericParam()
        {
            AssertHasError("RpcCantHaveGeneric cannot have generic parameters (at System.Void WeaverNetworkBehaviourTests.NetworkBehaviourClientRpcGenericParam.NetworkBehaviourClientRpcGenericParam::RpcCantHaveGeneric())");
        }

        [BatchedTest(false)]
        public void NetworkBehaviourClientRpcCoroutine()
        {
            AssertHasError("RpcCantHaveCoroutine cannot be a coroutine (at System.Collections.IEnumerator WeaverNetworkBehaviourTests.NetworkBehaviourClientRpcCoroutine.NetworkBehaviourClientRpcCoroutine::RpcCantHaveCoroutine())");
        }

        [BatchedTest(false)]
        public void NetworkBehaviourClientRpcVoidReturn()
        {
            AssertHasError("RpcCantHaveNonVoidReturn cannot return a value.  Make it void instead (at System.Int32 WeaverNetworkBehaviourTests.NetworkBehaviourClientRpcVoidReturn.NetworkBehaviourClientRpcVoidReturn::RpcCantHaveNonVoidReturn())");
        }

        [BatchedTest(false)]
        public void NetworkBehaviourClientRpcParamOut()
        {
            AssertHasError("RpcCantHaveParamOut cannot have out parameters (at System.Void WeaverNetworkBehaviourTests.NetworkBehaviourClientRpcParamOut.NetworkBehaviourClientRpcParamOut::RpcCantHaveParamOut(System.Int32&))");
        }

        [BatchedTest(false)]
        public void NetworkBehaviourClientRpcParamOptional()
        {
            AssertHasError("RpcCantHaveParamOptional cannot have optional parameters (at System.Void WeaverNetworkBehaviourTests.NetworkBehaviourClientRpcParamOptional.NetworkBehaviourClientRpcParamOptional::RpcCantHaveParamOptional(System.Int32))");
        }

        [BatchedTest(false)]
        public void NetworkBehaviourClientRpcParamRef()
        {
            AssertHasError("Cannot pass Int32& by reference (at System.Int32&)");
            AssertHasError("RpcCantHaveParamRef has invalid parameter monkeys (at System.Void WeaverNetworkBehaviourTests.NetworkBehaviourClientRpcParamRef.NetworkBehaviourClientRpcParamRef::RpcCantHaveParamRef(System.Int32&))");
            AssertHasError("Cannot pass type Int32& by reference (at System.Int32&)");
            AssertHasError("RpcCantHaveParamRef has invalid parameter monkeys.  Unsupported type System.Int32&,  use a supported Mirror type instead (at System.Void WeaverNetworkBehaviourTests.NetworkBehaviourClientRpcParamRef.NetworkBehaviourClientRpcParamRef::RpcCantHaveParamRef(System.Int32&))");
            ;
        }

        [BatchedTest(false)]
        public void NetworkBehaviourClientRpcParamAbstract()
        {
            AssertHasError("AbstractClass can't be deserialized because it has no default constructor (at WeaverNetworkBehaviourTests.NetworkBehaviourClientRpcParamAbstract.NetworkBehaviourClientRpcParamAbstract/AbstractClass)");
        }

        [BatchedTest(false)]
        public void NetworkBehaviourClientRpcParamComponent()
        {
            AssertHasError("Cannot generate writer for component type ComponentClass. Use a supported type or provide a custom writer (at WeaverNetworkBehaviourTests.NetworkBehaviourClientRpcParamComponent.NetworkBehaviourClientRpcParamComponent/ComponentClass)");
            AssertHasError("RpcCantHaveParamComponent has invalid parameter monkeyComp (at System.Void WeaverNetworkBehaviourTests.NetworkBehaviourClientRpcParamComponent.NetworkBehaviourClientRpcParamComponent::RpcCantHaveParamComponent(WeaverNetworkBehaviourTests.NetworkBehaviourClientRpcParamComponent.NetworkBehaviourClientRpcParamComponent/ComponentClass))");
            AssertHasError("Cannot generate reader for component type ComponentClass. Use a supported type or provide a custom reader (at WeaverNetworkBehaviourTests.NetworkBehaviourClientRpcParamComponent.NetworkBehaviourClientRpcParamComponent/ComponentClass)");
            AssertHasError("RpcCantHaveParamComponent has invalid parameter monkeyComp.  Unsupported type WeaverNetworkBehaviourTests.NetworkBehaviourClientRpcParamComponent.NetworkBehaviourClientRpcParamComponent/ComponentClass,  use a supported Mirror type instead (at System.Void WeaverNetworkBehaviourTests.NetworkBehaviourClientRpcParamComponent.NetworkBehaviourClientRpcParamComponent::RpcCantHaveParamComponent(WeaverNetworkBehaviourTests.NetworkBehaviourClientRpcParamComponent.NetworkBehaviourClientRpcParamComponent/ComponentClass))");
        }

        [BatchedTest(false)]
        public void NetworkBehaviourClientRpcParamNetworkConnection()
        {
            AssertHasError("RpcCantHaveParamOptional has invalid parameter monkeyCon. Cannot pass NetworkConnections (at System.Void WeaverNetworkBehaviourTests.NetworkBehaviourClientRpcParamNetworkConnection.NetworkBehaviourClientRpcParamNetworkConnection::RpcCantHaveParamOptional(Mirror.NetworkConnection))");
        }

        [BatchedTest(false)]
        public void NetworkBehaviourClientRpcDuplicateName()
        {
            AssertHasError("Duplicate ClientRpc name RpcCantHaveSameName (at System.Void WeaverNetworkBehaviourTests.NetworkBehaviourClientRpcDuplicateName.NetworkBehaviourClientRpcDuplicateName::RpcCantHaveSameName(System.Int32,System.Int32))");
        }

        [BatchedTest(false)]
        public void NetworkBehaviourCmdParamOut()
        {
            AssertHasError("CmdCantHaveParamOut cannot have out parameters (at System.Void WeaverNetworkBehaviourTests.NetworkBehaviourCmdParamOut.NetworkBehaviourCmdParamOut::CmdCantHaveParamOut(System.Int32&))");
        }

        [BatchedTest(false)]
        public void NetworkBehaviourCmdParamOptional()
        {
            AssertHasError("CmdCantHaveParamOptional cannot have optional parameters (at System.Void WeaverNetworkBehaviourTests.NetworkBehaviourCmdParamOptional.NetworkBehaviourCmdParamOptional::CmdCantHaveParamOptional(System.Int32))");
        }

        [BatchedTest(false)]
        public void NetworkBehaviourCmdParamRef()
        {
            AssertHasError("Cannot pass Int32& by reference (at System.Int32&)");
            AssertHasError("CmdCantHaveParamRef has invalid parameter monkeys (at System.Void WeaverNetworkBehaviourTests.NetworkBehaviourCmdParamRef.NetworkBehaviourCmdParamRef::CmdCantHaveParamRef(System.Int32&))");
            AssertHasError("Cannot pass type Int32& by reference (at System.Int32&)");
            AssertHasError("CmdCantHaveParamRef has invalid parameter monkeys.  Unsupported type System.Int32&,  use a supported Mirror type instead (at System.Void WeaverNetworkBehaviourTests.NetworkBehaviourCmdParamRef.NetworkBehaviourCmdParamRef::CmdCantHaveParamRef(System.Int32&))");
        }

        [BatchedTest(false)]
        public void NetworkBehaviourCmdParamAbstract()
        {
            AssertHasError("AbstractClass can't be deserialized because it has no default constructor (at WeaverNetworkBehaviourTests.NetworkBehaviourCmdParamAbstract.NetworkBehaviourCmdParamAbstract/AbstractClass)");
        }

        [BatchedTest(false)]
        public void NetworkBehaviourCmdParamComponent()
        {
            AssertHasError("Cannot generate writer for component type ComponentClass. Use a supported type or provide a custom writer (at WeaverNetworkBehaviourTests.NetworkBehaviourCmdParamComponent.NetworkBehaviourCmdParamComponent/ComponentClass)");
            AssertHasError("CmdCantHaveParamComponent has invalid parameter monkeyComp (at System.Void WeaverNetworkBehaviourTests.NetworkBehaviourCmdParamComponent.NetworkBehaviourCmdParamComponent::CmdCantHaveParamComponent(WeaverNetworkBehaviourTests.NetworkBehaviourCmdParamComponent.NetworkBehaviourCmdParamComponent/ComponentClass))");
            AssertHasError("Cannot generate reader for component type ComponentClass. Use a supported type or provide a custom reader (at WeaverNetworkBehaviourTests.NetworkBehaviourCmdParamComponent.NetworkBehaviourCmdParamComponent/ComponentClass)");
            AssertHasError("CmdCantHaveParamComponent has invalid parameter monkeyComp.  Unsupported type WeaverNetworkBehaviourTests.NetworkBehaviourCmdParamComponent.NetworkBehaviourCmdParamComponent/ComponentClass,  use a supported Mirror type instead (at System.Void WeaverNetworkBehaviourTests.NetworkBehaviourCmdParamComponent.NetworkBehaviourCmdParamComponent::CmdCantHaveParamComponent(WeaverNetworkBehaviourTests.NetworkBehaviourCmdParamComponent.NetworkBehaviourCmdParamComponent/ComponentClass))");
        }

        [BatchedTest(false)]
        public void NetworkBehaviourCmdParamNetworkConnection()
        {
            AssertHasError("CmdCantHaveParamOptional has invalid parameter monkeyCon, Cannot pass NetworkConnections. Instead use 'NetworkConnectionToClient conn = null' to get the sender's connection on the server (at System.Void WeaverNetworkBehaviourTests.NetworkBehaviourCmdParamNetworkConnection.NetworkBehaviourCmdParamNetworkConnection::CmdCantHaveParamOptional(Mirror.NetworkConnection))");
        }

        [BatchedTest(false)]
        public void NetworkBehaviourCmdDuplicateName()
        {
            AssertHasError("Duplicate Command name CmdCantHaveSameName (at System.Void WeaverNetworkBehaviourTests.NetworkBehaviourCmdDuplicateName.NetworkBehaviourCmdDuplicateName::CmdCantHaveSameName(System.Int32,System.Int32))");
        }
    }
}
