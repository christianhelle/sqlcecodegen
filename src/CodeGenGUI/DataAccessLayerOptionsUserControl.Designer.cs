namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenGUI
{
    partial class DataAccessLayerOptionsUserControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label debuggerOutputLabel;
            System.Windows.Forms.Label generateCountLabel;
            System.Windows.Forms.Label generateCreateLabel;
            System.Windows.Forms.Label generateCreateIgnoringPrimaryKeyLabel;
            System.Windows.Forms.Label generateCreateUsingAllColumnsLabel;
            System.Windows.Forms.Label generateDeleteLabel;
            System.Windows.Forms.Label generateDeleteAllLabel;
            System.Windows.Forms.Label generateDeleteByLabel;
            System.Windows.Forms.Label generatePopulateLabel;
            System.Windows.Forms.Label generateSaveChangesLabel;
            System.Windows.Forms.Label generateSelectAllLabel;
            System.Windows.Forms.Label generateSelectAllWithTopLabel;
            System.Windows.Forms.Label generateSelectByLabel;
            System.Windows.Forms.Label generateSelectByThreeColumnsLabel;
            System.Windows.Forms.Label generateSelectByTwoColumnsLabel;
            System.Windows.Forms.Label generateSelectByWithTopLabel;
            System.Windows.Forms.Label generateUpdateLabel;
            System.Windows.Forms.Label generateXmlDocumentationLabel;
            System.Windows.Forms.Label performanceMeasurementOutputLabel;
            System.Windows.Forms.Label throwExceptionsLabel;
            this.dataAccessLayerGeneratorOptionsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.debuggerOutputCheckBox = new System.Windows.Forms.CheckBox();
            this.generateCountCheckBox = new System.Windows.Forms.CheckBox();
            this.generateCreateCheckBox = new System.Windows.Forms.CheckBox();
            this.generateCreateIgnoringPrimaryKeyCheckBox = new System.Windows.Forms.CheckBox();
            this.generateCreateUsingAllColumnsCheckBox = new System.Windows.Forms.CheckBox();
            this.generateDeleteCheckBox = new System.Windows.Forms.CheckBox();
            this.generateDeleteAllCheckBox = new System.Windows.Forms.CheckBox();
            this.generateDeleteByCheckBox = new System.Windows.Forms.CheckBox();
            this.generatePopulateCheckBox = new System.Windows.Forms.CheckBox();
            this.generateSaveChangesCheckBox = new System.Windows.Forms.CheckBox();
            this.generateSelectAllCheckBox = new System.Windows.Forms.CheckBox();
            this.generateSelectAllWithTopCheckBox = new System.Windows.Forms.CheckBox();
            this.generateSelectByCheckBox = new System.Windows.Forms.CheckBox();
            this.generateSelectByThreeColumnsCheckBox = new System.Windows.Forms.CheckBox();
            this.generateSelectByTwoColumnsCheckBox = new System.Windows.Forms.CheckBox();
            this.generateSelectByWithTopCheckBox = new System.Windows.Forms.CheckBox();
            this.generateUpdateCheckBox = new System.Windows.Forms.CheckBox();
            this.generateXmlDocumentationCheckBox = new System.Windows.Forms.CheckBox();
            this.performanceMeasurementOutputCheckBox = new System.Windows.Forms.CheckBox();
            this.throwExceptionsCheckBox = new System.Windows.Forms.CheckBox();
            debuggerOutputLabel = new System.Windows.Forms.Label();
            generateCountLabel = new System.Windows.Forms.Label();
            generateCreateLabel = new System.Windows.Forms.Label();
            generateCreateIgnoringPrimaryKeyLabel = new System.Windows.Forms.Label();
            generateCreateUsingAllColumnsLabel = new System.Windows.Forms.Label();
            generateDeleteLabel = new System.Windows.Forms.Label();
            generateDeleteAllLabel = new System.Windows.Forms.Label();
            generateDeleteByLabel = new System.Windows.Forms.Label();
            generatePopulateLabel = new System.Windows.Forms.Label();
            generateSaveChangesLabel = new System.Windows.Forms.Label();
            generateSelectAllLabel = new System.Windows.Forms.Label();
            generateSelectAllWithTopLabel = new System.Windows.Forms.Label();
            generateSelectByLabel = new System.Windows.Forms.Label();
            generateSelectByThreeColumnsLabel = new System.Windows.Forms.Label();
            generateSelectByTwoColumnsLabel = new System.Windows.Forms.Label();
            generateSelectByWithTopLabel = new System.Windows.Forms.Label();
            generateUpdateLabel = new System.Windows.Forms.Label();
            generateXmlDocumentationLabel = new System.Windows.Forms.Label();
            performanceMeasurementOutputLabel = new System.Windows.Forms.Label();
            throwExceptionsLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataAccessLayerGeneratorOptionsBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dataAccessLayerGeneratorOptionsBindingSource
            // 
            this.dataAccessLayerGeneratorOptionsBindingSource.DataSource = typeof(ChristianHelle.DatabaseTools.SqlCe.CodeGenCore.DataAccessLayerGeneratorOptions);
            // 
            // debuggerOutputLabel
            // 
            debuggerOutputLabel.AutoSize = true;
            debuggerOutputLabel.Location = new System.Drawing.Point(37, 30);
            debuggerOutputLabel.Name = "debuggerOutputLabel";
            debuggerOutputLabel.Size = new System.Drawing.Size(92, 13);
            debuggerOutputLabel.TabIndex = 1;
            debuggerOutputLabel.Text = "Debugger Output:";
            // 
            // debuggerOutputCheckBox
            // 
            this.debuggerOutputCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.debuggerOutputCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("CheckState", this.dataAccessLayerGeneratorOptionsBindingSource, "DebuggerOutput", true));
            this.debuggerOutputCheckBox.Location = new System.Drawing.Point(301, 25);
            this.debuggerOutputCheckBox.Name = "debuggerOutputCheckBox";
            this.debuggerOutputCheckBox.Size = new System.Drawing.Size(22, 24);
            this.debuggerOutputCheckBox.TabIndex = 2;
            this.debuggerOutputCheckBox.UseVisualStyleBackColor = true;
            // 
            // generateCountLabel
            // 
            generateCountLabel.AutoSize = true;
            generateCountLabel.Location = new System.Drawing.Point(37, 60);
            generateCountLabel.Name = "generateCountLabel";
            generateCountLabel.Size = new System.Drawing.Size(85, 13);
            generateCountLabel.TabIndex = 3;
            generateCountLabel.Text = "Generate Count:";
            // 
            // generateCountCheckBox
            // 
            this.generateCountCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.generateCountCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("CheckState", this.dataAccessLayerGeneratorOptionsBindingSource, "GenerateCount", true));
            this.generateCountCheckBox.Location = new System.Drawing.Point(301, 55);
            this.generateCountCheckBox.Name = "generateCountCheckBox";
            this.generateCountCheckBox.Size = new System.Drawing.Size(22, 24);
            this.generateCountCheckBox.TabIndex = 4;
            this.generateCountCheckBox.UseVisualStyleBackColor = true;
            // 
            // generateCreateLabel
            // 
            generateCreateLabel.AutoSize = true;
            generateCreateLabel.Location = new System.Drawing.Point(37, 90);
            generateCreateLabel.Name = "generateCreateLabel";
            generateCreateLabel.Size = new System.Drawing.Size(88, 13);
            generateCreateLabel.TabIndex = 5;
            generateCreateLabel.Text = "Generate Create:";
            // 
            // generateCreateCheckBox
            // 
            this.generateCreateCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.generateCreateCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("CheckState", this.dataAccessLayerGeneratorOptionsBindingSource, "GenerateCreate", true));
            this.generateCreateCheckBox.Location = new System.Drawing.Point(301, 85);
            this.generateCreateCheckBox.Name = "generateCreateCheckBox";
            this.generateCreateCheckBox.Size = new System.Drawing.Size(22, 24);
            this.generateCreateCheckBox.TabIndex = 6;
            this.generateCreateCheckBox.UseVisualStyleBackColor = true;
            // 
            // generateCreateIgnoringPrimaryKeyLabel
            // 
            generateCreateIgnoringPrimaryKeyLabel.AutoSize = true;
            generateCreateIgnoringPrimaryKeyLabel.Location = new System.Drawing.Point(37, 120);
            generateCreateIgnoringPrimaryKeyLabel.Name = "generateCreateIgnoringPrimaryKeyLabel";
            generateCreateIgnoringPrimaryKeyLabel.Size = new System.Drawing.Size(187, 13);
            generateCreateIgnoringPrimaryKeyLabel.TabIndex = 7;
            generateCreateIgnoringPrimaryKeyLabel.Text = "Generate Create Ignoring Primary Key:";
            // 
            // generateCreateIgnoringPrimaryKeyCheckBox
            // 
            this.generateCreateIgnoringPrimaryKeyCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.generateCreateIgnoringPrimaryKeyCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("CheckState", this.dataAccessLayerGeneratorOptionsBindingSource, "GenerateCreateIgnoringPrimaryKey", true));
            this.generateCreateIgnoringPrimaryKeyCheckBox.Location = new System.Drawing.Point(301, 115);
            this.generateCreateIgnoringPrimaryKeyCheckBox.Name = "generateCreateIgnoringPrimaryKeyCheckBox";
            this.generateCreateIgnoringPrimaryKeyCheckBox.Size = new System.Drawing.Size(22, 24);
            this.generateCreateIgnoringPrimaryKeyCheckBox.TabIndex = 8;
            this.generateCreateIgnoringPrimaryKeyCheckBox.UseVisualStyleBackColor = true;
            // 
            // generateCreateUsingAllColumnsLabel
            // 
            generateCreateUsingAllColumnsLabel.AutoSize = true;
            generateCreateUsingAllColumnsLabel.Location = new System.Drawing.Point(37, 150);
            generateCreateUsingAllColumnsLabel.Name = "generateCreateUsingAllColumnsLabel";
            generateCreateUsingAllColumnsLabel.Size = new System.Drawing.Size(175, 13);
            generateCreateUsingAllColumnsLabel.TabIndex = 9;
            generateCreateUsingAllColumnsLabel.Text = "Generate Create Using All Columns:";
            // 
            // generateCreateUsingAllColumnsCheckBox
            // 
            this.generateCreateUsingAllColumnsCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.generateCreateUsingAllColumnsCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("CheckState", this.dataAccessLayerGeneratorOptionsBindingSource, "GenerateCreateUsingAllColumns", true));
            this.generateCreateUsingAllColumnsCheckBox.Location = new System.Drawing.Point(301, 145);
            this.generateCreateUsingAllColumnsCheckBox.Name = "generateCreateUsingAllColumnsCheckBox";
            this.generateCreateUsingAllColumnsCheckBox.Size = new System.Drawing.Size(22, 24);
            this.generateCreateUsingAllColumnsCheckBox.TabIndex = 10;
            this.generateCreateUsingAllColumnsCheckBox.UseVisualStyleBackColor = true;
            // 
            // generateDeleteLabel
            // 
            generateDeleteLabel.AutoSize = true;
            generateDeleteLabel.Location = new System.Drawing.Point(37, 180);
            generateDeleteLabel.Name = "generateDeleteLabel";
            generateDeleteLabel.Size = new System.Drawing.Size(88, 13);
            generateDeleteLabel.TabIndex = 11;
            generateDeleteLabel.Text = "Generate Delete:";
            // 
            // generateDeleteCheckBox
            // 
            this.generateDeleteCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.generateDeleteCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("CheckState", this.dataAccessLayerGeneratorOptionsBindingSource, "GenerateDelete", true));
            this.generateDeleteCheckBox.Location = new System.Drawing.Point(301, 175);
            this.generateDeleteCheckBox.Name = "generateDeleteCheckBox";
            this.generateDeleteCheckBox.Size = new System.Drawing.Size(22, 24);
            this.generateDeleteCheckBox.TabIndex = 12;
            this.generateDeleteCheckBox.UseVisualStyleBackColor = true;
            // 
            // generateDeleteAllLabel
            // 
            generateDeleteAllLabel.AutoSize = true;
            generateDeleteAllLabel.Location = new System.Drawing.Point(37, 210);
            generateDeleteAllLabel.Name = "generateDeleteAllLabel";
            generateDeleteAllLabel.Size = new System.Drawing.Size(102, 13);
            generateDeleteAllLabel.TabIndex = 13;
            generateDeleteAllLabel.Text = "Generate Delete All:";
            // 
            // generateDeleteAllCheckBox
            // 
            this.generateDeleteAllCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.generateDeleteAllCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("CheckState", this.dataAccessLayerGeneratorOptionsBindingSource, "GenerateDeleteAll", true));
            this.generateDeleteAllCheckBox.Location = new System.Drawing.Point(301, 205);
            this.generateDeleteAllCheckBox.Name = "generateDeleteAllCheckBox";
            this.generateDeleteAllCheckBox.Size = new System.Drawing.Size(22, 24);
            this.generateDeleteAllCheckBox.TabIndex = 14;
            this.generateDeleteAllCheckBox.UseVisualStyleBackColor = true;
            // 
            // generateDeleteByLabel
            // 
            generateDeleteByLabel.AutoSize = true;
            generateDeleteByLabel.Location = new System.Drawing.Point(37, 240);
            generateDeleteByLabel.Name = "generateDeleteByLabel";
            generateDeleteByLabel.Size = new System.Drawing.Size(103, 13);
            generateDeleteByLabel.TabIndex = 15;
            generateDeleteByLabel.Text = "Generate Delete By:";
            // 
            // generateDeleteByCheckBox
            // 
            this.generateDeleteByCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.generateDeleteByCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("CheckState", this.dataAccessLayerGeneratorOptionsBindingSource, "GenerateDeleteBy", true));
            this.generateDeleteByCheckBox.Location = new System.Drawing.Point(301, 235);
            this.generateDeleteByCheckBox.Name = "generateDeleteByCheckBox";
            this.generateDeleteByCheckBox.Size = new System.Drawing.Size(22, 24);
            this.generateDeleteByCheckBox.TabIndex = 16;
            this.generateDeleteByCheckBox.UseVisualStyleBackColor = true;
            // 
            // generatePopulateLabel
            // 
            generatePopulateLabel.AutoSize = true;
            generatePopulateLabel.Location = new System.Drawing.Point(37, 270);
            generatePopulateLabel.Name = "generatePopulateLabel";
            generatePopulateLabel.Size = new System.Drawing.Size(99, 13);
            generatePopulateLabel.TabIndex = 17;
            generatePopulateLabel.Text = "Generate Populate:";
            // 
            // generatePopulateCheckBox
            // 
            this.generatePopulateCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.generatePopulateCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("CheckState", this.dataAccessLayerGeneratorOptionsBindingSource, "GeneratePopulate", true));
            this.generatePopulateCheckBox.Location = new System.Drawing.Point(301, 265);
            this.generatePopulateCheckBox.Name = "generatePopulateCheckBox";
            this.generatePopulateCheckBox.Size = new System.Drawing.Size(22, 24);
            this.generatePopulateCheckBox.TabIndex = 18;
            this.generatePopulateCheckBox.UseVisualStyleBackColor = true;
            // 
            // generateSaveChangesLabel
            // 
            generateSaveChangesLabel.AutoSize = true;
            generateSaveChangesLabel.Location = new System.Drawing.Point(37, 300);
            generateSaveChangesLabel.Name = "generateSaveChangesLabel";
            generateSaveChangesLabel.Size = new System.Drawing.Size(127, 13);
            generateSaveChangesLabel.TabIndex = 19;
            generateSaveChangesLabel.Text = "Generate Save Changes:";
            // 
            // generateSaveChangesCheckBox
            // 
            this.generateSaveChangesCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.generateSaveChangesCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("CheckState", this.dataAccessLayerGeneratorOptionsBindingSource, "GenerateSaveChanges", true));
            this.generateSaveChangesCheckBox.Location = new System.Drawing.Point(301, 295);
            this.generateSaveChangesCheckBox.Name = "generateSaveChangesCheckBox";
            this.generateSaveChangesCheckBox.Size = new System.Drawing.Size(22, 24);
            this.generateSaveChangesCheckBox.TabIndex = 20;
            this.generateSaveChangesCheckBox.UseVisualStyleBackColor = true;
            // 
            // generateSelectAllLabel
            // 
            generateSelectAllLabel.AutoSize = true;
            generateSelectAllLabel.Location = new System.Drawing.Point(37, 330);
            generateSelectAllLabel.Name = "generateSelectAllLabel";
            generateSelectAllLabel.Size = new System.Drawing.Size(101, 13);
            generateSelectAllLabel.TabIndex = 21;
            generateSelectAllLabel.Text = "Generate Select All:";
            // 
            // generateSelectAllCheckBox
            // 
            this.generateSelectAllCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.generateSelectAllCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("CheckState", this.dataAccessLayerGeneratorOptionsBindingSource, "GenerateSelectAll", true));
            this.generateSelectAllCheckBox.Location = new System.Drawing.Point(301, 325);
            this.generateSelectAllCheckBox.Name = "generateSelectAllCheckBox";
            this.generateSelectAllCheckBox.Size = new System.Drawing.Size(22, 24);
            this.generateSelectAllCheckBox.TabIndex = 22;
            this.generateSelectAllCheckBox.UseVisualStyleBackColor = true;
            // 
            // generateSelectAllWithTopLabel
            // 
            generateSelectAllWithTopLabel.AutoSize = true;
            generateSelectAllWithTopLabel.Location = new System.Drawing.Point(37, 360);
            generateSelectAllWithTopLabel.Name = "generateSelectAllWithTopLabel";
            generateSelectAllWithTopLabel.Size = new System.Drawing.Size(148, 13);
            generateSelectAllWithTopLabel.TabIndex = 23;
            generateSelectAllWithTopLabel.Text = "Generate Select All With Top:";
            // 
            // generateSelectAllWithTopCheckBox
            // 
            this.generateSelectAllWithTopCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.generateSelectAllWithTopCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("CheckState", this.dataAccessLayerGeneratorOptionsBindingSource, "GenerateSelectAllWithTop", true));
            this.generateSelectAllWithTopCheckBox.Location = new System.Drawing.Point(301, 355);
            this.generateSelectAllWithTopCheckBox.Name = "generateSelectAllWithTopCheckBox";
            this.generateSelectAllWithTopCheckBox.Size = new System.Drawing.Size(22, 24);
            this.generateSelectAllWithTopCheckBox.TabIndex = 24;
            this.generateSelectAllWithTopCheckBox.UseVisualStyleBackColor = true;
            // 
            // generateSelectByLabel
            // 
            generateSelectByLabel.AutoSize = true;
            generateSelectByLabel.Location = new System.Drawing.Point(37, 390);
            generateSelectByLabel.Name = "generateSelectByLabel";
            generateSelectByLabel.Size = new System.Drawing.Size(102, 13);
            generateSelectByLabel.TabIndex = 25;
            generateSelectByLabel.Text = "Generate Select By:";
            // 
            // generateSelectByCheckBox
            // 
            this.generateSelectByCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.generateSelectByCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("CheckState", this.dataAccessLayerGeneratorOptionsBindingSource, "GenerateSelectBy", true));
            this.generateSelectByCheckBox.Location = new System.Drawing.Point(301, 385);
            this.generateSelectByCheckBox.Name = "generateSelectByCheckBox";
            this.generateSelectByCheckBox.Size = new System.Drawing.Size(22, 24);
            this.generateSelectByCheckBox.TabIndex = 26;
            this.generateSelectByCheckBox.UseVisualStyleBackColor = true;
            // 
            // generateSelectByThreeColumnsLabel
            // 
            generateSelectByThreeColumnsLabel.AutoSize = true;
            generateSelectByThreeColumnsLabel.Location = new System.Drawing.Point(37, 420);
            generateSelectByThreeColumnsLabel.Name = "generateSelectByThreeColumnsLabel";
            generateSelectByThreeColumnsLabel.Size = new System.Drawing.Size(176, 13);
            generateSelectByThreeColumnsLabel.TabIndex = 27;
            generateSelectByThreeColumnsLabel.Text = "Generate Select By Three Columns:";
            // 
            // generateSelectByThreeColumnsCheckBox
            // 
            this.generateSelectByThreeColumnsCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.generateSelectByThreeColumnsCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("CheckState", this.dataAccessLayerGeneratorOptionsBindingSource, "GenerateSelectByThreeColumns", true));
            this.generateSelectByThreeColumnsCheckBox.Location = new System.Drawing.Point(301, 415);
            this.generateSelectByThreeColumnsCheckBox.Name = "generateSelectByThreeColumnsCheckBox";
            this.generateSelectByThreeColumnsCheckBox.Size = new System.Drawing.Size(22, 24);
            this.generateSelectByThreeColumnsCheckBox.TabIndex = 28;
            this.generateSelectByThreeColumnsCheckBox.UseVisualStyleBackColor = true;
            // 
            // generateSelectByTwoColumnsLabel
            // 
            generateSelectByTwoColumnsLabel.AutoSize = true;
            generateSelectByTwoColumnsLabel.Location = new System.Drawing.Point(37, 450);
            generateSelectByTwoColumnsLabel.Name = "generateSelectByTwoColumnsLabel";
            generateSelectByTwoColumnsLabel.Size = new System.Drawing.Size(169, 13);
            generateSelectByTwoColumnsLabel.TabIndex = 29;
            generateSelectByTwoColumnsLabel.Text = "Generate Select By Two Columns:";
            // 
            // generateSelectByTwoColumnsCheckBox
            // 
            this.generateSelectByTwoColumnsCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.generateSelectByTwoColumnsCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("CheckState", this.dataAccessLayerGeneratorOptionsBindingSource, "GenerateSelectByTwoColumns", true));
            this.generateSelectByTwoColumnsCheckBox.Location = new System.Drawing.Point(301, 445);
            this.generateSelectByTwoColumnsCheckBox.Name = "generateSelectByTwoColumnsCheckBox";
            this.generateSelectByTwoColumnsCheckBox.Size = new System.Drawing.Size(22, 24);
            this.generateSelectByTwoColumnsCheckBox.TabIndex = 30;
            this.generateSelectByTwoColumnsCheckBox.UseVisualStyleBackColor = true;
            // 
            // generateSelectByWithTopLabel
            // 
            generateSelectByWithTopLabel.AutoSize = true;
            generateSelectByWithTopLabel.Location = new System.Drawing.Point(37, 480);
            generateSelectByWithTopLabel.Name = "generateSelectByWithTopLabel";
            generateSelectByWithTopLabel.Size = new System.Drawing.Size(149, 13);
            generateSelectByWithTopLabel.TabIndex = 31;
            generateSelectByWithTopLabel.Text = "Generate Select By With Top:";
            // 
            // generateSelectByWithTopCheckBox
            // 
            this.generateSelectByWithTopCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.generateSelectByWithTopCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("CheckState", this.dataAccessLayerGeneratorOptionsBindingSource, "GenerateSelectByWithTop", true));
            this.generateSelectByWithTopCheckBox.Location = new System.Drawing.Point(301, 475);
            this.generateSelectByWithTopCheckBox.Name = "generateSelectByWithTopCheckBox";
            this.generateSelectByWithTopCheckBox.Size = new System.Drawing.Size(22, 24);
            this.generateSelectByWithTopCheckBox.TabIndex = 32;
            this.generateSelectByWithTopCheckBox.UseVisualStyleBackColor = true;
            // 
            // generateUpdateLabel
            // 
            generateUpdateLabel.AutoSize = true;
            generateUpdateLabel.Location = new System.Drawing.Point(37, 510);
            generateUpdateLabel.Name = "generateUpdateLabel";
            generateUpdateLabel.Size = new System.Drawing.Size(92, 13);
            generateUpdateLabel.TabIndex = 33;
            generateUpdateLabel.Text = "Generate Update:";
            // 
            // generateUpdateCheckBox
            // 
            this.generateUpdateCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.generateUpdateCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("CheckState", this.dataAccessLayerGeneratorOptionsBindingSource, "GenerateUpdate", true));
            this.generateUpdateCheckBox.Location = new System.Drawing.Point(301, 505);
            this.generateUpdateCheckBox.Name = "generateUpdateCheckBox";
            this.generateUpdateCheckBox.Size = new System.Drawing.Size(22, 24);
            this.generateUpdateCheckBox.TabIndex = 34;
            this.generateUpdateCheckBox.UseVisualStyleBackColor = true;
            // 
            // generateXmlDocumentationLabel
            // 
            generateXmlDocumentationLabel.AutoSize = true;
            generateXmlDocumentationLabel.Location = new System.Drawing.Point(37, 540);
            generateXmlDocumentationLabel.Name = "generateXmlDocumentationLabel";
            generateXmlDocumentationLabel.Size = new System.Drawing.Size(149, 13);
            generateXmlDocumentationLabel.TabIndex = 35;
            generateXmlDocumentationLabel.Text = "Generate Xml Documentation:";
            // 
            // generateXmlDocumentationCheckBox
            // 
            this.generateXmlDocumentationCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.generateXmlDocumentationCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("CheckState", this.dataAccessLayerGeneratorOptionsBindingSource, "GenerateXmlDocumentation", true));
            this.generateXmlDocumentationCheckBox.Location = new System.Drawing.Point(301, 535);
            this.generateXmlDocumentationCheckBox.Name = "generateXmlDocumentationCheckBox";
            this.generateXmlDocumentationCheckBox.Size = new System.Drawing.Size(22, 24);
            this.generateXmlDocumentationCheckBox.TabIndex = 36;
            this.generateXmlDocumentationCheckBox.UseVisualStyleBackColor = true;
            // 
            // performanceMeasurementOutputLabel
            // 
            performanceMeasurementOutputLabel.AutoSize = true;
            performanceMeasurementOutputLabel.Location = new System.Drawing.Point(37, 570);
            performanceMeasurementOutputLabel.Name = "performanceMeasurementOutputLabel";
            performanceMeasurementOutputLabel.Size = new System.Drawing.Size(172, 13);
            performanceMeasurementOutputLabel.TabIndex = 37;
            performanceMeasurementOutputLabel.Text = "Performance Measurement Output:";
            // 
            // performanceMeasurementOutputCheckBox
            // 
            this.performanceMeasurementOutputCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.performanceMeasurementOutputCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("CheckState", this.dataAccessLayerGeneratorOptionsBindingSource, "PerformanceMeasurementOutput", true));
            this.performanceMeasurementOutputCheckBox.Location = new System.Drawing.Point(301, 565);
            this.performanceMeasurementOutputCheckBox.Name = "performanceMeasurementOutputCheckBox";
            this.performanceMeasurementOutputCheckBox.Size = new System.Drawing.Size(22, 24);
            this.performanceMeasurementOutputCheckBox.TabIndex = 38;
            this.performanceMeasurementOutputCheckBox.UseVisualStyleBackColor = true;
            // 
            // throwExceptionsLabel
            // 
            throwExceptionsLabel.AutoSize = true;
            throwExceptionsLabel.Location = new System.Drawing.Point(37, 600);
            throwExceptionsLabel.Name = "throwExceptionsLabel";
            throwExceptionsLabel.Size = new System.Drawing.Size(95, 13);
            throwExceptionsLabel.TabIndex = 39;
            throwExceptionsLabel.Text = "Throw Exceptions:";
            // 
            // throwExceptionsCheckBox
            // 
            this.throwExceptionsCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.throwExceptionsCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("CheckState", this.dataAccessLayerGeneratorOptionsBindingSource, "ThrowExceptions", true));
            this.throwExceptionsCheckBox.Location = new System.Drawing.Point(301, 595);
            this.throwExceptionsCheckBox.Name = "throwExceptionsCheckBox";
            this.throwExceptionsCheckBox.Size = new System.Drawing.Size(22, 24);
            this.throwExceptionsCheckBox.TabIndex = 40;
            this.throwExceptionsCheckBox.UseVisualStyleBackColor = true;
            // 
            // DataAccessLayerOptionsUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(debuggerOutputLabel);
            this.Controls.Add(this.debuggerOutputCheckBox);
            this.Controls.Add(generateCountLabel);
            this.Controls.Add(this.generateCountCheckBox);
            this.Controls.Add(generateCreateLabel);
            this.Controls.Add(this.generateCreateCheckBox);
            this.Controls.Add(generateCreateIgnoringPrimaryKeyLabel);
            this.Controls.Add(this.generateCreateIgnoringPrimaryKeyCheckBox);
            this.Controls.Add(generateCreateUsingAllColumnsLabel);
            this.Controls.Add(this.generateCreateUsingAllColumnsCheckBox);
            this.Controls.Add(generateDeleteLabel);
            this.Controls.Add(this.generateDeleteCheckBox);
            this.Controls.Add(generateDeleteAllLabel);
            this.Controls.Add(this.generateDeleteAllCheckBox);
            this.Controls.Add(generateDeleteByLabel);
            this.Controls.Add(this.generateDeleteByCheckBox);
            this.Controls.Add(generatePopulateLabel);
            this.Controls.Add(this.generatePopulateCheckBox);
            this.Controls.Add(generateSaveChangesLabel);
            this.Controls.Add(this.generateSaveChangesCheckBox);
            this.Controls.Add(generateSelectAllLabel);
            this.Controls.Add(this.generateSelectAllCheckBox);
            this.Controls.Add(generateSelectAllWithTopLabel);
            this.Controls.Add(this.generateSelectAllWithTopCheckBox);
            this.Controls.Add(generateSelectByLabel);
            this.Controls.Add(this.generateSelectByCheckBox);
            this.Controls.Add(generateSelectByThreeColumnsLabel);
            this.Controls.Add(this.generateSelectByThreeColumnsCheckBox);
            this.Controls.Add(generateSelectByTwoColumnsLabel);
            this.Controls.Add(this.generateSelectByTwoColumnsCheckBox);
            this.Controls.Add(generateSelectByWithTopLabel);
            this.Controls.Add(this.generateSelectByWithTopCheckBox);
            this.Controls.Add(generateUpdateLabel);
            this.Controls.Add(this.generateUpdateCheckBox);
            this.Controls.Add(generateXmlDocumentationLabel);
            this.Controls.Add(this.generateXmlDocumentationCheckBox);
            this.Controls.Add(performanceMeasurementOutputLabel);
            this.Controls.Add(this.performanceMeasurementOutputCheckBox);
            this.Controls.Add(throwExceptionsLabel);
            this.Controls.Add(this.throwExceptionsCheckBox);
            this.Name = "DataAccessLayerOptionsUserControl";
            this.Size = new System.Drawing.Size(361, 646);
            this.Load += new System.EventHandler(this.DataAccessLayerOptionsUserControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataAccessLayerGeneratorOptionsBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingSource dataAccessLayerGeneratorOptionsBindingSource;
        private System.Windows.Forms.CheckBox debuggerOutputCheckBox;
        private System.Windows.Forms.CheckBox generateCountCheckBox;
        private System.Windows.Forms.CheckBox generateCreateCheckBox;
        private System.Windows.Forms.CheckBox generateCreateIgnoringPrimaryKeyCheckBox;
        private System.Windows.Forms.CheckBox generateCreateUsingAllColumnsCheckBox;
        private System.Windows.Forms.CheckBox generateDeleteCheckBox;
        private System.Windows.Forms.CheckBox generateDeleteAllCheckBox;
        private System.Windows.Forms.CheckBox generateDeleteByCheckBox;
        private System.Windows.Forms.CheckBox generatePopulateCheckBox;
        private System.Windows.Forms.CheckBox generateSaveChangesCheckBox;
        private System.Windows.Forms.CheckBox generateSelectAllCheckBox;
        private System.Windows.Forms.CheckBox generateSelectAllWithTopCheckBox;
        private System.Windows.Forms.CheckBox generateSelectByCheckBox;
        private System.Windows.Forms.CheckBox generateSelectByThreeColumnsCheckBox;
        private System.Windows.Forms.CheckBox generateSelectByTwoColumnsCheckBox;
        private System.Windows.Forms.CheckBox generateSelectByWithTopCheckBox;
        private System.Windows.Forms.CheckBox generateUpdateCheckBox;
        private System.Windows.Forms.CheckBox generateXmlDocumentationCheckBox;
        private System.Windows.Forms.CheckBox performanceMeasurementOutputCheckBox;
        private System.Windows.Forms.CheckBox throwExceptionsCheckBox;

    }
}
