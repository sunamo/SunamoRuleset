namespace SunamoRuleset._sunamo;

/// <summary>
/// Helper class for splitting text into lines using various newline formats.
/// </summary>
internal class SHGetLines
{
    /// <summary>
    /// Splits text into lines handling all newline formats (CRLF, LFCR, CR, LF).
    /// </summary>
    /// <param name="text">The text to split into lines.</param>
    /// <returns>A list of lines from the text.</returns>
    internal static List<string> GetLines(string text)
    {
        var parts = text.Split(new[] { "\r\n", "\n\r" }, StringSplitOptions.None).ToList();
        SplitByUnixNewline(parts);
        return parts;
    }

    private static void SplitByUnixNewline(List<string> list)
    {
        SplitBy(list, "\r");
        SplitBy(list, "\n");
    }

    private static void SplitBy(List<string> list, string delimiter)
    {
        for (var i = list.Count - 1; i >= 0; i--)
        {
            if (delimiter == "\r")
            {
                var windowsNewlineParts = list[i].Split(new[] { "\r\n" }, StringSplitOptions.None);
                var reverseNewlineParts = list[i].Split(new[] { "\n\r" }, StringSplitOptions.None);

                if (windowsNewlineParts.Length > 1)
                    ThrowEx.Custom("cannot contain any \r\n, pass already split by this pattern");
                else if (reverseNewlineParts.Length > 1) ThrowEx.Custom("cannot contain any \n\r, pass already split by this pattern");
            }

            var splitParts = list[i].Split(new[] { delimiter }, StringSplitOptions.None);

            if (splitParts.Length > 1) InsertOnIndex(list, splitParts.ToList(), i);
        }
    }

    private static void InsertOnIndex(List<string> list, List<string> insertList, int index)
    {
        insertList.Reverse();

        list.RemoveAt(index);

        foreach (var item in insertList) list.Insert(index, item);
    }
}
