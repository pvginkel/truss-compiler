package truss.compiler;

public class Span {
    public static final String UNKNOWN_FILENAME = "(unknown)";
    public static final Span EMPTY = new Span(null);

    private final String fileName;
    private final int startLine;
    private final int startColumn;
    private final int endLine;
    private final int endColumn;

    public Span(String fileName) {
        this.fileName = fileName;
        startLine = -1;
        startColumn = -1;
        endLine = -1;
        endColumn = -1;
    }

    public Span(String fileName, int startLine, int startColumn) {
        this(fileName, startLine,  startColumn, startLine, startColumn);
    }

    public Span(String fileName, int startLine, int startColumn, int endLine, int endColumn) {
        this.fileName = fileName;
        this.startLine = startLine;
        this.startColumn = startColumn;
        this.endLine = endLine;
        this.endColumn = endColumn;
    }

    public String getFileName() {
        return fileName;
    }

    public int getStartLine() {
        return startLine;
    }

    public int getStartColumn() {
        return startColumn;
    }

    public int getEndLine() {
        return endLine;
    }

    public int getEndColumn() {
        return endColumn;
    }

    @Override
    public String toString() {
        StringBuilder sb = new StringBuilder();

        if (fileName == null) {
            sb.append(UNKNOWN_FILENAME);
        } else {
            sb.append(fileName);
        }

        if (startLine != -1 && startColumn != -1) {
            sb.append('(');
            sb.append(startLine);
            sb.append(',');
            sb.append(startColumn + 1);
            if (endLine != -1 && endColumn != -1 && (startLine != endLine || startColumn != endColumn)) {
                sb.append(',');
                sb.append(endLine);
                sb.append(',');
                sb.append(endColumn + 1);
            }
            sb.append(')');
        }

        return sb.toString();
    }
}
