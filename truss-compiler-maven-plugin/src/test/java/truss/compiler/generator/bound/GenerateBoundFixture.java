package truss.compiler.generator.bound;

import org.junit.Test;
import org.junit.runner.RunWith;
import org.junit.runners.JUnit4;

import java.io.File;

@RunWith(JUnit4.class)
public class GenerateBoundFixture {
    @Test
    public void test() throws Exception {
        new GenerateBound().generate(
            new File("truss-compiler/src/main/schema/BoundSchema.xml"),
            new File("truss-compiler/target/generated-sources/truss-compiler-bound")
        );
    }
}
