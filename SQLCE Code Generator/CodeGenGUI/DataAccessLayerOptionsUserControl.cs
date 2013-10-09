using System.Windows.Forms;
using ChristianHelle.DatabaseTools.SqlCe.CodeGenCore;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenGUI
{
    public partial class DataAccessLayerOptionsUserControl : UserControl
    {
        public DataAccessLayerOptionsUserControl()
        {
            InitializeComponent();
        }


        public DataAccessLayerGeneratorOptions GeneratorOptions
        {
            get { return dataAccessLayerGeneratorOptionsBindingSource.Current as DataAccessLayerGeneratorOptions; }
            set
            {
                dataAccessLayerGeneratorOptionsBindingSource.Clear();
                dataAccessLayerGeneratorOptionsBindingSource.Add(value);
            }
        }

        private void DataAccessLayerOptionsUserControl_Load(object sender, System.EventArgs e)
        {

        }
    }
}
