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
QuickUnityEditorEventsWatcher watcher = QuickUnityEditorEventsWatcher.Observe();
watcher.EditorApplication.onUpdate.AddListener(onUpdate);
watcher.SceneView.onSceneGUIDelegate.AddListener(onSceneViewGUI);
watcher.PrefabUtility.onPrefabInstanceUpdated.AddListener(OnPrefabInstanceUpdated);
```
            
## How to install?

### UPM Install via manifest.json

Locate to your Unity Project. In Packages folder, you will see a file named manifest.json. Open it with your text editor (such as Notepad++ or Visual Studio Code or Legacy Notepad)

Then merge this json format below.

(Do not just copy & paste the whole json! If you are not capable to merge json, please using online JSON merge tools like this)
```
{
  "dependencies": {
    "com.sourcemuch.quickeditor.monitor": "https://github.com/henry-yuxi/QuickEditor.Monitor.git#0.0.6",
    ...
  }
}
```

If you want to install the older version, please take a look at release tag in this git, then change the path after # to the version tag that you want.

### Unity 2019.3 Git URL

In Unity 2019.3 or greater, Package Manager is include the new feature that able to install the package via Git.

Open the package manager window (menu: Window > Package Manager), select "Add package from git URL...", fill in this in the pop-up textbox: 
https://github.com/henry-yuxi/QuickEditor.Monitor.git#0.0.6.

Make sure that you're select the latest version.

### Unity UPM Git Extension (For 2019.2 and older version)

If you doesn't have this package before, please redirect to this git https://github.com/mob-sakai/UpmGitExtension then follow the instruction in README.md to install the UPM Git Extension to your Unity.

If you already installed. Open the Package Manager UI, you will see the git icon around the bottom left connor, Open it then follow the instruction using this git URL to perform package install.

Make sure that you're select the latest version.
      
