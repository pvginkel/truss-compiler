package truss.compiler.support;

import java.util.ArrayList;
import java.util.List;

public class ArgumentMultiple extends Argument {
    private final List<String> values = new ArrayList<>();

    public ArgumentMultiple(String option, String description, boolean mandatory) {
        super(option, description, mandatory);
    }

    @Override
    public boolean allowMultiple() {
        return true;
    }

    @Override
    public boolean hasParameter() {
        return true;
    }

    @Override
    public void setValue(String value) {
        values.add(value);
    }

    public List<String> getValues() {
        return values;
    }
}
