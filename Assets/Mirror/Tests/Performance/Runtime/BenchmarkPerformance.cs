using System;
using System.Collections;
using System.Diagnostics;
//using Mirror.Examples;
using NUnit.Framework;
using Unity.PerformanceTesting;
using UnityEngine;
using UnityEngine.TestTools;

namespace Mirror.Tests.Performance
{
    public class BenchmarkNetworkManager : NetworkManager
    {
        /// <summary>
        /// hook for benchmarking
        /// </summary>
        public Action BeforeLateUpdate;
        /// <summary>
        /// hook for benchmarking
        /// </summary>
        public Action AfterLateUpdate;

        public override void Awake()
        {
            transport = gameObject.AddComponent<TelepathyTransport>();
            playerPrefab = new GameObject("testPlayerPrefab", typeof(NetworkIdentity));
            base.Awake();
        }

        public override void LateUpdate()
        {
            BeforeLateUpdate?.Invoke();
            base.LateUpdate();
            AfterLateUpdate?.Invoke();
        }
    }
    public class BenchmarkNetworkBehaviour : NetworkBehaviour
    {
        [SyncVar] public int health = 10;

        [ServerCallback]
        public void Update()
        {
            health = (health + 1) % 10;
        }
    }

    [Category("Performance")]
    [Category("Benchmark")]
    public class BenchmarkPerformance
    {
        const string ScenePath = "Assets/Mirror/Examples/Benchmarks/10k/Scenes/Scene.unity";
        const float Warmup = 1f;
        const int MeasureCount = 120;

        readonly SampleGroupDefinition NetworkManagerSample = new SampleGroupDefinition("NetworkManagerLateUpdate", SampleUnit.Millisecond, AggregationType.Average);
        readonly Stopwatch stopwatch = new Stopwatch();
        bool captureMeasurement;
        void BeforeLateUpdate()
        {
            if (!captureMeasurement)
                return;

            stopwatch.Start();
        }
        void AfterLateUpdate()
        {
            if (!captureMeasurement)
                return;

            stopwatch.Stop();
            double time = stopwatch.Elapsed.TotalMilliseconds;
            Measure.Custom(NetworkManagerSample, time);
            stopwatch.Reset();
        }

        BenchmarkNetworkManager networkManager;
        BenchmarkNetworkBehaviour[] behaviours;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            captureMeasurement = false;
            // load scene
            //yield return EditorSceneManager.LoadSceneAsyncInPlayMode(ScenePath, new LoadSceneParameters { loadSceneMode = LoadSceneMode.Additive });
            //Scene scene = SceneManager.GetSceneByPath(ScenePath);
            //SceneManager.SetActiveScene(scene);

            GameObject netManagerObject = new GameObject("BenchmarkNetMan");
            networkManager = netManagerObject.AddComponent<BenchmarkNetworkManager>();

            // wait for NetworkManager awake
            yield return null;
            // load host
            networkManager.StartHost();

            networkManager.BeforeLateUpdate = BeforeLateUpdate;
            networkManager.AfterLateUpdate = AfterLateUpdate;

            // wait for host to fully start
            yield return null;


            const int behaviourCount10k = 10000;
            behaviours = new BenchmarkNetworkBehaviour[behaviourCount10k];
            for (int i = 0; i < behaviourCount10k; i++)
            {
                GameObject go = new GameObject("Behaviour" + i);
                go.AddComponent<NetworkIdentity>();
                behaviours[i] = go.AddComponent<BenchmarkNetworkBehaviour>();
                NetworkServer.Spawn(go);
            }
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            // run benchmark
            yield return Measure.Frames().MeasurementCount(MeasureCount).Run();

            // shutdown
            NetworkManager.Shutdown();

            // destroy scene Objects

            GameObject.Destroy(networkManager.gameObject);
            for (int i = 0; i < behaviours.Length; i++)
            {
                GameObject.Destroy(behaviours[i].gameObject);
            }

        }

        void EnableHealth(bool value)
        {
            foreach (BenchmarkNetworkBehaviour behaviour in behaviours)
            {
                behaviour.enabled = value;
            }
        }

        [UnityTest]
#if UNITY_2019_2_OR_NEWER
        [Performance]
#else
        [PerformanceUnityTest]
#endif
        public IEnumerator Benchmark10k()
        {
            EnableHealth(true);
            // warmup
            yield return new WaitForSecondsRealtime(Warmup);

            captureMeasurement = true;

            for (int i = 0; i < MeasureCount; i++)
            {
                yield return null;
            }

            captureMeasurement = false;
        }

        [UnityTest]
#if UNITY_2019_2_OR_NEWER
        [Performance]
#else
        [PerformanceUnityTest]
#endif
        public IEnumerator Benchmark10kIdle()
        {
            EnableHealth(false);

            // warmup
            yield return new WaitForSecondsRealtime(Warmup);

            captureMeasurement = true;

            for (int i = 0; i < MeasureCount; i++)
            {
                yield return null;
            }

            captureMeasurement = false;
        }
    }
}
