using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Truss.Compiler.Generate.Bound.Xml {
    [XmlRoot("schema")]
    public class BoundSchema {
        [XmlAttribute("packageName")]
        public string PackageName { get; private set; }

        [XmlElement("class")]
        public List<BoundClass> Classes { get; private set; }
    }
}
