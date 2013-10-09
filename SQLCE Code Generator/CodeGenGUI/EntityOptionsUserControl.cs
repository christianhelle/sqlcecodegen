using System;
using System.Windows.Forms;
using ChristianHelle.DatabaseTools.SqlCe.CodeGenCore;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenGUI
{
    public partial class EntityOptionsUserControl : UserControl
    {
        public EntityOptionsUserControl()
        {
            InitializeComponent();
        }

        public EntityGeneratorOptions GeneratorOptions
        {
            get { return entityGeneratorOptionsBindingSource.Current as EntityGeneratorOptions; }
            set
            {
                entityGeneratorOptionsBindingSource.Clear();
                entityGeneratorOptionsBindingSource.Add(value);
            }
        }

        private void EntityOptionsUserControl_Load(object sender, EventArgs e)
        {

        }
    }
}
