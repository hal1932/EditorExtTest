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
