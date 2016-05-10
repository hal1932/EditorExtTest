using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Reflection;
using System.IO;
using Assets.Editor;
using UnityEditor.Callbacks;
using HlLib.Unity;

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
        InputScriptNameWindow.Open(
            targetObject,
            args => OnCreateScript(args));
    }

    [DidReloadScripts]
    private static void OnReloaded()
    {
        using (var info = AssetLoadingInfo.Load())
        {
            LoadNewAsset(info);
        }
        EditorUtility.ClearProgressBar();
    }

    private static void OnCreateScript(CreateScriptArgs args)
    {
        var newScriptPath = args.FullName;

        if (AssetUtil.Exists(newScriptPath))
        {
            var doDelete = EditorUtility.DisplayDialog(
                "エラー",
                string.Format("{0} は既に存在しています。上書きしますか？", newScriptPath),
                "OK", "キャンセル");
            if (doDelete)
            {
                AssetDatabase.DeleteAsset(newScriptPath);
            }
            else
            {
                return;
            }
        }

        var directory = Path.GetDirectoryName(args.FullName);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var templatePath = (string.IsNullOrEmpty(args.NameSpace)) ?
            PathUtil.Combine("Data", "template.cs")
            : PathUtil.Combine("Data", "templateWithNamespace.cs");

        using (var writer = new StreamWriter(newScriptPath))
        using (var reader = new StreamReader(templatePath))
        {
            var source = reader.ReadToEnd();

            var newSource = source.Replace("${ClassName}", Path.GetFileNameWithoutExtension(args.Name));
            if (!string.IsNullOrEmpty(args.NameSpace))
            {
                newSource = newSource.Replace("${NameSpace}", args.NameSpace);
            }

            writer.Write(newSource);
        }

        // リロード後に情報を渡すために、セッションを貼って作成情報をシリアライズしておく
        using (var info = AssetLoadingInfo.Load())
        {
            info.FilePath = newScriptPath;
            info.ObjectName = args.TargetObject.name;
            info.SessionId = Session.Create().Id;
        }

        // 作成したスクリプトを読み込んでリロード
        AssetDatabase.ImportAsset(newScriptPath, ImportAssetOptions.ForceUpdate);

        // とりあえずなんか出しとく
        EditorUtility.DisplayProgressBar("title", "info", 0.3f);
    }

    private static void LoadNewAsset(AssetLoadingInfo info)
    {
        // セッションが貼られていなければ何もしない
        var session = Session.Find(info.SessionId);
        if (session == null)
        {
            return;
        }
        session.Delete();

        if (!File.Exists(info.FilePath))
        {
            return;
        }

        var script = AssetDatabase.LoadAssetAtPath<MonoScript>(info.FilePath);
        if (script == null)
        {
            return;
        }

        var scriptClass = script.GetClass();
        if (scriptClass == null)
        {
            return;
        }

        var gameObject = GameObject.Find(info.ObjectName);
        if (gameObject == null)
        {
            return;
        }

        gameObject.AddComponent(scriptClass);
    }
}
