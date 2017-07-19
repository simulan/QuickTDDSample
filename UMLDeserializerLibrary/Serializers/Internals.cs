using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UMLSerializerLibrary.Serializers {
    //UMLContract would be more descriptive name
    class Internals {
        internal const char ACCESS_PUBLIC = '+';
        internal const char ACCESS_PROTECTED = '#';
        internal const char ACCESS_PRIVATE = '-';
        internal const char PARAMETER_START_DELIMITER = '(';
        internal const char PARAMETER_END_DELIMITER = ')';
        internal const char VARIABLES_DELIMITER = ',';
        internal const char TYPE_DELIMITER = ':';


        internal static void ValidateString(string input, string errorMsg) {
            if (input.Equals("")) {
                throw new FormatException(errorMsg);
            } else if (!IsAlpha(input)) {
                throw new FormatException(String.Format("'{0}' should contain no special chars. {1}", input, errorMsg));
            }
        }
        internal static bool IsAlpha(string input) {
            return Regex.IsMatch(input, "^[a-zA-Z1-9]+$");
        }
    }

}
