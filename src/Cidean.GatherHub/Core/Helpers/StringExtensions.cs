using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        /// <summary>
        /// Format a text string with dictionary values
        /// </summary>
        /// <param name="text"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        public static string FormatWithTags(this string text, object tags)
        {
            //return original string if no tag values given
            if (tags == null) return text;

            string result;
            //initialize tag dictionary of values
            IDictionary<string, string> tagsDictionary = null;


            //if already dictionary just cast
            if (tags is IDictionary<string, string>)
                tagsDictionary = tags as IDictionary<string, string>;
            else
            {
                //object or anonymous type so convert to dictionary of values
                tagsDictionary = TypeDescriptor
                .GetProperties(tags).Cast< PropertyDescriptor>()
                .ToDictionary(x => x.Name, x => x.GetValue(tags).ToString());
            }

            //replace all tags with values from dictionary
            result = tagsDictionary.Aggregate(text, (current, tag) => current.Replace("{" + tag.Key + "}", (tag.Value ?? string.Empty).ToString()));
                       
            return result;
        }
    }
}
