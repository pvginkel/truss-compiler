using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Truss.Compiler.Generate.Xml {
    [XmlRoot("schema")]
    public class Schema {
        [XmlAttribute("packageName")]
        public string PackageName { get; set; }

        [XmlElement("class")]
        public List<Class> Classes { get; set; }

        [XmlElement("enum")]
        public List<Enum> Enums { get; set; }
    }
}
