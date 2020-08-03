#if !UNITY_2019_2_OR_NEWER || UNITY_PERFORMANCE_TESTS_1_OR_OLDER
using System.Diagnostics;
using NUnit.Framework;
using Unity.PerformanceTesting;
using UnityEngine;

namespace Mirror.Tests.Performance
{
    [Category("Performance")]
    [Category("Benchmark")]
    public class NetworkTransformPerformance
    {
        GameObject gameObject;
        NetworkIdentity identity;
        NetworkTransform networkTransform;


        [SetUp]
        public void SetUp()
        {
            gameObject = new GameObject();
            identity = gameObject.AddComponent<NetworkIdentity>();
            identity.observers = new System.Collections.Generic.Dictionary<int, NetworkConnection>();
            identity.connectionToClient = new FakeNetworkConnection(1);
            identity.observers.Add(1, identity.connectionToClient);
            networkTransform = gameObject.AddComponent<NetworkTransform>();
            networkTransform.syncMode = SyncMode.Owner;
            networkTransform.syncInterval = 0f;
        }
        [TearDown]
        public void TearDown()
        {
            UnityEngine.Object.DestroyImmediate(gameObject);
        }

        [Test]
#if UNITY_2019_2_OR_NEWER
        [Performance]
#else
        [PerformanceTest]
#endif
        public void HasEitherMovedRotatedScaledNotDirty()
        {
            Measure.Method(RunNotDirty)
                .WarmupCount(10)
                .MeasurementCount(100)
                .Run();
        }

        void RunNotDirty()
        {
            for (int i = 0; i < 10000; i++)
            {
                networkTransform.HasEitherMovedRotatedScaled_OLD();
            }
        }


        [Test]
#if UNITY_2019_2_OR_NEWER
        [Performance]
#else
        [PerformanceTest]
#endif
        public void HasEitherMovedRotatedScaledPositionDirty()
        {
            Measure.Method(RunPositionDirty)
                .WarmupCount(10)
                .MeasurementCount(100)
                .Run();
        }

        void RunPositionDirty()
        {
            for (int j = 0; j < 10000; j++)
            {
                networkTransform.HasEitherMovedRotatedScaled_OLD();
                networkTransform.transform.position += new Vector3(Mathf.PingPong(j, 10), 0, 0);
            }
        }


        [Test]
#if UNITY_2019_2_OR_NEWER
        [Performance]
#else
        [PerformanceTest]
#endif
        public void DistancePerformance()
        {
            Measure.Method(RunDistance)
                .WarmupCount(10)
                .MeasurementCount(100)
                .Run();
        }

        [Test]
#if UNITY_2019_2_OR_NEWER
        [Performance]
#else
        [PerformanceTest]
#endif
        public void SqrPerformance()
        {
            Measure.Method(RunSqr)
                .WarmupCount(10)
                .MeasurementCount(100)
                .Run();
        }

        [Test]
#if UNITY_2019_2_OR_NEWER
        [Performance]
#else
        [PerformanceTest]
#endif
        public void ManualSqrPerformance()
        {
            Measure.Method(RunManualSqr)
                .WarmupCount(10)
                .MeasurementCount(100)
                .Run();
        }

        void RunDistance()
        {
            Vector3 v1 = new Vector3(20, 10, -10);
            Vector3 v2 = new Vector3(-30, 10, 20);
            float sens = 10f;
            for (int j = 0; j < 10000; j++)
            {
                bool moved = Vector3.Distance(v1, v2) > sens;
            }
        }
        void RunSqr()
        {
            Vector3 v1 = new Vector3(20, 10, -10);
            Vector3 v2 = new Vector3(-30, 10, 20);
            float sens = 10f;
            float sqSens = sens * sens;
            for (int j = 0; j < 10000; j++)
            {
                bool moved = (v1 - v2).sqrMagnitude > sqSens;
            }
        }
        void RunManualSqr()
        {
            Vector3 v1 = new Vector3(20, 10, -10);
            Vector3 v2 = new Vector3(-30, 10, 20);
            float sens = 10f;
            float sqSens = sens * sens;
            for (int j = 0; j < 10000; j++)
            {
                float dx = v1.x - v2.x;
                float dy = v1.y - v2.y;
                float dz = v1.z - v2.z;
                bool moved = (dx * dx + dy * dy + dz * dz) > sqSens;
            }
        }




        [Test]
#if UNITY_2019_2_OR_NEWER
        [Performance]
#else
        [PerformanceTest]
#endif
        public void HasEitherMovedRotatedScaledOptmisedNotDirty()
        {
            Measure.Method(RunOptmisedNotDirty)
                .WarmupCount(10)
                .MeasurementCount(100)
                .Run();
        }

        void RunOptmisedNotDirty()
        {
            Transform transform = networkTransform.transform;
            for (int j = 0; j < 10000; j++)
            {
                networkTransform.HasEitherMovedRotatedScaled_Optmised(transform.localPosition, transform.localRotation, transform.localScale);
            }
        }

        [Test]
#if UNITY_2019_2_OR_NEWER
        [Performance]
#else
        [PerformanceTest]
#endif
        public void HasEitherMovedRotatedScaledOptmisedPositionDirty()
        {
            Measure.Method(RunOptmisedPositionDirty)
                .WarmupCount(10)
                .MeasurementCount(100)
                .Run();
        }

        void RunOptmisedPositionDirty()
        {
            Transform transform = networkTransform.transform;
            for (int j = 0; j < 10000; j++)
            {
                networkTransform.HasEitherMovedRotatedScaled_Optmised(transform.localPosition, transform.localRotation, transform.localScale);
                transform.position += new Vector3(Mathf.PingPong(j, 10), 0, 0);
            }
        }



        [Test]
        public void QuickRun()
        {
            Vector3 pos1 = new Vector3(10, 20, 30);
            //Vector3 pos2 = pos1;
            Vector3 pos2 = new Vector3(0, 20, 30);

            Quaternion rot1 = Quaternion.Euler(20, 10, 200);
            //Quaternion rot2 = rot1;
            Quaternion rot2 = Quaternion.Euler(24f, 10, 200);

            Vector3 sca1 = new Vector3(1, 1, 1);
            //Vector3 sca2 = sca1;
            Vector3 sca2 = new Vector3(2, 1, 1);

            UnityEngine.Debug.Log($"all Changed");
            DoQuickRun(pos1, pos2, rot1, rot2, sca1, sca2);

            UnityEngine.Debug.Log($"none Changed");
            DoQuickRun(pos1, pos1, rot1, rot1, sca1, sca1);

            UnityEngine.Debug.Log($"pos Changed");
            DoQuickRun(pos1, pos2, rot1, rot1, sca1, sca1);

            UnityEngine.Debug.Log($"rot Changed");
            DoQuickRun(pos1, pos1, rot1, rot2, sca1, sca1);

            UnityEngine.Debug.Log($"sca Changed");
            DoQuickRun(pos1, pos1, rot1, rot1, sca1, sca2);
        }

        private void DoQuickRun(Vector3 pos1, Vector3 pos2, Quaternion rot1, Quaternion rot2, Vector3 sca1, Vector3 sca2)
        {
            const int count = 1000000;

            networkTransform.lastPosition = pos1;
            networkTransform.lastRotation = rot1;
            networkTransform.lastScale = sca1;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int j = 0; j < count; j++)
            {
                networkTransform.HasEitherMovedRotatedScaled_Optmised(pos2, rot2, sca2);
            }
            stopwatch.Stop();
            UnityEngine.Debug.Log("Optmised".PadRight(20) + stopwatch.ElapsedMilliseconds);

            stopwatch.Reset();
            stopwatch.Start();
            for (int j = 0; j < count; j++)
            {
                networkTransform.HasEitherMovedRotatedScaled_alt1(pos2, rot2, sca2);
            }
            stopwatch.Stop();
            UnityEngine.Debug.Log("Old".PadRight(20) + stopwatch.ElapsedMilliseconds);

            stopwatch.Reset();
            stopwatch.Start();
            for (int j = 0; j < count; j++)
            {
                networkTransform.HasEitherMovedRotatedScaled_alt2(pos2, rot2, sca2);
            }
            stopwatch.Stop();
            UnityEngine.Debug.Log("in-line if".PadRight(20) + stopwatch.ElapsedMilliseconds);
        }


        [Test]
#if UNITY_2019_2_OR_NEWER
        [Performance]
#else
        [PerformanceTest]
#endif
        public void CompareDistances()
        {
            const int count = 100000;
            const int warmup = count / 100;
            Vector3 v1 = new Vector3(10, 20, 30);
            Vector3 v2 = new Vector3(0, 20, 30);
            float sens = 0.5f;

            Measure.Method(() => Distance(v1, v2, sens))
                .Definition(name: "Distance", sampleUnit: SampleUnit.Nanosecond)
                .WarmupCount(warmup)
                .MeasurementCount(count)
                .Run();

            Measure.Method(() => SqMag(v1, v2, sens))
               .Definition(name: "SqMag", sampleUnit: SampleUnit.Nanosecond)
               .WarmupCount(warmup)
               .MeasurementCount(count)
               .Run();

            Measure.Method(() => SqManual(v1, v2, sens))
               .Definition(name: "SqManual", sampleUnit: SampleUnit.Nanosecond)
               .WarmupCount(warmup)
               .MeasurementCount(count)
               .Run();

            Measure.Method(() => HasMoved(v1, v2, sens))
               .Definition(name: "HasMoved", sampleUnit: SampleUnit.Nanosecond)
               .WarmupCount(warmup)
               .MeasurementCount(count)
               .Run();

            Measure.Method(() => HasMoved2(v1, v2, sens))
               .Definition(name: "HasMoved2", sampleUnit: SampleUnit.Nanosecond)
               .WarmupCount(warmup)
               .MeasurementCount(count)
               .Run();

            //Stopwatch stopwatch = new Stopwatch();
            //stopwatch.Start();
            //for (int j = 0; j < count; j++)
            //{
            //    Distance(v1, v2, sens);
            //}
            //stopwatch.Stop();
            //UnityEngine.Debug.Log("Distance".PadRight(20) + stopwatch.ElapsedMilliseconds);

            //stopwatch.Reset();
            //stopwatch.Start();
            //for (int j = 0; j < count; j++)
            //{
            //    SqMag(v1, v2, sens);
            //}
            //stopwatch.Stop();
            //UnityEngine.Debug.Log("SqMag".PadRight(20) + stopwatch.ElapsedMilliseconds);

            //stopwatch.Reset();
            //stopwatch.Start();
            //for (int j = 0; j < count; j++)
            //{
            //    SqManual(v1, v2, sens);
            //}
            //stopwatch.Stop();
            //UnityEngine.Debug.Log("SqManual".PadRight(20) + stopwatch.ElapsedMilliseconds);

            //stopwatch.Reset();
            //stopwatch.Start();
            //for (int j = 0; j < count; j++)
            //{
            //    HasMoved(v1, v2, sens);
            //}
            //stopwatch.Stop();
            //UnityEngine.Debug.Log("HasMoved".PadRight(20) + stopwatch.ElapsedMilliseconds);

            //stopwatch.Reset();
            //stopwatch.Start();
            //for (int j = 0; j < count; j++)
            //{
            //    HasMoved2(v1, v2, sens);
            //}
            //stopwatch.Stop();
            //UnityEngine.Debug.Log("HasMoved2".PadRight(20) + stopwatch.ElapsedMilliseconds);
        }

        // 87ms
        private static bool Distance(Vector3 v1, Vector3 v2, float sens)
        {
            return Vector3.Distance(v1, v2) > sens;
        }
        // 84ms
        private static bool SqMag(Vector3 v1, Vector3 v2, float sens)
        {
            return (v1 - v2).sqrMagnitude > sens * sens;
        }
        // 30ms
        private static bool SqManual(Vector3 v1, Vector3 v2, float sens)
        {
            float dx = v1.x - v2.x;
            float dy = v1.y - v2.y;
            float dz = v1.z - v2.z;
            return (dx * dx + dy * dy + dz * dz) > sens * sens;
        }

        // 30ms
        private static bool HasMoved(Vector3 v1, Vector3 v2, float sens)
        {
            float dx = v1.x - v2.x;
            float dy = v1.y - v2.y;
            float dz = v1.z - v2.z;

            return dx > sens || dy > sens || dz > sens;
        }
        // 30ms
        private static bool HasMoved2(Vector3 v1, Vector3 v2, float sens)
        {
            float dx = v1.x - v2.x;
            float dy = v1.y - v2.y;
            float dz = v1.z - v2.z;

            return dx > sens | dy > sens | dz > sens;
        }
    }
}
#endif
