package truss.compiler;

import org.apache.commons.lang.Validate;

public enum Severity {
    ERROR("error"),
    WARN("warning"),
    INFO("info");

    private final String code;

    Severity(String code) {
        Validate.notNull(code, "code");

        this.code = code;
    }

    public String getCode() {
        return code;
    }
}
