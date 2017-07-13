using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMLCreatorLibrary.Models.Decoders;

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
        public AccessScope AccessModifier { get; set; }
        public string Name { get; set; }
        public string ReturnType { get; set; }
        public List<Argument> Parameters { get; set; }

        public Method(AccessScope scope,string name,List<Argument> args,string returnType) {
            AccessModifier = scope;
            Name = name;
            Parameters = args;
            ReturnType = returnType;
        }

    }
}
