package truss.compiler.generator;

import org.apache.commons.lang.StringUtils;

public class CodeWriter {
    private boolean hadIndent = false;
    private int indent;
    private final StringBuilder sb = new StringBuilder();

    public void indent() {
        indent++;
    }

    public void unIndent() {
        indent--;
    }

    public void write(String format, Object... args) {
        if (!hadIndent && indent > 0) {
            sb.append(StringUtils.repeat(" ", indent * 4));
            hadIndent = true;
        }

        if (args != null && args.length > 0) {
            format = String.format(format, args);
        }

        sb.append(format);
    }

    public void writeln() {
        hadIndent = false;
        sb.append("\n");
    }

    public void writeln(String format, Object... args) {
        write(format, args);
        writeln();
    }

    @Override
    public String toString() {
        return sb.toString();
    }

    public void writeRaw(String validation) {
        sb.append(validation);
    }
}
