namespace QuickEditor.Monitor
{
    using System.IO;
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;

    //在project界面按alt显示文件后缀名
    [InitializeOnLoad]
    public static class ProjectWindowWatcher
    {
        static ProjectWindowWatcher()
        {
            QuickUnityEditorEventWatcher watcher = QuickUnityEditorEventWatcher.Observe();
            watcher.ProjectView.onProjectWindowItemOnGUI.AddListener(ProjectWindowItemOnGUI);
        }

        private static void ProjectWindowItemOnGUI(string guid, Rect rect)
        {
            if (Event.current.alt)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetPath);

                if (obj != null && AssetDatabase.IsMainAsset(obj) && !IsDirectory(obj))
                {
                    if (showsBigIcons)
                    {
                        string extension = Path.GetExtension(assetPath);
                        GUI.Label(rect, extension, EditorStyles.boldLabel);
                    }
                    else
                    {
                        var labelRect = rect.Translate(new Vector2(19f, 1f));
                        var fileName = Path.GetFileName(assetPath);
                        GUI.Label(labelRect, fileName);
                    }
                }

                EditorApplication.RepaintProjectWindow();
            }
        }

        private static bool showsBigIcons
        {
            get
            {
                return isTwoColumnMode && listAreaGridSize > 16f;
            }
        }

        private static bool isTwoColumnMode
        {
            get
            {
                var projectWindow = GetProjectWindow();

                var projectWindowType = projectWindow.GetType();
                var modeFieldInfo = projectWindowType.GetField("m_ViewMode", BindingFlags.Instance | BindingFlags.NonPublic);

                int mode = (int)modeFieldInfo.GetValue(projectWindow);
                return mode == 1;
            }
        }

        private static float listAreaGridSize
        {
            get
            {
                var projectWindow = GetProjectWindow();

                var projectWindowType = projectWindow.GetType();
                var propertyInfo = projectWindowType.GetProperty("listAreaGridSize", BindingFlags.Instance | BindingFlags.Public);
                return (float)propertyInfo.GetValue(projectWindow, null);
            }
        }

        private static EditorWindow GetProjectWindow()
        {
            if (EditorWindow.focusedWindow != null && EditorWindow.focusedWindow.titleContent.text == "Project")
            {
                return EditorWindow.focusedWindow;
            }
            return GetExistingWindowByName("Project");
        }

        private static EditorWindow GetExistingWindowByName(string name)
        {
            EditorWindow[] windows = Resources.FindObjectsOfTypeAll<EditorWindow>();
            foreach (var item in windows)
            {
                if (item.titleContent.text == name)
                {
                    return item;
                }
            }

            return default(EditorWindow);
        }

        private static Rect Translate(this Rect rect, Vector2 delta)
        {
            rect.x += delta.x;
            rect.y += delta.y;
            return rect;
        }

        private static bool IsDirectory(UnityEngine.Object obj)
        {
            if (obj == null)
            {
                return false;
            }

            return obj is DefaultAsset && !AssetDatabase.IsForeignAsset(obj);
        }
    }
}