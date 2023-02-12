using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Egg.Data
{
    /// <summary>
    /// 事务单元
    /// </summary>
    public class UnitOfWork : IDisposable
    {
        // sql构建器
        private readonly StringBuilder _sqlCache;
        // 数据库连接
        private readonly DatabaseConnection _connection;
        // 数据库连接
        private readonly UnitOfWork? _upperUnitOfWork;
        // 是否工作
        private bool _working;

        /// <summary>
        /// 事务单元
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="uow"></param>
        public UnitOfWork(DatabaseConnection connection)
        {
            _connection = connection;
            _upperUnitOfWork = connection.UnitOfWork;
            _working = true;
            _sqlCache = new StringBuilder();
        }

        /// <summary>
        /// 添加执行脚本
        /// </summary>
        /// <param name="sql"></param>
        public void Add(string sql)
        {
            if (!_working) throw new DatabaseException($"事务已结束");
            _sqlCache.Append(sql);
            if (!sql.EndsWith(";")) _sqlCache.Append(";");
            _sqlCache.AppendLine();
        }

        /// <summary>
        /// 获取sql脚本
        /// </summary>
        /// <returns></returns>
        public string GetSqlString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(_sqlCache.ToString());
            return sb.ToString();
        }

        /// <summary>
        /// 事务完成
        /// </summary>
        public void Complete()
        {
            if (!_working) throw new DatabaseException($"事务已结束");
            // 执行所有脚本
            _connection.DatabaseConnectionBase.ExecuteNonQuery(GetSqlString());
            _sqlCache.Clear();
            // 结束事务
            this.Exit();
        }

        /// <summary>
        /// 事务完成
        /// </summary>
        public async Task CompleteAsync()
        {
            if (!_working) throw new DatabaseException($"事务已结束");
            // 执行所有脚本
            await _connection.DatabaseConnectionBase.ExecuteNonQueryAsync(GetSqlString());
            _sqlCache.Clear();
            // 结束事务
            this.Exit();
        }

        /// <summary>
        /// 退出事务
        /// </summary>
        public void Exit()
        {
            if (!_working) throw new DatabaseException($"事务已结束");
            // 连接中设置上一层
            _connection.UnitOfWork = _upperUnitOfWork;
            _working = false;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (_working)
            {
                // 连接中设置上一层
                _connection.UnitOfWork = _upperUnitOfWork;
                _working = false;
            }
        }
    }
}
