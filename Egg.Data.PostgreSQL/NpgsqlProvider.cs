using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Egg.Data.PostgreSQL
{
    /// <summary>
    /// PostgreSQL数据库语法供应器
    /// </summary>
    public class NpgsqlProvider : IDatabaseProvider
    {
        public string GetColumnAddSqlString(string tableName, PropertyInfo column)
        {
            throw new NotImplementedException();
        }

        public string GetColumnExistsSqlString(string tableName, PropertyInfo property)
        {
            throw new NotImplementedException();
        }

        public IDatabaseConnectionBase GetDatabaseConnection(string connectionString)
        {
            throw new NotImplementedException();
        }

        public string GetFullTableName<T>()
        {
            throw new NotImplementedException();
        }

        public string GetFuncString(string name, object[]? args)
        {
            throw new NotImplementedException();
        }

        public string GetNameString(string name)
        {
            throw new NotImplementedException();
        }

        public string GetTableCreateSqlString<T>()
        {
            throw new NotImplementedException();
        }

        public string GetTableExistsSqlString<T>()
        {
            throw new NotImplementedException();
        }

        public string GetValueString(string value)
        {
            throw new NotImplementedException();
        }
    }
}
