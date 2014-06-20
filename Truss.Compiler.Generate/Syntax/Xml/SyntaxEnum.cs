using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Truss.Compiler.Generate.Syntax.Xml {
    [XmlRoot("enum")]
    public class SyntaxEnum {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("property")]
        public List<SyntaxProperty> Properties { get; set; }
    }
}
