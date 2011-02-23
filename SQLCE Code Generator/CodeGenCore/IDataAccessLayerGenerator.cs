using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore
{
    public interface IDataAccessLayerGenerator
    {
        void GenerateSelectAll(Table table);
        void GenerateSelectTop(Table table);
        void GenerateCreateIgnoringPrimaryKey(Table table);
        void GenerateCreateUsingAllColumns(Table table);
        void GenerateDelete(Table table);
        void GenerateDeleteAll(Table table);
        void GenerateSaveChanges(Table table);
    }
}
