using NUnit.Framework;

namespace Mirror.Weaver.Tests
{
    // Some tests for SyncObjects are in WeaverSyncListTests and apply to SyncDictionary too
    public class WeaverSyncSetTests : BatchedWeaverTests
    {
        [BatchedTest(true)]
        public void SyncSet()
        {
            Assert.Pass();
        }

        [BatchedTest(true)]
        public void SyncSetByteValid()
        {
            Assert.Pass();
        }

        [BatchedTest(true)]
        public void SyncSetGenericAbstractInheritance()
        {
            Assert.Pass();
        }

        [BatchedTest(true)]
        public void SyncSetGenericInheritance()
        {
            Assert.Pass();
        }

        [BatchedTest(true)]
        public void SyncSetInheritance()
        {
            Assert.Pass();
        }

        [BatchedTest(true)]
        public void SyncSetStruct()
        {
            Assert.Pass();
        }
    }
}
