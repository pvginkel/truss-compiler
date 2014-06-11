package truss.compiler.support;

public class ArgumentOption extends Argument {
    private String value;

    public ArgumentOption(String option, String description, boolean mandatory) {
        super(option, description, mandatory);
    }

    @Override
    public boolean allowMultiple() {
        return false;
    }

    @Override
    public boolean hasParameter() {
        return true;
    }

    public String getValue() {
        return value;
    }

    @Override
    public void setValue(String value) {
        this.value = value;
    }
}
