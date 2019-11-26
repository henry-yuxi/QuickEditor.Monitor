# QuickEditor.Monitor

## Getting Started
Take a look at the sample project provided here for an example.

### UnityAssetsWatcher example
```
QuickAssetWatcher watcher = QuickAssetWatcher.Observe();
watcher.onAssetCreated.AddListener(asset =>
{
});
watcher.onAssetDeleted.AddListener(asset =>
{
});
watcher.onAssetModified.AddListener(asset =>
{
});
watcher.onAssetMoved.AddListener((before, after) =>
{
});
watcher.onAssetRenamed.AddListener((before, after) =>
{
});
```

### UnityEditorEventWatcher example
```
QuickUnityEditorEventWatcher watcher = QuickUnityEditorEventWatcher.Observe();
watcher.EditorApplication.onUpdate.AddListener(onUpdate);
watcher.SceneView.onSceneGUIDelegate.AddListener(onSceneViewGUI);
watcher.PrefabUtility.onPrefabInstanceUpdated.AddListener(OnPrefabInstanceUpdated);

```
            
## Installation
Find Packages/manifest.json in your project and edit it to look like this:
{
  "dependencies": {
    "com.sourcemuch.quickeditor.monitor": "https://github.com/henry-yuxi/QuickEditor.Monitor.git#0.0.6",
    ...
  },
}

Or open the package manager window (menu: Window > Package Manager), select "Add package from git URL...", fill in this in the pop-up textbox: https://github.com/henry-yuxi/QuickEditor.Monitor.git#0.0.6.

## Requirement
Unity 2018.3 or later
Git (executable on command-line)
      
