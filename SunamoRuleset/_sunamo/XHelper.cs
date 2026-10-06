namespace SunamoRuleset._sunamo;

internal class XHelper
{
    internal static string? Attr(XElement element, string attributeName)
    {
        XAttribute? attribute = element.Attribute(XName.Get(attributeName));
        return attribute?.Value;
    }
}
