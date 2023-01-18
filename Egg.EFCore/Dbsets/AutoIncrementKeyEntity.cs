using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Egg.EFCore.Dbsets
{
    /// <summary>
    /// 带主键的实例
    /// </summary>
    public class AutoIncrementKeyEntity : Entity<long>
    {

        /// <summary>
        /// 自动增长标识
        /// </summary>
        [Key]
        [AutoIncrement]
        public override long Id { get => base.Id; set => base.Id = value; }

    }
}
