using System.IO;
using System.Linq;
using Mono.CecilX;
using Mono.CecilX.Cil;
using NUnit.Framework;

namespace Mirror.Weaver.Tests
{
    public class WeaverClientServerAttributeTests : BatchedWeaverTests
    {
        [BatchedTest(true)]
        public void NetworkBehaviourServer()
        {
            Assert.Pass();

            string networkServerGetActive = WeaverTypes.NetworkServerGetActive.ToString();
            CheckAddedCode(networkServerGetActive, "WeaverClientServerAttributeTests.NetworkBehaviourServer.NetworkBehaviourServer", "ServerOnlyMethod");
        }

        [BatchedTest(true)]
        public void ServerAttributeOnVirutalMethod()
        {
            Assert.Pass();

            string networkServerGetActive = WeaverTypes.NetworkServerGetActive.ToString();
            CheckAddedCode(networkServerGetActive, "WeaverClientServerAttributeTests.ServerAttributeOnVirutalMethod.ServerAttributeOnVirutalMethod", "ServerOnlyMethod");
        }

        [BatchedTest(false)]
        public void ServerAttributeOnAbstractMethod()
        {
            AssertHasError("Server or Client Attributes can't be added to abstract method. Server and Client Attributes are not inherited so they need to be applied to the override methods instead. (at System.Void WeaverClientServerAttributeTests.ServerAttributeOnAbstractMethod.ServerAttributeOnAbstractMethod::ServerOnlyMethod())");
        }

        [BatchedTest(true)]
        public void ServerAttributeOnOverrideMethod()
        {
            Assert.Pass();

            string networkServerGetActive = WeaverTypes.NetworkServerGetActive.ToString();
            CheckAddedCode(networkServerGetActive, "WeaverClientServerAttributeTests.ServerAttributeOnOverrideMethod.ServerAttributeOnOverrideMethod", "ServerOnlyMethod");
        }

        [BatchedTest(true)]
        public void NetworkBehaviourClient()
        {
            Assert.Pass();

            string networkClientGetActive = WeaverTypes.NetworkClientGetActive.ToString();
            CheckAddedCode(networkClientGetActive, "WeaverClientServerAttributeTests.NetworkBehaviourClient.NetworkBehaviourClient", "ClientOnlyMethod");
        }

        [BatchedTest(true)]
        public void ClientAttributeOnVirutalMethod()
        {
            Assert.Pass();

            string networkClientGetActive = WeaverTypes.NetworkClientGetActive.ToString();
            CheckAddedCode(networkClientGetActive, "WeaverClientServerAttributeTests.ClientAttributeOnVirutalMethod.ClientAttributeOnVirutalMethod", "ClientOnlyMethod");
        }

        [BatchedTest(false)]
        public void ClientAttributeOnAbstractMethod()
        {
            AssertHasError("Server or Client Attributes can't be added to abstract method. Server and Client Attributes are not inherited so they need to be applied to the override methods instead. (at System.Void WeaverClientServerAttributeTests.ClientAttributeOnAbstractMethod.ClientAttributeOnAbstractMethod::ClientOnlyMethod())");
        }

        [BatchedTest(true)]
        public void ClientAttributeOnOverrideMethod()
        {
            Assert.Pass();

            string networkClientGetActive = WeaverTypes.NetworkClientGetActive.ToString();
            CheckAddedCode(networkClientGetActive, "WeaverClientServerAttributeTests.ClientAttributeOnOverrideMethod.ClientAttributeOnOverrideMethod", "ClientOnlyMethod");
        }

        [BatchedTest(true)]
        public void StaticClassClient()
        {
            Assert.Pass();

            string networkClientGetActive = WeaverTypes.NetworkClientGetActive.ToString();
            CheckAddedCode(networkClientGetActive, "WeaverClientServerAttributeTests.StaticClassClient.StaticClassClient", "ClientOnlyMethod");
        }
        [BatchedTest(true)]
        public void RegularClassClient()
        {
            Assert.Pass();

            string networkClientGetActive = WeaverTypes.NetworkClientGetActive.ToString();
            CheckAddedCode(networkClientGetActive, "WeaverClientServerAttributeTests.RegularClassClient.RegularClassClient", "ClientOnlyMethod");
        }
        [BatchedTest(true)]
        public void MonoBehaviourClient()
        {
            Assert.Pass();

            string networkClientGetActive = WeaverTypes.NetworkClientGetActive.ToString();
            CheckAddedCode(networkClientGetActive, "WeaverClientServerAttributeTests.MonoBehaviourClient.MonoBehaviourClient", "ClientOnlyMethod");
        }

        [BatchedTest(true)]
        public void StaticClassServer()
        {
            Assert.Pass();

            string networkServerGetActive = WeaverTypes.NetworkServerGetActive.ToString();
            CheckAddedCode(networkServerGetActive, "WeaverClientServerAttributeTests.StaticClassServer.StaticClassServer", "ServerOnlyMethod");
        }
        [BatchedTest(true)]
        public void RegularClassServer()
        {
            Assert.Pass();

            string networkServerGetActive = WeaverTypes.NetworkServerGetActive.ToString();
            CheckAddedCode(networkServerGetActive, "WeaverClientServerAttributeTests.RegularClassServer.RegularClassServer", "ServerOnlyMethod");
        }
        [BatchedTest(true)]
        public void MonoBehaviourServer()
        {
            Assert.Pass();

            string networkServerGetActive = WeaverTypes.NetworkServerGetActive.ToString();
            CheckAddedCode(networkServerGetActive, "WeaverClientServerAttributeTests.MonoBehaviourServer.MonoBehaviourServer", "ServerOnlyMethod");
        }




        /// <summary>
        /// Checks that first Instructions in MethodBody is addedString
        /// </summary>
        /// <param name="addedString"></param>
        /// <param name="methodName"></param>
        static void CheckAddedCode(string addedString, string className, string methodName)
        {
            string assemblyName = Path.Combine(WeaverAssembler.OutputDirectory, WeaverAssembler.OutputFile);
            using (AssemblyDefinition assembly = AssemblyDefinition.ReadAssembly(assemblyName))
            {
                TypeDefinition type = assembly.MainModule.GetType(className);
                MethodDefinition method = type.Methods.First(m => m.Name == methodName);
                MethodBody body = method.Body;

                Instruction top = body.Instructions[0];

                Assert.AreEqual(top.OpCode, OpCodes.Call);
                Assert.AreEqual(top.Operand.ToString(), addedString);
            }
        }
    }
}
