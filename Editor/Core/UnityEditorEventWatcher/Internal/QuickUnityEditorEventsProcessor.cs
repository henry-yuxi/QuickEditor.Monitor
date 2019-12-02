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
            EditorPlayModeEventsProcessor.Process();
            SceneViewEventsProcessor.Process();
            PrefabUtilityEventsProcessor.Process();
            ProjectViewEventsProcessor.Process();
            HierarchyViewEventsProcessor.Process();

            //EditorUserBuildSettings.activeBuildTargetChanged -= OnActiveBuildTargetChanged;
            //EditorUserBuildSettings.activeBuildTargetChanged += OnActiveBuildTargetChanged;

            //if (IsInited && mOnLockingAssembly != null)
            //{
            //    EditorApplication.LockReloadAssemblies();
            //    mOnLockingAssembly();
            //    EditorApplication.UnlockReloadAssemblies();
            //}
        }

        internal interface IEventProcessor
        {
        }

        internal sealed partial class EditorApplicationEventsProcessor
        {
            public static void Process()
            {
                EditorApplication.contextualPropertyMenu -= onContextualPropertyMenu;
                EditorApplication.contextualPropertyMenu += onContextualPropertyMenu;

                EditorApplication.modifierKeysChanged -= onModifierKeysChanged;
                EditorApplication.modifierKeysChanged += onModifierKeysChanged;

                EditorApplication.delayCall -= onDelayCall;
                EditorApplication.delayCall += onDelayCall;

                EditorApplication.update -= onUpdate;
                EditorApplication.update += onUpdate;

                EditorApplication.searchChanged -= onSearchChanged;
                EditorApplication.searchChanged += onSearchChanged;

                // globalEventHandler
                EditorApplication.CallbackFunction function = () => onGlobalEventHandler(Event.current);
                FieldInfo info = typeof(EditorApplication).GetField("globalEventHandler", BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic);
                EditorApplication.CallbackFunction functions = (EditorApplication.CallbackFunction)info.GetValue(null);
                functions += function;
                info.SetValue(null, (object)functions);

                //EditorApplication.wantsToQuit -= onQuit;
                //EditorApplication.wantsToQuit += onQuit;
            }

            private static void onGlobalEventHandler(Event current)
            {
                foreach (QuickUnityEditorEventsWatcher w in QuickUnityEditorEventsWatcher.allWatchers)
                {
                    if (w == null || w.EditorApplication == null) { continue; }
                    w.EditorApplication.InvokeGlobalEvent(current, w.EditorApplication.onGlobal);
                }
            }

            private static bool onQuit()
            {
                return false;
            }

            private static void onContextualPropertyMenu(GenericMenu menu, SerializedProperty property)
            {
                foreach (QuickUnityEditorEventsWatcher w in QuickUnityEditorEventsWatcher.allWatchers)
                {
                    if (w == null || w.EditorApplication == null) { continue; }
                    w.EditorApplication.InvokeContextualPropertyMenu(menu, property, w.EditorApplication.onContextualPropertyMenu);
                }
            }

            private static void onModifierKeysChanged()
            {
                foreach (QuickUnityEditorEventsWatcher w in QuickUnityEditorEventsWatcher.allWatchers)
                {
                    if (w == null || w.EditorApplication == null) { continue; }
                    w.EditorApplication.InvokeModifierKeysChanged(w.EditorApplication.onModifierKeysChanged);
                }
            }

            private static void onDelayCall()
            {
                foreach (QuickUnityEditorEventsWatcher w in QuickUnityEditorEventsWatcher.allWatchers)
                {
                    if (w == null || w.EditorApplication == null) { continue; }
                    w.EditorApplication.InvokeDelayCall(w.EditorApplication.onDelayCall);
                }
            }

            private static void onUpdate()
            {
                foreach (QuickUnityEditorEventsWatcher w in QuickUnityEditorEventsWatcher.allWatchers)
                {
                    if (w == null || w.EditorApplication == null) { continue; }
                    w.EditorApplication.InvokeUpdate(w.EditorApplication.onUpdate);
                }
            }

            private static void onSearchChanged()
            {
                foreach (QuickUnityEditorEventsWatcher w in QuickUnityEditorEventsWatcher.allWatchers)
                {
                    if (w == null || w.EditorApplication == null) { continue; }
                    w.EditorApplication.InvokeSearchChanged(w.EditorApplication.onSearchChanged);
                }
            }
        }

        internal sealed partial class EditorPlayModeEventsProcessor
        {
            public static void Process()
            {
#if UNITY_2018_2_OR_NEWER
                //                EditorApplication.playModeStateChanged -= onPlayModeStateChanged;
                //EditorApplication.playModeStateChanged += onPlayModeStateChanged;
#else
                EditorApplication.playmodeStateChanged -= onPlayModeStateChanged;
                EditorApplication.playmodeStateChanged += onPlayModeStateChanged;
#endif
            }

            private static void onPlayModeStateChanged()
            {
            }

            //private static void onPlayModeStateChanged(PlayModeStateChange playMode)
            //{
            //    foreach (QuickUnityEditorEventWatcher w in QuickUnityEditorEventWatcher.allWatchers)
            //    {
            //        if (w == null) { continue; }
            //        w.InvokePlayModeStateChanged(QuickUnityEditorEventWatcher.PlayModeState.Playing, w.onPlayModeStateChanged);
            //    }

            //    if (EditorApplication.isPlayingOrWillChangePlaymode)
            //    {
            //        if (!EditorApplication.isPlaying)
            //        {
            //        }
            //        else
            //        {
            //        }
            //    }
            //    else
            //    {
            //        if (EditorApplication.isPlaying)
            //        {
            //        }
            //    }
            //}
        }

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
                    w.HierarchyView.InvokeHierarchyChanged(w.HierarchyView.onHierarchyChanged);
                }
            }

            private static void onHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
            {
                foreach (QuickUnityEditorEventsWatcher w in QuickUnityEditorEventsWatcher.allWatchers)
                {
                    if (w == null || w.HierarchyView == null) { continue; }
                    w.HierarchyView.InvokeHierarchyWindowItemOnGUI(instanceID, selectionRect, w.HierarchyView.onHierarchyWindowItemOnGUI);
                }
            }
        }

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
                    w.PrefabUtility.InvokePrefabInstanceUpdated(instance, w.PrefabUtility.onPrefabInstanceUpdated);
                }
            }
        }

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
                    w.ProjectView.InvokeProjectChanged(w.ProjectView.onProjectChanged);
                }
            }

            private static void onProjectWindowItemOnGUI(string guid, Rect selectionRect)
            {
                foreach (QuickUnityEditorEventsWatcher w in QuickUnityEditorEventsWatcher.allWatchers)
                {
                    if (w == null || w.ProjectView == null) { continue; }
                    w.ProjectView.InvokeProjectWindowItemOnGUI(guid, selectionRect, w.ProjectView.onProjectWindowItemOnGUI);
                }
            }
        }

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
                    w.SceneView.InvokeSceneGUIDelegate(sceneView, w.SceneView.onSceneGUIDelegate);
                }
            }
        }

        #region BuildTarget Processor

#if UNITY_2018_2_OR_NEWER

        internal sealed partial class BuildTargetChangedProcessor : UnityEditor.Build.IActiveBuildTargetChanged
        {
            public int callbackOrder
            {
                get { return 0; }
            }

            public void OnActiveBuildTargetChanged(BuildTarget previousTarget, BuildTarget newTarget)
            {
                BuildTargetChangedExtension.Process(newTarget);
            }
        }

        internal sealed partial class BuildTargetChangedExtension
        {
            public static void Process(BuildTarget activeBuildTarget)
            {
                foreach (QuickUnityEditorEventsWatcher w in QuickUnityEditorEventsWatcher.allWatchers)
                {
                    if (w == null || w.BuildTarget == null) { continue; }
                    w.BuildTarget.InvokeActiveBuildTargetChanged(activeBuildTarget, w.BuildTarget.onActiveBuildTargetChanged);
                }
            }
        }

#endif

        #endregion BuildTarget Processor

        #region Unity Build Pipeline

#if UNITY_2018_2_OR_NEWER

        internal sealed partial class ProcessSceneProcessor : UnityEditor.Build.IProcessSceneWithReport
        {
            public int callbackOrder
            {
                get { return 0; }
            }

            public void OnProcessScene(Scene scene, UnityEditor.Build.Reporting.BuildReport report)
            {
                BuildPipelineExtension.ProcessScene(scene);
            }
        }

#elif UNITY_5_6 || UNITY_2017

        internal sealed partial class ProcessSceneProcessor : UnityEditor.Build.IProcessScene
        {
            int UnityEditor.Build.IOrderedCallback.callbackOrder
            {
                get { return 0; }
            }

            void UnityEditor.Build.IProcessScene.OnProcessScene(Scene scene)
            {
                BuildPipelineExtension.ProcessScene(scene);
            }
        }

#else

 internal sealed partial class ProcessSceneProcessor
{
    [UnityEditor.Callbacks.ProcessScene(1)]
    private static void OnProcessScene(Scene scene)
    {
        BuildPipelineExtension.ProcessScene(scene);
    }
}

#endif

#if UNITY_2018_2_OR_NEWER

        internal sealed partial class PreprocessBuildProcessor : UnityEditor.Build.IPreprocessBuildWithReport
        {
            public int callbackOrder
            {
                get { return 0; }
            }

            public void OnPreprocessBuild(UnityEditor.Build.Reporting.BuildReport report)
            {
                if (report == null) { return; }
                BuildPipelineExtension.PreprocessBuild(report.summary.platform, report.summary.outputPath);
            }
        }

#elif UNITY_5_6 || UNITY_2017

        internal sealed partial class PreprocessBuildProcessor : UnityEditor.Build.IPreprocessBuild
        {
            int UnityEditor.Build.IOrderedCallback.callbackOrder
            {
                get { return 0; }
            }

            void UnityEditor.Build.IPreprocessBuild.OnPreprocessBuild(BuildTarget target, string pathToBuiltProject)
            {
                BuildPipelineExtension.PreprocessBuild(target, pathToBuiltProject);
            }
        }

#else

 internal sealed partial  class PreprocessBuildProcessor
{
    [UnityEditor.Callbacks.PreProcessBuild(1)]
    private static void OnPreprocessBuild(BuildTarget target, string pathToBuiltProject)
    {
        BuildPipelineExtension.PreprocessBuild(target, path);
    }
}

#endif

#if UNITY_2018_2_OR_NEWER

        internal sealed partial class PostprocessBuildProcessor : UnityEditor.Build.IPostprocessBuildWithReport
        {
            public int callbackOrder
            {
                get { return 0; }
            }

            public void OnPostprocessBuild(UnityEditor.Build.Reporting.BuildReport report)
            {
                if (report == null) { return; }
                BuildPipelineExtension.PostprocessBuild(report.summary.platform, report.summary.outputPath);
            }
        }

#elif UNITY_5_6 || UNITY_2017

        internal sealed partial class PostprocessBuildProcessor : UnityEditor.Build.IPostprocessBuild
        {
            int UnityEditor.Build.IOrderedCallback.callbackOrder
            {
                get { return 0; }
            }

            void UnityEditor.Build.IPostprocessBuild.OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
            {
                BuildPipelineExtension.PostprocessBuild(target, pathToBuiltProject);
            }
        }

#else

 internal sealed partial class PostprocessBuildProcessor
{
    [UnityEditor.Callbacks.PostProcessBuild(1)]
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
                    w.BuildPipeline.InvokeProcessScene(scene, w.BuildPipeline.onProcessScene);
                }
            }

            public static void PreprocessBuild(BuildTarget target, string path)
            {
                foreach (QuickUnityEditorEventsWatcher w in QuickUnityEditorEventsWatcher.allWatchers)
                {
                    if (w == null || w.BuildPipeline == null) { continue; }
                    w.BuildPipeline.InvokePreprocessBuild(target, path, w.BuildPipeline.onPreprocessBuild);
                }
            }

            public static void PostprocessBuild(BuildTarget target, string path)
            {
                foreach (QuickUnityEditorEventsWatcher w in QuickUnityEditorEventsWatcher.allWatchers)
                {
                    if (w == null || w.BuildPipeline == null) { continue; }
                    w.BuildPipeline.InvokePostprocessBuild(target, path, w.BuildPipeline.onPostprocessBuild);
                }
            }
        }

        #endregion Unity Build Pipeline
    }
}

#endif