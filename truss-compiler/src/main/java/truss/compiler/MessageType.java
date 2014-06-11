package truss.compiler;

public enum MessageType {
    UNEXPECTED_SYNTAX(1, Severity.ERROR, "Unexpected token '%s'"),
    UNKNOWN(2, Severity.ERROR, "Unexpected error: %s"),
    DUPLICATE_GETTER(3, Severity.ERROR, "Getter has already been defined"),
    DUPLICATE_SETTER(4, Severity.ERROR, "Setter has already been defined"),
    UNEXPECTED_EOF(5, Severity.ERROR, "Unexpected end of file"),
    EXPECTED_SYNTAX(6, Severity.ERROR, "Unexpected syntax; expected '%s'"),
    DUPLICATE_MEMBER(7, Severity.ERROR, "Member '%s' has already been declared"),
    CANNOT_RESOLVE_TYPE(8, Severity.ERROR, "Cannot resolve type '%s'"),
    CANNOT_RESOLVE_IDENTIFIER(9, Severity.ERROR, "Cannot resolve identifier '%s'"),
    DUPLICATE_PARAMETER(10, Severity.ERROR, "Duplicate parameter '%s'"),
    DUPLICATE_LOCAL(11, Severity.ERROR, "Duplicate variable declaration '%s'"),
    CANNOT_INVOKE_EXPRESSION(12, Severity.ERROR, "Cannot invoke expression"),
    CANNOT_RESOLVE_FIELD(13, Severity.ERROR, "Cannot resolve field '%s'"),
    CANNOT_RESOLVE_MODULE(14, Severity.ERROR, "Cannot resolve module '%s'"),
    INVALID_CHAR_CONSTANT(15, Severity.ERROR, "Invalid char constant"),
    INVALID_STRING_CONSTANT(16, Severity.ERROR, "Invalid string constant"),
    INVALID_HEX_CONSTANT(17, Severity.ERROR, "Invalid hex constant"),
    INVALID_INT_CONSTANT(18, Severity.ERROR, "Invalid integer constant"),
    NO_CONTINUE_TARGET(19, Severity.ERROR, "Cannot find target for continue statement"),
    NO_BREAK_TARGET(20, Severity.ERROR, "Cannot find target for break statement"),
    CANNOT_ACCESS_NONE_STATIC_FIELD(21, Severity.ERROR, "Cannot access non-static field '%s'"),
    CANNOT_ACCESS_NONE_STATIC_METHOD(22, Severity.ERROR, "Cannot access non-static method '%s'"),
    CANNOT_CREATE_DELEGATE(23, Severity.ERROR, "Cannot create delegate for the referenced expression"),
    CONSTRUCTOR_CANNOT_BE_STATIC(24, Severity.ERROR, "Constructor cannot be static"),
    INVALID_ENTRY_METHOD_PARAMETER_COUNT(25, Severity.ERROR, "Entry method cannot have more than one argument"),
    ENTRY_METHOD_MUST_BE_STATIC(26, Severity.ERROR, "Entry method must be static"),
    MULTIPLE_ENTRY_METHODS(27, Severity.ERROR, "Only one entry method may appear per library"),
    MULTIPLE_STATIC_INITIALIZERS(28, Severity.ERROR, "Static initializer may only be added once");

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
