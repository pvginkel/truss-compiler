using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Truss.Compiler.Support;
using Truss.Compiler.Syntax;

namespace Truss.Compiler.Symbols {
    public class TypeParameterSymbol : TypeSymbol {
        public Variance Variance { get; private set; }
        
        public TypeFamily Family { get; private set; }
        
        public Nullability Nullability { get; private set; }
        
        public bool RequireDefaultConstructor { get; private set; }
        
        public TypeSymbol BaseTypeConstraint { get; private set; }
        
        public ImmutableArray<TypeSymbol> InterfaceConstraints { get; private set; }

        public override bool IsTracked {
            get { return Family == TypeFamily.Tracked; }
        }

        internal TypeParameterSymbol(Variance variance, string name, string metadataName, ContainerSymbol parent)
            : base(name, metadataName, parent) {
            Variance = variance;
        }

        public override TypeKind TypeKind {
            get { return TypeKind.TypeParameter; }
        }

        public override void Accept(ISymbolVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitTypeParameter(this);
            }
        }

        internal void SetConstraints(TypeFamily family, Nullability nullability, bool requireDefaultConstructor, TypeSymbol baseTypeConstraint, ImmutableArray<TypeSymbol> interfaceConstraints) {
            if (baseTypeConstraint == null) {
                throw new ArgumentNullException("baseTypeConstraint");
            }
            if (interfaceConstraints == null) {
                throw new ArgumentNullException("interfaceConstraints");
            }

            Family = family;
            Nullability = nullability;
            RequireDefaultConstructor = requireDefaultConstructor;
            BaseTypeConstraint = baseTypeConstraint;
            InterfaceConstraints = interfaceConstraints;
        }
    }
}
