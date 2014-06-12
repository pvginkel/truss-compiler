package truss.compiler;

public enum MessageType {
    UNEXPECTED_SYNTAX(1, Severity.ERROR, "Unexpected token '%s'"),
    UNKNOWN(2, Severity.ERROR, "Unexpected error: %s"),
    UNEXPECTED_EOF(3, Severity.ERROR, "Unexpected end of file"),
    EXPECTED_SYNTAX(4, Severity.ERROR, "Unexpected syntax; expected '%s'"),
    INVALID_ATTRIBUTE_TARGET(5, Severity.ERROR, "Invalid attribute target"),
    INVALID_ACCESSOR_DECLARATION_TYPE(6, Severity.ERROR, "Invalid accessor declaration type");

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
