using System;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Collections.Generic;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore
{
    public static class CodeCompiler
    {
        public static CompilerResults CompileCSharpFiles(params string[] sourceCode)
        {
            var csc = new CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", "v3.5" } });
            return CompileFiles(csc, sourceCode);
        }

        public static CompilerResults CompileVBharpFiles(params string[] sourceCode)
        {
            return CompileFiles(CodeDomProvider.CreateProvider("VisualBasic"), sourceCode);
        }

        public static CompilerResults CompileFiles(CodeDomProvider provider, params string[] sourceFiles)
        {
            var exeName = String.Format(@"{0}\DataAccess.dll", Environment.CurrentDirectory);
            var compilerParameters = new CompilerParameters
            {
                GenerateExecutable = false,
                OutputAssembly = exeName,
                GenerateInMemory = false,
                TreatWarningsAsErrors = false,
                IncludeDebugInformation = true,
            };
            compilerParameters.ReferencedAssemblies.Add("System.dll");
            compilerParameters.ReferencedAssemblies.Add("System.Data.dll");
            compilerParameters.ReferencedAssemblies.Add("System.Data.SqlServerCe.dll");
            compilerParameters.ReferencedAssemblies.Add("Microsoft.VisualStudio.QualityTools.UnitTestFramework.dll");

            return provider.CompileAssemblyFromFile(compilerParameters, sourceFiles);
        }

        public static CompilerResults CompileCSharpSource(params string[] sourceCode)
        {
            var csc = new CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", "v3.5" } });
            return CompileSource(csc, sourceCode);
        }

        public static CompilerResults CompileVisualBasicSource(params string[] sourceCode)
        {
            return CompileSource(CodeDomProvider.CreateProvider("VisualBasic"), sourceCode);
        }

        public static CompilerResults CompileSource(CodeDomProvider provider, params string[] sourceCode)
        {
            var exeName = String.Format(@"{0}\DataAccess.dll", Environment.CurrentDirectory);
            var compilerParameters = new CompilerParameters
            {
                GenerateExecutable = false,
                OutputAssembly = exeName,
                GenerateInMemory = false,
                TreatWarningsAsErrors = false,
                IncludeDebugInformation = true,
            };
            compilerParameters.ReferencedAssemblies.Add("System.dll");
            compilerParameters.ReferencedAssemblies.Add("System.Data.dll");
            compilerParameters.ReferencedAssemblies.Add("System.Data.SqlServerCe.dll");
            compilerParameters.ReferencedAssemblies.Add("Microsoft.VisualStudio.QualityTools.UnitTestFramework.dll");

            return provider.CompileAssemblyFromSource(compilerParameters, sourceCode);
        }
    }
}
