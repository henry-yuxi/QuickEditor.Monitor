#if UNITY_EDITOR

namespace QuickEditor.Monitor
{
    using UnityEditor;
    using Debug = LoggerUtils;

    [InitializeOnLoad]
    internal sealed partial class ProjectAssetsWatcher
    {
        static ProjectAssetsWatcher()
        {
            QuickAssetWatcher watcher = QuickAssetWatcher.Observe();

            watcher.onAssetCreated.AddListener(asset =>
            {
                Debug.Log("<color=cyan>Created</color> asset {0} of type {1}", asset.Name, asset.Type);
            });

            watcher.onAssetDeleted.AddListener(asset =>
            {
                Debug.Log("<color=red>Deleted</color> asset {0} of type {1}", asset.Name, asset.Type);
            });

            watcher.onAssetModified.AddListener(asset =>
            {
                Debug.Log("<color=orange>Modified</color> asset {0} of type {1}", asset.Name, asset.Type);
            });

            watcher.onAssetMoved.AddListener((before, after) =>
            {
                Debug.Log("<color=blue>Moved</color> asset {0} from {1} to {2}", before.Name, before.DirectoryName, after.DirectoryName);
            });

            watcher.onAssetRenamed.AddListener((before, after) =>
            {
                Debug.Log("<color=magenta>Renamed</color> asset from {0} to {1}", before.Name, after.Name);
            });
        }
    }
}

#endif