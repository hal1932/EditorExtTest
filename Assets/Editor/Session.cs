using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;

namespace Assets.Editor
{
    public class Session
    {
        public string Id { get; private set; }

        public static Session Create(string content = null)
        {
            var filepath = FileUtil.GetUniqueTempPathInProject();
            File.WriteAllText(filepath, content);
            if (File.Exists(filepath))
            {
                return new Session(filepath);
            }
            return null;
        }

        public static Session Find(string id)
        {
            var filepath = GetFilePath(id);
            if (File.Exists(filepath))
            {
                return new Session(filepath);
            }
            return null;
        }

        public void Delete()
        {
            var filepath = GetFilePath(Id);
            if (File.Exists(filepath))
            {
                File.Delete(filepath);
            }
            Id = null;
        }

        public string LoadContent()
        {
            var filepath = GetFilePath(Id);
            if (File.Exists(filepath))
            {
                return File.ReadAllText(filepath);
            }
            return null;
        }

        private Session(string id)
        {
            Id = id;
        }

        private static string GetFilePath(string id)
        {
            return id;
        }
    }
}
