using System;
using Microsoft.CustomTool;
using System.Runtime.InteropServices;
using CodeGenCore;
using System.Text;

namespace CodeGenCustomTool
{
    [Guid("64264FF6-2DD0-489a-A8C2-8FD7855FE3BF")]
    [ComVisible(true)]
    public class SQLCECodeGenerator : BaseCodeGeneratorWithSite
    {
        protected override byte[] GenerateCode(string inputFileName, string inputFileContent)
        {
            string connectionString = "Data Source=" + inputFileName;
            Database database = new Database(FileNameSpace, connectionString);
            CodeGeneratorFactory factory = new CodeGeneratorFactory(database);
            CodeGenerator codeGenerator = factory.Create();

            codeGenerator.GenerateEntities();
            codeGenerator.GenerateDataAccessLayer();

            string generatedCode = codeGenerator.GetCode();
            return Encoding.ASCII.GetBytes(generatedCode);
        }
    }
}
