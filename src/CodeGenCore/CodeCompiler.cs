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
using System;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Collections.Generic;
using System.IO;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore
{
    public static class CodeCompiler
    {
        #region C#
        
        private static CSharpCodeProvider CreateCSharpCodeProvider()
        {
            return new CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", "v3.5" } });
            //return new CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", "v4.0" } });
        }

        public static CompilerResults CompileCSharpFiles(params string[] sourceCode)
        {
            var csc = CreateCSharpCodeProvider();
            return CompileFiles(csc, sourceCode);
        }

        public static CompilerResults CompileFiles(CodeDomProvider provider, params string[] sourceFiles)
        {
            var exeName = GetOutputFilename();
            var compilerParameters = GetCompilerParameters(exeName);

            return provider.CompileAssemblyFromFile(compilerParameters, sourceFiles);
        }

        public static CompilerResults CompileCSharpSource(params string[] sourceCode)
        {
            var csc = CreateCSharpCodeProvider();
            return CompileSource(csc, sourceCode);
        } 

        #endregion

        #region Visual Basic

        public static CompilerResults CompileVBharpFiles(params string[] sourceCode)
        {
            return CompileFiles(CodeDomProvider.CreateProvider("VisualBasic"), sourceCode);
        }

        public static CompilerResults CompileVisualBasicSource(params string[] sourceCode)
        {
            return CompileSource(CodeDomProvider.CreateProvider("VisualBasic"), sourceCode);
        }

        public static CompilerResults CompileSource(CodeDomProvider provider, params string[] sourceCode)
        {
            var exeName = GetOutputFilename();
            var compilerParameters = GetCompilerParameters(exeName);

            return provider.CompileAssemblyFromSource(compilerParameters, sourceCode);
        }

        #endregion

        private static string GetOutputFilename()
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SQL Compact Code Generator");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return string.Format(@"{0}\DataAccess.dll", path);
        }

        private static CompilerParameters GetCompilerParameters(string exeName)
        {
            var compilerParameters = new CompilerParameters
            {
                GenerateExecutable = false,
                OutputAssembly = exeName,
                GenerateInMemory = false,
                TreatWarningsAsErrors = false,
                IncludeDebugInformation = false,
            };
            compilerParameters.ReferencedAssemblies.Add("System.dll");
            compilerParameters.ReferencedAssemblies.Add("System.Core.dll");
            compilerParameters.ReferencedAssemblies.Add("System.Data.dll");
            compilerParameters.ReferencedAssemblies.Add("System.Data.Linq.dll");
            compilerParameters.ReferencedAssemblies.Add("System.Data.SqlServerCe.dll");
            compilerParameters.ReferencedAssemblies.Add("Microsoft.VisualStudio.QualityTools.UnitTestFramework.dll");
            compilerParameters.ReferencedAssemblies.Add("nunit.framework.dll");
            compilerParameters.ReferencedAssemblies.Add("xunit.dll");
            return compilerParameters;
        }
    }
}
