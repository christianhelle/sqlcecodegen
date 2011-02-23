using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using ChristianHelle.DatabaseTools.SqlCe.CodeGenCore;
using ICSharpCode.TextEditor.Document;

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
            Database database = new Database(generatedNamespace, connectionString);
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
                rootNode.Nodes.Add(node);

                var columns = new TreeNode("Columns");
                node.Nodes.Add(columns);

                foreach (var column in item.Columns)
                    columns.Nodes.Add(string.Format("{0} ({1})", column.Key, column.Value));
            }

            treeView.Nodes.Clear();
            treeView.Nodes.Add(rootNode);
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

                string generatedNamespace = GetType().Namespace + new FileInfo(file.DataSource).Name;
                string connectionString = "Data Source=" + file.DataSource;
                Database database = new Database(generatedNamespace, connectionString);
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
    }
}
