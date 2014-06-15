package truss.compiler.printing;

import truss.compiler.syntax.LiteralType;
import org.apache.commons.lang.StringUtils;
import org.apache.commons.lang.Validate;

import java.io.Writer;

public class TextPrinter implements Printer {
    private final Writer writer;
    private int indent;
    private String indentation;

    public TextPrinter(Writer writer) {
        Validate.notNull(writer, "writer");

        this.writer = writer;
        indentation = "    ";
    }

    public String getIndentation() {
        return indentation;
    }

    public void setIndentation(String indentation) {
        this.indentation = indentation;
    }

    @Override
    public void identifier(String identifier) throws Exception {
        writer.write(identifier);
    }

    @Override
    public void indent() throws Exception {
        indent++;
    }

    @Override
    public void keyword(String keyword) throws Exception {
        writer.write(keyword);
    }

    @Override
    public void lead() throws Exception {
        writer.write(StringUtils.repeat(indentation, indent));
    }

    @Override
    public void literal(String value, LiteralType type) throws Exception {
        writer.write(value);
    }

    @Override
    public void nl() throws Exception {
        writer.write("\n");
    }

    @Override
    public void ws() throws Exception {
        writer.write(" ");
    }

    @Override
    public void syntax(String syntax) throws Exception {
        writer.write(syntax);
    }

    @Override
    public void unIndent() throws Exception {
        indent--;
    }
}
