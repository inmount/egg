using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Egg.Data.Entities
{
    /// <summary>
    /// 带雪花Id主键的实例
    /// </summary>
    public class SnowflakeKeyEntity : Entity<long>
    {

        /// <summary>
        /// 雪花Id
        /// </summary>
        public override long Id { get => base.Id; set => base.Id = value; }

        /// <summary>
        /// 对象实例化
        /// </summary>
        public SnowflakeKeyEntity()
        {
            base.Id = egg.Generator.GetSnowflakeId();
        }

    }
}
