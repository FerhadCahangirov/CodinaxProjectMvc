namespace CodinaxProjectMvc.Helpers
{
    public static class TimeSpanFormatter
    {
        public static string FormatTimeSpan(this TimeSpan timeSpan)
        {
            string formatted = "";
            if (timeSpan.Days > 0)
            {
                formatted += $"{timeSpan.Days}d ";
            }
            if (timeSpan.Hours > 0)
            {
                formatted += $"{timeSpan.Hours}h ";
            }
            if (timeSpan.Minutes > 0)
            {
                formatted += $"{timeSpan.Minutes}m ";
            }
            if (timeSpan.Seconds > 0)
            {
                formatted += $"{timeSpan.Seconds}s";
            }
            return formatted.Trim();
        }
    }
}
