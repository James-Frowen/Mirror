using NUnit.Framework;

namespace Mirror.Weaver.Tests
{
    public class WeaverMessageTests : BatchedWeaverTests
    {
        [BatchedTest(true)]
        public void MessageValid()
        {
            Assert.Pass();
        }

        [BatchedTest(true)]
        public void MessageWithBaseClass()
        {
            Assert.Pass();
        }

        [BatchedTest(false)]
        public void MessageSelfReferencing()
        {
            AssertHasError("MessageSelfReferencing has field selfReference that references itself (at WeaverMessageTests.MessageSelfReferencing.MessageSelfReferencing WeaverMessageTests.MessageSelfReferencing.MessageSelfReferencing::selfReference)");
        }

        [BatchedTest(false)]
        public void MessageMemberGeneric()
        {
            AssertHasError("Cannot generate writer for generic type HasGeneric`1. Use a supported type or provide a custom writer (at WeaverMessageTests.MessageMemberGeneric.HasGeneric`1<System.Int32>)");
            AssertHasError("invalidField has unsupported type (at WeaverMessageTests.MessageMemberGeneric.HasGeneric`1<System.Int32> WeaverMessageTests.MessageMemberGeneric.MessageMemberGeneric::invalidField)");
        }

        [BatchedTest(false)]
        public void MessageMemberInterface()
        {
            AssertHasError("Cannot generate writer for interface SuperCoolInterface. Use a supported type or provide a custom writer (at WeaverMessageTests.MessageMemberInterface.SuperCoolInterface)");
            AssertHasError("invalidField has unsupported type (at WeaverMessageTests.MessageMemberInterface.SuperCoolInterface WeaverMessageTests.MessageMemberInterface.MessageMemberInterface::invalidField)");
        }

        [BatchedTest(true)]
        public void MessageNestedInheritance()
        {
            Assert.Pass();
        }

        [BatchedTest(true)]
        public void AbstractMessageMethods()
        {
            Assert.Pass();
        }
    }
}

