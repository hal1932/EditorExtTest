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
            var evt = new ManualResetEvent(false);

            var count = toExclusive - fromInclusive;
            var current = 0L;

            for (var i = fromInclusive; i < toExclusive; ++i)
            {
                ThreadPool.QueueUserWorkItem((index) =>
                {
                    body((int)index);
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
            var evt = new ManualResetEvent(false);

            var last = source.Last();
            foreach (var item in source)
            {
                ThreadPool.QueueUserWorkItem((itemObj) =>
                {
                    var sourceItem = (TSource)itemObj;
                    body(sourceItem);
                    if (sourceItem.Equals(last))
                    {
                        evt.Set();
                    }
                }, item);
            }

            evt.WaitOne();
        }
    }
}
