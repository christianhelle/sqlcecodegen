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