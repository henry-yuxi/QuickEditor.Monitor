#if UNITY_EDITOR

namespace QuickEditor.Monitor
{
    using UnityEditor;
    using UnityEngine;

    [InitializeOnLoad]
    internal sealed partial class ProjectEditorViewWatcher
    {
        private static string TAG = typeof(ProjectEditorViewWatcher).Name;
        private const string DefaultPrefix = "> ";
        private const string FORMAT = DefaultPrefix + "<color=black><b>[{0}]</b></color> --> {1}";

        static ProjectEditorViewWatcher()
        {
            QuickUnityEditorEventsWatcher watcher = QuickUnityEditorEventsWatcher.Observe();
            watcher.SceneView.onSceneGUIDelegate.AddListener(onSceneViewGUI);
            watcher.PrefabUtility.onPrefabInstanceUpdated.AddListener(OnPrefabInstanceUpdated);
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
                            Debug.LogError(string.Format(FORMAT, TAG, handleObj.GetType()));
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
                EditorApplication.delayCall = delegate
                {
                    Selection.activeGameObject = go;
                };
            }
        }

        //private static void OnPlayModeStateChanged()
        //{
        //    if (EditorApplication.isPlayingOrWillChangePlaymode)
        //    {
        //        if (!EditorApplication.isPlaying)
        //        {
        //            if (IsInited && mOnWillPlayEvent != null)
        //            {
        //                mOnWillPlayEvent();
        //            }
        //        }
        //        else
        //        {
        //            if (IsInited && mOnBeginPlayEvent != null)
        //            {
        //                mOnBeginPlayEvent();
        //            }
        //        }
        //    }
        //    else
        //    {
        //        if (EditorApplication.isPlaying)
        //        {
        //            if (IsInited && mOnWillStopEvent != null)
        //            {
        //                mOnWillStopEvent();
        //            }
        //        }
        //    }
        //}
    }
}

#endif