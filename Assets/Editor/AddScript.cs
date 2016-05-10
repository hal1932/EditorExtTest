using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Reflection;
using System.IO;
using Assets.Editor;
using UnityEditor.Callbacks;

public class AddScript : EditorWindow
{
    [MenuItem("GameObject/Add C# Script", false, 20)]
    public static void AddCsScript()
    {
        var targetObject = Selection.activeGameObject;
        if (targetObject == null)
        {
            return;
        }
        InputScriptNameWindow.Open(window => OnCreateScript(targetObject, window.ScriptName));
    }

    [DidReloadScripts]
    private static void OnReloaded()
    {
        using (var info = AssetLoadingInfo.Load())
        {
            var session = Session.Find(info.SessionId);
            if (session == null)
            {
                return;
            }
            session.Delete();

            if (File.Exists(info.FilePath))
            {
                var script = AssetDatabase.LoadAssetAtPath<MonoScript>(info.FilePath);
                if (script == null)
                {
                    return;
                }
                var scriptClass = script.GetClass();
                if (scriptClass != null)
                {
                    Attach(scriptClass, info.ObjectName);
                    info.FilePath = null;
                }
            }
        }
        EditorUtility.ClearProgressBar();
    }

    private static void OnCreateScript(GameObject targetObject, string scriptName)
    {
        var src = "Assets/Scripts/test.cs";
        var dst = "Assets/Scripts/test1.cs";

        var existScripts = AssetDatabase.FindAssets(Path.GetFileNameWithoutExtension(dst), new string[] { Path.GetDirectoryName(dst) });
        if(existScripts.Length > 0)
        {
            var doDelete = EditorUtility.DisplayDialog("duplicated name", "test1 is already exists. delete it?", "delete", "cancel");
            if (doDelete)
            {
                AssetDatabase.DeleteAsset(dst);
            }
            else
            {
                return;
            }
        }

        using (var writer = new StreamWriter(dst))
        using (var reader = new StreamReader(src))
        {
            var text = reader.ReadToEnd();
            writer.Write(text.Replace("test", "test1"));
        }

        using (var info = AssetLoadingInfo.Load())
        {
            info.FilePath = dst;
            info.ObjectName = targetObject.name;
            info.SessionId = Session.Create().Id;
        }
        AssetDatabase.ImportAsset(dst, ImportAssetOptions.ForceUpdate);

        EditorUtility.DisplayProgressBar("title", "info", 0.3f);
    }

    private static void Attach(Type scriptClass, string objectName)
    {
        var gameObject = GameObject.Find(objectName);
        gameObject.AddComponent(scriptClass);
    }
}
