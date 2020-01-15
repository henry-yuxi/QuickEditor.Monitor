namespace QuickEditor.Monitor
{
    using System;
    using System.Reflection;
    using UnityEditor;
    using Debug = LoggerUtils;

    public struct UnityConsoleCountsByType
    {
        public int errorCount;
        public int warningCount;
        public int logCount;
    }

    public static class UnityEditorConsoleUtils
    {
        private static MethodInfo _clearMethod;
        private static MethodInfo _getCountMethod;
        private static MethodInfo _getCountsByTypeMethod;

        static UnityEditorConsoleUtils()
        {
            Assembly assembly = Assembly.GetAssembly(typeof(SceneView));
            Type logEntriesType;
#if UNITY_2017_1_OR_NEWER
            logEntriesType = assembly.GetType("UnityEditor.LogEntries");
#else
      logEntriesType  = assembly.GetType("UnityEditorInternal.LogEntries");
#endif

            UnityEditorConsoleUtils._clearMethod = logEntriesType.GetMethod("Clear");
            UnityEditorConsoleUtils._getCountMethod = logEntriesType.GetMethod("GetCount");
            UnityEditorConsoleUtils._getCountsByTypeMethod = logEntriesType.GetMethod("GetCountsByType");
        }

        public static void Clear()
        {
            if (UnityEditorConsoleUtils._clearMethod == null)
            {
                Debug.LogError("Failed to find LogEntries.Clear method info!");
                return;
            }

            UnityEditorConsoleUtils._clearMethod.Invoke(null, null);
        }

        public static int GetCount()
        {
            if (UnityEditorConsoleUtils._getCountMethod == null)
            {
                Debug.LogError("Failed to find LogEntries.GetCount method info!");
                return 0;
            }

            return (int)UnityEditorConsoleUtils._getCountMethod.Invoke(null, null);
        }

        public static UnityConsoleCountsByType GetCountsByType()
        {
            UnityConsoleCountsByType countsByType = new UnityConsoleCountsByType();

            if (UnityEditorConsoleUtils._getCountsByTypeMethod == null)
            {
                Debug.LogError("Failed to find LogEntries.GetCountsByType method info!");
                return countsByType;
            }

            object[] arguments = new object[] { 0, 0, 0 };
            UnityEditorConsoleUtils._getCountsByTypeMethod.Invoke(null, arguments);

            countsByType.errorCount = (int)arguments[0];
            countsByType.warningCount = (int)arguments[1];
            countsByType.logCount = (int)arguments[2];

            return countsByType;
        }
    }
}