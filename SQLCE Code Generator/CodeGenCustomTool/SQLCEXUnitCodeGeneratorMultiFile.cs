using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using ChristianHelle.DatabaseTools.SqlCe.CodeGenCore;
using Microsoft.CustomTool;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCustomTool
{
    [Guid("2A70A634-C234-46B8-B484-96FB9C74CF69")]
    [ComVisible(true)]
    public class SQLCEXUnitCodeGeneratorMultiFile : MultipleFileGenerator
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
                var codeGenerator = factory.Create("xUnit");
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
                var codeGen = new SQLCEXUnitCodeGenerator();
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