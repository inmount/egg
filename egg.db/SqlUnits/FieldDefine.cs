using System;
using System.Collections.Generic;
using System.Text;

namespace egg.db.SqlUnits {

    /// <summary>
    /// 字段定义
    /// </summary>
    public class FieldDefine : egg.Object, ISqlStringable {

        /// <summary>
        /// 获取或设置字段名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置字段类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 获取或设置字段大小
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// 获取或设置字段精度
        /// </summary>
        public int Float { get; set; }

        /// <summary>
        /// 获取是否为复杂对象
        /// </summary>
        public bool IsComplicated { get { return false; } set { } }

        /// <summary>
        /// 获取对应数据库标准的类型表示方式
        /// </summary>
        /// <param name="tp"></param>
        /// <returns></returns>
        public string GetTypeString(DatabaseTypes tp) {
            string res = "";
            string stp = this.Type.ToLower();
            switch (tp) {
                case DatabaseTypes.Microsoft_Office_Access:
                case DatabaseTypes.Microsoft_Office_Access_v12:
                case DatabaseTypes.Microsoft_SQL_Server:
                    switch (stp) {
                        case "string":
                        case "varchar":
                        case "nvarchar":
                            //字符串类型
                            res = $"VARCHAR({this.Size})";
                            break;
                        case "double":
                        case "float":
                        case "decimal":
                        case "numeric":
                            //带经度的数字
                            res = $"numeric({this.Size},{this.Float})";
                            break;
                        case "long":
                        case "bigint":
                            //长整型
                            res = $"bigint";
                            break;
                        case "integer":
                        case "int":
                            //整型
                            res = $"int";
                            break;
                        case "text":
                            //可扩展字符串
                            res = $"text";
                            break;
                        default:
                            throw new Exception($"未知数据类型:{this.Type}");
                    }
                    break;
                case DatabaseTypes.MySQL:
                    switch (stp) {
                        case "string":
                        case "varchar":
                        case "nvarchar":
                            //字符串类型
                            res = $"VARCHAR({this.Size})";
                            break;
                        case "double":
                        case "float":
                        case "decimal":
                        case "numeric":
                            //带经度的数字
                            res = $"DECIMAL({this.Size},{this.Float})";
                            break;
                        case "long":
                        case "bigint":
                            //长整型
                            res = $"bigint";
                            break;
                        case "integer":
                        case "int":
                            //整型
                            res = $"int";
                            break;
                        case "text":
                            //可扩展字符串
                            res = $"text";
                            break;
                        default:
                            throw new Exception($"未知数据类型:{this.Type}");
                    }
                    break;
                case DatabaseTypes.SQLite:
                case DatabaseTypes.SQLite_3:
                    switch (stp) {
                        case "string":
                        case "varchar":
                        case "nvarchar":
                        case "text":
                            //字符串类型
                            res = $"TEXT";
                            break;
                        case "double":
                        case "float":
                        case "decimal":
                        case "numeric":
                            //带经度的数字
                            res = $"REAL";
                            break;
                        case "long":
                        case "bigint":
                        case "integer":
                        case "int":
                            //整型
                            res = $"INTEGER";
                            break;
                        default:
                            throw new Exception($"未知数据类型:{this.Type}");
                    }
                    break;
                case DatabaseTypes.PostgreSQL:
                    switch (stp) {
                        case "string":
                        case "varchar":
                        case "nvarchar":
                            //字符串类型
                            res = $"varchar({this.Size})";
                            break;
                        case "double":
                        case "float":
                        case "decimal":
                        case "numeric":
                            //带经度的数字
                            res = $"numeric({this.Size},{this.Float})";
                            break;
                        case "long":
                        case "bigint":
                            //长整型
                            res = $"bigint";
                            break;
                        case "integer":
                        case "int":
                            //整型
                            res = $"integer";
                            break;
                        case "text":
                            //可扩展字符串
                            res = $"text";
                            break;
                        default:
                            throw new Exception($"未知数据类型:{this.Type}");
                    }
                    break;
                default: throw new Exception($"尚未支持\"{tp.ToString()}\"数据库字段定义");
            }
            return res;
        }

        /// <summary>
        /// 获取对象标准的SQL表示形式
        /// </summary>
        /// <param name="tp"></param>
        /// <param name="multiTable"></param>
        /// <returns></returns>
        public string ToSqlString(DatabaseTypes tp, bool multiTable = false) {
            string res = "";
            string stp = GetTypeString(tp);
            switch (tp) {
                case DatabaseTypes.Microsoft_Office_Access:
                case DatabaseTypes.Microsoft_Office_Access_v12:
                case DatabaseTypes.Microsoft_SQL_Server:
                case DatabaseTypes.SQLite:
                case DatabaseTypes.SQLite_3:
                    res = $"[{this.Name}] {stp}";
                    break;
                case DatabaseTypes.MySQL:
                    res = $"`{this.Name}` {stp}";
                    break;
                case DatabaseTypes.PostgreSQL:
                    res = $"\"{this.Name}\" {stp}";
                    break;
                default: throw new Exception($"尚未支持\"{tp.ToString()}\"数据库字段定义");
            }
            return res;
        }
    }
}
