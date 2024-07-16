
namespace SunamoRuleset._sunamo;
using System;

internal class EnumHelper
{
    internal static T Parse<T>(string web, T _def, bool returnDefIfNull = false)
        where T : struct
    {
        if (returnDefIfNull)
        {
            return _def;
        }
        T result;
        if (Enum.TryParse<T>(web, true, out result))
        {
            return result;
        }
        return _def;
    }
}