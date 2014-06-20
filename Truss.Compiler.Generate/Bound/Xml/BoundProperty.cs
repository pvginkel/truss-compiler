using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Truss.Compiler.Generate.Bound.Xml {
    [XmlRoot("property")]
    public class BoundProperty {
        [XmlAttribute("name")]
        public string Name { get; private set; }

        [XmlAttribute("type")]
        public string Type { get; private set; }

        [XmlAttribute("list")]
        public bool IsList { get; private set; }

        [XmlAttribute("nullable")]
        public bool IsNullable { get; private set; }

        [XmlAttribute("last")]
        public bool IsLast { get; private set; }
    }
}
