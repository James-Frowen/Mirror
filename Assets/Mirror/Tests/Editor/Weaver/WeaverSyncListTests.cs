using NUnit.Framework;

namespace Mirror.Weaver.Tests
{
    public class WeaverSyncListTests : BatchedWeaverTests
    {
        [BatchedTest(true)]
        public void SyncList()
        {
            Assert.Pass();
        }

        [BatchedTest(true)]
        public void SyncListByteValid()
        {
            Assert.Pass();
        }

        [BatchedTest(true)]
        public void SyncListGenericAbstractInheritance()
        {
            Assert.Pass();
        }

        [BatchedTest(true)]
        public void SyncListGenericInheritance()
        {
            Assert.Pass();
        }

        [BatchedTest(false)]
        public void SyncListGenericInheritanceWithMultipleGeneric()
        {
            AssertHasError("Could not find generic arguments for SyncList`1 in WeaverSyncListTests.SyncListGenericInheritanceWithMultipleGeneric.SyncListGenericInheritanceWithMultipleGeneric/SomeListInt (at WeaverSyncListTests.SyncListGenericInheritanceWithMultipleGeneric.SyncListGenericInheritanceWithMultipleGeneric/SomeListInt)");
        }

        [BatchedTest(true)]
        public void SyncListInheritance()
        {
            Assert.Pass();
        }

        [BatchedTest(false)]
        public void SyncListMissingParamlessCtor()
        {
            AssertHasError("Can not initialize field Foo because no default constructor was found. Manually initialize the field (call the constructor) or add constructor without Parameter (at WeaverSyncListTests.SyncListMissingParamlessCtor.SyncListMissingParamlessCtor/SyncListString2 WeaverSyncListTests.SyncListMissingParamlessCtor.SyncListMissingParamlessCtor::Foo)");
        }

        [BatchedTest(true)]
        public void SyncListMissingParamlessCtorManuallyInitialized()
        {
            Assert.Pass();
        }

        [BatchedTest(true)]
        public void SyncListNestedStruct()
        {
            Assert.Pass();
        }

        [BatchedTest(true)]
        public void SyncListNestedInAbstractClass()
        {
            Assert.Pass();
        }

        [BatchedTest(true)]
        public void SyncListNestedInAbstractClassWithInvalid()
        {
            // we need this negative test to make sure that SyncList is being processed 
            AssertHasError("Cannot generate writer for Object. Use a supported type or provide a custom writer (at UnityEngine.Object)");
            AssertHasError("target has unsupported type. Use a type supported by Mirror instead (at UnityEngine.Object WeaverSyncListTests.SyncListNestedInAbstractClassWithInvalid.SyncListNestedStructWithInvalid/SomeAbstractClass/MyNestedStruct::target)");
            AssertHasError("MyNestedStructList has sync object generic type MyNestedStruct.  Use a type supported by mirror instead (at WeaverSyncListTests.SyncListNestedInAbstractClassWithInvalid.SyncListNestedStructWithInvalid/SomeAbstractClass/MyNestedStructList)");
        }

        [BatchedTest(true)]
        public void SyncListNestedInStruct()
        {
            Assert.Pass();
        }

        [BatchedTest(true)]
        public void SyncListNestedInStructWithInvalid()
        {
            // we need this negative test to make sure that SyncList is being processed 
            AssertHasError("Cannot generate writer for Object. Use a supported type or provide a custom writer (at UnityEngine.Object)");
            AssertHasError("target has unsupported type. Use a type supported by Mirror instead (at UnityEngine.Object WeaverSyncListTests.SyncListNestedInStructWithInvalid.SyncListNestedInStructWithInvalid/SomeData::target)");
            AssertHasError("SyncList has sync object generic type SomeData.  Use a type supported by mirror instead (at WeaverSyncListTests.SyncListNestedInStructWithInvalid.SyncListNestedInStructWithInvalid/SomeData/SyncList)");
        }

        [BatchedTest(true)]
        public void SyncListStruct()
        {
            Assert.Pass();
        }

        [BatchedTest(true)]
        public void SyncListStructWithCustomDeserializeOnly()
        {
            Assert.Pass();
        }

        [BatchedTest(true)]
        public void SyncListStructWithCustomMethods()
        {
            Assert.Pass();
        }

        [BatchedTest(true)]
        public void SyncListStructWithCustomSerializeOnly()
        {
            Assert.Pass();
        }

        [BatchedTest(false)]
        public void SyncListErrorForGenericStruct()
        {
            AssertHasError("Can not create Serialize or Deserialize for generic element in MyGenericStructList. Override virtual methods with custom Serialize and Deserialize to use WeaverSyncListTests.SyncListErrorForGenericStruct.SyncListErrorForGenericStruct/MyGenericStruct`1<System.Single> in SyncList (at WeaverSyncListTests.SyncListErrorForGenericStruct.SyncListErrorForGenericStruct/MyGenericStructList)");
        }

        [BatchedTest(false)]
        public void SyncListErrorForGenericStructWithCustomDeserializeOnly()
        {
            AssertHasError("Can not create Serialize or Deserialize for generic element in MyGenericStructList. Override virtual methods with custom Serialize and Deserialize to use WeaverSyncListTests.SyncListErrorForGenericStructWithCustomDeserializeOnly.SyncListErrorForGenericStructWithCustomDeserializeOnly/MyGenericStruct`1<System.Single> in SyncList (at WeaverSyncListTests.SyncListErrorForGenericStructWithCustomDeserializeOnly.SyncListErrorForGenericStructWithCustomDeserializeOnly/MyGenericStructList)");
        }

        [BatchedTest(false)]
        public void SyncListErrorForGenericStructWithCustomSerializeOnly()
        {
            AssertHasError("Can not create Serialize or Deserialize for generic element in MyGenericStructList. Override virtual methods with custom Serialize and Deserialize to use MyGenericStruct`1 in SyncList (at WeaverSyncListTests.SyncListErrorForGenericStructWithCustomSerializeOnly.SyncListErrorForGenericStructWithCustomSerializeOnly/MyGenericStructList)");
        }

        [BatchedTest(true)]
        public void SyncListGenericStructWithCustomMethods()
        {
            Assert.Pass();
        }

        [BatchedTest(false)]
        public void SyncListErrorForInterface()
        {
            AssertHasError("Cannot generate writer for interface MyInterface. Use a supported type or provide a custom writer (at WeaverSyncListTests.SyncListErrorForInterface.MyInterface)");
            AssertHasError("MyInterfaceList has sync object generic type MyInterface.  Use a type supported by mirror instead (at WeaverSyncListTests.SyncListErrorForInterface.MyInterfaceList)");
        }

        [BatchedTest(true)]
        public void SyncListInterfaceWithCustomMethods()
        {
            Assert.Pass();
        }

        [BatchedTest(true)]
        public void SyncListInheritanceWithOverrides()
        {
            Assert.Pass();
        }

        [BatchedTest(false)]
        public void SyncListErrorWhenUsingGenericListInNetworkBehaviour()
        {
            AssertHasError("Cannot use generic SyncObject someList directly in NetworkBehaviour. Create a class and inherit from the generic SyncObject instead (at WeaverSyncListTests.SyncListErrorWhenUsingGenericListInNetworkBehaviour.SyncListErrorWhenUsingGenericListInNetworkBehaviour/SomeList`1<System.Int32> WeaverSyncListTests.SyncListErrorWhenUsingGenericListInNetworkBehaviour.SyncListErrorWhenUsingGenericListInNetworkBehaviour::someList)");
        }
    }
}
