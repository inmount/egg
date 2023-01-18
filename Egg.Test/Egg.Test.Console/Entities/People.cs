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
    /// 人类
    /// </summary>
    [Table("people", Schema = "bas")]
    public class People : AutoIncrementKeyEntity
    {

        /// <summary>
        /// 姓名
        /// </summary>
        public virtual string? Name { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        [Required]
        public virtual int Age { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public virtual bool? Sex { get; set; }
    }
}
