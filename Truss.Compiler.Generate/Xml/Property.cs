using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Truss.Compiler.Generate.Xml {
    [XmlRoot("property")]
    public class Property {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlAttribute("list")]
        public bool IsList { get; set; }

        [XmlAttribute("nullable")]
        public bool IsNullable { get; set; }
        
        [XmlAttribute("last")]
        public bool IsLast { get; set; }
    }
}
