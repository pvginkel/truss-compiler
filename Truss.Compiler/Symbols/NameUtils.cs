using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Truss.Compiler.Printing;
using Truss.Compiler.Support;
using Truss.Compiler.Syntax;

namespace Truss.Compiler.Symbols {
    public static class NameUtils {
        public static string MakeMetadataName(string name, ImmutableArray<TypeParameterSyntax> typeParameters) {
            if (name == null) {
                throw new ArgumentNullException("name");
            }
            if (typeParameters == null) {
                throw new ArgumentNullException("typeParameters");
            }

            if (typeParameters.Count == 0) {
                return name;
            }

            return name + "`" + typeParameters.Count;
        }

        public static string GetMetadataName(SimpleNameSyntax name) {
            if (name == null) {
                throw new ArgumentNullException("name");
            }

            string metadataName = name.Identifier;

            if (name is GenericNameSyntax) {
                metadataName += "`" + ((GenericNameSyntax)name).TypeArguments.Count;
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
    }
}
