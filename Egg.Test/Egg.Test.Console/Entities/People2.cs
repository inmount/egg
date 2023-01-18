using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Egg.EFCore.Dbsets;

namespace Egg.Test.Console.Entities
{
    /// <summary>
    /// 人类2
    /// </summary>
    [Table("people2", Schema = "public")]
    public class People2 : GuidKeyEntity
    {

        /// <summary>
        /// 姓名
        /// </summary>
        [Column("name", TypeName = "varchar(100)")]
        public virtual string? Name { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Column("name_1", TypeName = "varchar")]
        public virtual string? Name1 { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public virtual byte b { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public virtual int Age { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        [Column("age_1", TypeName = "varchar")]
        public virtual int Age1 { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public virtual bool? Sex { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public virtual double? db { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public virtual float? flt { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public virtual long? lng { get; set; }

        /// <summary>
        /// 详情
        /// </summary>
        public virtual string? Detail { get; set; }
    }
}
