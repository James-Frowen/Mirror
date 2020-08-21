using NUnit.Framework;

namespace Mirror.Weaver.Tests
{
    public class WeaverExtensionsTests
    {
        [BatchedTest(true)]
        public void StripGenericParametersFromClassName()
        {
            Assert.That(Extensions.StripGenericParametersFromClassName("Type"), Is.EqualTo("Type"));
            Assert.That(Extensions.StripGenericParametersFromClassName("Type.Subtype"), Is.EqualTo("Type.Subtype"));
            Assert.That(Extensions.StripGenericParametersFromClassName("Type.Subtype<T>"), Is.EqualTo("Type.Subtype"));
            Assert.That(Extensions.StripGenericParametersFromClassName("Type.Subtype<T,U>"), Is.EqualTo("Type.Subtype"));
        }
    }
}
