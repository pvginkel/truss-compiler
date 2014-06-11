package truss.compiler.preprocessor;

import org.apache.commons.io.IOUtils;
import org.junit.Test;
import org.junit.runner.RunWith;
import org.junit.runners.JUnit4;
import truss.compiler.TestCase;
import truss.compiler.TestCaseCallback;

import java.io.InputStream;
import java.util.Arrays;
import java.util.Collections;
import java.util.HashSet;

import static org.junit.Assert.assertEquals;

@RunWith(JUnit4.class)
public class PreProcessorFixture implements TestCaseCallback {
    @Test
    public void withoutDirectives() throws Exception {
        test("WithoutDirectives.t3s");
    }

    @Test
    public void expressionDirectives() throws Exception {
        test("ExpressionDirectives.t3s");
    }

    @Test
    public void defineExpressions() throws Exception {
        test("DefineExpressions.t3s");
    }

    @Test
    public void nestedDirectives() throws Exception {
        test("NestedDirectives.t3s");
    }

    @Override
    public void test(String name, String fileName, String code, String expected) throws Exception {
        assertEquals(String.format("Parse of '%s' failed", name), expected, parse(name, code).trim());
    }

    private void test(String resourceName) throws Exception {
        try (InputStream is = getClass().getResourceAsStream(resourceName)) {
            TestCase testCase = null;

            for (String line : IOUtils.readLines(is)) {
                if (line.equals("<<<")) {
                    testCase.beginExpected();
                } else if (line.startsWith(">>>")) {
                    if (testCase != null) {
                        testCase.test(this);
                    }
                    testCase = new TestCase(line.substring(3).trim(), resourceName);
                } else if (testCase != null) {
                    testCase.append(line);
                }
            }

            if (testCase != null) {
                testCase.test(this);
            }
        }
    }

    protected String parse(String name, String code) throws Exception {
        char[] data;
        try {
            data = PreProcessor.process(
                code.toCharArray(),
                code.length(),
                Collections.unmodifiableSet(new HashSet<>(Arrays.asList("DEFINE1", "DEFINE2")))
            );
        } catch (PreProcessException e) {
            return e.getMessage();
        }

        return new String(data);
    }
}
