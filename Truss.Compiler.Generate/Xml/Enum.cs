using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Truss.Compiler.Generate.Xml {
    [XmlRoot("enum")]
    public class Enum {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("property")]
        public List<Property> Properties { get; set; }
    }
}
