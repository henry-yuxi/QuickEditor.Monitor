using QuickEditor.Monitor;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class ProjectAssetsWatcher
{
    private const string LOG_TAG = "<color=black><b>[ProjectAssetsWatcher]</b></color>";
    private const string LOG_FORMAT = "{0} --> {1}";

    static ProjectAssetsWatcher()
    {
        QuickUnityEditorEventWatcher editorEventWatcher = QuickUnityEditorEventWatcher.Observe();
        editorEventWatcher.BuildTarget.onActiveBuildTargetChanged.AddListener(target =>
        {
            UnityEngine.Debug.Log("当前平台为: " + target);
        });

        // Observe the entire assets folder for changes
        var watcher = QuickAssetWatcher.Observe();

        watcher.onAssetCreated.AddListener(asset =>
        {
            Debug.Log(string.Format(LOG_FORMAT, LOG_TAG, " <color=cyan>Created</color> asset '" + asset.Name + "' of type " + asset.Type));
        });

        watcher.onAssetDeleted.AddListener(asset =>
        {
            Debug.Log(string.Format(LOG_FORMAT, LOG_TAG, " <color=red>Deleted</color> asset '" + asset.Name + "' of type " + asset.Type));
        });

        watcher.onAssetModified.AddListener(asset =>
        {
            Debug.Log(string.Format(LOG_FORMAT, LOG_TAG, " <color=orange>Modified</color> asset '" + asset.Name + "' of type " + asset.Type));
        });

        watcher.onAssetMoved.AddListener((before, after) =>
        {
            Debug.Log(string.Format(LOG_FORMAT, LOG_TAG, " <color=blue>Moved</color> asset '" + before.Name + "' from '" + before.DirectoryName + "' to '" + after.DirectoryName + "'"));
        });

        watcher.onAssetRenamed.AddListener((before, after) =>
        {
            Debug.Log(string.Format(LOG_FORMAT, LOG_TAG, " <color=magenta>Renamed</color> asset from '" + before.Name + "' to '" + after.Name + "'"));
        });
    }

}