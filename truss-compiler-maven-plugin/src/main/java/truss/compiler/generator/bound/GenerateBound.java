package truss.compiler.generator.bound;

import truss.compiler.generator.CodeWriter;
import truss.compiler.generator.bound.xml.BoundClass;
import truss.compiler.generator.bound.xml.BoundProperty;
import truss.compiler.generator.bound.xml.BoundSchema;
import truss.compiler.generator.bound.xml.BoundSuperArgument;
import org.apache.commons.io.IOUtils;

import javax.xml.bind.JAXBContext;
import javax.xml.bind.JAXBException;
import java.io.*;
import java.util.*;

public class GenerateBound {
    private final Map<String, BoundClass> classes = new HashMap<>();

    public void generate(File definition, File outputDirectory) throws JAXBException, IOException {
        BoundSchema schema = (BoundSchema)JAXBContext.newInstance(BoundSchema.class)
            .createUnmarshaller()
            .unmarshal(definition);

        Collections.sort(schema.getClasses(), new Comparator<BoundClass>() {
            @Override
            public int compare(BoundClass a, BoundClass b) {
                return a.getName().compareTo(b.getName());
            }
        });

        outputDirectory = new File(outputDirectory, schema.getPackageName().replace('.', File.separatorChar));

        for (BoundClass boundClass : schema.getClasses()) {
            classes.put(boundClass.getName(), boundClass);
        }

        for (BoundClass boundClass : schema.getClasses()) {
            if (!boundClass.isIgnore()) {
                generateClass(outputDirectory, boundClass, schema.getPackageName());
            }
        }

        generateVisitor(schema, outputDirectory);
        generateActionVisitor(schema, outputDirectory);
        generateWalker(schema, outputDirectory);
        generateRewriter(schema, outputDirectory);
        generateBoundKind(schema, outputDirectory);
    }

    private void generateVisitor(BoundSchema schema, File outputDirectory) throws IOException {
        CodeWriter cw = new CodeWriter();

        cw.writeln("package %s;", schema.getPackageName());
        cw.writeln();

        cw.writeln("public interface BoundVisitor {");
        cw.indent();

        cw.writeln("boolean isDone();");

        for (BoundClass boundClass : schema.getClasses()) {
            if (boundClass.isAbstract()) {
                continue;
            }

            cw.writeln();
            cw.writeln("void visit%s(%s node) throws Exception;", getStrippedName(boundClass), boundClass.getName());
        }

        cw.unIndent();
        cw.writeln("}");

        writeWhenChanged(new File(outputDirectory, "BoundVisitor.java"), cw.toString());
    }

    private void generateActionVisitor(BoundSchema schema, File outputDirectory) throws IOException {
        CodeWriter cw = new CodeWriter();

        cw.writeln("package %s;", schema.getPackageName());
        cw.writeln();

        cw.writeln("public interface BoundActionVisitor<T> {");
        cw.indent();

        boolean hadOne = false;

        for (BoundClass boundClass : schema.getClasses()) {
            if (boundClass.isAbstract()) {
                continue;
            }

            if (hadOne) {
                cw.writeln();
            } else {
                hadOne = true;
            }
            cw.writeln("T visit%s(%s node) throws Exception;", getStrippedName(boundClass), boundClass.getName());
        }

        cw.unIndent();
        cw.writeln("}");

        writeWhenChanged(new File(outputDirectory, "BoundActionVisitor.java"), cw.toString());
    }

    private void generateWalker(BoundSchema schema, File outputDirectory) throws IOException {
        CodeWriter cw = new CodeWriter();

        cw.writeln("package %s;", schema.getPackageName());
        cw.writeln();
        cw.writeln("import java.util.List;");
        cw.writeln();

        cw.writeln("public class BoundTreeWalker implements BoundVisitor {");
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
        cw.writeln("public void visitList(List<? extends BoundNode> list) throws Exception {");
        cw.indent();
        cw.writeln("for (BoundNode node : list) {");
        cw.indent();
        cw.writeln("visit(node);");
        cw.unIndent();
        cw.writeln("}");
        cw.unIndent();
        cw.writeln("}");
        cw.writeln();
        cw.writeln("public void visit(BoundNode node) throws Exception {");
        cw.indent();
        cw.writeln("if (node != null) {");
        cw.indent();
        cw.writeln("node.accept(this);");
        cw.unIndent();
        cw.writeln("}");
        cw.unIndent();
        cw.writeln("}");

        for (BoundClass boundClass : schema.getClasses()) {
            if (boundClass.isAbstract()) {
                continue;
            }

            cw.writeln();
            cw.writeln("@Override");
            cw.writeln("public void visit%s(%s node) throws Exception {", getStrippedName(boundClass), boundClass.getName());
            cw.indent();

            for (Parameter parameter : resolveParameters(boundClass, false)) {
                BoundProperty property = parameter.property;

                if (!isBoundNode(property)) {
                    continue;
                }

                if (property.isList()) {
                    cw.writeln("visitList(node.%s());", getMethodName(property));
                } else {
                    cw.writeln("visit(node.%s());", getMethodName(property));
                }
            }

            cw.unIndent();
            cw.writeln("}");
        }

        cw.unIndent();
        cw.writeln("}");

        writeWhenChanged(new File(outputDirectory, "BoundTreeWalker.java"), cw.toString());
    }

    private void generateRewriter(BoundSchema schema, File outputDirectory) throws IOException {
        CodeWriter cw = new CodeWriter();

        cw.writeln("package %s;", schema.getPackageName());
        cw.writeln();
        cw.writeln("import org.apache.commons.lang.Validate;");
        cw.writeln();
        cw.writeln("import java.util.List;");
        cw.writeln("import java.util.ArrayList;");
        cw.writeln();

        cw.writeln("@SuppressWarnings(\"unchecked\")");
        cw.writeln("public class BoundTreeRewriter implements BoundActionVisitor<BoundNode> {");
        cw.indent();

        cw.writeln("public <T extends BoundNode> List<T> visitList(List<T> nodes) throws Exception {");
        cw.indent();

        cw.writeln("Validate.notNull(nodes, \"nodes\");");
        cw.writeln();

        cw.writeln("if (nodes.size() == 0) {");
        cw.indent();

        cw.writeln("return nodes;");

        cw.unIndent();
        cw.writeln("}");
        cw.writeln();
        cw.writeln("List<T> result = null;");
        cw.writeln();
        cw.writeln("for (int i = 0; i < nodes.size(); i++) {");
        cw.indent();

        cw.writeln("T item = nodes.get(i);");
        cw.writeln("T visited = (T)item.accept(this);");
        cw.writeln();
        cw.writeln("if (item != visited && result == null) {");
        cw.indent();

        cw.writeln("result = new ArrayList<>();");
        cw.writeln("for (int j = 0; j < i; j++) {");
        cw.indent();

        cw.writeln("result.add(nodes.get(j));");

        cw.unIndent();
        cw.writeln("}");

        cw.unIndent();
        cw.writeln("}");
        cw.writeln();
        cw.writeln("if (result != null && visited != null) {");
        cw.indent();

        cw.writeln("result.add(visited);");

        cw.unIndent();
        cw.writeln("}");

        cw.unIndent();
        cw.writeln("}");
        cw.writeln();
        cw.writeln("if (result != null) {");
        cw.indent();

        cw.writeln("return result;");

        cw.unIndent();
        cw.writeln("}");
        cw.writeln();
        cw.writeln("return nodes;");

        cw.unIndent();
        cw.writeln("}");

        cw.writeln();
        cw.writeln("public <T extends BoundNode> T visit(T node) throws Exception {");
        cw.indent();

        cw.writeln("if (node != null) {");
        cw.indent();
        cw.writeln("return (T)node.accept(this);");
        cw.unIndent();
        cw.writeln("}");
        cw.writeln();
        cw.writeln("return null;");
        cw.unIndent();
        cw.writeln("}");

        for (BoundClass boundClass : schema.getClasses()) {
            if (boundClass.isAbstract()) {
                continue;
            }

            cw.writeln();
            cw.writeln("@Override");
            cw.writeln("public BoundNode visit%s(%s node) throws Exception {", getStrippedName(boundClass), boundClass.getName());
            cw.indent();

            List<Parameter> parameters = resolveParameters(boundClass, true);

            for (Parameter parameter : parameters) {
                BoundProperty property = parameter.property;

                if (!isBoundNode(property) || parameter.superArgument != null) {
                    continue;
                }

                if (property.isList()) {
                    cw.writeln(
                        "%s %s = visitList(node.%s());",
                        getType(property),
                        getLocalName(property),
                        getMethodName(property)
                    );
                } else {
                    cw.writeln(
                        "%s %s = visit(node.%s());",
                        getType(property),
                        getLocalName(property),
                        getMethodName(property)
                    );
                }
            }

            cw.write("return node.update(");

            boolean hadOne = false;
            for (Parameter parameter : parameters) {
                if (parameter.superArgument != null) {
                    continue;
                }

                BoundProperty property = parameter.property;

                if (hadOne) {
                    cw.write(", ");
                } else {
                    hadOne = true;
                }
                if (isBoundNode(property)) {
                    cw.write(getLocalName(property));
                } else {
                    cw.write("node.%s()", getMethodName(property));
                }
            }

            cw.writeln(");");

            cw.unIndent();
            cw.writeln("}");
        }

        cw.unIndent();
        cw.writeln("}");

        writeWhenChanged(new File(outputDirectory, "BoundTreeRewriter.java"), cw.toString());
    }

    private boolean isBoundNode(BoundProperty property) {
        return
            property.getType().startsWith("Bound") &&
            !property.getType().endsWith("Operator");
    }

    private void generateClass(File outputDirectory, BoundClass boundClass, String packageName) throws IOException {
        CodeWriter cw = new CodeWriter();

        cw.writeln("package %s;", packageName);
        cw.writeln();

        cw.writeln("import truss.compiler.*;");
        cw.writeln("import truss.compiler.symbols.*;");
        cw.writeln("import truss.compiler.syntax.*;");
        cw.writeln("import org.apache.commons.lang.Validate;");
        cw.writeln();
        cw.writeln("import java.util.List;");
        cw.writeln();

        cw.writeln(
            "public %sclass %s extends %s {",
            boundClass.isAbstract() ? "abstract " : "",
            boundClass.getName(),
            boundClass.getBase()
        );

        cw.indent();

        for (BoundProperty property : boundClass.getProperties()) {
            cw.writeln("private final %s %s;", getType(property), getLocalName(property));
        }

        List<Parameter> parameters = resolveParameters(boundClass, true);

        cw.writeln();
        cw.write(
            "%s %s(",
            boundClass.isAbstract() ? "protected" : "public",
            boundClass.getName()
        );

        writeParameters(cw, parameters);

        cw.writeln(") {");

        cw.indent();

        cw.write("super(");

        boolean hadOne = false;
        for (Parameter parameter : parameters) {
            BoundProperty property = parameter.property;

            if (boundClass.getProperties().indexOf(property) != -1) {
                continue;
            }

            if (hadOne) {
                cw.write(", ");
            } else {
                hadOne = true;
            }

            if (parameter.superArgument == null) {
                cw.write(getLocalName(property));
            } else {
                cw.write(parameter.superArgument);
            }
        }

        cw.writeln(");");
        cw.writeln();

        for (Parameter parameter : parameters) {
            if (parameter.superArgument != null) {
                continue;
            }

            BoundProperty property = parameter.property;

            if (boundClass.getProperties().indexOf(property) == -1) {
                continue;
            }

            if (!property.isNullable() && !property.isList() && !isValueType(property.getType())) {
                cw.writeln("Validate.notNull(%s, \"%s\");", getLocalName(property), getLocalName(property));
            }
        }

        if (boundClass.getValidation() != null) {
            cw.writeRaw(boundClass.getValidation());
        }

        cw.writeln();

        for (BoundProperty property : boundClass.getProperties()) {
            if (property.isList()) {
                cw.writeln("this.%s = CollectionUtils.copyList(%s);", getLocalName(property), getLocalName(property));
            } else {
                cw.writeln("this.%s = %s;", getLocalName(property), getLocalName(property));
            }
        }

        cw.unIndent();
        cw.writeln("}");

        for (BoundProperty property : boundClass.getProperties()) {
            cw.writeln();
            cw.writeln("public %s %s() {", getType(property), getMethodName(property));
            cw.indent();
            cw.writeln("return %s;", getLocalName(property));
            cw.unIndent();
            cw.writeln("}");
        }

        if (!boundClass.isAbstract()) {            cw.writeln();
            cw.writeln("@Override");
            cw.writeln("public BoundKind getKind() {");
            cw.indent();
            cw.writeln("return BoundKind.%s;", getBoundKind(boundClass));
            cw.unIndent();
            cw.writeln("}");

            cw.writeln();
            cw.write("public %s update(", boundClass.getName());
            writeParameters(cw, parameters);
            cw.writeln(") {");
            cw.indent();

            cw.write("if (");
            hadOne = false;
            for (Parameter parameter : parameters) {
                if (parameter.superArgument != null) {
                    continue;
                }

                BoundProperty property = parameter.property;

                if (hadOne) {
                    cw.write(" || ");
                } else {
                    hadOne = true;
                }

                String getter;
                if (boundClass.getProperties().indexOf(property) == -1) {
                    getter = getMethodName(property) + "()";
                } else {
                    getter = "this." + getLocalName(property);
                }

                cw.write(
                    "%s != %s",
                    getter,
                    getLocalName(property)
                );
            }
            cw.writeln(") {");
            cw.indent();

            cw.write("return new %s(", boundClass.getName());
            hadOne = false;
            for (Parameter parameter : parameters) {
                if (parameter.superArgument != null) {
                    continue;
                }

                if (hadOne) {
                    cw.write(", ");
                } else {
                    hadOne = true;
                }
                cw.write(getLocalName(parameter.property));
            }
            cw.writeln(");");

            cw.unIndent();
            cw.writeln("}");
            cw.writeln();
            cw.writeln("return this;");

            cw.unIndent();
            cw.writeln("}");

            cw.writeln();
            cw.writeln("@Override");
            cw.writeln("public void accept(BoundVisitor visitor) throws Exception {");
            cw.indent();
            cw.writeln("if (!visitor.isDone()) {");
            cw.indent();
            cw.writeln("visitor.visit%s(this);", getStrippedName(boundClass));
            cw.unIndent();
            cw.writeln("}");
            cw.unIndent();
            cw.writeln("}");

            cw.writeln();
            cw.writeln("@Override");
            cw.writeln("public <T> T accept(BoundActionVisitor<T> visitor) throws Exception {");
            cw.indent();
            cw.writeln("return visitor.visit%s(this);", getStrippedName(boundClass));
            cw.unIndent();
            cw.writeln("}");
        }

        cw.unIndent();
        cw.writeln("}");

        writeWhenChanged(new File(outputDirectory, boundClass.getName() + ".java"), cw.toString());
    }

    private void generateBoundKind(BoundSchema schema, File outputDirectory) throws IOException {
        CodeWriter cw = new CodeWriter();

        cw.writeln("package %s;", schema.getPackageName());
        cw.writeln();

        cw.writeln("public enum BoundKind {");
        cw.indent();

        for (BoundClass syntaxClass : schema.getClasses()) {
            if (!syntaxClass.isAbstract()) {
                cw.writeln("%s,", getBoundKind(syntaxClass));
            }
        }

        cw.unIndent();
        cw.writeln("}");

        writeWhenChanged(new File(outputDirectory, "BoundKind.java"), cw.toString());
    }

    private String getBoundKind(BoundClass boundClass) {
        String name = getStrippedName(boundClass);

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

    private void writeParameters(CodeWriter cw, List<Parameter> parameters) {
        boolean hadOne = false;

        for (Parameter parameter : parameters) {
            if (parameter.superArgument != null) {
                continue;
            }

            BoundProperty property = parameter.property;

            if (hadOne) {
                cw.write(", ");
            } else {
                hadOne = true;
            }

            cw.write("%s %s", getType(property), getLocalName(property));
        }
    }

    private String getMethodName(BoundProperty property) {
        String methodName = property.getName();
        if (methodName.startsWith("Is") || methodName.startsWith("Has")) {
            methodName = getLocalName(property);
        } else {
            methodName = "get" + methodName;
        }
        return methodName;
    }

    private String getStrippedName(BoundClass boundClass) {
        final String prefix = "Bound";
        assert boundClass.getName().startsWith(prefix);
        return boundClass.getName().substring(prefix.length());
    }

    private boolean isValueType(String type) {
        switch (type) {
            case "boolean":
                return true;
            default:
                return false;
        }
    }

    private String getLocalName(BoundProperty property) {
        String name = property.getName();

        return name.substring(0, 1).toLowerCase() + name.substring(1);
    }

    private String getType(BoundProperty property) {
        if (property.isList()) {
            return "List<" + property.getType() + ">";
        }

        return property.getType();
    }

    private List<Parameter> resolveParameters(BoundClass boundClass, boolean resolveSuperArguments) {
        List<Parameter> baseParameters;

        if (boundClass.getBase() != null) {
            baseParameters = resolveParameters(classes.get(boundClass.getBase()), resolveSuperArguments);
        } else {
            baseParameters = Collections.emptyList();
        }

        List<Parameter> parameters = new ArrayList<>();
        boolean hadLast = false;

        for (Parameter baseParameter : baseParameters) {
            if (!hadLast && baseParameter.property.isLast()) {
                hadLast = true;
                for (BoundProperty property : boundClass.getProperties()) {
                    parameters.add(new Parameter(property));
                }
            }

            if (!resolveSuperArguments || baseParameter.superArgument == null) {
                parameters.add(baseParameter);
            }
        }

        if (!hadLast) {
            for (BoundProperty property : boundClass.getProperties()) {
                parameters.add(new Parameter(property));
            }
        }

        if (resolveSuperArguments) {
            for (BoundSuperArgument superArgument : boundClass.getSuperArguments()) {
                Parameter parameter = null;

                for (Parameter item : parameters) {
                    if (item.property.getName().equals(superArgument.getName())) {
                        parameter = item;
                        break;
                    }
                }

                assert parameter != null;

                parameter.superArgument = superArgument.getValue();
            }
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

    private static class Parameter {
        private Parameter(BoundProperty property) {
            this.property = property;
        }

        BoundProperty property;
        String superArgument;
    }
}
