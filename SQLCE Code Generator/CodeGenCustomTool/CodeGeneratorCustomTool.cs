using System.Data.SqlServerCe;
using System.IO;
using System.Text;
using System.Windows.Forms;
using ChristianHelle.DatabaseTools.SqlCe.CodeGenCore;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCustomTool
{
    public static class CodeGeneratorCustomTool
    {
        public static byte[] GenerateCode(string fileNameSpace, string inputFileName, string fileExtension = "CSharp")
        {
            var database = GetDatabase(fileNameSpace, inputFileName);
            var factory = new CodeGeneratorFactory(database);
            var codeGenerator = factory.Create(fileExtension);

            return GetData(codeGenerator);
        }

        public static byte[] GenerateUnitTestCode(string fileNameSpace, string inputFileName, string fileExtension = "CSharp", string testFramework = "MSTest")
        {
            var database = GetDatabase(fileNameSpace, inputFileName);
            var factory = new UnitTestCodeGeneratorFactory(database);
            var codeGenerator = factory.Create(testFramework);

            return GetData(codeGenerator);
        }

        //private static SqlCeDatabase GetDatabase(string generatedNamespace, string connectionString)
        //{
        //    var database = new SqlCeDatabase(generatedNamespace, connectionString);
        //    database.Verify();
        //    database.AnalyzeDatabase();
        //    return database;
        //}

        private static SqlCeDatabase GetDatabase(string fileNameSpace, string inputFileName, string password = null)
        {
            try
            {
                var fi = new FileInfo(inputFileName);
                var generatedNamespace = fileNameSpace + "." + fi.Name.Replace(fi.Extension, string.Empty).Replace(" ", string.Empty);
                var connectionString = GetConnectionString(inputFileName, password);
                var database= new SqlCeDatabase(generatedNamespace, connectionString);
                database.Verify();
                database.AnalyzeDatabase();
                return database;
            }
            catch (SqlCeException e)
            {
                if (e.NativeError == 25028 || e.NativeError == 25140 || e.Message.ToLower().Contains("password"))
                {
                    var passwordResult = PromptForPassword();
                    if (!string.IsNullOrEmpty(passwordResult))
                        return GetDatabase(fileNameSpace, inputFileName, passwordResult);
                }
                throw;
            }
        }

        private static string PromptForPassword()
        {
            using (var form = new PasswordForm())
            {
                var result = form.ShowDialog();
                return result != DialogResult.OK ? null : form.Password;
            }
        }

        private static string GetConnectionString(string inputFileName, string password = null)
        {
            return string.Format("Data Source={0}; Password={1}", inputFileName, password);
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
