#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class DebugPanelWindow : EditorWindow
{
    [MenuItem("Window/UI Toolkit/MyEditorWindow")]
    public static void ShowDebugPanel()
    {
        DebugPanelWindow wnd = GetWindow<DebugPanelWindow>();
        wnd.titleContent = new GUIContent("MyEditorWindow");
    }

    public void CreateGUI()
    {
        //// Each editor window contains a root VisualElement object
        //VisualElement root = rootVisualElement;

        //// VisualElements objects can contain other VisualElement following a tree hierarchy
        //Label label = new Label("Hello World!");
        //root.Add(label);

        //// Create button
        //Button button = new Button();
        //button.name = "button";
        //button.text = "Button";
        //root.Add(button);

        //// Create toggle
        //Toggle toggle = new Toggle();
        //toggle.name = "toggle";
        //toggle.label = "Toggle";
        //root.Add(toggle);
    }

    public void OnGUI()
    {
        if (GUILayout.Button("Loading³¡¾°"))
        {
            EditorSceneManager.OpenScene("Assets/Scenes/" + "StartGame.unity", OpenSceneMode.Single);
        }

        if (GUILayout.Button("Ö÷³¡¾°Scene1"))
        {
            EditorSceneManager.OpenScene("Assets/Scenes/" + "TestCDNScenes.unity", OpenSceneMode.Single);
        }
    }
}
#endif