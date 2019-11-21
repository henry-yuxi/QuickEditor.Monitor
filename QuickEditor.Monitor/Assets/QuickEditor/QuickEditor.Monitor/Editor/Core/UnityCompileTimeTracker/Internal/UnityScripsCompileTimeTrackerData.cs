namespace QuickEditor.Monitor
{
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;

    internal sealed partial class UnityScripsCompileTimeTrackerData
    {
        private const int kHistoryKeyframeMaxCount = 100;

        public int StartTime
        {
            get { return this._startTime; }
            set
            {
                this._startTime = value;
                this.Save();
            }
        }

        public void AddCompileTimeKeyframe(UnityScripsCompileTimeKeyframe keyframe)
        {
            this._compileTimeHistory.Add(keyframe);
            this.Save();
        }

        public IList<UnityScripsCompileTimeKeyframe> GetCompileTimeHistory()
        {
            return this._compileTimeHistory;
        }

        public UnityScripsCompileTimeTrackerData(string editorPrefKey)
        {
            this._editorPrefKey = editorPrefKey;
            this.Load();
        }

        [SerializeField] private int _startTime;
        [SerializeField] private List<UnityScripsCompileTimeKeyframe> _compileTimeHistory;

        private string _editorPrefKey;

        private void Save()
        {
            while (this._compileTimeHistory.Count > UnityScripsCompileTimeTrackerData.kHistoryKeyframeMaxCount)
            {
                this._compileTimeHistory.RemoveAt(0);
            }

            EditorPrefs.SetInt(this._editorPrefKey + "._startTime", this._startTime);
            EditorPrefs.SetString(this._editorPrefKey + "._compileTimeHistory", UnityScripsCompileTimeKeyframe.SerializeList(this._compileTimeHistory));
        }

        private void Load()
        {
            this._startTime = EditorPrefs.GetInt(this._editorPrefKey + "._startTime");
            string key = this._editorPrefKey + "._compileTimeHistory";
            if (EditorPrefs.HasKey(key))
            {
                this._compileTimeHistory = UnityScripsCompileTimeKeyframe.DeserializeList(EditorPrefs.GetString(key));
            }
            else
            {
                this._compileTimeHistory = new List<UnityScripsCompileTimeKeyframe>();
            }
        }
    }
}