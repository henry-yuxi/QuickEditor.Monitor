namespace QuickEditor.Monitor
{
    using System;
    using System.Collections.Generic;
    using UnityEditor;

    [InitializeOnLoad]
    internal sealed partial class UnityScripsCompileTimeTracker
    {
        public static event Action<UnityScripsCompileTimeKeyframe> KeyframeAdded = delegate { };

        public static IList<UnityScripsCompileTimeKeyframe> GetCompileTimeHistory()
        {
            return UnityScripsCompileTimeTracker._Data.GetCompileTimeHistory();
        }

        static UnityScripsCompileTimeTracker()
        {
            UnityScripsCompileWatcher.StartedCompiling += UnityScripsCompileTimeTracker.HandleEditorStartedCompiling;
            UnityScripsCompileWatcher.FinishedCompiling += UnityScripsCompileTimeTracker.HandleEditorFinishedCompiling;
        }

        private const string kCompileTimeTrackerKey = "UnityScripsCompileTimeTracker::_data";
        private static UnityScripsCompileTimeTrackerData _data = null;

        private static UnityScripsCompileTimeTrackerData _Data
        {
            get
            {
                if (_data == null)
                {
                    _data = new UnityScripsCompileTimeTrackerData(kCompileTimeTrackerKey);
                }
                return _data;
            }
        }

        private static int StoredErrorCount
        {
            get { return EditorPrefs.GetInt("UnityScripsCompileTimeTracker::StoredErrorCount"); }
            set { EditorPrefs.SetInt("UnityScripsCompileTimeTracker::StoredErrorCount", value); }
        }

        private static void HandleEditorStartedCompiling()
        {
            UnityScripsCompileTimeTracker._Data.StartTime = TrackingUtils.GetMilliseconds();

            UnityConsoleCountsByType countsByType = UnityEditorConsoleUtils.GetCountsByType();
            UnityScripsCompileTimeTracker.StoredErrorCount = countsByType.errorCount;
        }

        private static void HandleEditorFinishedCompiling()
        {
            int elapsedTime = TrackingUtils.GetMilliseconds() - UnityScripsCompileTimeTracker._Data.StartTime;

            UnityConsoleCountsByType countsByType = UnityEditorConsoleUtils.GetCountsByType();
            bool hasErrors = (countsByType.errorCount - UnityScripsCompileTimeTracker.StoredErrorCount) > 0;

            UnityScripsCompileTimeKeyframe keyframe = new UnityScripsCompileTimeKeyframe(elapsedTime, hasErrors);
            UnityScripsCompileTimeTracker._Data.AddCompileTimeKeyframe(keyframe);
            UnityScripsCompileTimeTracker.KeyframeAdded.Invoke(keyframe);
        }
    }
}