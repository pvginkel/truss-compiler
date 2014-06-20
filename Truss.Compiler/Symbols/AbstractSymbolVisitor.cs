using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Truss.Compiler.Symbols {
    public class AbstractSymbolVisitor : ISymbolVisitor {
        public bool Done { get; set; }

        public virtual void DefaultVisit(Symbol symbol) {
        }

        public virtual void VisitConstructor(ConstructorSymbol symbol) {
            DefaultVisit(symbol);
        }

        public virtual void VisitConversionOperator(ConversionOperatorSymbol symbol) {
            DefaultVisit(symbol);
        }

        public virtual void VisitDestructor(DestructorSymbol symbol) {
            DefaultVisit(symbol);
        }

        public virtual void VisitEvent(EventSymbol symbol) {
            DefaultVisit(symbol);
        }

        public virtual void VisitEventAccessor(EventAccessorSymbol symbol) {
            DefaultVisit(symbol);
        }

        public virtual void VisitField(FieldSymbol symbol) {
            DefaultVisit(symbol);
        }

        public virtual void VisitGlobal(GlobalSymbol symbol) {
            DefaultVisit(symbol);
        }

        public virtual void VisitMemberMethod(MemberMethodSymbol symbol) {
            DefaultVisit(symbol);
        }

        public virtual void VisitNamespace(NamespaceSymbol symbol) {
            DefaultVisit(symbol);
        }

        public virtual void VisitOperator(OperatorSymbol symbol) {
            DefaultVisit(symbol);
        }

        public virtual void VisitParameter(ParameterSymbol symbol) {
            DefaultVisit(symbol);
        }

        public virtual void VisitProperty(PropertySymbol symbol) {
            DefaultVisit(symbol);
        }

        public virtual void VisitPropertyAccessor(PropertyAccessorSymbol symbol) {
            DefaultVisit(symbol);
        }

        public virtual void VisitNamedType(NamedTypeSymbol symbol) {
            DefaultVisit(symbol);
        }

        public virtual void VisitTypeParameter(TypeParameterSymbol symbol) {
            DefaultVisit(symbol);
        }

        public virtual void VisitInvalidType(InvalidTypeSymbol symbol) {
            DefaultVisit(symbol);
        }

        public void VisitArrayType(ArrayTypeSymbol symbol) {
            DefaultVisit(symbol);
        }

        public void VisitNullableType(NullableTypeSymbol symbol) {
            DefaultVisit(symbol);
        }

        public void VisitTrackedType(TrackedTypeSymbol symbol) {
            DefaultVisit(symbol);
        }

        public void VisitNakedNullableType(NakedNullableTypeSymbol symbol) {
            DefaultVisit(symbol);
        }

        public void VisitVarType(VarTypeSymbol symbol) {
            DefaultVisit(symbol);
        }
    }
}
