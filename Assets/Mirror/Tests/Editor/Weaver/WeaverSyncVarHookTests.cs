using NUnit.Framework;

namespace Mirror.Weaver.Tests
{
    public class WeaverSyncVarHookTests : BatchedWeaverTests
    {
        [BatchedTest(true)]
        public void FindsPrivateHook()
        {
            Assert.Pass();
        }

        [BatchedTest(true)]
        public void FindsPublicHook()
        {
            Assert.Pass();
        }

        [BatchedTest(true)]
        public void FindsStaticHook()
        {
            Assert.Pass();
        }

        [BatchedTest(true)]
        public void FindsHookWithGameObjects()
        {
            Assert.Pass();
        }

        [BatchedTest(true)]
        public void FindsHookWithNetworkIdentity()
        {
            Assert.Pass();
        }

        [BatchedTest(true)]
        public void FindsHookWithOtherOverloadsInOrder()
        {
            Assert.Pass();
        }

        [BatchedTest(true)]
        public void FindsHookWithOtherOverloadsInReverseOrder()
        {
            Assert.Pass();
        }

        static string OldNewMethodFormat(string hookName, string ValueType)
        {
            return string.Format("void {0}({1} oldValue, {1} newValue)", hookName, ValueType);
        }

        [BatchedTest(false)]
        public void ErrorWhenNoHookFound()
        {
            AssertHasError($"Could not find hook for 'health', hook name 'onChangeHealth'. " +
                $"Method signature should be {OldNewMethodFormat("onChangeHealth", "System.Int32")} " +
                $"(at System.Int32 WeaverSyncVarHookTests.ErrorWhenNoHookFound.ErrorWhenNoHookFound::health)");
        }

        [BatchedTest(false)]
        public void ErrorWhenNoHookWithCorrectParametersFound()
        {
            AssertHasError($"Could not find hook for 'health', hook name 'onChangeHealth'. " +
                $"Method signature should be {OldNewMethodFormat("onChangeHealth", "System.Int32")} " +
                $"(at System.Int32 WeaverSyncVarHookTests.ErrorWhenNoHookWithCorrectParametersFound.ErrorWhenNoHookWithCorrectParametersFound::health)");
        }

        [BatchedTest(false)]
        public void ErrorForWrongTypeOldParameter()
        {
            AssertHasError($"Wrong type for Parameter in hook for 'health', hook name 'onChangeHealth'. " +
                $"Method signature should be {OldNewMethodFormat("onChangeHealth", "System.Int32")} " +
                $"(at System.Int32 WeaverSyncVarHookTests.ErrorForWrongTypeOldParameter.ErrorForWrongTypeOldParameter::health)");
        }

        [BatchedTest(false)]
        public void ErrorForWrongTypeNewParameter()
        {
            AssertHasError($"Wrong type for Parameter in hook for 'health', hook name 'onChangeHealth'. " +
                $"Method signature should be {OldNewMethodFormat("onChangeHealth", "System.Int32")} " +
                $"(at System.Int32 WeaverSyncVarHookTests.ErrorForWrongTypeNewParameter.ErrorForWrongTypeNewParameter::health)");
        }
    }
}
