using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Egg.EFCore.Dbsets
{
    /// <summary>
    /// 带主键的实例
    /// </summary>
    public class GuidKeyEntity : Entity<string>
    {

        /// <summary>
        /// GUID标识
        /// </summary>
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Column(TypeName = "varchar(50)")]
        public override string Id { get => base.Id; set => base.Id = value; }

        /// <summary>
        /// 对象实例化
        /// </summary>
        public GuidKeyEntity()
        {
            base.Id = Guid.NewGuid().ToString("N");
        }

    }
}
