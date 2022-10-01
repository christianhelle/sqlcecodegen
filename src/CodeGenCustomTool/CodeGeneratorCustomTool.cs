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
using System.Data.SqlServerCe;
using System.IO;
using System.Text;
using System.Windows.Forms;
using ChristianHelle.DatabaseTools.SqlCe.CodeGenCore;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCustomTool
{
    public static class CodeGeneratorCustomTool
    {
        public static string GenerateCodeString(string wszInputFilePath, string wszDefaultNamespace, string fileExtension = "CSharp")
        {
            var database = GetDatabase(wszDefaultNamespace, wszInputFilePath);
            var factory = new CodeGeneratorFactory(database);
            var codeGenerator = factory.Create(fileExtension);
            return GenerateCode(codeGenerator);
        }

        public static byte[] GenerateCode(string fileNameSpace, string inputFileName, string fileExtension = "CSharp")
        {
            var database = GetDatabase(fileNameSpace, inputFileName);
            var factory = new CodeGeneratorFactory(database);
            var codeGenerator = factory.Create(fileExtension);

            return GetData(codeGenerator);
        }

        public static byte[] GenerateUnitTestCode(string fileNameSpace, string inputFileName,
                                                  string fileExtension = "CSharp", string testFramework = "MSTest")
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

        public static ISqlCeDatabase GetDatabase(string fileNameSpace, string inputFileName, string password = null)
        {
            try
            {
                var fi = new FileInfo(inputFileName);
                var generatedNamespace = fileNameSpace + "." +
                                         fi.Name.Replace(fi.Extension, string.Empty).Replace(" ", string.Empty);
                var connectionString = GetConnectionString(inputFileName, password);
                var database = SqlCeDatabaseFactory.Create(generatedNamespace, connectionString);
                database.DatabaseFilename = inputFileName;
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

        public static byte[] GetData(CodeGenerator codeGenerator)
        {
            var generatedCode = GenerateCode(codeGenerator);
            return Encoding.Default.GetBytes(generatedCode);
        }

        private static string GenerateCode(CodeGenerator codeGenerator)
        {
            codeGenerator.WriteHeaderInformation();
            codeGenerator.GenerateEntities();
            codeGenerator.GenerateDataAccessLayer();

            var generatedCode = codeGenerator.GetCode();
            return generatedCode;
        }
    }
}