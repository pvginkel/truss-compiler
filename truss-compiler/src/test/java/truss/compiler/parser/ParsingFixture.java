package truss.compiler.parser;

import truss.compiler.Closeable;
import truss.compiler.FixtureBase;
import truss.compiler.MessageCollection;
import truss.compiler.MessageCollectionScope;
import truss.compiler.printing.SyntaxPrinter;
import truss.compiler.printing.TextPrinter;
import truss.compiler.syntax.CompilationUnitSyntax;
import org.junit.Test;
import org.junit.runner.RunWith;
import org.junit.runners.JUnit4;

import java.io.StringWriter;
import java.io.Writer;

import static org.junit.Assert.assertEquals;

@RunWith(JUnit4.class)
public class ParsingFixture extends FixtureBase {
    @Test
    public void basicSyntax() throws Exception {
        test("BasicSyntax.t3s");
    }

    @Test
    public void namespace() throws Exception {
        test("Namespace.t3s");
    }

    @Test
    public void attributes() throws Exception {
        test("Attributes.t3s");
    }

    @Test
    public void types() throws Exception {
        test("Types.t3s");
    }

    @Test
    public void typeMembers() throws Exception {
        test("TypeMembers.t3s");
    }

    @Test
    public void statements() throws Exception {
        test("Statements.t3s");
    }

    @Test
    public void names() throws Exception {
        test("Names.t3s");
    }

    @Test
    public void expressions() throws Exception {
        test("Expressions.t3s");
    }

    @Override
    public void test(String name, String fileName, String code, String expected) throws Exception {
        String actual;

        try (Writer writer = new StringWriter()) {
            MessageCollection messages = new MessageCollection();
            CompilationUnitSyntax compilationUnit;

            try (Closeable ignored = MessageCollectionScope.create(messages)) {
                compilationUnit = parse(name, code);
            }

            if (messages.hasMessages()) {
                actual = messages.toString().trim();
            } else {
                compilationUnit.accept(new SyntaxPrinter(new TextPrinter(writer)));
                actual = writer.toString().trim();
            }
        }

        assertEquals(String.format("Parse of '%s' failed", name), expected, actual);
    }
}
