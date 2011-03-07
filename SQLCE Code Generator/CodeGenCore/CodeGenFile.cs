using System;
using System.IO;
using System.IO.Compression;
using System.Xml.Serialization;

namespace CodeGenGUI
{
    [Serializable]
    public class CodeGenFile
    {
        public string DataSource { get; set; }
        public GeneratedCode GeneratedCode { get; set; }
    }

    [Serializable]
    public class GeneratedCode
    {
        public string Entities { get; set; }
        public string DataAccessCode { get; set; }
        public string UnitTests { get; set; }
    }

    public class CodeGenFileSerializer
    {
        public CodeGenFile LoadFile(string filename)
        {
            var serializer = GetSerializer();
            using (var file = new FileStream(filename, FileMode.Open))
            using (var stream = new DeflateStream(file, CompressionMode.Decompress))
                return serializer.Deserialize(stream) as CodeGenFile;
        }

        public void SaveFile(CodeGenFile codeGenFile, string filename)
        {
            var serializer = GetSerializer();
            using (var file = new FileStream(filename, FileMode.Create))
            using (var stream = new DeflateStream(file, CompressionMode.Compress))
                serializer.Serialize(stream, codeGenFile);
        }

        private static XmlSerializer GetSerializer()
        {
            var serializer = new XmlSerializer(typeof(CodeGenFile));
            return serializer;
        }
    }
}
