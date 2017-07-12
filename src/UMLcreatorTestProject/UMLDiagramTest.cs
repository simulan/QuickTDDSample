using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UMLcreator.models;

namespace UMLcreatorTestProject {
    [TestClass]
    public class UMLDiagramTest {
        [TestMethod]
        public void testGetName() {
            const string NAME = "name";
            Diagram diagram = new Diagram(NAME);
            Assert.IsTrue(diagram.Name.Equals(NAME));
        }
        [TestMethod]
        public void testGetFunctions() {
            const string NAME = "name";
            Diagram diagram = new Diagram(NAME);
            diagram.AddFunction("+GetPropertyA : int");
            diagram.AddFunction("-SetPropertyA : void");
            diagram.AddFunction("+DoSomething : void");
            Assert.IsNotNull(diagram.GetFunctions());
        }
    }
}
