using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace test {
    internal class ConfigureNode {

        [DataMember(Name = "name")]
        public virtual string Name { get; set; }

        public virtual List<string> exclude { get; set; }
    }
}
