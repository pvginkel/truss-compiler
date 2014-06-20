using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Truss.Compiler.Symbols {
    public interface ISymbolVisitor {
        bool Done { get; }

        void VisitConstructor(ConstructorSymbol symbol);

        void VisitConversionOperator(ConversionOperatorSymbol symbol);

        void VisitDestructor(DestructorSymbol symbol);

        void VisitEvent(EventSymbol symbol);

        void VisitEventAccessor(EventAccessorSymbol symbol);

        void VisitField(FieldSymbol symbol);

        void VisitGlobal(GlobalSymbol symbol);

        void VisitMemberMethod(MemberMethodSymbol symbol);

        void VisitNamespace(NamespaceSymbol symbol);

        void VisitOperator(OperatorSymbol symbol);

        void VisitParameter(ParameterSymbol symbol);

        void VisitProperty(PropertySymbol symbol);

        void VisitPropertyAccessor(PropertyAccessorSymbol symbol);

        void VisitNamedType(NamedTypeSymbol symbol);

        void VisitTypeParameter(TypeParameterSymbol symbol);

        void VisitInvalidType(InvalidTypeSymbol symbol);

        void VisitArrayType(ArrayTypeSymbol symbol);

        void VisitNullableType(NullableTypeSymbol symbol);

        void VisitTrackedType(TrackedTypeSymbol symbol);

        void VisitNakedNullableType(NakedNullableTypeSymbol symbol);

        void VisitVarType(VarTypeSymbol symbol);
    }
}
