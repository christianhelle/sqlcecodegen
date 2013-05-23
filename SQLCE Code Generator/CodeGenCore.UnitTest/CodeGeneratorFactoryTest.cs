using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore.UnitTest
{
    [TestClass]
    public class CodeGeneratorFactoryTest : CodeGenBaseTest
    {
        [TestMethod]
        public void CodeGeneratorFactoryCreateTest()
        {
            var defaultNamespace = typeof(CodeGenTest).Namespace;
            var connectionString = "Data Source=Northwind.sdf";
            var database = SqlCeDatabaseFactory.Create(defaultNamespace, connectionString);
            var factory = new CodeGeneratorFactory(database);
            var codeGenerator = factory.Create();

            Assert.IsNotNull(codeGenerator);
            Assert.IsInstanceOfType(codeGenerator, typeof(CodeGenerator));
            Assert.IsInstanceOfType(codeGenerator, typeof(CSharpCodeGenerator));
        }

        [TestMethod]
        public void CodeGeneratorFactoryStaticCreateTest()
        {
            var defaultNamespace = typeof(CodeGenTest).Namespace;
            var connectionString = "Data Source=Northwind.sdf";
            var database = SqlCeDatabaseFactory.Create(defaultNamespace, connectionString);
            var codeGenerator = CodeGeneratorFactory.Create(database);

            Assert.IsNotNull(codeGenerator);
            Assert.IsInstanceOfType(codeGenerator, typeof(CodeGenerator));
            Assert.IsInstanceOfType(codeGenerator, typeof(CSharpCodeGenerator));
        }

        [TestMethod]
        public void CodeGeneratorFactoryCreateVisualBasicTest()
        {
            var defaultNamespace = typeof(CodeGenTest).Namespace;
            var connectionString = "Data Source=Northwind.sdf";
            var database = SqlCeDatabaseFactory.Create(defaultNamespace, connectionString);
            var factory = new CodeGeneratorFactory(database);
            var codeGenerator = factory.Create("VB");

            Assert.IsNotNull(codeGenerator);
            Assert.IsInstanceOfType(codeGenerator, typeof(CodeGenerator));
            //Assert.IsInstanceOfType(codeGenerator, typeof(VisualBasicCodeGenerator));
        }

        [TestMethod]
        public void CodeGeneratorFactoryStaticCreateVisualBasicTest()
        {
            var defaultNamespace = typeof(CodeGenTest).Namespace;
            var connectionString = "Data Source=Northwind.sdf";
            var database = SqlCeDatabaseFactory.Create(defaultNamespace, connectionString);
            var codeGenerator = CodeGeneratorFactory.Create(database, "VB");

            Assert.IsNotNull(codeGenerator);
            Assert.IsInstanceOfType(codeGenerator, typeof(CodeGenerator));
            //Assert.IsInstanceOfType(codeGenerator, typeof(VisualBasicCodeGenerator));
        }

        [TestMethod]
        public void CodeGeneratorFactoryCreateCsharpTest()
        {
            var defaultNamespace = typeof(CodeGenTest).Namespace;
            var connectionString = "Data Source=Northwind.sdf";
            var database = SqlCeDatabaseFactory.Create(defaultNamespace, connectionString);
            var factory = new CodeGeneratorFactory(database);
            var codeGenerator = factory.Create("CSharp");

            Assert.IsNotNull(codeGenerator);
            Assert.IsInstanceOfType(codeGenerator, typeof(CodeGenerator));
            Assert.IsInstanceOfType(codeGenerator, typeof(CSharpCodeGenerator));
        }

        [TestMethod]
        public void CodeGeneratorFactoryStaticCreateCsharpTest()
        {
            var defaultNamespace = typeof(CodeGenTest).Namespace;
            var connectionString = "Data Source=Northwind.sdf";
            var database = SqlCeDatabaseFactory.Create(defaultNamespace, connectionString);
            var codeGenerator = CodeGeneratorFactory.Create(database, "CSharp");

            Assert.IsNotNull(codeGenerator);
            Assert.IsInstanceOfType(codeGenerator, typeof(CodeGenerator));
            Assert.IsInstanceOfType(codeGenerator, typeof(CSharpCodeGenerator));
        }
    }
}