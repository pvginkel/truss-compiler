using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Truss.Compiler.Generate.Bound.Xml {
    [XmlRoot("superArgument")]
    public class BoundSuperArgument {
        [XmlAttribute("name")]
        public string Name { get; private set; }

        [XmlAttribute("value")]
        public string Value { get; private set; }
    }
}
