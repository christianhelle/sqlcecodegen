namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenGUI
{
    partial class OptionsForm
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Entity Generator");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Data Access Layer Generator");
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.treeView = new System.Windows.Forms.TreeView();
            this.entityOptionsUserControl = new ChristianHelle.DatabaseTools.SqlCe.CodeGenGUI.EntityOptionsUserControl();
            this.dataAccessLayerOptionsUserControl = new ChristianHelle.DatabaseTools.SqlCe.CodeGenGUI.DataAccessLayerOptionsUserControl();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer.IsSplitterFixed = true;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.treeView);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.AutoScroll = true;
            this.splitContainer.Panel2.Controls.Add(this.entityOptionsUserControl);
            this.splitContainer.Panel2.Controls.Add(this.dataAccessLayerOptionsUserControl);
            this.splitContainer.Size = new System.Drawing.Size(533, 313);
            this.splitContainer.SplitterDistance = 176;
            this.splitContainer.TabIndex = 0;
            // 
            // treeView
            // 
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.Indent = 10;
            this.treeView.ItemHeight = 20;
            this.treeView.Location = new System.Drawing.Point(0, 0);
            this.treeView.Name = "treeView";
            treeNode1.Name = "nodeEntityGenerator";
            treeNode1.Text = "Entity Generator";
            treeNode2.Name = "nodeDataAccessLayerGenerator";
            treeNode2.Text = "Data Access Layer Generator";
            this.treeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2});
            this.treeView.ShowLines = false;
            this.treeView.Size = new System.Drawing.Size(176, 313);
            this.treeView.TabIndex = 0;
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
            // 
            // entityOptionsUserControl
            // 
            this.entityOptionsUserControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.entityOptionsUserControl.GeneratorOptions = null;
            this.entityOptionsUserControl.Location = new System.Drawing.Point(0, 0);
            this.entityOptionsUserControl.Name = "entityOptionsUserControl";
            this.entityOptionsUserControl.Size = new System.Drawing.Size(336, 564);
            this.entityOptionsUserControl.TabIndex = 0;
            // 
            // dataAccessLayerOptionsUserControl
            // 
            this.dataAccessLayerOptionsUserControl.GeneratorOptions = null;
            this.dataAccessLayerOptionsUserControl.Location = new System.Drawing.Point(0, 0);
            this.dataAccessLayerOptionsUserControl.Name = "dataAccessLayerOptionsUserControl";
            this.dataAccessLayerOptionsUserControl.Size = new System.Drawing.Size(335, 564);
            this.dataAccessLayerOptionsUserControl.TabIndex = 1;
            this.dataAccessLayerOptionsUserControl.Visible = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(446, 328);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(365, 328);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // OptionsForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(533, 363);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.splitContainer);
            this.Name = "OptionsForm";
            this.Text = "Code Generator Options";
            this.Load += new System.EventHandler(this.OptionsForm_Load);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private EntityOptionsUserControl entityOptionsUserControl;
        private DataAccessLayerOptionsUserControl dataAccessLayerOptionsUserControl;
    }
}