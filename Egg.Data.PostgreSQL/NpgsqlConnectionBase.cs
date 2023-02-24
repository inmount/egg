using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Egg.Data.PostgreSQL
{
    public class NpgsqlConnectionBase : IDatabaseConnectionBase
    {
        public bool Any(string sql)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AnyAsync(string sql)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public int ExecuteNonQuery(string sql)
        {
            throw new NotImplementedException();
        }

        public Task<int> ExecuteNonQueryAsync(string sql)
        {
            throw new NotImplementedException();
        }

        public T? GetRow<T>(string sql) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<T?> GetRowAsync<T>(string sql) where T : class
        {
            throw new NotImplementedException();
        }

        public List<T> GetRows<T>(string sql) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<List<T>> GetRowsAsync<T>(string sql) where T : class
        {
            throw new NotImplementedException();
        }

        public T GetValue<T>(string sql)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetValueAsync<T>(string sql)
        {
            throw new NotImplementedException();
        }

        public List<T> GetValues<T>(string sql)
        {
            throw new NotImplementedException();
        }

        public Task<List<T>> GetValuesAsync<T>(string sql)
        {
            throw new NotImplementedException();
        }

        public void Open(string connectionString)
        {
            throw new NotImplementedException();
        }

        public void Open()
        {
            throw new NotImplementedException();
        }
    }
}
