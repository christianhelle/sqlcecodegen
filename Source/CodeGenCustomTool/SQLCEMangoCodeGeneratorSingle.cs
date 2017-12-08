using System;
using System.Runtime.InteropServices;
using ChristianHelle.DatabaseTools.SqlCe.CodeGenCore;
using Microsoft.CustomTool;
using Microsoft.SqlServer.MessageBox;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCustomTool
{
    [Guid("687B7864-F622-4C84-BFCD-8F1AEDB2138F")]
    [ComVisible(true)]
    public class SQLCEMangoCodeGeneratorSingle : CSharpFileGenerator
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
                var codeGenerator = CodeGeneratorFactory.Create<CSharpMangoLinqToSqlCodeGenerator>(database);

                byte[] data = CodeGeneratorCustomTool.GetData(codeGenerator);
                if (data == null)
                {
                    rgbOutputFileContents = IntPtr.Zero;
                    pcbOutput = 0;
                }
                else
                {
                    rgbOutputFileContents = Marshal.AllocCoTaskMem(data.Length);
                    Marshal.Copy(data, 0, rgbOutputFileContents, data.Length);
                    pcbOutput = data.Length;
                }
            }
            catch (Exception e)
            {
                var applicationException = new ApplicationException("Unable to generate code", e);
                var messageBox = new ExceptionMessageBox(applicationException);
                messageBox.Show(null);
                throw;
            }
        } 
    }
}