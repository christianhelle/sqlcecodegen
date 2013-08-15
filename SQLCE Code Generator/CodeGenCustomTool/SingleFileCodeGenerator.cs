using System;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.VisualStudio.Shell.Interop;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCustomTool
{
    public abstract class SingleFileCodeGenerator : IVsSingleFileGenerator
    {
        private readonly string defaultExtension;

        protected SingleFileCodeGenerator(string defaultExtension)
        {
            this.defaultExtension = defaultExtension;
        }

        public int DefaultExtension(out string pbstrDefaultExtension)
        {
            pbstrDefaultExtension = defaultExtension;
            return 0;
        }

        public int Generate(string wszInputFilePath,
                            string bstrInputFileContents,
                            string wszDefaultNamespace,
                            IntPtr[] rgbOutputFileContents,
                            out uint pcbOutput,
                            IVsGeneratorProgress pGenerateProgress)
        {
            var code = Generate(wszInputFilePath, wszDefaultNamespace);
            var data = Encoding.Default.GetBytes(code);
            var ptr = Marshal.AllocCoTaskMem(data.Length);
            Marshal.Copy(data, 0, ptr, data.Length);

            rgbOutputFileContents[0] = ptr;
            pcbOutput = (uint)code.Length;

            return 0;
        }

        protected abstract string Generate(string wszInputFilePath, string wszDefaultNamespace);
    }

    public class CSharpSingleFileCodeGenerator : SingleFileCodeGenerator
    {
        public CSharpSingleFileCodeGenerator() : base(".cs")
        {
        }

        protected override string Generate(string wszInputFilePath, string wszDefaultNamespace)
        {
            return CodeGeneratorCustomTool.GenerateCodeString(wszDefaultNamespace, wszInputFilePath);
        }
    }
}