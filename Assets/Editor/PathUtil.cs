using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Assets.Editor
{
    public static class PathUtil
    {
        public static string Combine(params string[] names)
        {
            return string.Join("/", names);
        }
    }
}
