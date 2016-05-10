using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
    class CreateScriptArgs
    {
        public GameObject TargetObject { get; private set; }
        public string FullName { get; private set; }
        public string Name { get { return Path.GetFileName(FullName); } }
        public string NameSpace { get; private set; }

        public CreateScriptArgs(GameObject targetObject, string fullname, string nameSpace)
        {
            TargetObject = targetObject;
            FullName = fullname;
            NameSpace = nameSpace;
        }
    }

    class InputScriptNameWindow : EditorWindow
    {
        public string Directory { get; private set; }
        public string ScriptName { get; private set; }
        public string NameSpace { get; private set; }

        public static void Open(GameObject targetObject, Action<CreateScriptArgs> onCreate)
        {
            var window = CreateInstance<InputScriptNameWindow>();
            window.Setup(targetObject, onCreate);
            window.ShowUtility();
        }

        private void Setup(GameObject targetObject, Action<CreateScriptArgs> onCreate)
        {
            var sceneName = Path.GetFileNameWithoutExtension(targetObject.scene.name);

            Directory = PathUtil.Combine("Assets", "Scripts", sceneName);
            ScriptName = targetObject.name.Replace(" ", string.Empty) + ".cs";
            NameSpace = sceneName;

            _targetObject = targetObject;
            _onCreate = onCreate;

            titleContent = new GUIContent("作成するスクリプト名を入力してください");
        }

        void OnGUI()
        {
            Directory = EditorGUILayout.TextField("ディレクトリ", Directory);
            ScriptName = EditorGUILayout.TextField("スクリプト名", ScriptName);
            NameSpace = EditorGUILayout.TextField("ネームスペース", NameSpace);

            using (new ScopedGuiEnabled(!string.IsNullOrEmpty(ScriptName)))
            using (new ScopedHorizontalAlignment(Alignment.Right))
            {
                if (LayoutUtil.SizedButton("作成", 75))
                {
                    Close();
                }
            }
        }

        void OnDestroy()
        {
            if (_onCreate != null)
            {
                _onCreate(new CreateScriptArgs(
                    _targetObject,
                    PathUtil.Combine(Directory, ScriptName),
                    NameSpace));
            }
        }

        private GameObject _targetObject;
        private Action<CreateScriptArgs> _onCreate;
    }
}
