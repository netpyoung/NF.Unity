using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace NFRuntime.Profile
{
    public class Monitor
    {
        public void Test()
        {

            //usedHeapSizeLong
            //public static long GetMonoHeapSizeLong();
            //public static long GetMonoUsedSizeLong();
            //public static long GetRuntimeMemorySizeLong(Object o);
            //public static uint GetTempAllocatorSize();
            //public static long GetTotalAllocatedMemoryLong();
            //public static long GetTotalReservedMemoryLong();

            // Marshal.SizeOf(object yourObj);
            // sizeof(object val)
            // 메가바이트 단위의, 시스템 메모리의 대략적인 양을 나타냅니다.

            //Debug.Log($"X {SystemInfo.graphicsMemorySize} / {SystemInfo.systemMemorySize}");

            //long mm = UnityEngine.Profiling.Profiler.GetTotalUnusedReservedMemoryLong();
            //long ma = UnityEngine.Profiling.Profiler.GetRuntimeMemorySizeLong(this);
            //Debug.Log($"{mm} {ma}");
            //long memory = GC.GetTotalMemory(false);
            //Debug.Log($" memory: {memory}");
        }
    }
}