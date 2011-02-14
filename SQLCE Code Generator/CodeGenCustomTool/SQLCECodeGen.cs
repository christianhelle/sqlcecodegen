using System;
using Microsoft.CustomTool;
using System.Runtime.InteropServices;
using CodeGenCore;
using System.Text;
using System.IO;

namespace CodeGenCustomTool
{
    [Guid("64264FF6-2DD0-489a-A8C2-8FD7855FE3BF")]
    [ComVisible(true)]
    public class SQLCECodeGenerator : BaseCodeGeneratorWithSite
    {
        protected override byte[] GenerateCode(string inputFileName, string inputFileContent)
        {
            string generatedNamespace = FileNameSpace + new FileInfo(inputFileContent).Name;
            string connectionString = "Data Source=" + inputFileName;
            Database database = new Database(generatedNamespace, connectionString);
            CodeGeneratorFactory factory = new CodeGeneratorFactory(database);
            CodeGenerator codeGenerator = factory.Create();

            codeGenerator.WriteHeaderInformation();
            codeGenerator.GenerateEntities();
            codeGenerator.GenerateDataAccessLayer();

            string generatedCode = codeGenerator.GetCode();
            return Encoding.UTF8.GetBytes(generatedCode);
        }
    }
}
