﻿using System.Text;
using ChristianHelle.DatabaseTools.SqlCe.CodeGenCustomTool;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore.UnitTest
{
    [TestClass]
    [DeploymentItem("nunit.framework.dll")]
    public class GeneratedCSharpCodeNUnitTest : CodeGenBaseTest
    {
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
            var generateUnitTestCode = CodeGeneratorCustomTool.GenerateUnitTestCode(database.DefaultNamespace, "Northwind.sdf", ".cs", "NUnit");

            Assert.IsNotNull(generateUnitTestCode);
            Assert.AreNotEqual(0, generateUnitTestCode.Length);
            AssertCSharpCompile(Encoding.Default.GetString(generateUnitTestCode),
                                Encoding.Default.GetString(CodeGeneratorCustomTool.GenerateCode(database.DefaultNamespace, "Northwind.sdf")));
        }
    }
}