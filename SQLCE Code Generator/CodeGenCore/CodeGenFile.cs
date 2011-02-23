using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

namespace CodeGenGUI
{
    [Serializable]
    public class CodeGenFile
    {
        public string DataSource { get; set; }
        public string GeneratedCode { get; set; }
    }

    public class CodeGenFileSerializer
    {
        public CodeGenFile LoadFile(string filename)
        {
            var serializer = new XmlSerializer(typeof(CodeGenFile));
            using (var stream = new StreamReader(filename))
                return serializer.Deserialize(stream) as CodeGenFile;
        }

        public void SaveFile(CodeGenFile codeGenFile, string filename)
        {
            var serializer = new XmlSerializer(typeof(CodeGenFile));
            using (var stream = new StreamWriter(filename))
                serializer.Serialize(stream, codeGenFile);
        }
    }
}
