namespace SunamoRuleset._sunamo.SunamoExceptions;

internal sealed partial class Exceptions
{
    internal static string CheckBefore(string prefix)
    {
        return string.IsNullOrWhiteSpace(prefix) ? string.Empty : prefix + ": ";
    }

    internal static Tuple<string, string, string> PlaceOfException(bool isFillingFirstTwo = true)
    {
        StackTrace stackTrace = new();
        var stackTraceText = stackTrace.ToString();
        var lines = stackTraceText.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
        lines.RemoveAt(0);
        var index = 0;
        string typeName = string.Empty;
        string methodName = string.Empty;
        for (; index < lines.Count; index++)
        {
            var line = lines[index];
            if (isFillingFirstTwo)
                if (!line.StartsWith("   at ThrowEx"))
                {
                    TypeAndMethodName(line, out typeName, out methodName);
                    isFillingFirstTwo = false;
                }
            if (line.StartsWith("at System."))
            {
                lines.Add(string.Empty);
                lines.Add(string.Empty);
                break;
            }
        }
        return new Tuple<string, string, string>(typeName, methodName, string.Join(Environment.NewLine, lines));
    }

    internal static void TypeAndMethodName(string stackFrameText, out string typeName, out string methodName)
    {
        var trimmedText = stackFrameText.Split("at ")[1].Trim();
        var fullMethodPath = trimmedText.Split("(")[0];
        var segments = fullMethodPath.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        methodName = segments[^1];
        segments.RemoveAt(segments.Count - 1);
        typeName = string.Join(".", segments);
    }

    internal static string CallingMethod(int depth = 1)
    {
        StackTrace stackTrace = new();
        var methodBase = stackTrace.GetFrame(depth)?.GetMethod();
        if (methodBase is null)
        {
            return "Method name cannot be get";
        }
        return methodBase.Name;
    }

    internal static string? Custom(string prefix, string message)
    {
        return CheckBefore(prefix) + message;
    }

    internal static string? NotImplementedCase(string prefix, object notImplementedName)
    {
        var suffix = string.Empty;
        if (notImplementedName != null)
        {
            suffix = " for ";
            if (notImplementedName.GetType() == typeof(Type))
                suffix += ((Type)notImplementedName).FullName;
            else
                suffix += notImplementedName.ToString();
        }
        return CheckBefore(prefix) + "Not implemented case" + suffix + " . internal program error. Please contact developer" +
        ".";
    }
}
