#if UNITY_EDITOR

namespace QuickEditor.Monitor
{
    using System;
    using UnityEditor;
    using Debug = LoggerUtils;

    [InitializeOnLoad]
    internal sealed partial class UnityScripsCompileWatcher
    {
        public static event Action StartedCompiling = delegate { };

        public static event Action FinishedCompiling = delegate { };

        //public enum CompilerOptions
        //{
        //    ForceStopPlay,
        //    LockReloadAssemblies
        //}

        //private static CompilerOptions mCompiler = CompilerOptions.ForceStopPlay;
        //private static bool mIsLockReloadAssemblies = false;
        //private static bool mIsImportScripts = false;

        static UnityScripsCompileWatcher()
        {
            QuickUnityEditorEventsWatcher mEventWatcher = QuickUnityEditorEventsWatcher.Observe();
            mEventWatcher.EditorApplication.OnUpdate.AddListener(onEditorUpdate);

            //QuickAssetWatcher watcher = QuickAssetWatcher.Observe();
            //watcher.onAssetCreated.AddListener(onAssetHandle);
            //watcher.onAssetModified.AddListener(onAssetHandle);

            UnityScripsCompileTimeTracker.KeyframeAdded += UnityScripsCompileWatcher.LogCompileTimeKeyframe;
        }

        private static bool StoredCompilingState
        {
            get { return EditorPrefs.GetBool("EditorApplicationCompilationUtil::StoredCompilingState"); }
            set { EditorPrefs.SetBool("EditorApplicationCompilationUtil::StoredCompilingState", value); }
        }

        private static void LogCompileTimeKeyframe(UnityScripsCompileTimeKeyframe keyframe)
        {
            string compilationFinishedLog = "Compilation Finished, Elapsed time : " + TrackingUtils.FormatMSTime(keyframe.elapsedCompileTimeInMS);
            if (keyframe.hadErrors)
            {
                compilationFinishedLog += " (error)";
            }
            Debug.Log("<color=green>Unity Scripts Compiling Completed. </color>" + compilationFinishedLog);
        }

        private static void onEditorUpdate()
        {
            if (EditorApplication.isCompiling && UnityScripsCompileWatcher.StoredCompilingState == false)
            {
                UnityScripsCompileWatcher.StoredCompilingState = true;
                UnityScripsCompileWatcher.StartedCompiling.Invoke();
            }

            if (!EditorApplication.isCompiling && UnityScripsCompileWatcher.StoredCompilingState == true)
            {
                UnityScripsCompileWatcher.StoredCompilingState = false;
                UnityScripsCompileWatcher.FinishedCompiling.Invoke();
            }
            //    switch (mCompiler)
            //    {
            //        case CompilerOptions.ForceStopPlay:
            //            mIsImportScripts = Convert.ToBoolean(PlayerPrefs.GetInt("ImportScripts", 1));
            //            if (mIsImportScripts && !EditorApplication.isCompiling)
            //            {
            //                mIsImportScripts = false;
            //                PlayerPrefs.SetInt("ImportScripts", 0);
            //                EditorApplication.update -= onUpdate;
            //            }
            //            CheckComplie();
            //            break;

            //        case CompilerOptions.LockReloadAssemblies:
            //            if (!mIsLockReloadAssemblies && EditorApplication.isCompiling && EditorApplication.isPlaying)
            //            {
            //                mIsLockReloadAssemblies = true;
            //                EditorApplication.LockReloadAssemblies();
            //                EditorApplication.playModeStateChanged += onPlaymodeChanged;
            //            }

            //            break;

            //        default:
            //            break;
            //    }
        }

        //private static void onPlaymodeChanged(PlayModeStateChange playModeState)
        //{
        //    if (!EditorApplication.isPlaying && mIsLockReloadAssemblies)
        //    {
        //        mIsLockReloadAssemblies = false;
        //        EditorApplication.UnlockReloadAssemblies();
        //        EditorApplication.playModeStateChanged -= onPlaymodeChanged;
        //    }
        //}

        //private static void onAssetHandle(QuickAssetFileInfo asset)
        //{
        //    if (asset == null || string.IsNullOrEmpty(asset.AssetsRelativePath)) { return; }
        //    List<string> importedKeys = new List<string>(ComplieRules);

        //    for (int j = 0; j < importedKeys.Count; j++)
        //    {
        //        if (asset.AssetsRelativePath.Contains(importedKeys[j]))
        //        {
        //            PlayerPrefs.SetInt("ImportScripts", 1);
        //            return;
        //        }
        //    }
        //}

        //private static List<string> ComplieRules = new List<string>()
        //{
        //    "Asset/Scripts",
        //    "Editor",
        //};

        //private static void CheckComplie()
        //{
        //    mIsImportScripts = Convert.ToBoolean(PlayerPrefs.GetInt("ImportScripts", 1));
        //    // 检查编译中, 立刻暂停游戏！
        //    if (mIsImportScripts || EditorApplication.isCompiling)
        //    {
        //        if (EditorApplication.isPlaying)
        //        {
        //            Debug.Log(string.Format(LOG_FORMAT, LOG_TAG, "<color=red>Force Stop Play, Because of Compiling.</color>"));
        //            EditorApplication.isPlaying = false;
        //        }
        //    }
        //}

        [UnityEditor.Callbacks.DidReloadScripts]
        private static void OnUnityScripsCompilingCompleted()
        {
            Debug.Log("<color=green>Unity Scripts Compiling Completed.</color>");
        }
    }
}

#endif