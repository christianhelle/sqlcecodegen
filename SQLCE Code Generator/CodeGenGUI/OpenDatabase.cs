using System;
using System.Windows.Forms;

namespace CodeGenGUI
{
    public partial class OpenDatabase : Form
    {
        public OpenDatabase()
        {
            InitializeComponent();
        }

        public string ConnectionString { get; private set; }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {

        }

        private void cbDatabase_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
