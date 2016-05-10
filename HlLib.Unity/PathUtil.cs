using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HlLib.Unity
{
    public static class PathUtil
    {
        public static string Combine(params string[] names)
        {
            return string.Join("/", names);
        }
    }
}
