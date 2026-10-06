namespace SunamoRuleset._sunamo;

internal class XmlGenerator
{
    internal StringBuilder ContentBuilder { get; set; } = new StringBuilder();
    private bool useStack = false;
    private Stack<string>? stack = null;

    internal XmlGenerator() : this(false)
    {
    }

    internal XmlGenerator(bool isUsingStack)
    {
        useStack = isUsingStack;
        if (isUsingStack)
        {
            stack = new Stack<string>();
        }
    }

    internal void WriteRaw(string text)
    {
        ContentBuilder.Append(text);
    }

    internal void TerminateTag(string tagName)
    {
        ContentBuilder.AppendFormat("</{0}>", tagName);
    }

    public override string ToString()
        => ContentBuilder.ToString();

    internal void WriteTagWithAttrs(string tagName, params string[] attributes)
    {
        WriteTagWithAttrs(true, tagName, attributes);
    }

    private bool IsNulledOrEmpty(string text)
        => string.IsNullOrEmpty(text) || text == "(null)";

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

    internal void WriteXmlDeclaration()
    {
        ContentBuilder.Append(XmlTemplates.Xml);
    }
}
