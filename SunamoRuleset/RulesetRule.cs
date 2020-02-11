using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

public class RulesetRule : IXParser
{
    public string Id = null;
    public RulesetActions Action = RulesetActions.None;

    public void Parse(XElement node)
    {
        Id = XHelper.Attr(node, "Id");
        Action = EnumHelper.Parse<RulesetActions>( XHelper.Attr(node, RulesetConsts.Action), RulesetActions.None);
    }

    public string ToXml()
    {
        return "<Rule Id=\""+Id+"\" Action=\""+Action+"\" />";
    }
}
