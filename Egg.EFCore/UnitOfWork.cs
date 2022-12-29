using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Egg.EFCore
{
    /// <summary>
    /// 工作单元
    /// </summary>
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly DbContext _context;

        /// <summary>
        /// 工作单元
        /// </summary>
        /// <param name="context"></param>
        public UnitOfWork(DbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 完成
        /// </summary>
        public void Complete()
        {
            _context.SaveChanges();
        }

        /// <summary>
        /// 完成
        /// </summary>
        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose() { }
    }
}
