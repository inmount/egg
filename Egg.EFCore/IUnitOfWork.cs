using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Egg.EFCore
{
    /// <summary>
    /// 工作单元
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// 完成
        /// </summary>
        void Complete();

        /// <summary>
        /// 完成
        /// </summary>
        Task CompleteAsync();
    }
}
