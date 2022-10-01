using System.Windows.Forms;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenGUI
{
    partial class EntityOptionsUserControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public BindingSource EntityGeneratorOptionsBindingSource
        {
            get { return entityGeneratorOptionsBindingSource; }
        }

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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label autoPropertiesOnlyLabel;
            System.Windows.Forms.Label debuggerOutputLabel;
            System.Windows.Forms.Label throwExceptionsLabel;
            this.entityGeneratorOptionsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.autoPropertiesOnlyCheckBox = new System.Windows.Forms.CheckBox();
            this.debuggerOutputCheckBox = new System.Windows.Forms.CheckBox();
            this.throwExceptionsCheckBox = new System.Windows.Forms.CheckBox();
            autoPropertiesOnlyLabel = new System.Windows.Forms.Label();
            debuggerOutputLabel = new System.Windows.Forms.Label();
            throwExceptionsLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.EntityGeneratorOptionsBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // autoPropertiesOnlyLabel
            // 
            autoPropertiesOnlyLabel.AutoSize = true;
            autoPropertiesOnlyLabel.Location = new System.Drawing.Point(19, 29);
            autoPropertiesOnlyLabel.Name = "autoPropertiesOnlyLabel";
            autoPropertiesOnlyLabel.Size = new System.Drawing.Size(106, 13);
            autoPropertiesOnlyLabel.TabIndex = 0;
            autoPropertiesOnlyLabel.Text = "Auto Properties Only:";
            // 
            // debuggerOutputLabel
            // 
            debuggerOutputLabel.AutoSize = true;
            debuggerOutputLabel.Location = new System.Drawing.Point(19, 59);
            debuggerOutputLabel.Name = "debuggerOutputLabel";
            debuggerOutputLabel.Size = new System.Drawing.Size(92, 13);
            debuggerOutputLabel.TabIndex = 2;
            debuggerOutputLabel.Text = "Debugger Output:";
            // 
            // throwExceptionsLabel
            // 
            throwExceptionsLabel.AutoSize = true;
            throwExceptionsLabel.Location = new System.Drawing.Point(19, 89);
            throwExceptionsLabel.Name = "throwExceptionsLabel";
            throwExceptionsLabel.Size = new System.Drawing.Size(95, 13);
            throwExceptionsLabel.TabIndex = 4;
            throwExceptionsLabel.Text = "Throw Exceptions:";
            // 
            // entityGeneratorOptionsBindingSource
            // 
            this.EntityGeneratorOptionsBindingSource.DataSource = typeof(ChristianHelle.DatabaseTools.SqlCe.CodeGenCore.EntityGeneratorOptions);
            // 
            // autoPropertiesOnlyCheckBox
            // 
            this.autoPropertiesOnlyCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.autoPropertiesOnlyCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("CheckState", this.EntityGeneratorOptionsBindingSource, "AutoPropertiesOnly", true));
            this.autoPropertiesOnlyCheckBox.Location = new System.Drawing.Point(197, 24);
            this.autoPropertiesOnlyCheckBox.Name = "autoPropertiesOnlyCheckBox";
            this.autoPropertiesOnlyCheckBox.Size = new System.Drawing.Size(22, 24);
            this.autoPropertiesOnlyCheckBox.TabIndex = 1;
            this.autoPropertiesOnlyCheckBox.UseVisualStyleBackColor = true;
            // 
            // debuggerOutputCheckBox
            // 
            this.debuggerOutputCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.debuggerOutputCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("CheckState", this.EntityGeneratorOptionsBindingSource, "DebuggerOutput", true));
            this.debuggerOutputCheckBox.Location = new System.Drawing.Point(197, 54);
            this.debuggerOutputCheckBox.Name = "debuggerOutputCheckBox";
            this.debuggerOutputCheckBox.Size = new System.Drawing.Size(22, 24);
            this.debuggerOutputCheckBox.TabIndex = 3;
            this.debuggerOutputCheckBox.UseVisualStyleBackColor = true;
            // 
            // throwExceptionsCheckBox
            // 
            this.throwExceptionsCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.throwExceptionsCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("CheckState", this.EntityGeneratorOptionsBindingSource, "ThrowExceptions", true));
            this.throwExceptionsCheckBox.Location = new System.Drawing.Point(197, 84);
            this.throwExceptionsCheckBox.Name = "throwExceptionsCheckBox";
            this.throwExceptionsCheckBox.Size = new System.Drawing.Size(22, 24);
            this.throwExceptionsCheckBox.TabIndex = 5;
            this.throwExceptionsCheckBox.UseVisualStyleBackColor = true;
            // 
            // EntityOptionsUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(autoPropertiesOnlyLabel);
            this.Controls.Add(this.autoPropertiesOnlyCheckBox);
            this.Controls.Add(debuggerOutputLabel);
            this.Controls.Add(this.debuggerOutputCheckBox);
            this.Controls.Add(throwExceptionsLabel);
            this.Controls.Add(this.throwExceptionsCheckBox);
            this.Name = "EntityOptionsUserControl";
            this.Size = new System.Drawing.Size(239, 133);
            this.Load += new System.EventHandler(this.EntityOptionsUserControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.EntityGeneratorOptionsBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox autoPropertiesOnlyCheckBox;
        private System.Windows.Forms.CheckBox debuggerOutputCheckBox;
        private System.Windows.Forms.CheckBox throwExceptionsCheckBox;
        private System.Windows.Forms.BindingSource entityGeneratorOptionsBindingSource;
    }
}
