﻿using System;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Collections.Generic;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore
{
    public abstract class CodeGenerator
    {
        protected readonly StringBuilder Code;

        protected CodeGenerator(ISqlCeDatabase database)
        {
            Database = database;
            CodeFiles = new Dictionary<string, StringBuilder>();
            Code = new StringBuilder();
            Code.AppendLine();
        }

        public ISqlCeDatabase Database { get; set; }
        public Dictionary<string, StringBuilder> CodeFiles { get; protected set; }

        public virtual void GenerateEntities() { }
        public virtual void GenerateEntities(EntityGeneratorOptions options) { }
        public virtual void GenerateDataAccessLayer() { }
        public virtual void GenerateDataAccessLayer(DataAccessLayerGeneratorOptions options) { }

        public virtual void WriteHeaderInformation()
        {
            WriteHeaderInformation(Code);
        }

        public void WriteHeaderInformation(StringBuilder code)
        {
            code.AppendLine("/*");
            code.AppendLine("\tThis code was generated by SQL Compact Code Generator version " + Assembly.GetCallingAssembly().GetName().Version);
            code.AppendLine();
            code.AppendLine("\tSQL Compact Code Generator was written by Christian Resma Helle (http://sqlcecodegen.codeplex.com)");
            code.AppendLine("\tand is under the GNU General Public License version 2 (GPLv2)");
            code.AppendLine();
            code.AppendLine("\tGenerated: " + DateTime.Now.ToString(CultureInfo.InvariantCulture));
            code.AppendLine("*/");
            code.AppendLine();
        }

        public virtual string GetCode()
        {
            return Code.ToString();
        }

        public virtual void ClearCode()
        {
            Code.Remove(0, Code.Length - 1);
        }

        protected static void GenerateXmlDoc(StringBuilder code, int tabPrefixCount, string summary, params KeyValuePair<string, string>[] parameters)
        {
            for (var i = 0; i < tabPrefixCount; i++)
                code.Append("\t");
            code.AppendLine("/// <summary>");

            for (var i = 0; i < tabPrefixCount; i++)
                code.Append("\t");
            code.AppendLine("/// " + summary);

            for (var i = 0; i < tabPrefixCount; i++)
                code.Append("\t");
            code.AppendLine("/// </summary>");

            foreach (var parameter in parameters)
            {
                for (var i = 0; i < tabPrefixCount; i++)
                    code.Append("\t");
                code.AppendFormat("/// <param name=\"{0}\">{1}</param>", parameter.Key, parameter.Value);
                code.AppendLine();
            }
        }

        protected void AppendCode(string className, StringBuilder code)
        {
            Code.Append(code);
            CodeFiles.Add(className, code);
        }
    }
}
