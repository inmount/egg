using System;
using System.Collections.Generic;
using System.Text;

namespace egg.db.Orm {

    /// <summary>
    /// 定义为Orm列
    /// </summary>
    public class FieldAttribute : Attribute, ISqlStringable {

        /// <summary>
        /// 获取或设置名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否为真实字段
        /// </summary>
        public bool IsRealField { get; set; }

        /// <summary>
        /// 字段类型
        /// </summary>
        public FieldTypes FieldType { get; set; }

        /// <summary>
        /// 字段大小
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// 字段精度
        /// </summary>
        public int Float { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        public Values.Value DefaultValue { get; set; }

        /// <summary>
        /// 对象实例化
        /// </summary>
        public FieldAttribute() {
            Name = null;
            IsRealField = true;
            FieldType = FieldTypes.String;
            Size = 0;
            Float = 0;
        }

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
            switch (tp) {
                case DatabaseTypes.Microsoft_Office_Access:
                case DatabaseTypes.Microsoft_Office_Access_v12:
                case DatabaseTypes.Microsoft_SQL_Server:
                    switch (this.FieldType) {
                        case FieldTypes.String:
                            //字符串类型
                            if (this.Size > 0) return $"VARCHAR({this.Size})";
                            return $"text";
                        case FieldTypes.Decimal:
                            //带经度的数字
                            return $"numeric({this.Size},{this.Float})";
                        case FieldTypes.Long:
                            //长整型
                            return $"bigint";
                        case FieldTypes.Integer:
                            //整型
                            return $"int";
                        default:
                            throw new Exception($"未知数据类型:{this.FieldType.ToString()}");
                    }
                case DatabaseTypes.MySQL:
                    switch (this.FieldType) {
                        case FieldTypes.String:
                            //字符串类型
                            if (this.Size > 0) return $"VARCHAR({this.Size})";
                            return $"text";
                        case FieldTypes.Decimal:
                            //带经度的数字
                            return $"DECIMAL({this.Size},{this.Float})";
                        case FieldTypes.Long:
                            //长整型
                            return $"bigint";
                        case FieldTypes.Integer:
                            //整型
                            return $"int";
                        default:
                            throw new Exception($"未知数据类型:{this.FieldType.ToString()}");
                    }
                case DatabaseTypes.SQLite:
                case DatabaseTypes.SQLite_3:
                    switch (this.FieldType) {
                        case FieldTypes.String:
                            //字符串类型
                            return $"TEXT";
                        case FieldTypes.Decimal:
                            //带经度的数字
                            if (this.Float > 0) {
                                return $"REAL";
                            } else {
                                return $"INTEGER";
                            }
                        case FieldTypes.Long:
                        case FieldTypes.Integer:
                            //整型
                            return $"INTEGER";
                        default:
                            throw new Exception($"未知数据类型:{this.FieldType.ToString()}");
                    }
                case DatabaseTypes.PostgreSQL:
                    switch (this.FieldType) {
                        case FieldTypes.String:
                            //字符串类型
                            if (this.Size > 0) return $"varchar({this.Size})";
                            return $"text";
                        case FieldTypes.Decimal:
                            //带经度的数字
                            return $"numeric({this.Size},{this.Float})";
                        case FieldTypes.Long:
                            //长整型
                            return $"bigint";
                        case FieldTypes.Integer:
                            //整型
                            return $"integer";
                        default:
                            throw new Exception($"未知数据类型:{this.FieldType.ToString()}");
                    }
                default: throw new Exception($"尚未支持\"{tp.ToString()}\"数据库字段定义");
            }
        }

        /// <summary>
        /// 获取Sql语句
        /// </summary>
        /// <param name="tp"></param>
        /// <param name="multiTable"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public string ToSqlString(DatabaseTypes tp, bool multiTable = false) {
            string stp = GetTypeString(tp);
            switch (tp) {
                case DatabaseTypes.Microsoft_Office_Access:
                case DatabaseTypes.Microsoft_Office_Access_v12:
                case DatabaseTypes.Microsoft_SQL_Server:
                case DatabaseTypes.SQLite:
                case DatabaseTypes.SQLite_3:
                    return $"[{this.Name}] {stp}";
                case DatabaseTypes.MySQL:
                    return $"`{this.Name}` {stp}";
                case DatabaseTypes.PostgreSQL:
                    return $"\"{this.Name}\" {stp}";
                default: throw new Exception($"尚未支持\"{tp.ToString()}\"数据库字段定义");
            }
        }
    }
}
