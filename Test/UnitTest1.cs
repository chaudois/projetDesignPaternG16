using System;
using BO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void should_import_CSV()
        {
            using (System.IO.StreamReader sr = new System.IO.StreamReader(@"C:\Users\d.chaudois\source\repos\projetDesignPatern4AL1G16\projetDesignPatern4AL1G16\bin\Debug\exportCSV-01-07-2018.txt"))
            {
                string importedData = sr.ReadToEnd();
                var result = new Import(importedData).ImportContact();
                Assert.AreEqual(3, result.Count);
                Assert.AreEqual("Chaudois", result[2].lastName);
                Assert.AreEqual("nissan micra", result[2].fields[2].value);
            }

        }
        [TestMethod]
        public void should_import_JSON()
        {

            using (System.IO.StreamReader sr = new System.IO.StreamReader(@"C:\Users\d.chaudois\source\repos\projetDesignPatern4AL1G16\projetDesignPatern4AL1G16\bin\Debug\exportJSON-01-07-2018.txt"))
            {
                string importedData = sr.ReadToEnd();
                var result = new Import(importedData).ImportContact();
                Assert.AreEqual(3, result.Count);
                Assert.AreEqual("Chaudois", result[2].lastName);
                Assert.AreEqual("nissan micra", result[2].fields[2].value);
            }
        }
    }
}
