#if !UNITY_2019_2_OR_NEWER || UNITY_PERFORMANCE_TESTS_1_OR_OLDER
using System;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace Mirror.Tests.Performance
{
    [Category("Performance")]
    [Category("Benchmark")]
    public class EventPerformance
    {
        delegate void OnClientData(ArraySegment<byte> data, int channel);
        class Holder
        {
            public ClientDataReceivedEvent ClientDataReceived = new ClientDataReceivedEvent();
            public event OnClientData onClientData;

            internal void CallUnityEvent()
            {
                ClientDataReceived.Invoke(default, default);
            }

            internal void CallCSharpEvent()
            {
                onClientData?.Invoke(default, default);
            }
        }
        int number;

        [Test]
#if UNITY_2019_2_OR_NEWER
        [Performance]
#else
        [PerformanceTest]
#endif
        public void CheckPerformance()
        {
            Holder holder = new Holder();
            holder.ClientDataReceived.AddListener(addOne);
            holder.onClientData += addOne;

            const int innerLoopCount = 100000;
            const int measureCount = 100;
            const int warmUp = measureCount / 10;

            number = 0;
            Measure.Method(() =>
            {
                for (int i = 0; i < innerLoopCount; i++)
                {
                    holder.CallUnityEvent();

                }
            }).Definition("UnityEvent")
                .WarmupCount(warmUp)
                .MeasurementCount(measureCount)
                .Run();

            number = 0;
            Measure.Method(() =>
            {
                for (int i = 0; i < innerLoopCount; i++)
                {
                    holder.CallCSharpEvent();
                }
            }).Definition("CsharpEvent")
                .WarmupCount(warmUp)
                .MeasurementCount(measureCount)
                .Run();
        }

        private void addOne(ArraySegment<byte> arg0, int arg1)
        {
            number++;
        }
    }
}
#endif
