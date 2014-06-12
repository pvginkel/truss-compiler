package truss.compiler.support;

public class ImmutableArrayException extends RuntimeException {
    public ImmutableArrayException(String message) {
        super(message);
    }
}
