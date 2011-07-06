using System;
using System.Windows.Forms;
using Microsoft.SqlServer.MessageBox;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenGUI
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(args));
        }

        static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var form = new ExceptionMessageBox(e.ExceptionObject as Exception);
            form.Caption = "Unexpected Error";
            form.Text = "An unexpected error has occurred";
            form.Show(null);
        }
    }
}