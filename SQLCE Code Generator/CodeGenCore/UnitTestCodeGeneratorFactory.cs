using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore
{
    public class UnitTestCodeGeneratorFactory
    {
        private readonly SqlCeDatabase database;

        public UnitTestCodeGeneratorFactory(SqlCeDatabase database)
        {
            this.database = database;
        }

        public CodeGenerator Create(string testfx)
        {
            if (string.Compare(testfx, "mstest", true) == 0)
                return new MsTestUnitTestCodeGenerator(database);

            if (string.Compare(testfx, "nunit", true) == 0)
                return new NUnitTestCodeGenerator(database);

            return Create(database);
        }

        public static CodeGenerator Create(SqlCeDatabase database, string testfx)
        {
            if (string.Compare(testfx, "mstest", true) == 0)
                return new MsTestUnitTestCodeGenerator(database);

            if (string.Compare(testfx, "nunit", true) == 0)
                return new NUnitTestCodeGenerator(database);

            return Create(database);
        }

        public CodeGenerator Create()
        {
            return new MsTestUnitTestCodeGenerator(database);
        }

        public static CodeGenerator Create(SqlCeDatabase database)
        {
            return new MsTestUnitTestCodeGenerator(database);
        }
    }
}
