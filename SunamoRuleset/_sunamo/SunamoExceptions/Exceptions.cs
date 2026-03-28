namespace SunamoRuleset._sunamo.SunamoExceptions;

/// <summary>
/// Provides exception message formatting and stack trace analysis utilities.
/// </summary>
internal sealed partial class Exceptions
{
    /// <summary>
    /// Formats a prefix string for exception messages.
    /// </summary>
    /// <param name="prefix">The prefix to prepend to the exception message.</param>
    /// <returns>The formatted prefix with colon separator, or empty string if prefix is blank.</returns>
    internal static string CheckBefore(string prefix)
    {
        return string.IsNullOrWhiteSpace(prefix) ? string.Empty : prefix + ": ";
    }

    /// <summary>
    /// Analyzes the current stack trace to determine the place of exception.
    /// </summary>
    /// <param name="isFillingFirstTwo">Whether to fill type and method name from the first non-ThrowEx frame.</param>
    /// <returns>A tuple containing (type name, method name, full stack trace text).</returns>
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

    /// <summary>
    /// Extracts type name and method name from a stack trace frame line.
    /// </summary>
    /// <param name="stackFrameText">A single line from the stack trace.</param>
    /// <param name="typeName">Output: the fully qualified type name.</param>
    /// <param name="methodName">Output: the method name.</param>
    internal static void TypeAndMethodName(string stackFrameText, out string typeName, out string methodName)
    {
        var trimmedText = stackFrameText.Split("at ")[1].Trim();
        var fullMethodPath = trimmedText.Split("(")[0];
        var segments = fullMethodPath.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        methodName = segments[^1];
        segments.RemoveAt(segments.Count - 1);
        typeName = string.Join(".", segments);
    }

    /// <summary>
    /// Gets the name of the calling method at the specified stack depth.
    /// </summary>
    /// <param name="depth">The stack frame depth to inspect.</param>
    /// <returns>The name of the calling method.</returns>
    internal static string CallingMethod(int depth = 1)
    {
        StackTrace stackTrace = new();
        var methodBase = stackTrace.GetFrame(depth)?.GetMethod();
        if (methodBase == null)
        {
            return "Method name cannot be get";
        }
        var methodName = methodBase.Name;
        return methodName;
    }

    /// <summary>
    /// Creates an exception message for a custom error.
    /// </summary>
    /// <param name="prefix">The prefix identifying the source of the exception.</param>
    /// <param name="message">The custom error message.</param>
    /// <returns>The formatted exception message.</returns>
    internal static string? Custom(string prefix, string message)
    {
        return CheckBefore(prefix) + message;
    }

    /// <summary>
    /// Creates an exception message for a not-implemented case.
    /// </summary>
    /// <param name="prefix">The prefix identifying the source of the exception.</param>
    /// <param name="notImplementedName">The name or value of the not-implemented case.</param>
    /// <returns>The formatted exception message.</returns>
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
