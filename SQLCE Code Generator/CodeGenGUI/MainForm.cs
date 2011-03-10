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
            rtbGeneratedCodeUnitTests.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy("C#");

            if (args != null && args.Length == 1)
            {
                launchedWithArgument = true;
                var argument = args[0];

                var ext = Path.GetExtension(argument).ToLower();
                if (ext == ".sdf")
                {
                    Text = "SQL CE Code Generator - Untitled";

                    var sw = Stopwatch.StartNew();

                    GenerateCode();

                    WriteToOutputWindow(Environment.NewLine + "Executed in " + sw.Elapsed);
                }
                else if (ext == ".codegen")
                {
                    Text = "SQL CE Code Generator - " + Path.GetFileNameWithoutExtension(argument);

                    LoadFile(argument);
                }
            }
        }

        private void LoadFile(string argument)
        {
            var codeGen = new CodeGenFileSerializer();
            var file = codeGen.LoadFile(argument);
            rtbGeneratedCodeEntities.Text = file.GeneratedCode.Entities;
            rtbGeneratedCodeDataAccess.Text = file.GeneratedCode.DataAccessCode;

            var fi = new FileInfo(file.DataSource);
            string generatedNamespace = GetType().Namespace + "." + fi.Name.Replace(fi.Extension, string.Empty);
            string connectionString = "Data Source=" + file.DataSource;
            SqlCeDatabase database = new SqlCeDatabase(generatedNamespace, connectionString);
            PopulateTables(database.Tables);
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

                GenerateCode();

                WriteToOutputWindow(Environment.NewLine + "Executed in " + sw.Elapsed);

                Text = "SQL CE Code Generator - Untitled";
            }
        }

        private void GenerateCode()
        {
            rtbOutput.ResetText();
            var codeGenerator = CreateCodeGenerator(dataSource);
            rtbGeneratedCodeEntities.Text = GenerateEntitiesCode(codeGenerator);
            rtbGeneratedCodeDataAccess.Text = GenerateDataAccessCode(codeGenerator);
            rtbGeneratedCodeUnitTests.Text = GenerateUnitTestsCode(codeGenerator); ;
        }

        private string GenerateUnitTestsCode(CodeGenerator codeGenerator)
        {
            WriteToOutputWindow("Generating Unit Tests Code");
            var unitTestGenerator = new UnitTestCodeGenerator(codeGenerator.Database);
            unitTestGenerator.WriteHeaderInformation();
            unitTestGenerator.GenerateEntities();
            var code = unitTestGenerator.GetCode();
            return code;
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
                rtbGeneratedCodeUnitTests.Text = file.GeneratedCode.UnitTests;

                FileInfo fi = new FileInfo(file.DataSource);
                string generatedNamespace = GetType().Namespace + "." + fi.Name.Replace(fi.Extension, string.Empty);
                string connectionString = "Data Source=" + file.DataSource;
                SqlCeDatabase database = new SqlCeDatabase(generatedNamespace, connectionString);
                PopulateTables(database.Tables);

                Text = "SQL CE Code Generator - " + Path.GetFileNameWithoutExtension(dialog.FileName);
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
                        DataAccessCode = rtbGeneratedCodeDataAccess.Text,
                        UnitTests = rtbGeneratedCodeUnitTests.Text
                    },
                    DataSource = dataSource
                };
                var serializer = new CodeGenFileSerializer();
                serializer.SaveFile(codeGen, dialog.FileName);

                Text = "SQL CE Code Generator - " + Path.GetFileNameWithoutExtension(dialog.FileName);
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

            if (currentCodeViewTab == 2)
                rtbGeneratedCodeUnitTests.Undo();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentCodeViewTab == 1)
                rtbGeneratedCodeDataAccess.Redo();

            if (currentCodeViewTab == 0)
                rtbGeneratedCodeEntities.Redo();

            if (currentCodeViewTab == 2)
                rtbGeneratedCodeUnitTests.Redo();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentCodeViewTab == 1)
                rtbGeneratedCodeDataAccess.ActiveTextAreaControl.TextArea.ClipboardHandler.Cut(sender, e);

            if (currentCodeViewTab == 0)
                rtbGeneratedCodeEntities.ActiveTextAreaControl.TextArea.ClipboardHandler.Cut(sender, e);

            if (currentCodeViewTab == 2)
                rtbGeneratedCodeUnitTests.ActiveTextAreaControl.TextArea.ClipboardHandler.Cut(sender, e);
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentCodeViewTab == 1)
                rtbGeneratedCodeDataAccess.ActiveTextAreaControl.TextArea.ClipboardHandler.Copy(sender, e);

            if (currentCodeViewTab == 0)
                rtbGeneratedCodeEntities.ActiveTextAreaControl.TextArea.ClipboardHandler.Copy(sender, e);

            if (currentCodeViewTab == 2)
                rtbGeneratedCodeUnitTests.ActiveTextAreaControl.TextArea.ClipboardHandler.Copy(sender, e);
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentCodeViewTab == 1)
                rtbGeneratedCodeDataAccess.ActiveTextAreaControl.TextArea.ClipboardHandler.Paste(sender, e);

            if (currentCodeViewTab == 0)
                rtbGeneratedCodeEntities.ActiveTextAreaControl.TextArea.ClipboardHandler.Paste(sender, e);

            if (currentCodeViewTab == 2)
                rtbGeneratedCodeUnitTests.ActiveTextAreaControl.TextArea.ClipboardHandler.Paste(sender, e);
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentCodeViewTab == 1)
                rtbGeneratedCodeDataAccess.ActiveTextAreaControl.TextArea.ClipboardHandler.SelectAll(sender, e);

            if (currentCodeViewTab == 0)
                rtbGeneratedCodeEntities.ActiveTextAreaControl.TextArea.ClipboardHandler.SelectAll(sender, e);

            if (currentCodeViewTab == 2)
                rtbGeneratedCodeUnitTests.ActiveTextAreaControl.TextArea.ClipboardHandler.SelectAll(sender, e);
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
            var args = string.Format(@"/target:library /optimize /out:DataAccess.dll /reference:""{0}\System.Data.SqlServerCe.dll"" /reference:""{0}\Microsoft.VisualStudio.QualityTools.UnitTestFramework.dll"" ""{0}\*.cs""", Environment.CurrentDirectory);

            WriteToCompilerOutputWindow("Compiling using C# 3.0");
            WriteToCompilerOutputWindow(string.Format(Environment.NewLine + "Executing {0} {1}" + Environment.NewLine, csc, args));

            var psi = new ProcessStartInfo(csc, args);
            psi.RedirectStandardOutput = true;
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;

            var process = Process.Start(psi);
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            WriteToCompilerOutputWindow(output);
            WriteToCompilerOutputWindow("Executed in " + sw.Elapsed);

            File.Delete(string.Format("{0}\\Entities.cs", Environment.CurrentDirectory));
            File.Delete(string.Format("{0}\\DataAccess.cs", Environment.CurrentDirectory));
            File.Delete(string.Format("{0}\\UnitTests.cs", Environment.CurrentDirectory));
        }

        private void CreateOutputCSharpFile()
        {
            using (var stream = File.CreateText("Entities.cs"))
            {
                stream.Write(rtbGeneratedCodeEntities.Text);
                stream.WriteLine();
            }
            using (var stream = File.CreateText("DataAccess.cs"))
            {
                stream.Write(rtbGeneratedCodeDataAccess.Text);
                stream.WriteLine();
            }
            using (var stream = File.CreateText("UnitTests.cs"))
            {
                stream.Write(rtbGeneratedCodeUnitTests.Text);
                stream.WriteLine();
            }
        }

        void WriteToOutputWindow(string text)
        {
            rtbOutput.Text += Environment.NewLine + text;
            rtbOutput.SelectionStart = rtbOutput.TextLength;
            rtbOutput.ScrollToCaret();
            rtbOutput.Update();

            Trace.WriteLine(text);
        }

        void WriteToCompilerOutputWindow(string text)
        {
            rtbCompilerOutput.Text += Environment.NewLine + text;
            rtbCompilerOutput.SelectionStart = rtbCompilerOutput.TextLength;
            rtbCompilerOutput.ScrollToCaret();
            rtbCompilerOutput.Update();

            Trace.WriteLine(text);
        }

        void WriteToTestResultsWindow(string text)
        {
            rtbUnitTestOutput.Text += Environment.NewLine + text;
            rtbUnitTestOutput.SelectionStart = rtbUnitTestOutput.TextLength;
            rtbUnitTestOutput.ScrollToCaret();
            rtbUnitTestOutput.Update();

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
                    Text = "SQL CE Code Generator - Untitled";

                    var sw = Stopwatch.StartNew();
                    GenerateCode();
                    WriteToOutputWindow(Environment.NewLine + "Executed in " + sw.Elapsed);
                }
                else if (ext == ".codegen")
                {
                    Text = "SQL CE Code Generator - " + Path.GetFileNameWithoutExtension(filePaths[0]);

                    var codeGen = new CodeGenFileSerializer();
                    var file = codeGen.LoadFile(filePaths[0]);
                    rtbGeneratedCodeEntities.Text = file.GeneratedCode.Entities;
                    rtbGeneratedCodeDataAccess.Text = file.GeneratedCode.DataAccessCode;
                    rtbGeneratedCodeUnitTests.Text = file.GeneratedCode.UnitTests;

                    var fi = new FileInfo(file.DataSource);
                    string generatedNamespace = GetType().Namespace + "." + fi.Name.Replace(fi.Extension, string.Empty);
                    string connectionString = "Data Source=" + file.DataSource;
                    SqlCeDatabase database = new SqlCeDatabase(generatedNamespace, connectionString);
                    PopulateTables(database.Tables);
                }
            }
        }

        private void runUnitTestsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (testsRunning)
                return;

            tabOutput.SelectedTab = tabPageCompilerOutput;
            CompileCSharp30();

            tabOutput.SelectedTab = tabPageTestResults;
            rtbUnitTestOutput.ResetText();
            rtbUnitTestOutput.Update();

            RunUnitTests();
        }

        private static volatile bool testsRunning;
        private void RunUnitTests()
        {
            ThreadPool.QueueUserWorkItem((state) =>
            {
                try
                {
                    testsRunning = true;

                    var sw = Stopwatch.StartNew();

                    var mstest = Environment.ExpandEnvironmentVariables(@"%VS90COMNTOOLS%\..\IDE\mstest.exe");
                    var args = string.Format(@"/noresults /testcontainer:""{0}\DataAccess.dll""", Environment.CurrentDirectory);

                    var psi = new ProcessStartInfo(mstest, args);
                    psi.RedirectStandardOutput = true;
                    psi.CreateNoWindow = true;
                    psi.UseShellExecute = false;

                    var process = Process.Start(psi);
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    Invoke((Action)delegate
                    {
                        WriteToTestResultsWindow(output);
                        WriteToTestResultsWindow(Environment.NewLine + "Executed in " + sw.Elapsed);
                    });
                }
                finally
                {
                    testsRunning = false;
                }
            });
        }

        private void regenerateCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sw = Stopwatch.StartNew();

            treeView.Nodes.Clear();
            treeView.Update();
            rtbGeneratedCodeEntities.ResetText();
            rtbGeneratedCodeDataAccess.ResetText();
            rtbGeneratedCodeUnitTests.ResetText();
            tabGeneratedCode.Refresh();

            GenerateCode();
            WriteToOutputWindow(Environment.NewLine + "Executed in " + sw.Elapsed);
        }
    }
}
