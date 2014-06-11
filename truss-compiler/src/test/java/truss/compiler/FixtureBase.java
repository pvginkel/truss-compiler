package truss.compiler;

import truss.compiler.parser.TrussLexer;
import truss.compiler.parser.TrussParser;
import truss.compiler.syntax.CompilationUnitSyntax;
import org.antlr.runtime.ANTLRStringStream;
import org.antlr.runtime.CommonTokenStream;
import org.apache.commons.io.IOUtils;

import java.io.InputStream;

public abstract class FixtureBase implements TestCaseCallback {
    @Override
    public abstract void test(String name, String fileName, String code, String expected) throws Exception;

    protected void test(String resourceName) throws Exception {
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

    protected CompilationUnitSyntax parse(String name, String code) throws Exception {
        TrussLexer lexer = new TrussLexer(new ANTLRStringStream(code));
        lexer.setFileName(name);
        TrussParser parser = new TrussParser(new CommonTokenStream(lexer));
        parser.setFileName(name);

        CompilationUnitSyntax compilationUnit;

        try {
            compilationUnit = parser.compilationUnit();
        } finally {
            if (MessageCollectionScope.getCurrent().hasMessages()) {
                compilationUnit = null;
            }
        }

        return compilationUnit;
    }

    protected InputStream getSystemResource() {
        return FixtureBase.class.getResourceAsStream("System.dtl");
    }
}
