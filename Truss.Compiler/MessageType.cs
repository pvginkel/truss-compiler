using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Truss.Compiler {
    public class MessageType {
        public static MessageType UnexpectedSyntax = new MessageType(1, Severity.Error, "Unexpected token '{0}'");
        public static MessageType Unknown = new MessageType(2, Severity.Error, "Unexpected error: {0}");
        public static MessageType UnexpectedEof = new MessageType(3, Severity.Error, "Unexpected end of file");
        public static MessageType ExpectedSyntax = new MessageType(4, Severity.Error, "Unexpected syntax; expected '{0}'");
        public static MessageType InvalidAttributeTarget = new MessageType(5, Severity.Error, "Invalid attribute target");
        public static MessageType InvalidAccessorDeclarationType = new MessageType(6, Severity.Error, "Invalid accessor declaration type");
        public static MessageType GenericTypeIllegalAttributes = new MessageType(7, Severity.Error, "Attributes are not allowed here");
        public static MessageType GenericTypeIllegalVariance = new MessageType(8, Severity.Error, "Variance is not allowed here");
        public static MessageType GenericTypeIllegalTypeParameter = new MessageType(9, Severity.Error, "Invalid type parameter name");
        public static MessageType GenericTypeIllegalMissingTypeParameter = new MessageType(10, Severity.Error, "Expected an identifier");
        public static MessageType InternalError = new MessageType(11, Severity.Error, "An unknown internal error has occurred: {0}");
        public static MessageType DelegateInvalidModifier = new MessageType(12, Severity.Error, "Modifier '{0}' is invalid on a delegate");
        public static MessageType IdentifierAlreadyDefined = new MessageType(13, Severity.Error, "Identifier '{0}' has already been declared");
        public static MessageType EnumInvalidModifier = new MessageType(14, Severity.Error, "Modifier '{0}' is invalid on an enum");
        public static MessageType InvalidNamespaceIdentifier = new MessageType(15, Severity.Error, "Invalid namespace identifier");
        public static MessageType ClassInvalidModifier = new MessageType(16, Severity.Error, "Modifier '{0}' is invalid on a class");
        public static MessageType StructInvalidModifier = new MessageType(17, Severity.Error, "Modifier '{0}' is invalid on a struct");
        public static MessageType InterfaceInvalidModifier = new MessageType(18, Severity.Error, "Modifier '{0}' is invalid on an interface");
        public static MessageType TypeOtherNotPartial = new MessageType(19, Severity.Error, "All type declarations must be partial");
        public static MessageType InvalidAbstractTypeCombination = new MessageType(20, Severity.Error, "Abstract cannot be combined with sealed or static");
        public static MessageType DuplicateAccessModifier = new MessageType(21, Severity.Error, "Access modifiers conflict");
        public static MessageType StaticImportCannotHaveAlias = new MessageType(22, Severity.Error, "Static imports cannot have an alias defined");
        public static MessageType InvalidAlias = new MessageType(23, Severity.Error, "Alias '{0}' is invalid");
        public static MessageType AmbiguousContainerSymbolMatch = new MessageType(24, Severity.Error, "Namespace or type '{0}' cannot be matched unambiguously");
        public static MessageType CannotResolveName = new MessageType(25, Severity.Error, "Cannot resolve '{0}'");
        public static MessageType MultipleConstraintClauses = new MessageType(26, Severity.Error, "Type parameter cannot appear in multiple type parameter constraint clauses");
        public static MessageType ConstraintWithoutTypeParameter = new MessageType(27, Severity.Error, "Type parameter constraint references an undeclared type parameter");
        public static MessageType DuplicateClassOrStructConstraint = new MessageType(28, Severity.Error, "Duplicate class or struct constraint");
        public static MessageType DuplicateConstructorConstraint = new MessageType(29, Severity.Error, "Duplicate constructor constraint");

        private MessageType(int number, Severity severity, string message) {
            Number = number;
            Severity = severity;
            Message = message;
        }

        public int Number { get; private set; }

        public Severity Severity { get; private set; }

        public string Message { get; private set; }
    }
}
