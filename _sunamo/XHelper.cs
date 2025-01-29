namespace SunamoRuleset._sunamo;
using System.Collections.Generic;
using System.Xml;

internal class XHelper
{
    internal static string Attr(XElement item, string attr)
    {
        XAttribute xa = item.Attribute(XName.Get(attr));
        if (xa != null)
        {
            return xa.Value;
        }
        return null;
    }
}