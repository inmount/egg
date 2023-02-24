using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Egg.Data
{
    /// <summary>
    /// 数据库连接
    /// </summary>
    public interface IDatabaseConnection : IDatabaseConnectionBase
    {
        /// <summary>
        /// 数据库供应商
        /// </summary>
        IDatabaseProvider Provider { get; }

        #region [=====数据库基础=====]

        /// <summary>
        /// 数据库基础连接
        /// </summary>
        IDatabaseConnectionBase DatabaseConnectionBase { get; }

        /// <summary>
        /// 数据库连接
        /// </summary>
        string ConnectionString { get; }

        #endregion

        #region [=====工作单元=====]

        /// <summary>
        /// 事务单元
        /// </summary>
        UnitOfWork? UnitOfWork { get; }

        /// <summary>
        /// 开始一个新的事务单元
        /// </summary>
        /// <returns></returns>
        UnitOfWork BeginUnitOfWork();

        /// <summary>
        /// 结束当前事务单元
        /// </summary>
        void EndUnitOfWork();

        #endregion

        #region [=====自动化=====]



        #endregion

    }
}
