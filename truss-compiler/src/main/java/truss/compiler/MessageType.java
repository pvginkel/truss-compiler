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
    INTERNAL_ERROR(11, Severity.ERROR, "An unknown internal error has occurred: %s"),
    DELEGATE_INVALID_MODIFIER(12, Severity.ERROR, "Modifier '%s' is invalid on a delegate"),
    IDENTIFIER_ALREADY_DEFINED(13, Severity.ERROR, "Identifier '%s' has already been declared"),
    ENUM_INVALID_MODIFIER(14, Severity.ERROR, "Modifier '%s' is invalid on an enum"),
    INVALID_NAMESPACE_IDENTIFIER(15, Severity.ERROR, "Invalid namespace identifier"),
    CLASS_INVALID_MODIFIER(16, Severity.ERROR, "Modifier '%s' is invalid on a class"),
    STRUCT_INVALID_MODIFIER(17, Severity.ERROR, "Modifier '%s' is invalid on a struct"),
    INTERFACE_INVALID_MODIFIER(18, Severity.ERROR, "Modifier '%s' is invalid on an interface"),
    TYPE_OTHER_NOT_PARTIAL(19, Severity.ERROR, "All type declarations must be partial"),
    INVALID_ABSTRACT_TYPE_COMBINATION(20, Severity.ERROR, "Abstract cannot be combined with sealed or static"),
    DUPLICATE_ACCESS_MODIFIER(21, Severity.ERROR, "Access modifiers conflict"),
    STATIC_IMPORT_CANNOT_HAVE_ALIAS(22, Severity.ERROR, "Static imports cannot have an alias defined"),
    INVALID_ALIAS(23, Severity.ERROR, "Alias '%s' is invalid"),
    AMBIGUOUS_CONTAINER_SYMBOL_MATCH(24, Severity.ERROR, "Namespace or type '%s' cannot be matched unambiguously"),
    CANNOT_RESOLVE_NAME(25, Severity.ERROR, "Cannot resolve '%s'"),
    MULTIPLE_CONSTRAINT_CLAUSES(26, Severity.ERROR, "Type parameter cannot appear in multiple type parameter constraint clauses"),
    CONSTRAINT_WITHOUT_TYPE_PARAMETER(27, Severity.ERROR, "Type parameter constraint references an undeclared type parameter"),
    DUPLICATE_CLASS_OR_STRUCT_CONSTRAINT(28, Severity.ERROR, "Duplicate class or struct constraint"),
    DUPLICATE_CONSTRUCTOR_CONSTRAINT(29, Severity.ERROR, "Duplicate constructor constraint");

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
