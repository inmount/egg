using egg.db.Orm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test {

    [Table("Test")]
    internal class OrmTest : Row {

        /// <summary>
        /// 名称
        /// </summary>
        [Field(FieldType = FieldTypes.String, Size = 50)]
        public string Name { get { return base["Name"]; } set { base["Name"] = value; } }

    }
}
