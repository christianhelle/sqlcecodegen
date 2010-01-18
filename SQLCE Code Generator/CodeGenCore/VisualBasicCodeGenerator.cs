using System;

namespace CodeGenCore
{
    public class VisualBasicCodeGenerator : CodeGenerator
    {
        public VisualBasicCodeGenerator(Database database)
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