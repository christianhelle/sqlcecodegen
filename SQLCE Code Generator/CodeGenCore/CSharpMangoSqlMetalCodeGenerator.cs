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
using System;
using System.Diagnostics;
using System.IO;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore
{
    public class CSharpMangoSqlMetalCodeGenerator : CodeGenerator
    {
        private const string SQL_METAL_PATH = "\\Microsoft SDKs\\Windows\\v7.0A\\Bin\\SqlMetal.exe";

        public CSharpMangoSqlMetalCodeGenerator(ISqlCeDatabase database)
            : base(database)
        {
        }

        public override void GenerateDataAccessLayer()
        {
            var programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            var sqlmetal = programFiles + SQL_METAL_PATH;
            if (!File.Exists(sqlmetal))
                sqlmetal = programFiles + " (x86)" + SQL_METAL_PATH;

            var processStartInfo = new ProcessStartInfo(sqlmetal, "/code /namespace:" + Database.DefaultNamespace + " \"" + Database.DatabaseFilename + "\"")
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
