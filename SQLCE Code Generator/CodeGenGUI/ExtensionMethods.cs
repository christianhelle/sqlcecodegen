using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenGUI
{
    public static class ExtensionMethods
    {
        public static void DoubleBuffered(this DataGridView dgv, bool setting)
        {
            var dgvType = dgv.GetType();
            var pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, setting, null);
        }

        public static int GetLineCount(this string str)
        {
            if (string.IsNullOrEmpty(str)) return 0;

            var lineCount = 0;
            using (var reader = new StringReader(str))
            {
                while (reader.Peek() > 0)
                {
                    reader.ReadLine();
                    lineCount++;
                }
            }
            return lineCount;
        }

        public static int GetLineCount(this IEnumerable<string> strings)
        {
            return strings.Sum(str => GetLineCount(str));
        }
    }
}
