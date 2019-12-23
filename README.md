# QuickEditor.Monitor

## Getting Started
Take a look at the sample project provided here for an example.

### UnityAssetsWatcher example
```
QuickAssetWatcher watcher = QuickAssetWatcher.Observe();
watcher.OnAssetCreated.AddListener(asset =>
{
});
watcher.OnAssetDeleted.AddListener(asset =>
{
});
watcher.OnAssetModified.AddListener(asset =>
{
});
watcher.OnAssetMoved.AddListener((before, after) =>
{
});
watcher.OnAssetRenamed.AddListener((before, after) =>
{
});
```

### UnityEditorEventWatcher example
```
QuickUnityEditorEventsWatcher watcher = QuickUnityEditorEventsWatcher.Observe();
watcher.EditorApplication.OnUpdate.AddListener(onUpdate);
watcher.SceneView.OnSceneGUIDelegate.AddListener(onSceneViewGUI);
watcher.PrefabUtility.OnPrefabInstanceUpdated.AddListener(OnPrefabInstanceUpdated);
```
            
## How to install?

### UPM Install via manifest.json

In Packages folder, you will see a file named manifest.json. 

using this package add lines into ./Packages/manifest.json like next sample:
```
{
  "dependencies": {
    "com.sourcemuch.quickeditor.monitor": "https://github.com/henry-yuxi/QuickEditor.Monitor.git#0.0.8",
  }
}
```

### Unity 2019.3 Git URL

In Unity 2019.3 or greater, Package Manager is include the new feature that able to install the package via Git.

Open the package manager window (menu: Window > Package Manager), select "Add package from git URL...", fill in this in the pop-up textbox: 
https://github.com/henry-yuxi/QuickEditor.Monitor.git#0.0.8.


### Unity UPM Git Extension (For 2019.2 and older version)

If you doesn't have this package before, please redirect to this git https://github.com/mob-sakai/UpmGitExtension then follow the instruction in README.md to install the UPM Git Extension to your Unity.

If you already installed. Open the Package Manager UI, you will see the git icon around the bottom left connor, Open it then follow the instruction using this git URL to perform package install.

请确保使用的UPM包是最终版本。
      
