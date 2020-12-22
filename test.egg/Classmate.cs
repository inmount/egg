using System;
using System.Collections.Generic;
using System.Text;
using egg.JsonBean;

namespace test {
    class Classmate : JObject {

        public JString Name { get; set; }

        [JsonOptional]
        public People Manager { get; set; }

        public JArray Peoples { get; private set; }

        [JsonOptional]
        public JString Rename { get; set; }

        public Classmate() {
            this.Peoples = new JArray();
        }

    }
}
