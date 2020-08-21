using NUnit.Framework;

namespace Mirror.Weaver.Tests
{
    public class WeaverSyncVarTests : BatchedWeaverTests
    {
        [BatchedTest(true)]
        public void SyncVarsValid()
        {
            Assert.Pass();
        }

        [BatchedTest(false)]
        public void SyncVarsDerivedNetworkBehaviour()
        {
            AssertHasError("Cannot generate writer for component type MySyncVar. Use a supported type or provide a custom writer (at WeaverSyncVarTests.SyncVarsDerivedNetworkBehaviour.SyncVarsDerivedNetworkBehaviour/MySyncVar)");
            AssertHasError("invalidVar has unsupported type. Use a supported Mirror type instead (at WeaverSyncVarTests.SyncVarsDerivedNetworkBehaviour.SyncVarsDerivedNetworkBehaviour/MySyncVar WeaverSyncVarTests.SyncVarsDerivedNetworkBehaviour.SyncVarsDerivedNetworkBehaviour::invalidVar)");
        }

        [BatchedTest(false)]
        public void SyncVarsStatic()
        {
            AssertHasError("invalidVar cannot be static (at System.Int32 WeaverSyncVarTests.SyncVarsStatic.SyncVarsStatic::invalidVar)");
        }

        [BatchedTest(false)]
        public void SyncVarsGenericParam()
        {
            AssertHasError("Cannot generate writer for generic type MySyncVar`1. Use a supported type or provide a custom writer (at WeaverSyncVarTests.SyncVarsGenericParam.SyncVarsGenericParam/MySyncVar`1<System.Int32>)");
            AssertHasError("invalidVar has unsupported type. Use a supported Mirror type instead (at WeaverSyncVarTests.SyncVarsGenericParam.SyncVarsGenericParam/MySyncVar`1<System.Int32> WeaverSyncVarTests.SyncVarsGenericParam.SyncVarsGenericParam::invalidVar)");
        }

        [BatchedTest(false)]
        public void SyncVarsInterface()
        {
            AssertHasError("Cannot generate writer for interface MySyncVar. Use a supported type or provide a custom writer (at WeaverSyncVarTests.SyncVarsInterface.SyncVarsInterface/MySyncVar)");
            AssertHasError("invalidVar has unsupported type. Use a supported Mirror type instead (at WeaverSyncVarTests.SyncVarsInterface.SyncVarsInterface/MySyncVar WeaverSyncVarTests.SyncVarsInterface.SyncVarsInterface::invalidVar)");
        }

        [BatchedTest(false)]
        public void SyncVarsDifferentModule()
        {
            AssertHasError("Cannot generate writer for component type TextMesh. Use a supported type or provide a custom writer (at UnityEngine.TextMesh)");
            AssertHasError("invalidVar has unsupported type. Use a supported Mirror type instead (at UnityEngine.TextMesh WeaverSyncVarTests.SyncVarsDifferentModule.SyncVarsDifferentModule::invalidVar)");
        }

        [BatchedTest(false)]
        public void SyncVarsCantBeArray()
        {
            AssertHasError("thisShouldntWork has invalid type. Use SyncLists instead of arrays (at System.Int32[] WeaverSyncVarTests.SyncVarsCantBeArray.SyncVarsCantBeArray::thisShouldntWork)");
        }

        //[BatchedTest(true)]
        //public void SyncVarsSyncList()
        //{
        //    Assert.Pass();
        //    Assert.That(weaverWarnings, Contains.Item("syncobj has [SyncVar] attribute. SyncLists should not be marked with SyncVar (at WeaverSyncVarTests.SyncVarsSyncList.SyncVarsSyncList/SyncObjImplementer WeaverSyncVarTests.SyncVarsSyncList.SyncVarsSyncList::syncobj)");
        //    Assert.That(weaverWarnings, Contains.Item("syncints has [SyncVar] attribute. SyncLists should not be marked with SyncVar (at Mirror.SyncListInt WeaverSyncVarTests.SyncVarsSyncList.SyncVarsSyncList::syncints)"));


        //}

        [BatchedTest(false)]
        public void SyncVarsMoreThan63()
        {
            AssertHasError("SyncVarsMoreThan63 has too many SyncVars. Consider refactoring your class into multiple components (at WeaverSyncVarTests.SyncVarsMoreThan63.SyncVarsMoreThan63)");
        }
    }
}
