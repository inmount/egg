using System;
using System.Collections.Generic;
using System.Text;

namespace egg.db.Orm {

    /// <summary>
    /// Orm专用表定义
    /// </summary>
    public class Table : SqlUnits.Table {

        // 字段集合
        private KeyList<SqlUnits.TableField> fields;

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="name"></param>
        public Table(string name) : base(name) {
            fields = new KeyList<SqlUnits.TableField>();
        }

        /// <summary>
        /// 获取相关的字段定义
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public SqlUnits.TableField this[string name] {
            get {
                if (!fields.ContainsKey(name)) fields[name] = new SqlUnits.TableField(this, name);
                return fields[name];
            }
        }

        /// <summary>
        /// 从现有类型中创建表格定义
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Table CreateTableDefine<T>() where T : Row {
            return CreateTableDefine(typeof(T));
        }

        /// <summary>
        /// 从现有类型中创建表格定义
        /// </summary>
        /// <param name="tp"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Table CreateTableDefine(Type tp) {
            // 获取表相关的设定
            TableAttribute[] tables = (TableAttribute[])tp.GetCustomAttributes(typeof(TableAttribute), false);
            if (tables.Length <= 0) throw new Exception("该对象没有包含表格定义");
            var table = tables[tables.Length - 1];
            if (table.Name.IsEmpty()) table.Name = tp.Name;
            return new Table(table.Name);
        }

    }
}
