package truss.compiler.preprocessor;

public class PreProcessException extends Exception {
    private final int line;

    public PreProcessException(String message, int line) {
        super(message);

        this.line = line;
    }

    public int getLine() {
        return line;
    }
}
