package truss.compiler.symbols;

import org.apache.commons.lang.Validate;
import truss.compiler.Span;

import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

public abstract class Symbol {
    private final List<Span> spans = new ArrayList<>();
    private final List<Span> unmodifiableSpans = Collections.unmodifiableList(spans);

    protected void addSpan(Span span) {
        Validate.notNull(span, "span");

        spans.add(span);
    }

    public List<Span> getSpans() {
        return unmodifiableSpans;
    }

    public String getName() {
        return null;
    }

    public String getMetadataName() {
        return getName();
    }

    public abstract SymbolKind getKind();

    public abstract void accept(SymbolVisitor visitor);
}
