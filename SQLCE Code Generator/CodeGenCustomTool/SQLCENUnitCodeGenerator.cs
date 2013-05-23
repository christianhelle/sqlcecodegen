using System;
using System.Runtime.InteropServices;
using Microsoft.CustomTool;
using Microsoft.SqlServer.MessageBox;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCustomTool
{
    [Guid("4866956A-C9D7-4113-9944-2DDB0C638D48")]
    [ComVisible(true)]
    public class SQLCENUnitCodeGenerator : CSharpFileGenerator
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
                var data = CodeGeneratorCustomTool.GenerateUnitTestCode(wszDefaultNamespace, wszInputFilePath,
                                                                        "CSharp", "NUnit");
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