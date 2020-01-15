#if UNITY_EDITOR

namespace QuickEditor.Monitor
{
    using UnityEditor;
    using UnityEngine;
    using Debug = LoggerUtils;

    [InitializeOnLoad]
    internal sealed partial class QuickSDKBuildWatcher : MonoBehaviour
    {
        static QuickSDKBuildWatcher()
        {
            QuickUnityEditorEventsWatcher watcher = QuickUnityEditorEventsWatcher.Observe();
            watcher.QuickSDKBuildPipeline.OnPreApply.AddListener((target, path) =>
            {
                Debug.Log("QuickSDKBuildPipeline -> OnPreApply : {0}, {1}", target, path);
            });
            watcher.QuickSDKBuildPipeline.OnApplySDK.AddListener((target, path) =>
            {
                Debug.Log("QuickSDKBuildPipeline -> OnApplySDK : {0}, {1}", target, path);
            });
            watcher.QuickSDKBuildPipeline.OnPreBuild.AddListener((target, path) =>
            {
                Debug.Log("QuickSDKBuildPipeline -> OnPreBuild : {0}, {1}", target, path);
            });
            watcher.QuickSDKBuildPipeline.OnPostBuild.AddListener((target, path) =>
            {
                Debug.Log("QuickSDKBuildPipeline -> OnPostBuild : {0}, {1}", target, path);
            });
        }
    }
}

#endif