using NUnit.Framework;

namespace Mirror.Weaver.Tests
{
    public class GenerateQuestion : WeaverTestsBuildFromTestName
    {
        [Test]
        public void TestRecursive()
        {
            Assert.That(CompilationFinishedHook.WeaveFailed, Is.False);
        }

        [Test]
        public void TestExeption()
        {
            Assert.That(CompilationFinishedHook.WeaveFailed, Is.False);
        }

        [Test]
        public void TestWriter()
        {
            Assert.That(CompilationFinishedHook.WeaveFailed, Is.False);
        }
    }
    public class GenerateQuestion2 : WeaverTests
    {
        [SetUp]
        public void Setup()
        {
            WeaverAssembler.AddReferencesByAssemblyName(new string[] { "UnityEngine.AnimationModule.dll" });
            BuildAndWeaveTestAssembly(nameof(GenerateQuestion), TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void TestCLip()
        {
            Assert.That(CompilationFinishedHook.WeaveFailed, Is.False);
        }
    }
}
