using ChristianHelle.DatabaseTools.SqlCe.CodeGenCore.UnitTest.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore.UnitTest
{
    [TestClass]
    public class GeneratedCSharpCodeTest : CodeGenBaseTest
    {
        [TestMethod]
        public void EntitiesCodeCanCompileTest()
        {
            var defaultNamespace = typeof(CodeGenTest).Namespace;
            var connectionString = Settings.Default.TestDatabaseConnectionString;
            var database = new SqlCeDatabase(defaultNamespace, connectionString);
            var factory = new CodeGeneratorFactory(database);
            var codeGenerator = factory.Create();
            codeGenerator.GenerateEntities();

            var actual = CodeCompiler.CompileCSharpSource(codeGenerator.GetCode());
            Assert.AreEqual(0, actual.Errors.Count);
        }

        [TestMethod]
        public void DataAccessCodeCanCompileTest()
        {
            var defaultNamespace = typeof(CodeGenTest).Namespace;
            var connectionString = Settings.Default.TestDatabaseConnectionString;
            var database = new SqlCeDatabase(defaultNamespace, connectionString);
            var factory = new CodeGeneratorFactory(database);
            var codeGenerator = factory.Create();
            codeGenerator.GenerateEntities();           
            codeGenerator.GenerateDataAccessLayer();

            var actual = CodeCompiler.CompileCSharpSource(codeGenerator.GetCode());
            Assert.AreEqual(0, actual.Errors.Count);
        }
        
        [TestMethod]
        public void EntityUnitTestCodeCanCompileTest()
        {
            var defaultNamespace = typeof(CodeGenTest).Namespace;
            var connectionString = Settings.Default.TestDatabaseConnectionString;
            var database = new SqlCeDatabase(defaultNamespace, connectionString);
            
            var factory = new CodeGeneratorFactory(database);
            var codeGenerator = factory.Create();
            codeGenerator.GenerateEntities();

            var unitTestCodeGenerator = new CSharpUnitTestCodeGenerator(database);
            unitTestCodeGenerator.GenerateEntities();

            var actual = CodeCompiler.CompileCSharpSource(codeGenerator.GetCode(), unitTestCodeGenerator.GetCode());
            Assert.AreEqual(0, actual.Errors.Count);
        }

        [TestMethod]
        public void DataAccessUnitTestCodeCanCompileTest()
        {
            var defaultNamespace = typeof(CodeGenTest).Namespace;
            var connectionString = Settings.Default.TestDatabaseConnectionString;
            var database = new SqlCeDatabase(defaultNamespace, connectionString);

            var factory = new CodeGeneratorFactory(database);
            var codeGenerator = factory.Create();
            codeGenerator.GenerateEntities();

            var unitTestCodeGenerator = new CSharpUnitTestCodeGenerator(database);
            unitTestCodeGenerator.GenerateDataAccessLayer();

            var actual = CodeCompiler.CompileCSharpSource(codeGenerator.GetCode());
            Assert.AreEqual(0, actual.Errors.Count);
        }
    }
}
