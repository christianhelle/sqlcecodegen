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
    public class CodeGenTest : CodeGenBaseTest
    {
        private static ISqlCeDatabase GetDatabase(string defaultNamespace, string connectionString)
        {
            var database = SqlCeDatabaseFactory.Create(defaultNamespace, connectionString);
            database.AnalyzeDatabase();
            return database;
        }

        [TestMethod]
        public void DatabaseConstructorTest()
        {
            var defaultNamespace = typeof(CodeGenTest).Namespace;
            //var connectionString = "Data Source=Northwind.sdf";
            var target = GetDatabase();

            Assert.AreEqual(defaultNamespace, target.DefaultNamespace);
        }

        [TestMethod]
        public void WriteHeaderInformationTest()
        {
            var defaultNamespace = typeof(CodeGenTest).Namespace;
            const string connectionString = "Data Source=Northwind.sdf";
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
            const string connectionString = "Data Source=Northwind.sdf";
            var database = GetDatabase(defaultNamespace, connectionString);
            var factory = new CodeGeneratorFactory(database);
            var codeGenerator = factory.Create();
            codeGenerator.GenerateEntities();

            var actual = codeGenerator.GetCode();
            Assert.IsNotNull(actual);
            Assert.IsFalse(string.IsNullOrEmpty(actual));
        }

        [TestMethod]
        public void GenerateMultiFileEntitiesTest()
        {
            var defaultNamespace = typeof(CodeGenTest).Namespace;
            const string connectionString = "Data Source=Northwind.sdf";
            var database = GetDatabase(defaultNamespace, connectionString);
            var factory = new CodeGeneratorFactory(database);
            var codeGenerator = factory.Create();
            codeGenerator.GenerateEntities();

            CollectionAssert.AllItemsAreNotNull(codeGenerator.CodeFiles);
            foreach (var codeFile in codeGenerator.CodeFiles)
            {
                Assert.IsNotNull(codeFile);
                Assert.IsFalse(string.IsNullOrEmpty(codeFile.Key));
                Assert.IsNotNull(codeFile.Value);
                Assert.IsFalse(string.IsNullOrEmpty(codeFile.Value.ToString()));
            }
        }

        [TestMethod]
        public void GenerateMultiFileDataAccessTest()
        {
            var defaultNamespace = typeof(CodeGenTest).Namespace;
            const string connectionString = "Data Source=Northwind.sdf";
            var database = GetDatabase(defaultNamespace, connectionString);
            var factory = new CodeGeneratorFactory(database);
            var codeGenerator = factory.Create();
            codeGenerator.GenerateEntities();
            codeGenerator.GenerateDataAccessLayer();

            CollectionAssert.AllItemsAreNotNull(codeGenerator.CodeFiles);
            foreach (var codeFile in codeGenerator.CodeFiles)
            {
                Assert.IsNotNull(codeFile);
                Assert.IsFalse(string.IsNullOrEmpty(codeFile.Key));
                Assert.IsNotNull(codeFile.Value);
                Assert.IsFalse(string.IsNullOrEmpty(codeFile.Value.ToString()));
            }
        }

        [TestMethod]
        public void GenerateEntitiesCanCompileTest()
        {
            var defaultNamespace = typeof(CodeGenTest).Namespace;
            const string connectionString = "Data Source=Northwind.sdf";
            var database = GetDatabase(defaultNamespace, connectionString);
            var factory = new CodeGeneratorFactory(database);
            var codeGenerator = factory.Create();
            codeGenerator.GenerateEntities();

            var actual = codeGenerator.GetCode();
            Assert.IsNotNull(actual);
            Assert.IsFalse(string.IsNullOrEmpty(actual));

            AssertCSharpCompile(actual);
        }

        [TestMethod]
        public void GenerateDataAccessLayerTest()
        {
            var defaultNamespace = typeof(CodeGenTest).Namespace;
            const string connectionString = "Data Source=Northwind.sdf";
            var database = GetDatabase(defaultNamespace, connectionString);
            var factory = new CodeGeneratorFactory(database);
            var codeGenerator = factory.Create();
            codeGenerator.GenerateDataAccessLayer();

            var actual = codeGenerator.GetCode();
            Assert.IsNotNull(actual);
            Assert.IsFalse(string.IsNullOrEmpty(actual));
        }

        [TestMethod]
        public void GenerateDataAccessLayerCanCompileTest()
        {
            var defaultNamespace = typeof(CodeGenTest).Namespace;
            const string connectionString = "Data Source=Northwind.sdf";
            var database = GetDatabase(defaultNamespace, connectionString);
            var factory = new CodeGeneratorFactory(database);
            var codeGenerator = factory.Create();
            codeGenerator.GenerateEntities();
            codeGenerator.GenerateDataAccessLayer();

            var actual = codeGenerator.GetCode();
            Assert.IsNotNull(actual);
            Assert.IsFalse(string.IsNullOrEmpty(actual));

            AssertCSharpCompile(actual);
        }
    }
}