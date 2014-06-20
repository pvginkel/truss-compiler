using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr.Runtime;
using Truss.Compiler.Parser;

namespace Truss.Compiler {
    public class Error {
        public static readonly ErrorType UnexpectedSyntax = new ErrorType(1, Severity.Error, "Unexpected token");
        public static readonly ErrorType Unknown = new ErrorType(2, Severity.Error, "Unexpected error: {0}");
        public static readonly ErrorType UnexpectedEof = new ErrorType(3, Severity.Error, "Unexpected end of file");
        public static readonly ErrorType ExpectedSyntax = new ErrorType(4, Severity.Error, "Unexpected syntax; expected '{0}'");
        public static readonly ErrorType InvalidAttributeTarget = new ErrorType(5, Severity.Error, "Invalid attribute target");
        public static readonly ErrorType InvalidAccessorDeclarationType = new ErrorType(6, Severity.Error, "Invalid accessor declaration type");
        public static readonly ErrorType GenericTypeIllegalAttributes = new ErrorType(7, Severity.Error, "Attributes are not allowed here");
        public static readonly ErrorType GenericTypeIllegalVariance = new ErrorType(8, Severity.Error, "Variance is not allowed here");
        public static readonly ErrorType GenericTypeIllegalTypeParameter = new ErrorType(9, Severity.Error, "Invalid type parameter name");
        public static readonly ErrorType GenericTypeIllegalMissingTypeParameter = new ErrorType(10, Severity.Error, "Expected an identifier");
        public static readonly ErrorType InternalError = new ErrorType(11, Severity.Error, "An unknown internal error has occurred: {0}");
        public static readonly ErrorType IdentifierAlreadyDefined = new ErrorType(13, Severity.Error, "Identifier '{0}' has already been declared");
        public static readonly ErrorType InvalidNamespaceIdentifier = new ErrorType(15, Severity.Error, "Invalid namespace identifier");
        public static readonly ErrorType InvalidModifier = new ErrorType(18, Severity.Error, "Invalid modifier '{0}'");
        public static readonly ErrorType TypeOtherNotPartial = new ErrorType(19, Severity.Error, "All type declarations must be partial");
        public static readonly ErrorType InvalidAbstractTypeCombination = new ErrorType(20, Severity.Error, "Abstract cannot be combined with sealed or static");
        public static readonly ErrorType DuplicateAccessModifier = new ErrorType(21, Severity.Error, "Access modifiers conflict");
        public static readonly ErrorType StaticImportCannotHaveAlias = new ErrorType(22, Severity.Error, "Static imports cannot have an alias defined");
        public static readonly ErrorType InvalidAlias = new ErrorType(23, Severity.Error, "Alias '{0}' is invalid");
        public static readonly ErrorType AmbiguousContainerSymbolMatch = new ErrorType(24, Severity.Error, "Namespace or type '{0}' cannot be matched unambiguously");
        public static readonly ErrorType CannotResolveName = new ErrorType(25, Severity.Error, "Cannot resolve '{0}'");
        public static readonly ErrorType MultipleConstraintClauses = new ErrorType(26, Severity.Error, "Type parameter cannot appear in multiple type parameter constraint clauses");
        public static readonly ErrorType ConstraintWithoutTypeParameter = new ErrorType(27, Severity.Error, "Type parameter constraint references an undeclared type parameter");
        public static readonly ErrorType DuplicateClassOrStructConstraint = new ErrorType(28, Severity.Error, "Duplicate class or struct constraint");
        public static readonly ErrorType DuplicateConstructorConstraint = new ErrorType(29, Severity.Error, "Duplicate constructor constraint");
        public static readonly ErrorType DuplicateIdentifier = new ErrorType(30, Severity.Error, "Duplicate identifier '{0}'");
        public static readonly ErrorType UnexpectedArraySizeExpression = new ErrorType(31, Severity.Error, "Unexpected array size expression");
        public static readonly ErrorType InvalidEnumBaseType = new ErrorType(32, Severity.Error, "Enum type can only inherit from an integer type");
        public static readonly ErrorType IllegalClassBaseType = new ErrorType(33, Severity.Error, "Cannot inherit from type");
        public static readonly ErrorType ExpectedInterfaceBaseType = new ErrorType(34, Severity.Error, "Expected interface type");
        public static readonly ErrorType TypeParametersAlreadyDeclared = new ErrorType(35, Severity.Error, "Type parameters have already been declared");
        public static readonly ErrorType DuplicateTypeParameterConstraint = new ErrorType(36, Severity.Error, "Type parameter constraints can only be declared once");
        public static readonly ErrorType InvalidConstructorConstraintPosition = new ErrorType(37, Severity.Error, "Constructor constraint must come last");
        public static readonly ErrorType TypeParameterConstraintNotClassOrInterface = new ErrorType(38, Severity.Error, "Type constraint must be a class or interface");
        public static readonly ErrorType InvalidClassTypeConstraintPosition = new ErrorType(39, Severity.Error, "Class type constraint must come first");
        public static readonly ErrorType InvalidTypeFamilyPosition = new ErrorType(40, Severity.Error, "Type family constraint must come first");
        public static readonly ErrorType UnresolvedIdentifier = new ErrorType(41, Severity.Error, "Cannot resolve identifier '{0}'");
        public static readonly ErrorType MethodMissingReturnType = new ErrorType(42, Severity.Error, "Expected return type");
        public static readonly ErrorType InvalidAccessModifiers = new ErrorType(43, Severity.Error, "Access modifiers cannot be combined");
        public static readonly ErrorType InvalidAbstractModifier = new ErrorType(44, Severity.Error, "Modifier is not valid in combination with the abstract modifier");
        public static readonly ErrorType InvalidExternModifier = new ErrorType(45, Severity.Error, "Modifier is not valid in combination with the abstract extern");
        public static readonly ErrorType InvalidStaticModifier = new ErrorType(46, Severity.Error, "Modifier is not valid in combination with the abstract static");
        public static readonly ErrorType InvalidConstructorModifier = new ErrorType(47, Severity.Error, "Modifier is not valid for constructors");
        public static readonly ErrorType DestructorCannotDeclareParameters = new ErrorType(48, Severity.Error, "Destructor cannot declare parameters");
        public static readonly ErrorType CannotConsumeUntrackedType = new ErrorType(49, Severity.Error, "Consume only applies to tracked types");
        public static readonly ErrorType ParamsMustBeLastParameter = new ErrorType(50, Severity.Error, "Params modifier must be the last modifier");
        public static readonly ErrorType ThisMustBeFirstParameter = new ErrorType(51, Severity.Error, "This modifier must be the first modifier");
        public static readonly ErrorType ExtensionMethodMustBeStatic = new ErrorType(52, Severity.Error, "Extension methods must be static");
        public static readonly ErrorType ExtensionMethodOnlyValidOnStaticMethod = new ErrorType(53, Severity.Error, "Extension methods can only be declared on static classes");
        public static readonly ErrorType ParamsParameterMustBeArray = new ErrorType(54, Severity.Error, "Params parameter type must be an array type");
        public static readonly ErrorType IllegalTypeForTypeConstraint = new ErrorType(55, Severity.Error, "Type is not valid as a type constraint");
        public static readonly ErrorType ExpectedBody = new ErrorType(56, Severity.Error, "Body is mandatory unless method is abstract or extern");
        public static readonly ErrorType EventTypeMustBeDelegate = new ErrorType(57, Severity.Error, "Event type must be a delegate type");
        public static readonly ErrorType DuplicateAccessorDeclaration = new ErrorType(58, Severity.Error, "Duplicate accessor");
        public static readonly ErrorType ExpectedType = new ErrorType(59, Severity.Error, "Expected type");
        public static readonly ErrorType InconsistentAccessModifier = new ErrorType(60, Severity.Error, "Inconsistent access modifier");
        public static readonly ErrorType DuplicateSignature = new ErrorType(61, Severity.Error, "Method has already been declared");

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
