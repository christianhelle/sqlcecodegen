using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using ChristianHelle.DatabaseTools.SqlCe.CodeGenCore;
using ChristianHelle.DatabaseTools.SqlCe.CodeGenGUI.Properties;
using ICSharpCode.TextEditor.Document;
using Microsoft.SqlServer.MessageBox;
using System.Data.SqlServerCe;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenGUI
{
    public partial class MainForm : Form
    {
        private const string TITLE = "SQL Compact Code Generator";
        private const string LANGUAGE = "C#";

        private string dataSource;
        private readonly bool launchedWithArgument;
        private SqlCeDatabase database;
        private readonly string appDataPath;
        private const string MSTEST = "MSTest";
        private const string NUNIT = "NUnit";
        private const string XUNIT = "xUnit";
        private const string NETCF = "NETCF";
        private const string WP7 = "Mango";

        public MainForm(string[] args)
        {
            InitializeComponent();
            dataGridView.DoubleBuffered(true);

            rtbGeneratedCodeEntities.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy(LANGUAGE);
            rtbGeneratedCodeDataAccess.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy(LANGUAGE);
            rtbGeneratedCodeEntityUnitTests.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy(LANGUAGE);
            rtbGeneratedCodeDataAccessUnitTests.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy(LANGUAGE);
            appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), TITLE);

            LoadSettings();

            if (args == null || args.Length != 1) return;
            launchedWithArgument = true;
            var argument = args[0];

            var ext = Path.GetExtension(argument);
            if (string.IsNullOrEmpty(ext)) return;
            switch (ext.ToLower())
            {
                case ".sdf":
                    Text = string.Format("{0} - Untitled", TITLE);
                    var sw = Stopwatch.StartNew();
                    GenerateCode();
                    WriteToOutputWindow(Environment.NewLine + "Executed in " + sw.Elapsed);
                    break;
                case ".codegen":
                    Text = string.Format("{0} - {1}", TITLE, Path.GetFileNameWithoutExtension(argument));
                    LoadFile(argument);
                    break;
            }
        }

        public override sealed string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        private void LoadFile(string argument)
        {
            var sw = Stopwatch.StartNew();
            ClearAllControls();

            WriteToOutputWindow("Loading file...");

            var codeGen = new CodeGenFileSerializer();
            var file = codeGen.LoadFile(argument);

            dataSource = file.DataSource;
            var fi = new FileInfo(dataSource);
            string generatedNamespace = GetType().Namespace + "." + fi.Name.Replace(fi.Extension, string.Empty);
            string connectionString = "Data Source=" + file.DataSource;

            GetDatabase(file.DataSource, generatedNamespace, connectionString);
            if (database == null)
                return;

            if (!string.IsNullOrEmpty(file.TestFramework))
            {
                Settings.Default.TestFramework = file.TestFramework;
                LoadSettings();
            }

            WriteToOutputWindow(string.Format("Found {0} tables{1}", database.Tables.Count, Environment.NewLine));
            PopulateTables(database.Tables);

            WriteToOutputWindow("Loading Generated Entities Code");
            rtbGeneratedCodeEntities.Text = file.GeneratedCode.Entities;

            WriteToOutputWindow("Loading Generated Data Access Code");
            rtbGeneratedCodeDataAccess.Text = file.GeneratedCode.DataAccessCode;

            if (!string.IsNullOrEmpty(file.GeneratedCode.EntityUnitTests))
            {
                WriteToOutputWindow("Loading Generated Entities Unit Test Code");
                rtbGeneratedCodeEntityUnitTests.Text = file.GeneratedCode.EntityUnitTests;
            }
            else
                entityUnitTestsToolStripMenuItem.Checked = false;

            if (!string.IsNullOrEmpty(file.GeneratedCode.DataAccessUnitTests))
            {
                WriteToOutputWindow("Loading Generated Data Access Unit Test Code");
                rtbGeneratedCodeDataAccessUnitTests.Text = file.GeneratedCode.DataAccessUnitTests;
            }
            else
                dataAccessUnitTestsToolStripMenuItem.Checked = false;

            var lineCount = 0;
            lineCount += rtbGeneratedCodeEntities.Text.GetLineCount();
            lineCount += rtbGeneratedCodeDataAccess.Text.GetLineCount();
            lineCount += rtbGeneratedCodeEntityUnitTests.Text.GetLineCount();
            lineCount += rtbGeneratedCodeDataAccessUnitTests.Text.GetLineCount();

            WriteToOutputWindow(string.Format("{0}Loaded {1} lines of code in {2}{0}", Environment.NewLine, lineCount, sw.Elapsed));
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

                GenerateCode();

                Text = string.Format("{0} - Untitled", TITLE);
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
            var sw = Stopwatch.StartNew();

            ClearAllControls();

            var codeGenerator = CreateCodeGenerator(dataSource);
            if (codeGenerator == null)
                return;

            var entitiesCode = GenerateEntitiesCode(codeGenerator);
            var dataAccessCode = GenerateDataAccessCode(codeGenerator);

            string entityUnitTestsCode, dataAccessUnitTestsCode;
            try
            {
                entityUnitTestsCode = GenerateEntityUnitTestsCode(codeGenerator);
                dataAccessUnitTestsCode = GenerateDataAccessUnitTestsCode(codeGenerator);
            }
            catch (NotSupportedException)
            {
                entityUnitTestsCode = string.Empty;
                dataAccessUnitTestsCode = string.Empty;
            }

            var lineCount = 0;
            lineCount += entitiesCode.GetLineCount();
            lineCount += dataAccessCode.GetLineCount();
            lineCount += entityUnitTestsCode.GetLineCount();
            lineCount += dataAccessUnitTestsCode.GetLineCount();

            WriteToOutputWindow(string.Format("{0}Generated {1} lines of code in {2}{0}", Environment.NewLine, lineCount, sw.Elapsed));

            WriteToOutputWindow("Loading Generated Entities Code");
            rtbGeneratedCodeEntities.Text = entitiesCode;

            WriteToOutputWindow("Loading Generated Data Access Code");
            rtbGeneratedCodeDataAccess.Text = dataAccessCode;

            if (!string.IsNullOrEmpty(entityUnitTestsCode))
            {
                WriteToOutputWindow("Loading Generated Entities Unit Test Code");
                rtbGeneratedCodeEntityUnitTests.Text = entityUnitTestsCode;
            }

            if (!string.IsNullOrEmpty(dataAccessUnitTestsCode))
            {
                WriteToOutputWindow("Loading Generated Data Access Unit Test Code");
                rtbGeneratedCodeDataAccessUnitTests.Text = dataAccessUnitTestsCode;
            }

            WriteToOutputWindow(string.Format("{0}Executed in {1}", Environment.NewLine, sw.Elapsed));
        }

        private string GenerateDataAccessUnitTestsCode(CodeGenerator codeGenerator)
        {
            if (!Convert.ToBoolean(Settings.Default.GenerateDataAccessUnitTests))
                return null;

            WriteToOutputWindow("Generating Data Access Unit Tests Code");
            var unitTestGenerator = UnitTestCodeGeneratorFactory.Create(codeGenerator.Database, Settings.Default.TestFramework, Settings.Default.Target);
            unitTestGenerator.WriteHeaderInformation();
            unitTestGenerator.GenerateDataAccessLayer();
            var code = unitTestGenerator.GetCode();
            return code;
        }

        private string GenerateEntityUnitTestsCode(CodeGenerator codeGenerator)
        {
            if (!Convert.ToBoolean(Settings.Default.GenerateEntityUnitTests))
                return null;

            WriteToOutputWindow("Generating Entity Unit Tests Code");
            var unitTestGenerator = UnitTestCodeGeneratorFactory.Create(codeGenerator.Database, Settings.Default.TestFramework, Settings.Default.Target);
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
            var connectionString = GetConnectionString(inputFileName);

            WriteToOutputWindow("Analyzing Database...");
            GetDatabase(inputFileName, generatedNamespace, connectionString);
            if (database == null)
                return null;

            WriteToOutputWindow(string.Format("Found {0} tables{1}", database.Tables.Count, Environment.NewLine));
            PopulateTables(database.Tables);

            var factory = new CodeGeneratorFactory(database);
            var codeGenerator = factory.Create("C#", Settings.Default.Target);
            return codeGenerator;
        }

        private void GetDatabase(string inputFileName, string generatedNamespace, string connectionString)
        {
            try
            {
                database = new SqlCeDatabase(generatedNamespace, connectionString);
            }
            catch (SqlCeException e)
            {
                if (e.NativeError == 25028 || e.NativeError == 25140 || e.Message.ToLower().Contains("password"))
                {
                    var password = PromptForPassword();
                    if (!string.IsNullOrEmpty(password))
                        GetDatabase(inputFileName, generatedNamespace, GetConnectionString(inputFileName, password));
                }
            }
        }

        private string PromptForPassword()
        {
            using (var form = new PasswordForm())
            {
                form.Owner = this;
                var result = form.ShowDialog();
                if (result != DialogResult.OK)
                {
                    database = null;
                    WriteToOutputWindow(Environment.NewLine + "Operation Cancelled...");
                    return null;
                }
                return form.Password;
            }
        }

        private static string GetConnectionString(string inputFileName, string password = null)
        {
            return string.Format("Data Source={0}; Password={1}", inputFileName, password);
        }

        private void PopulateTables(IEnumerable<Table> list)
        {
            var rootNode = new TreeNode(new FileInfo(dataSource).Name);
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
                dialog.Filter = "SQL Compact Code Generator files (*.codegen)|*.codegen";
                if (dialog.ShowDialog() == DialogResult.Cancel)
                    return;

                dataSource = dialog.FileName;
                LoadFile(dataSource);
                //var codeGen = new CodeGenFileSerializer();
                //CodeGenFile file = codeGen.LoadFile(dialog.FileName);
                //rtbGeneratedCodeEntities.Text = file.GeneratedCode.Entities;
                //rtbGeneratedCodeDataAccess.Text = file.GeneratedCode.DataAccessCode;
                //rtbGeneratedCodeEntityUnitTests.Text = file.GeneratedCode.EntityUnitTests;
                //rtbGeneratedCodeDataAccessUnitTests.Text = file.GeneratedCode.DataAccessUnitTests;

                //FileInfo fi = new FileInfo(file.DataSource);
                //string generatedNamespace = GetType().Namespace + "." + fi.Name.Replace(fi.Extension, string.Empty);
                //string connectionString = "Data Source=" + file.DataSource;
                //database = new SqlCeDatabase(generatedNamespace, connectionString);
                //PopulateTables(database.Tables);

                //Text = string.Format("{0} - {1}", TITLE, Path.GetFileNameWithoutExtension(dialog.FileName));
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
                dialog.Filter = "SQL Compact Code Generator files (*.codegen)|*.codegen";
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
                                      DataSource = dataSource,
                                      TestFramework = Settings.Default.TestFramework
                                  };
                var serializer = new CodeGenFileSerializer();
                serializer.SaveFile(codeGen, dialog.FileName);

                Text = string.Format("{0} - {1}", TITLE, Path.GetFileNameWithoutExtension(dialog.FileName));
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
                Invoke((Action)delegate
                {
                    //if (!string.IsNullOrEmpty(errorMessage))
                    //    WriteToOutputWindow(Environment.NewLine + "Error: " + errorMessage);
                    //else
                    //    WriteToOutputWindow(Environment.NewLine + "Error: " + ex.Message);

                    var exception = new ApplicationException(errorMessage, ex);
                    var mbox = new ExceptionMessageBox(exception) { Caption = "Error" };
                    mbox.Show(null);
                });
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

        int currentCodeViewTab;

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
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            SafeOperation(delegate
            {
                if (!e.Data.GetDataPresent(DataFormats.FileDrop))
                    return;

                var filePaths = (string[])(e.Data.GetData(DataFormats.FileDrop));
                var ext = Path.GetExtension(filePaths[0]);
                if (string.IsNullOrEmpty(ext)) return;

                switch (ext.ToLower())
                {
                    case ".sdf":
                        Text = string.Format("{0} - Untitled", TITLE);
                        var sw = Stopwatch.StartNew();
                        GenerateCode();
                        WriteToOutputWindow(Environment.NewLine + "Executed in " + sw.Elapsed);
                        break;

                    case ".codegen":
                        Text = string.Format("{0} - {1}", TITLE, Path.GetFileNameWithoutExtension(filePaths[0]));

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
                        break;
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
            CreateOutputFiles();

            var sw = Stopwatch.StartNew();

            var csc = Environment.ExpandEnvironmentVariables(@"%SystemRoot%\Microsoft.Net\Framework\v3.5\csc.exe");
            var args = string.Format(@"/target:library /optimize /out:""{0}\DataAccess.dll"" /reference:""{1}\System.Data.SqlServerCe.dll"" /reference:""{1}\Microsoft.VisualStudio.QualityTools.UnitTestFramework.dll"" /reference:""{1}\NUnit\nunit.framework.dll"" /reference:""{1}\xUnit\xunit.dll"" ""{0}\*.cs""", appDataPath, Environment.CurrentDirectory);

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

            File.Delete(string.Format("{0}\\Entities.cs", appDataPath));
            File.Delete(string.Format("{0}\\DataAccess.cs", appDataPath));
            File.Delete(string.Format("{0}\\EntityUnitTests.cs", appDataPath));
            File.Delete(string.Format("{0}\\DataAccessUnitTests.cs", appDataPath));

            WriteToCompilerOutputWindow("Executed in " + sw.Elapsed);
        }

        private void CreateOutputFiles()
        {
            if (!Directory.Exists(appDataPath))
                Directory.CreateDirectory(appDataPath);

            using (var stream = File.CreateText(Path.Combine(appDataPath, "Entities.cs")))
            {
                stream.Write(rtbGeneratedCodeEntities.Text);
                stream.WriteLine();
            }
            using (var stream = File.CreateText(Path.Combine(appDataPath, "DataAccess.cs")))
            {
                stream.Write(rtbGeneratedCodeDataAccess.Text);
                stream.WriteLine();
            }
            using (var stream = File.CreateText(Path.Combine(appDataPath, "EntityUnitTests.cs")))
            {
                stream.Write(rtbGeneratedCodeEntityUnitTests.Text);
                stream.WriteLine();
            }
            using (var stream = File.CreateText(Path.Combine(appDataPath, "DataAccessUnitTests.cs")))
            {
                stream.Write(rtbGeneratedCodeDataAccessUnitTests.Text);
                stream.WriteLine();
            }

            using (var stream = File.Create(Path.Combine(appDataPath, "xunit.dll")))
            {
                var buffer = File.ReadAllBytes(Path.Combine(Environment.CurrentDirectory, "xUnit\\xunit.dll"));
                stream.Write(buffer, 0, buffer.Length);
            }

            using (var stream = File.Create(Path.Combine(appDataPath, "nunit.framework.dll")))
            {
                var buffer = File.ReadAllBytes(Path.Combine(Environment.CurrentDirectory, "NUnit\\nunit.framework.dll"));
                stream.Write(buffer, 0, buffer.Length);
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
            ThreadPool.QueueUserWorkItem(state => RunUnitTestWorker());
        }

        private void RunUnitTestWorker()
        {
            if (testsRunning)
                return;

            try
            {
                testsRunning = true;

                SafeOperation(() =>
                {
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
                }, "Unable to run unit tests");
            }
            finally
            {
                testsRunning = false;
            }
        }

        private string ExecuteUnitTestRunner()
        {
            string args;
            var testRunner = GetTestRunner(out args);
            var psi = new ProcessStartInfo(testRunner, args) { RedirectStandardOutput = true, CreateNoWindow = true, UseShellExecute = false };
            var process = Process.Start(psi);
            var output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return output;
        }

        private string GetTestRunner(out string args)
        {
            string testRunner;
            switch (Settings.Default.TestFramework)
            {
                case MSTEST:
                    testRunner = Environment.ExpandEnvironmentVariables(@"%VS90COMNTOOLS%\..\IDE\mstest.exe");
                    if (!File.Exists(testRunner))
                        testRunner = Environment.ExpandEnvironmentVariables(@"%VS100COMNTOOLS%\..\IDE\mstest.exe");
                    args = string.Format(@"/testcontainer:""{0}\DataAccess.dll""", appDataPath);
                    break;

                case NUNIT:
                    testRunner = Path.Combine(Environment.CurrentDirectory, "NUnit\\nunit-console.exe");
                    args = string.Format(@" /nodots ""{0}\DataAccess.dll""", appDataPath);
                    break;

                case XUNIT:
                    testRunner = Path.Combine(Environment.CurrentDirectory, "xUnit\\xunit.console.exe");
                    args = string.Format(@" ""{0}\DataAccess.dll"" /silent", appDataPath);
                    break;

                default:
                    throw new ApplicationException("Missing Unit Test Runner");
            }
            return testRunner;
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
            treeView.Nodes.Clear();
            treeView.Update();
            rtbGeneratedCodeEntities.ResetText();
            rtbGeneratedCodeDataAccess.ResetText();
            rtbGeneratedCodeEntityUnitTests.ResetText();
            tabGeneratedCode.Refresh();

            GenerateCode();
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            SafeOperation(() =>
            {
                var table = e.Node.Tag as Table;
                if (table != null)
                {
                    dataGridView.DataSource = database.GetTableData(table);
                    return;
                }

                var columnData = e.Node.Tag as KeyValuePair<string, string>?;
                if (columnData != null)
                {
                    dataGridView.DataSource = database.GetTableData(columnData.Value.Key, columnData.Value.Value);
                    return;
                }

                dataGridView.DataSource = null;
            },
            "Unable to load table data");
        }

        private void dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = false;
            e.ThrowException = false;
        }

        #region Options

        private void LoadSettings()
        {
            switch (Settings.Default.TestFramework)
            {
                case NUNIT:
                    nUnitToolStripMenuItem.Checked = true;
                    mSTestToolStripMenuItem.Checked = false;
                    xUnitToolStripMenuItem.Checked = false;
                    break;
                case MSTEST:
                    nUnitToolStripMenuItem.Checked = false;
                    mSTestToolStripMenuItem.Checked = true;
                    xUnitToolStripMenuItem.Checked = false;
                    break;
                default:
                    nUnitToolStripMenuItem.Checked = false;
                    mSTestToolStripMenuItem.Checked = false;
                    xUnitToolStripMenuItem.Checked = true;
                    break;
            }

            switch (Settings.Default.Target)
            {
                case NETCF:
                    nETCompactFrameworkCompatibleToolStripMenuItem.Checked = true;
                    windowsPhone7MangoToolStripMenuItem.Checked = false;
                    break;
                case WP7:
                    nETCompactFrameworkCompatibleToolStripMenuItem.Checked = false;
                    windowsPhone7MangoToolStripMenuItem.Checked = true;
                    break;
                default:
                    nETCompactFrameworkCompatibleToolStripMenuItem.Checked = true;
                    windowsPhone7MangoToolStripMenuItem.Checked = false;
                    break;
            }

            entityUnitTestsToolStripMenuItem.Checked = Settings.Default.GenerateEntityUnitTests;
            dataAccessUnitTestsToolStripMenuItem.Checked = Settings.Default.GenerateDataAccessUnitTests;
        }

        private void PromptToRegenerateCode()
        {
            var dialogResult = MessageBox.Show(
                "Do you want to re-generate the code?",
                "Re-generate code",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (dialogResult == DialogResult.Yes)
                SafeOperation(GenerateCode);
        }

        private void nUnitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SafeOperation(() =>
            {
                nUnitToolStripMenuItem.Checked = true;
                mSTestToolStripMenuItem.Checked = false;
                xUnitToolStripMenuItem.Checked = false;
                Settings.Default.TestFramework = NUNIT;
                Settings.Default.Save();

                PromptToRegenerateCode();
            });
        }

        private void mSTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SafeOperation(() =>
            {
                nUnitToolStripMenuItem.Checked = false;
                mSTestToolStripMenuItem.Checked = true;
                xUnitToolStripMenuItem.Checked = false;
                Settings.Default.TestFramework = MSTEST;
                Settings.Default.Save();

                PromptToRegenerateCode();
            });
        }

        private void xUnitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SafeOperation(() =>
            {
                nUnitToolStripMenuItem.Checked = false;
                mSTestToolStripMenuItem.Checked = false;
                xUnitToolStripMenuItem.Checked = true;
                Settings.Default.TestFramework = XUNIT;
                Settings.Default.Save();

                PromptToRegenerateCode();
            });
        }

        private void entityUnitTestsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SafeOperation(() =>
            {
                entityUnitTestsToolStripMenuItem.Checked = !entityUnitTestsToolStripMenuItem.Checked;
                Settings.Default.GenerateEntityUnitTests = !Settings.Default.GenerateEntityUnitTests;
                Settings.Default.Save();

                PromptToRegenerateCode();
            });
        }

        private void dataAccessUnitTestsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SafeOperation(() =>
            {
                dataAccessUnitTestsToolStripMenuItem.Checked = !dataAccessUnitTestsToolStripMenuItem.Checked;
                Settings.Default.GenerateDataAccessUnitTests = !Settings.Default.GenerateDataAccessUnitTests;
                Settings.Default.Save();

                PromptToRegenerateCode();
            });
        }

        private void nETCompactFrameworkCompatibleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SafeOperation(() =>
            {
                nETCompactFrameworkCompatibleToolStripMenuItem.Checked = true;
                windowsPhone7MangoToolStripMenuItem.Checked = false;
                Settings.Default.Target = NETCF;
                Settings.Default.Save();

                PromptToRegenerateCode();
            });
        }

        private void windowsPhone7MangoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SafeOperation(() =>
            {
                nETCompactFrameworkCompatibleToolStripMenuItem.Checked = false;
                windowsPhone7MangoToolStripMenuItem.Checked = true;
                Settings.Default.Target = WP7;
                Settings.Default.Save();

                PromptToRegenerateCode();
            });
        }

        #endregion
    }
}
