namespace SunamoRuleset._sunamo;

/// <summary>
/// Generates XML content using a StringBuilder with support for tags, attributes, and optional stack tracking.
/// </summary>
internal class XmlGenerator
{
    internal StringBuilder ContentBuilder { get; set; } = new StringBuilder();
    private bool useStack = false;
    private Stack<string>? stack = null;

    /// <summary>
    /// Initializes a new instance of the <see cref="XmlGenerator"/> class without stack tracking.
    /// </summary>
    internal XmlGenerator() : this(false)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XmlGenerator"/> class.
    /// </summary>
    /// <param name="isUsingStack">If true, enables stack tracking for written tags.</param>
    internal XmlGenerator(bool isUsingStack)
    {
        useStack = isUsingStack;
        if (isUsingStack)
        {
            stack = new Stack<string>();
        }
    }

    /// <summary>
    /// Writes raw XML content directly to the output.
    /// </summary>
    /// <param name="text">The raw XML text to append.</param>
    internal void WriteRaw(string text)
    {
        ContentBuilder.Append(text);
    }

    /// <summary>
    /// Writes a closing tag for the specified element.
    /// </summary>
    /// <param name="tagName">The name of the tag to terminate.</param>
    internal void TerminateTag(string tagName)
    {
        ContentBuilder.AppendFormat("</{0}>", tagName);
    }

    /// <summary>
    /// Returns the generated XML content as a string.
    /// </summary>
    /// <returns>The complete XML content.</returns>
    public override string ToString()
    {
        return ContentBuilder.ToString();
    }

    /// <summary>
    /// Writes an opening tag with the specified attributes. Null attributes are included.
    /// </summary>
    /// <param name="tagName">The name of the XML tag.</param>
    /// <param name="attributes">Alternating attribute name/value pairs.</param>
    internal void WriteTagWithAttrs(string tagName, params string[] attributes)
    {
        WriteTagWithAttrs(true, tagName, attributes);
    }

    private bool IsNulledOrEmpty(string text)
    {
        if (string.IsNullOrEmpty(text) || text == "(null)")
        {
            return true;
        }
        return false;
    }

    private void WriteTagWithAttrs(bool isAppendingNull, string tagName, params string[] attributes)
    {
        StringBuilder tagBuilder = new StringBuilder();
        tagBuilder.AppendFormat("<{0} ", tagName);
        for (int i = 0; i < attributes.Length; i++)
        {
            var attributeName = attributes[i];
            var attributeValue = attributes[++i];
            if (string.IsNullOrEmpty(attributeValue) && isAppendingNull || !string.IsNullOrEmpty(attributeValue))
            {
                if (!IsNulledOrEmpty(attributeName) && isAppendingNull || !IsNulledOrEmpty(attributeValue))
                {
                    tagBuilder.AppendFormat("{0}=\"{1}\" ", attributeName, attributeValue);
                }
            }
        }
        tagBuilder.Append('<');
        string result = tagBuilder.ToString();
        if (useStack)
        {
            stack!.Push(result);
        }
        ContentBuilder.Append(result);
    }

    /// <summary>
    /// Writes the standard XML declaration header.
    /// </summary>
    internal void WriteXmlDeclaration()
    {
        ContentBuilder.Append(XmlTemplates.Xml);
    }
}
