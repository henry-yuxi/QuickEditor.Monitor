#if UNITY_EDITOR

namespace QuickEditor.Monitor
{
    public sealed partial class Utils
    {
        private const string DefaultPrefix = "> ";
        public const string Assembly = "QuickEditor.Monitor";
        public const string FORMAT = DefaultPrefix + "<b>[{0}]</b> --> {1}";
    }
}

#endif