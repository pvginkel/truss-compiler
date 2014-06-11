package truss.compiler.support;

import org.apache.commons.lang.Validate;

public abstract class Argument {
    private final String option;
    private final String description;
    private final boolean mandatory;
    private boolean isProvided;

    protected Argument(String option, String description, boolean mandatory) {
        Validate.notNull(option, "option");
        Validate.notNull(description, "description");

        this.option = option;
        this.description = description;
        this.mandatory = mandatory;
    }

    public String getOption() {
        return option;
    }

    public String getDescription() {
        return description;
    }

    public boolean isProvided() {
        return isProvided;
    }

    public void setProvided(boolean isProvided) {
        this.isProvided = isProvided;
    }

    public boolean isMandatory() {
        return mandatory;
    }

    public abstract boolean allowMultiple();

    public abstract boolean hasParameter();

    public abstract void setValue(String value);
}
