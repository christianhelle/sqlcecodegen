using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore.UnitTest
{
    [TestClass]
    public class CodeGenTest : CodeGenBaseTest
    {
        private static SqlCeDatabase GetDatabase(string defaultNamespace, string connectionString)
        {
            var database = new SqlCeDatabase(defaultNamespace, connectionString);
            database.AnalyzeDatabase();
            return database;
        }

        [TestMethod]
        public void DatabaseConstructorTest()
        {
            var defaultNamespace = typeof(CodeGenTest).Namespace;
            //var connectionString = "Data Source=TestDatabase.sdf";
            var target = GetDatabase();

            Assert.AreEqual(defaultNamespace, target.Namespace);
        }

        [TestMethod]
        public void WriteHeaderInformationTest()
        {
            var defaultNamespace = typeof(CodeGenTest).Namespace;
            var connectionString = "Data Source=TestDatabase.sdf";
            var database = GetDatabase(defaultNamespace, connectionString);
            var factory = new CodeGeneratorFactory(database);
            var codeGenerator = factory.Create();
            codeGenerator.WriteHeaderInformation();

            Assert.IsNotNull(codeGenerator.GetCode());
            Assert.IsFalse(string.IsNullOrEmpty(codeGenerator.GetCode()));
        }

        [TestMethod]
        public void GenerateEntitiesTest()
        {
            var defaultNamespace = typeof(CodeGenTest).Namespace;
            var connectionString = "Data Source=TestDatabase.sdf";
            var database = GetDatabase(defaultNamespace, connectionString);
            var factory = new CodeGeneratorFactory(database);
            var codeGenerator = factory.Create();
            codeGenerator.GenerateEntities();

            Assert.IsNotNull(codeGenerator.GetCode());
            Assert.IsFalse(string.IsNullOrEmpty(codeGenerator.GetCode()));
        }

        [TestMethod]
        public void GenerateDataAccessLayerTest()
        {
            var defaultNamespace = typeof(CodeGenTest).Namespace;
            var connectionString = "Data Source=TestDatabase.sdf";
            var database = GetDatabase(defaultNamespace, connectionString);
            var factory = new CodeGeneratorFactory(database);
            var codeGenerator = factory.Create();
            codeGenerator.GenerateDataAccessLayer();

            Assert.IsNotNull(codeGenerator.GetCode());
            Assert.IsFalse(string.IsNullOrEmpty(codeGenerator.GetCode()));
        }
    }
}