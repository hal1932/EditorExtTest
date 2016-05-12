using HlLib.Unity;
using HlLib.Unity.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
    public class HttpTest : ScriptableObject
    {
        [MenuItem("GameObject/test", false, 20)]
        public static void Test()
        {
            var count = 0;
            var begin = DateTime.Now;
            ParallelUtil.For(0, 10000, (index) => count += f(index));
            Debug.Log("1 " + (DateTime.Now - begin).Ticks);

            count = 0;
            begin = DateTime.Now;
            ParallelUtil.ForEach(Enumerable.Range(0, 10000), (index) => count += f(index));
            Debug.Log("3 " + (DateTime.Now - begin).Ticks);

            //EditorUtility.DisplayProgressBar("", "", 0);
            //Thread.Sleep(5000);
            //EditorUtility.ClearProgressBar();

#if false
            using (var req = new HttpClient("http://google.co.jp", HttpMethod.Get))
            {
                var res = req.Send();
                using (var stream = res.GetStream())
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    Debug.Log(reader.ReadToEnd());
                }
            }

            using (var req = new HttpClient("http://google.co.jp", HttpMethod.Get))
            {
                var sendComplete = req.SendAsync(null, r => Debug.Log("hoge"));
                sendComplete.WaitOne();
                using (var stream = req.LastResponse.GetStream())
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    Debug.Log(reader.ReadToEnd());
                }
            }

            using (var req = new HttpClient("http://google.co.jp", HttpMethod.Get))
            {
                req.Timeout = 1;
                var sendComplete = req.SendAsync(null, res =>
                {
                    if (res == null)
                    {
                        Debug.Log("timeout");
                    }
                });
                sendComplete.WaitOne();
            }
#endif

            using (var req = new HttpClient("http://zipcloud.ibsnet.co.jp/api/search?zipcode=1000000", HttpMethod.Get))
            {
                var res = req.Send();
                using (var stream = res.GetStream())
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    Debug.Log(reader.ReadToEnd());
                }
            }

            using (var req = new HttpClient("http://zipcloud.ibsnet.co.jp/api/search", HttpMethod.Post))
            {
                var content = new FormUrlEncodedContent(new { zipcode = 1000000 });
                var res = req.Send(content);
                using (var stream = res.GetStream())
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    Debug.Log(reader.ReadToEnd());
                }
            }
        }

        private static int f(int i)
        {
            return i;
        }
    }
}
