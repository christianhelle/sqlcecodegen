#region License
// The MIT License (MIT)
// 
// Copyright (c) 2009 Christian Resma Helle
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
#endregion
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore.UnitTest
{
    [TestClass]
    public class UnitTestCodeGeneratorFactoryTests : CodeGenBaseTest
    {
        [TestMethod]
        public void UnitTestCodeGeneratorFactoryCreateTest()
        {
            var defaultNamespace = typeof(CodeGenTest).Namespace;
            const string connectionString = "Data Source=Northwind.sdf";
            var database = SqlCeDatabaseFactory.Create(defaultNamespace, connectionString);
            var factory = new UnitTestCodeGeneratorFactory(database);
            var codeGenerator = factory.Create();

            Assert.IsNotNull(codeGenerator);
            Assert.IsInstanceOfType(codeGenerator, typeof(CodeGenerator));
            Assert.IsInstanceOfType(codeGenerator, typeof(CSharpCodeGenerator));
            Assert.IsInstanceOfType(codeGenerator, typeof(CSharpUnitTestCodeGenerator));
            Assert.IsInstanceOfType(codeGenerator, typeof(MSTestUnitTestCodeGenerator));
        }

        [TestMethod]
        public void UnitTestCodeGeneratorFactoryStaticCreateTest()
        {
            var defaultNamespace = typeof(CodeGenTest).Namespace;
            const string connectionString = "Data Source=Northwind.sdf";
            var database = SqlCeDatabaseFactory.Create(defaultNamespace, connectionString);
            var codeGenerator = UnitTestCodeGeneratorFactory.Create(database);

            Assert.IsNotNull(codeGenerator);
            Assert.IsInstanceOfType(codeGenerator, typeof(CodeGenerator));
            Assert.IsInstanceOfType(codeGenerator, typeof(CSharpCodeGenerator));
            Assert.IsInstanceOfType(codeGenerator, typeof(CSharpUnitTestCodeGenerator));
            Assert.IsInstanceOfType(codeGenerator, typeof(MSTestUnitTestCodeGenerator));
        }

        [TestMethod]
        public void UnitTestCodeGeneratorFactoryCreateMSTestTest()
        {
            var defaultNamespace = typeof(CodeGenTest).Namespace;
            const string connectionString = "Data Source=Northwind.sdf";
            var database = SqlCeDatabaseFactory.Create(defaultNamespace, connectionString);
            var factory = new UnitTestCodeGeneratorFactory(database);
            var codeGenerator = factory.Create("MSTest");

            Assert.IsNotNull(codeGenerator);
            Assert.IsInstanceOfType(codeGenerator, typeof(CodeGenerator));
            Assert.IsInstanceOfType(codeGenerator, typeof(CSharpCodeGenerator));
            Assert.IsInstanceOfType(codeGenerator, typeof(CSharpUnitTestCodeGenerator));
            Assert.IsInstanceOfType(codeGenerator, typeof(MSTestUnitTestCodeGenerator));
        }

        [TestMethod]
        public void UnitTestCodeGeneratorFactoryStaticCreateMSTestTest()
        {
            var defaultNamespace = typeof(CodeGenTest).Namespace;
            const string connectionString = "Data Source=Northwind.sdf";
            var database = SqlCeDatabaseFactory.Create(defaultNamespace, connectionString);
            var codeGenerator = UnitTestCodeGeneratorFactory.Create(database, "MSTest");

            Assert.IsNotNull(codeGenerator);
            Assert.IsInstanceOfType(codeGenerator, typeof(CodeGenerator));
            Assert.IsInstanceOfType(codeGenerator, typeof(CSharpCodeGenerator));
            Assert.IsInstanceOfType(codeGenerator, typeof(CSharpUnitTestCodeGenerator));
            Assert.IsInstanceOfType(codeGenerator, typeof(MSTestUnitTestCodeGenerator));
        }

        [TestMethod]
        public void UnitTestCodeGeneratorFactoryStaticCreateNUnitTest()
        {
            var defaultNamespace = typeof(CodeGenTest).Namespace;
            const string connectionString = "Data Source=Northwind.sdf";
            var database = SqlCeDatabaseFactory.Create(defaultNamespace, connectionString);
            var codeGenerator = UnitTestCodeGeneratorFactory.Create(database, "NUnit");

            Assert.IsNotNull(codeGenerator);
            Assert.IsInstanceOfType(codeGenerator, typeof(CodeGenerator));
            Assert.IsInstanceOfType(codeGenerator, typeof(CSharpCodeGenerator));
            Assert.IsInstanceOfType(codeGenerator, typeof(CSharpUnitTestCodeGenerator));
            Assert.IsInstanceOfType(codeGenerator, typeof(NUnitTestCodeGenerator));
        }

        [TestMethod]
        public void UnitTestCodeGeneratorFactoryCreateNUnitTest()
        {
            var defaultNamespace = typeof(CodeGenTest).Namespace;
            const string connectionString = "Data Source=Northwind.sdf";
            var database = SqlCeDatabaseFactory.Create(defaultNamespace, connectionString);
            var factory = new UnitTestCodeGeneratorFactory(database);
            var codeGenerator = factory.Create("NUnit");

            Assert.IsNotNull(codeGenerator);
            Assert.IsInstanceOfType(codeGenerator, typeof(CodeGenerator));
            Assert.IsInstanceOfType(codeGenerator, typeof(CSharpCodeGenerator));
            Assert.IsInstanceOfType(codeGenerator, typeof(CSharpUnitTestCodeGenerator));
            Assert.IsInstanceOfType(codeGenerator, typeof(NUnitTestCodeGenerator));
        }

        [TestMethod]
        public void UnitTestCodeGeneratorFactoryStaticCreateXUnitTest()
        {
            var defaultNamespace = typeof(CodeGenTest).Namespace;
            const string connectionString = "Data Source=Northwind.sdf";
            var database = SqlCeDatabaseFactory.Create(defaultNamespace, connectionString);
            var codeGenerator = UnitTestCodeGeneratorFactory.Create(database, "xUnit");

            Assert.IsNotNull(codeGenerator);
            Assert.IsInstanceOfType(codeGenerator, typeof(CodeGenerator));
            Assert.IsInstanceOfType(codeGenerator, typeof(CSharpCodeGenerator));
            Assert.IsInstanceOfType(codeGenerator, typeof(CSharpUnitTestCodeGenerator));
            Assert.IsInstanceOfType(codeGenerator, typeof(XUnitTestCodeGenerator));
        }

        [TestMethod]
        public void UnitTestCodeGeneratorFactoryCreateXUnitTest()
        {
            var defaultNamespace = typeof(CodeGenTest).Namespace;
            const string connectionString = "Data Source=Northwind.sdf";
            var database = SqlCeDatabaseFactory.Create(defaultNamespace, connectionString);
            var factory = new UnitTestCodeGeneratorFactory(database);
            var codeGenerator = factory.Create("xUnit");

            Assert.IsNotNull(codeGenerator);
            Assert.IsInstanceOfType(codeGenerator, typeof(CodeGenerator));
            Assert.IsInstanceOfType(codeGenerator, typeof(CSharpCodeGenerator));
            Assert.IsInstanceOfType(codeGenerator, typeof(CSharpUnitTestCodeGenerator));
            Assert.IsInstanceOfType(codeGenerator, typeof(XUnitTestCodeGenerator));
        }
    }
}