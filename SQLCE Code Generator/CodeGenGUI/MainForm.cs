﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ChristianHelle.DatabaseTools.SqlCe.CodeGenCore;
using ICSharpCode.TextEditor.Document;
using System.Diagnostics;

namespace CodeGenGUI
{
    public partial class MainForm : Form
    {
        private string dataSource;

        public MainForm()
        {
            InitializeComponent();

            rtbGeneratedCode.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy("C#");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SafeOperation((Action)delegate { NewFile(); });
        }

        private void NewFile()
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                dialog.Filter = "Database files (*.sdf)|*.sdf";
                if (dialog.ShowDialog() == DialogResult.Cancel)
                    return;

                dataSource = dialog.FileName;
                string generatedCode = GenerateCode(dataSource);
                rtbGeneratedCode.Text = generatedCode;
            }
        }

        private string GenerateCode(string inputFileName)
        {
            FileInfo fi = new FileInfo(inputFileName);
            string generatedNamespace = GetType().Namespace + "." + fi.Name.Replace(fi.Extension, string.Empty);
            string connectionString = "Data Source=" + inputFileName;
            SqlCeDatabase database = new SqlCeDatabase(generatedNamespace, connectionString);
            PopulateTables(database.Tables);

            CodeGeneratorFactory factory = new CodeGeneratorFactory(database);
            CodeGenerator codeGenerator = factory.Create();

            codeGenerator.WriteHeaderInformation();
            codeGenerator.GenerateEntities();
            codeGenerator.GenerateDataAccessLayer();

            string generatedCode = codeGenerator.GetCode();
            return generatedCode;
        }

        private void PopulateTables(List<Table> list)
        {
            var rootNode = new TreeNode("Database Tables");
            rootNode.Expand();

            foreach (var item in list)
            {
                var node = new TreeNode(item.TableName);
                node.NodeFont = new Font(treeView.Font, FontStyle.Bold);
                node.Expand();
                rootNode.Nodes.Add(node);

                var columns = new TreeNode("Columns");
                columns.Expand();
                node.Nodes.Add(columns);

                foreach (var column in item.Columns)
                {
                    var columnNode = new TreeNode(column.Key);
                    columnNode.Nodes.Add("Database Type - " + column.Value.DatabaseType);
                    columnNode.Nodes.Add("Managed Type - " + column.Value.ManagedType);
                    columnNode.Nodes.Add("Max Length - " + column.Value.MaxLength);
                    columns.Nodes.Add(columnNode);
                }
            }

            treeView.Nodes.Clear();
            treeView.Nodes.Add(rootNode);
            treeView.SelectedNode = rootNode;

            Refresh();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            SafeOperation((Action)delegate { NewFile(); });
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SafeOperation((Action)delegate { OpenFile(); });
        }

        private void OpenFile()
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                dialog.Filter = "SQLCE Code Generator files (*.codegen)|*.codegen";
                if (dialog.ShowDialog() == DialogResult.Cancel)
                    return;

                var codeGen = new CodeGenFileSerializer();
                CodeGenFile file = codeGen.LoadFile(dialog.FileName);
                rtbGeneratedCode.Text = file.GeneratedCode;

                FileInfo fi = new FileInfo(file.DataSource);
                string generatedNamespace = GetType().Namespace + "." + fi.Name.Replace(fi.Extension, string.Empty);
                string connectionString = "Data Source=" + file.DataSource;
                SqlCeDatabase database = new SqlCeDatabase(generatedNamespace, connectionString);
                PopulateTables(database.Tables);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SafeOperation((Action)delegate { SaveFile(); });
        }

        private void SaveFile()
        {
            using (var dialog = new SaveFileDialog())
            {
                dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                dialog.Filter = "SQLCE Code Generator files (*.codegen)|*.codegen";
                if (dialog.ShowDialog() == DialogResult.Cancel)
                    return;

                var codeGen = new CodeGenFile
                {
                    GeneratedCode = rtbGeneratedCode.Text,
                    DataSource = dataSource
                };
                var serializer = new CodeGenFileSerializer();
                serializer.SaveFile(codeGen, dialog.FileName);
            }
        }

        static void SafeOperation(Action action)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SafeOperation((Action)delegate { SaveFile(); });
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var about = new AboutBox())
                about.ShowDialog();
        }

        #region Clipboard Handling
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtbGeneratedCode.Undo();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtbGeneratedCode.Redo();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtbGeneratedCode.ActiveTextAreaControl.TextArea.ClipboardHandler.Cut(sender, e);
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtbGeneratedCode.ActiveTextAreaControl.TextArea.ClipboardHandler.Copy(sender, e);
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtbGeneratedCode.ActiveTextAreaControl.TextArea.ClipboardHandler.Paste(sender, e);
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtbGeneratedCode.ActiveTextAreaControl.TextArea.ClipboardHandler.SelectAll(sender, e);
        }
        #endregion

        private void compileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CompileCSharp20();
        }

        private void CompileCSharp20()
        {
            rtbOutput.ResetText();

            CreateOutputCSharpFile();

            var csc = Environment.ExpandEnvironmentVariables(@"%SystemRoot%\Microsoft.Net\Framework\v3.5\csc.exe");
            var args = string.Format(@"/target:library /reference:""{0}\System.Data.SqlServerCe.dll"" ""{0}\Output.cs""", Environment.CurrentDirectory);

            WriteToOutputWindow("Compiling using C# 3.0");
            WriteToOutputWindow(string.Format("{0} {1}", csc, args));

            var psi = new ProcessStartInfo(csc, args);
            psi.RedirectStandardOutput = true;
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;

            var process = Process.Start(psi);
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            WriteToOutputWindow(output);
        }

        private void CreateOutputCSharpFile()
        {
            using (var stream = File.CreateText("Output.cs"))
                stream.Write(rtbGeneratedCode.Text);
        }

        void WriteToOutputWindow(string text)
        {
            rtbOutput.Text += "\n" + text;
            rtbOutput.SelectionStart = rtbOutput.TextLength;
            rtbOutput.ScrollToCaret();

            Trace.WriteLine(text);
        }
    }
}
