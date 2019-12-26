# QuickEditor.Monitor

## Overview

QuickEditor.Monitor是对Unity的编辑器事件、资源导入回调做了二次封装, 使用者不需要关注Unity版本API的修改、删减. 

### Runtime Environment (运行环境)

Version : Unity 4.x ~ 2020.x

## How to use?

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

### UnityEditorEventsWatcher example
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
    "com.sourcemuch.quickeditor.monitor": "https://github.com/henry-yuxi/QuickEditor.Monitor.git#0.1.4",
  }
}
```

### Unity 2019.3 Git URL

In Unity 2019.3 or greater, Package Manager is include the new feature that able to install the package via Git.

Open the package manager window (menu: Window > Package Manager), select "Add package from git URL...", fill in this in the pop-up textbox: 
https://github.com/henry-yuxi/QuickEditor.Monitor.git#0.1.4.


### Unity UPM Git Extension (For 2019.2 and older version)

If you doesn't have this package before, please redirect to this git https://github.com/mob-sakai/UpmGitExtension then follow the instruction in README.md to install the UPM Git Extension to your Unity.

If you already installed. Open the Package Manager UI, you will see the git icon around the bottom left connor, Open it then follow the instruction using this git URL to perform package install.

请确保使用的UPM包是最终版本。
      
### Package URL's
<table>
  <tbody>
    <tr style="text-align: center; font-weight:bold">
      <td width="200" style="border-width: 1px; background: rgb(248, 248, 248); padding: 6px 13px; ">
        <b>Version</b>
      </td>
      <td width="500" style="border-top-width: 1px; border-right-width: 1px; border-bottom-width: 1px; border-left: none; background: rgb(248, 248, 248); padding: 6px 13px;">
        <b>Link</b>
      </td>
    </tr>
    <tr class="firstRow">
      <td width="200" style="border-width: 1px; background: rgb(248, 248, 248); padding: 6px 13px;">
        0.1.4
      </td>
      <td width="500" style="border-top-width: 1px; border-right-width: 1px; border-bottom-width: 1px; border-left: none; background: rgb(248, 248, 248); padding: 6px 13px;">
        https://github.com/henry-yuxi/QuickEditor.Monitor.git#0.1.4
      </td>
    </tr>      
  </tbody>
</table>      

## See Also

GitHub Page : https://github.com/henry-yuxi/QuickEditor.Monitor/

Issue tracker : https://github.com/henry-yuxi/QuickEditor.Monitor/issues
