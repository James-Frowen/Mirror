using NUnit.Framework;

namespace Mirror.Weaver.Tests
{
    // Some tests for SyncObjects are in WeaverSyncListTests and apply to SyncDictionary too
    public class WeaverSyncDictionaryTests : BatchedWeaverTests
    {
        [BatchedTest(true)]
        public void SyncDictionary()
        {
            Assert.Pass();
        }

        [BatchedTest(true)]
        public void SyncDictionaryGenericAbstractInheritance()
        {
            Assert.Pass();
        }

        [BatchedTest(true)]
        public void SyncDictionaryGenericInheritance()
        {
            Assert.Pass();
        }

        [BatchedTest(true)]
        public void SyncDictionaryInheritance()
        {
            Assert.Pass();
        }

        [BatchedTest(true)]
        public void SyncDictionaryStructKey()
        {
            Assert.Pass();
        }

        [BatchedTest(true)]
        public void SyncDictionaryStructItem()
        {
            Assert.Pass();
        }

        [BatchedTest(true)]
        public void SyncDictionaryStructKeyWithCustomDeserializeOnly()
        {
            Assert.Pass();
        }

        [BatchedTest(true)]
        public void SyncDictionaryStructItemWithCustomDeserializeOnly()
        {
            Assert.Pass();
        }

        [BatchedTest(true)]
        public void SyncDictionaryStructKeyWithCustomMethods()
        {
            Assert.Pass();
        }

        [BatchedTest(true)]
        public void SyncDictionaryStructItemWithCustomMethods()
        {
            Assert.Pass();
        }

        [BatchedTest(true)]
        public void SyncDictionaryStructKeyWithCustomSerializeOnly()
        {
            Assert.Pass();
        }


        [BatchedTest(true)]
        public void SyncDictionaryStructItemWithCustomSerializeOnly()
        {
            Assert.Pass();
        }

        [BatchedTest(false)]
        public void SyncDictionaryErrorForGenericStructKey()
        {
            AssertHasError("Can not create Serialize or Deserialize for generic element in MyGenericStructDictionary. Override virtual methods with custom Serialize and Deserialize to use WeaverSyncDictionaryTests.SyncDictionaryErrorForGenericStructKey.SyncDictionaryErrorForGenericStructKey/MyGenericStruct`1<System.Single> in SyncList (at WeaverSyncDictionaryTests.SyncDictionaryErrorForGenericStructKey.SyncDictionaryErrorForGenericStructKey/MyGenericStructDictionary)");
        }

        [BatchedTest(false)]
        public void SyncDictionaryErrorForGenericStructItem()
        {
            AssertHasError("Can not create Serialize or Deserialize for generic element in MyGenericStructDictionary. Override virtual methods with custom Serialize and Deserialize to use WeaverSyncDictionaryTests.SyncDictionaryErrorForGenericStructItem.SyncDictionaryErrorForGenericStructItem/MyGenericStruct`1<System.Single> in SyncList (at WeaverSyncDictionaryTests.SyncDictionaryErrorForGenericStructItem.SyncDictionaryErrorForGenericStructItem/MyGenericStructDictionary)");
        }

        [BatchedTest(false)]
        public void SyncDictionaryErrorForGenericStructKeyWithCustomDeserializeOnly()
        {
            AssertHasError("Can not create Serialize or Deserialize for generic element in MyGenericStructDictionary. Override virtual methods with custom Serialize and Deserialize to use WeaverSyncDictionaryTests.SyncDictionaryErrorForGenericStructKeyWithCustomDeserializeOnly.SyncDictionaryErrorForGenericStructKeyWithCustomDeserializeOnly/MyGenericStruct`1<System.Single> in SyncList (at WeaverSyncDictionaryTests.SyncDictionaryErrorForGenericStructKeyWithCustomDeserializeOnly.SyncDictionaryErrorForGenericStructKeyWithCustomDeserializeOnly/MyGenericStructDictionary)");
        }

        [BatchedTest(false)]
        public void SyncDictionaryErrorForGenericStructItemWithCustomDeserializeOnly()
        {
            AssertHasError("Can not create Serialize or Deserialize for generic element in MyGenericStructDictionary. Override virtual methods with custom Serialize and Deserialize to use WeaverSyncDictionaryTests.SyncDictionaryErrorForGenericStructItemWithCustomDeserializeOnly.SyncDictionaryErrorForGenericStructItemWithCustomDeserializeOnly/MyGenericStruct`1<System.Single> in SyncList (at WeaverSyncDictionaryTests.SyncDictionaryErrorForGenericStructItemWithCustomDeserializeOnly.SyncDictionaryErrorForGenericStructItemWithCustomDeserializeOnly/MyGenericStructDictionary)");
        }

        [BatchedTest(false)]
        public void SyncDictionaryErrorForGenericStructKeyWithCustomSerializeOnly()
        {
            AssertHasError("Can not create Serialize or Deserialize for generic element in MyGenericStructDictionary. Override virtual methods with custom Serialize and Deserialize to use MyGenericStruct`1 in SyncList (at WeaverSyncDictionaryTests.SyncDictionaryErrorForGenericStructKeyWithCustomSerializeOnly.SyncDictionaryErrorForGenericStructKeyWithCustomSerializeOnly/MyGenericStructDictionary)");
        }

        [BatchedTest(false)]
        public void SyncDictionaryErrorForGenericStructItemWithCustomSerializeOnly()
        {
            AssertHasError("Can not create Serialize or Deserialize for generic element in MyGenericStructDictionary. Override virtual methods with custom Serialize and Deserialize to use MyGenericStruct`1 in SyncList (at WeaverSyncDictionaryTests.SyncDictionaryErrorForGenericStructItemWithCustomSerializeOnly.SyncDictionaryErrorForGenericStructItemWithCustomSerializeOnly/MyGenericStructDictionary)");
        }

        [BatchedTest(true)]
        public void SyncDictionaryGenericStructKeyWithCustomMethods()
        {
            Assert.Pass();
        }

        [BatchedTest(true)]
        public void SyncDictionaryGenericStructItemWithCustomMethods()
        {
            Assert.Pass();
        }

        [BatchedTest(false)]
        public void SyncDictionaryErrorWhenUsingGenericInNetworkBehaviour()
        {
            AssertHasError("Cannot use generic SyncObject someDictionary directly in NetworkBehaviour. Create a class and inherit from the generic SyncObject instead (at WeaverSyncDictionaryTests.SyncDictionaryErrorWhenUsingGenericInNetworkBehaviour.SyncDictionaryErrorWhenUsingGenericInNetworkBehaviour/SomeSyncDictionary`2<System.Int32,System.String> WeaverSyncDictionaryTests.SyncDictionaryErrorWhenUsingGenericInNetworkBehaviour.SyncDictionaryErrorWhenUsingGenericInNetworkBehaviour::someDictionary)");
        }
    }
}
