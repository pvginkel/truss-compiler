using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Truss.Compiler.Symbols {
    public class InvalidNamespaceSymbol : NamespaceSymbol {
        public const string InvalidName = "**INVALID NAMESPACE**";

        public InvalidNamespaceSymbol(ContainerSymbol container)
            : base(InvalidName, container) {
        }
    }
}
