using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Truss.Compiler.Generate {
    internal static class XmlSerialization {
        public static T Deserialize<T>(string path) {
            using (var stream = File.OpenRead(path)) {
                return (T)new XmlSerializer(typeof(T)).Deserialize(stream);
            }
        }
    }
}
