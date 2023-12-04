namespace cli_menu.Utils;

public static class StringExtension
{
    public static string PadCenter(this string str, int totalWidth, char paddingChar)
    {
        str ??= string.Empty;

        var padding = (totalWidth - str.Length) / 2;

        return str.PadLeft(padding + str.Length, paddingChar).PadRight(totalWidth, paddingChar);
    }
}
