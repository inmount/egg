using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Egg.EFCore
{
    /// <summary>
    /// 带主键的实例
    /// </summary>
    public class Entity<T>
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public virtual T Id { get; set; }
    }
}
