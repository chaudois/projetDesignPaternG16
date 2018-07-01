using System;
using System.Collections.Generic;
using BO;
using DAL;
using DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
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
        [TestMethod]
        public void should_export_csv()
        {
            List<ContactDTO> listToExport = new List<ContactDTO>
            {
                new ContactDTO
                {
                    id=0,
                    firstName="firstnametest1",
                    lastName="lastnametest1",
                    fields=new List<FieldDTO>
                    {
                        new FieldDTO
                        {
                            idContact=0,
                            name="fieldtest1",
                            value="valuetest1"
                        },
                        new FieldDTO
                        {
                            idContact=0,
                            name="fieldtest2",
                            value="valuetest2"
                        },
                        new FieldDTO
                        {
                            idContact=0,
                            name="fieldtest3",
                            value="valuetest3"
                        }

                    }
                },new ContactDTO
                {
                    id=1,
                    firstName="firstnametest2",
                    lastName="lastnametest2",
                    fields=new List<FieldDTO>
                    {
                        new FieldDTO
                        {
                            idContact=0,
                            name="fieldtest1",
                            value="valuetest4"
                        },
                        new FieldDTO
                        {
                            idContact=0,
                            name="fieldtest2",
                            value="valuetest5"
                        },
                        new FieldDTO
                        {
                            idContact=0,
                            name="fieldtest3",
                            value="valuetest6"
                        }

                    }
                }
            };
            new Export(new CSVWork(), listToExport).ExportContact();
            using (System.IO.StreamReader sr = new System.IO.StreamReader(@"C:\Users\d.chaudois\source\repos\projetDesignPatern4AL1G16\test\bin\Debug\exportCSV-" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt"))
            {
                string dataToImport = sr.ReadToEnd();
                Assert.IsTrue(dataToImport == "firstName,lastName,fieldtest1,fieldtest2,fieldtest3\nfirstnametest1,lastnametest1,valuetest1,valuetest2,valuetest3\nfirstnametest2,lastnametest2,valuetest4,valuetest5,valuetest6\n");
            }


        }
        [TestMethod]
        public void should_export_json()
        {
            List<ContactDTO> listToExport = new List<ContactDTO>
            {
                new ContactDTO
                {
                    id=0,
                    firstName="firstnametest1",
                    lastName="lastnametest1",
                    fields=new List<FieldDTO>
                    {
                        new FieldDTO
                        {
                            idContact=0,
                            name="fieldtest1",
                            value="valuetest1"
                        },
                        new FieldDTO
                        {
                            idContact=0,
                            name="fieldtest2",
                            value="valuetest2"
                        },
                        new FieldDTO
                        {
                            idContact=0,
                            name="fieldtest3",
                            value="valuetest3"
                        }

                    }
                },new ContactDTO
                {
                    id=1,
                    firstName="firstnametest2",
                    lastName="lastnametest2",
                    fields=new List<FieldDTO>
                    {
                        new FieldDTO
                        {
                            idContact=0,
                            name="fieldtest1",
                            value="valuetest4"
                        },
                        new FieldDTO
                        {
                            idContact=0,
                            name="fieldtest2",
                            value="valuetest5"
                        },
                        new FieldDTO
                        {
                            idContact=0,
                            name="fieldtest3",
                            value="valuetest6"
                        }

                    }
                }
            };
            new Export(new JSONWork(), listToExport).ExportContact();
            using (System.IO.StreamReader sr = new System.IO.StreamReader(@"C:\Users\d.chaudois\source\repos\projetDesignPatern4AL1G16\test\bin\Debug\exportJSON-" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt"))
            {
                string dataToImport = sr.ReadToEnd();
                Assert.IsTrue(dataToImport == "[{\"firstName\":\"firstnametest1\",\"lastName\":\"lastnametest1\",\"fields\":[{\"name\":\"fieldtest1\",\"value\":\"valuetest1\",\"idContact\":\"0\"},{\"name\":\"fieldtest2\",\"value\":\"valuetest2\",\"idContact\":\"0\"},{\"name\":\"fieldtest3\",\"value\":\"valuetest3\",\"idContact\":\"0\"}]},{\"firstName\":\"firstnametest2\",\"lastName\":\"lastnametest2\",\"fields\":[{\"name\":\"fieldtest1\",\"value\":\"valuetest4\",\"idContact\":\"0\"},{\"name\":\"fieldtest2\",\"value\":\"valuetest5\",\"idContact\":\"0\"},{\"name\":\"fieldtest3\",\"value\":\"valuetest6\",\"idContact\":\"0\"}]}]");
            }


        }
        [TestMethod]
        public void should_save_and_remove_new_contact()
        {
            new ContactSQL().Add(new ContactDTO
            {
                firstName = "testinsertfirstaname",
                lastName = "testinsertlastname",
                fields = new List<FieldDTO>
                {
                    new FieldDTO
                    {
                        name="testfield1",
                        value="testvalue1"
                    }
                }
            });
            var lastContactAdded = new ContactSQL().GetAll().OrderBy(e=>e.id).Single();

            Assert.IsTrue(lastContactAdded.firstName == "testinsertfirstaname");
            Assert.IsTrue(lastContactAdded.lastName == "testinsertlastname");
            Assert.IsTrue(lastContactAdded.fields[0].name == "testfield1");
            Assert.IsTrue(lastContactAdded.fields[0].value == "testvalue1");
            new ContactSQL().remove(lastContactAdded);
            var allContact = new ContactSQL().GetAll();
            Assert.IsTrue(allContact.Count()==0);

        }

    }
}
