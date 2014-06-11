package truss.compiler.preprocessor;

import org.antlr.runtime.ANTLRFileStream;

import java.io.IOException;
import java.util.Set;

public class PreProcessorFileStream extends ANTLRFileStream {
    public PreProcessorFileStream(String fileName, Set<String> defines) throws IOException, PreProcessException {
        super(fileName);

        init(defines);
    }

    public PreProcessorFileStream(String fileName, String encoding, Set<String> defines) throws IOException, PreProcessException {
        super(fileName, encoding);

        init(defines);
    }

    private void init(Set<String> defines) throws PreProcessException {
        data = PreProcessor.process(data, n, defines);
        n = data.length;
    }
}
