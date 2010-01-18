using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;
using System.Diagnostics;
using System.Text;
using CodeGenConsole.Properties;
using System.Windows.Forms;
using CodeGenCore;

namespace CodeGenConsole
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            string connectionString = null, baseNamespace = null;
            if (Debugger.IsAttached)
            {
                if (args == null || args.Length < 2)
                {
                    Console.WriteLine("Invalid parameters");
                    return;
                }
                connectionString = args[0];
                baseNamespace = args[1];
            }

            Database database;
            if (string.IsNullOrEmpty(connectionString) && string.IsNullOrEmpty(baseNamespace))
                database = new Database(typeof(Program).Namespace, Settings.Default.TestDatabaseConnectionString);
            else
                database = new Database(baseNamespace, connectionString);

            CodeGeneratorFactory factory = new CodeGeneratorFactory(database);
            CodeGenerator codeGenerator = factory.Create();

            codeGenerator.GenerateEntities();
            codeGenerator.GenerateDataAccessLayer();

            Output(codeGenerator.GetCode());
            Clipboard.SetText(codeGenerator.GetCode());
        }

        static void Output(object text)
        {
            Console.WriteLine(text);
            Trace.WriteLine(text);
        }
    }
}
