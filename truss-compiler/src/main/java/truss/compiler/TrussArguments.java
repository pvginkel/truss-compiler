package truss.compiler;

import truss.compiler.support.ArgumentFlag;
import truss.compiler.support.ArgumentMultiple;
import truss.compiler.support.ArgumentOption;
import truss.compiler.support.Arguments;

import java.util.List;

public class TrussArguments extends Arguments {
    private final ArgumentFlag noCorLib = new ArgumentFlag("--no-corlib", "Do not link corlib.dar");
    private final ArgumentOption output = new ArgumentOption("-o", "Output file name", true);
    private final ArgumentMultiple link = new ArgumentMultiple("-l", "DTL libraries to link against", false);

    public TrussArguments(String[] args) throws ArgumentException {
        addArgument(noCorLib);
        addArgument(output);
        addArgument(link);

        parse(args);
    }

    public boolean isNoCorLib() {
        return noCorLib.isProvided();
    }

    public String getOutput() {
        return output.getValue();
    }

    public List<String> getLink() {
        return link.getValues();
    }

    public List<String> getFiles() {
        return getRemaining();
    }
}
