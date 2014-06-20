using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Truss.Compiler.Generate.Syntax.Xml {
    [XmlRoot("schema")]
    public class SyntaxSchema {
        [XmlAttribute("packageName")]
        public string PackageName { get; set; }

        [XmlElement("class")]
        public List<SyntaxClass> Classes { get; set; }

        [XmlElement("enum")]
        public List<SyntaxEnum> Enums { get; set; }
    }
}
