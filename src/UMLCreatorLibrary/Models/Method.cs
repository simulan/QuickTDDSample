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
        public string Name { get;private set; }
        public string ReturnType { get; private set; }

        public Method(String input) {
            Decode(input);
        }

        private void Decode(String input) {
            int indexColon = input.IndexOf(':');
            if (indexColon == -1) throw new FormatException("Every method should have a return type seperated by a colon, try ':void' if method returns nothing");
            AccessModifier = DecodeAccessModifier(input[0]);
            Name = DecodeName(input, indexColon);
            ReturnType = DecodeReturnType(input, indexColon);
        }
        private AccessScope DecodeAccessModifier(char c) {
            switch (c) {
                case '+': return AccessScope.PUBLIC;
                case '#': return AccessScope.PROTECTED;
                case '-': return AccessScope.PRIVATE;
                default: throw new NotImplementedException("There is no AccessModifier defined with "+c+", try {+,-,#} prefix");
            }
        }
        private string DecodeName(string input,int indexColon) {
            const int PREFIX_OFFSET = 1;
            string name = input.Substring(PREFIX_OFFSET, indexColon - PREFIX_OFFSET);
            return name.Replace(" ", "");   
        }
        private string DecodeReturnType(string input, int indexColon) {
            const int NEXT_OFFSET = 1;
            string returnType = input.Substring(indexColon + NEXT_OFFSET);
            return returnType.Replace(" ", "");
        }

        public enum AccessScope {
            PUBLIC, PROTECTED, PRIVATE
        }
    }
}
