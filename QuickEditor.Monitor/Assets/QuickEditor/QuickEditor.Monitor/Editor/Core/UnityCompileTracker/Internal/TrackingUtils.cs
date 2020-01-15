namespace QuickEditor.Monitor
{
    using System;
    using System.Globalization;

    internal sealed partial class TrackingUtils
    {
        public static string FormatMSTime(int ms)
        {
            return string.Format("{0}s", (ms / 1000.0f).ToString("F2", CultureInfo.InvariantCulture));
        }

        public static int GetMilliseconds()
        {
            return (int)(DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);
        }
    }
}