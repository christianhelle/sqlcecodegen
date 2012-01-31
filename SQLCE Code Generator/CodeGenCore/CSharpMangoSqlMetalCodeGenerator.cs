using System;
using System.Diagnostics;
using System.IO;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore
{
    public class CSharpMangoSqlMetalCodeGenerator : CodeGenerator
    {
        private const string SQL_METAL_PATH = "\\Microsoft SDKs\\Windows\\v7.0A\\Bin\\SqlMetal.exe";

        public CSharpMangoSqlMetalCodeGenerator(SqlCeDatabase database)
            : base(database)
        {
        }

        public override void GenerateDataAccessLayer()
        {
            var programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            var sqlmetal = programFiles + SQL_METAL_PATH;
            if (!File.Exists(sqlmetal))
                sqlmetal = programFiles + " (x86)" + SQL_METAL_PATH;

            var processStartInfo = new ProcessStartInfo(sqlmetal, "/code /namespace:" + Database.Namespace + " \"" + Database.DatabaseFilename + "\"")
            {
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                UseShellExecute = false
            };

            var process = Process.Start(processStartInfo);
            var output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            if (output.Contains("Error :"))
                return;

            var cleanCode = false;
            var unsupportedCodeBlock = false;
            using (var reader = new StringReader(output))
            {
                while (true)
                {
                    var value = reader.ReadLine();
                    if (value == "#pragma warning restore 1591")
                        break;

                    if (value == "#pragma warning disable 1591")
                        cleanCode = true;

                    if (value != null && value.Contains("System.Data.IDbConnection"))
                        unsupportedCodeBlock = true;

                    if (unsupportedCodeBlock && value != null && value.Contains("}"))
                    {
                        unsupportedCodeBlock = false;
                        continue;
                    }

                    var commentCode = value != null && value.StartsWith("//");

                    if (!cleanCode || unsupportedCodeBlock || commentCode)
                        continue;

                    Code.AppendLine(value);
                }
            }
        }
    }
}
