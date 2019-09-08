namespace QuickEditor.Monitor
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using UnityEditor;

    public sealed class QuickAssetWatcherPostprocessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromPaths)
        {
            string[] created = importedAssets.Except(QuickAssetWatcher.allAssets).ToArray();
            string[] modified = importedAssets.Except(created).ToArray();

            Dictionary<string, string> allMoved = new Dictionary<string, string>();
            for (int i = 0; i < movedAssets.Length; i++)
            {
                allMoved.Add(movedAssets[i], movedFromPaths[i]);
            }

            // Renamed to, renamed from
            Dictionary<string, string> renamed =
                (from m in allMoved
                 where (Path.GetDirectoryName(m.Key)) == (Path.GetDirectoryName(m.Value))
                 select m).ToDictionary(p => p.Key, p => p.Value);

            Dictionary<string, string> moved = allMoved.Except(renamed).ToDictionary(p => p.Key, p => p.Value);

            // Dispatch asset events to available watchers
            foreach (QuickAssetWatcher w in QuickAssetWatcher.allWatchers)
            {
                w.InvokeEventForPaths(created, w.onAssetCreated);
                w.InvokeEventForPaths(deletedAssets, w.onAssetDeleted);
                w.InvokeEventForPaths(modified, w.onAssetModified);
                w.InvokeMovedEventForPaths(renamed, w.onAssetRenamed);
                w.InvokeMovedEventForPaths(moved, w.onAssetMoved);
            }

            // Update asset paths cache
            QuickAssetWatcher.allAssets = AssetDatabase.GetAllAssetPaths();
        }

        public void OnPreprocessTexture()
        {
        }

        public void OnPostprocessTexture()
        {
        }

        //        //a.Asset Auditing
        //        //在资源导入时进行检测，避免资源出错，规范资源命名等，具体做法可以参考AssetPostprocessor类，直接在文档里检索就可以了
        //        //b.Common Rules : Texture
        //        //1.Make sure Read/Write is disable
        //        //2.Disable mipmap if possible
        //        //3.Make sure textures are Compressed
        //        //4.Ensure sizes aren’t too large
        //        //   2048*2048  / 1024*1024  for UI atlas
        //        //   512*512 or smaller for model textures
        //        //c.Common Rules : Model
        //        //1.Make sure Read/Write is disable
        //        //2.Disable rig on non-character models
        //        //3.Enable mesh compression

        //        //d.Common Rules : Audio
        //        //1.MP3 compression on IOS
        //        //2.Vorbis compression on Android
        //        //3.”Force Mono”for mobile games
        //        //d.set bitrate as low as possible

        //        //模型导入之前调用
        //        public void OnPreprocessModel()
        //        {
        //            ModelImporter modelImporter = (ModelImporter)assetImporter;
        //            if (assetImporter.assetPath.Contains(".fbx"))
        //            {
        //                modelImporter.globalScale = 1.0f;
        //                //modelImporter.importMaterials = false;
        //            }
        //            Debug.Log("OnPreprocessModel = " + this.assetPath);
        //        }

        //        //模型导入之后调用
        //        public void OnPostprocessModel(GameObject go)
        //        {
        //            Debug.Log("OnPostprocessModel = " + this.assetPath);
        //        }

        //        //音频导入之前
        //        public void OnPreprocessAudio()
        //        {
        //            AudioImporter mAudioImporter = this.assetImporter as AudioImporter;
        //            if (mAudioImporter == null) return;

        //            AudioImporterSampleSettings settings = new AudioImporterSampleSettings();
        //            settings.compressionFormat = AudioCompressionFormat.Vorbis;
        //            settings.loadType = AudioClipLoadType.Streaming;
        //            settings.quality = 100;
        //            settings.sampleRateSetting = AudioSampleRateSetting.PreserveSampleRate;
        //            mAudioImporter.defaultSampleSettings = settings;

        //            mAudioImporter.preloadAudioData = true;
        //            var mOverrideSampleSettings = mAudioImporter.GetOverrideSampleSettings(AssetPostprocessorDefine.PlatformName_Android);
        //            mOverrideSampleSettings.compressionFormat = AudioCompressionFormat.Vorbis;
        //            mOverrideSampleSettings.loadType = AudioClipLoadType.Streaming;
        //            mAudioImporter.SetOverrideSampleSettings(AssetPostprocessorDefine.PlatformName_Android, mOverrideSampleSettings);

        //            mOverrideSampleSettings = mAudioImporter.GetOverrideSampleSettings(AssetPostprocessorDefine.PlatformName_IOS);
        //            mOverrideSampleSettings.compressionFormat = AudioCompressionFormat.MP3;
        //            mOverrideSampleSettings.loadType = AudioClipLoadType.Streaming;
        //            mAudioImporter.SetOverrideSampleSettings(AssetPostprocessorDefine.PlatformName_IOS, mOverrideSampleSettings);

        //            mAudioImporter.forceToMono = false;
        //            mAudioImporter.loadInBackground = false;
        //            mAudioImporter.preloadAudioData = false;
        //            AssetDatabase.WriteImportSettingsIfDirty(assetPath);
        //            Debug.Log("OnPreprocessAudio = " + this.assetPath);
        //        }

        //        //音频导入之后
        //        public void OnPostprocessAudio(AudioClip clip)
        //        {
        //            Debug.Log("OnPostprocessAudio = " + this.assetPath);
        //        }
    }
}