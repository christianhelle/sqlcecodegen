using System;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore
{
    public class VisualBasicCodeGenerator : CodeGenerator
    {
        public VisualBasicCodeGenerator(SqlCeDatabase database)
            : base(database)
        {
        }

        public override void GenerateEntities()
        {
            throw new NotImplementedException();
        }

        public override void GenerateDataAccessLayer()
        {
            throw new NotImplementedException();
        }
    }
}