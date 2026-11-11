using System.Text;
using System.Text.RegularExpressions;

namespace Web.Services
{
    /// <summary>
    /// Slug для категорій і виробників: латиниця, цифри та дефіс. Українські літери транслітеруються.
    /// </summary>
    public static partial class SlugHelper
    {
        private static readonly Dictionary<char, string> Transliteration = new()
        {
            ['а'] = "a", ['б'] = "b", ['в'] = "v", ['г'] = "h", ['ґ'] = "g", ['д'] = "d", ['е'] = "e",
            ['є'] = "ye", ['ж'] = "zh", ['з'] = "z", ['и'] = "y", ['і'] = "i", ['ї'] = "yi", ['й'] = "y",
            ['к'] = "k", ['л'] = "l", ['м'] = "m", ['н'] = "n", ['о'] = "o", ['п'] = "p", ['р'] = "r",
            ['с'] = "s", ['т'] = "t", ['у'] = "u", ['ф'] = "f", ['х'] = "kh", ['ц'] = "ts", ['ч'] = "ch",
            ['ш'] = "sh", ['щ'] = "shch", ['ь'] = "", ['ю'] = "yu", ['я'] = "ya",
            ['ы'] = "y", ['э'] = "e", ['ё'] = "yo", ['ъ'] = ""
        };

        public static string FromName(string name)
        {
            var builder = new StringBuilder();
            foreach (var ch in name.Trim().ToLowerInvariant())
            {
                if (Transliteration.TryGetValue(ch, out var latin))
                {
                    builder.Append(latin);
                }
                else if (ch is >= 'a' and <= 'z' or >= '0' and <= '9')
                {
                    builder.Append(ch);
                }
                else
                {
                    builder.Append('-');
                }
            }

            // Кілька дефісів підряд і дефіси по краях зайві
            return MultipleDashes().Replace(builder.ToString(), "-").Trim('-');
        }

        public static bool IsValid(string slug) => ValidSlug().IsMatch(slug);

        [GeneratedRegex("-{2,}")]
        private static partial Regex MultipleDashes();

        [GeneratedRegex("^[a-z0-9]+(-[a-z0-9]+)*$")]
        private static partial Regex ValidSlug();
    }
}
