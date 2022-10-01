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
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using ChristianHelle.DatabaseTools.SqlCe.CodeGenCore;
using Microsoft.CustomTool;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCustomTool
{
    [Guid("7690FAC3-0662-49AB-BA02-E65A9F0974A6")]
    [ComVisible(true)]
    public class SQLCEMSTestCodeGeneratorMultiFile : MultipleFileGenerator
    {
        public override void Generate(string wszInputFilePath,
                                      string bstrInputFileContents,
                                      string wszDefaultNamespace,
                                      out IntPtr rgbOutputFileContents,
                                      out int pcbOutput,
                                      IVsGeneratorProgress pGenerateProgress)
        {
            try
            {
                var database = CodeGeneratorCustomTool.GetDatabase(wszDefaultNamespace, wszInputFilePath);
                var factory = new UnitTestCodeGeneratorFactory(database);
                var codeGenerator = factory.Create();
                codeGenerator.GenerateEntities();
                codeGenerator.GenerateDataAccessLayer();

                var header = new StringBuilder();
                codeGenerator.WriteHeaderInformation(header);

                var files = new Dictionary<string, StringBuilder>(codeGenerator.CodeFiles.Count);
                foreach (var codeFile in codeGenerator.CodeFiles)
                    files.Add(codeFile.Key + GetDefaultExtension(), codeFile.Value);

                AddOutputToProject(wszInputFilePath, files, header);

                base.Generate(wszInputFilePath,
                              bstrInputFileContents,
                              wszDefaultNamespace,
                              out rgbOutputFileContents,
                              out pcbOutput,
                              pGenerateProgress);
            }
            catch (Exception)
            {
                var codeGen = new SQLCEMSTestCodeGenerator();
                codeGen.Generate(wszInputFilePath, bstrInputFileContents, wszDefaultNamespace, out rgbOutputFileContents,
                                 out pcbOutput, pGenerateProgress);

                //var applicationException = new ApplicationException("Unable to generate code", e);
                //var messageBox = new ExceptionMessageBox(applicationException);
                //messageBox.Show(null);
                //throw;
            }
        }
    }
}