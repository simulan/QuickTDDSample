using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMLCreatorLibrary.Models {
    /**
     * Class String codes :
     * 
     * First Char -> Access Modifier :
     * + public
     * - private
     * # protected
     * 
     * Following characters : Name
     * 
     * First ( begin arguments
     * Last ) end arguments
     */
    public class Class {
        public AccessScope AccessModifier { get; set; }
        public string Name { get; set; }
        public List<Method> Methods { get; set; }
        public List<Argument> Parameters { get; set; }

        public Class(String name) {
            this.Name = name;
            Methods = new List<Method>();
        }
        public Class(AccessScope accessModifier, String name, List<Argument> parameters) {
            this.AccessModifier = accessModifier;
            this.Name = name;
            this.Parameters = parameters;
        }

        public void AddMethod(Method method) {
            Methods.Add(method);
        }
    }
}
