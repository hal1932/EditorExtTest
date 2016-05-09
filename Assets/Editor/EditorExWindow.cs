using UnityEngine;
using System.Collections;
using UnityEditor;

public class EditorExWindow : EditorWindow
{
    [MenuItem("Window/EditorEx")]
    private static void Open()
    {
        var window = GetWindow<EditorExWindow>("EditorEx");
        window.minSize = new Vector2(400, 500);
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("hello world");

        GUILayout.Label("ラベル");

        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("ボタン1", GUILayout.Width(100), GUILayout.Height(50)))
            {
                Debug.Log("ボタン1");
            }
            if (GUILayout.Button("ボタン2", GUILayout.Width(150), GUILayout.Height(75)))
            {
                Debug.Log("ボタン2");
            }
        }
        EditorGUILayout.EndHorizontal();

        if (GUILayout.RepeatButton("repeat button"))
        {
            Debug.Log("repeat button");
        }

        GUILayout.Label("toggle");
        toggle = GUILayout.Toggle(toggle, "toggle");
        //Debug.Log(toggle);

        GUILayout.Label("Toolbar");
        toolbar = GUILayout.Toolbar(toolbar, new[] { "0", "1", "2", "3" });
        //Debug.Log(toolbar);

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.BeginVertical(GUI.skin.box, GUILayout.Width(300));
            {
                EditorGUILayout.LabelField("左側");

                GUILayout.Label("textfield");
                textfield = GUILayout.TextField(textfield);
                //Debug.Log(textfield);

                color = EditorGUILayout.ColorField("colorfield", color);
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                EditorGUILayout.LabelField("右側");

                GUILayout.Label("passwordfield");
                password = GUILayout.PasswordField(password, '*');
                //Debug.Log(password);

                GUILayout.Label("HorizontalScrollbar");
                horizontalScroll = GUILayout.HorizontalScrollbar(horizontalScroll, 10, 0, 100);
                //Debug.Log(horizontalScroll);
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Label("SelectionGrid");
        selectionGrid = GUILayout.SelectionGrid(selectionGrid, new[] { "0", "1", "2", "3" }, 2);
        //Debug.Log(selectionGrid);

        GUILayout.Label("textarea");
        textarea = GUILayout.TextArea(textarea);
        //Debug.Log(textarea);

        leftScrollPos = EditorGUILayout.BeginScrollView(leftScrollPos, GUI.skin.box, GUILayout.Height(200));
        {
            for (var i = 0; i < 30; ++i)
            {
                EditorGUILayout.LabelField(i.ToString());
            }
        }
        EditorGUILayout.EndScrollView();

        GUILayout.Label("VerticalScrollbar");
        verticalScroll = GUILayout.VerticalScrollbar(verticalScroll, 5, 0, 100);
        //Debug.Log(verticalScroll);

        GUILayout.Label("VerticalSlider");
        verticalSlider = GUILayout.VerticalSlider(verticalSlider, 0, 100);
        //Debug.Log(verticalSlider);

        GUILayout.Box("Box");

        GUILayout.Label("ここからSpace");
        GUILayout.Space(50);
        GUILayout.Label("ここまでSpace");

        GUILayout.Label("ここからFlexibleSpace");
        GUILayout.FlexibleSpace();
        GUILayout.Label("ここまでFlexibleSpace");
    }

    private bool toggle = false;
    private string textfield = "textfield";
    private string textarea = "textarea";
    private string password = "password";
    private float horizontalScroll = 0;
    private float verticalScroll = 0;
    private float verticalSlider = 0;
    private int toolbar = 0;
    private int selectionGrid = 0;
    private Color color = Color.black;
    private Vector2 leftScrollPos;
}
