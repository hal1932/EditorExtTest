using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;

namespace HlLib.Unity
{
    public static class ParallelUtil
    {
        public static void For(int fromInclusive, int toExclusive, Action<int> body)
        {
            var count = toExclusive - fromInclusive;
            var current = 0L;

            var evt = new ManualResetEvent(false);

            for (var i = fromInclusive; i < toExclusive; ++i)
            {
                ThreadPool.QueueUserWorkItem((indexObj) =>
                {
                    var index = (int)indexObj;
                    body(index);
                    if (Interlocked.Increment(ref current) == count)
                    {
                        evt.Set();
                    }
                }, i);
            }

            evt.WaitOne();
        }

        public static void ForEach<TSource>(IEnumerable<TSource> source, Action<TSource> body)
        {
            var sourceArray = source.ToArray();
            var count = sourceArray.Length;
            var current = 0L;

            var evt = new ManualResetEvent(false);

            for (var i = 0; i < count; ++i)
            {
                ThreadPool.QueueUserWorkItem((itemObj) =>
                {
                    var sourceItem = (TSource)itemObj;
                    body(sourceItem);
                    if (Interlocked.Increment(ref current) == count)
                    {
                        evt.Set();
                    }
                }, sourceArray[i]);
            }

            evt.WaitOne();
        }
    }
}
