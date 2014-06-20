using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr.Runtime;
using Truss.Compiler.Parser;

namespace Truss.Compiler {
    public class Error {
        public static ErrorType UnexpectedSyntax = new ErrorType(1, Severity.Error, "Unexpected token '{0}'");
        public static ErrorType Unknown = new ErrorType(2, Severity.Error, "Unexpected error: {0}");
        public static ErrorType UnexpectedEof = new ErrorType(3, Severity.Error, "Unexpected end of file");
        public static ErrorType ExpectedSyntax = new ErrorType(4, Severity.Error, "Unexpected syntax; expected '{0}'");
        public static ErrorType InvalidAttributeTarget = new ErrorType(5, Severity.Error, "Invalid attribute target");
        public static ErrorType InvalidAccessorDeclarationType = new ErrorType(6, Severity.Error, "Invalid accessor declaration type");
        public static ErrorType GenericTypeIllegalAttributes = new ErrorType(7, Severity.Error, "Attributes are not allowed here");
        public static ErrorType GenericTypeIllegalVariance = new ErrorType(8, Severity.Error, "Variance is not allowed here");
        public static ErrorType GenericTypeIllegalTypeParameter = new ErrorType(9, Severity.Error, "Invalid type parameter name");
        public static ErrorType GenericTypeIllegalMissingTypeParameter = new ErrorType(10, Severity.Error, "Expected an identifier");
        public static ErrorType InternalError = new ErrorType(11, Severity.Error, "An unknown internal error has occurred: {0}");
        public static ErrorType DelegateInvalidModifier = new ErrorType(12, Severity.Error, "Modifier '{0}' is invalid on a delegate");
        public static ErrorType IdentifierAlreadyDefined = new ErrorType(13, Severity.Error, "Identifier '{0}' has already been declared");
        public static ErrorType EnumInvalidModifier = new ErrorType(14, Severity.Error, "Modifier '{0}' is invalid on an enum");
        public static ErrorType InvalidNamespaceIdentifier = new ErrorType(15, Severity.Error, "Invalid namespace identifier");
        public static ErrorType ClassInvalidModifier = new ErrorType(16, Severity.Error, "Modifier '{0}' is invalid on a class");
        public static ErrorType StructInvalidModifier = new ErrorType(17, Severity.Error, "Modifier '{0}' is invalid on a struct");
        public static ErrorType InterfaceInvalidModifier = new ErrorType(18, Severity.Error, "Modifier '{0}' is invalid on an interface");
        public static ErrorType TypeOtherNotPartial = new ErrorType(19, Severity.Error, "All type declarations must be partial");
        public static ErrorType InvalidAbstractTypeCombination = new ErrorType(20, Severity.Error, "Abstract cannot be combined with sealed or static");
        public static ErrorType DuplicateAccessModifier = new ErrorType(21, Severity.Error, "Access modifiers conflict");
        public static ErrorType StaticImportCannotHaveAlias = new ErrorType(22, Severity.Error, "Static imports cannot have an alias defined");
        public static ErrorType InvalidAlias = new ErrorType(23, Severity.Error, "Alias '{0}' is invalid");
        public static ErrorType AmbiguousContainerSymbolMatch = new ErrorType(24, Severity.Error, "Namespace or type '{0}' cannot be matched unambiguously");
        public static ErrorType CannotResolveName = new ErrorType(25, Severity.Error, "Cannot resolve '{0}'");
        public static ErrorType MultipleConstraintClauses = new ErrorType(26, Severity.Error, "Type parameter cannot appear in multiple type parameter constraint clauses");
        public static ErrorType ConstraintWithoutTypeParameter = new ErrorType(27, Severity.Error, "Type parameter constraint references an undeclared type parameter");
        public static ErrorType DuplicateClassOrStructConstraint = new ErrorType(28, Severity.Error, "Duplicate class or struct constraint");
        public static ErrorType DuplicateConstructorConstraint = new ErrorType(29, Severity.Error, "Duplicate constructor constraint");

        public const string ErrorCodeApplication = "TRS";

        public ErrorType Type { get; private set; }

        public Span Span { get; private set; }

        public string Text { get; private set; }

        public Error(ErrorType type, params object[] args)
            : this(type, Span.Empty, args) {
        }

        public Error(ErrorType type, string fileName, params object[] args)
            : this(type, new Span(fileName), args) {
        }

        public Error(ErrorType type, Span span, params object[] args) {
            if (type == null) {
                throw new ArgumentNullException("type");
            }
            if (span == null) {
                throw new ArgumentNullException("span");
            }

            Type = type;
            Span = span;

            var sb = new StringBuilder();

            sb.Append(span);
            sb.Append(": ");
            sb.Append(type.Severity.ToString().ToLower());
            sb.Append(' ');
            sb.Append(ErrorCodeApplication);
            sb.Append(string.Format("{0:0000}", type.Number));
            sb.Append(": ");

            if (args != null && args.Length > 0) {
                sb.Append(string.Format(type.Message, args));
            } else {
                sb.Append(type.Message);
            }

            Text = sb.ToString();
        }

        public override string ToString() {
            return Text;
        }

        public static Error FromRecognitionException(string fileName, RecognitionException e) {
            var span = new Span(fileName, e.Line, e.CharPositionInLine);

            if (e.Token != null && e.Token.Type == TrussLexer.EOF) {
                return new Error(UnexpectedEof, span);
            }

            return new Error(
                UnexpectedSyntax,
                span,
                e.Token == null ? "" : TrussParser.tokenNames[e.Token.Type]
            );
        }

        public static Error FromMismatchedToken(Span span, IIntStream input, int ttype) {
            if (ttype == TrussLexer.EOF) {
                return new Error(UnexpectedEof, span);
            }

            return new Error(ExpectedSyntax, span, TrussParser.tokenNames[ttype]);
        }

        public static Error FromException(Exception exception) {
            if (exception is RecognitionException) {
                return FromRecognitionException(null, (RecognitionException)exception);
            }

            return new Error(Unknown, exception.Message);
        }
    }
}
