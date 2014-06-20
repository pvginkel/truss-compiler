using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Truss.Compiler {
    public static class WellKnownNames {
        public const string AliasGlobal = "global";
        public const string TypeVar = "var";
        public const string DelegateConstructorObjectParameter = "object";
        public const string DelegateConstructorMethodParameter = "method";
        public const string DelegateInvokeMethod = "Invoke";
        public const string PropertySetterValueParameter = "value";
        public const string PropertyIndexer = "Item";

        public static class System {
            public const string Namespace = "System";

            public const string Array = "Array";
            public const string Boolean = "Boolean";
            public const string Char = "Char";
            public const string Decimal = "Decimal";
            public const string Delegate = "Delegate";
            public const string Double = "Double";
            public const string Enum = "Enum";
            public const string Float = "Float";
            public const string Int16 = "Int16";
            public const string Int32 = "Int32";
            public const string Int64 = "Int64";
            public const string Int8 = "Int8";
            public const string Object = "Object";
            public const string String = "String";
            public const string UInt16 = "UInt16";
            public const string UInt32 = "UInt32";
            public const string UInt64 = "UInt64";
            public const string UInt8 = "UInt8";
            public const string ValueType = "ValueType";
            public const string Void = "Void";

            public static class Reflection {
                public const string Namespace = "Reflection";

                public const string TypeHandle = "TypeHandle";
                public const string MethodHandle = "MethodHandle";
                public const string FieldHandle = "FieldHandle";
            }
        }
    }
}
