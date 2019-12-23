#if UNITY_EDITOR

namespace QuickEditor.Monitor
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using UnityEditor;

    public sealed class QuickAssetPostprocessor : AssetPostprocessor
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
                w.InvokeEventForPaths(created, w.OnAssetCreated);
                w.InvokeEventForPaths(deletedAssets, w.OnAssetDeleted);
                w.InvokeEventForPaths(modified, w.OnAssetModified);
                w.InvokeMovedEventForPaths(renamed, w.OnAssetRenamed);
                w.InvokeMovedEventForPaths(moved, w.OnAssetMoved);
            }

            // Update asset paths cache
            QuickAssetWatcher.allAssets = AssetDatabase.GetAllAssetPaths();
        }
    }
}

#endif