package truss.compiler.preprocessor;

import org.antlr.runtime.ANTLRStringStream;

import java.util.Set;

public class PreProcessorStringStream extends ANTLRStringStream {
    public PreProcessorStringStream(String input, Set<String> defines) throws PreProcessException {
        super(input);

        init(defines);
    }

    public PreProcessorStringStream(char[] data, int numberOfActualCharsInArray, Set<String> defines) throws PreProcessException {
        super(data, numberOfActualCharsInArray);

        init(defines);
    }

    private void init(Set<String> defines) throws PreProcessException {
        data = PreProcessor.process(data, n, defines);
        n = data.length;
    }
}
