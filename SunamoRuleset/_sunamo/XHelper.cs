namespace SunamoRuleset._sunamo;

/// <summary>
/// Helper class for working with XML elements and attributes.
/// </summary>
internal class XHelper
{
    /// <summary>
    /// Gets the value of an attribute from an XML element.
    /// </summary>
    /// <param name="element">The XML element to read the attribute from.</param>
    /// <param name="attributeName">The name of the attribute to retrieve.</param>
    /// <returns>The attribute value, or null if the attribute does not exist.</returns>
    internal static string? Attr(XElement element, string attributeName)
    {
        XAttribute? attribute = element.Attribute(XName.Get(attributeName));
        if (attribute != null)
        {
            return attribute.Value;
        }
        return null;
    }
}
