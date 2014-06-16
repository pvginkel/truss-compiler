package truss.compiler.symbols;

import org.apache.commons.lang.Validate;
import truss.compiler.printing.SyntaxPrinter;
import truss.compiler.printing.TextPrinter;
import truss.compiler.support.ImmutableArray;
import truss.compiler.syntax.GenericNameSyntax;
import truss.compiler.syntax.NameSyntax;
import truss.compiler.syntax.SimpleNameSyntax;
import truss.compiler.syntax.TypeParameterSyntax;

import java.io.StringWriter;
import java.io.Writer;

public class NameUtils {
    public static String makeMetadataName(String name, ImmutableArray<TypeParameterSyntax> typeParameters) {
        Validate.notNull(name, "name");
        Validate.notNull(typeParameters, "typeParameters");

        if (typeParameters.size() == 0) {
            return name;
        }

        return name + "`" + typeParameters.size();
    }

    public static String getMetadataName(SimpleNameSyntax name) {
        Validate.notNull(name, "name");

        String metadataName = name.getIdentifier();

        if (name instanceof GenericNameSyntax) {
            metadataName += "`" + ((GenericNameSyntax)name).getTypeArguments().size();
        }

        return metadataName;
    }

    public static String prettyPrint(NameSyntax name) {
        Validate.notNull(name, "name");

        try (Writer writer = new StringWriter()) {
            name.accept(new SyntaxPrinter(new TextPrinter(writer)));

            return writer.toString();
        } catch (Exception e) {
            // This will not fail. However, if it does, we need to return something...

            return name.toString();
        }
    }
}
