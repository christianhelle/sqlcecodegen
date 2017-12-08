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
using System.IO;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenGUI
{
    public partial class OpenDatabase : Form
    {
        private List<DatabaseItem> cachedItems;

        public OpenDatabase()
        {
            InitializeComponent();
        }

        public string ConnectionString
        {
            get
            {
                var item = (DatabaseItem)cbDatabase.SelectedItem;
                return item != null ? string.Format("Data Source= \"{0}\"; Password=\"{1}\"", item.Database.FullName, item.Password) : null;
            }
        }

        private void OpenDatabase_Load(object sender, EventArgs e)
        {
            var serializer = new XmlSerializer(typeof(List<DatabaseItem>));
            using (var stream = new FileStream("ConnectionCache.xml", FileMode.OpenOrCreate))
                cachedItems = (List<DatabaseItem>)serializer.Deserialize(stream);

            if (cachedItems == null)
                return;

            foreach (var item in cachedItems)
            {
                
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void cbDatabase_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbPassword.ResetText();

            var item = (DatabaseItem)cbDatabase.SelectedItem;
            if (item != null)
                tbPassword.Text = item.Password;
        }

        private void btnOpenFileDialog_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "Database files (*.sdf)|*.sdf";
                if (dialog.ShowDialog() == DialogResult.Cancel)
                    return;

                var item = new DatabaseItem { Database = new FileInfo(dialog.FileName) };
                cbDatabase.Items.Add(item);
                cbDatabase.SelectedItem = item;
            }
        }

        private void OpenDatabase_FormClosing(object sender, FormClosingEventArgs e)
        {
            var serializer = new XmlSerializer(typeof(List<DatabaseItem>));
            using (var stream = new FileStream("ConnectionCache.xml", FileMode.OpenOrCreate))
                serializer.Serialize(stream, cachedItems);
        }
    }

    public class DatabaseItem
    {
        public FileInfo Database { get; set; }
        public string Password { get; set; }

        public override string ToString()
        {
            return Database.Name;
        }
    }
}