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
using System.Collections.Generic;
using System.Text;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore
{
    public abstract class DataAccessLayerGenerator
    {
        protected StringBuilder Code;
        protected Table Table;

        protected DataAccessLayerGenerator(StringBuilder code, Table table)
        {
            this.Code = code;
            this.Table = table;
        }

        public string GetCode()
        {
            return Code.ToString();
        }

        public DataAccessLayerGeneratorOptions Options { get; set; }

        public virtual void GenerateCreateEntity() { }
        public abstract void GenerateSelectAll();
        public abstract void GenerateSelectBy();
        public abstract void GenerateSelectWithTop();
        public abstract void GenerateSelectByWithTop();
        public abstract void SelectByThreeColumns();
        public abstract void SelectByTwoColumns();
        public abstract void GenerateCreateIgnoringPrimaryKey();
        public abstract void GenerateCreateUsingAllColumns();
        public abstract void GenerateDelete();
        public abstract void GenerateDeleteBy();
        public abstract void GenerateDeleteAll();
        public abstract void GenerateUpdate();
        public abstract void GeneratePopulate();
        public abstract void GenerateCreate();
        public abstract void GenerateCount();

        protected void GenerateXmlDoc(int tabPrefixCount, string summary, params KeyValuePair<string, string>[] parameters)
        {
            for (int i = 0; i < tabPrefixCount; i++)
                Code.Append("\t");
            Code.AppendLine("/// <summary>");

            for (int i = 0; i < tabPrefixCount; i++)
                Code.Append("\t");
            Code.AppendLine("/// " + summary);

            for (int i = 0; i < tabPrefixCount; i++)
                Code.Append("\t");
            Code.AppendLine("/// </summary>");

            foreach (var parameter in parameters)
            {
                for (int i = 0; i < tabPrefixCount; i++)
                    Code.Append("\t");
                Code.AppendFormat("/// <param name=\"{0}\">{1}</param>", parameter.Key, parameter.Value);
                Code.AppendLine();
            }
        }
    }
}
