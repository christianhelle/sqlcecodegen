using System;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore
{
    public class UnitTestCodeGeneratorFactory
    {
        private readonly SqlCeDatabase database;

        public UnitTestCodeGeneratorFactory(SqlCeDatabase database)
        {
            this.database = database;
        }

        public CodeGenerator Create(string testfx, string target = null)
        {
            return Create(database, testfx, target);
        }

        public static CodeGenerator Create(SqlCeDatabase database, string testfx, string target = null)
        {
            if (!string.IsNullOrEmpty(target) && target == "Mango")
                return new CSharpMangoMockDataAccessLayerCodeGenerator(database);

            switch (testfx.ToLower())
            {
                case "mstest":
                    return new MSTestUnitTestCodeGenerator(database);
                case "nunit":
                    return new NUnitTestCodeGenerator(database);
                case "xunit":
                    return new XUnitTestCodeGenerator(database);
            }

            throw new NotSupportedException("Unit Test Framework not supported");
        }

        public CodeGenerator Create()
        {
            return Create(database);
        }

        public static CodeGenerator Create(SqlCeDatabase database)
        {
            return new MSTestUnitTestCodeGenerator(database);
        }
    }
}
