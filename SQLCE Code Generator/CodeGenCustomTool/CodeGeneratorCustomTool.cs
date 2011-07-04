using System.IO;
using System.Text;
using ChristianHelle.DatabaseTools.SqlCe.CodeGenCore;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCustomTool
{
    public static class CodeGeneratorCustomTool
    {
        public static byte[] GenerateCode(string fileNameSpace, string inputFileName, string fileExtension = "CSharp")
        {
            var fi = new FileInfo(inputFileName);
            var generatedNamespace = fileNameSpace + "." + fi.Name.Replace(fi.Extension, string.Empty);
            var connectionString = "Data Source=" + inputFileName;
            var database = GetDatabase(generatedNamespace, connectionString);
            var factory = new CodeGeneratorFactory(database);
            var codeGenerator = factory.Create(fileExtension);

            return GetData(codeGenerator);
        }

        public static byte[] GenerateUnitTestCode(string fileNameSpace, string inputFileName, string fileExtension = "CSharp", string testFramework = "MSTest")
        {
            var fi = new FileInfo(inputFileName);
            var generatedNamespace = fileNameSpace + "." + fi.Name.Replace(fi.Extension, string.Empty);
            var connectionString = "Data Source=" + inputFileName;
            var database = GetDatabase(generatedNamespace, connectionString);
            var factory = new UnitTestCodeGeneratorFactory(database);
            var codeGenerator = factory.Create(testFramework);

            return GetData(codeGenerator);
        }

        private static SqlCeDatabase GetDatabase(string generatedNamespace, string connectionString)
        {
            var database = new SqlCeDatabase(generatedNamespace, connectionString);
            database.Verify();
            database.AnalyzeDatabase();
            return database;
        }

        private static byte[] GetData(CodeGenerator codeGenerator)
        {
            codeGenerator.WriteHeaderInformation();
            codeGenerator.GenerateEntities();
            codeGenerator.GenerateDataAccessLayer();

            var generatedCode = codeGenerator.GetCode();
            return Encoding.Default.GetBytes(generatedCode);
        }
    }
}
