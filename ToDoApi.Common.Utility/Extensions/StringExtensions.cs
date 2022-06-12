using System;
using System.Text;
using System.Text.RegularExpressions;

namespace ToDoApi.Common.Utility.Extensions;

public static class StringExtensions
{
    public static string ToSnakeCase(this string text)
    {
        if (text == null) throw new ArgumentNullException(nameof(text));

        var stringBuilder = new StringBuilder();
        stringBuilder.Append(text[0].ToLowerInvariant());

        for (var i = 1; i < text.Length; i++)
        {
            var character = text[i];
            var isUpper = char.IsUpper(character);

            stringBuilder.Append(isUpper ? $"_{character.ToLowerInvariant()}" : character);
        }

        return stringBuilder.ToString();
    }

    public static string RemoveByRegex(this string source, string regex)
    {
        return Regex.Replace(source, regex, "");
    }
}