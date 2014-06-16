package truss.compiler.binding;

import org.apache.commons.lang.Validate;
import truss.compiler.symbols.GlobalSymbol;
import truss.compiler.symbols.Symbol;
import truss.compiler.syntax.SyntaxNode;

import java.util.HashMap;
import java.util.Map;

class SymbolManager {
    private final Map<SyntaxNode, Symbol> map = new HashMap<>();
    private final GlobalSymbol globalSymbol = new GlobalSymbol();

    public GlobalSymbol getGlobalSymbol() {
        return globalSymbol;
    }

    public Symbol find(SyntaxNode node) {
        Validate.notNull(node, "node");

        return map.get(node);
    }

    public void put(SyntaxNode node, Symbol symbol) {
        Validate.notNull(node, "node");
        Validate.notNull(symbol, "symbol");

        map.put(node, symbol);
    }
}
