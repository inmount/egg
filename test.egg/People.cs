using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using egg.JsonBean;

namespace test {
    public class People : JObject {

        public JString Name { get; set; }

        public JNumber Age { get; set; }

        public JBoolean IsMale { get; set; }


    }
}
