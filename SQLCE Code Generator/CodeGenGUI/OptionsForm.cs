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
