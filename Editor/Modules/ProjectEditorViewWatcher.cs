#if UNITY_EDITOR

namespace QuickEditor.Monitor
{
    using UnityEditor;
    using UnityEngine;
    using Debug = LoggerUtils;

    [InitializeOnLoad]
    internal sealed partial class ProjectEditorViewWatcher
    {
        static ProjectEditorViewWatcher()
        {
            QuickUnityEditorEventsWatcher watcher = QuickUnityEditorEventsWatcher.Observe();
            watcher.SceneView.onSceneGUIDelegate.AddListener(onSceneViewGUI);
            watcher.PrefabUtility.onPrefabInstanceUpdated.AddListener(OnPrefabInstanceUpdated);
            watcher.EditorApplication.onPlayModeStateChanged.AddListener(OnPlayModeStateChanged);
        }

        private static void OnPlayModeStateChanged(QuickUnityEditorEventsWatcher.PlayModeState arg0)
        {
            Debug.Log("Editor PlayModeState: {0}", arg0);
        }

        private static void onSceneViewGUI(SceneView sceneview)
        {
            Event e = Event.current;
            if (e.type == EventType.DragUpdated || e.type == EventType.DragPerform)
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                if (e.type == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();
                    for (int i = 0; i < DragAndDrop.objectReferences.Length; i++)
                    {
                        UnityEngine.Object handleObj = DragAndDrop.objectReferences[i];
                        if (handleObj != null)
                        {
                            //Debug.Log(handleObj.GetType());
                        }
                    }
                }
            }
        }

        private static void OnPrefabInstanceUpdated(GameObject instance)
        {
            GameObject go = null;
            if (Selection.activeTransform)
            {
                go = Selection.activeGameObject;
            }
            AssetDatabase.SaveAssets();
            if (go)
            {
                QuickUnityEditorEventsWatcher.Observe().EditorApplication.onDelayCall.AddListener(delegate
                {
                    Selection.activeGameObject = go;
                });
            }
        }
    }
}

#endif