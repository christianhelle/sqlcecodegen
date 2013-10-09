namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenGUI
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Database Tables");
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("Source Code");
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("Unit Tests");
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("Mocks");
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.codeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buildToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runUnitTestsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.regenerateCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testFrameworkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nUnitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mSTestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xUnitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.codeGenerationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.entityUnitTestsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataAccessUnitTestsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.targetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nETCompactFrameworkCompatibleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windowsPhone7MangoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lINQToSQLDataContextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.treeView = new System.Windows.Forms.TreeView();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.rtbCode = new ICSharpCode.TextEditor.TextEditorControl();
            this.treeViewFiles = new System.Windows.Forms.TreeView();
            this.tabOutput = new System.Windows.Forms.TabControl();
            this.tabPageOutput = new System.Windows.Forms.TabPage();
            this.rtbOutput = new System.Windows.Forms.TextBox();
            this.tabTableData = new System.Windows.Forms.TabPage();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.tabPageCompilerOutput = new System.Windows.Forms.TabPage();
            this.rtbCompilerOutput = new System.Windows.Forms.TextBox();
            this.tabPageTestResults = new System.Windows.Forms.TabPage();
            this.rtbUnitTestOutput = new System.Windows.Forms.TextBox();
            this.advancedOptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabOutput.SuspendLayout();
            this.tabPageOutput.SuspendLayout();
            this.tabTableData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.tabPageCompilerOutput.SuspendLayout();
            this.tabPageTestResults.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.codeToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(784, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.toolStripSeparator,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("newToolStripMenuItem.Image")));
            this.newToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.newToolStripMenuItem.Text = "&New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.NewToolStripMenuItemClick);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripMenuItem.Image")));
            this.openToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.OpenToolStripMenuItemClick);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(143, 6);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripMenuItem.Image")));
            this.saveToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.SaveToolStripMenuItemClick);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.saveAsToolStripMenuItem.Text = "Save &As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.SaveAsToolStripMenuItemClick);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(143, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItemClick);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem,
            this.toolStripSeparator3,
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.toolStripSeparator4,
            this.selectAllToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "&Edit";
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.undoToolStripMenuItem.Text = "&Undo";
            this.undoToolStripMenuItem.Click += new System.EventHandler(this.UndoToolStripMenuItemClick);
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.redoToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.redoToolStripMenuItem.Text = "&Redo";
            this.redoToolStripMenuItem.Click += new System.EventHandler(this.RedoToolStripMenuItemClick);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(141, 6);
            // 
            // cutToolStripMenuItem
            // 
            this.cutToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("cutToolStripMenuItem.Image")));
            this.cutToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            this.cutToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.cutToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.cutToolStripMenuItem.Text = "Cu&t";
            this.cutToolStripMenuItem.Click += new System.EventHandler(this.CutToolStripMenuItemClick);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("copyToolStripMenuItem.Image")));
            this.copyToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.copyToolStripMenuItem.Text = "&Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.CopyToolStripMenuItemClick);
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("pasteToolStripMenuItem.Image")));
            this.pasteToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.pasteToolStripMenuItem.Text = "&Paste";
            this.pasteToolStripMenuItem.Click += new System.EventHandler(this.PasteToolStripMenuItemClick);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(141, 6);
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.selectAllToolStripMenuItem.Text = "Select &All";
            this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.SelectAllToolStripMenuItemClick);
            // 
            // codeToolStripMenuItem
            // 
            this.codeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.buildToolStripMenuItem,
            this.runUnitTestsToolStripMenuItem,
            this.regenerateCodeToolStripMenuItem});
            this.codeToolStripMenuItem.Name = "codeToolStripMenuItem";
            this.codeToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.codeToolStripMenuItem.Text = "Code";
            // 
            // buildToolStripMenuItem
            // 
            this.buildToolStripMenuItem.Name = "buildToolStripMenuItem";
            this.buildToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F6;
            this.buildToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.buildToolStripMenuItem.Text = "Build";
            this.buildToolStripMenuItem.Click += new System.EventHandler(this.CompileToolStripMenuItemClick);
            // 
            // runUnitTestsToolStripMenuItem
            // 
            this.runUnitTestsToolStripMenuItem.Name = "runUnitTestsToolStripMenuItem";
            this.runUnitTestsToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.runUnitTestsToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.runUnitTestsToolStripMenuItem.Text = "Run Unit Tests";
            this.runUnitTestsToolStripMenuItem.Click += new System.EventHandler(this.RunUnitTestsToolStripMenuItemClick);
            // 
            // regenerateCodeToolStripMenuItem
            // 
            this.regenerateCodeToolStripMenuItem.Name = "regenerateCodeToolStripMenuItem";
            this.regenerateCodeToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.regenerateCodeToolStripMenuItem.Text = "Re-generate Code";
            this.regenerateCodeToolStripMenuItem.Click += new System.EventHandler(this.RegenerateCodeToolStripMenuItemClick);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem,
            this.exportToolStripMenuItem,
            this.advancedOptionsToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testFrameworkToolStripMenuItem,
            this.codeGenerationToolStripMenuItem,
            this.targetToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.optionsToolStripMenuItem.Text = "Configuration";
            // 
            // testFrameworkToolStripMenuItem
            // 
            this.testFrameworkToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nUnitToolStripMenuItem,
            this.mSTestToolStripMenuItem,
            this.xUnitToolStripMenuItem});
            this.testFrameworkToolStripMenuItem.Name = "testFrameworkToolStripMenuItem";
            this.testFrameworkToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.testFrameworkToolStripMenuItem.Text = "Test Framework";
            // 
            // nUnitToolStripMenuItem
            // 
            this.nUnitToolStripMenuItem.Checked = true;
            this.nUnitToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.nUnitToolStripMenuItem.Name = "nUnitToolStripMenuItem";
            this.nUnitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.nUnitToolStripMenuItem.Text = "NUnit";
            this.nUnitToolStripMenuItem.Click += new System.EventHandler(this.NUnitToolStripMenuItemClick);
            // 
            // mSTestToolStripMenuItem
            // 
            this.mSTestToolStripMenuItem.Name = "mSTestToolStripMenuItem";
            this.mSTestToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.mSTestToolStripMenuItem.Text = "MSTest";
            this.mSTestToolStripMenuItem.Click += new System.EventHandler(this.MSTestToolStripMenuItemClick);
            // 
            // xUnitToolStripMenuItem
            // 
            this.xUnitToolStripMenuItem.Name = "xUnitToolStripMenuItem";
            this.xUnitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.xUnitToolStripMenuItem.Text = "xUnit";
            this.xUnitToolStripMenuItem.Click += new System.EventHandler(this.XUnitToolStripMenuItemClick);
            // 
            // codeGenerationToolStripMenuItem
            // 
            this.codeGenerationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.entityUnitTestsToolStripMenuItem,
            this.dataAccessUnitTestsToolStripMenuItem});
            this.codeGenerationToolStripMenuItem.Name = "codeGenerationToolStripMenuItem";
            this.codeGenerationToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.codeGenerationToolStripMenuItem.Text = "Unit Test Code Generation";
            // 
            // entityUnitTestsToolStripMenuItem
            // 
            this.entityUnitTestsToolStripMenuItem.Checked = true;
            this.entityUnitTestsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.entityUnitTestsToolStripMenuItem.Name = "entityUnitTestsToolStripMenuItem";
            this.entityUnitTestsToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.entityUnitTestsToolStripMenuItem.Text = "Entity Unit Tests";
            this.entityUnitTestsToolStripMenuItem.Click += new System.EventHandler(this.EntityUnitTestsToolStripMenuItemClick);
            // 
            // dataAccessUnitTestsToolStripMenuItem
            // 
            this.dataAccessUnitTestsToolStripMenuItem.Checked = true;
            this.dataAccessUnitTestsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.dataAccessUnitTestsToolStripMenuItem.Name = "dataAccessUnitTestsToolStripMenuItem";
            this.dataAccessUnitTestsToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.dataAccessUnitTestsToolStripMenuItem.Text = "Data Access Unit Tests";
            this.dataAccessUnitTestsToolStripMenuItem.Click += new System.EventHandler(this.DataAccessUnitTestsToolStripMenuItemClick);
            // 
            // targetToolStripMenuItem
            // 
            this.targetToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nETCompactFrameworkCompatibleToolStripMenuItem,
            this.windowsPhone7MangoToolStripMenuItem,
            this.lINQToSQLDataContextToolStripMenuItem});
            this.targetToolStripMenuItem.Name = "targetToolStripMenuItem";
            this.targetToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.targetToolStripMenuItem.Text = "Target";
            // 
            // nETCompactFrameworkCompatibleToolStripMenuItem
            // 
            this.nETCompactFrameworkCompatibleToolStripMenuItem.Checked = true;
            this.nETCompactFrameworkCompatibleToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.nETCompactFrameworkCompatibleToolStripMenuItem.Name = "nETCompactFrameworkCompatibleToolStripMenuItem";
            this.nETCompactFrameworkCompatibleToolStripMenuItem.Size = new System.Drawing.Size(299, 22);
            this.nETCompactFrameworkCompatibleToolStripMenuItem.Text = ".NET Compact Framework Compatible";
            this.nETCompactFrameworkCompatibleToolStripMenuItem.Click += new System.EventHandler(this.NEtCompactFrameworkCompatibleToolStripMenuItemClick);
            // 
            // windowsPhone7MangoToolStripMenuItem
            // 
            this.windowsPhone7MangoToolStripMenuItem.Name = "windowsPhone7MangoToolStripMenuItem";
            this.windowsPhone7MangoToolStripMenuItem.Size = new System.Drawing.Size(299, 22);
            this.windowsPhone7MangoToolStripMenuItem.Text = "Windows Phone LINQ to SQL Data Context";
            this.windowsPhone7MangoToolStripMenuItem.Click += new System.EventHandler(this.WindowsPhone7MangoToolStripMenuItemClick);
            // 
            // lINQToSQLDataContextToolStripMenuItem
            // 
            this.lINQToSQLDataContextToolStripMenuItem.Name = "lINQToSQLDataContextToolStripMenuItem";
            this.lINQToSQLDataContextToolStripMenuItem.Size = new System.Drawing.Size(299, 22);
            this.lINQToSQLDataContextToolStripMenuItem.Text = "LINQ to SQL Data Context";
            this.lINQToSQLDataContextToolStripMenuItem.Visible = false;
            this.lINQToSQLDataContextToolStripMenuItem.Click += new System.EventHandler(this.LInqtoSqlDataContextToolStripMenuItemClick);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.exportToolStripMenuItem.Text = "&Export Files";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.ExportToolStripMenuItemClick);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator5,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(113, 6);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.aboutToolStripMenuItem.Text = "&About...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.AboutToolStripMenuItemClick);
            // 
            // statusStrip
            // 
            this.statusStrip.Location = new System.Drawing.Point(0, 539);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(784, 22);
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip1";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabOutput);
            this.splitContainer1.Size = new System.Drawing.Size(784, 515);
            this.splitContainer1.SplitterDistance = 400;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.treeView);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer.Size = new System.Drawing.Size(784, 400);
            this.splitContainer.SplitterDistance = 187;
            this.splitContainer.TabIndex = 3;
            // 
            // treeView
            // 
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.Location = new System.Drawing.Point(0, 0);
            this.treeView.Name = "treeView";
            treeNode9.Name = "nodeTables";
            treeNode9.Text = "Database Tables";
            this.treeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode9});
            this.treeView.Size = new System.Drawing.Size(187, 400);
            this.treeView.TabIndex = 0;
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TablesTreeViewAfterSelect);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.rtbCode);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.treeViewFiles);
            this.splitContainer2.Size = new System.Drawing.Size(593, 400);
            this.splitContainer2.SplitterDistance = 406;
            this.splitContainer2.TabIndex = 0;
            // 
            // rtbCode
            // 
            this.rtbCode.AllowDrop = true;
            this.rtbCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbCode.IsReadOnly = false;
            this.rtbCode.Location = new System.Drawing.Point(0, 0);
            this.rtbCode.Name = "rtbCode";
            this.rtbCode.ShowVRuler = false;
            this.rtbCode.Size = new System.Drawing.Size(406, 400);
            this.rtbCode.TabIndex = 5;
            // 
            // treeViewFiles
            // 
            this.treeViewFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewFiles.Location = new System.Drawing.Point(0, 0);
            this.treeViewFiles.Name = "treeViewFiles";
            treeNode10.Name = "nodeSource";
            treeNode10.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            treeNode10.Text = "Source Code";
            treeNode11.Name = "nodeUnitTests";
            treeNode11.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            treeNode11.Text = "Unit Tests";
            treeNode12.Name = "nodeMocks";
            treeNode12.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            treeNode12.Text = "Mocks";
            this.treeViewFiles.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode10,
            treeNode11,
            treeNode12});
            this.treeViewFiles.Size = new System.Drawing.Size(183, 400);
            this.treeViewFiles.TabIndex = 0;
            this.treeViewFiles.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OnSourceTreeViewFilesAfterSelect);
            // 
            // tabOutput
            // 
            this.tabOutput.Controls.Add(this.tabPageOutput);
            this.tabOutput.Controls.Add(this.tabTableData);
            this.tabOutput.Controls.Add(this.tabPageCompilerOutput);
            this.tabOutput.Controls.Add(this.tabPageTestResults);
            this.tabOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabOutput.HotTrack = true;
            this.tabOutput.Location = new System.Drawing.Point(0, 0);
            this.tabOutput.Name = "tabOutput";
            this.tabOutput.SelectedIndex = 0;
            this.tabOutput.Size = new System.Drawing.Size(784, 111);
            this.tabOutput.TabIndex = 0;
            // 
            // tabPageOutput
            // 
            this.tabPageOutput.Controls.Add(this.rtbOutput);
            this.tabPageOutput.Location = new System.Drawing.Point(4, 22);
            this.tabPageOutput.Name = "tabPageOutput";
            this.tabPageOutput.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageOutput.Size = new System.Drawing.Size(776, 85);
            this.tabPageOutput.TabIndex = 0;
            this.tabPageOutput.Text = "Output";
            this.tabPageOutput.UseVisualStyleBackColor = true;
            // 
            // rtbOutput
            // 
            this.rtbOutput.AcceptsReturn = true;
            this.rtbOutput.AcceptsTab = true;
            this.rtbOutput.AllowDrop = true;
            this.rtbOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbOutput.Location = new System.Drawing.Point(3, 3);
            this.rtbOutput.Multiline = true;
            this.rtbOutput.Name = "rtbOutput";
            this.rtbOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.rtbOutput.Size = new System.Drawing.Size(770, 79);
            this.rtbOutput.TabIndex = 0;
            // 
            // tabTableData
            // 
            this.tabTableData.Controls.Add(this.dataGridView);
            this.tabTableData.Location = new System.Drawing.Point(4, 22);
            this.tabTableData.Name = "tabTableData";
            this.tabTableData.Padding = new System.Windows.Forms.Padding(3);
            this.tabTableData.Size = new System.Drawing.Size(776, 85);
            this.tabTableData.TabIndex = 3;
            this.tabTableData.Text = "Table Data";
            this.tabTableData.UseVisualStyleBackColor = true;
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.Location = new System.Drawing.Point(3, 3);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.Size = new System.Drawing.Size(770, 79);
            this.dataGridView.TabIndex = 0;
            this.dataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.DataGridViewDataError);
            // 
            // tabPageCompilerOutput
            // 
            this.tabPageCompilerOutput.Controls.Add(this.rtbCompilerOutput);
            this.tabPageCompilerOutput.Location = new System.Drawing.Point(4, 22);
            this.tabPageCompilerOutput.Name = "tabPageCompilerOutput";
            this.tabPageCompilerOutput.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCompilerOutput.Size = new System.Drawing.Size(776, 85);
            this.tabPageCompilerOutput.TabIndex = 1;
            this.tabPageCompilerOutput.Text = "Compiler Output";
            this.tabPageCompilerOutput.UseVisualStyleBackColor = true;
            // 
            // rtbCompilerOutput
            // 
            this.rtbCompilerOutput.AcceptsReturn = true;
            this.rtbCompilerOutput.AcceptsTab = true;
            this.rtbCompilerOutput.AllowDrop = true;
            this.rtbCompilerOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbCompilerOutput.Location = new System.Drawing.Point(3, 3);
            this.rtbCompilerOutput.Multiline = true;
            this.rtbCompilerOutput.Name = "rtbCompilerOutput";
            this.rtbCompilerOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.rtbCompilerOutput.Size = new System.Drawing.Size(770, 79);
            this.rtbCompilerOutput.TabIndex = 0;
            // 
            // tabPageTestResults
            // 
            this.tabPageTestResults.Controls.Add(this.rtbUnitTestOutput);
            this.tabPageTestResults.Location = new System.Drawing.Point(4, 22);
            this.tabPageTestResults.Name = "tabPageTestResults";
            this.tabPageTestResults.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTestResults.Size = new System.Drawing.Size(776, 85);
            this.tabPageTestResults.TabIndex = 2;
            this.tabPageTestResults.Text = "Test Results";
            this.tabPageTestResults.UseVisualStyleBackColor = true;
            // 
            // rtbUnitTestOutput
            // 
            this.rtbUnitTestOutput.AcceptsReturn = true;
            this.rtbUnitTestOutput.AcceptsTab = true;
            this.rtbUnitTestOutput.AllowDrop = true;
            this.rtbUnitTestOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbUnitTestOutput.Location = new System.Drawing.Point(3, 3);
            this.rtbUnitTestOutput.Multiline = true;
            this.rtbUnitTestOutput.Name = "rtbUnitTestOutput";
            this.rtbUnitTestOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.rtbUnitTestOutput.Size = new System.Drawing.Size(770, 79);
            this.rtbUnitTestOutput.TabIndex = 0;
            // 
            // advancedOptionsToolStripMenuItem
            // 
            this.advancedOptionsToolStripMenuItem.Name = "advancedOptionsToolStripMenuItem";
            this.advancedOptionsToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.advancedOptionsToolStripMenuItem.Text = "Options...";
            this.advancedOptionsToolStripMenuItem.Click += new System.EventHandler(this.advancedOptionsToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainForm";
            this.Text = "SQL Compact Code Generator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFormFormClosing);
            this.Load += new System.EventHandler(this.MainFormLoad);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainFormDragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainFormDragEnter);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.tabOutput.ResumeLayout(false);
            this.tabPageOutput.ResumeLayout(false);
            this.tabPageOutput.PerformLayout();
            this.tabTableData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.tabPageCompilerOutput.ResumeLayout(false);
            this.tabPageCompilerOutput.PerformLayout();
            this.tabPageTestResults.ResumeLayout(false);
            this.tabPageTestResults.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripMenuItem codeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem buildToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.TabControl tabOutput;
        private System.Windows.Forms.TabPage tabPageOutput;
        private System.Windows.Forms.TabPage tabPageCompilerOutput;
        private System.Windows.Forms.TabPage tabPageTestResults;
        private System.Windows.Forms.ToolStripMenuItem runUnitTestsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem regenerateCodeToolStripMenuItem;
        private System.Windows.Forms.TextBox rtbOutput;
        private System.Windows.Forms.TextBox rtbCompilerOutput;
        private System.Windows.Forms.TextBox rtbUnitTestOutput;
        private System.Windows.Forms.TabPage tabTableData;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.ToolStripMenuItem testFrameworkToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nUnitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mSTestToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem codeGenerationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem entityUnitTestsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dataAccessUnitTestsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xUnitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem targetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nETCompactFrameworkCompatibleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem windowsPhone7MangoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lINQToSQLDataContextToolStripMenuItem;
        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TreeView treeViewFiles;
        private ICSharpCode.TextEditor.TextEditorControl rtbCode;
        private System.Windows.Forms.ToolStripMenuItem advancedOptionsToolStripMenuItem;

    }
}