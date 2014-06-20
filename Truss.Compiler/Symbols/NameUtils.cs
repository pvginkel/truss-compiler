using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Truss.Compiler.Printing;
using Truss.Compiler.Syntax;

namespace Truss.Compiler.Symbols {
    internal static class NameUtils {
        private static readonly Dictionary<PredefinedType, SpecialType> PredefinedSpecialTypes = new Dictionary<PredefinedType, SpecialType> {
            { PredefinedType.Bool, SpecialType.System_Boolean },
            { PredefinedType.Byte, SpecialType.System_UInt8 },
            { PredefinedType.Char, SpecialType.System_Char },
            { PredefinedType.Decimal, SpecialType.System_Decimal },
            { PredefinedType.Double, SpecialType.System_Double },
            { PredefinedType.Float, SpecialType.System_Float },
            { PredefinedType.Int, SpecialType.System_Int32 },
            { PredefinedType.Long, SpecialType.System_Int64 },
            { PredefinedType.Object, SpecialType.System_Object },
            { PredefinedType.SByte, SpecialType.System_Int8 },
            { PredefinedType.Short, SpecialType.System_Int16 },
            { PredefinedType.String, SpecialType.System_String },
            { PredefinedType.UInt, SpecialType.System_UInt32 },
            { PredefinedType.ULong, SpecialType.System_UInt64 },
            { PredefinedType.UShort, SpecialType.System_UInt16 },
            { PredefinedType.Void, SpecialType.System_Void }
        };

        private static readonly Dictionary<Operator, string> BinaryOperatorNames = new Dictionary<Operator, string> {
            { Operator.Ampersand, "op_BitwiseAnd" },
            { Operator.Asterisk, "op_Multiply" },
            { Operator.Bar, "op_BitwiseOr" },
            { Operator.Caret, "op_ExclusiveOr" },
            { Operator.EqualsEquals, "op_Equality" },
            { Operator.ExclamationEquals, "op_Inequality" },
            { Operator.GreaterThan, "op_GreaterThan" },
            { Operator.GreaterThanEquals, "op_GreaterThanOrEqual" },
            { Operator.GreaterThanGreaterThan, "op_RightShift" },
            { Operator.LessThan, "op_LessThan" },
            { Operator.LessThanEquals, "op_LessThanOrEqual" },
            { Operator.LessThanLessThan, "op_LeftShift" },
            { Operator.Minus, "op_Subtraction" },
            { Operator.Percent, "op_Modulus" },
            { Operator.Plus, "op_Addition" },
            { Operator.Slash, "op_Division" },
            { Operator.Tilde, "op_OnesComplement" }
        };

        private static readonly Dictionary<Operator, string> UnaryOperatorNames = new Dictionary<Operator, string> {
            { Operator.Exclamation, "op_LogicalNot" },
            { Operator.False, "op_False" },
            { Operator.Minus, "op_UnaryNegation" },
            { Operator.MinusMinus, "op_Decrement" },
            { Operator.Plus, "op_UnaryPlus" },
            { Operator.PlusPlus, "op_Increment" },
            { Operator.True, "op_True" }
        };

        private static readonly Dictionary<SpecialType, string> SpecialTypeNames = new Dictionary<SpecialType, string> {
            { SpecialType.System_Boolean, WellKnownNames.System.Boolean },
            { SpecialType.System_Char, WellKnownNames.System.Char },
            { SpecialType.System_Decimal, WellKnownNames.System.Decimal },
            { SpecialType.System_Double, WellKnownNames.System.Double },
            { SpecialType.System_Float, WellKnownNames.System.Float },
            { SpecialType.System_Int16, WellKnownNames.System.Int16 },
            { SpecialType.System_Int32, WellKnownNames.System.Int32 },
            { SpecialType.System_Int64, WellKnownNames.System.Int64 },
            { SpecialType.System_Int8, WellKnownNames.System.Int8 },
            { SpecialType.System_Object, WellKnownNames.System.Object },
            { SpecialType.System_String, WellKnownNames.System.String },
            { SpecialType.System_UInt16, WellKnownNames.System.UInt16 },
            { SpecialType.System_UInt32, WellKnownNames.System.UInt32 },
            { SpecialType.System_UInt64, WellKnownNames.System.UInt64 },
            { SpecialType.System_UInt8, WellKnownNames.System.UInt8 },
            { SpecialType.System_Void, WellKnownNames.System.Void },
            { SpecialType.System_Delegate, WellKnownNames.System.Delegate },
            { SpecialType.System_ValueType, WellKnownNames.System.ValueType },
            { SpecialType.System_Array, WellKnownNames.System.Array },
            { SpecialType.System_Enum, WellKnownNames.System.Enum },
            { SpecialType.System_Reflection_TypeHandle, WellKnownNames.System.Reflection.TypeHandle },
            { SpecialType.System_Reflection_MethodHandle, WellKnownNames.System.Reflection.MethodHandle },
            { SpecialType.System_Reflection_FieldHandle, WellKnownNames.System.Reflection.FieldHandle },
        };

        public static string GetGenericMetadataName(string name, int typeParameterCount) {
            if (name == null) {
                throw new ArgumentNullException("name");
            }

            switch (typeParameterCount) {
                case 0:
                    return name;

                case 1:
                    return name + "<>";

                default:
                    return name + "<" + new string(',', typeParameterCount - 1) + ">";
            }
        }

        public static string GetMetadataName(SimpleNameSyntax name) {
            if (name == null) {
                throw new ArgumentNullException("name");
            }

            string metadataName = name.Identifier;

            if (name is GenericNameSyntax) {
                metadataName = GetGenericMetadataName(metadataName, ((GenericNameSyntax)name).TypeArguments.Count);
            }

            return metadataName;
        }

        public static string PrettyPrint(NameSyntax name) {
            if (name == null) {
                throw new ArgumentNullException("name");
            }

            using (var writer = new StringWriter()) {
                name.Accept(new SyntaxPrinter(new TextPrinter(writer)));

                return writer.ToString();
            }
        }

        public static NameSyntax BuildName(Span span, params string[] parts) {
            NameSyntax result = null;

            foreach (string part in parts) {
                if (result == null) {
                    result = new IdentifierNameSyntax(part, Span.Empty);
                } else {
                    result = new QualifiedNameSyntax(
                        result,
                        new IdentifierNameSyntax(part, Span.Empty),
                        span
                    );
                }
            }

            return result;
        }

        public static NameSyntax BuildPredefinedTypeName(PredefinedType type, Span span) {
            return BuildSpecialTypeName(PredefinedSpecialTypes[type], span);
        }

        public static NameSyntax BuildSpecialTypeName(SpecialType type, Span span) {
            if (span == null) {
                throw new ArgumentNullException("span");
            }

            switch (type) {
                case SpecialType.System_Boolean:
                case SpecialType.System_Char:
                case SpecialType.System_Decimal:
                case SpecialType.System_Double:
                case SpecialType.System_Float:
                case SpecialType.System_Int16:
                case SpecialType.System_Int32:
                case SpecialType.System_Int64:
                case SpecialType.System_Int8:
                case SpecialType.System_Object:
                case SpecialType.System_String:
                case SpecialType.System_UInt16:
                case SpecialType.System_UInt32:
                case SpecialType.System_UInt64:
                case SpecialType.System_UInt8:
                case SpecialType.System_Void:
                    return BuildName(span, WellKnownNames.System.Namespace, SpecialTypeNames[type]);

                case SpecialType.System_Reflection_TypeHandle:
                case SpecialType.System_Reflection_MethodHandle:
                case SpecialType.System_Reflection_FieldHandle:
                    return BuildName(span, WellKnownNames.System.Namespace, WellKnownNames.System.Reflection.Namespace, SpecialTypeNames[type]);

                default:
                    throw new ArgumentOutOfRangeException("type");
            }
        }

        public static string GetArrayName(TypeSymbol elementType, NameType type) {
            return GetName(elementType, type) + "[]";
        }

        private static string GetName(TypeSymbol elementType, NameType type) {
            return type == NameType.Normal ? elementType.Name : elementType.MetadataName;
        }

        public static string GetNullableName(TypeSymbol elementType, NameType type) {
            return GetName(elementType, type) + "?";
        }

        public static string GetTrackedName(TypeSymbol elementType, NameType type) {
            return GetName(elementType, type) + "^";
        }

        public static string GetEventAccessorName(EventAccessorSymbol symbol) {
            string prefix = symbol.AccessorKind == EventAccessorKind.Add ? "add_" : "remove_";

            return prefix + symbol.Event.Name;
        }

        public static string GetPropertyAccessorName(PropertyAccessorSymbol symbol) {
            string prefix = symbol.AccessorKind == PropertyAccessorKind.Get ? "get_" : "set_";

            return prefix + symbol.Property.Name;
        }

        public static string GetConversionOperatorName(ConversionOperatorSymbol symbol) {
            return symbol.Type == ImplicitOrExplicit.Explicit ? "op_Explicit" : "op_Implicit";
        }

        public static string GetOperatorName(OperatorSymbol symbol) {
            if (symbol.Parameters.Count == 1) {
                return UnaryOperatorNames[symbol.Operator];
            }

            return BinaryOperatorNames[symbol.Operator];
        }
    }
}
