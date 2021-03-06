﻿#if UNITY_EDITOR

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

        public readonly QuickSDKBuildPipelineEvents QuickSDKBuildPipeline = new QuickSDKBuildPipelineEvents();
        public readonly BuildPipelineEvents BuildPipeline = new BuildPipelineEvents();
        public readonly BuildTargetEvents BuildTarget = new BuildTargetEvents();
        public readonly EditorApplicationEvents EditorApplication = new EditorApplicationEvents();
        public readonly HierarchyViewEvents HierarchyView = new HierarchyViewEvents();
        public readonly PrefabUtilityEvents PrefabUtility = new PrefabUtilityEvents();
        public readonly ProjectViewEvents ProjectView = new ProjectViewEvents();
        public readonly SceneViewEvents SceneView = new SceneViewEvents();

        #endregion API

        #region 事件相关定义

        public partial class QuickSDKBuildPipelineEvents
        {
            public class QuickSDKBuildBuildEvent : UnityEvent<BuildTarget, string> { }

            public readonly QuickSDKBuildBuildEvent OnPreApply = new QuickSDKBuildBuildEvent();
            public readonly QuickSDKBuildBuildEvent OnApplySDK = new QuickSDKBuildBuildEvent();
            public readonly QuickSDKBuildBuildEvent OnPreBuild = new QuickSDKBuildBuildEvent();

            //public readonly QuickSDKBuildBuildEvent OnBuildPlayer = new QuickSDKBuildBuildEvent();
            //public readonly QuickSDKBuildBuildEvent OnPreFinalPack = new QuickSDKBuildBuildEvent();
            public readonly QuickSDKBuildBuildEvent OnPostBuild = new QuickSDKBuildBuildEvent();

            internal void InvokePreApply(BuildTarget target, string path, QuickSDKBuildBuildEvent e)
            {
                if (e == null) { return; }
                e.Invoke(target, path);
            }

            internal void InvokeApplySDK(BuildTarget target, string path, QuickSDKBuildBuildEvent e)
            {
                if (e == null) { return; }
                e.Invoke(target, path);
            }

            internal void InvokePreBuild(BuildTarget target, string path, QuickSDKBuildBuildEvent e)
            {
                if (e == null) { return; }
                e.Invoke(target, path);
            }

            internal void InvokeBuildPlayer(BuildTarget target, string path, QuickSDKBuildBuildEvent e)
            {
                if (e == null) { return; }
                e.Invoke(target, path);
            }

            internal void InvokePreFinalPack(BuildTarget target, string path, QuickSDKBuildBuildEvent e)
            {
                if (e == null) { return; }
                e.Invoke(target, path);
            }

            internal void InvokePostBuild(BuildTarget target, string path, QuickSDKBuildBuildEvent e)
            {
                if (e == null) { return; }
                e.Invoke(target, path);
            }
        }

        public partial class BuildPipelineEvents
        {
            public class ProcessSceneEvent : UnityEvent<Scene> { }

            public class PreprocessBuildEvent : UnityEvent<BuildTarget, string> { }

            public class PostprocessBuildEvent : UnityEvent<BuildTarget, string> { }

            public readonly ProcessSceneEvent OnProcessScene = new ProcessSceneEvent();
            public readonly PreprocessBuildEvent OnPreprocessBuild = new PreprocessBuildEvent();
            public readonly PostprocessBuildEvent OnPostprocessBuild = new PostprocessBuildEvent();

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

            public readonly ActiveBuildTargetChangedEvent OnBuildTargetChanged = new ActiveBuildTargetChangedEvent();

            internal void InvokeActiveBuildTargetChanged(BuildTarget activeBuildTarget, ActiveBuildTargetChangedEvent e)
            {
                if (e == null) { return; }
                e.Invoke(activeBuildTarget);
            }
        }

        public partial class EditorApplicationEvents
        {
            public class DelayCallEvent : UnityEvent { }

            public class UpdateEvent : UnityEvent { }

            public class GlobalEvent : UnityEvent<Event> { }

            public class ContextualPropertyMenuEvent : UnityEvent<GenericMenu, SerializedProperty> { }

            public class ModifierKeysChangedEvent : UnityEvent { }

            public class SearchChangedEvent : UnityEvent { }

            public class PlayModeStateEvent : UnityEvent<PlayModeState> { }

            //[System.Obsolete("This event is obsolete in UNITY_2020_0_OR_NEWER")]
            public readonly DelayCallEvent OnDelayCall = new DelayCallEvent();

            public readonly UpdateEvent OnUpdate = new UpdateEvent();

            public readonly GlobalEvent OnGlobal = new GlobalEvent();

            public readonly ContextualPropertyMenuEvent OnContextualPropertyMenu = new ContextualPropertyMenuEvent();
            public readonly ModifierKeysChangedEvent OnModifierKeysChanged = new ModifierKeysChangedEvent();
            public readonly SearchChangedEvent OnSearchChanged = new SearchChangedEvent();

            public readonly PlayModeStateEvent OnPlayModeStateChanged = new PlayModeStateEvent();

            internal void InvokeDelayCall(DelayCallEvent e)
            {
                if (e == null) { return; }
                e.Invoke();
            }

            internal void InvokeUpdate(UpdateEvent e)
            {
                if (e == null) { return; }
                e.Invoke();
            }

            internal void InvokeGlobalEvent(Event current, GlobalEvent e)
            {
                if (e == null) { return; }
                e.Invoke(current);
            }

            internal void InvokeContextualPropertyMenu(GenericMenu menu, SerializedProperty property, ContextualPropertyMenuEvent e)
            {
                if (e == null) { return; }
                e.Invoke(menu, property);
            }

            internal void InvokeModifierKeysChanged(ModifierKeysChangedEvent e)
            {
                if (e == null) { return; }
                e.Invoke();
            }

            internal void InvokeSearchChanged(SearchChangedEvent e)
            {
                if (e == null) { return; }
                e.Invoke();
            }

            internal void InvokePlayModeStateChanged(PlayModeState playModeState, PlayModeStateEvent e)
            {
                if (e == null) { return; }
                e.Invoke(playModeState);
            }
        }

        public partial class HierarchyViewEvents
        {
            public class HierarchyChangedEvent : UnityEvent { }

            public class HierarchyWindowItemOnGUIEvent : UnityEvent<int, Rect> { }

            public readonly HierarchyChangedEvent OnHierarchyChanged = new HierarchyChangedEvent();
            public readonly HierarchyWindowItemOnGUIEvent OnHierarchyWindowItemOnGUI = new HierarchyWindowItemOnGUIEvent();

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

        public partial class PrefabUtilityEvents
        {
            public class PrefabInstanceUpdatedEvent : UnityEvent<GameObject> { }

            public readonly PrefabInstanceUpdatedEvent OnPrefabInstanceUpdated = new PrefabInstanceUpdatedEvent();

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

            public readonly ProjectChangedEvent OnProjectChanged = new ProjectChangedEvent();
            public readonly ProjectWindowItemOnGUIEvent OnProjectWindowItemOnGUI = new ProjectWindowItemOnGUIEvent();

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

        public partial class SceneViewEvents
        {
            public class SceneGUIDelegateEvent : UnityEvent<SceneView> { }

            public readonly SceneGUIDelegateEvent OnSceneGUIDelegate = new SceneGUIDelegateEvent();

            internal void InvokeSceneGUIDelegate(SceneView sceneview, SceneGUIDelegateEvent e)
            {
                if (e == null) { return; }
                e.Invoke(sceneview);
            }
        }

        public enum PlayModeStateChange
        {
            //
            // 摘要:
            //     Occurs during the next update of the Editor application if it is in edit mode
            //     and was previously in play mode.
            EnteredEditMode = 0,

            //
            // 摘要:
            //     Occurs when exiting edit mode, before the Editor is in play mode.
            ExitingEditMode = 1,

            //
            // 摘要:
            //     Occurs during the next update of the Editor application if it is in play mode
            //     and was previously in edit mode.
            EnteredPlayMode = 2,

            //
            // 摘要:
            //     Occurs when exiting play mode, before the Editor is in edit mode.
            ExitingPlayMode = 3,

            EnteredPauseMode = 4,

            ExitingPauseMode = 5,
        }

        public enum PlayModeState
        {
            None = -1,
            Stopped,
            Playing,
            Paused,
            PlayingOrWillChangePlayMode
        }

        #endregion 事件相关定义
    }
}

#endif