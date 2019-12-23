#if UNITY_EDITOR

namespace QuickEditor.Monitor
{
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    [InitializeOnLoad]
    internal sealed partial class QuickUnityEditorEventsProcessor
    {
        static QuickUnityEditorEventsProcessor()
        {
            EditorApplicationEventsProcessor.Process();
            HierarchyViewEventsProcessor.Process();
            PrefabUtilityEventsProcessor.Process();
            ProjectViewEventsProcessor.Process();
            SceneViewEventsProcessor.Process();

            //if (IsInited && mOnLockingAssembly != null)
            //{
            //    EditorApplication.LockReloadAssemblies();
            //    mOnLockingAssembly();
            //    EditorApplication.UnlockReloadAssemblies();
            //}
        }

        #region EditorApplication Events Processor

        internal sealed partial class EditorApplicationEventsProcessor
        {
            private static QuickUnityEditorEventsWatcher.PlayModeState mCurState = QuickUnityEditorEventsWatcher.PlayModeState.Stopped;
            private static bool mIgnore = false;

            public static void Process()
            {
#if UNITY_4 || UNITY_5 || UNITY_2017 || UNITY_2018 || UNITY_2019
                EditorApplication.delayCall -= onDelayCall;
                EditorApplication.delayCall += onDelayCall;
#endif
                EditorApplication.update -= onUpdate;
                EditorApplication.update += onUpdate;

                // globalEventHandler
                EditorApplication.CallbackFunction function = () => onGlobalEventHandler(Event.current);
                FieldInfo info = typeof(EditorApplication).GetField("globalEventHandler", BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic);
                EditorApplication.CallbackFunction functions = (EditorApplication.CallbackFunction)info.GetValue(null);
                functions += function;
                info.SetValue(null, (object)functions);

                EditorApplication.contextualPropertyMenu -= onContextualPropertyMenu;
                EditorApplication.contextualPropertyMenu += onContextualPropertyMenu;

                EditorApplication.modifierKeysChanged -= onModifierKeysChanged;
                EditorApplication.modifierKeysChanged += onModifierKeysChanged;

                EditorApplication.searchChanged -= onSearchChanged;
                EditorApplication.searchChanged += onSearchChanged;

#if UNITY_2017_2_OR_NEWER
                EditorApplication.playModeStateChanged -= onPlayModeStateChanged;
                EditorApplication.playModeStateChanged += onPlayModeStateChanged;
#else
                EditorApplication.playmodeStateChanged -= onPlayModeStateChanged;
                EditorApplication.playmodeStateChanged += onPlayModeStateChanged;
#endif
            }

            private static void onPlayModeStateChanged()
            {
                var changedState = QuickUnityEditorEventsWatcher.PlayModeState.None;
                switch (mCurState)
                {
                    case QuickUnityEditorEventsWatcher.PlayModeState.Stopped:
                        if (EditorApplication.isPlayingOrWillChangePlaymode)
                        {
                            if (EditorApplication.isPlaying)
                            {
                                changedState = QuickUnityEditorEventsWatcher.PlayModeState.Playing;
                            }
                            else
                            {
                                mIgnore = true;
                            }
                        }

                        break;

                    case QuickUnityEditorEventsWatcher.PlayModeState.Playing:
                        if (EditorApplication.isPaused)
                        {
                            changedState = QuickUnityEditorEventsWatcher.PlayModeState.Paused;
                        }
                        else
                        {
                            changedState = QuickUnityEditorEventsWatcher.PlayModeState.Stopped;
                        }
                        break;

                    case QuickUnityEditorEventsWatcher.PlayModeState.Paused:
                        if (EditorApplication.isPlayingOrWillChangePlaymode)
                        {
                            changedState = QuickUnityEditorEventsWatcher.PlayModeState.Playing;
                        }
                        else
                        {
                            changedState = QuickUnityEditorEventsWatcher.PlayModeState.Stopped;
                        }
                        break;
                }
                if (mIgnore || changedState == QuickUnityEditorEventsWatcher.PlayModeState.None) { return; }
                foreach (QuickUnityEditorEventsWatcher w in QuickUnityEditorEventsWatcher.allWatchers)
                {
                    if (w == null) { continue; }
                    w.EditorApplication.InvokePlayModeStateChanged(changedState, w.EditorApplication.OnPlayModeStateChanged);
                }
                mCurState = changedState;
                mIgnore = false;
            }

            private static void onPlayModeStateChanged(PlayModeStateChange playMode)
            {
                var changedState = QuickUnityEditorEventsWatcher.PlayModeState.None;
                switch (playMode)
                {
                    case PlayModeStateChange.EnteredEditMode:
                        changedState = QuickUnityEditorEventsWatcher.PlayModeState.Stopped;
                        break;

                    case PlayModeStateChange.ExitingEditMode:
                        break;

                    case PlayModeStateChange.EnteredPlayMode:
                        changedState = QuickUnityEditorEventsWatcher.PlayModeState.Playing;
                        break;

                    case PlayModeStateChange.ExitingPlayMode:
                        break;

                    default:
                        break;
                }
                //switch (mCurState)
                //{
                //    case QuickUnityEditorEventsWatcher.PlayModeState.Playing:
                //        if (EditorApplication.isPaused)
                //        {
                //            changedState = QuickUnityEditorEventsWatcher.PlayModeState.Paused;
                //        }
                //        else
                //        {
                //            changedState = QuickUnityEditorEventsWatcher.PlayModeState.Stopped;
                //        }
                //        break;
                //}
                if (mIgnore || changedState == QuickUnityEditorEventsWatcher.PlayModeState.None) { return; }
                foreach (QuickUnityEditorEventsWatcher w in QuickUnityEditorEventsWatcher.allWatchers)
                {
                    if (w == null) { continue; }
                    w.EditorApplication.InvokePlayModeStateChanged(changedState, w.EditorApplication.OnPlayModeStateChanged);
                }
                mCurState = changedState;
            }

            private static void onDelayCall()
            {
                foreach (QuickUnityEditorEventsWatcher w in QuickUnityEditorEventsWatcher.allWatchers)
                {
                    if (w == null || w.EditorApplication == null) { continue; }
                    w.EditorApplication.InvokeDelayCall(w.EditorApplication.OnDelayCall);
                }
            }

            private static void onUpdate()
            {
                foreach (QuickUnityEditorEventsWatcher w in QuickUnityEditorEventsWatcher.allWatchers)
                {
                    if (w == null || w.EditorApplication == null) { continue; }
                    w.EditorApplication.InvokeUpdate(w.EditorApplication.OnUpdate);
                }
            }

            private static void onGlobalEventHandler(Event current)
            {
                foreach (QuickUnityEditorEventsWatcher w in QuickUnityEditorEventsWatcher.allWatchers)
                {
                    if (w == null || w.EditorApplication == null) { continue; }
                    w.EditorApplication.InvokeGlobalEvent(current, w.EditorApplication.OnGlobal);
                }
            }

            private static void onContextualPropertyMenu(GenericMenu menu, SerializedProperty property)
            {
                foreach (QuickUnityEditorEventsWatcher w in QuickUnityEditorEventsWatcher.allWatchers)
                {
                    if (w == null || w.EditorApplication == null) { continue; }
                    w.EditorApplication.InvokeContextualPropertyMenu(menu, property, w.EditorApplication.OnContextualPropertyMenu);
                }
            }

            private static void onModifierKeysChanged()
            {
                foreach (QuickUnityEditorEventsWatcher w in QuickUnityEditorEventsWatcher.allWatchers)
                {
                    if (w == null || w.EditorApplication == null) { continue; }
                    w.EditorApplication.InvokeModifierKeysChanged(w.EditorApplication.OnModifierKeysChanged);
                }
            }

            private static void onSearchChanged()
            {
                foreach (QuickUnityEditorEventsWatcher w in QuickUnityEditorEventsWatcher.allWatchers)
                {
                    if (w == null || w.EditorApplication == null) { continue; }
                    w.EditorApplication.InvokeSearchChanged(w.EditorApplication.OnSearchChanged);
                }
            }
        }

        #endregion EditorApplication Events Processor

        #region HierarchyView Events Processor

        internal sealed partial class HierarchyViewEventsProcessor
        {
            public static void Process()
            {
#if UNITY_2017_1_OR_NEWER
                EditorApplication.hierarchyChanged -= onHierarchyChanged;
                EditorApplication.hierarchyChanged += onHierarchyChanged;
#endif
                EditorApplication.hierarchyWindowItemOnGUI -= onHierarchyWindowItemOnGUI;
                EditorApplication.hierarchyWindowItemOnGUI += onHierarchyWindowItemOnGUI;
            }

            private static void onHierarchyChanged()
            {
                foreach (QuickUnityEditorEventsWatcher w in QuickUnityEditorEventsWatcher.allWatchers)
                {
                    if (w == null || w.HierarchyView == null) { continue; }
                    w.HierarchyView.InvokeHierarchyChanged(w.HierarchyView.OnHierarchyChanged);
                }
            }

            private static void onHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
            {
                foreach (QuickUnityEditorEventsWatcher w in QuickUnityEditorEventsWatcher.allWatchers)
                {
                    if (w == null || w.HierarchyView == null) { continue; }
                    w.HierarchyView.InvokeHierarchyWindowItemOnGUI(instanceID, selectionRect, w.HierarchyView.OnHierarchyWindowItemOnGUI);
                }
            }
        }

        #endregion HierarchyView Events Processor

        #region PrefabUtility Events Processor

        internal sealed partial class PrefabUtilityEventsProcessor
        {
            public static void Process()
            {
                PrefabUtility.prefabInstanceUpdated -= onPrefabInstanceUpdated;
                PrefabUtility.prefabInstanceUpdated += onPrefabInstanceUpdated;
            }

            private static void onPrefabInstanceUpdated(GameObject instance)
            {
                foreach (QuickUnityEditorEventsWatcher w in QuickUnityEditorEventsWatcher.allWatchers)
                {
                    if (w == null || w.PrefabUtility == null) { continue; }
                    w.PrefabUtility.InvokePrefabInstanceUpdated(instance, w.PrefabUtility.OnPrefabInstanceUpdated);
                }
            }
        }

        #endregion PrefabUtility Events Processor

        #region ProjectView Events Processor

        internal sealed partial class ProjectViewEventsProcessor
        {
            public static void Process()
            {
#if UNITY_2017_1_OR_NEWER
                EditorApplication.projectChanged -= onProjectChanged;
                EditorApplication.projectChanged += onProjectChanged;
#endif

                EditorApplication.projectWindowItemOnGUI -= onProjectWindowItemOnGUI;
                EditorApplication.projectWindowItemOnGUI += onProjectWindowItemOnGUI;
            }

            private static void onProjectChanged()
            {
                foreach (QuickUnityEditorEventsWatcher w in QuickUnityEditorEventsWatcher.allWatchers)
                {
                    if (w == null || w.ProjectView == null) { continue; }
                    w.ProjectView.InvokeProjectChanged(w.ProjectView.OnProjectChanged);
                }
            }

            private static void onProjectWindowItemOnGUI(string guid, Rect selectionRect)
            {
                foreach (QuickUnityEditorEventsWatcher w in QuickUnityEditorEventsWatcher.allWatchers)
                {
                    if (w == null || w.ProjectView == null) { continue; }
                    w.ProjectView.InvokeProjectWindowItemOnGUI(guid, selectionRect, w.ProjectView.OnProjectWindowItemOnGUI);
                }
            }
        }

        #endregion ProjectView Events Processor

        #region SceneView Events Processor

        internal sealed partial class SceneViewEventsProcessor
        {
            public static void Process()
            {
#if UNITY_2019_1_OR_NEWER
                SceneView.duringSceneGui -= onSceneGUIDelegate;
                SceneView.duringSceneGui += onSceneGUIDelegate;
#else
                SceneView.onSceneGUIDelegate -= onSceneGUIDelegate;
                SceneView.onSceneGUIDelegate += onSceneGUIDelegate;
#endif
            }

            private static void onSceneGUIDelegate(SceneView sceneView)
            {
                foreach (QuickUnityEditorEventsWatcher w in QuickUnityEditorEventsWatcher.allWatchers)
                {
                    if (w == null || w.SceneView == null) { continue; }
                    w.SceneView.InvokeSceneGUIDelegate(sceneView, w.SceneView.OnSceneGUIDelegate);
                }
            }
        }

        #endregion SceneView Events Processor

        #region BuildTarget Events Processor

#if UNITY_2017_1_OR_NEWER

        internal sealed partial class BuildTargetEventsProcessor : UnityEditor.Build.IActiveBuildTargetChanged
        {
            public int callbackOrder
            {
                get { return 0; }
            }

            public void OnActiveBuildTargetChanged(BuildTarget previousTarget, BuildTarget newTarget)
            {
                BuildTargetExtension.Process(newTarget);
            }
        }

        internal sealed partial class BuildTargetExtension
        {
            public static void Process(BuildTarget activeBuildTarget)
            {
                foreach (QuickUnityEditorEventsWatcher w in QuickUnityEditorEventsWatcher.allWatchers)
                {
                    if (w == null || w.BuildTarget == null) { continue; }
                    w.BuildTarget.InvokeActiveBuildTargetChanged(activeBuildTarget, w.BuildTarget.OnBuildTargetChanged);
                }
            }
        }

#endif

        #endregion BuildTarget Events Processor

        #region Build Pipeline Events Processor

#if UNITY_2018_1_OR_NEWER

        internal sealed partial class BuildPipelineEventsProcessor : UnityEditor.Build.IProcessSceneWithReport, UnityEditor.Build.IPreprocessBuildWithReport, UnityEditor.Build.IPostprocessBuildWithReport
        {
            public int callbackOrder
            {
                get { return 0; }
            }

            void UnityEditor.Build.IProcessSceneWithReport.OnProcessScene(Scene scene, UnityEditor.Build.Reporting.BuildReport report)
            {
                BuildPipelineExtension.ProcessScene(scene);
            }

            void UnityEditor.Build.IPreprocessBuildWithReport.OnPreprocessBuild(UnityEditor.Build.Reporting.BuildReport report)
            {
                if (report == null) { return; }
                BuildPipelineExtension.PreprocessBuild(report.summary.platform, report.summary.outputPath);
            }

            void UnityEditor.Build.IPostprocessBuildWithReport.OnPostprocessBuild(UnityEditor.Build.Reporting.BuildReport report)
            {
                if (report == null) { return; }
                BuildPipelineExtension.PostprocessBuild(report.summary.platform, report.summary.outputPath);
            }
        }

#elif UNITY_5_6 || UNITY_2017

        internal sealed partial class BuildPipelineEventsProcessor : UnityEditor.Build.IProcessScene, UnityEditor.Build.IPreprocessBuild, UnityEditor.Build.IPostprocessBuild
        {
            int UnityEditor.Build.IOrderedCallback.callbackOrder
            {
                get { return 0; }
            }

            void UnityEditor.Build.IProcessScene.OnProcessScene(Scene scene)
            {
                BuildPipelineExtension.ProcessScene(scene);
            }

            void UnityEditor.Build.IPreprocessBuild.OnPreprocessBuild(BuildTarget target, string pathToBuiltProject)
            {
                BuildPipelineExtension.PreprocessBuild(target, pathToBuiltProject);
            }

            void UnityEditor.Build.IPostprocessBuild.OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
            {
                BuildPipelineExtension.PostprocessBuild(target, pathToBuiltProject);
            }
        }

#else

        internal sealed partial class BuildPipelineEventsProcessor
        {
            [UnityEditor.Callbacks.ProcessScene(0)]
            private static void OnProcessScene(Scene scene)
            {
                BuildPipelineExtension.ProcessScene(scene);
            }

            [UnityEditor.Callbacks.PreProcessBuild(0)]
            private static void OnPreprocessBuild(BuildTarget target, string pathToBuiltProject)
            {
                BuildPipelineExtension.PreprocessBuild(target, pathToBuiltProject);
            }

            [UnityEditor.Callbacks.PostProcessBuild(0)]
            private static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
            {
                BuildPipelineExtension.PreprocessBuild(target, pathToBuiltProject);
            }
        }

#endif

        internal sealed partial class BuildPipelineExtension
        {
            public static void ProcessScene(Scene scene)
            {
                foreach (QuickUnityEditorEventsWatcher w in QuickUnityEditorEventsWatcher.allWatchers)
                {
                    if (w == null || w.BuildPipeline == null) { continue; }
                    w.BuildPipeline.InvokeProcessScene(scene, w.BuildPipeline.OnProcessScene);
                }
            }

            public static void PreprocessBuild(BuildTarget target, string path)
            {
                foreach (QuickUnityEditorEventsWatcher w in QuickUnityEditorEventsWatcher.allWatchers)
                {
                    if (w == null || w.BuildPipeline == null) { continue; }
                    w.BuildPipeline.InvokePreprocessBuild(target, path, w.BuildPipeline.OnPreprocessBuild);
                }
            }

            public static void PostprocessBuild(BuildTarget target, string path)
            {
                foreach (QuickUnityEditorEventsWatcher w in QuickUnityEditorEventsWatcher.allWatchers)
                {
                    if (w == null || w.BuildPipeline == null) { continue; }
                    w.BuildPipeline.InvokePostprocessBuild(target, path, w.BuildPipeline.OnPostprocessBuild);
                }
            }
        }

        #endregion Build Pipeline Events Processor
    }
}

#endif