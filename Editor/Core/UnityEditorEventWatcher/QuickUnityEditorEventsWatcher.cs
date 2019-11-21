#if UNITY_EDITOR

namespace QuickEditor.Monitor
{
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.SceneManagement;

    public sealed partial class QuickUnityEditorEventsWatcher
    {
        internal static List<QuickUnityEditorEventsWatcher> allWatchers;

        static QuickUnityEditorEventsWatcher()
        {
            allWatchers = new List<QuickUnityEditorEventsWatcher>();
        }

        private QuickUnityEditorEventsWatcher()
        {
        }

        ~QuickUnityEditorEventsWatcher()
        {
            QuickUnityEditorEventsWatcher.RemoveWatcher(this);
        }

        #region API

        public readonly BuildPipelineEvents BuildPipeline = new BuildPipelineEvents();
        public readonly BuildTargetEvents BuildTarget = new BuildTargetEvents();
        public readonly EditorApplicationEvents EditorApplication = new EditorApplicationEvents();
        public readonly SceneViewEvents SceneView = new SceneViewEvents();
        public readonly PrefabUtilityEvents PrefabUtility = new PrefabUtilityEvents();
        public readonly ProjectViewEvents ProjectView = new ProjectViewEvents();
        public readonly HierarchyViewEvents HierarchyView = new HierarchyViewEvents();

        public static QuickUnityEditorEventsWatcher Observe()
        {
            QuickUnityEditorEventsWatcher w = new QuickUnityEditorEventsWatcher();
            allWatchers.Add(w);
            return w;
        }

        public void Disable()
        {
            allWatchers.Remove(this);
        }

        private static void RemoveWatcher(QuickUnityEditorEventsWatcher watcher)
        {
            allWatchers.Remove(watcher);
        }

        #endregion API

        #region 事件相关定义

        public partial class BuildPipelineEvents
        {
            public class ProcessSceneEvent : UnityEvent<Scene> { }

            public class PreprocessBuildEvent : UnityEvent<BuildTarget, string> { }

            public class PostprocessBuildEvent : UnityEvent<BuildTarget, string> { }

            public readonly ProcessSceneEvent onProcessScene = new ProcessSceneEvent();
            public readonly PreprocessBuildEvent onPreprocessBuild = new PreprocessBuildEvent();
            public readonly PostprocessBuildEvent onPostprocessBuild = new PostprocessBuildEvent();

            internal void InvokeProcessScene(Scene scene, ProcessSceneEvent e)
            {
                if (e == null) { return; }
                e.Invoke(scene);
            }

            internal void InvokePreprocessBuild(BuildTarget target, string path, PreprocessBuildEvent e)
            {
                if (e == null) { return; }
                e.Invoke(target, path);
            }

            internal void InvokePostprocessBuild(BuildTarget target, string path, PostprocessBuildEvent e)
            {
                if (e == null) { return; }
                e.Invoke(target, path);
            }
        }

        public partial class BuildTargetEvents
        {
            public class ActiveBuildTargetChangedEvent : UnityEvent<BuildTarget> { }

            public readonly ActiveBuildTargetChangedEvent onActiveBuildTargetChanged = new ActiveBuildTargetChangedEvent();

            internal void InvokeActiveBuildTargetChanged(BuildTarget activeBuildTarget, ActiveBuildTargetChangedEvent e)
            {
                if (e == null) { return; }
                e.Invoke(activeBuildTarget);
            }
        }

        public partial class EditorApplicationEvents
        {
            public class GlobalEvent : UnityEvent<Event> { }

            public class SearchChangedEvent : UnityEvent { }

            public class ModifierKeysChangedEvent : UnityEvent { }

            public class UpdateEvent : UnityEvent { }

            public class DelayCallEvent : UnityEvent { }

            public class ContextualPropertyMenuEvent : UnityEvent<GenericMenu, SerializedProperty> { }

            public readonly GlobalEvent onGlobal = new GlobalEvent();
            public readonly SearchChangedEvent onSearchChanged = new SearchChangedEvent();
            public readonly ModifierKeysChangedEvent onModifierKeysChanged = new ModifierKeysChangedEvent();

            public readonly UpdateEvent onUpdate = new UpdateEvent();
            public readonly DelayCallEvent onDelayCall = new DelayCallEvent();

            public readonly ContextualPropertyMenuEvent onContextualPropertyMenu = new ContextualPropertyMenuEvent();

            internal void InvokeGlobalEvent(Event current, GlobalEvent e)
            {
                if (e == null) { return; }
                e.Invoke(current);
            }

            internal void InvokeSearchChanged(SearchChangedEvent e)
            {
                if (e == null) { return; }
                e.Invoke();
            }

            internal void InvokeModifierKeysChanged(ModifierKeysChangedEvent e)
            {
                if (e == null) { return; }
                e.Invoke();
            }

            internal void InvokeUpdate(UpdateEvent e)
            {
                if (e == null) { return; }
                e.Invoke();
            }

            internal void InvokeDelayCall(DelayCallEvent e)
            {
                if (e == null) { return; }
                e.Invoke();
            }

            internal void InvokeContextualPropertyMenu(GenericMenu menu, SerializedProperty property, ContextualPropertyMenuEvent e)
            {
                if (e == null) { return; }
                e.Invoke(menu, property);
            }
        }

        public partial class SceneViewEvents
        {
            public class SceneGUIDelegateEvent : UnityEvent<SceneView> { }

            public readonly SceneGUIDelegateEvent onSceneGUIDelegate = new SceneGUIDelegateEvent();

            internal void InvokeSceneGUIDelegate(SceneView sceneview, SceneGUIDelegateEvent e)
            {
                if (e == null) { return; }
                e.Invoke(sceneview);
            }
        }

        public partial class PrefabUtilityEvents
        {
            public class PrefabInstanceUpdatedEvent : UnityEvent<GameObject> { }

            public readonly PrefabInstanceUpdatedEvent onPrefabInstanceUpdated = new PrefabInstanceUpdatedEvent();

            internal void InvokePrefabInstanceUpdated(GameObject instance, PrefabInstanceUpdatedEvent e)
            {
                if (e == null) { return; }
                e.Invoke(instance);
            }
        }

        public partial class ProjectViewEvents
        {
            public class ProjectChangedEvent : UnityEvent { }

            public class ProjectWindowItemOnGUIEvent : UnityEvent<string, Rect> { }

            public readonly ProjectChangedEvent onProjectChanged = new ProjectChangedEvent();
            public readonly ProjectWindowItemOnGUIEvent onProjectWindowItemOnGUI = new ProjectWindowItemOnGUIEvent();

            internal void InvokeProjectChanged(ProjectChangedEvent e)
            {
                if (e == null) { return; }
                e.Invoke();
            }

            internal void InvokeProjectWindowItemOnGUI(string guid, Rect selectionRect, ProjectWindowItemOnGUIEvent e)
            {
                if (e == null) { return; }
                e.Invoke(guid, selectionRect);
            }
        }

        public partial class HierarchyViewEvents
        {
            public class HierarchyChangedEvent : UnityEvent { }

            public class HierarchyWindowItemOnGUIEvent : UnityEvent<int, Rect> { }

            public readonly HierarchyChangedEvent onHierarchyChanged = new HierarchyChangedEvent();
            public readonly HierarchyWindowItemOnGUIEvent onHierarchyWindowItemOnGUI = new HierarchyWindowItemOnGUIEvent();

            internal void InvokeHierarchyChanged(HierarchyChangedEvent e)
            {
                if (e == null) { return; }
                e.Invoke();
            }

            internal void InvokeHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect, HierarchyWindowItemOnGUIEvent e)
            {
                if (e == null) { return; }
                e.Invoke(instanceID, selectionRect);
            }
        }

        //public enum PlayModeState
        //{
        //    EnteredEditMode,
        //    ExitingEditMode,
        //    EnteredPlayMode,
        //    ExitingPlayMode,
        //    Playing,
        //    Paused,
        //    UnPaused,
        //    Quit,
        //    PlayingOrWillChangePlaymode
        //}

        //public class PlayModeStateChangedEvent : UnityEvent<PlayModeState> { }
        //public readonly PlayModeStateChangedEvent onPlayModeStateChanged = new PlayModeStateChangedEvent();

        //public readonly PlayModeStateChangedEvent onWillPlay = new PlayModeStateChangedEvent();
        //public readonly PlayModeStateChangedEvent onBeginPlay = new PlayModeStateChangedEvent();
        //public readonly PlayModeStateChangedEvent onWillStop = new PlayModeStateChangedEvent();
        //internal void InvokePlayModeStateChanged(PlayModeState playModeState, PlayModeStateChangedEvent e)
        //{
        //    if (e == null) { return; }
        //    e.Invoke(playModeState);
        //}

        #endregion 事件相关定义
    }
}

#endif