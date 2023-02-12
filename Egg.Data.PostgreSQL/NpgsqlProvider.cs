using System;
using System.Collections.Generic;
using System.Text;

namespace Egg.Data.PostgreSQL
{
    /// <summary>
    /// PostgreSQL数据库语法供应器
    /// </summary>
    public class NpgsqlProvider : IDatabaseProvider
    {

        public IDatabaseConnectionBase GetDatabaseConnection(string connectionString)
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

        public string GetValueString(string value)
        {
            throw new NotImplementedException();
        }
    }
}
