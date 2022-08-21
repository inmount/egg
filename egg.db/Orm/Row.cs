using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace egg.db.Orm {

    /// <summary>
    /// Orm专用数据类
    /// </summary>
    [Table()]
    public class Row : egg.db.Row {

        /// <summary>
        /// 唯一标识符
        /// </summary>
        [Field(IsRealField = false)]
        public long ID { get { return base["ID"]; } }

    }
}
