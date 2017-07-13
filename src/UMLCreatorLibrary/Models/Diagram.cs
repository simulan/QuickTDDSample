using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UMLCreatorLibrary.Models;

namespace UMLcreator.models
{
    public class Diagram
    {
        public string Name { get; private set; }
        public List<Method> Methods { get;private set; }

        public Diagram(String name) {
            Name = name;
            Methods = new List<Method>();
        }

        public void AddMethod(Method addition) {
            Methods.Add(addition);
        }
    }
}
