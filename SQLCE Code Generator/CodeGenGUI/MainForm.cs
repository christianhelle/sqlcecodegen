using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ChristianHelle.DatabaseTools.SqlCe.CodeGenCore;
using ICSharpCode.TextEditor.Document;
using System.Threading;

namespace CodeGenGUI
{
    public partial class MainForm : Form
    {
        private string dataSource;
        private bool launchedWithArgument;

        public MainForm(string[] args)
        {
            InitializeComponent();

            rtbGeneratedCodeEntities.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy("C#");
            rtbGeneratedCodeDataAccess.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy("C#");

            if (args != null && args.Length == 1)
            {
                launchedWithArgument = true;

                var ext = Path.GetExtension(args[0]).ToLower();
                if (ext == ".sdf")
                {
                    Text += " - Untitled";

                    var sw = Stopwatch.StartNew();

                    var codeGenerator = CreateCodeGenerator(dataSource);
                    rtbGeneratedCodeEntities.Text = GenerateEntitiesCode(codeGenerator);
                    rtbGeneratedCodeDataAccess.Text = GenerateDataAccessCode(codeGenerator);

                    WriteToOutputWindow("\nExecuted in " + sw.Elapsed);
                }
                else if (ext == ".codegen")
                {
                    Text += " - " + Path.GetFileNameWithoutExtension(args[0]);

                    var codeGen = new CodeGenFileSerializer();
                    var file = codeGen.LoadFile(args[0]);
                    rtbGeneratedCodeEntities.Text = file.GeneratedCode.Entities;
                    rtbGeneratedCodeDataAccess.Text = file.GeneratedCode.DataAccessCode;

                    var fi = new FileInfo(file.DataSource);
                    string generatedNamespace = GetType().Namespace + "." + fi.Name.Replace(fi.Extension, string.Empty);
                    string connectionString = "Data Source=" + file.DataSource;
                    SqlCeDatabase database = new SqlCeDatabase(generatedNamespace, connectionString);
                    PopulateTables(database.Tables);
                }
            }
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

                var fi = new FileInfo(dialog.FileName);
                fi.Attributes = FileAttributes.Normal;

                dataSource = dialog.FileName;
                var sw = Stopwatch.StartNew();

                var codeGenerator = CreateCodeGenerator(dataSource);
                rtbGeneratedCodeEntities.Text = GenerateEntitiesCode(codeGenerator);
                rtbGeneratedCodeDataAccess.Text = GenerateDataAccessCode(codeGenerator);

                WriteToOutputWindow("\nExecuted in " + sw.Elapsed);

                Text += " - Untitled";
            }
        }

        private string GenerateEntitiesCode(CodeGenerator codeGenerator)
        {
            WriteToOutputWindow("Generating Entities Code");
            codeGenerator.ClearCode();
            codeGenerator.WriteHeaderInformation();
            codeGenerator.GenerateEntities();

            var generatedCode = codeGenerator.GetCode();
            return generatedCode;
        }

        private string GenerateDataAccessCode(CodeGenerator codeGenerator)
        {
            WriteToOutputWindow("Generating Data Access Code");
            codeGenerator.ClearCode();
            codeGenerator.WriteHeaderInformation();
            codeGenerator.GenerateDataAccessLayer();

            var generatedCode = codeGenerator.GetCode();
            return generatedCode;
        }

        private CodeGenerator CreateCodeGenerator(string inputFileName)
        {
            var fi = new FileInfo(inputFileName);
            var generatedNamespace = "ChristianHelle.DatabaseTools.SqlCe." + fi.Name.Replace(fi.Extension, string.Empty);
            var connectionString = "Data Source=" + inputFileName;

            WriteToOutputWindow("Analyzing Database...");
            var database = new SqlCeDatabase(generatedNamespace, connectionString);

            WriteToOutputWindow(string.Format("Found {0} tables", database.Tables.Count));
            PopulateTables(database.Tables);

            var factory = new CodeGeneratorFactory(database);
            var codeGenerator = factory.Create();
            return codeGenerator;
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
            if (!launchedWithArgument)
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
                rtbGeneratedCodeEntities.Text = file.GeneratedCode.Entities;
                rtbGeneratedCodeDataAccess.Text = file.GeneratedCode.DataAccessCode;

                FileInfo fi = new FileInfo(file.DataSource);
                string generatedNamespace = GetType().Namespace + "." + fi.Name.Replace(fi.Extension, string.Empty);
                string connectionString = "Data Source=" + file.DataSource;
                SqlCeDatabase database = new SqlCeDatabase(generatedNamespace, connectionString);
                PopulateTables(database.Tables);

                Text += " - " + Path.GetFileNameWithoutExtension(dialog.FileName);
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
                    GeneratedCode = new GeneratedCode
                    {
                        Entities = rtbGeneratedCodeEntities.Text,
                        DataAccessCode = rtbGeneratedCodeDataAccess.Text
                    },
                    DataSource = dataSource
                };
                var serializer = new CodeGenFileSerializer();
                serializer.SaveFile(codeGen, dialog.FileName);

                Text += " - " + Path.GetFileNameWithoutExtension(dialog.FileName);
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

        int currentCodeViewTab = 0;

        private void tabGeneratedCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentCodeViewTab = tabGeneratedCode.SelectedIndex;
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentCodeViewTab == 1)
                rtbGeneratedCodeDataAccess.Undo();

            if (currentCodeViewTab == 0)
                rtbGeneratedCodeEntities.Undo();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentCodeViewTab == 1)
                rtbGeneratedCodeDataAccess.Redo();

            if (currentCodeViewTab == 0)
                rtbGeneratedCodeEntities.Redo();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentCodeViewTab == 1)
                rtbGeneratedCodeDataAccess.ActiveTextAreaControl.TextArea.ClipboardHandler.Cut(sender, e);

            if (currentCodeViewTab == 0)
                rtbGeneratedCodeEntities.ActiveTextAreaControl.TextArea.ClipboardHandler.Cut(sender, e);
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentCodeViewTab == 1)
                rtbGeneratedCodeDataAccess.ActiveTextAreaControl.TextArea.ClipboardHandler.Copy(sender, e);

            if (currentCodeViewTab == 0)
                rtbGeneratedCodeEntities.ActiveTextAreaControl.TextArea.ClipboardHandler.Copy(sender, e);
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentCodeViewTab == 1)
                rtbGeneratedCodeDataAccess.ActiveTextAreaControl.TextArea.ClipboardHandler.Paste(sender, e);

            if (currentCodeViewTab == 0)
                rtbGeneratedCodeEntities.ActiveTextAreaControl.TextArea.ClipboardHandler.Paste(sender, e);
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentCodeViewTab == 1)
                rtbGeneratedCodeDataAccess.ActiveTextAreaControl.TextArea.ClipboardHandler.SelectAll(sender, e);

            if (currentCodeViewTab == 0)
                rtbGeneratedCodeEntities.ActiveTextAreaControl.TextArea.ClipboardHandler.SelectAll(sender, e);
        }
        #endregion

        private void compileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabOutput.SelectedTab = tabPageCompilerOutput;
            CompileCSharp30();
        }

        private void CompileCSharp30()
        {
            rtbCompilerOutput.ResetText();
            CreateOutputCSharpFile();

            var sw = Stopwatch.StartNew();

            var csc = Environment.ExpandEnvironmentVariables(@"%SystemRoot%\Microsoft.Net\Framework\v3.5\csc.exe");
            var args = string.Format(@"/target:library /reference:""{0}\System.Data.SqlServerCe.dll"" ""{0}\Output.cs""", Environment.CurrentDirectory);

            WriteToCompilerOutputWindow("Compiling using C# 3.0");
            WriteToCompilerOutputWindow(string.Format("\n{0} {1}", csc, args));

            var psi = new ProcessStartInfo(csc, args);
            psi.RedirectStandardOutput = true;
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;

            var process = Process.Start(psi);
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            WriteToCompilerOutputWindow(output);
            WriteToCompilerOutputWindow("Executed in " + sw.Elapsed);
        }

        private void CreateOutputCSharpFile()
        {
            using (var stream = File.CreateText("Output.cs"))
            {
                stream.Write(rtbGeneratedCodeEntities.Text);
                stream.WriteLine();
                stream.Write(rtbGeneratedCodeDataAccess.Text);
            }
        }

        void WriteToOutputWindow(string text)
        {
            rtbOutput.Text += "\n" + text;
            rtbOutput.SelectionStart = rtbOutput.TextLength;
            rtbOutput.ScrollToCaret();
            rtbOutput.Update();

            Trace.WriteLine(text);
        }

        void WriteToCompilerOutputWindow(string text)
        {
            rtbCompilerOutput.Text += "\n" + text;
            rtbCompilerOutput.SelectionStart = rtbCompilerOutput.TextLength;
            rtbCompilerOutput.ScrollToCaret();
            rtbCompilerOutput.Update();

            Trace.WriteLine(text);
        }

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] filePaths = (string[])(e.Data.GetData(DataFormats.FileDrop));
                var ext = Path.GetExtension(filePaths[0]).ToLower();
                if (ext == ".sdf")
                {
                    Text += " - Untitled";

                    var sw = Stopwatch.StartNew();

                    var codeGenerator = CreateCodeGenerator(dataSource);
                    rtbGeneratedCodeEntities.Text = GenerateEntitiesCode(codeGenerator);
                    rtbGeneratedCodeDataAccess.Text = GenerateDataAccessCode(codeGenerator);

                    WriteToOutputWindow("\nExecuted in " + sw.Elapsed);
                }
                else if (ext == ".codegen")
                {
                    Text += " - " + Path.GetFileNameWithoutExtension(filePaths[0]);

                    var codeGen = new CodeGenFileSerializer();
                    var file = codeGen.LoadFile(filePaths[0]);
                    rtbGeneratedCodeEntities.Text = file.GeneratedCode.Entities;
                    rtbGeneratedCodeDataAccess.Text = file.GeneratedCode.DataAccessCode;

                    var fi = new FileInfo(file.DataSource);
                    string generatedNamespace = GetType().Namespace + "." + fi.Name.Replace(fi.Extension, string.Empty);
                    string connectionString = "Data Source=" + file.DataSource;
                    SqlCeDatabase database = new SqlCeDatabase(generatedNamespace, connectionString);
                    PopulateTables(database.Tables);
                }
            }
        }
    }
}
