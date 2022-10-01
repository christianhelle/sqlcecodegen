using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore.UnitTest
{
    [TestClass]
    public class UnitTestCodeGeneratorFactoryTest : CodeGenBaseTest
    {
        [TestMethod]
        public void UnitTestCodeGeneratorFactoryCreateTest()
        {
            var defaultNamespace = typeof(CodeGenTest).Namespace;
            var connectionString = "Data Source=TestDatabase.sdf";
            var database = new SqlCeDatabase(defaultNamespace, connectionString);
            var factory = new UnitTestCodeGeneratorFactory(database);
            var codeGenerator = factory.Create();

            Assert.IsNotNull(codeGenerator);
            Assert.IsInstanceOfType(codeGenerator, typeof(CodeGenerator));
            Assert.IsInstanceOfType(codeGenerator, typeof(CSharpCodeGenerator));
            Assert.IsInstanceOfType(codeGenerator, typeof(CSharpUnitTestCodeGenerator));
            Assert.IsInstanceOfType(codeGenerator, typeof(MsTestUnitTestCodeGenerator));
        }

        [TestMethod]
        public void UnitTestCodeGeneratorFactoryStaticCreateTest()
        {
            var defaultNamespace = typeof(CodeGenTest).Namespace;
            var connectionString = "Data Source=TestDatabase.sdf";
            var database = new SqlCeDatabase(defaultNamespace, connectionString);
            var codeGenerator = UnitTestCodeGeneratorFactory.Create(database);

            Assert.IsNotNull(codeGenerator);
            Assert.IsInstanceOfType(codeGenerator, typeof(CodeGenerator));
            Assert.IsInstanceOfType(codeGenerator, typeof(CSharpCodeGenerator));
            Assert.IsInstanceOfType(codeGenerator, typeof(CSharpUnitTestCodeGenerator));
            Assert.IsInstanceOfType(codeGenerator, typeof(MsTestUnitTestCodeGenerator));
        }

        [TestMethod]
        public void UnitTestCodeGeneratorFactoryCreateMSTestTest()
        {
            var defaultNamespace = typeof(CodeGenTest).Namespace;
            var connectionString = "Data Source=TestDatabase.sdf";
            var database = new SqlCeDatabase(defaultNamespace, connectionString);
            var factory = new UnitTestCodeGeneratorFactory(database);
            var codeGenerator = factory.Create("MSTest");

            Assert.IsNotNull(codeGenerator);
            Assert.IsInstanceOfType(codeGenerator, typeof(CodeGenerator));
            Assert.IsInstanceOfType(codeGenerator, typeof(CSharpCodeGenerator));
            Assert.IsInstanceOfType(codeGenerator, typeof(CSharpUnitTestCodeGenerator));
            Assert.IsInstanceOfType(codeGenerator, typeof(MsTestUnitTestCodeGenerator));
        }

        [TestMethod]
        public void UnitTestCodeGeneratorFactoryCreateNUnitTest()
        {
            var defaultNamespace = typeof(CodeGenTest).Namespace;
            var connectionString = "Data Source=TestDatabase.sdf";
            var database = new SqlCeDatabase(defaultNamespace, connectionString);
            var factory = new UnitTestCodeGeneratorFactory(database);
            var codeGenerator = factory.Create("NUnit");

            Assert.IsNotNull(codeGenerator);
            Assert.IsInstanceOfType(codeGenerator, typeof(CodeGenerator));
            Assert.IsInstanceOfType(codeGenerator, typeof(CSharpCodeGenerator));
            Assert.IsInstanceOfType(codeGenerator, typeof(CSharpUnitTestCodeGenerator));
            Assert.IsInstanceOfType(codeGenerator, typeof(NUnitTestCodeGenerator));
        }

        [TestMethod]
        public void UnitTestCodeGeneratorFactoryStaticCreateMSTestTest()
        {
            var defaultNamespace = typeof(CodeGenTest).Namespace;
            var connectionString = "Data Source=TestDatabase.sdf";
            var database = new SqlCeDatabase(defaultNamespace, connectionString);
            var codeGenerator = UnitTestCodeGeneratorFactory.Create(database, "MSTest");

            Assert.IsNotNull(codeGenerator);
            Assert.IsInstanceOfType(codeGenerator, typeof(CodeGenerator));
            Assert.IsInstanceOfType(codeGenerator, typeof(CSharpCodeGenerator));
            Assert.IsInstanceOfType(codeGenerator, typeof(CSharpUnitTestCodeGenerator));
            Assert.IsInstanceOfType(codeGenerator, typeof(MsTestUnitTestCodeGenerator));
        }

        [TestMethod]
        public void UnitTestCodeGeneratorFactoryStaticCreateNUnitTest()
        {
            var defaultNamespace = typeof(CodeGenTest).Namespace;
            var connectionString = "Data Source=TestDatabase.sdf";
            var database = new SqlCeDatabase(defaultNamespace, connectionString);
            var codeGenerator = UnitTestCodeGeneratorFactory.Create(database, "NUnit");

            Assert.IsNotNull(codeGenerator);
            Assert.IsInstanceOfType(codeGenerator, typeof(CodeGenerator));
            Assert.IsInstanceOfType(codeGenerator, typeof(CSharpCodeGenerator));
            Assert.IsInstanceOfType(codeGenerator, typeof(CSharpUnitTestCodeGenerator));
            Assert.IsInstanceOfType(codeGenerator, typeof(NUnitTestCodeGenerator));
        }
    }
}