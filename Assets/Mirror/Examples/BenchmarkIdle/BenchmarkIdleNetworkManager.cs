using System.Collections;
using System.Collections.Generic;
using JamesFrowen.Benchmarker;
using JamesFrowen.Benchmarker.Weaver;
using UnityEngine;

namespace Mirror.Examples.BenchmarkIdle
{
    [AddComponentMenu("")]
    public class BenchmarkIdleNetworkManager : NetworkManager
    {
        [Header("Spawns")]
        public int PlayerCount = 4;

        public int spawnAmount = 10_000;
        public float interleave = 1;
        public GameObject spawnPrefab;

        // player spawn positions should be spread across the world.
        // not all at one place.
        // but _some_ at the same place.
        // => deterministic random is ideal
        [Range(0, 1)] public float spawnPositionRatio = 0.01f;
        private System.Random random = new System.Random(42);
        private List<Npc> npcs = new List<Npc>();
        private bool _benchmarkStarted;

        private void SpawnAll()
        {
            // clear previous player spawn positions in case we start twice
            foreach (Transform position in startPositions)
                Destroy(position.gameObject);

            startPositions.Clear();

            // calculate sqrt so we can spawn N * N = Amount
            float sqrt = Mathf.Sqrt(spawnAmount);

            // calculate spawn xz start positions
            // based on spawnAmount * distance
            float offset = -sqrt / 2 * interleave;

            // spawn exactly the amount, not one more.
            int spawned = 0;
            for (int spawnX = 0; spawnX < sqrt; ++spawnX)
            {
                for (int spawnZ = 0; spawnZ < sqrt; ++spawnZ)
                {
                    // spawn exactly the amount, not any more
                    // (our sqrt method isn't 100% precise)
                    if (spawned < spawnAmount)
                    {
                        // spawn & position
                        GameObject go = Instantiate(spawnPrefab);
                        float x = offset + (spawnX * interleave);
                        float z = offset + (spawnZ * interleave);
                        Vector3 position = new Vector3(x, 0, z);
                        go.transform.position = position;

                        // spawn
                        NetworkServer.Spawn(go);
                        ++spawned;

                        npcs.Add(go.GetComponent<Npc>());

                        // add random spawn position for players.
                        // don't have them all in the same place.
                        if (random.NextDouble() <= spawnPositionRatio)
                        {
                            GameObject spawnGO = new GameObject("Spawn");
                            spawnGO.transform.position = position;
                            spawnGO.AddComponent<NetworkStartPosition>();
                        }
                    }
                }
            }
        }

        // overwrite random spawn position selection:
        // - needs to be deterministic so every CCU test results in the same
        // - needs to be random so not only are the spawn positions spread out
        //   randomly, we also have a random amount of players per spawn position
        public override Transform GetStartPosition()
        {
            // first remove any dead transforms
            startPositions.RemoveAll(t => t == null);

            if (startPositions.Count == 0)
                return null;

            // pick a random one
            int index = random.Next(0, startPositions.Count); // DETERMINISTIC
            return startPositions[index];
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            SpawnAll();
        }


        public override void Awake()
        {
            base.Awake();
            Camera.main.enabled = false;
        }

        public override void OnServerConnect(NetworkConnectionToClient conn)
        {
            base.OnServerConnect(conn);

            if (NetworkServer.connections.Count == PlayerCount)
            {
                StartCoroutine(DelayStart());
            }
        }

        public override void OnClientDisconnect()
        {
            base.OnClientDisconnect();
            Quit();
        }

        private IEnumerator DelayStart()
        {
            yield return new WaitForSeconds(1);

            BenchmarkRunner.ResultName = "Results-Mirror";
            BenchmarkRunner.ResultFolder = "../Mirage/Results";
            BenchmarkRunner.StartRecording(3000, true, true);
            BenchmarkRunner.MetaData = new List<string>()
            {
                $"PlayerCount:{PlayerCount}",
                $"spawnAmount:{spawnAmount}",
                $"Idle %:{npcs[0].sleepingProbability}",
#if GET_ID_CACHE
                $"GET_ID_CACHE",
#endif
#if SET_DIRTY_NO_LOG
                $"SET_DIRTY_NO_LOG",
#endif
#if WAS_ZERO
                $"WAS_ZERO",
#endif
#if SHOULD_SYNC_CACHE
                $"SHOULD_SYNC_CACHE",
#endif
#if DIRTY_SET_CACHE
                $"DIRTY_SET_CACHE",
#elif DIRTY_LIST_CACHE
                $"DIRTY_LIST_CACHE",
#endif
#if DIRTY_LIST
                $"DIRTY_LIST",
#endif
#if NO_DIRTY_LIST
                $"NO_DIRTY_LIST",
#endif
                "SyncVarSender.Update Self Only",
                "No_interval"
            };


            _benchmarkStarted = true;
        }

        public override void Update()
        {
            base.Update();
            if (_benchmarkStarted)
            {
                if (!BenchmarkHelper.IsRunning)
                {
                    // finished
                    Quit();
                    return;
                }

                if (NetworkServer.active)
                    ServerUpdate();
            }
            else
            {
                NetworkServer.NetworkLateUpdate();
                NetworkServer.NetworkEarlyUpdate();
            }
        }

        private static void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
        }

        [BenchmarkMethod("Server Update")]
        private void ServerUpdate()
        {
            UpdateReceived();
            UpdateSyncVar();
            UpdateSent();
        }

        [BenchmarkMethod("Set Sync var field")]
        private void UpdateSyncVar()
        {
            foreach (Npc npc in npcs)
            {
                npc.Update_SetSyncVar();
            }
        }


        [BenchmarkMethod("Late Update")]
        private void UpdateSent()
        {
            NetworkServer.NetworkLateUpdate();
        }

        [BenchmarkMethod("Early Update")]
        private void UpdateReceived()
        {
            NetworkServer.NetworkEarlyUpdate();
        }
    }
}
