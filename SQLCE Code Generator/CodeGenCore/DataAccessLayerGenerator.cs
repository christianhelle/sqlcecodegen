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
