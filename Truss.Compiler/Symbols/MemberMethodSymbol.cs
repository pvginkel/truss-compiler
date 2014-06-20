using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Truss.Compiler.Symbols {
    public class MemberMethodSymbol : MethodSymbol {
        private readonly string _name;
        private readonly string _metadataName;

        public override string Name {
            get { return _name; }
        }

        public override string MetadataName {
            get { return _metadataName; }
        }

        public TypeSymbol ExplicitInterface { get; private set; }

        public MemberMethodSymbol(string name, string metadataName, TypeSymbol explicitInterface, NamedTypeSymbol declaringType, SymbolModifier modifiers, TypeSymbol returnType)
            : base(declaringType, modifiers, returnType) {
            if (name == null) {
                throw new ArgumentNullException("name");
            }
            if (metadataName == null) {
                throw new ArgumentNullException("metadataName");
            }

            _name = name;
            _metadataName = metadataName;
            ExplicitInterface = explicitInterface;
        }

        public override MethodKind MethodKind {
            get { return MethodKind.MemberMethod; }
        }

        public override void Accept(ISymbolVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitMemberMethod(this);
            }
        }
    }
}
