using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Truss.Compiler.Symbols {
    public class PropertySymbol : Symbol {
        private readonly string _name;
        private readonly SymbolKind _kind;

        public PropertySymbol(NamedTypeSymbol declaringType, SymbolModifier modifiers, TypeSymbol type, TypeSymbol explicitInterface, string name, bool isIndexer) {
            if (declaringType == null) {
                throw new ArgumentNullException("declaringType");
            }
            if (type == null) {
                throw new ArgumentNullException("type");
            }
            if (name == null) {
                throw new ArgumentNullException("name");
            }
            Debug.Assert((modifiers & SymbolModifier.AccessModifiers) != 0);

            _name = name;
            DeclaringType = declaringType;
            Modifiers = modifiers;
            Type = type;
            ExplicitInterface = explicitInterface;
            _kind = isIndexer ? SymbolKind.IndexerProperty : SymbolKind.Property;
        }

        public NamedTypeSymbol DeclaringType { get; private set; }

        public SymbolModifier Modifiers { get; private set; }

        public SymbolModifier Access {
            get { return Modifiers & SymbolModifier.AccessModifiers; }
        }

        public TypeSymbol Type { get; private set; }

        public TypeSymbol ExplicitInterface { get; private set; }

        public override string Name {
            get { return _name; }
        }

        public PropertyAccessorSymbol GetMethod { get; internal set; }

        public PropertyAccessorSymbol SetMethod { get; internal set; }

        public override SymbolKind Kind {
            get { return _kind; }
        }

        public override void Accept(ISymbolVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitProperty(this);
            }
        }
    }
}
