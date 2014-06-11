package truss.compiler;

public interface TestCaseCallback {
    void test(String name, String fileName, String code, String expected) throws Exception;
}
