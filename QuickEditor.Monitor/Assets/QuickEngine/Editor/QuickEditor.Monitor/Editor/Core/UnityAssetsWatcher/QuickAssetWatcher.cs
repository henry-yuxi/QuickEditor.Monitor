namespace QuickEditor.Monitor
{
    using System.Collections.Generic;
    using System.IO;
    using UnityEditor;
    using UnityEngine.Events;

    public class QuickAssetWatcher
    {
        public class FileEvent : UnityEvent<QuickAssetFileInfo> { }

        public class FileMoveEvent : UnityEvent<QuickAssetFileInfo, QuickAssetFileInfo> { } // AssetFileInfo before and after the move

        internal static string[] allAssets;
        internal static List<QuickAssetWatcher> allWatchers;

        /// <summary>
        /// Occurs when an asset is first created.
        /// </summary>
        public readonly FileEvent onAssetCreated = new FileEvent();

        /// <summary>
        /// Occurs when an asset is deleted or is moved out of scope.
        /// </summary>
        public readonly FileEvent onAssetDeleted = new FileEvent();

        /// <summary>
        /// Occurs when the content of an asset is modified.
        /// </summary>
        public readonly FileEvent onAssetModified = new FileEvent();

        /// <summary>
        /// Occurs when an asset is renamed in-place.
        /// </summary>
        public readonly FileMoveEvent onAssetRenamed = new FileMoveEvent();

        /// <summary>
        /// Occurs when an asset is moved to a new location within scope.
        /// </summary>
        public readonly FileMoveEvent onAssetMoved = new FileMoveEvent();

        public readonly string basePath;
        public readonly UnityAssetType observedAssetTypes;
        public readonly bool recurseSubdirectories;

        /// <summary>
        /// Initialize the AssetsWatcher when a project is loaded.
        /// </summary>
        static QuickAssetWatcher()
        {
            // Cache asset paths
            allAssets = AssetDatabase.GetAllAssetPaths();
            allWatchers = new List<QuickAssetWatcher>();
        }

        private QuickAssetWatcher(string path, UnityAssetType assetType, bool recurseSubdirectories)
        {
            this.basePath = Path.Combine("Assets", path);
            this.observedAssetTypes = assetType;
            this.recurseSubdirectories = recurseSubdirectories;
        }

        ~QuickAssetWatcher()
        {
            QuickAssetWatcher.RemoveWatcher(this);
        }

        internal void InvokeEventForPaths(string[] paths, FileEvent e)
        {
            if (e == null)
                return;
            foreach (var p in paths)
            {
                if (IsValidPath(p))
                {
                    QuickAssetFileInfo asset = new QuickAssetFileInfo(p);
                    if (observedAssetTypes == UnityAssetType.None || (observedAssetTypes & asset.Type) == asset.Type)
                    {
                        e.Invoke(asset);
                    }
                }
            }
        }

        internal void InvokeMovedEventForPaths(Dictionary<string, string> paths, FileMoveEvent e)
        {
            if (e == null)
                return;
            foreach (var p in paths)
            {
                bool beforePathValid = IsValidPath(p.Value);
                bool afterPathValid = IsValidPath(p.Key);
                if (beforePathValid || afterPathValid)
                {
                    var before = beforePathValid ? new QuickAssetFileInfo(p.Value) : null;
                    var after = afterPathValid ? new QuickAssetFileInfo(p.Key) : null;
                    e.Invoke(before, after);
                }
            }
        }

        /// <summary>
        /// Determines whether the specified assetPath is valid given the current path constraints.
        /// </summary>
        private bool IsValidPath(string assetPath)
        {
            if (recurseSubdirectories)
                return assetPath.StartsWith(this.basePath);
            else
                return Path.GetDirectoryName(assetPath) == this.basePath;
        }

        #region API

        /// <summary>
        /// Watch for changes to the given asset type flags in the given path, and optionally recursing subdirectories.
        /// If no path is specified, the entire Assets folder will be used.
        /// If no asset type is specified, all asset types will be observed.
        /// </summary>
        public static QuickAssetWatcher Observe(string path = "", UnityAssetType assetType = UnityAssetType.None, bool recurseSubdirectories = true)
        {
            QuickAssetWatcher w = new QuickAssetWatcher(path, assetType, recurseSubdirectories);
            allWatchers.Add(w);
            return w;
        }

        public void Disable()
        {
            onAssetCreated.RemoveAllListeners();
            onAssetDeleted.RemoveAllListeners();
            onAssetModified.RemoveAllListeners();
            onAssetMoved.RemoveAllListeners();
            onAssetRenamed.RemoveAllListeners();
            allWatchers.Remove(this);
        }

        /// <summary>
        /// Disable the specified watcher.
        /// </summary>
        private static void RemoveWatcher(QuickAssetWatcher watcher)
        {
            allWatchers.Remove(watcher);
        }

        #endregion API
    }
}