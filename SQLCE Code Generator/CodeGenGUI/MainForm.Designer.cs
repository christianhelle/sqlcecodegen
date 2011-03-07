namespace CodeGenGUI
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Database Tables");
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
            this.buildToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.customizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.treeView = new System.Windows.Forms.TreeView();
            this.tabGeneratedCode = new System.Windows.Forms.TabControl();
            this.tabPageEntities = new System.Windows.Forms.TabPage();
            this.rtbGeneratedCodeEntities = new ICSharpCode.TextEditor.TextEditorControl();
            this.tabPageDataAccess = new System.Windows.Forms.TabPage();
            this.rtbGeneratedCodeDataAccess = new ICSharpCode.TextEditor.TextEditorControl();
            this.tabOutput = new System.Windows.Forms.TabControl();
            this.tabPageOutput = new System.Windows.Forms.TabPage();
            this.rtbOutput = new System.Windows.Forms.RichTextBox();
            this.tabPageCompilerOutput = new System.Windows.Forms.TabPage();
            this.rtbCompilerOutput = new System.Windows.Forms.RichTextBox();
            this.tabPageUnitTests = new System.Windows.Forms.TabPage();
            this.rtbGeneratedCodeUnitTests = new ICSharpCode.TextEditor.TextEditorControl();
            this.menuStrip.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.tabGeneratedCode.SuspendLayout();
            this.tabPageEntities.SuspendLayout();
            this.tabPageDataAccess.SuspendLayout();
            this.tabOutput.SuspendLayout();
            this.tabPageOutput.SuspendLayout();
            this.tabPageCompilerOutput.SuspendLayout();
            this.tabPageUnitTests.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.buildToolStripMenuItem,
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
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripMenuItem.Image")));
            this.openToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
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
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.saveAsToolStripMenuItem.Text = "Save &As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
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
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
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
            this.undoToolStripMenuItem.Click += new System.EventHandler(this.undoToolStripMenuItem_Click);
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.redoToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.redoToolStripMenuItem.Text = "&Redo";
            this.redoToolStripMenuItem.Click += new System.EventHandler(this.redoToolStripMenuItem_Click);
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
            this.cutToolStripMenuItem.Click += new System.EventHandler(this.cutToolStripMenuItem_Click);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("copyToolStripMenuItem.Image")));
            this.copyToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.copyToolStripMenuItem.Text = "&Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("pasteToolStripMenuItem.Image")));
            this.pasteToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.pasteToolStripMenuItem.Text = "&Paste";
            this.pasteToolStripMenuItem.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
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
            this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.selectAllToolStripMenuItem_Click);
            // 
            // buildToolStripMenuItem
            // 
            this.buildToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.compileToolStripMenuItem});
            this.buildToolStripMenuItem.Name = "buildToolStripMenuItem";
            this.buildToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.buildToolStripMenuItem.Text = "Build";
            // 
            // compileToolStripMenuItem
            // 
            this.compileToolStripMenuItem.Name = "compileToolStripMenuItem";
            this.compileToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F6;
            this.compileToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.compileToolStripMenuItem.Text = "Compile";
            this.compileToolStripMenuItem.Click += new System.EventHandler(this.compileToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.customizeToolStripMenuItem,
            this.optionsToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "&Tools";
            this.toolsToolStripMenuItem.Visible = false;
            // 
            // customizeToolStripMenuItem
            // 
            this.customizeToolStripMenuItem.Name = "customizeToolStripMenuItem";
            this.customizeToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.customizeToolStripMenuItem.Text = "&Customize";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.optionsToolStripMenuItem.Text = "&Options";
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
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Location = new System.Drawing.Point(0, 540);
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
            this.splitContainer1.Size = new System.Drawing.Size(784, 516);
            this.splitContainer1.SplitterDistance = 401;
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
            this.splitContainer.Panel2.Controls.Add(this.tabGeneratedCode);
            this.splitContainer.Size = new System.Drawing.Size(784, 401);
            this.splitContainer.SplitterDistance = 202;
            this.splitContainer.TabIndex = 3;
            // 
            // treeView
            // 
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.Location = new System.Drawing.Point(0, 0);
            this.treeView.Name = "treeView";
            treeNode1.Name = "nodeTables";
            treeNode1.Text = "Database Tables";
            this.treeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.treeView.Size = new System.Drawing.Size(202, 401);
            this.treeView.TabIndex = 0;
            // 
            // tabGeneratedCode
            // 
            this.tabGeneratedCode.Controls.Add(this.tabPageEntities);
            this.tabGeneratedCode.Controls.Add(this.tabPageDataAccess);
            this.tabGeneratedCode.Controls.Add(this.tabPageUnitTests);
            this.tabGeneratedCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabGeneratedCode.Location = new System.Drawing.Point(0, 0);
            this.tabGeneratedCode.Name = "tabGeneratedCode";
            this.tabGeneratedCode.SelectedIndex = 0;
            this.tabGeneratedCode.Size = new System.Drawing.Size(578, 401);
            this.tabGeneratedCode.TabIndex = 0;
            this.tabGeneratedCode.SelectedIndexChanged += new System.EventHandler(this.tabGeneratedCode_SelectedIndexChanged);
            // 
            // tabPageEntities
            // 
            this.tabPageEntities.Controls.Add(this.rtbGeneratedCodeEntities);
            this.tabPageEntities.Location = new System.Drawing.Point(4, 22);
            this.tabPageEntities.Name = "tabPageEntities";
            this.tabPageEntities.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageEntities.Size = new System.Drawing.Size(570, 375);
            this.tabPageEntities.TabIndex = 0;
            this.tabPageEntities.Text = "Entities";
            this.tabPageEntities.UseVisualStyleBackColor = true;
            // 
            // rtbGeneratedCodeEntities
            // 
            this.rtbGeneratedCodeEntities.AllowDrop = true;
            this.rtbGeneratedCodeEntities.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbGeneratedCodeEntities.IsReadOnly = false;
            this.rtbGeneratedCodeEntities.Location = new System.Drawing.Point(3, 3);
            this.rtbGeneratedCodeEntities.Name = "rtbGeneratedCodeEntities";
            this.rtbGeneratedCodeEntities.ShowVRuler = false;
            this.rtbGeneratedCodeEntities.Size = new System.Drawing.Size(564, 369);
            this.rtbGeneratedCodeEntities.TabIndex = 2;
            // 
            // tabPageDataAccess
            // 
            this.tabPageDataAccess.Controls.Add(this.rtbGeneratedCodeDataAccess);
            this.tabPageDataAccess.Location = new System.Drawing.Point(4, 22);
            this.tabPageDataAccess.Name = "tabPageDataAccess";
            this.tabPageDataAccess.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDataAccess.Size = new System.Drawing.Size(570, 375);
            this.tabPageDataAccess.TabIndex = 1;
            this.tabPageDataAccess.Text = "Data Access Code";
            this.tabPageDataAccess.UseVisualStyleBackColor = true;
            // 
            // rtbGeneratedCodeDataAccess
            // 
            this.rtbGeneratedCodeDataAccess.AllowDrop = true;
            this.rtbGeneratedCodeDataAccess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbGeneratedCodeDataAccess.IsReadOnly = false;
            this.rtbGeneratedCodeDataAccess.Location = new System.Drawing.Point(3, 3);
            this.rtbGeneratedCodeDataAccess.Name = "rtbGeneratedCodeDataAccess";
            this.rtbGeneratedCodeDataAccess.ShowVRuler = false;
            this.rtbGeneratedCodeDataAccess.Size = new System.Drawing.Size(564, 369);
            this.rtbGeneratedCodeDataAccess.TabIndex = 3;
            // 
            // tabOutput
            // 
            this.tabOutput.Controls.Add(this.tabPageOutput);
            this.tabOutput.Controls.Add(this.tabPageCompilerOutput);
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
            this.rtbOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbOutput.Location = new System.Drawing.Point(3, 3);
            this.rtbOutput.Name = "rtbOutput";
            this.rtbOutput.Size = new System.Drawing.Size(770, 79);
            this.rtbOutput.TabIndex = 0;
            this.rtbOutput.Text = "";
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
            this.rtbCompilerOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbCompilerOutput.Location = new System.Drawing.Point(3, 3);
            this.rtbCompilerOutput.Name = "rtbCompilerOutput";
            this.rtbCompilerOutput.Size = new System.Drawing.Size(770, 79);
            this.rtbCompilerOutput.TabIndex = 1;
            this.rtbCompilerOutput.Text = "";
            // 
            // tabPageUnitTests
            // 
            this.tabPageUnitTests.Controls.Add(this.rtbGeneratedCodeUnitTests);
            this.tabPageUnitTests.Location = new System.Drawing.Point(4, 22);
            this.tabPageUnitTests.Name = "tabPageUnitTests";
            this.tabPageUnitTests.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageUnitTests.Size = new System.Drawing.Size(570, 375);
            this.tabPageUnitTests.TabIndex = 2;
            this.tabPageUnitTests.Text = "Unit Tests";
            this.tabPageUnitTests.UseVisualStyleBackColor = true;
            // 
            // rtbGeneratedCodeUnitTests
            // 
            this.rtbGeneratedCodeUnitTests.AllowDrop = true;
            this.rtbGeneratedCodeUnitTests.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbGeneratedCodeUnitTests.IsReadOnly = false;
            this.rtbGeneratedCodeUnitTests.Location = new System.Drawing.Point(3, 3);
            this.rtbGeneratedCodeUnitTests.Name = "rtbGeneratedCodeUnitTests";
            this.rtbGeneratedCodeUnitTests.ShowVRuler = false;
            this.rtbGeneratedCodeUnitTests.Size = new System.Drawing.Size(564, 369);
            this.rtbGeneratedCodeUnitTests.TabIndex = 3;
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainForm";
            this.Text = "SQL CE Code Generator";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.ResumeLayout(false);
            this.tabGeneratedCode.ResumeLayout(false);
            this.tabPageEntities.ResumeLayout(false);
            this.tabPageDataAccess.ResumeLayout(false);
            this.tabOutput.ResumeLayout(false);
            this.tabPageOutput.ResumeLayout(false);
            this.tabPageCompilerOutput.ResumeLayout(false);
            this.tabPageUnitTests.ResumeLayout(false);
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
        private System.Windows.Forms.ToolStripMenuItem customizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripMenuItem buildToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem compileToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.TabControl tabOutput;
        private System.Windows.Forms.TabPage tabPageOutput;
        private System.Windows.Forms.RichTextBox rtbOutput;
        private System.Windows.Forms.TabControl tabGeneratedCode;
        private System.Windows.Forms.TabPage tabPageEntities;
        private ICSharpCode.TextEditor.TextEditorControl rtbGeneratedCodeEntities;
        private System.Windows.Forms.TabPage tabPageDataAccess;
        private ICSharpCode.TextEditor.TextEditorControl rtbGeneratedCodeDataAccess;
        private System.Windows.Forms.TabPage tabPageCompilerOutput;
        private System.Windows.Forms.RichTextBox rtbCompilerOutput;
        private System.Windows.Forms.TabPage tabPageUnitTests;
        private ICSharpCode.TextEditor.TextEditorControl rtbGeneratedCodeUnitTests;

    }
}

