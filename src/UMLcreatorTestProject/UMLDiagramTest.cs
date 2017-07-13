using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UMLcreator.models;
using System.Collections.Generic;
using UMLCreatorLibrary.Models;
using UMLCreatorLibrary.Models.Decoders;

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
        public void testGetMethods() {
            const string NAME = "name";
            Diagram diagram = new Diagram(NAME);
            MethodDecoder decoder = new MethodDecoder();
            diagram.AddMethod(decoder.Decode("+GetPropertyA() : int"));
            diagram.AddMethod(decoder.Decode("-SetPropertyA() : void"));
            diagram.AddMethod(decoder.Decode("+DoSomething() : void"));
            Assert.IsTrue(diagram.Methods != null && diagram.Methods.Count != 0);
        }
        [TestMethod]
        public void testAddMethodShouldThrowExceptionWhenInvalidAccessModifier() {
            const string NAME = "name";
            Diagram diagram = new Diagram(NAME);
            MethodDecoder decoder = new MethodDecoder();
            try {
                diagram.AddMethod(decoder.Decode("func(var:type):void"));
                Assert.Inconclusive("Should've thrown exception");
            } catch (AssertInconclusiveException exception) {
                Assert.Fail(exception.Message);
            } catch (Exception) {
                Assert.IsTrue(true);
            }
        }
        [TestMethod]
        public void testAddMethodShouldThrowExceptionWhenInvalidName() {
            const string NAME = "name";
            Diagram diagram = new Diagram(NAME);
            MethodDecoder decoder = new MethodDecoder();
            try {
                diagram.AddMethod(decoder.Decode("++(var:type):void"));
                Assert.Inconclusive("Should've thrown exception");
            } catch (AssertInconclusiveException exception) {
                Assert.Fail(exception.Message);
            } catch (Exception) {
                Assert.IsTrue(true);
            }
        }
        [TestMethod]
        public void testGetMethodsShouldThrowExceptionWhenNoName() {
            const string NAME = "name";
            Diagram diagram = new Diagram(NAME);
            MethodDecoder decoder = new MethodDecoder();
            try {
                diagram.AddMethod(decoder.Decode("+(var:type):void"));
                Assert.Inconclusive("Should've thrown exception");
            } catch (AssertInconclusiveException exception) {
                Assert.Fail(exception.Message);
            } catch (Exception) {
                Assert.IsTrue(true);
            }
        }
        [TestMethod]
        public void testGetMethodsShouldThrowExceptionWhenNoParameterName() {
            const string NAME = "name";
            Diagram diagram = new Diagram(NAME);
            MethodDecoder decoder = new MethodDecoder();
            try {
                diagram.AddMethod(decoder.Decode("+(:type):void"));
                Assert.Inconclusive("Should've thrown exception");
            } catch (AssertInconclusiveException exception) {
                Assert.Fail(exception.Message);
            } catch (Exception) {
                Assert.IsTrue(true);
            }
        }
        [TestMethod]
        public void testGetMethodsShouldThrowExceptionWhenNoParameterType() {
            const string NAME = "name";
            Diagram diagram = new Diagram(NAME);
            MethodDecoder decoder = new MethodDecoder();
            try {
                diagram.AddMethod(decoder.Decode("+(var:):void"));
                Assert.Inconclusive("Should've thrown exception");
            } catch (AssertInconclusiveException exception) {
                Assert.Fail(exception.Message);
            } catch (Exception) {
                Assert.IsTrue(true);
            }
        }
        [TestMethod]
        public void testGetMethodProperties() {
            const string NAME = "name";
            Diagram diagram = new Diagram(NAME);
            MethodDecoder decoder = new MethodDecoder();

            diagram.AddMethod(decoder.Decode("+GetPropertyA() : int"));
            diagram.AddMethod(decoder.Decode("-SetPropertyA() : void"));
            diagram.AddMethod(decoder.Decode("+DoSomething() : void"));
            diagram.AddMethod(decoder.Decode("+DoSomething2(amount:Int) : void"));
            diagram.AddMethod(decoder.Decode("#DoSomething3(number:Int, flag:Boolean) : void"));

            List<Method> methods = diagram.Methods;
            Assert.IsFalse(methods[0].AccessModifier.Equals(AccessScope.PRIVATE));
            Assert.IsTrue(methods[1].Name.Equals("SetPropertyA"));
            Assert.IsTrue(methods[2].ReturnType.Equals("void"));
            Assert.IsNotNull(methods[3].Parameters);
            Assert.IsTrue(methods[4].Parameters.Count==2);
        }

    }
}
