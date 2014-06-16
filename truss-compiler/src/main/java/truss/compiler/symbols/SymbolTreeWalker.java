package truss.compiler.symbols;

public class SymbolTreeWalker extends AbstractSymbolVisitor {
    @Override
    public void defaultVisit(Symbol symbol) {
        if (symbol instanceof ContainerSymbol) {
            visitList((ContainerSymbol)symbol);
        }
    }

    public void visitList(ContainerSymbol container) {
        for (Symbol symbol : container.getMembers()) {
            symbol.accept(this);
        }
    }
}
