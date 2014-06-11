package truss.compiler;

public class TestCase {
    private final StringBuilder sb = new StringBuilder();
    private final String name;
    private final String fileName;
    private String code;

    public TestCase(String name, String fileName) {
        this.name = name;
        this.fileName = fileName;
    }

    public void beginExpected() {
        code = sb.toString();
        sb.setLength(0);
    }

    public void append(String line) {
        sb.append(line);
        sb.append("\n");
    }

    public void test(TestCaseCallback callback) throws Exception {
        String expected;
        if (code == null) {
            code = sb.toString();
            expected = code;
        } else {
            expected = sb.toString();
        }

        callback.test(name, fileName, code.trim(), expected.trim());
    }
}
