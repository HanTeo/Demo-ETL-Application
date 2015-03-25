using NUnit.Framework;
using System.Linq;

namespace FileImporter.Tests
{
    [TestFixture]
    public class Test
    {
        private string filePath;

        [SetUp]
        public void Setup()
        {
            filePath = string.Format("{0}/{1}", System.Environment.CurrentDirectory, "TestFlatFile");
        }

        [Test]
        public void ImportOnlyRowsWhereField_Type_D()
        {
            var importer = new FlatFileImporter();
            var importedData = importer.Import(filePath, "Type", "D");
            Assert.AreEqual(85, importedData.Item2.Count());
        }

        [Test]
        public void ImportOnlyRowsWhereField_Type_Q()
        {
            var importer = new FlatFileImporter();
            var importedData = importer.Import(filePath, "Type", "Q");
            Assert.AreEqual(18, importedData.Item2.Count());
        }

        [Test]
        public void ImportAllRowsWithoutFilter()
        {
            var importer = new FlatFileImporter();
            var importedData = importer.Import(filePath);
            Assert.AreEqual(18 + 85, importedData.Item2.Count());
        }
    }
}