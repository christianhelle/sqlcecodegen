using System;
using System.Runtime.InteropServices;
using Microsoft.CustomTool;
using Microsoft.SqlServer.MessageBox;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCustomTool
{
    [Guid("7690FAC3-0662-49AB-BA02-E65A9F0974A6")]
    [ComVisible(true)]
    public class SQLCEMSTestCodeGenerator : IVsSingleFileGenerator
    {
        #region IVsSingleFileGenerator Members

        public string GetDefaultExtension()
        {
            return ".cs";
        }

        public void Generate(string wszInputFilePath,
                             string bstrInputFileContents,
                             string wszDefaultNamespace,
                             out IntPtr rgbOutputFileContents,
                             out int pcbOutput,
                             IVsGeneratorProgress pGenerateProgress)
        {
            try
            {
                var data = CodeGeneratorCustomTool.GenerateUnitTestCode(wszDefaultNamespace, wszInputFilePath, "CSharp", "MSTest");
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

        #endregion
    }
}
