using Egg.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Egg.Tests.Entity
{
    [Table("people")]
    public class People : SnowflakeKeyEntity
    {
        /// <summary>
        /// Id
        /// </summary>
        [Column("id")]
        public override long Id { get => base.Id; set => base.Id = value; }

        /// <summary>
        /// 名称
        /// </summary>
        [Column("name")]
        [StringLength(128)]
        [Index]
        [Description("名称")]
        public virtual string Name { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        [Column("age")]
        public virtual int Age { get; set; }
    }
}
