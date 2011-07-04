using System.Text;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore
{
    public abstract class CodeGenerator
    {
        protected readonly StringBuilder code;

        protected CodeGenerator(SqlCeDatabase database)
        {
            Database = database;
            code = new StringBuilder();
            code.AppendLine();
        }

        public SqlCeDatabase Database { get; set; }
        public abstract void WriteHeaderInformation();
        public abstract void GenerateEntities();
        public abstract void GenerateEntities(EntityGeneratorOptions options);
        public abstract void GenerateDataAccessLayer();
        public abstract void GenerateDataAccessLayer(DataAccessLayerGeneratorOptions options);

        public virtual string GetCode()
        {
            return code.ToString();
        }

        public virtual void ClearCode()
        {
            code.Remove(0, code.Length - 1);
        }
    }
}
