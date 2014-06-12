package truss.compiler.generator.syntax;

import truss.compiler.generator.CodeWriter;
import truss.compiler.generator.syntax.xml.SyntaxClass;
import truss.compiler.generator.syntax.xml.SyntaxEnum;
import truss.compiler.generator.syntax.xml.SyntaxProperty;
import truss.compiler.generator.syntax.xml.SyntaxSchema;
import org.apache.commons.io.IOUtils;

import javax.xml.bind.JAXBContext;
import javax.xml.bind.JAXBException;
import java.io.*;
import java.util.*;

public class GenerateSyntax {
    private final Map<String, SyntaxClass> classes = new HashMap<>();

    public void generate(File definition, File outputDirectory) throws JAXBException, IOException {
        SyntaxSchema schema = (SyntaxSchema)JAXBContext.newInstance(SyntaxSchema.class)
            .createUnmarshaller()
            .unmarshal(definition);

        Collections.sort(schema.getClasses(), new Comparator<SyntaxClass>() {
            @Override
            public int compare(SyntaxClass a, SyntaxClass b) {
                return a.getName().compareTo(b.getName());
            }
        });

        outputDirectory = new File(outputDirectory, schema.getPackageName().replace('.', File.separatorChar));

        for (SyntaxClass syntaxClass : schema.getClasses()) {
            classes.put(syntaxClass.getName(), syntaxClass);
        }

        for (SyntaxClass syntaxClass : schema.getClasses()) {
            if (!syntaxClass.isIgnore()) {
                generateClass(outputDirectory, syntaxClass, schema.getPackageName());
            }
        }

        for (SyntaxEnum syntaxEnum : schema.getEnums()) {
            generateEnum(outputDirectory, syntaxEnum, schema.getPackageName());
        }

        generateVisitor(schema, outputDirectory);
        generateActionVisitor(schema, outputDirectory);
        generateAbstractVisitor(schema, outputDirectory);
        generateAbstractActionVisitor(schema, outputDirectory);
        generateWalker(schema, outputDirectory);
        generateSyntaxKind(schema, outputDirectory);
    }

    private void generateVisitor(SyntaxSchema schema, File outputDirectory) throws IOException {
        CodeWriter cw = new CodeWriter();

        cw.writeln("package %s;", schema.getPackageName());
        cw.writeln();

        cw.writeln("public interface SyntaxVisitor {");
        cw.indent();

        cw.writeln("boolean isDone();");

        for (SyntaxClass syntaxClass : schema.getClasses()) {
            if (syntaxClass.isAbstract()) {
                continue;
            }

            cw.writeln();
            cw.writeln("void visit%s(%s syntax) throws Exception;", getStrippedName(syntaxClass), syntaxClass.getName());
        }

        cw.unIndent();
        cw.writeln("}");

        writeWhenChanged(new File(outputDirectory, "SyntaxVisitor.java"), cw.toString());
    }

    private void generateActionVisitor(SyntaxSchema schema, File outputDirectory) throws IOException {
        CodeWriter cw = new CodeWriter();

        cw.writeln("package %s;", schema.getPackageName());
        cw.writeln();

        cw.writeln("public interface SyntaxActionVisitor<T> {");
        cw.indent();

        boolean hadOne = false;

        for (SyntaxClass syntaxClass : schema.getClasses()) {
            if (syntaxClass.isAbstract()) {
                continue;
            }

            if (hadOne) {
                cw.writeln();
            } else {
                hadOne = true;
            }
            cw.writeln("T visit%s(%s syntax) throws Exception;", getStrippedName(syntaxClass), syntaxClass.getName());
        }

        cw.unIndent();
        cw.writeln("}");

        writeWhenChanged(new File(outputDirectory, "SyntaxActionVisitor.java"), cw.toString());
    }

    private void generateAbstractVisitor(SyntaxSchema schema, File outputDirectory) throws IOException {
        CodeWriter cw = new CodeWriter();

        cw.writeln("package %s;", schema.getPackageName());
        cw.writeln();

        cw.writeln("public class AbstractSyntaxVisitor implements SyntaxVisitor {");
        cw.indent();

        cw.writeln("private boolean done;");
        cw.writeln();

        cw.writeln("public boolean isDone() {");
        cw.indent();
        cw.writeln("return done;");
        cw.unIndent();
        cw.writeln("}");
        cw.writeln();
        cw.writeln("public void setDone(boolean done) {");
        cw.indent();
        cw.writeln("this.done = done;");
        cw.unIndent();
        cw.writeln("}");
        cw.writeln();
        cw.writeln("public void defaultVisit(SyntaxNode syntax) throws Exception {");
        cw.writeln("}");

        for (SyntaxClass syntaxClass : schema.getClasses()) {
            if (syntaxClass.isAbstract()) {
                continue;
            }

            cw.writeln();
            cw.writeln("@Override");
            cw.writeln("public void visit%s(%s syntax) throws Exception {", getStrippedName(syntaxClass), syntaxClass.getName());
            cw.indent();
            cw.writeln("defaultVisit(syntax);");
            cw.unIndent();
            cw.writeln("}");
        }

        cw.unIndent();
        cw.writeln("}");

        writeWhenChanged(new File(outputDirectory, "AbstractSyntaxVisitor.java"), cw.toString());
    }

    private void generateAbstractActionVisitor(SyntaxSchema schema, File outputDirectory) throws IOException {
        CodeWriter cw = new CodeWriter();

        cw.writeln("package %s;", schema.getPackageName());
        cw.writeln();

        cw.writeln("public class AbstractSyntaxActionVisitor<T> implements SyntaxActionVisitor<T> {");
        cw.indent();

        cw.writeln("public T defaultVisit(SyntaxNode syntax) throws Exception {");
        cw.indent();
        cw.writeln("return null;");
        cw.unIndent();
        cw.writeln("}");

        boolean hadOne = false;

        for (SyntaxClass syntaxClass : schema.getClasses()) {
            if (syntaxClass.isAbstract()) {
                continue;
            }

            if (hadOne) {
                cw.writeln();
            } else {
                hadOne = true;
            }
            cw.writeln("@Override");
            cw.writeln("public T visit%s(%s syntax) throws Exception {", getStrippedName(syntaxClass), syntaxClass.getName());
            cw.indent();
            cw.writeln("return defaultVisit(syntax);");
            cw.unIndent();
            cw.writeln("}");
        }

        cw.unIndent();
        cw.writeln("}");

        writeWhenChanged(new File(outputDirectory, "AbstractSyntaxActionVisitor.java"), cw.toString());
    }

    private void generateWalker(SyntaxSchema schema, File outputDirectory) throws IOException {
        CodeWriter cw = new CodeWriter();

        cw.writeln("package %s;", schema.getPackageName());
        cw.writeln();
        cw.writeln("import truss.compiler.support.ImmutableArray;");
        cw.writeln();

        cw.writeln("public class SyntaxTreeWalker implements SyntaxVisitor {");
        cw.indent();

        cw.writeln("private boolean done;");
        cw.writeln();

        cw.writeln("public boolean isDone() {");
        cw.indent();
        cw.writeln("return done;");
        cw.unIndent();
        cw.writeln("}");
        cw.writeln();
        cw.writeln("public void setDone(boolean done) {");
        cw.indent();
        cw.writeln("this.done = done;");
        cw.unIndent();
        cw.writeln("}");
        cw.writeln();
        cw.writeln("public void visitList(ImmutableArray<? extends SyntaxNode> list) throws Exception {");
        cw.indent();
        cw.writeln("for (SyntaxNode node : list) {");
        cw.indent();
        cw.writeln("visit(node);");
        cw.unIndent();
        cw.writeln("}");
        cw.unIndent();
        cw.writeln("}");
        cw.writeln();
        cw.writeln("public void visit(SyntaxNode syntax) throws Exception {");
        cw.indent();
        cw.writeln("if (syntax != null) {");
        cw.indent();
        cw.writeln("syntax.accept(this);");
        cw.unIndent();
        cw.writeln("}");
        cw.unIndent();
        cw.writeln("}");

        for (SyntaxClass syntaxClass : schema.getClasses()) {
            if (syntaxClass.isAbstract()) {
                continue;
            }

            cw.writeln();
            cw.writeln("@Override");
            cw.writeln("public void visit%s(%s syntax) throws Exception {", getStrippedName(syntaxClass), syntaxClass.getName());
            cw.indent();

            for (SyntaxProperty property : resolveParameters(syntaxClass)) {
                if (!isSyntaxNode(property)) {
                    continue;
                }

                if (property.isList()) {
                    cw.writeln("visitList(syntax.%s());", getMethodName(property));
                } else {
                    cw.writeln("visit(syntax.%s());", getMethodName(property));
                }
            }

            cw.unIndent();
            cw.writeln("}");
        }

        cw.unIndent();
        cw.writeln("}");

        writeWhenChanged(new File(outputDirectory, "SyntaxTreeWalker.java"), cw.toString());
    }

    private void generateSyntaxKind(SyntaxSchema schema, File outputDirectory) throws IOException {
        CodeWriter cw = new CodeWriter();

        cw.writeln("package %s;", schema.getPackageName());
        cw.writeln();

        cw.writeln("public enum SyntaxKind {");
        cw.indent();

        for (SyntaxClass syntaxClass : schema.getClasses()) {
            if (!syntaxClass.isAbstract()) {
                cw.writeln("%s,", getSyntaxKind(syntaxClass));
            }
        }

        cw.unIndent();
        cw.writeln("}");

        writeWhenChanged(new File(outputDirectory, "SyntaxKind.java"), cw.toString());
    }

    private boolean isSyntaxNode(SyntaxProperty property) {
        return property.getType().endsWith("Syntax");
    }

    private void generateClass(File outputDirectory, SyntaxClass schemaClass, String packageName) throws IOException {
        CodeWriter cw = new CodeWriter();

        cw.writeln("package %s;", packageName);
        cw.writeln();

        cw.writeln("import truss.compiler.*;");
        cw.writeln("import truss.compiler.symbols.*;");
        cw.writeln("import truss.compiler.syntax.*;");
        cw.writeln("import org.apache.commons.lang.Validate;");
        cw.writeln();
        cw.writeln("import truss.compiler.support.ImmutableArray;");
        cw.writeln();

        cw.writeln(
            "public %sclass %s extends %s {",
            schemaClass.isAbstract() ? "abstract " : "",
            schemaClass.getName(),
            schemaClass.getBase()
        );

        cw.indent();

        for (SyntaxProperty property : schemaClass.getProperties()) {
            cw.writeln("private final %s %s;", getType(property), getLocalName(property));
        }

        List<SyntaxProperty> parameters = resolveParameters(schemaClass);

        cw.writeln();
        cw.write(
            "%s %s(",
            schemaClass.isAbstract() ? "protected" : "public",
            schemaClass.getName()
        );

        writeParameters(cw, parameters);

        cw.writeln(") {");

        cw.indent();

        cw.write("super(");

        boolean hadOne = false;
        for (SyntaxProperty parameter : parameters) {
            if (schemaClass.getProperties().indexOf(parameter) != -1) {
                continue;
            }

            if (hadOne) {
                cw.write(", ");
            } else {
                hadOne = true;
            }

            cw.write(getLocalName(parameter));
        }

        cw.writeln(");");
        cw.writeln();

        for (SyntaxProperty parameter : parameters) {
            if (schemaClass.getProperties().indexOf(parameter) == -1) {
                continue;
            }

            if (!parameter.isNullable() && !isValueType(parameter.getType())) {
                cw.writeln("Validate.notNull(%s, \"%s\");", getLocalName(parameter), getLocalName(parameter));
            }
        }

        if (schemaClass.getValidation() != null) {
            cw.writeRaw(schemaClass.getValidation());
            cw.writeRaw("\n");
        }

        cw.writeln();

        for (SyntaxProperty property : schemaClass.getProperties()) {
            cw.writeln("this.%s = %s;", getLocalName(property), getLocalName(property));
        }

        cw.unIndent();
        cw.writeln("}");

        for (SyntaxProperty property : schemaClass.getProperties()) {
            cw.writeln();
            cw.writeln("public %s %s() {", getType(property), getMethodName(property));
            cw.indent();
            cw.writeln("return %s;", getLocalName(property));
            cw.unIndent();
            cw.writeln("}");
        }

        if (!schemaClass.isAbstract()) {
            cw.writeln();
            cw.writeln("@Override");
            cw.writeln("public SyntaxKind getKind() {");
            cw.indent();
            cw.writeln("return SyntaxKind.%s;", getSyntaxKind(schemaClass));
            cw.unIndent();
            cw.writeln("}");

            cw.writeln();
            cw.writeln("@Override");
            cw.writeln("public void accept(SyntaxVisitor visitor) throws Exception {");
            cw.indent();
            cw.writeln("if (!visitor.isDone()) {");
            cw.indent();
            cw.writeln("visitor.visit%s(this);", getStrippedName(schemaClass));
            cw.unIndent();
            cw.writeln("}");
            cw.unIndent();
            cw.writeln("}");

            cw.writeln();
            cw.writeln("@Override");
            cw.writeln("public <T> T accept(SyntaxActionVisitor<T> visitor) throws Exception {");
            cw.indent();
            cw.writeln("return visitor.visit%s(this);", getStrippedName(schemaClass));
            cw.unIndent();
            cw.writeln("}");
        }

        if (schemaClass.getMembers() != null) {
            cw.writeRaw(schemaClass.getMembers());
            cw.writeRaw("\n");
        }

        cw.unIndent();
        cw.writeln("}");

        writeWhenChanged(new File(outputDirectory, schemaClass.getName() + ".java"), cw.toString());
    }

    private void generateEnum(File outputDirectory, SyntaxEnum schemaEnum, String packageName) throws IOException {
        CodeWriter cw = new CodeWriter();

        cw.writeln("package %s;", packageName);
        cw.writeln();

        cw.writeln("import truss.compiler.*;");
        cw.writeln("import truss.compiler.symbols.*;");
        cw.writeln("import truss.compiler.syntax.*;");
        cw.writeln("import org.apache.commons.lang.Validate;");
        cw.writeln();
        cw.writeln("import truss.compiler.support.ImmutableArray;");
        cw.writeln();

        cw.writeln(
            "public enum %s {",
            schemaEnum.getName()
        );

        cw.indent();

        for (SyntaxProperty property : schemaEnum.getProperties()) {
            cw.writeln("%s,", property.getName());
        }

        cw.unIndent();
        cw.writeln("}");

        writeWhenChanged(new File(outputDirectory, schemaEnum.getName() + ".java"), cw.toString());
    }

    private String getSyntaxKind(SyntaxClass schemaClass) {
        String name = getStrippedName(schemaClass);

        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < name.length(); i++) {
            char c = name.charAt(i);

            if (i > 0 && Character.isUpperCase(c)) {
                sb.append('_');
            }
            sb.append(Character.toUpperCase(c));
        }

        return sb.toString();
    }

    private void writeParameters(CodeWriter cw, List<SyntaxProperty> parameters) {
        boolean hadOne = false;

        for (SyntaxProperty parameter : parameters) {
            if (hadOne) {
                cw.write(", ");
            } else {
                hadOne = true;
            }

            cw.write("%s %s", getType(parameter), getLocalName(parameter));
        }
    }

    private String getMethodName(SyntaxProperty property) {
        String methodName = property.getName();
        if (methodName.startsWith("Is") || methodName.startsWith("Has")) {
            methodName = getLocalName(property);
        } else {
            methodName = "get" + methodName;
        }
        return methodName;
    }

    private String getStrippedName(SyntaxClass syntaxClass) {
        final String postfix = "Syntax";
        assert syntaxClass.getName().endsWith(postfix);
        return syntaxClass.getName().substring(0, syntaxClass.getName().length() - postfix.length());
    }

    private boolean isValueType(String type) {
        switch (type) {
            case "boolean": return true;
            default: return false;
        }
    }

    private String getLocalName(SyntaxProperty property) {
        String name = property.getName();

        name = name.substring(0, 1).toLowerCase() + name.substring(1);

        switch (name) {
            case "finally":
            case "else":
            case "default":
                name = name + "_";
                break;
        }

        return name;
    }

    private String getType(SyntaxProperty property) {
        if (property.isList()) {
            return "ImmutableArray<" + property.getType() + ">";
        }

        return property.getType();
    }

    private List<SyntaxProperty> resolveParameters(SyntaxClass SyntaxClass) {
        List<SyntaxProperty> baseParameters;

        if (SyntaxClass.getBase() != null) {
            baseParameters = resolveParameters(classes.get(SyntaxClass.getBase()));
        } else {
            baseParameters = Collections.emptyList();
        }

        List<SyntaxProperty> parameters = new ArrayList<>();
        boolean hadLast = false;

        for (SyntaxProperty baseParameter : baseParameters) {
            if (!hadLast && baseParameter.isLast()) {
                hadLast = true;
                parameters.addAll(SyntaxClass.getProperties());
            }

            parameters.add(baseParameter);
        }

        if (!hadLast) {
            parameters.addAll(SyntaxClass.getProperties());
        }

        return parameters;
    }

    private void writeWhenChanged(File file, String content) throws IOException {
        if (file.exists()) {
            try (InputStream is = new FileInputStream(file)) {
                String current = IOUtils.toString(is);
                if (current.equals(content)) {
                    return;
                }
            }
        }

        file.getParentFile().mkdirs();

        try (OutputStream os = new FileOutputStream(file)) {
            IOUtils.write(content, os);
        }
    }
}
