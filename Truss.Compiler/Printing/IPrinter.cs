using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Truss.Compiler.Syntax;

namespace Truss.Compiler.Printing {
    public interface IPrinter {
        void Identifier(string identifier);

        void Indent();

        void Keyword(string keyword);

        void Lead();

        void Literal(string value, LiteralType type);

        void Nl();

        void Ws();

        void Syntax(string syntax);

        void UnIndent();
    }
}
