
namespace SunamoRuleset._sunamo;
using System;

internal class ThrowEx
{
    internal static void Custom(string v)
    {
        throw new Exception(v);
    }
    internal static void NotImplementedCase<T>(T rtype)
    {
        throw new Exception("NotImplementedCase: " + rtype);
    }
    internal static void NotImplementedMethod()
    {
        throw new Exception("NotImplementedMethod");
    }
}