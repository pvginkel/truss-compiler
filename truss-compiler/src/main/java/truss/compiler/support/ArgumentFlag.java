package truss.compiler.support;

public class ArgumentFlag extends Argument {
    public ArgumentFlag(String option, String description) {
        super(option, description, false);
    }

    @Override
    public boolean allowMultiple() {
        return false;
    }

    @Override
    public boolean hasParameter() {
        return false;
    }

    @Override
    public void setValue(String value) {
        throw new IllegalStateException();
    }
}
