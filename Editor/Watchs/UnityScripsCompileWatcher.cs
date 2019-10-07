#if UNITY_EDITOR
namespace QuickEditor.Monitor
{
    using System;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;

    [InitializeOnLoad]
    internal sealed partial class UnityScripsCompileWatcher
    {
        private const string LOG_TAG = "<color=black><b>[UnityScripsCompileWatcher]</b></color>";
        private const string LOG_FORMAT = "{0} --> {1}";

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
            //QuickUnityEditorEventWatcher projectEditorEventWatcher = QuickUnityEditorEventWatcher.Observe();
            //projectEditorEventWatcher.EditorApplication.onUpdate.AddListener(onUpdate);

            //QuickAssetWatcher watcher = QuickAssetWatcher.Observe();
            //watcher.onAssetCreated.AddListener(onAssetHandle);
            //watcher.onAssetModified.AddListener(onAssetHandle);
        }

        //private static void onUpdate()
        //{
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
        //}

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
            Debug.Log(string.Format(LOG_FORMAT, LOG_TAG, "<color=green>Unity Scripts Compiling Completed.</color>"));
        }
    }
}
#endif