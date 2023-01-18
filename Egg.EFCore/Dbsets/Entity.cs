using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Egg.EFCore.Dbsets
{
    /// <summary>
    /// 带主键的实例
    /// </summary>
    public class Entity<TId> : IEntity<TId>
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public virtual TId Id { get; set; }
    }
}
