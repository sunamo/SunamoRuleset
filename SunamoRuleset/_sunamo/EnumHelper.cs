namespace SunamoRuleset._sunamo;

/// <summary>
/// Helper class for parsing enum values from strings.
/// </summary>
internal class EnumHelper
{
    /// <summary>
    /// Parses a string to an enum value, returning a default if parsing fails.
    /// </summary>
    /// <typeparam name="T">The enum type to parse to.</typeparam>
    /// <param name="text">The string to parse.</param>
    /// <param name="defaultValue">The default value to return if parsing fails.</param>
    /// <param name="isReturningDefaultIfNull">If true, returns the default value immediately without attempting to parse.</param>
    /// <returns>The parsed enum value or the default value.</returns>
    internal static T Parse<T>(string text, T defaultValue, bool isReturningDefaultIfNull = false)
        where T : struct
    {
        if (isReturningDefaultIfNull)
        {
            return defaultValue;
        }
        if (Enum.TryParse<T>(text, true, out T result))
        {
            return result;
        }
        return defaultValue;
    }
}
