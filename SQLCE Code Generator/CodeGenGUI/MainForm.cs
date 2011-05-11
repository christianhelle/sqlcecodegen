using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using ChristianHelle.DatabaseTools.SqlCe.CodeGenCore;
using ICSharpCode.TextEditor.Document;
using System.Reflection;
using Microsoft.SqlServer.MessageBox;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenGUI
{
    public partial class MainForm : Form
    {
        private string dataSource;
        private bool launchedWithArgument;
        private SqlCeDatabase database;
        private readonly string path;

        public MainForm(string[] args)
        {
            InitializeComponent();

            rtbGeneratedCodeEntities.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy("C#");
            rtbGeneratedCodeDataAccess.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy("C#");
            rtbGeneratedCodeEntityUnitTests.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy("C#");
            rtbGeneratedCodeDataAccessUnitTests.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy("C#");
            path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SQLCE Code Generator");

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
            database = new SqlCeDatabase(generatedNamespace, connectionString);
            PopulateTables(database.Tables);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SafeOperation(NewFile);
        }

        private void NewFile()
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                dialog.Filter = "Database files (*.sdf)|*.sdf";
                if (dialog.ShowDialog() == DialogResult.Cancel)
                    return;

                ClearAllControls();

                var fi = new FileInfo(dialog.FileName);
                fi.Attributes = FileAttributes.Normal;

                dataSource = dialog.FileName;
                var sw = Stopwatch.StartNew();

                GenerateCode();

                WriteToOutputWindow(Environment.NewLine + "Executed in " + sw.Elapsed);

                Text = "SQL CE Code Generator - Untitled";
            }
        }

        private void ClearAllControls()
        {
            rtbUnitTestOutput.ResetText();
            rtbOutput.ResetText();
            rtbGeneratedCodeEntityUnitTests.ResetText();
            rtbGeneratedCodeEntities.ResetText();
            rtbGeneratedCodeDataAccessUnitTests.ResetText();
            rtbGeneratedCodeDataAccess.ResetText();
            rtbCompilerOutput.ResetText();
            treeView.Nodes.Clear();

            tabOutput.SelectedTab = tabPageOutput;

            Refresh();
        }

        private void GenerateCode()
        {
            rtbOutput.ResetText();
            var codeGenerator = CreateCodeGenerator(dataSource);
            rtbGeneratedCodeEntities.Text = GenerateEntitiesCode(codeGenerator);
            rtbGeneratedCodeDataAccess.Text = GenerateDataAccessCode(codeGenerator);
            rtbGeneratedCodeEntityUnitTests.Text = GenerateEntityUnitTestsCode(codeGenerator);
            rtbGeneratedCodeDataAccessUnitTests.Text = GenerateDataAccessUnitTestsCode(codeGenerator);
        }

        private string GenerateDataAccessUnitTestsCode(CodeGenerator codeGenerator)
        {
            WriteToOutputWindow("Generating Data Access Unit Tests Code");
            var unitTestGenerator = new NUnitTestCodeGenerator(codeGenerator.Database);
            unitTestGenerator.WriteHeaderInformation();
            unitTestGenerator.GenerateDataAccessLayer();
            var code = unitTestGenerator.GetCode();
            return code;
        }

        private string GenerateEntityUnitTestsCode(CodeGenerator codeGenerator)
        {
            WriteToOutputWindow("Generating Entity Unit Tests Code");
            var unitTestGenerator = new NUnitTestCodeGenerator(codeGenerator.Database);
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
            database = new SqlCeDatabase(generatedNamespace, connectionString);

            WriteToOutputWindow(string.Format("Found {0} tables", database.Tables.Count));
            PopulateTables(database.Tables);

            var factory = new CodeGeneratorFactory(database);
            var codeGenerator = factory.Create();
            return codeGenerator;
        }

        private void PopulateTables(IEnumerable<Table> list)
        {
            var rootNode = new TreeNode("Database Tables");
            rootNode.Expand();

            foreach (var item in list)
            {
                var node = new TreeNode(item.DisplayName);
                node.Tag = item;
                node.NodeFont = new Font(treeView.Font, FontStyle.Bold);
                node.Expand();
                rootNode.Nodes.Add(node);

                var columns = new TreeNode("Columns");
                columns.Expand();
                node.Nodes.Add(columns);

                foreach (var column in item.Columns)
                {
                    var columnNode = new TreeNode(column.Value.DisplayName);
                    columnNode.Tag = new KeyValuePair<string, string>(item.Name, column.Value.Name);
                    //columnNode.Nodes.Add("Ordinal Position - " + column.Value.Ordinal);
                    if (column.Value.IsPrimaryKey)
                        columnNode.Nodes.Add("Primary Key");
                    if (column.Value.AutoIncrement.HasValue)
                        columnNode.Nodes.Add("Auto Increment");
                    if (column.Value.IsForeignKey)
                        columnNode.Nodes.Add("Foreign Key");
                    columnNode.Nodes.Add("Database Type - " + column.Value.DatabaseType);
                    columnNode.Nodes.Add("Managed Type - " + column.Value.ManagedType);
                    if (column.Value.ManagedType.Equals(typeof(string)))
                        columnNode.Nodes.Add("Max Length - " + column.Value.MaxLength);
                    columnNode.Nodes.Add("Allows Null - " + column.Value.AllowsNull);
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
                SafeOperation(NewFile);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SafeOperation(OpenFile);
        }

        private void OpenFile()
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                dialog.Filter = "SQLCE Code Generator files (*.codegen)|*.codegen";
                if (dialog.ShowDialog() == DialogResult.Cancel)
                    return;

                dataSource = dialog.FileName;
                var codeGen = new CodeGenFileSerializer();
                CodeGenFile file = codeGen.LoadFile(dialog.FileName);
                rtbGeneratedCodeEntities.Text = file.GeneratedCode.Entities;
                rtbGeneratedCodeDataAccess.Text = file.GeneratedCode.DataAccessCode;
                rtbGeneratedCodeEntityUnitTests.Text = file.GeneratedCode.EntityUnitTests;
                rtbGeneratedCodeDataAccessUnitTests.Text = file.GeneratedCode.DataAccessUnitTests;

                FileInfo fi = new FileInfo(file.DataSource);
                string generatedNamespace = GetType().Namespace + "." + fi.Name.Replace(fi.Extension, string.Empty);
                string connectionString = "Data Source=" + file.DataSource;
                database = new SqlCeDatabase(generatedNamespace, connectionString);
                PopulateTables(database.Tables);

                Text = "SQL CE Code Generator - " + Path.GetFileNameWithoutExtension(dialog.FileName);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SafeOperation(SaveFile);
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
                                                              EntityUnitTests = rtbGeneratedCodeEntityUnitTests.Text,
                                                              DataAccessUnitTests = rtbGeneratedCodeDataAccessUnitTests.Text
                                                          },
                                      DataSource = dataSource
                                  };
                var serializer = new CodeGenFileSerializer();
                serializer.SaveFile(codeGen, dialog.FileName);

                Text = "SQL CE Code Generator - " + Path.GetFileNameWithoutExtension(dialog.FileName);
            }
        }

        void SafeOperation(Action action, string errorMessage = null)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(errorMessage))
                    WriteToOutputWindow(Environment.NewLine + "Error: " + errorMessage);
                else
                    WriteToOutputWindow(Environment.NewLine + "Error: " + ex.Message);

                var exception = new ApplicationException(errorMessage, ex);
                var mbox = new ExceptionMessageBox(exception) { Caption = "Error" };
                mbox.Show(null);
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SafeOperation(SaveFile);
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
                rtbGeneratedCodeEntityUnitTests.Undo();

            if (currentCodeViewTab == 3)
                rtbGeneratedCodeDataAccessUnitTests.Undo();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentCodeViewTab == 1)
                rtbGeneratedCodeDataAccess.Redo();

            if (currentCodeViewTab == 0)
                rtbGeneratedCodeEntities.Redo();

            if (currentCodeViewTab == 2)
                rtbGeneratedCodeEntityUnitTests.Redo();

            if (currentCodeViewTab == 3)
                rtbGeneratedCodeDataAccessUnitTests.Redo();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentCodeViewTab == 1)
                rtbGeneratedCodeDataAccess.ActiveTextAreaControl.TextArea.ClipboardHandler.Cut(sender, e);

            if (currentCodeViewTab == 0)
                rtbGeneratedCodeEntities.ActiveTextAreaControl.TextArea.ClipboardHandler.Cut(sender, e);

            if (currentCodeViewTab == 2)
                rtbGeneratedCodeEntityUnitTests.ActiveTextAreaControl.TextArea.ClipboardHandler.Cut(sender, e);

            if (currentCodeViewTab == 3)
                rtbGeneratedCodeDataAccessUnitTests.ActiveTextAreaControl.TextArea.ClipboardHandler.Cut(sender, e);
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentCodeViewTab == 1)
                rtbGeneratedCodeDataAccess.ActiveTextAreaControl.TextArea.ClipboardHandler.Copy(sender, e);

            if (currentCodeViewTab == 0)
                rtbGeneratedCodeEntities.ActiveTextAreaControl.TextArea.ClipboardHandler.Copy(sender, e);

            if (currentCodeViewTab == 2)
                rtbGeneratedCodeEntityUnitTests.ActiveTextAreaControl.TextArea.ClipboardHandler.Copy(sender, e);

            if (currentCodeViewTab == 3)
                rtbGeneratedCodeDataAccessUnitTests.ActiveTextAreaControl.TextArea.ClipboardHandler.Copy(sender, e);
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentCodeViewTab == 1)
                rtbGeneratedCodeDataAccess.ActiveTextAreaControl.TextArea.ClipboardHandler.Paste(sender, e);

            if (currentCodeViewTab == 0)
                rtbGeneratedCodeEntities.ActiveTextAreaControl.TextArea.ClipboardHandler.Paste(sender, e);

            if (currentCodeViewTab == 2)
                rtbGeneratedCodeEntityUnitTests.ActiveTextAreaControl.TextArea.ClipboardHandler.Paste(sender, e);

            if (currentCodeViewTab == 3)
                rtbGeneratedCodeDataAccessUnitTests.ActiveTextAreaControl.TextArea.ClipboardHandler.Paste(sender, e);
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentCodeViewTab == 1)
                rtbGeneratedCodeDataAccess.ActiveTextAreaControl.TextArea.ClipboardHandler.SelectAll(sender, e);

            if (currentCodeViewTab == 0)
                rtbGeneratedCodeEntities.ActiveTextAreaControl.TextArea.ClipboardHandler.SelectAll(sender, e);

            if (currentCodeViewTab == 2)
                rtbGeneratedCodeEntityUnitTests.ActiveTextAreaControl.TextArea.ClipboardHandler.SelectAll(sender, e);

            if (currentCodeViewTab == 3)
                rtbGeneratedCodeDataAccessUnitTests.ActiveTextAreaControl.TextArea.ClipboardHandler.SelectAll(sender, e);
        }
        #endregion

        #region Drag and Drop handling
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
            SafeOperation(delegate
            {
                if (!e.Data.GetDataPresent(DataFormats.FileDrop))
                    return;
                var filePaths = (string[])(e.Data.GetData(DataFormats.FileDrop));
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
                    rtbGeneratedCodeEntityUnitTests.Text = file.GeneratedCode.EntityUnitTests;
                    rtbGeneratedCodeDataAccessUnitTests.Text = file.GeneratedCode.DataAccessUnitTests;

                    var fi = new FileInfo(file.DataSource);
                    string generatedNamespace = GetType().Namespace + "." + fi.Name.Replace(fi.Extension, string.Empty);
                    string connectionString = "Data Source=" + file.DataSource;
                    database = new SqlCeDatabase(generatedNamespace, connectionString);
                    PopulateTables(database.Tables);
                }
            });
        }
        #endregion

        #region Compile Code
        private void compileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabOutput.SelectedTab = tabPageCompilerOutput;
            SafeOperation(CompileCSharp30);
        }

        private void CompileCSharp30()
        {
            rtbCompilerOutput.ResetText();
            CreateOutputCSharpFile();

            var sw = Stopwatch.StartNew();


            var csc = Environment.ExpandEnvironmentVariables(@"%SystemRoot%\Microsoft.Net\Framework\v3.5\csc.exe");
            var args = string.Format(@"/target:library /optimize /out:""{0}\DataAccess.dll"" /reference:""{1}\System.Data.SqlServerCe.dll"" /reference:""{1}\Microsoft.VisualStudio.QualityTools.UnitTestFramework.dll"" /reference:""{1}\nunit.framework.dll"" ""{0}\*.cs""", path, Environment.CurrentDirectory);

            WriteToCompilerOutputWindow("Compiling using C# 3.0");
            WriteToCompilerOutputWindow(string.Format(Environment.NewLine + "Executing {0} {1}" + Environment.NewLine, csc, args));

            var psi = new ProcessStartInfo(csc, args)
                          {
                              RedirectStandardOutput = true,
                              CreateNoWindow = true,
                              UseShellExecute = false
                          };

            var process = Process.Start(psi);
            var output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            WriteToCompilerOutputWindow(output);

            //var list = new List<string> 
            //{
            //    string.Format("{0}\\Entities.cs", path),
            //    string.Format("{0}\\DataAccess.cs", path),
            //    string.Format("{0}\\EntityUnitTests.cs", path),
            //    string.Format("{0}\\DataAccessUnitTests.cs", path)
            //};
            //var result = CodeCompiler.CompileCSharpFiles(list.ToArray());

            //foreach (var item in result.Output)
            //    WriteToCompilerOutputWindow(item);

            File.Delete(string.Format("{0}\\Entities.cs", path));
            File.Delete(string.Format("{0}\\DataAccess.cs", path));
            File.Delete(string.Format("{0}\\EntityUnitTests.cs", path));
            File.Delete(string.Format("{0}\\DataAccessUnitTests.cs", path));

            WriteToCompilerOutputWindow("Executed in " + sw.Elapsed);
        }

        private void CreateOutputCSharpFile()
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            using (var stream = File.CreateText(Path.Combine(path, "Entities.cs")))
            {
                stream.Write(rtbGeneratedCodeEntities.Text);
                stream.WriteLine();
            }
            using (var stream = File.CreateText(Path.Combine(path, "DataAccess.cs")))
            {
                stream.Write(rtbGeneratedCodeDataAccess.Text);
                stream.WriteLine();
            }
            using (var stream = File.CreateText(Path.Combine(path, "EntityUnitTests.cs")))
            {
                stream.Write(rtbGeneratedCodeEntityUnitTests.Text);
                stream.WriteLine();
            }
            using (var stream = File.CreateText(Path.Combine(path, "DataAccessUnitTests.cs")))
            {
                stream.Write(rtbGeneratedCodeDataAccessUnitTests.Text);
                stream.WriteLine();
            }
        }
        #endregion

        #region Run Unit Tests
        private void runUnitTestsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SafeOperation(ExecuteUnitTests);
        }

        private void ExecuteUnitTests()
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
            WriteToTestResultsWindow("Executing tests...");
            ThreadPool.QueueUserWorkItem((state) => RunUnitTestWorker());
        }

        private void RunUnitTestWorker()
        {
            try
            {
                testsRunning = true;

                var fi = new FileInfo(dataSource);
                fi.Attributes = FileAttributes.Normal;

                var sw = Stopwatch.StartNew();
                string output = ExecuteUnitTestRunner();

                Invoke((Action)delegate
                {
                    rtbUnitTestOutput.ResetText();
                    WriteToTestResultsWindow(output);
                    WriteToTestResultsWindow(Environment.NewLine + "Executed in " + sw.Elapsed);
                });
            }
            finally
            {
                testsRunning = false;
            }
        }

        private string ExecuteUnitTestRunner()
        {
            //var testRunner = Environment.ExpandEnvironmentVariables(@"%VS90COMNTOOLS%\..\IDE\mstest.exe");
            //if (!File.Exists(testRunner))
            //    testRunner = Environment.ExpandEnvironmentVariables(@"%VS100COMNTOOLS%\..\IDE\mstest.exe");
            //var args = string.Format(@"/testcontainer:""{0}\DataAccess.dll""", path);

            var testRunner = Path.Combine(Environment.CurrentDirectory, "NUnit\\nunit-console.exe");
            var args = string.Format(@" /nodots ""{0}\DataAccess.dll""", path);

            var psi = new ProcessStartInfo(testRunner, args);
            psi.RedirectStandardOutput = true;
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;

            var process = Process.Start(psi);
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return output;
        }
        #endregion

        #region Output Window
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
        #endregion

        private void regenerateCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sw = Stopwatch.StartNew();

            treeView.Nodes.Clear();
            treeView.Update();
            rtbGeneratedCodeEntities.ResetText();
            rtbGeneratedCodeDataAccess.ResetText();
            rtbGeneratedCodeEntityUnitTests.ResetText();
            tabGeneratedCode.Refresh();

            GenerateCode();
            WriteToOutputWindow(Environment.NewLine + "Executed in " + sw.Elapsed);
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            SafeOperation((Action)delegate
            {
                var table = e.Node.Tag as Table;
                if (table != null)
                {
                    dataGridView.DataSource = database.GetTableData(table);
                    return;
                }

                var columnData = e.Node.Tag as Nullable<KeyValuePair<string, string>>;
                if (columnData != null)
                {
                    dataGridView.DataSource = database.GetTableData(columnData.Value.Key, columnData.Value.Value);
                    return;
                }

                dataGridView.DataSource = null;
            }, "Unable to load table data");
        }

        private void dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
            e.ThrowException = false;
        }
    }
}