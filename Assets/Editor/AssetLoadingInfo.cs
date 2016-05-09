using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Assets.Editor
{
    [Serializable]
    public class AssetLoadingInfo : IDisposable
    {
        public string FilePath { get; set; }
        public string ObjectName { get; set; }
        public string SessionId { get; set; }

        public static AssetLoadingInfo Load()
        {
            if (!File.Exists(cFilePath))
            {
                return new AssetLoadingInfo();
            }

            var serializer = new XmlSerializer(typeof(AssetLoadingInfo));
            using (var reader = new StreamReader(cFilePath))
            {
                return serializer.Deserialize(reader) as AssetLoadingInfo;
            }
        }

        private AssetLoadingInfo()
        { }

        private static void Save(AssetLoadingInfo instance)
        {
            var serializer = new XmlSerializer(typeof(AssetLoadingInfo));
            using (var writer = new StreamWriter(cFilePath))
            {
                serializer.Serialize(writer, instance);
            }
        }

        private const string cFilePath = "Temp/_AssetLoadingInfo.xml";

        #region IDisposable
        ~AssetLoadingInfo()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            lock(_disposingLock)
            {
                if (_disposed)
                {
                    return;
                }
                _disposed = true;

                if (disposing)
                { }

                Save(this);
            }
        }

        private bool _disposed;
        private object _disposingLock = new object();
        #endregion
    }
}
