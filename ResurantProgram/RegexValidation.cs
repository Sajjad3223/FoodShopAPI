using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ResturantProgram
{
    public static class RegexValidation
    {
        private static readonly Regex _regex = new Regex("[^0-9]+");

        public static bool IsNumeric(string text)
        {
            return _regex.IsMatch(text);
        }
    }
}
