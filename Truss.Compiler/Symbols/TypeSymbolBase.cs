using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Truss.Compiler.Symbols {
    public abstract class TypeSymbolBase : ContainerSymbol {
        private readonly string _name;
        private readonly string _metadataName;

        protected TypeSymbolBase(string name, string metadataName, ContainerSymbol parent)
            : base(parent) {
            _name = name;
            _metadataName = metadataName;
        }

        public override string Name {
            get { return _name; }
        }

        public override string MetadataName {
            get { return _metadataName; }
        }

        public abstract TypeKind TypeKind { get; }
    }
}
