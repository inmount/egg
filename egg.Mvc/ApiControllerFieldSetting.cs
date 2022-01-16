using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Mvc {

    /// <summary>
    /// 控制器字段设置
    /// </summary>
    public class ApiControllerFieldSetting {

        /// <summary>
        /// 字段数据类型
        /// </summary>
        public enum DataTypes {

            /// <summary>
            /// 字符串
            /// </summary>
            String = 0x00,

            /// <summary>
            /// 整型
            /// </summary>
            Integer = 0x11,

            /// <summary>
            /// 长整型
            /// </summary>
            Long = 0x12,

            /// <summary>
            /// 小数
            /// </summary>
            Decimal = 0x21
        }

        /// <summary>
        /// 获取或设置名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置可用状态
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// 获取或设置字段是否必须指定
        /// </summary>
        public bool IsMust { get; set; }

        /// <summary>
        /// 获取或设置数据类型
        /// </summary>
        public DataTypes DataType { get; set; }

        /// <summary>
        /// 获取或设置默认值
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// 对象实例化
        /// </summary>
        public ApiControllerFieldSetting() {
            this.Enabled = true;
            this.DataType = DataTypes.String;
            this.IsMust = false;
            this.DefaultValue = "";
        }

    }
}
