using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMLCreatorLibrary.Models {
    /**
     * Method String codes :
     * 
     * First Char -> Access Modifier :
     * + public
     * - private
     * # protected
     * 
     * First ':' char -> Name & Return type delimiter
     * left = Name of the method
     * right = Return type of the method
     * 
     */
    public class Method {
        public AccessScope AccessModifier { get; private set; }
        public string Name { get; private set; }
        public string ReturnType { get; private set; }
        public List<Argument> Parameters { get; private set; }

        public Method(String input) {
            Decode(input);
        }

        private void Decode(String input) {
            int indexLastColon = FindLastIndexOrThrowError(input, ':');
            int indexFirstParenthesis = FindIndexOrThrowError(input, '(');
            int indexLastParenthesis = FindIndexOrThrowError(input, ')');
            bool hasTwoParenthesis = FindAmountOfCharInstancesWithin(input, '(') == 1 && FindAmountOfCharInstancesWithin(input, ')') == 1;

            if (indexLastColon < indexLastParenthesis) {
                throw new FormatException("A UML method notation must have a return type defined after last parenthesis");
            }

            if (hasTwoParenthesis) {
                AccessModifier = DecodeAccessModifier(input[0]);
                Name = DecodeName(input, indexFirstParenthesis);
                Parameters = DecodeParameters(input,indexFirstParenthesis, indexLastParenthesis);
                ReturnType = DecodeReturnType(input, indexLastColon);
            } else {
                throw new FormatException("A UML method notation must have 1 opening and 1 closing parenthesis");
            }
        }
        private int FindAmountOfCharInstancesWithin(String text, Char c) {
            int amountOfCharInstances = 0;
            Char[] chars = text.ToCharArray();
            foreach (Char iteratedChar in chars) {
                if (iteratedChar == c) amountOfCharInstances++;
            }
            return amountOfCharInstances;
        }
        private int FindIndexOrThrowError(string chars, char c) {
            int indexOf = chars.IndexOf(c);
            if (indexOf == -1) throw new FormatException(c+" character expected in "+chars+" , follow UML method notation");
            return indexOf;
        }
        private int FindLastIndexOrThrowError(string chars, char c) {
            int indexOf = chars.LastIndexOf(':');
            if (indexOf == -1) throw new FormatException(c + " character expected in " + chars + " , follow UML method notation");
            return indexOf;
        }
        private AccessScope DecodeAccessModifier(char c) {
            switch (c) {
                case '+': return AccessScope.PUBLIC;
                case '#': return AccessScope.PROTECTED;
                case '-': return AccessScope.PRIVATE;
                default: throw new NotImplementedException("There is no AccessModifier defined with " + c + ", try {+,-,#} prefix");
            }
        }
        private string DecodeName(string input, int indexFirstParenthesis) {
            const int PREFIX_OFFSET = 1;
            int argsLength = input.Length - indexFirstParenthesis;
            string name = input.Substring(PREFIX_OFFSET, input.Length - argsLength - PREFIX_OFFSET);
            return name.Replace(" ", "");
        }
        private List<Argument> DecodeParameters(string input, int indexFirstParenthesis, int indexLastParenthesis) {
            const int SINGLE_OFFSET = 1;
            List<Argument> arguments = new List<Argument>();
            String argumentsInput = input.Substring(indexFirstParenthesis+SINGLE_OFFSET, indexLastParenthesis - indexFirstParenthesis - SINGLE_OFFSET);
            foreach (String part in argumentsInput.Split(',')) {
                if (part.Trim() != "") {
                    arguments.Add(DecodeParameter(part));
                }
            }
            return arguments;
        }
        private Argument DecodeParameter(string input) {
            Argument arg = new Argument();
            String[] parts = input.Split(':');
            if (parts.Count() == 2) {
                arg.Name = parts[0];
                arg.ReturnType = parts[1];
                return arg;
            } else {
                throw new NotImplementedException("Parameters should have name + colon + returntype");
            }

        }
        private string DecodeReturnType(string input, int indexColon) {
            const int NEXT_OFFSET = 1;
            string returnType = input.Substring(indexColon + NEXT_OFFSET);
            return returnType.Replace(" ", "");
        }
    }
}
