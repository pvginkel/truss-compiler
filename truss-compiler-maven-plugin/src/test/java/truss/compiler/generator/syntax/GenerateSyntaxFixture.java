package truss.compiler.generator.syntax;

import org.junit.Test;
import org.junit.runner.RunWith;
import org.junit.runners.JUnit4;

import java.io.File;

@RunWith(JUnit4.class)
public class GenerateSyntaxFixture {
    @Test
    public void test() throws Exception {
        new GenerateSyntax().generate(
            new File("truss-compiler/src/main/schema/SyntaxSchema.xml"),
            new File("truss-compiler/target/generated-sources/truss-compiler-syntax")
        );
    }
}
