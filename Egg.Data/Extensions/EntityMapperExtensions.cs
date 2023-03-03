using Egg.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Egg.Data.Extensions
{
    /// <summary>
    /// 数据映射器扩展
    /// </summary>
    public static class EntityMapperExtensions
    {

        /// <summary>
        /// 从数据库查询结果映射
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mapper"></param>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static T MapReaderToEntity<T>(this EntityMapper<T> mapper, DbDataReader reader)
        {
            return mapper.Map(pro =>
            {
                int idx = reader.GetOrdinal(pro.GetColumnName());
                if (idx >= 0) return reader.GetValue(idx);
                return null;
            });
        }

    }
}
