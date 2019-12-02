#if UNITY_EDITOR

namespace QuickEditor.Monitor
{
    using UnityEditor;
    using UnityEngine;

    [InitializeOnLoad]
    internal sealed partial class ProjectBuildTargetWatcher
    {
        static ProjectBuildTargetWatcher()
        {
            QuickUnityEditorEventsWatcher watcher = QuickUnityEditorEventsWatcher.Observe();
            watcher.BuildTarget.onActiveBuildTargetChanged.AddListener(target =>
            {
                Debug.Log(string.Format(Utils.FORMAT, Utils.Assembly, "Switch Platform Successed, Current Platform : " + target));
            });
        }
    }
}

#endif