#if UNITY_EDITOR

namespace QuickEditor.Monitor
{
    using UnityEditor;
    using UnityEngine;

    [InitializeOnLoad]
    internal sealed partial class ProjectAssetsWatcher
    {
        private static string TAG = typeof(ProjectAssetsWatcher).Name;
        private const string DefaultPrefix = "> ";
        private const string FORMAT = DefaultPrefix + "<color=black><b>[{0}]</b></color> --> {1}";

        static ProjectAssetsWatcher()
        {
            // Observe the entire assets folder for changes
            var watcher = QuickAssetWatcher.Observe();

            watcher.onAssetCreated.AddListener(asset =>
            {
                Debug.Log(string.Format(FORMAT, TAG, " <color=cyan>Created</color> asset '" + asset.Name + "' of type " + asset.Type));
            });

            watcher.onAssetDeleted.AddListener(asset =>
            {
                Debug.Log(string.Format(FORMAT, TAG, " <color=red>Deleted</color> asset '" + asset.Name + "' of type " + asset.Type));
            });

            watcher.onAssetModified.AddListener(asset =>
            {
                Debug.Log(string.Format(FORMAT, TAG, " <color=orange>Modified</color> asset '" + asset.Name + "' of type " + asset.Type));
            });

            watcher.onAssetMoved.AddListener((before, after) =>
            {
                Debug.Log(string.Format(FORMAT, TAG, " <color=blue>Moved</color> asset '" + before.Name + "' from '" + before.DirectoryName + "' to '" + after.DirectoryName + "'"));
            });

            watcher.onAssetRenamed.AddListener((before, after) =>
            {
                Debug.Log(string.Format(FORMAT, TAG, " <color=magenta>Renamed</color> asset from '" + before.Name + "' to '" + after.Name + "'"));
            });
        }
    }
}

#endif