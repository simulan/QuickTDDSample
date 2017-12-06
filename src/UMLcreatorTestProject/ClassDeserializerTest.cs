using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMLCreatorLibrary.Models;
using UMLCreatorLibrary.Models.Deserializers;

namespace UMLcreatorTestProject {
    [TestClass]
    public class ClassDeserializerTest {
        [TestMethod]
        public void deserializeShouldThrowOnNoAccessModifier() {
            UML uml = new UML();
            try {
                uml.Deserialize<Class>("a(var:type)");
                Assert.Inconclusive("Should've thrown exception");
            } catch (AssertInconclusiveException exception) {
                Assert.Fail(exception.Message);
            } catch (Exception) {
                Assert.IsTrue(true);
            }
        }
    }
}
