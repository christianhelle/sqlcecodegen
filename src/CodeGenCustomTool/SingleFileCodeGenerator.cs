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