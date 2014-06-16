package truss.compiler.symbols;

public class AbstractSymbolVisitor implements SymbolVisitor {
    private boolean done;

    @Override
    public boolean isDone() {
        return done;
    }

    public void setDone(boolean done) {
        this.done = done;
    }

    public void defaultVisit(Symbol symbol) {
    }

    @Override
    public void visitConstructor(ConstructorSymbol symbol) {
        defaultVisit(symbol);
    }

    @Override
    public void visitConversionOperator(ConversionOperatorSymbol symbol) {
        defaultVisit(symbol);
    }

    @Override
    public void visitDestructor(DestructorSymbol symbol) {
        defaultVisit(symbol);
    }

    @Override
    public void visitEvent(EventSymbol symbol) {
        defaultVisit(symbol);
    }

    @Override
    public void visitEventAccessor(EventAccessorSymbol symbol) {
        defaultVisit(symbol);
    }

    @Override
    public void visitField(FieldSymbol symbol) {
        defaultVisit(symbol);
    }

    @Override
    public void visitGlobal(GlobalSymbol symbol) {
        defaultVisit(symbol);
    }

    @Override
    public void visitMemberMethod(MemberMethodSymbol symbol) {
        defaultVisit(symbol);
    }

    @Override
    public void visitNamespace(NamespaceSymbol symbol) {
        defaultVisit(symbol);
    }

    @Override
    public void visitOperator(OperatorSymbol symbol) {
        defaultVisit(symbol);
    }

    @Override
    public void visitParameter(ParameterSymbol symbol) {
        defaultVisit(symbol);
    }

    @Override
    public void visitProperty(PropertySymbol symbol) {
        defaultVisit(symbol);
    }

    @Override
    public void visitPropertyAccessor(PropertyAccessorSymbol symbol) {
        defaultVisit(symbol);
    }

    @Override
    public void visitType(TypeSymbol symbol) {
        defaultVisit(symbol);
    }

    @Override
    public void visitTypeParameter(TypeParameterSymbol symbol) {
        defaultVisit(symbol);
    }

    @Override
    public void visitInvalidType(InvalidTypeSymbol symbol) {
        defaultVisit(symbol);
    }
}
