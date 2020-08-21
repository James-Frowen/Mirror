using NUnit.Framework;

namespace Mirror.Weaver.Tests
{
    public class WeaverGeneralTests : BatchedWeaverTests
    {
        [BatchedTest(false)]
        public void RecursionCount()
        {
            AssertHasError("Potato1 can't be serialized because it references itself (at WeaverGeneralTests.RecursionCount.RecursionCount/Potato1)");
        }

        [BatchedTest(true)]
        public void TestingScriptableObjectArraySerialization()
        {
            Assert.Pass();
        }
    }
}
