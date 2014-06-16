package truss.compiler.symbols;

public class MemberMethodSymbol extends MethodSymbol {
    @Override
    public SymbolKind getKind() {
        return SymbolKind.MEMBER_METHOD;
    }

    @Override
    public void accept(SymbolVisitor visitor) {
        if (!visitor.isDone()) {
            visitor.visitMemberMethod(this);
        }
    }
}
