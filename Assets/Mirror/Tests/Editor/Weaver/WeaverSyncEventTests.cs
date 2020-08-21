using NUnit.Framework;

namespace Mirror.Weaver.Tests
{
    public class WeaverSyncEventTests : BatchedWeaverTests
    {
        [BatchedTest(true)]
        public void SyncEventValid()
        {
            Assert.Pass();
        }

        [BatchedTest(true)]
        public void MultipleSyncEvent()
        {
            Assert.Pass();
        }

        [BatchedTest(false)]
        public void ErrorWhenSyncEventUsesGenericParameter()
        {
            AssertHasError("EventDoCoolThingsWithExcitingPeople must not have generic parameters.  " +
                "Consider creating a new class that inherits from WeaverSyncEventTests.ErrorWhenSyncEventUsesGenericParameter.ErrorWhenSyncEventUsesGenericParameter/MySyncEventDelegate`1<System.Int32> instead " +
                "(at WeaverSyncEventTests.ErrorWhenSyncEventUsesGenericParameter.ErrorWhenSyncEventUsesGenericParameter/MySyncEventDelegate`1<System.Int32> WeaverSyncEventTests.ErrorWhenSyncEventUsesGenericParameter.ErrorWhenSyncEventUsesGenericParameter::EventDoCoolThingsWithExcitingPeople)");
        }
    }
}
