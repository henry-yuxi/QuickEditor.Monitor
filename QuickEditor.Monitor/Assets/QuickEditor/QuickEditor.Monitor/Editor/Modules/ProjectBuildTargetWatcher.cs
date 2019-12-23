#if UNITY_EDITOR

namespace QuickEditor.Monitor
{
    using UnityEditor;
    using Debug = LoggerUtils;

    [InitializeOnLoad]
    internal sealed partial class ProjectBuildTargetWatcher
    {
        static ProjectBuildTargetWatcher()
        {
            QuickUnityEditorEventsWatcher watcher = QuickUnityEditorEventsWatcher.Observe();
            watcher.BuildTarget.OnBuildTargetChanged.AddListener(target =>
            {
                Debug.Log("Switch Platform Successed, Current Platform : {0}", target);
            });
        }
    }
}

#endif