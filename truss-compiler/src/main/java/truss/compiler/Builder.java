package truss.compiler;

import org.apache.commons.lang.Validate;

import java.io.IOException;

public class Builder {
    private final TrussArguments arguments;

    public Builder(TrussArguments arguments) {
        Validate.notNull(arguments, "arguments");

        this.arguments = arguments;
    }

    public int build() throws IOException {
        return 0;
    }
}
