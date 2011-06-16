using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using CodeGenWPF.Views;
using System.Data;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using ChristianHelle.DatabaseTools.SqlCe.CodeGenCore;
using ICSharpCode.AvalonEdit.Document;
using ChristianHelle.DatabaseTools.SqlCe;
using System.Drawing;
using System.Collections.ObjectModel;

namespace CodeGenWPF.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string dataSource;
        private bool launchedWithArgument;
        private SqlCeDatabase database;
        private readonly string path;

        public MainViewModel()
        {
            Tree = new ObservableCollection<TreeViewItem>();
            EntitySourceCode = new TextDocument();
            DataAccessSourceCode = new TextDocument();
            EntityUnitTestSourceCode = new TextDocument();
            DataAccessUnitTestSourceCode = new TextDocument();

            path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SQLCE Code Generator");
        }

        public string Text { get; set; }

        public string Output { get; set; }
        public DataTable TableData { get; set; }
        public string CompilerOutput { get; set; }
        public string UnitTestResults { get; set; }
        public ObservableCollection<TreeViewItem> Tree { get; set; }
        public int SelectedTabIndex { get; set; }

        public TextDocument EntitySourceCode { get; set; }
        public TextDocument DataAccessSourceCode { get; set; }
        public TextDocument EntityUnitTestSourceCode { get; set; }
        public TextDocument DataAccessUnitTestSourceCode { get; set; }

        private bool isBusyAnalyzingTables;
        public bool IsBusyAnalyzingTables
        {
            get { return isBusyAnalyzingTables; }
            set
            {
                isBusyAnalyzingTables = value;
                RaisePropertyChanged("IsBusyAnalyzingTables");
            }
        }

        private bool isBusyGeneratingCode;
        public bool IsBusyGeneratingCode
        {
            get { return isBusyGeneratingCode; }
            set
            {
                isBusyGeneratingCode = value;
                RaisePropertyChanged("IsBusyGeneratingCode");
            }
        }

        public void ShowAboutBox()
        {
            var about = new AboutBox(null);
            about.ShowDialog();
        }

        public void New()
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

        public void Open()
        {

        }

        public void Save()
        {

        }

        public void SaveAs()
        {

        }

        private void WriteToOutputWindow(string text)
        {
            Output += Environment.NewLine + text;
            RaisePropertyChanged("Output");

            Trace.WriteLine(text);
        }

        public void GenerateCode()
        {
            Output = null;
            var codeGenerator = CreateCodeGenerator(dataSource);
            EntitySourceCode.Text = GenerateEntitiesCode(codeGenerator);
            DataAccessSourceCode.Text = GenerateDataAccessCode(codeGenerator);
            EntityUnitTestSourceCode.Text = GenerateEntityUnitTestsCode(codeGenerator);
            DataAccessUnitTestSourceCode.Text = GenerateDataAccessUnitTestsCode(codeGenerator);

            RaisePropertyChanged("EntitySourceCode");
            RaisePropertyChanged("DataAccessSourceCode");
            RaisePropertyChanged("EntityUnitTestSourceCode");
            RaisePropertyChanged("DataAccessUnitTestSourceCode");
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
            var rootNode = new TreeViewItem { Header = "Database Tables" };
            rootNode.ExpandSubtree();

            foreach (var item in list)
            {
                var node = new TreeViewItem { Header = item.DisplayName };
                node.Tag = item;
                node.FontWeight = FontWeights.Bold;
                node.ExpandSubtree();
                rootNode.Items.Add(node);

                var columns = new TreeViewItem { Header = "Columns" };
                columns.FontWeight = FontWeights.Normal;
                columns.ExpandSubtree();
                node.Items.Add(columns);

                foreach (var column in item.Columns)
                {
                    var columnNode = new TreeViewItem { Header = column.Value.DisplayName };
                    columnNode.Tag = new KeyValuePair<string, string>(item.Name, column.Value.Name);
                    //columnNode.Items.Add("Ordinal Position - " + column.Value.Ordinal);
                    if (column.Value.IsPrimaryKey)
                        columnNode.Items.Add("Primary Key");
                    if (column.Value.AutoIncrement.HasValue)
                        columnNode.Items.Add("Auto Increment");
                    if (column.Value.IsForeignKey)
                        columnNode.Items.Add("Foreign Key");
                    columnNode.Items.Add(new TreeViewItem { Header = "Database Type - " + column.Value.DatabaseType });
                    columnNode.Items.Add(new TreeViewItem { Header = "Managed Type - " + column.Value.ManagedType });
                    if (column.Value.ManagedType.Equals(typeof(string)))
                        columnNode.Items.Add("Max Length - " + column.Value.MaxLength);
                    columnNode.Items.Add(new TreeViewItem { Header = "Allows Null - " + column.Value.AllowsNull });
                    columns.Items.Add(columnNode);
                }
            }

            Tree.Clear();
            Tree.Add(rootNode);
            RaisePropertyChanged("Tree");
        }

        private void ClearAllControls()
        {
            Output = null;
            TableData = null;
            CompilerOutput = null;
            UnitTestResults = null;
        }

        public void CompileCSharp30()
        {
            SelectedTabIndex = 2;
            RaisePropertyChanged("SelectedTabIndex");

            CompilerOutput = null;
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

            File.Delete(string.Format("{0}\\Entities.cs", path));
            File.Delete(string.Format("{0}\\DataAccess.cs", path));
            File.Delete(string.Format("{0}\\EntityUnitTests.cs", path));
            File.Delete(string.Format("{0}\\DataAccessUnitTests.cs", path));

            WriteToCompilerOutputWindow("Executed in " + sw.Elapsed);
        }

        void WriteToCompilerOutputWindow(string text)
        {
            CompilerOutput += Environment.NewLine + text;
            RaisePropertyChanged("CompilerOutput");

            Trace.WriteLine(text);
        }

        private void CreateOutputCSharpFile()
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            using (var stream = File.CreateText(Path.Combine(path, "Entities.cs")))
            {
                stream.Write(EntitySourceCode.Text);
                stream.WriteLine();
            }
            using (var stream = File.CreateText(Path.Combine(path, "DataAccess.cs")))
            {
                stream.Write(DataAccessSourceCode.Text);
                stream.WriteLine();
            }
            using (var stream = File.CreateText(Path.Combine(path, "EntityUnitTests.cs")))
            {
                stream.Write(EntityUnitTestSourceCode.Text);
                stream.WriteLine();
            }
            using (var stream = File.CreateText(Path.Combine(path, "DataAccessUnitTests.cs")))
            {
                stream.Write(DataAccessUnitTestSourceCode.Text);
                stream.WriteLine();
            }
        }

        public void RunUnitTests()
        {
            CompileCSharp30();

            SelectedTabIndex = 3;
            RaisePropertyChanged("SelectedTabIndex");
            WriteToTestResultsWindow("Executing tests...");

            var fi = new FileInfo(dataSource);
            fi.Attributes = FileAttributes.Normal;

            var sw = Stopwatch.StartNew();
            string output = ExecuteUnitTestRunner();

            UnitTestResults = null;
            WriteToTestResultsWindow(output);
            WriteToTestResultsWindow(Environment.NewLine + "Executed in " + sw.Elapsed);
        }

        void WriteToTestResultsWindow(string text)
        {
            UnitTestResults += Environment.NewLine + text;
            RaisePropertyChanged("UnitTestResults");

            Trace.WriteLine(text);
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
    }
}
