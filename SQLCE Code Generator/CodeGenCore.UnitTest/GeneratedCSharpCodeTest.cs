using System.Diagnostics;
using ChristianHelle.DatabaseTools.SqlCe.CodeGenCore.UnitTest.Properties;
using ChristianHelle.DatabaseTools.SqlCe.CodeGenCustomTool;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore.UnitTest
{
    [TestClass]
    public class GeneratedCSharpCodeTest : CodeGenBaseTest
    {
        [TestMethod]
        public void EntitiesCodeCanCompileTest()
        {
            var database = GetDatabase();
            var factory = new CodeGeneratorFactory(database);

            var codeGenerator = factory.Create();
            codeGenerator.GenerateEntities();

            AssertCompile(codeGenerator.GetCode());
        }

        private static SqlCeDatabase GetDatabase()
        {
            var defaultNamespace = typeof(CodeGenTest).Namespace;
            var connectionString = Settings.Default.TestDatabaseConnectionString;
            return new SqlCeDatabase(defaultNamespace, connectionString);
        }

        [TestMethod]
        public void DataAccessCodeCanCompileTest()
        {
            var database = GetDatabase();
            var factory = new CodeGeneratorFactory(database);

            var codeGenerator = factory.Create();
            codeGenerator.GenerateEntities();           
            codeGenerator.GenerateDataAccessLayer();

            AssertCompile(codeGenerator.GetCode());
        }
        
        [TestMethod]
        public void EntityUnitTestCodeCanCompileTest()
        {
            var database = GetDatabase();
            var factory = new CodeGeneratorFactory(database);

            var codeGenerator = factory.Create();
            codeGenerator.GenerateEntities();

            var unitTestCodeGenerator = new CSharpUnitTestCodeGenerator(database);
            unitTestCodeGenerator.GenerateEntities();

            AssertCompile(codeGenerator.GetCode(), unitTestCodeGenerator.GetCode());
        }

        [TestMethod]
        public void DataAccessUnitTestCodeCanCompileTest()
        {
            var database = GetDatabase();
            var factory = new CodeGeneratorFactory(database);

            var codeGenerator = factory.Create();
            codeGenerator.GenerateEntities();
            codeGenerator.GenerateDataAccessLayer();

            var unitTestCodeGenerator = new CSharpUnitTestCodeGenerator(database);
            unitTestCodeGenerator.GenerateEntities();
            unitTestCodeGenerator.GenerateDataAccessLayer();

            AssertCompile(codeGenerator.GetCode(), unitTestCodeGenerator.GetCode());
        }

        [TestMethod]
        public void AllGeneratedCodeCanCompileTest()
        {
            var database = GetDatabase();
            var factory = new CodeGeneratorFactory(database);

            var codeGenerator = factory.Create();
            codeGenerator.WriteHeaderInformation();
            codeGenerator.GenerateEntities();
            codeGenerator.GenerateDataAccessLayer();

            var unitTestCodeGenerator = new CSharpUnitTestCodeGenerator(database);
            unitTestCodeGenerator.WriteHeaderInformation();
            unitTestCodeGenerator.GenerateEntities();
            unitTestCodeGenerator.GenerateDataAccessLayer();

            AssertCompile(codeGenerator.GetCode(), unitTestCodeGenerator.GetCode());
        }

        [TestMethod]
        public void CustomToolGeneratedCodeCanCompileTest()
        {
            var database = GetDatabase();
            var actual = SQLCECodeGenerator.GenerateCode(database.Namespace, "TestDatabase.sdf", ".cs");

            Assert.IsNotNull(actual);
            Assert.AreNotEqual(0, actual.Length);
            AssertCompile(Encoding.Default.GetString(actual));
        }

        private static void AssertCompile(params string[] sourceCode)
        {
            var actual = CodeCompiler.CompileCSharpSource(sourceCode);

            foreach (var error in actual.Errors)
                Trace.WriteLine(error, "ERROR");

            Assert.AreEqual(0, actual.Errors.Count);
        }
    }
}
