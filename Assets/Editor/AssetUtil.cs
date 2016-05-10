﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;

namespace Assets.Editor
{
    public static class AssetUtil
    {
        public static bool Exists(string path)
        {
            return FindByPath(path).Length > 0;
        }

        public static string[] FindByPath(string path)
        {
            var name = Path.GetFileNameWithoutExtension(path);
            var directory = Path.GetDirectoryName(path);

            return (string.IsNullOrEmpty(directory)) ?
                AssetDatabase.FindAssets(name)
                : AssetDatabase.FindAssets(name, new string[] { directory });
        }
    }
}
