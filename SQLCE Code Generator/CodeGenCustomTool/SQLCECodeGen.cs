using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using ChristianHelle.DatabaseTools.SqlCe.CodeGenCore;
using Microsoft.CustomTool;
using Microsoft.SqlServer.MessageBox;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCustomTool
{
    [Guid("64264FF6-2DD0-489a-A8C2-8FD7855FE3BF")]
    [ComVisible(true)]
    public class SQLCECodeGenerator : IVsSingleFileGenerator
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
                var data = CodeGeneratorCustomTool.GenerateCode(wszDefaultNamespace, wszInputFilePath, "CSharp");
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

    public static class CodeGeneratorCustomTool
    {
        public static byte[] GenerateCode(string fileNameSpace, string inputFileName, string fileExtension)
        {
            var fi = new FileInfo(inputFileName);
            var generatedNamespace = fileNameSpace + "." + fi.Name.Replace(fi.Extension, string.Empty);
            var connectionString = "Data Source=" + inputFileName;
            var database = new SqlCeDatabase(generatedNamespace, connectionString);
            var factory = new CodeGeneratorFactory(database);
            var codeGenerator = factory.Create(fileExtension);

            //if (codeGenerator is VisualBasicCodeGenerator)
            //{
            //    const string WARNING = "Visual Basic .NET code generation is currently not supported";
            //    Trace.WriteLine(WARNING, "NOT SUPPORT");
            //    MessageBox.Show(WARNING, "Coming soon...");
            //}

            codeGenerator.WriteHeaderInformation();
            codeGenerator.GenerateEntities();
            codeGenerator.GenerateDataAccessLayer();

            var generatedCode = codeGenerator.GetCode();
            return Encoding.Default.GetBytes(generatedCode);
        }
    }
}