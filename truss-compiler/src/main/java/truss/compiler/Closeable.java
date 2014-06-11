package truss.compiler;

public interface Closeable extends AutoCloseable {
    @Override
    void close();
}
