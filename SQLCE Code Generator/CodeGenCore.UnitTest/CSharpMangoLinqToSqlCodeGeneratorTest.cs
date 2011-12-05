using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore.UnitTest
{
    [TestClass]
    public class CSharpMangoLinqToSqlCodeGeneratorTest : CodeGenBaseTest
    {
        private static SqlCeDatabase GetDatabase(string defaultNamespace, string connectionString)
        {
            var database = new SqlCeDatabase(defaultNamespace, connectionString);
            database.AnalyzeDatabase();
            return database;
        }

        [TestMethod]
        public void WriteHeaderInformationTest()
        {
            var defaultNamespace = typeof(CodeGenTest).Namespace;
            const string connectionString = "Data Source=Northwind.sdf";
            var database = GetDatabase(defaultNamespace, connectionString);
            var codeGenerator = CodeGeneratorFactory.Create<CSharpMangoLinqToSqlCodeGenerator>(database);
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
            var codeGenerator = CodeGeneratorFactory.Create<CSharpMangoLinqToSqlCodeGenerator>(database);
            codeGenerator.GenerateEntities();

            var actual = codeGenerator.GetCode();
            Assert.IsNotNull(actual);
            Assert.IsFalse(string.IsNullOrEmpty(actual));
        }

        [TestMethod]
        public void GenerateEntitiesCanCompileTest()
        {
            var defaultNamespace = typeof(CodeGenTest).Namespace;
            const string connectionString = "Data Source=Northwind.sdf";
            var database = GetDatabase(defaultNamespace, connectionString);
            var codeGenerator = CodeGeneratorFactory.Create<CSharpMangoLinqToSqlCodeGenerator>(database);
            codeGenerator.GenerateEntities();

            var actual = codeGenerator.GetCode();
            Assert.IsNotNull(actual);
            Assert.IsFalse(string.IsNullOrEmpty(actual));

            AssertCSharpCompileWP7(actual);
        }

        [TestMethod]
        public void GenerateDataAccessLayerTest()
        {
            var defaultNamespace = typeof(CodeGenTest).Namespace;
            const string connectionString = "Data Source=Northwind.sdf";
            var database = GetDatabase(defaultNamespace, connectionString);
            var codeGenerator = CodeGeneratorFactory.Create<CSharpMangoLinqToSqlCodeGenerator>(database);
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
            var codeGenerator = CodeGeneratorFactory.Create<CSharpMangoLinqToSqlCodeGenerator>(database);
            codeGenerator.GenerateEntities();
            codeGenerator.GenerateDataAccessLayer();

            var actual = codeGenerator.GetCode();
            Assert.IsNotNull(actual);
            Assert.IsFalse(string.IsNullOrEmpty(actual));

            AssertCSharpCompileWP7(actual);
        }

        [TestMethod]
        public void GenerateMultiFileEntitiesTest()
        {
            var defaultNamespace = typeof(CodeGenTest).Namespace;
            const string connectionString = "Data Source=Northwind.sdf";
            var database = GetDatabase(defaultNamespace, connectionString);
            var codeGenerator = CodeGeneratorFactory.Create<CSharpMangoLinqToSqlCodeGenerator>(database);
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
            var codeGenerator = CodeGeneratorFactory.Create<CSharpMangoLinqToSqlCodeGenerator>(database);
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
        public void GenerateMultiFileEntitiesCanCompileTest()
        {
            var defaultNamespace = typeof(CodeGenTest).Namespace;
            const string connectionString = "Data Source=Northwind.sdf";
            var database = GetDatabase(defaultNamespace, connectionString);
            var codeGenerator = CodeGeneratorFactory.Create<CSharpMangoLinqToSqlCodeGenerator>(database);
            codeGenerator.GenerateEntities();

            var code = new List<string>();
            codeGenerator.CodeFiles.Values.ToList().ForEach(c => code.Add(c.ToString()));
            AssertCSharpCompileWP7(code.ToArray());
        }

        [TestMethod]
        public void GenerateMultiFileDataAccessCanCompileTest()
        {
            var defaultNamespace = typeof(CodeGenTest).Namespace;
            const string connectionString = "Data Source=Northwind.sdf";
            var database = GetDatabase(defaultNamespace, connectionString);
            var codeGenerator = CodeGeneratorFactory.Create<CSharpMangoLinqToSqlCodeGenerator>(database);
            codeGenerator.GenerateEntities();
            codeGenerator.GenerateDataAccessLayer();

            var code = new List<string>();
            codeGenerator.CodeFiles.Values.ToList().ForEach(c => code.Add(c.ToString()));
            AssertCSharpCompileWP7(code.ToArray());
        }

        private static void AssertCSharpCompileWP7(params string[] sourceCode)
        {
            var csc = new CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", "v4.0" } });
            var outputFile = GetOutputFilename();
            var compilerParameters = GetCompilerParameters(outputFile);

            var actual = csc.CompileAssemblyFromSource(compilerParameters, sourceCode);

            foreach (var error in actual.Errors)
                Trace.WriteLine(error, "ERROR");

            Assert.AreEqual(0, actual.Errors.Count);
        }

        private static string GetOutputFilename()
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SQL Compact Code Generator");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return string.Format(@"{0}\DataAccess.dll", path);
        }

        private static CompilerParameters GetCompilerParameters(string exeName)
        {
            var mangoSdkPath = Environment.GetEnvironmentVariable("ProgramFiles(x86)") + @"\Reference Assemblies\Microsoft\Framework\Silverlight\v4.0\Profile\WindowsPhone71";

            var compilerParameters = new CompilerParameters
            {
                GenerateExecutable = false,
                OutputAssembly = exeName,
                GenerateInMemory = false,
                TreatWarningsAsErrors = false,
                IncludeDebugInformation = false,
                CompilerOptions = "/noconfig /nostdlib+"
            };

            compilerParameters.ReferencedAssemblies.Add(string.Format("{0}\\" + "Mscorlib.dll", mangoSdkPath));
            compilerParameters.ReferencedAssemblies.Add(string.Format("{0}\\" + "Mscorlib.Extensions.dll", mangoSdkPath));
            compilerParameters.ReferencedAssemblies.Add(string.Format("{0}\\" + "System.dll", mangoSdkPath));
            compilerParameters.ReferencedAssemblies.Add(string.Format("{0}\\" + "System.Core.dll", mangoSdkPath));
            compilerParameters.ReferencedAssemblies.Add(string.Format("{0}\\" + "System.Data.Linq.dll", mangoSdkPath));
            compilerParameters.ReferencedAssemblies.Add(string.Format("{0}\\" + "Microsoft.Phone.dll", mangoSdkPath));

            return compilerParameters;
        }
    }
}
