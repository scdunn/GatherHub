using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cidean.GatherHub.Core.Helpers
{
    public static class StringExtensions
    {
        /// <summary>
        /// Remove html tags from a string
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string StripHtml(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            return System.Text.RegularExpressions.Regex.Replace(text, "<[^>]*>", string.Empty);
        }


    }
}
