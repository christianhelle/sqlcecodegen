using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore.UnitTest
{
    [TestClass]
    [DeploymentItem("app.config")]
    [DeploymentItem("TestDatabase.sdf")]
    public class CodeGenBaseTest
    {
        [TestInitialize]
        public void Initialize()
        {
            var fi = new FileInfo("TestDatabase.sdf");
            fi.Attributes = FileAttributes.Normal;
        }

        protected static SqlCeDatabase GetDatabase()
        {
            var defaultNamespace = typeof(CodeGenTest).Namespace;
            var connectionString = "Data Source=TestDatabase.sdf";
            return new SqlCeDatabase(defaultNamespace, connectionString);
        }

        protected static void AssertCSharpCompile(params string[] sourceCode)
        {
            var actual = CodeCompiler.CompileCSharpSource(sourceCode);

            foreach (var error in actual.Errors)
                Trace.WriteLine(error, "ERROR");

            Assert.AreEqual(0, actual.Errors.Count);
        }

        protected static void AssertVisualBasicCompile(params string[] sourceCode)
        {
            var actual = CodeCompiler.CompileVisualBasicSource(sourceCode);

            foreach (var error in actual.Errors)
                Trace.WriteLine(error, "ERROR");

            Assert.AreEqual(0, actual.Errors.Count);
        }
    }
}