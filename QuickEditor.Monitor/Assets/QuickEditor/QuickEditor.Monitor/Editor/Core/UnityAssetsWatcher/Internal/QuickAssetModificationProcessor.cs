#if UNITY_EDITOR

namespace QuickEditor.Monitor
{
    using System.Collections.Generic;
    using System.IO;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using Debug = LoggerUtils;
    internal sealed partial class QuickAssetModificationProcessor : UnityEditor.AssetModificationProcessor
    {
        public static void OnWillCreateAsset(string path)
        {
            //Debug.Log("OnWillCreateAsset " + path);
        }

        public static string[] OnWillSaveAssets(string[] paths)
        {
            List<string> result = new List<string>();
            foreach (var path in paths)
            {
                if (IsUnlocked(path))
                    result.Add(path);
                else
                    Debug.LogError(path + " is read-only.");
                if (path.EndsWith(".unity"))
                {
                    Scene scene = SceneManager.GetSceneByPath(path);
                    Debug.Log("Currernt Save Scene ：" + scene.name);
                }
            }
            return result.ToArray();
        }

        public static AssetMoveResult OnWillMoveAsset(string oldPath, string newPath)
        {
            AssetMoveResult result = AssetMoveResult.DidNotMove;
            if (IsLocked(oldPath))
            {
                Debug.LogError(string.Format("Could not move {0} to {1} because {0} is locked!", oldPath, newPath));
                result = AssetMoveResult.FailedMove;
            }
            else if (IsLocked(newPath))
            {
                Debug.LogError(string.Format("Could not move {0} to {1} because {1} is locked!", oldPath, newPath));
                result = AssetMoveResult.FailedMove;
            }
            return result;
        }

        public static AssetDeleteResult OnWillDeleteAsset(string assetPath, RemoveAssetOptions option)
        {
            if (IsLocked(assetPath))
            {
                Debug.LogError(string.Format("Could not delete {0} because it is locked!", assetPath));
                return AssetDeleteResult.FailedDelete;
            }
            return AssetDeleteResult.DidNotDelete;
        }

        public static bool IsOpenForEdit(string assetPath, out string message)
        {
            if (IsLocked(assetPath))
            {
                message = "File is locked for editing!";
                return false;
            }
            else
            {
                message = null;
                return true;
            }
        }

        private static bool IsUnlocked(string path)
        {
            return !IsLocked(path);
        }

        private static bool IsLocked(string path)
        {
            if (!File.Exists(path))
                return false;
            FileInfo fi = new FileInfo(path);
            return fi.IsReadOnly;
        }
    }
}

#endif