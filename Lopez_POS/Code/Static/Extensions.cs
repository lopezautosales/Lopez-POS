namespace Lopez_POS
{
    public static class Extensions
    {
        public static string ToCapital(this string message)
        {
            return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(message.ToLower());
        }
    }
}