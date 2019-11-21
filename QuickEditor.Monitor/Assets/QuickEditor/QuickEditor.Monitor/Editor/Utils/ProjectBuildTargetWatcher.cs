#if UNITY_EDITOR

namespace QuickEditor.Monitor
{
    using UnityEditor;
    using UnityEngine;

    [InitializeOnLoad]
    internal sealed partial class ProjectBuildTargetWatcher
    {
        private static string TAG = typeof(ProjectBuildTargetWatcher).Name;
        private const string DefaultPrefix = "> ";
        private const string FORMAT = DefaultPrefix + "<color=black><b>[{0}]</b></color> --> {1}";

        static ProjectBuildTargetWatcher()
        {
            QuickUnityEditorEventsWatcher watcher = QuickUnityEditorEventsWatcher.Observe();
            watcher.BuildTarget.onActiveBuildTargetChanged.AddListener(target =>
            {
                Debug.Log(string.Format(FORMAT, TAG, "Switch Platform Successed, Current Platform : " + target));
            });
        }
    }
}

#endif