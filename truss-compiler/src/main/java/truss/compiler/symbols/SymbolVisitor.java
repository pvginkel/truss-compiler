package truss.compiler.symbols;

public interface SymbolVisitor {
    boolean isDone();

    void visitConstructor(ConstructorSymbol symbol);

    void visitConversionOperator(ConversionOperatorSymbol symbol);

    void visitDestructor(DestructorSymbol symbol);

    void visitEvent(EventSymbol symbol);

    void visitEventAccessor(EventAccessorSymbol symbol);

    void visitField(FieldSymbol symbol);

    void visitGlobal(GlobalSymbol symbol);

    void visitMemberMethod(MemberMethodSymbol symbol);

    void visitNamespace(NamespaceSymbol symbol);

    void visitOperator(OperatorSymbol symbol);

    void visitParameter(ParameterSymbol symbol);

    void visitProperty(PropertySymbol symbol);

    void visitPropertyAccessor(PropertyAccessorSymbol symbol);

    void visitType(TypeSymbol symbol);

    void visitTypeParameter(TypeParameterSymbol symbol);

    void visitInvalidType(InvalidTypeSymbol symbol);
}
