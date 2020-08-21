using NUnit.Framework;

namespace Mirror.Weaver.Tests
{
    public class WeaverGeneratedReaderWriterTests : BatchedWeaverTests
    {
        [OneTimeSetUp]
        public override void FixtureSetup()
        {
            WeaverAssembler.AddReferencesByAssemblyName(new string[] { "WeaverTestExtraAssembly.dll" });

            base.FixtureSetup();
        }

        [BatchedTest(true)]
        public void CreatesForStructs()
        {
            Assert.Pass();
        }

        [BatchedTest(true)]
        public void CreatesForClass()
        {
            Assert.Pass();
        }

        [BatchedTest(true)]
        public void CreatesForClassInherited()
        {
            Assert.Pass();
        }

        [BatchedTest(true)]
        public void CreatesForClassWithValidConstructor()
        {
            Assert.Pass();
        }

        [BatchedTest(false)]
        public void GivesErrorForClassWithNoValidConstructor()
        {
            AssertHasError("SomeOtherData can't be deserialized because it has no default constructor (at GeneratedReaderWriter.GivesErrorForClassWithNoValidConstructor.SomeOtherData)");
        }

        [BatchedTest(true)]
        public void CreatesForInheritedFromScriptableObject()
        {
            Assert.Pass();
        }

        [BatchedTest(true)]
        public void CreatesForStructFromDifferentAssemblies()
        {
            Assert.Pass();
        }

        [BatchedTest(true)]
        public void CreatesForClassFromDifferentAssemblies()
        {
            Assert.Pass();
        }

        [BatchedTest(true)]
        public void CreatesForClassFromDifferentAssembliesWithValidConstructor()
        {
            Assert.Pass();
        }

        [BatchedTest(true)]
        public void CanUseCustomReadWriteForTypesFromDifferentAssemblies()
        {
            Assert.Pass();
        }

        [BatchedTest(false)]
        public void GivesErrorWhenUsingUnityAsset()
        {
            AssertHasError("Material can't be deserialized because it has no default constructor (at UnityEngine.Material)");
        }

        [BatchedTest(true)]
        public void GivesErrorWhenUsingObject()
        {
            // TODO: decide if we want to block sending of Object
            // would only want to be send as an arg as a base type for an Inherited object
            AssertHasError("Cannot generate writer for Object. Use a supported type or provide a custom writer (at UnityEngine.Object)");
            AssertHasError("Cannot generate reader for Object. Use a supported type or provide a custom reader (at UnityEngine.Object)");
        }

        [BatchedTest(true)]
        public void GivesErrorWhenUsingScriptableObject()
        {
            // TODO: decide if we want to block sending of ScripableObject
            // would only want to be send as an arg as a base type for an Inherited object
            AssertHasError("Cannot generate writer for ScriptableObject. Use a supported type or provide a custom writer (at UnityEngine.ScriptableObject)");
            AssertHasError("Cannot generate reader for ScriptableObject. Use a supported type or provide a custom reader (at UnityEngine.ScriptableObject)");
        }

        [BatchedTest(false)]
        public void GivesErrorWhenUsingMonoBehaviour()
        {
            AssertHasError("Cannot generate writer for component type MonoBehaviour. Use a supported type or provide a custom writer (at UnityEngine.MonoBehaviour)");
            AssertHasError("Cannot generate reader for component type MonoBehaviour. Use a supported type or provide a custom reader (at UnityEngine.MonoBehaviour)");
        }

        [BatchedTest(false)]
        public void GivesErrorWhenUsingTypeInheritedFromMonoBehaviour()
        {
            AssertHasError("Cannot generate writer for component type MyBehaviour. Use a supported type or provide a custom writer (at GeneratedReaderWriter.GivesErrorWhenUsingTypeInheritedFromMonoBehaviour.MyBehaviour)");
            AssertHasError("Cannot generate reader for component type MyBehaviour. Use a supported type or provide a custom reader (at GeneratedReaderWriter.GivesErrorWhenUsingTypeInheritedFromMonoBehaviour.MyBehaviour)");
        }

        [BatchedTest(true)]
        public void ExcludesNonSerializedFields()
        {
            // we test this by having a not allowed type in the class, but mark it with NonSerialized
            Assert.Pass();
        }

        [BatchedTest(false)]
        public void GivesErrorWhenUsingInterface()
        {
            AssertHasError("Cannot generate writer for interface IData. Use a supported type or provide a custom writer (at GeneratedReaderWriter.GivesErrorWhenUsingInterface.IData)");
            AssertHasError("Cannot generate reader for interface IData. Use a supported type or provide a custom reader (at GeneratedReaderWriter.GivesErrorWhenUsingInterface.IData)");
        }

        [BatchedTest(true)]
        public void CanUseCustomReadWriteForInterfaces()
        {
            Assert.Pass();
        }

        [BatchedTest(true)]
        public void CreatesForEnums()
        {
            Assert.Pass();
        }

        [BatchedTest(true)]
        public void CreatesForArraySegment()
        {
            Assert.Pass();
        }

        [BatchedTest(true)]
        public void CreatesForStructArraySegment()
        {
            Assert.Pass();
        }

        [BatchedTest(false)]
        public void GivesErrorForJaggedArray()
        {
            AssertHasError($"Int32[][] is an unsupported type. Jagged and multidimensional arrays are not supported (at System.Int32[][])");
        }

        [BatchedTest(false)]
        public void GivesErrorForMultidimensionalArray()
        {
            AssertHasError($"Int32[0...,0...] is an unsupported type. Jagged and multidimensional arrays are not supported (at System.Int32[0...,0...])");
        }

        [BatchedTest(false)]
        public void GivesErrorForInvalidArrayType()
        {
            AssertHasError("Cannot generate writer for Array because element MonoBehaviour does not have a writer. Use a supported type or provide a custom writer (at UnityEngine.MonoBehaviour[])");
            AssertHasError("Cannot generate reader for Array because element MonoBehaviour does not have a reader. Use a supported type or provide a custom reader (at UnityEngine.MonoBehaviour[])");
        }

        [BatchedTest(false)]
        public void GivesErrorForInvalidArraySegmentType()
        {
            AssertHasError("Cannot generate writer for ArraySegment because element MonoBehaviour does not have a writer. Use a supported type or provide a custom writer (at System.ArraySegment`1<UnityEngine.MonoBehaviour>)");
            AssertHasError("Cannot generate reader for ArraySegment because element MonoBehaviour does not have a reader. Use a supported type or provide a custom reader (at System.ArraySegment`1<UnityEngine.MonoBehaviour>)");
        }

        [BatchedTest(true)]
        public void CreatesForList()
        {
            Assert.Pass();
        }

        [BatchedTest(true)]
        public void CreatesForStructList()
        {
            Assert.Pass();
        }

        [BatchedTest(false)]
        public void GivesErrorForInvalidListType()
        {
            AssertHasError("Cannot generate writer for List because element MonoBehaviour does not have a writer. Use a supported type or provide a custom writer (at System.Collections.Generic.List`1<UnityEngine.MonoBehaviour>)");
            AssertHasError("Cannot generate reader for List because element MonoBehaviour does not have a reader. Use a supported type or provide a custom reader (at System.Collections.Generic.List`1<UnityEngine.MonoBehaviour>)");
        }
    }
}
