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
            var compilerParameters = new CompilerParameters
            {
                GenerateExecutable = false,
                OutputAssembly = exeName,
                GenerateInMemory = false,
                TreatWarningsAsErrors = false,
                IncludeDebugInformation = false,
            };
            compilerParameters.ReferencedAssemblies.Add("System.dll");
            compilerParameters.ReferencedAssemblies.Add("System.Data.dll");
            compilerParameters.ReferencedAssemblies.Add("System.Data.SqlServerCe.dll");
            compilerParameters.ReferencedAssemblies.Add("Microsoft.VisualStudio.QualityTools.UnitTestFramework.dll");
            compilerParameters.ReferencedAssemblies.Add("nunit.framework.dll");

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
            var compilerParameters = new CompilerParameters
            {
                GenerateExecutable = false,
                OutputAssembly = exeName,
                GenerateInMemory = false,
                TreatWarningsAsErrors = false,
                IncludeDebugInformation = false,
            };
            compilerParameters.ReferencedAssemblies.Add("System.dll");
            compilerParameters.ReferencedAssemblies.Add("System.Data.dll");
            compilerParameters.ReferencedAssemblies.Add("System.Data.SqlServerCe.dll");
            compilerParameters.ReferencedAssemblies.Add("Microsoft.VisualStudio.QualityTools.UnitTestFramework.dll");
            compilerParameters.ReferencedAssemblies.Add("nunit.framework.dll");

            return provider.CompileAssemblyFromSource(compilerParameters, sourceCode);
        }

        #endregion

        private static string GetOutputFilename()
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SQLCE Code Generator");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return string.Format(@"{0}\DataAccess.dll", path);
        }
    }
}
