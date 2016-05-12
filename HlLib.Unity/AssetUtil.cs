using System.IO;
using UnityEditor;

namespace HlLib.Unity
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
