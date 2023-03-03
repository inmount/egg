using Egg.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Egg.Data.Extensions
{
    /// <summary>
    /// 数据库阅读器
    /// </summary>
    public static class DbDataReaderExtensions
    {

        /// <summary>
        /// 从数据库查询结果映射
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mapper"></param>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static T ToEntity<T>(this DbDataReader reader, EntityMapper<T> mapper)
        {
            return mapper.MapReaderToEntity(reader);
        }

        /// <summary>
        /// 读取单个数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        /// <exception cref="DatabaseException"></exception>
        public static T ToValue<T>(this DbDataReader reader)
        {
            return reader[0].ConvertTo<T>();
        }

    }
}
