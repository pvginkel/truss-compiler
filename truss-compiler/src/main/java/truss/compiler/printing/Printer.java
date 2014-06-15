package truss.compiler.printing;

import truss.compiler.syntax.LiteralType;

public interface Printer {
    void identifier(String identifier) throws Exception;

    void indent() throws Exception;

    void keyword(String keyword) throws Exception;

    void lead() throws Exception;

    void literal(String value, LiteralType type) throws Exception;

    void nl() throws Exception;

    void ws() throws Exception;

    void syntax(String syntax) throws Exception;

    void unIndent() throws Exception;
}
