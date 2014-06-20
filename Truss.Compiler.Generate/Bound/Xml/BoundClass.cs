using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Truss.Compiler.Generate.Bound.Xml {
    [XmlRoot("class")]
    public class BoundClass {
        [XmlAttribute("name")]
        public string Name { get; private set; }

        [XmlAttribute("base")]
        public string Base { get; private set; }

        [XmlAttribute("abstract")]
        public bool IsAbstract { get; private set; }

        [XmlAttribute("ignore")]
        public bool IsIgnore { get; private set; }

        [XmlElement("property")]
        public List<BoundProperty> Properties { get; private set; }

        [XmlElement("superArgument")]
        public List<BoundSuperArgument> SuperArgument { get; private set; }

        [XmlElement("validation")]
        public string Validation { get; private set; }
    }
}
