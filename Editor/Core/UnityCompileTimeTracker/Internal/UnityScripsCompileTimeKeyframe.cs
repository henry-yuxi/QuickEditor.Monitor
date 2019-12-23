namespace QuickEditor.Monitor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Debug = LoggerUtils;

    [Serializable]
    internal sealed partial class UnityScripsCompileTimeKeyframe
    {
        private const string kKeyframeDelimiter = "@";
        private const string kListDelimiter = "#";

        private static readonly string[] kKeyframeDelimiterArray = new string[] { UnityScripsCompileTimeKeyframe.kKeyframeDelimiter };
        private static readonly string[] kListDelimiterArray = new string[] { UnityScripsCompileTimeKeyframe.kListDelimiter };

        public static UnityScripsCompileTimeKeyframe Deserialize(string serialized)
        {
            string[] tokens = serialized.Split(UnityScripsCompileTimeKeyframe.kKeyframeDelimiterArray, StringSplitOptions.None);
            if (tokens.Length != 3)
            {
                Debug.LogError("Failed to deserialize CompileTimeKeyframe because splitting by " + UnityScripsCompileTimeKeyframe.kKeyframeDelimiter + " did not result in 3 tokens!");
                return null;
            }

            UnityScripsCompileTimeKeyframe keyframe = new UnityScripsCompileTimeKeyframe();
            keyframe.elapsedCompileTimeInMS = Convert.ToInt32(tokens[0]);
            keyframe.serializedDate = tokens[1];
            keyframe.hadErrors = Convert.ToBoolean(tokens[2]);

            return keyframe;
        }

        public static string Serialize(UnityScripsCompileTimeKeyframe keyframe)
        {
            if (keyframe == null)
            {
                return "";
            }

            return string.Format("{1}{0}{2}{0}{3}", UnityScripsCompileTimeKeyframe.kKeyframeDelimiter, keyframe.elapsedCompileTimeInMS, keyframe.serializedDate, keyframe.hadErrors);
        }

        public static List<UnityScripsCompileTimeKeyframe> DeserializeList(string serialized)
        {
            if (String.IsNullOrEmpty(serialized))
            {
                return new List<UnityScripsCompileTimeKeyframe>();
            }

            string[] serializedKeyframes = serialized.Split(UnityScripsCompileTimeKeyframe.kListDelimiterArray, StringSplitOptions.None);

            return serializedKeyframes.Select(s => UnityScripsCompileTimeKeyframe.Deserialize(s)).Where(k => k != null).ToList();
        }

        public static string SerializeList(List<UnityScripsCompileTimeKeyframe> keyframes)
        {
            string[] serializedKeyframes = keyframes.Where(k => k != null).Select(k => UnityScripsCompileTimeKeyframe.Serialize(k)).ToArray();
            return string.Join(UnityScripsCompileTimeKeyframe.kListDelimiter, serializedKeyframes);
        }

        public static string ToCSV(UnityScripsCompileTimeKeyframe keyframe)
        {
            if (keyframe == null)
            {
                return "";
            }

            return string.Format("{0},{1},{2}", keyframe._computedDate, keyframe.elapsedCompileTimeInMS, keyframe.hadErrors);
        }

        public static string ToCSV(List<UnityScripsCompileTimeKeyframe> keyframes)
        {
            string[] serializedKeyframes = keyframes.Where(k => k != null).Select(k => UnityScripsCompileTimeKeyframe.ToCSV(k)).ToArray();
            string fields = "date,compile_time,had_errors\n";
            return fields + string.Join("\n", serializedKeyframes);
        }

        // PRAGMA MARK - Public Interface
        public DateTime Date
        {
            get
            {
                if (this._computedDate == null)
                {
                    if (string.IsNullOrEmpty(this.serializedDate))
                    {
                        this._computedDate = DateTime.MinValue;
                    }
                    else
                    {
                        this._computedDate = DateTime.Parse(this.serializedDate);
                    }
                }

                return this._computedDate.Value;
            }
        }

        public int elapsedCompileTimeInMS = 0;
        public string serializedDate = "";
        public bool hadErrors = false;

        private UnityScripsCompileTimeKeyframe()
        {
        }

        public UnityScripsCompileTimeKeyframe(int elapsedCompileTimeInMS, bool hadErrors)
        {
            this.elapsedCompileTimeInMS = elapsedCompileTimeInMS;
            this.serializedDate = DateTime.Now.ToString();
            this.hadErrors = hadErrors;
        }

        [NonSerialized] private DateTime? _computedDate;
    }
}