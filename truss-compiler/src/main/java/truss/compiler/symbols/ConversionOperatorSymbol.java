package truss.compiler.symbols;

public class ConversionOperatorSymbol extends OperatorSymbolBase {
    @Override
    public SymbolKind getKind() {
        return SymbolKind.CONVERSION_OPERATOR;
    }

    @Override
    public void accept(SymbolVisitor visitor) {
        if (!visitor.isDone()) {
            visitor.visitConversionOperator(this);
        }
    }
}
