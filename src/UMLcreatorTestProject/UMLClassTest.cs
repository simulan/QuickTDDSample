using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UMLcreator.models;
using System.Collections.Generic;
using UMLCreatorLibrary.Models;
using UMLCreatorLibrary.Models.Deserializers;

namespace UMLcreatorTestProject {
    [TestClass]
    public class UMLClassTest {
        [TestMethod]
        public void testGetName() {
            const string NAME = "name";
            Class @class = new Class(NAME);
            Assert.IsTrue(@class.Name.Equals(NAME));
        }
        [TestMethod]
        public void testGetMethods() {
            const string NAME = "name";
            Class table = new Class(NAME);
            UML uml = new UML();
            table.AddMethod(uml.Deserialize<Method>("+GetPropertyA() : int"));
            table.AddMethod(uml.Deserialize<Method>("-SetPropertyA() : void"));
            table.AddMethod(uml.Deserialize<Method>("+DoSomething() : void"));
            Assert.IsTrue(table.Methods != null && table.Methods.Count != 0);
        }
        [TestMethod]
        public void testGetMethodProperties() {
            const string NAME = "name";
            Class @class = new Class(NAME);
            UML uml = new UML();
            @class.AddMethod(uml.Deserialize<Method>("+GetPropertyA() : int"));
            @class.AddMethod(uml.Deserialize<Method>("-SetPropertyA() : void"));
            @class.AddMethod(uml.Deserialize<Method>("+DoSomething() : void"));
            @class.AddMethod(uml.Deserialize<Method>("+DoSomething2(amount:Int) : void"));
            @class.AddMethod(uml.Deserialize<Method>("#DoSomething3(number:Int, flag:Boolean) : void"));

            List<Method> methods = @class.Methods;
            Assert.IsFalse(methods[0].AccessModifier.Equals(AccessScope.PRIVATE));
            Assert.IsTrue(methods[1].Name.Equals("SetPropertyA"));
            Assert.IsTrue(methods[2].ReturnType.Equals("void"));
            Assert.IsNotNull(methods[3].Parameters);
            Assert.IsTrue(methods[4].Parameters.Count == 2);
        }

        
    }
}
