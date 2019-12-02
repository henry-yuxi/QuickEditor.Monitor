#if UNITY_EDITOR

namespace QuickEditor.Monitor
{
    using UnityEditor;
    using UnityEngine;

    [InitializeOnLoad]
    internal sealed partial class ProjectAssetsWatcher
    {

        static ProjectAssetsWatcher()
        {
            // Observe the entire assets folder for changes
            var watcher = QuickAssetWatcher.Observe();

            watcher.onAssetCreated.AddListener(asset =>
            {
                Debug.Log(string.Format(Utils.FORMAT, Utils.Assembly, " <color=cyan>Created</color> asset '" + asset.Name + "' of type " + asset.Type));
            });

            watcher.onAssetDeleted.AddListener(asset =>
            {
                Debug.Log(string.Format(Utils.FORMAT, Utils.Assembly, " <color=red>Deleted</color> asset '" + asset.Name + "' of type " + asset.Type));
            });

            watcher.onAssetModified.AddListener(asset =>
            {
                Debug.Log(string.Format(Utils.FORMAT, Utils.Assembly, " <color=orange>Modified</color> asset '" + asset.Name + "' of type " + asset.Type));
            });

            watcher.onAssetMoved.AddListener((before, after) =>
            {
                Debug.Log(string.Format(Utils.FORMAT, Utils.Assembly, " <color=blue>Moved</color> asset '" + before.Name + "' from '" + before.DirectoryName + "' to '" + after.DirectoryName + "'"));
            });

            watcher.onAssetRenamed.AddListener((before, after) =>
            {
                Debug.Log(string.Format(Utils.FORMAT, Utils.Assembly, " <color=magenta>Renamed</color> asset from '" + before.Name + "' to '" + after.Name + "'"));
            });
        }
    }
}

#endif