using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Truss.Compiler.Generate.Xml {
    [XmlRoot("class")]
    public class Class {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("base")]
        public string Base { get; set; }

        [XmlAttribute("abstract")]
        public bool IsAbstract { get; set; }

        [XmlAttribute("ignore")]
        public bool IsIgnore { get; set; }

        [XmlElement("property")]
        public List<Property> Properties { get; set; }

        [XmlElement("validation")]
        public string Validation { get; set; }

        [XmlElement("members")]
        public string Members { get; set; }

        [XmlElement("superArgument")]
        public List<SuperArgument> SuperArguments { get; set; } 
    }
}
