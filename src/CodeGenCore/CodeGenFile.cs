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
using System.IO;
using System.IO.Compression;
using System.Xml.Serialization;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore
{
    [Serializable]
    public class CodeGenFile
    {
        public string DataSource { get; set; }
        public GeneratedCode GeneratedCode { get; set; }
        public string TestFramework { get; set; }
    }

    [Serializable]
    public class GeneratedCode
    {
        public string Entities { get; set; }
        public string DataAccessCode { get; set; }
        public string EntityUnitTests { get; set; }
        public string DataAccessUnitTests { get; set; }
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