using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UMLCreatorLibrary.Models;

namespace UMLcreator.models
{
    public class Diagram
    {
        public List<Class> Classes { get;private set; }

        public Diagram() {
            Classes = new List<Class>();
        }
        public void AddClass(Class addition) {
            Classes.Add(addition);
        }
    }
}
