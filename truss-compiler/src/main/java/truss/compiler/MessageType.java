package truss.compiler;

public enum MessageType {
    UNEXPECTED_SYNTAX(1, Severity.ERROR, "Unexpected token '%s'"),
    UNKNOWN(2, Severity.ERROR, "Unexpected error: %s"),
    UNEXPECTED_EOF(3, Severity.ERROR, "Unexpected end of file"),
    EXPECTED_SYNTAX(4, Severity.ERROR, "Unexpected syntax; expected '%s'"),
    INVALID_ATTRIBUTE_TARGET(5, Severity.ERROR, "Invalid attribute target"),
    INVALID_ACCESSOR_DECLARATION_TYPE(6, Severity.ERROR, "Invalid accessor declaration type"),
    GENERIC_TYPE_ILLEGAL_ATTRIBUTES(7, Severity.ERROR, "Attributes are not allowed here"),
    GENERIC_TYPE_ILLEGAL_VARIANCE(8, Severity.ERROR, "Variance is not allowed here"),
    GENERIC_TYPE_ILLEGAL_TYPE_PARAMETER(9, Severity.ERROR, "Invalid type parameter name"),
    GENERIC_TYPE_ILLEGAL_MISSING_TYPE_PARAMETER(10, Severity.ERROR, "Expected an identifier"),
    INTERNAL_ERROR(11, Severity.ERROR, "An unknown internal error has occurred: %s");

    private final int number;
    private final Severity severity;
    private final String message;

    MessageType(int number, Severity severity, String message) {
        this.number = number;
        this.severity = severity;
        this.message = message;
    }

    public int getNumber() {
        return number;
    }

    public Severity getSeverity() {
        return severity;
    }

    public String getMessage() {
        return message;
    }
}
