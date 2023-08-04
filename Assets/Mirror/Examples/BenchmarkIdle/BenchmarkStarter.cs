using System;
using UnityEngine;

namespace Mirror.Examples.BenchmarkIdle
{
    public class BenchmarkStarter : MonoBehaviour
    {
        public BenchmarkIdleNetworkManager Manager;

        public string editorArgs;

        private void Awake()
        {
            Application.targetFrameRate = 60;
            string[] args;
            if (Application.isEditor)
            {
                args = editorArgs.Split(' ');
            }
            else
            {
                args = Environment.GetCommandLineArgs();
            }

            for (var i = 0; i < args.Length; i++)
            {
                var arg = args[i];
                if (arg.ToLower().Contains("-count".ToLower()))
                {
                    Manager.PlayerCount = int.Parse(args[i + 1]);
                }

                if (arg.ToLower().Contains("-server".ToLower()))
                {
                    Manager.StartServer();
                }

                if (arg.ToLower().Contains("-client".ToLower()))
                {
                    Manager.StartClient();
                }
            }
        }
    }
}
