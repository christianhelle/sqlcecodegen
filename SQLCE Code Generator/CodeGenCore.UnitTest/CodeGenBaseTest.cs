using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore.UnitTest
{
    [TestClass]
    [DeploymentItem("app.config")]
    [DeploymentItem("Northwind.sdf")]
    [DeploymentItem("Microsoft.VisualStudio.QualityTools.UnitTestFramework.dll")]
    [DeploymentItem("nunit.framework.dll")]
    [DeploymentItem("xunit.dll")]
    public class CodeGenBaseTest
    {
        [TestInitialize]
        public void Initialize()
        {
            File.SetAttributes("Northwind.sdf", FileAttributes.Normal);
        }

        protected static ISqlCeDatabase GetDatabase()
        {
            var defaultNamespace = typeof(CodeGenTest).Namespace;
            const string connectionString = "Data Source=Northwind.sdf;";
            return SqlCeDatabaseFactory.Create(defaultNamespace, connectionString);
        }

        protected static void AssertCSharpCompile(params string[] sourceCode)
        {
            var actual = CodeCompiler.CompileCSharpSource(sourceCode);

            foreach (var error in actual.Errors)
                Trace.WriteLine(error, "ERROR");

            //if (actual.Errors.Count > 0)
            //    foreach (var code in sourceCode)
            //        Trace.WriteLine(code);

            Assert.AreEqual(0, actual.Errors.Count);
        }

        protected static void AssertVisualBasicCompile(params string[] sourceCode)
        {
            var actual = CodeCompiler.CompileVisualBasicSource(sourceCode);

            foreach (var error in actual.Errors)
                Trace.WriteLine(error, "ERROR");

            //if (actual.Errors.Count > 0)
            //    foreach (var code in sourceCode)
            //        Trace.WriteLine(code);

            Assert.AreEqual(0, actual.Errors.Count);
        }
    }
}