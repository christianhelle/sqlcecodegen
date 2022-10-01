#region License
// The MIT License (MIT)
// 
// Copyright (c) 2009 Christian Resma Helle
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
#endregion
using System;
using System.Windows.Forms;
using ChristianHelle.DatabaseTools.SqlCe.CodeGenCore;
using ChristianHelle.DatabaseTools.SqlCe.CodeGenGUI.Properties;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenGUI
{
    public partial class OptionsForm : Form
    {
        public OptionsForm()
        {
            InitializeComponent();
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            switch (e.Node.Name)
            {
                case "nodeEntityGenerator":
                    entityOptionsUserControl.Visible = true;
                    dataAccessLayerOptionsUserControl.Visible = false;
                    break;

                case "nodeDataAccessLayerGenerator":
                    entityOptionsUserControl.Visible = false;
                    dataAccessLayerOptionsUserControl.Visible = true;
                    break;
            }
        }

        private void OptionsForm_Load(object sender, EventArgs e)
        {
            entityOptionsUserControl.GeneratorOptions = Settings.Default.EntityOptions ?? new EntityGeneratorOptions();
            dataAccessLayerOptionsUserControl.GeneratorOptions = Settings.Default.DataLayerOptions ?? new DataAccessLayerGeneratorOptions();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Settings.Default.EntityOptions = entityOptionsUserControl.GeneratorOptions;
            Settings.Default.DataLayerOptions = dataAccessLayerOptionsUserControl.GeneratorOptions;
            Settings.Default.Save();
            
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
