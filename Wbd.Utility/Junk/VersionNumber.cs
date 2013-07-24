using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Wbd
{
    public class VersionNumber
    {
        private string originalString;

        public double ToDouble()
        {
            StringBuilder parsable = new StringBuilder();
            bool foundDecimal = false;
            for (int c = 0; c < originalString.Length; c++)
            {
                if (Char.IsDigit(originalString[c]))
                    parsable.Append(originalString[c]);
                else if (foundDecimal == false && originalString[c] == '.')
                {
                    parsable.Append('.');
                    foundDecimal = true;
                }
            }
            return Double.Parse(parsable.ToString());
        }

        public string ToString()
        {
            return originalString;
        }

        public VersionNumber(string originalString)
        {
            if (originalString == null)
                throw new ArgumentNullException();
            if (!Regex.IsMatch(originalString, "[0-9]+"))
                throw new ArgumentException(
                    "The version number must contain a digit.");
            this.originalString = originalString.Trim();
        }
    }
}
