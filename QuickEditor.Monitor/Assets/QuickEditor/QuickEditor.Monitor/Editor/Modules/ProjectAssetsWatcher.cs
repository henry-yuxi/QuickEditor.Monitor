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

            watcher.OnAssetCreated.AddListener(asset =>
            {
                Debug.Log("<color=cyan>Created</color> asset {0} of type {1}", asset.Name, asset.Type);
            });

            watcher.OnAssetDeleted.AddListener(asset =>
            {
                Debug.Log("<color=red>Deleted</color> asset {0} of type {1}", asset.Name, asset.Type);
            });

            watcher.OnAssetModified.AddListener(asset =>
            {
                Debug.Log("<color=orange>Modified</color> asset {0} of type {1}", asset.Name, asset.Type);
            });

            watcher.OnAssetMoved.AddListener((before, after) =>
            {
                Debug.Log("<color=blue>Moved</color> asset {0} from {1} to {2}", before.Name, before.DirectoryName, after.DirectoryName);
            });

            watcher.OnAssetRenamed.AddListener((before, after) =>
            {
                Debug.Log("<color=magenta>Renamed</color> asset from {0} to {1}", before.Name, after.Name);
            });
        }
    }
}

#endif