using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
    class InputScriptNameWindow : EditorWindow
    {
        public string ScriptName { get; private set; }

        public InputScriptNameWindow()
        {
            ScriptName = "Script.cs";
            titleContent = new GUIContent("作成するスクリプト名を入力してください");
        }

        public static void Open(Action<InputScriptNameWindow> onClosed)
        {
            var window = CreateInstance<InputScriptNameWindow>();
            window._onClosed = onClosed;
            window.ShowUtility();
        }

        void OnGUI()
        {
            ScriptName = EditorGUILayout.TextField("スクリプト名", ScriptName);

            using (new ScopedGuiEnabled(!string.IsNullOrEmpty(ScriptName)))
            using (new ScopedHorizontalAlignment(Alignment.Right))
            {
                if (LayoutUtil.SizedButton("作成", 75))
                {
                    Close();
                }
            }

            GUILayout.Button("hoge");
        }

        void OnDestroy()
        {
            if (_onClosed != null)
            {
                _onClosed(this);
            }
        }

        private Action<InputScriptNameWindow> _onClosed;
    }
}
