package truss.compiler.symbols;

public class OperatorSymbol extends OperatorSymbolBase {
    @Override
    public SymbolKind getKind() {
        return SymbolKind.OPERATOR;
    }

    @Override
    public void accept(SymbolVisitor visitor) {
        if (!visitor.isDone()) {
            visitor.visitOperator(this);
        }
    }
}
