using System.Reflection;
using NUnit.Framework;
using UnityEngine;

namespace Mirror.Tests
{
    [TestFixture]
    public class AnimatorHelperTest
    {
        [Test]
        public void TestMoveParmetersToStartOfArray()
        {
            AnimatorControllerParameter[] parameters = new []
            {
                new AnimatorControllerParameter(),
                new AnimatorControllerParameter(),
                null,
                new AnimatorControllerParameter(),
            };
            int count = invokeMoveParmetersToStartOfArray(parameters);

            Assert.AreEqual(count, 3);
            CheckNullElements(parameters, count);
        }

        [Test]
        public void TestMoveParmetersToStartOfArrayWithMultipleNull()
        {
            AnimatorControllerParameter[] parameters = new []
            {
                new AnimatorControllerParameter(),
                new AnimatorControllerParameter(),
                null,
                new AnimatorControllerParameter(),
                null,
                null,
                new AnimatorControllerParameter(),
            };

            int count = invokeMoveParmetersToStartOfArray(parameters);


            Assert.AreEqual(count, 4);
            CheckNullElements(parameters, count);
        }
        [Test]
        public void TestMoveParmetersToStartOfArrayWithNoNull()
        {
            AnimatorControllerParameter[] parameters = new []
            {
                new AnimatorControllerParameter(),
                new AnimatorControllerParameter(),
                new AnimatorControllerParameter(),
            };

            int count = invokeMoveParmetersToStartOfArray(parameters);

            Assert.AreEqual(count, 3);
            CheckNullElements(parameters, count);
        }


        private static int invokeMoveParmetersToStartOfArray(AnimatorControllerParameter[] parameters)
        {
            MethodInfo method = typeof(AnimatorHelper).GetMethod("moveParmetersToStartOfArray", BindingFlags.NonPublic | BindingFlags.Static);
            int count = (int)method.Invoke(null, new object[] { parameters });
            return count;
        }
        private static void CheckNullElements(AnimatorControllerParameter[] parameters, int count)
        {
            for (int i = 0; i < count; i++)
            {
                Assert.IsNotNull(parameters[i]);
            }
            for (int i = count; i < parameters.Length; i++)
            {
                Assert.IsNull(parameters[i]);
            }
        }
    }
}
