package truss.compiler.support;

import truss.compiler.ArgumentException;
import org.apache.commons.lang.Validate;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class Arguments {
    private final List<String> remaining = new ArrayList<>();
    private final Map<String, Argument> arguments = new HashMap<>();

    public void addArgument(Argument argument) {
        Validate.notNull(argument, "argument");

        Validate.isTrue(!arguments.containsKey(argument.getOption()));

        arguments.put(argument.getOption(), argument);
    }

    public void parse(String[] args) throws ArgumentException {
        Argument parsing = null;

        for (String arg : args) {
            if (parsing != null) {
                parsing.setProvided(true);
                parsing.setValue(arg);
                parsing = null;
            } else {
                parsing = arguments.get(arg);

                if (parsing == null) {
                    remaining.add(arg);
                } else {
                    if (!parsing.hasParameter()) {
                        parsing.setProvided(true);
                        parsing = null;
                    } else if (parsing.isProvided() && !parsing.allowMultiple()) {
                        throw new ArgumentException(String.format("Option '%s' can only appear once", arg));
                    }
                }
            }
        }

        for (Argument argument : arguments.values()) {
            if (argument.isMandatory() && !argument.isProvided()) {
                throw new ArgumentException(String.format("Option '%s' is mandatory", argument.getOption()));
            }
        }
    }

    public List<String> getRemaining() {
        return remaining;
    }
}
