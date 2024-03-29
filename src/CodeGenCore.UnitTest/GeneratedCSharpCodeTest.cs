﻿using ChristianHelle.DatabaseTools.SqlCe.CodeGenCustomTool;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore.UnitTest
{
    [TestClass]
    [DeploymentItem("Microsoft.VisualStudio.QualityTools.UnitTestFramework.dll")]
    public class GeneratedCSharpCodeMsTest : CodeGenBaseTest
    {
        [TestMethod]
        public void WriteHeaderInformationTest()
        {
            var database = GetDatabase();
            var factory = new CodeGeneratorFactory(database);

            var codeGenerator = factory.Create();
            codeGenerator.WriteHeaderInformation();

            Assert.IsFalse(string.IsNullOrEmpty(codeGenerator.GetCode()));
        }

        [TestMethod]
        public void EntitiesCodeCanCompileTest()
        {
            var database = GetDatabase();
            var factory = new CodeGeneratorFactory(database);

            var codeGenerator = factory.Create();
            codeGenerator.WriteHeaderInformation();
            codeGenerator.GenerateEntities();

            AssertCSharpCompile(codeGenerator.GetCode());
        }

        [TestMethod]
        public void DataAccessCodeCanCompileTest()
        {
            var database = GetDatabase();
            var factory = new CodeGeneratorFactory(database);

            var codeGenerator = factory.Create();
            codeGenerator.WriteHeaderInformation();
            codeGenerator.GenerateEntities();
            codeGenerator.GenerateDataAccessLayer();

            AssertCSharpCompile(codeGenerator.GetCode());
        }

        [TestMethod]
        public void EntityUnitTestCodeCanCompileTest()
        {
            var database = GetDatabase();
            var factory = new CodeGeneratorFactory(database);

            var codeGenerator = factory.Create();
            codeGenerator.GenerateEntities();

            var unitTestCodeGenerator = new MsTestUnitTestCodeGenerator(database);
            unitTestCodeGenerator.WriteHeaderInformation();
            unitTestCodeGenerator.GenerateEntities();

            AssertCSharpCompile(codeGenerator.GetCode(), unitTestCodeGenerator.GetCode());
        }

        [TestMethod]
        public void DataAccessUnitTestCodeCanCompileTest()
        {
            var database = GetDatabase();
            var factory = new CodeGeneratorFactory(database);

            var codeGenerator = factory.Create();
            codeGenerator.GenerateEntities();
            codeGenerator.GenerateDataAccessLayer();

            var unitTestCodeGenerator = new MsTestUnitTestCodeGenerator(database);
            unitTestCodeGenerator.WriteHeaderInformation();
            unitTestCodeGenerator.GenerateDataAccessLayer();

            AssertCSharpCompile(codeGenerator.GetCode(), unitTestCodeGenerator.GetCode());
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

            var unitTestCodeGenerator = new MsTestUnitTestCodeGenerator(database);
            unitTestCodeGenerator.WriteHeaderInformation();
            unitTestCodeGenerator.GenerateEntities();
            unitTestCodeGenerator.GenerateDataAccessLayer();

            AssertCSharpCompile(codeGenerator.GetCode(), unitTestCodeGenerator.GetCode());
        }

        [TestMethod]
        public void CustomToolGeneratedCodeCanCompileTest()
        {
            var database = GetDatabase();
            var actual = CodeGeneratorCustomTool.GenerateCode(database.Namespace, "TestDatabase.sdf", ".cs");

            Assert.IsNotNull(actual);
            Assert.AreNotEqual(0, actual.Length);
            AssertCSharpCompile(Encoding.Default.GetString(actual));
        }
    }

    [TestClass]
    [DeploymentItem("nunit.framework.dll")]
    public class GeneratedCSharpCodeNUnitTest : CodeGenBaseTest
    {
        [TestMethod]
        public void WriteHeaderInformationTest()
        {
            var database = GetDatabase();
            var factory = new CodeGeneratorFactory(database);

            var codeGenerator = factory.Create();
            codeGenerator.WriteHeaderInformation();

            Assert.IsFalse(string.IsNullOrEmpty(codeGenerator.GetCode()));
        }

        [TestMethod]
        public void EntitiesCodeCanCompileTest()
        {
            var database = GetDatabase();
            var factory = new CodeGeneratorFactory(database);

            var codeGenerator = factory.Create();
            codeGenerator.WriteHeaderInformation();
            codeGenerator.GenerateEntities();

            AssertCSharpCompile(codeGenerator.GetCode());
        }

        [TestMethod]
        public void DataAccessCodeCanCompileTest()
        {
            var database = GetDatabase();
            var factory = new CodeGeneratorFactory(database);

            var codeGenerator = factory.Create();
            codeGenerator.WriteHeaderInformation();
            codeGenerator.GenerateEntities();
            codeGenerator.GenerateDataAccessLayer();

            AssertCSharpCompile(codeGenerator.GetCode());
        }

        [TestMethod]
        public void EntityUnitTestCodeCanCompileTest()
        {
            var database = GetDatabase();
            var factory = new CodeGeneratorFactory(database);

            var codeGenerator = factory.Create();
            codeGenerator.GenerateEntities();

            var unitTestCodeGenerator = new NUnitTestCodeGenerator(database);
            unitTestCodeGenerator.WriteHeaderInformation();
            unitTestCodeGenerator.GenerateEntities();

            AssertCSharpCompile(codeGenerator.GetCode(), unitTestCodeGenerator.GetCode());
        }

        [TestMethod]
        public void DataAccessUnitTestCodeCanCompileTest()
        {
            var database = GetDatabase();
            var factory = new CodeGeneratorFactory(database);

            var codeGenerator = factory.Create();
            codeGenerator.GenerateEntities();
            codeGenerator.GenerateDataAccessLayer();

            var unitTestCodeGenerator = new NUnitTestCodeGenerator(database);
            unitTestCodeGenerator.WriteHeaderInformation();
            unitTestCodeGenerator.GenerateDataAccessLayer();

            AssertCSharpCompile(codeGenerator.GetCode(), unitTestCodeGenerator.GetCode());
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

            var unitTestCodeGenerator = new NUnitTestCodeGenerator(database);
            unitTestCodeGenerator.WriteHeaderInformation();
            unitTestCodeGenerator.GenerateEntities();
            unitTestCodeGenerator.GenerateDataAccessLayer();

            AssertCSharpCompile(codeGenerator.GetCode(), unitTestCodeGenerator.GetCode());
        }

        [TestMethod]
        public void CustomToolGeneratedCodeCanCompileTest()
        {
            var database = GetDatabase();
            var actual = CodeGeneratorCustomTool.GenerateCode(database.Namespace, "TestDatabase.sdf", ".cs");

            Assert.IsNotNull(actual);
            Assert.AreNotEqual(0, actual.Length);
            AssertCSharpCompile(Encoding.Default.GetString(actual));
        }
    }
}
