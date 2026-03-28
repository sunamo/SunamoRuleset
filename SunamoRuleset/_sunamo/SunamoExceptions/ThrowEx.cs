namespace SunamoRuleset._sunamo.SunamoExceptions;

/// <summary>
/// Provides methods for throwing formatted exceptions with stack trace context.
/// </summary>
internal partial class ThrowEx
{
    /// <summary>
    /// Throws a custom exception with the specified message.
    /// </summary>
    /// <param name="message">The primary error message.</param>
    /// <param name="isReallyThrowing">If true, actually throws the exception; otherwise just returns whether an exception would be thrown.</param>
    /// <param name="secondMessage">An optional second message to append.</param>
    /// <returns>True if an exception was generated, false otherwise.</returns>
    internal static bool Custom(string message, bool isReallyThrowing = true, string secondMessage = "")
    {
        string joined = string.Join(" ", message, secondMessage);
        string? exceptionText = Exceptions.Custom(FullNameOfExecutedCode(), joined);
        return ThrowIsNotNull(exceptionText, isReallyThrowing);
    }

    /// <summary>
    /// Throws an exception for a not-implemented case.
    /// </summary>
    /// <param name="notImplementedName">The name or value of the not-implemented case.</param>
    /// <returns>True if an exception was generated, false otherwise.</returns>
    internal static bool NotImplementedCase(object notImplementedName)
    { return ThrowIsNotNull(Exceptions.NotImplementedCase, notImplementedName); }

    /// <summary>
    /// Gets the fully qualified name of the currently executed code location.
    /// </summary>
    /// <returns>A string in the format "TypeName.MethodName".</returns>
    internal static string FullNameOfExecutedCode()
    {
        Tuple<string, string, string> placeOfException = Exceptions.PlaceOfException();
        string fullName = FullNameOfExecutedCode(placeOfException.Item1, placeOfException.Item2, true);
        return fullName;
    }

    private static string FullNameOfExecutedCode(object type, string methodName, bool isFromThrowEx = false)
    {
        if (methodName == null)
        {
            int depth = 2;
            if (isFromThrowEx)
            {
                depth++;
            }

            methodName = Exceptions.CallingMethod(depth);
        }
        string typeFullName;
        if (type is Type typeValue)
        {
            typeFullName = typeValue.FullName ?? "Type cannot be get via type is Type typeValue";
        }
        else if (type is MethodBase method)
        {
            typeFullName = method.ReflectedType?.FullName ?? "Type cannot be get via type is MethodBase method";
            methodName = method.Name;
        }
        else if (type is string)
        {
            typeFullName = type.ToString() ?? "Type cannot be get via type is string";
        }
        else
        {
            Type objectType = type.GetType();
            typeFullName = objectType.FullName ?? "Type cannot be get via type.GetType()";
        }
        return string.Concat(typeFullName, ".", methodName);
    }

    /// <summary>
    /// Throws an exception if the exception text is not null.
    /// </summary>
    /// <param name="exception">The exception message to evaluate.</param>
    /// <param name="isReallyThrowing">If true, actually throws; otherwise just returns the result.</param>
    /// <returns>True if an exception was generated, false otherwise.</returns>
    internal static bool ThrowIsNotNull(string? exception, bool isReallyThrowing = true)
    {
        if (exception != null)
        {
            Debugger.Break();
            if (isReallyThrowing)
            {
                throw new Exception(exception);
            }
            return true;
        }
        return false;
    }

    /// <summary>
    /// Invokes an exception factory function with the current execution context and throws if result is not null.
    /// </summary>
    /// <typeparam name="TArgument">The type of the argument passed to the exception factory.</typeparam>
    /// <param name="exceptionFactory">The function that generates the exception message.</param>
    /// <param name="argument">The argument to pass to the exception factory.</param>
    /// <returns>True if an exception was generated, false otherwise.</returns>
    internal static bool ThrowIsNotNull<TArgument>(Func<string, TArgument, string?> exceptionFactory, TArgument argument)
    {
        string? exceptionText = exceptionFactory(FullNameOfExecutedCode(), argument);
        return ThrowIsNotNull(exceptionText);
    }
}
