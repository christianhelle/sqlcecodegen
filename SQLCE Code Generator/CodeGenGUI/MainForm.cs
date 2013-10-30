using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ChristianHelle.DatabaseTools.SqlCe.CodeGenCore;
using ChristianHelle.DatabaseTools.SqlCe.CodeGenGUI.Properties;
using ICSharpCode.TextEditor.Document;
using Microsoft.SqlServer.MessageBox;
using Microsoft.VisualBasic;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenGUI
{
    public partial class MainForm : Form
    {
        private const string TITLE = "SQL Compact Code Generator";
        private const string LANGUAGE = "C#";
        private const string MSTEST = "MSTest";
        private const string NUNIT = "NUnit";
        private const string XUNIT = "xUnit";
        private const string NETCF = "NETCF";
        private const string WP7 = "Mango";
        private const string LINQTOSQL = "LinqToSql";

        private string dataSource;
        private ISqlCeDatabase database;
        private readonly Dictionary<string, StringBuilder> generatedCodeFiles;
        private readonly Dictionary<string, StringBuilder> generatedUnitTestFiles;
        private readonly Dictionary<string, StringBuilder> generatedMockFiles;
        private readonly bool launchedWithArgument;
        private readonly string appDataPath;

        public MainForm(string[] args)
        {
            InitializeComponent();

            if (!Settings.Default.WindowSize.IsEmpty)
                Size = Settings.Default.WindowSize;
            else
                Size = new System.Drawing.Size(800, 600);

            if (!Settings.Default.WindowPosition.IsEmpty)
                Location = Settings.Default.WindowPosition;
            else
                CenterToScreen();

            if (Settings.Default.WindowState == FormWindowState.Maximized)
                WindowState = Settings.Default.WindowState;

            dataGridView.DoubleBuffered(true);

            generatedCodeFiles = new Dictionary<string, StringBuilder>();
            generatedUnitTestFiles = new Dictionary<string, StringBuilder>();
            generatedMockFiles = new Dictionary<string, StringBuilder>();

            //rtbGeneratedCodeEntities.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy(LANGUAGE);
            //rtbGeneratedCodeDataAccess.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy(LANGUAGE);
            //rtbGeneratedCodeEntityUnitTests.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy(LANGUAGE);
            rtbCode.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy(LANGUAGE);
            //rtbGeneratedMockDataAccessCode.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy(LANGUAGE);
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
            PopulateDatabaseTables(database.Tables);

            var lineCount = 0;
            lineCount += file.GeneratedCode.Entities.GetLineCount();
            lineCount += file.GeneratedCode.DataAccessCode.GetLineCount();
            lineCount += file.GeneratedCode.EntityUnitTests.GetLineCount();
            lineCount += file.GeneratedCode.DataAccessUnitTests.GetLineCount();

            WriteToOutputWindow(string.Format("{0}Loaded {1} lines of code in {2}{0}", Environment.NewLine, lineCount, sw.Elapsed));
        }

        private void ExitToolStripMenuItemClick(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void NewToolStripMenuItemClick(object sender, EventArgs e)
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
            rtbCode.ResetText();
            rtbCompilerOutput.ResetText();
            treeView.Nodes.Clear();

            tabOutput.SelectedTab = tabPageOutput;

            generatedCodeFiles.Clear();
            generatedUnitTestFiles.Clear();
            generatedMockFiles.Clear();

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

            string entityUnitTestsCode, dataAccessUnitTestsCode, mockDataAccessCode;
            try
            {
                entityUnitTestsCode = GenerateEntityUnitTestsCode(codeGenerator);
                dataAccessUnitTestsCode = GenerateDataAccessUnitTestsCode(codeGenerator);
                mockDataAccessCode = GeneratedMockDataAccessCode(codeGenerator);
            }
            catch (NotSupportedException)
            {
                entityUnitTestsCode = string.Empty;
                dataAccessUnitTestsCode = string.Empty;
                mockDataAccessCode = string.Empty;
            }

            var lineCount = 0;
            lineCount += entitiesCode.GetLineCount();
            lineCount += dataAccessCode.GetLineCount();
            lineCount += entityUnitTestsCode.GetLineCount();
            lineCount += dataAccessUnitTestsCode.GetLineCount();
            lineCount += mockDataAccessCode.GetLineCount();

            WriteToOutputWindow(string.Format("{0}Generated {1} lines of code in {2}{0}", Environment.NewLine, lineCount, sw.Elapsed));

            PopulateSourceTree();

            WriteToOutputWindow(string.Format("{0}Executed in {1}", Environment.NewLine, sw.Elapsed));
        }

        private void PopulateSourceTree()
        {
            foreach (TreeNode node in treeViewFiles.Nodes)
                node.Nodes.Clear();

            PopulateNode(generatedCodeFiles, 0);
            PopulateNode(generatedUnitTestFiles, 1);
            PopulateNode(generatedMockFiles, 2);

            treeViewFiles.ExpandAll();
        }

        private void PopulateNode(Dictionary<string, StringBuilder> files, int treeIndex)
        {
            foreach (var item in files)
                treeViewFiles.Nodes[treeIndex].Nodes.Add(new TreeNode(item.Key + GetFileExtension()) { Tag = item.Value });
        }

        private void AddToCodeFiles(CodeGenerator codeGenerator)
        {
            var header = new StringBuilder();
            if (Settings.Default.WriteHeaderInformation)
                codeGenerator.WriteHeaderInformation(header);
            header.AppendLine();

            foreach (var code in codeGenerator.CodeFiles)
                generatedCodeFiles.Add(code.Key, code.Value.Insert(0, header.ToString()));
        }

        private void AddToUnitTestFiles(CodeGenerator codeGenerator)
        {
            var header = new StringBuilder();
            if (Settings.Default.WriteHeaderInformation)
                codeGenerator.WriteHeaderInformation(header);
            header.AppendLine();

            foreach (var code in codeGenerator.CodeFiles)
            {
                if (code.Key.StartsWith("Mock"))
                    generatedMockFiles.Add(code.Key, code.Value.Insert(0, header.ToString()));
                else
                    generatedUnitTestFiles.Add(code.Key, code.Value.Insert(0, header.ToString()));
            }
        }

        private string GeneratedMockDataAccessCode(CodeGenerator codeGenerator)
        {
            if (!Convert.ToBoolean(Settings.Default.GenerateDataAccessUnitTests) || Settings.Default.Target == LINQTOSQL)
                return null;

            WriteToOutputWindow("Generating Mock Data Access Code");

            var code = new StringBuilder();
            var unitTestGenerator = UnitTestCodeGeneratorFactory.Create(codeGenerator.Database, Settings.Default.TestFramework, Settings.Default.Target);
            SetOptions(unitTestGenerator);
            if (Settings.Default.WriteHeaderInformation)
                unitTestGenerator.WriteHeaderInformation();
            code.Append(unitTestGenerator.GetCode());

            unitTestGenerator.GenerateDataAccessLayer();
            foreach (var keyValue in unitTestGenerator.CodeFiles.Where(keyValue => keyValue.Key.StartsWith("Mock")))
                code.Append(keyValue.Value);

            return code.ToString();
        }

        private string GenerateDataAccessUnitTestsCode(CodeGenerator codeGenerator)
        {
            if (!Convert.ToBoolean(Settings.Default.GenerateDataAccessUnitTests) || Settings.Default.Target == LINQTOSQL)
                return null;

            WriteToOutputWindow("Generating Data Access Unit Tests Code");

            var code = new StringBuilder();
            var unitTestGenerator = UnitTestCodeGeneratorFactory.Create(codeGenerator.Database, Settings.Default.TestFramework, Settings.Default.Target);
            SetOptions(unitTestGenerator);
            if (Settings.Default.WriteHeaderInformation)
                unitTestGenerator.WriteHeaderInformation();
            code.Append(unitTestGenerator.GetCode());

            unitTestGenerator.GenerateDataAccessLayer();
            foreach (var keyValue in unitTestGenerator.CodeFiles.Where(keyValue => !keyValue.Key.StartsWith("Mock")))
                code.Append(keyValue.Value);

            AddToUnitTestFiles(unitTestGenerator);

            return code.ToString();
        }

        private string GenerateEntityUnitTestsCode(CodeGenerator codeGenerator)
        {
            if (!Convert.ToBoolean(Settings.Default.GenerateEntityUnitTests) || Settings.Default.Target == LINQTOSQL)
                return null;

            WriteToOutputWindow("Generating Entity Unit Tests Code");
            var unitTestGenerator = UnitTestCodeGeneratorFactory.Create(codeGenerator.Database, Settings.Default.TestFramework, Settings.Default.Target);
            SetOptions(unitTestGenerator);
            if (Settings.Default.WriteHeaderInformation)
                unitTestGenerator.WriteHeaderInformation();
            unitTestGenerator.GenerateEntities();
            AddToUnitTestFiles(unitTestGenerator);
            var code = unitTestGenerator.GetCode();
            return code;
        }

        private string GenerateEntitiesCode(CodeGenerator codeGenerator)
        {
            WriteToOutputWindow("Generating Entities Code");
            codeGenerator.ClearCode();
            if (Settings.Default.WriteHeaderInformation)
                codeGenerator.WriteHeaderInformation();
            codeGenerator.GenerateEntities();
            var generatedCode = codeGenerator.GetCode();
            return generatedCode;
        }

        private string GenerateDataAccessCode(CodeGenerator codeGenerator)
        {
            WriteToOutputWindow("Generating Data Access Code");
            codeGenerator.ClearCode();
            if (Settings.Default.WriteHeaderInformation)
                codeGenerator.WriteHeaderInformation();
            codeGenerator.GenerateDataAccessLayer();
            AddToCodeFiles(codeGenerator);

            var generatedCode = codeGenerator.GetCode();
            return generatedCode;
        }

        private CodeGenerator CreateCodeGenerator(string inputFileName)
        {
            var generatedNamespace = GetGeneratedNamespace();
            if (string.IsNullOrEmpty(generatedNamespace))
                return null;

            var connectionString = GetConnectionString(inputFileName);

            WriteToOutputWindow("Analyzing Database...");
            GetDatabase(inputFileName, generatedNamespace, connectionString);
            if (database == null)
                return null;

            WriteToOutputWindow(string.Format("Found {0} tables{1}", database.Tables.Count, Environment.NewLine));
            PopulateDatabaseTables(database.Tables);

            var factory = new CodeGeneratorFactory(database);
            var codeGenerator = factory.Create("C#", Settings.Default.Target);
            SetOptions(codeGenerator);
            return codeGenerator;
        }

        private static void SetOptions(CodeGenerator codeGenerator)
        {
            codeGenerator.EntityGeneratorOptions = Settings.Default.EntityOptions ?? new EntityGeneratorOptions();
            codeGenerator.DataAccessLayerGeneratorOptions = Settings.Default.DataLayerOptions ?? new DataAccessLayerGeneratorOptions();
        }

        private static string GetGeneratedNamespace()
        {
            var generatedNamespace = Interaction.InputBox("Enter the namespace to use for the generated code",
                                                          "Default Namespace",
                                                          Settings.Default.DefaultNamespace);

            if (string.IsNullOrEmpty(generatedNamespace))
                return null;

            if (generatedNamespace.EndsWith("."))
            {
                MessageBox.Show("Invalid Namespace", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            Settings.Default.DefaultNamespace = generatedNamespace;
            Settings.Default.Save();

            return generatedNamespace;
        }

        private void GetDatabase(string inputFileName, string generatedNamespace, string connectionString)
        {
            try
            {
                database = SqlCeDatabaseFactory.Create(generatedNamespace, connectionString);
                database.DatabaseFilename = dataSource;
            }
            catch (SqlCeDatabaseException e)
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
            return string.Format("Data Source={0}; Password={1}", inputFileName, password ?? "'';");
        }

        private void PopulateDatabaseTables(IEnumerable<Table> list)
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
                    if (column.Value.ManagedType == typeof(string))
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

        private void MainFormLoad(object sender, EventArgs e)
        {
            if (!launchedWithArgument)
                SafeOperation(NewFile);
        }

        private void MainFormFormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.WindowState = WindowState;
            Settings.Default.WindowSize = Size;
            Settings.Default.WindowPosition = Location;
            Settings.Default.Save();
        }

        private void OpenToolStripMenuItemClick(object sender, EventArgs e)
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
                //string generatedNamespace = GetType().DefaultNamespace + "." + fi.Name.Replace(fi.Extension, string.Empty);
                //string connectionString = "Data Source=" + file.DataSource;
                //database = new SqlCeDatabase(generatedNamespace, connectionString);
                //PopulateDatabaseTables(database.Tables);

                //Text = string.Format("{0} - {1}", TITLE, Path.GetFileNameWithoutExtension(dialog.FileName));
            }
        }

        private void SaveToolStripMenuItemClick(object sender, EventArgs e)
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
                                      //GeneratedCode = new GeneratedCode
                                      //                    {
                                      //                        Entities = rtbGeneratedCodeEntities.Text,
                                      //                        DataAccessCode = rtbGeneratedCodeDataAccess.Text,
                                      //                        EntityUnitTests = rtbGeneratedCodeEntityUnitTests.Text,
                                      //                        DataAccessUnitTests = rtbCode.Text
                                      //                    },
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

        private void SaveAsToolStripMenuItemClick(object sender, EventArgs e)
        {
            SafeOperation(SaveFile);
        }

        private void AboutToolStripMenuItemClick(object sender, EventArgs e)
        {
            using (var about = new AboutBox())
                about.ShowDialog();
        }

        #region Clipboard Handling

        private int currentCodeViewTab = 0;

        //private void TabGeneratedCodeSelectedIndexChanged(object sender, EventArgs e)
        //{
        //    currentCodeViewTab = tabGeneratedCode.SelectedIndex;
        //}

        private void UndoToolStripMenuItemClick(object sender, EventArgs e)
        {
            //if (currentCodeViewTab == 1)
            //    rtbGeneratedCodeDataAccess.Undo();

            //if (currentCodeViewTab == 0)
            //    rtbGeneratedCodeEntities.Undo();

            //if (currentCodeViewTab == 2)
            //    rtbGeneratedCodeEntityUnitTests.Undo();

            //if (currentCodeViewTab == 3)
            rtbCode.Undo();

            //if (currentCodeViewTab == 4)
            //    rtbGeneratedMockDataAccessCode.Undo();
        }

        private void RedoToolStripMenuItemClick(object sender, EventArgs e)
        {
            //if (currentCodeViewTab == 1)
            //    rtbGeneratedCodeDataAccess.Redo();

            //if (currentCodeViewTab == 0)
            //    rtbGeneratedCodeEntities.Redo();

            //if (currentCodeViewTab == 2)
            //    rtbGeneratedCodeEntityUnitTests.Redo();

            if (currentCodeViewTab == 3)
                rtbCode.Redo();

            //if (currentCodeViewTab == 4)
            //    rtbGeneratedMockDataAccessCode.Redo();
        }

        private void CutToolStripMenuItemClick(object sender, EventArgs e)
        {
            //if (currentCodeViewTab == 1)
            //    rtbGeneratedCodeDataAccess.ActiveTextAreaControl.TextArea.ClipboardHandler.Cut(sender, e);

            //if (currentCodeViewTab == 0)
            //    rtbGeneratedCodeEntities.ActiveTextAreaControl.TextArea.ClipboardHandler.Cut(sender, e);

            //if (currentCodeViewTab == 2)
            //    rtbGeneratedCodeEntityUnitTests.ActiveTextAreaControl.TextArea.ClipboardHandler.Cut(sender, e);

            //if (currentCodeViewTab == 3)
            rtbCode.ActiveTextAreaControl.TextArea.ClipboardHandler.Cut(sender, e);

            //if (currentCodeViewTab == 4)
            //    rtbGeneratedMockDataAccessCode.ActiveTextAreaControl.TextArea.ClipboardHandler.Cut(sender, e);
        }

        private void CopyToolStripMenuItemClick(object sender, EventArgs e)
        {
            //if (currentCodeViewTab == 1)
            //    rtbGeneratedCodeDataAccess.ActiveTextAreaControl.TextArea.ClipboardHandler.Copy(sender, e);

            //if (currentCodeViewTab == 0)
            //    rtbGeneratedCodeEntities.ActiveTextAreaControl.TextArea.ClipboardHandler.Copy(sender, e);

            //if (currentCodeViewTab == 2)
            //    rtbGeneratedCodeEntityUnitTests.ActiveTextAreaControl.TextArea.ClipboardHandler.Copy(sender, e);

            //if (currentCodeViewTab == 3)
            rtbCode.ActiveTextAreaControl.TextArea.ClipboardHandler.Copy(sender, e);

            //if (currentCodeViewTab == 4)
            //    rtbGeneratedMockDataAccessCode.ActiveTextAreaControl.TextArea.ClipboardHandler.Copy(sender, e);
        }

        private void PasteToolStripMenuItemClick(object sender, EventArgs e)
        {
            //if (currentCodeViewTab == 1)
            //    rtbGeneratedCodeDataAccess.ActiveTextAreaControl.TextArea.ClipboardHandler.Paste(sender, e);

            //if (currentCodeViewTab == 0)
            //    rtbGeneratedCodeEntities.ActiveTextAreaControl.TextArea.ClipboardHandler.Paste(sender, e);

            //if (currentCodeViewTab == 2)
            //    rtbGeneratedCodeEntityUnitTests.ActiveTextAreaControl.TextArea.ClipboardHandler.Paste(sender, e);

            //if (currentCodeViewTab == 3)
            rtbCode.ActiveTextAreaControl.TextArea.ClipboardHandler.Paste(sender, e);

            //if (currentCodeViewTab == 4)
            //    rtbGeneratedMockDataAccessCode.ActiveTextAreaControl.TextArea.ClipboardHandler.Paste(sender, e);
        }

        private void SelectAllToolStripMenuItemClick(object sender, EventArgs e)
        {
            //if (currentCodeViewTab == 1)
            //    rtbGeneratedCodeDataAccess.ActiveTextAreaControl.TextArea.ClipboardHandler.SelectAll(sender, e);

            //if (currentCodeViewTab == 0)
            //    rtbGeneratedCodeEntities.ActiveTextAreaControl.TextArea.ClipboardHandler.SelectAll(sender, e);

            //if (currentCodeViewTab == 2)
            //    rtbGeneratedCodeEntityUnitTests.ActiveTextAreaControl.TextArea.ClipboardHandler.SelectAll(sender, e);

            //if (currentCodeViewTab == 3)
            rtbCode.ActiveTextAreaControl.TextArea.ClipboardHandler.SelectAll(sender, e);

            //if (currentCodeViewTab == 4)
            //    rtbGeneratedMockDataAccessCode.ActiveTextAreaControl.TextArea.ClipboardHandler.SelectAll(sender, e);
        }
        #endregion

        #region Drag and Drop handling

        private void MainFormDragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
        }

        private void MainFormDragDrop(object sender, DragEventArgs e)
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
                        //rtbGeneratedCodeEntities.Text = file.GeneratedCode.Entities;
                        //rtbGeneratedCodeDataAccess.Text = file.GeneratedCode.DataAccessCode;
                        //rtbGeneratedCodeEntityUnitTests.Text = file.GeneratedCode.EntityUnitTests;
                        //rtbCode.Text = file.GeneratedCode.DataAccessUnitTests;

                        var fi = new FileInfo(file.DataSource);
                        string generatedNamespace = GetType().Namespace + "." + fi.Name.Replace(fi.Extension, string.Empty);
                        string connectionString = "Data Source=" + file.DataSource;
                        database = SqlCeDatabaseFactory.Create(generatedNamespace, connectionString);
                        PopulateDatabaseTables(database.Tables);
                        break;
                }
            });
        }
        #endregion

        #region Compile Code
        private void CompileToolStripMenuItemClick(object sender, EventArgs e)
        {
            tabOutput.SelectedTab = tabPageCompilerOutput;
            SafeOperation(CompileCSharp30);
        }

        private void CompileCSharp30()
        {
            rtbCompilerOutput.ResetText();

            CreateOutputFiles();

            var sw = Stopwatch.StartNew();

            string csc;
            string args;
            if (Settings.Default.Target == "Mango")
            {
                var mangoSdkPath = Environment.GetEnvironmentVariable("ProgramFiles(x86)") + @"\Reference Assemblies\Microsoft\Framework\Silverlight\v4.0\Profile\WindowsPhone71";
                if (!Directory.Exists(mangoSdkPath))
                {
                    WriteToCompilerOutputWindow("Unable to find Windows Phone 7 Mango Libraries");
                    return;
                }

                csc = Environment.ExpandEnvironmentVariables(@"%SystemRoot%\Microsoft.Net\Framework\v4.0.30319\csc.exe");
                args =
                    string.Format(
                        @"/target:library /noconfig /nostdlib+ /optimize /define:DEBUG;TRACE;SILVERLIGHT;WINDOWS_PHONE /out:""{0}\DataAccess.dll"" /reference:""{1}\mscorlib.dll"" /reference:""{1}\mscorlib.extensions.dll"" /reference:""{1}\System.dll"" /reference:""{1}\System.Windows.dll"" /reference:""{1}\System.Core.dll"" /reference:""{1}\System.Data.Linq.dll"" /reference:""{1}\Microsoft.Phone.dll"" ""{0}\*.cs""",
                        appDataPath, mangoSdkPath);

                WriteToCompilerOutputWindow("Compiling using C# 4.0");
            }
            else
            {
                csc = Environment.ExpandEnvironmentVariables(@"%SystemRoot%\Microsoft.Net\Framework\v3.5\csc.exe");
                args =
                    string.Format(
                        @"/target:library /optimize /out:""{0}\DataAccess.dll"" /reference:""{1}\System.Data.SqlServerCe.dll"" /reference:""{1}\Microsoft.VisualStudio.QualityTools.UnitTestFramework.dll"" /reference:""{1}\NUnit\nunit.framework.dll"" /reference:""{1}\xUnit\xunit.dll"" /reference:""System.dll"" /reference:""System.Data.dll""  ""{0}\*.cs""",
                        appDataPath, Environment.CurrentDirectory);

                WriteToCompilerOutputWindow("Compiling using C# 3.0");
            }

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

            File.Delete(string.Format("{0}\\DataAccess.cs", appDataPath));
            File.Delete(string.Format("{0}\\UnitTests.cs", appDataPath));

            WriteToCompilerOutputWindow("Executed in " + sw.Elapsed);
        }

        private void CreateOutputFiles()
        {
            if (!Directory.Exists(appDataPath))
                Directory.CreateDirectory(appDataPath);

            using (var stream = File.CreateText(Path.Combine(appDataPath, "DataAccess.cs")))
            {
                foreach (var item in generatedCodeFiles.Values)
                {
                    stream.Write(item);
                    stream.WriteLine();
                }
            }

            using (var stream = File.CreateText(Path.Combine(appDataPath, "UnitTests.cs")))
            {
                foreach (var item in generatedUnitTestFiles.Values)
                {
                    stream.Write(item);
                    stream.WriteLine();
                }
            }

            using (var stream = File.CreateText(Path.Combine(appDataPath, "Mocks.cs")))
            {
                foreach (var item in generatedMockFiles.Values)
                {
                    stream.Write(item);
                    stream.WriteLine();
                }
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
        private void RunUnitTestsToolStripMenuItemClick(object sender, EventArgs e)
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
                    var testResult = string.Format("{0}\\MSTestResult.trx", appDataPath);
                    if (File.Exists(testResult))
                        File.Delete(testResult);
                    args = string.Format(@"/resultsfile:""{1}"" /testcontainer:""{0}\DataAccess.dll""", appDataPath, testResult);
                    break;

                case NUNIT:
                    testRunner = Path.Combine(Environment.CurrentDirectory, "NUnit\\nunit-console.exe");
                    args = string.Format(@"/xml=""{0}\NUnitTestResult.xml"" /nodots ""{0}\DataAccess.dll""", appDataPath);
                    break;

                case XUNIT:
                    testRunner = Path.Combine(Environment.CurrentDirectory, "xUnit\\xunit.console.exe");
                    args = string.Format(@"""{0}\DataAccess.dll"" /silent /xml ""{0}\xUnitTestResult.xml""", appDataPath);
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

        #region Export Files
        private void ExportFiles()
        {
            string path;
            using (var dialog = new FolderBrowserDialog())
            {
                var dialogResult = dialog.ShowDialog();
                if (dialogResult != DialogResult.OK)
                    return;

                path = dialog.SelectedPath;
            }

            var sourcePath = Path.Combine(path, "Source\\");
            if (Directory.Exists(sourcePath))
                Directory.Delete(sourcePath, true);
            Directory.CreateDirectory(sourcePath);

            var testPath = Path.Combine(path, "Tests\\");
            if (Directory.Exists(testPath))
                Directory.Delete(testPath, true);
            Directory.CreateDirectory(testPath);

            var mockPath = Path.Combine(path, "Mocks\\");
            if (Directory.Exists(mockPath))
                Directory.Delete(mockPath, true);
            Directory.CreateDirectory(mockPath);

            if (generatedCodeFiles.Count > 0)
            {
                foreach (var code in generatedCodeFiles)
                    using (var stream = File.CreateText(sourcePath + code.Key + GetFileExtension()))
                        stream.WriteLine(code.Value);
            }

            if (generatedUnitTestFiles.Count > 0)
            {
                foreach (var code in generatedUnitTestFiles)
                    using (var stream = File.CreateText(testPath + code.Key + GetFileExtension()))
                        stream.WriteLine(code.Value);
            }

            if (generatedMockFiles.Count > 0)
            {
                foreach (var code in generatedMockFiles)
                    using (var stream = File.CreateText(mockPath + code.Key + GetFileExtension()))
                        stream.WriteLine(code.Value);
            }
        }

        private static string GetFileExtension()
        {
            return ".cs";
        }

        #endregion

        private void RegenerateCodeToolStripMenuItemClick(object sender, EventArgs e)
        {
            treeView.Nodes.Clear();
            treeView.Update();

            GenerateCode();
        }

        private void TablesTreeViewAfterSelect(object sender, TreeViewEventArgs e)
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

        private void DataGridViewDataError(object sender, DataGridViewDataErrorEventArgs e)
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
                    lINQToSQLDataContextToolStripMenuItem.Checked = false;
                    //if (!tabGeneratedCode.TabPages.Contains(tabPageEntityUnitTests))
                    //    tabGeneratedCode.TabPages.Add(tabPageEntityUnitTests);
                    //if (!tabGeneratedCode.TabPages.Contains(tabPageGeneratedCode))
                    //    tabGeneratedCode.TabPages.Add(tabPageGeneratedCode);
                    //if (!tabGeneratedCode.TabPages.Contains(tabMockDataAccessCode))
                    //    tabGeneratedCode.TabPages.Add(tabMockDataAccessCode);
                    //if (!tabGeneratedCode.TabPages.Contains(tabPageEntities))
                    //    tabGeneratedCode.TabPages.Add(tabPageEntities);
                    break;
                case WP7:
                    nETCompactFrameworkCompatibleToolStripMenuItem.Checked = false;
                    windowsPhone7MangoToolStripMenuItem.Checked = true;
                    lINQToSQLDataContextToolStripMenuItem.Checked = false;
                    //if (tabGeneratedCode.TabPages.Contains(tabPageEntityUnitTests))
                    //    tabGeneratedCode.TabPages.Remove(tabPageEntityUnitTests);
                    //if (tabGeneratedCode.TabPages.Contains(tabPageGeneratedCode))
                    //    tabGeneratedCode.TabPages.Remove(tabPageGeneratedCode);
                    //if (tabGeneratedCode.TabPages.Contains(tabMockDataAccessCode))
                    //    tabGeneratedCode.TabPages.Remove(tabMockDataAccessCode);
                    //if (!tabGeneratedCode.TabPages.Contains(tabPageEntities))
                    //    tabGeneratedCode.TabPages.Add(tabPageEntities);
                    break;
                case LINQTOSQL:
                    nETCompactFrameworkCompatibleToolStripMenuItem.Checked = false;
                    windowsPhone7MangoToolStripMenuItem.Checked = false;
                    lINQToSQLDataContextToolStripMenuItem.Checked = true;
                    //if (tabGeneratedCode.TabPages.Contains(tabPageEntityUnitTests))
                    //    tabGeneratedCode.TabPages.Remove(tabPageEntityUnitTests);
                    //if (tabGeneratedCode.TabPages.Contains(tabPageGeneratedCode))
                    //    tabGeneratedCode.TabPages.Remove(tabPageGeneratedCode);
                    //if (tabGeneratedCode.TabPages.Contains(tabMockDataAccessCode))
                    //    tabGeneratedCode.TabPages.Remove(tabMockDataAccessCode);
                    //if (tabGeneratedCode.TabPages.Contains(tabPageEntities))
                    //    tabGeneratedCode.TabPages.Remove(tabPageEntities);
                    break;
                default:
                    nETCompactFrameworkCompatibleToolStripMenuItem.Checked = true;
                    windowsPhone7MangoToolStripMenuItem.Checked = false;
                    lINQToSQLDataContextToolStripMenuItem.Checked = false;
                    //if (!tabGeneratedCode.TabPages.Contains(tabPageEntityUnitTests))
                    //    tabGeneratedCode.TabPages.Add(tabPageEntityUnitTests);
                    //if (!tabGeneratedCode.TabPages.Contains(tabPageGeneratedCode))
                    //    tabGeneratedCode.TabPages.Add(tabPageGeneratedCode);
                    //if (!tabGeneratedCode.TabPages.Contains(tabMockDataAccessCode))
                    //    tabGeneratedCode.TabPages.Add(tabMockDataAccessCode);
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

        private void NUnitToolStripMenuItemClick(object sender, EventArgs e)
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

        private void MSTestToolStripMenuItemClick(object sender, EventArgs e)
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

        private void XUnitToolStripMenuItemClick(object sender, EventArgs e)
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

        private void EntityUnitTestsToolStripMenuItemClick(object sender, EventArgs e)
        {
            SafeOperation(() =>
            {
                entityUnitTestsToolStripMenuItem.Checked = !entityUnitTestsToolStripMenuItem.Checked;
                Settings.Default.GenerateEntityUnitTests = !Settings.Default.GenerateEntityUnitTests;
                Settings.Default.Save();

                PromptToRegenerateCode();
            });
        }

        private void DataAccessUnitTestsToolStripMenuItemClick(object sender, EventArgs e)
        {
            SafeOperation(() =>
            {
                dataAccessUnitTestsToolStripMenuItem.Checked = !dataAccessUnitTestsToolStripMenuItem.Checked;
                Settings.Default.GenerateDataAccessUnitTests = !Settings.Default.GenerateDataAccessUnitTests;
                Settings.Default.Save();

                PromptToRegenerateCode();
            });
        }

        private void NEtCompactFrameworkCompatibleToolStripMenuItemClick(object sender, EventArgs e)
        {
            SafeOperation(() =>
            {
                nETCompactFrameworkCompatibleToolStripMenuItem.Checked = true;
                windowsPhone7MangoToolStripMenuItem.Checked = false;
                lINQToSQLDataContextToolStripMenuItem.Checked = false;
                Settings.Default.Target = NETCF;
                Settings.Default.Save();
                LoadSettings();

                PromptToRegenerateCode();
            });
        }

        private void WindowsPhone7MangoToolStripMenuItemClick(object sender, EventArgs e)
        {
            SafeOperation(() =>
            {
                nETCompactFrameworkCompatibleToolStripMenuItem.Checked = false;
                windowsPhone7MangoToolStripMenuItem.Checked = true;
                lINQToSQLDataContextToolStripMenuItem.Checked = false;
                Settings.Default.Target = WP7;
                Settings.Default.Save();
                LoadSettings();

                PromptToRegenerateCode();
            });
        }

        private void LInqtoSqlDataContextToolStripMenuItemClick(object sender, EventArgs e)
        {
            SafeOperation(() =>
            {
                nETCompactFrameworkCompatibleToolStripMenuItem.Checked = false;
                windowsPhone7MangoToolStripMenuItem.Checked = false;
                lINQToSQLDataContextToolStripMenuItem.Checked = true;
                Settings.Default.Target = LINQTOSQL;
                Settings.Default.Save();
                LoadSettings();

                PromptToRegenerateCode();
            });
        }

        #endregion

        private void ExportToolStripMenuItemClick(object sender, EventArgs e)
        {
            SafeOperation(() =>
            {
                ExportFiles();
                return;
            });
        }

        private void OnSourceTreeViewFilesAfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag != null)
                rtbCode.Text = e.Node.Tag.ToString();
            else
                rtbCode.ResetText();
        }

        private void advancedOptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new OptionsForm { Owner = this })
                form.ShowDialog();

            var result = MessageBox.Show("Do you want to re-generate the code?",
                                         "Confirm",
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Question,
                                         MessageBoxDefaultButton.Button1);
            if (result != DialogResult.Yes)
                return;

            treeView.Nodes.Clear();
            treeView.Update();

            GenerateCode();
        }

        private void writeHeaderInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings.Default.WriteHeaderInformation = writeHeaderInformationToolStripMenuItem.Checked;
            Settings.Default.Save();
        }
    }
}
