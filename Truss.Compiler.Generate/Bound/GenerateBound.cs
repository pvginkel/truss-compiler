using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Truss.Compiler.Generate.Bound.Xml;

namespace Truss.Compiler.Generate.Bound {
    public class GenerateBound {
        private readonly Dictionary<string, BoundClass> classes = new Dictionary<string, BoundClass>();

        public void Generate(string definition, string outputDirectory) {
            var schema = XmlSerialization.Deserialize<BoundSchema>(definition);

            schema.Classes.Sort((a, b) => a.Name.CompareTo(b.Name));

            outputDirectory = Path.Combine(outputDirectory, schema.PackageName.Replace('.', Path.DirectorySeparatorChar));

            foreach (var boundClass in schema.Classes) {
                classes.Add(boundClass.Name, boundClass);
            }

            var cw = new CodeWriter();

            cw.WriteLine("using System;");
            cw.WriteLine("using System.Collections.Generic;");
            cw.WriteLine("using System.Linq;");
            cw.WriteLine("using System.Text;");
            cw.WriteLine();
            cw.WriteLine("namespace {0} {", schema.PackageName);

            cw.Indent();

            foreach (var boundClass in schema.Classes) {
                if (!boundClass.IsIgnore) {
                    GenerateClass(cw, boundClass, schema.PackageName);
                }
            }

            GenerateVisitor(schema, cw);
            GenerateActionVisitor(schema, cw);
            GenerateWalker(schema, cw);
            GenerateRewriter(schema, cw);
            GenerateBoundKind(schema, cw);

            cw.UnIndent();
            cw.WriteLine("}");

            WriteWhenChanged(Path.Combine(outputDirectory, "Bound", "BoundSchema.Generated.xml"), cw.ToString());
        }

        private void GenerateVisitor(BoundSchema schema, CodeWriter cw) {
            cw.WriteLine("public interface BoundVisitor {");
            cw.Indent();

            cw.WriteLine("bool isDone();");

            foreach (var boundClass in schema.Classes) {
                if (boundClass.IsAbstract) {
                    continue;
                }

                cw.WriteLine();
                cw.WriteLine("void visit{0}({1} node) throws Exception;", GetStrippedName(boundClass), boundClass.Name);
            }

            cw.UnIndent();
            cw.WriteLine("}");
        }

        private void GenerateActionVisitor(BoundSchema schema, CodeWriter cw) {
            cw.WriteLine("public interface BoundActionVisitor<T> {");
            cw.Indent();

            bool hadOne = false;

            foreach (var boundClass in schema.Classes) {
                if (boundClass.IsAbstract) {
                    continue;
                }

                if (hadOne) {
                    cw.WriteLine();
                } else {
                    hadOne = true;
                }
                cw.WriteLine("T visit{0}({1} node) throws Exception;", GetStrippedName(boundClass), boundClass.Name);
            }

            cw.UnIndent();
            cw.WriteLine("}");
        }

        private void GenerateWalker(BoundSchema schema, CodeWriter cw) {
            cw.WriteLine("public class BoundTreeWalker implements BoundVisitor {");
            cw.Indent();

            cw.WriteLine("private bool done;");
            cw.WriteLine();

            cw.WriteLine("public bool isDone() {");
            cw.Indent();
            cw.WriteLine("return done;");
            cw.UnIndent();
            cw.WriteLine("}");
            cw.WriteLine();
            cw.WriteLine("public void setDone(bool done) {");
            cw.Indent();
            cw.WriteLine("this.done = done;");
            cw.UnIndent();
            cw.WriteLine("}");
            cw.WriteLine();
            cw.WriteLine("public void visitList(ImmutableArray<? extends BoundNode> list) {");
            cw.Indent();
            cw.WriteLine("foreach (BoundNode node in list) {");
            cw.Indent();
            cw.WriteLine("visit(node);");
            cw.UnIndent();
            cw.WriteLine("}");
            cw.UnIndent();
            cw.WriteLine("}");
            cw.WriteLine();
            cw.WriteLine("public void visit(BoundNode node) {");
            cw.Indent();
            cw.WriteLine("if (node != null) {");
            cw.Indent();
            cw.WriteLine("node.accept(this);");
            cw.UnIndent();
            cw.WriteLine("}");
            cw.UnIndent();
            cw.WriteLine("}");

            foreach (var boundClass in schema.Classes) {
                if (boundClass.IsAbstract) {
                    continue;
                }

                cw.WriteLine();
                cw.WriteLine("@Override");
                cw.WriteLine("public void visit{0}({1} node) {", GetStrippedName(boundClass), boundClass.Name);
                cw.Indent();

                foreach (var parameter in ResolveParameters(boundClass, false)) {
                    var property = parameter.Property;

                    if (!IsBoundNode(property)) {
                        continue;
                    }

                    if (property.IsList) {
                        cw.WriteLine("visitList(node.{0}());", GetMethodName(property));
                    } else {
                        cw.WriteLine("visit(node.{0}());", GetMethodName(property));
                    }
                }

                cw.UnIndent();
                cw.WriteLine("}");
            }

            cw.UnIndent();
            cw.WriteLine("}");
        }

        private void GenerateRewriter(BoundSchema schema, CodeWriter cw) {
            cw.WriteLine("public class BoundTreeRewriter implements BoundActionVisitor<BoundNode> {");
            cw.Indent();

            cw.WriteLine("public <T extends BoundNode> ImmutableArray<T> visitList(ImmutableArray<T> nodes) {");
            cw.Indent();

            cw.WriteLine("Validate.notNull(nodes, \"nodes\");");
            cw.WriteLine();

            cw.WriteLine("if (nodes.Count == 0) {");
            cw.Indent();

            cw.WriteLine("return nodes;");

            cw.UnIndent();
            cw.WriteLine("}");
            cw.WriteLine();
            cw.WriteLine("ImmutableArray.Builder<T> result = null;");
            cw.WriteLine();
            cw.WriteLine("for (int i = 0; i < nodes.Count; i++) {");
            cw.Indent();

            cw.WriteLine("T item = nodes.get(i);");
            cw.WriteLine("T visited = (T)item.accept(this);");
            cw.WriteLine();
            cw.WriteLine("if (item != visited && result == null) {");
            cw.Indent();

            cw.WriteLine("result = new ImmutableArray.Builder<>();");
            cw.WriteLine("for (int j = 0; j < i; j++) {");
            cw.Indent();

            cw.WriteLine("result.add(nodes.get(j));");

            cw.UnIndent();
            cw.WriteLine("}");

            cw.UnIndent();
            cw.WriteLine("}");
            cw.WriteLine();
            cw.WriteLine("if (result != null && visited != null) {");
            cw.Indent();

            cw.WriteLine("result.add(visited);");

            cw.UnIndent();
            cw.WriteLine("}");

            cw.UnIndent();
            cw.WriteLine("}");
            cw.WriteLine();
            cw.WriteLine("if (result != null) {");
            cw.Indent();

            cw.WriteLine("return result.build();");

            cw.UnIndent();
            cw.WriteLine("}");
            cw.WriteLine();
            cw.WriteLine("return nodes;");

            cw.UnIndent();
            cw.WriteLine("}");

            cw.WriteLine();
            cw.WriteLine("public <T extends BoundNode> T visit(T node) {");
            cw.Indent();

            cw.WriteLine("if (node != null) {");
            cw.Indent();
            cw.WriteLine("return (T)node.accept(this);");
            cw.UnIndent();
            cw.WriteLine("}");
            cw.WriteLine();
            cw.WriteLine("return null;");
            cw.UnIndent();
            cw.WriteLine("}");

            foreach (var boundClass in schema.Classes) {
                if (boundClass.IsAbstract) {
                    continue;
                }

                cw.WriteLine();
                cw.WriteLine("@Override");
                cw.WriteLine("public BoundNode visit{0}({1} node) {", GetStrippedName(boundClass), boundClass.Name);
                cw.Indent();

                var parameters = ResolveParameters(boundClass, true);

                foreach (var parameter in parameters) {
                    var property = parameter.Property;

                    if (!IsBoundNode(property) || parameter.SuperArgument != null) {
                        continue;
                    }

                    if (property.IsList) {
                        cw.WriteLine(
                            "{0} {1} = visitList(node.{2}());",
                            GetTypeName(property),
                            GetLocalName(property),
                            GetMethodName(property)
                            );
                    } else {
                        cw.WriteLine(
                            "{0} {1} = visit(node.{2}());",
                            GetTypeName(property),
                            GetLocalName(property),
                            GetMethodName(property)
                            );
                    }
                }

                cw.Write("return node.update(");

                bool hadOne = false;
                foreach (var parameter in parameters) {
                    if (parameter.SuperArgument != null) {
                        continue;
                    }

                    var property = parameter.Property;

                    if (hadOne) {
                        cw.Write(", ");
                    } else {
                        hadOne = true;
                    }
                    if (IsBoundNode(property)) {
                        cw.Write(GetLocalName(property));
                    } else {
                        cw.Write("node.{0}()", GetMethodName(property));
                    }
                }

                cw.WriteLine(");");

                cw.UnIndent();
                cw.WriteLine("}");
            }

            cw.UnIndent();
            cw.WriteLine("}");
        }

        private static bool IsBoundNode(BoundProperty property) {
            return
                property.Type.StartsWith("Bound") &&
                !property.Type.EndsWith("Operator");
        }

        private void GenerateClass(CodeWriter cw, BoundClass boundClass, string packageName) {
            cw.WriteLine(
                "public {0}class {1} extends {2} {",
                boundClass.IsAbstract ? "abstract " : "",
                boundClass.Name,
                boundClass.Base
                );

            cw.Indent();

            foreach (var property in boundClass.Properties) {
                cw.WriteLine("private readonly {0} {1};", GetTypeName(property), GetLocalName(property));
            }

            var parameters = ResolveParameters(boundClass, true);

            cw.WriteLine();
            cw.Write(
                "{0} {1}(",
                boundClass.IsAbstract ? "protected" : "public",
                boundClass.Name
                );

            WriteParameters(cw, parameters);

            cw.WriteLine(") {");

            cw.Indent();

            cw.Write("super(");

            bool hadOne = false;
            foreach (var parameter in parameters) {
                var property = parameter.Property;

                if (boundClass.Properties.IndexOf(property) != -1) {
                    continue;
                }

                if (hadOne) {
                    cw.Write(", ");
                } else {
                    hadOne = true;
                }

                if (parameter.SuperArgument == null) {
                    cw.Write(GetLocalName(property));
                } else {
                    cw.Write(parameter.SuperArgument);
                }
            }

            cw.WriteLine(");");
            cw.WriteLine();

            foreach (var parameter in parameters) {
                if (parameter.SuperArgument != null) {
                    continue;
                }

                var property = parameter.Property;

                if (boundClass.Properties.IndexOf(property) == -1) {
                    continue;
                }

                if (!property.IsNullable && !IsValueType(property.Type)) {
                    cw.WriteLine("Validate.notNull({0}, \"{1}\");", GetLocalName(property), GetLocalName(property));
                }
            }

            if (boundClass.Validation != null) {
                cw.WriteRaw(boundClass.Validation);
            }

            cw.WriteLine();

            foreach (var property in boundClass.Properties) {
                cw.WriteLine("this.{0} = {1};", GetLocalName(property), GetLocalName(property));
            }

            cw.UnIndent();
            cw.WriteLine("}");

            foreach (var property in boundClass.Properties) {
                cw.WriteLine();
                cw.WriteLine("public {0} {1}() {{", GetTypeName(property), GetMethodName(property));
                cw.Indent();
                cw.WriteLine("return {0};", GetLocalName(property));
                cw.UnIndent();
                cw.WriteLine("}");
            }

            if (!boundClass.IsAbstract) {
                cw.WriteLine();
                cw.WriteLine("@Override");
                cw.WriteLine("public BoundKind getKind() {");
                cw.Indent();
                cw.WriteLine("return BoundKind.{0};", GetBoundKind(boundClass));
                cw.UnIndent();
                cw.WriteLine("}");

                cw.WriteLine();
                cw.Write("public {0} update(", boundClass.Name);
                WriteParameters(cw, parameters);
                cw.WriteLine(") {");
                cw.Indent();

                cw.Write("if (");
                hadOne = false;
                foreach (var parameter in parameters) {
                    if (parameter.SuperArgument != null) {
                        continue;
                    }

                    var property = parameter.Property;

                    if (hadOne) {
                        cw.Write(" || ");
                    } else {
                        hadOne = true;
                    }

                    string getter;
                    if (boundClass.Properties.IndexOf(property) == -1) {
                        getter = GetMethodName(property) + "()";
                    } else {
                        getter = "this." + GetLocalName(property);
                    }

                    cw.Write(
                        "{0} != {1}",
                        getter,
                        GetLocalName(property)
                        );
                }
                cw.WriteLine(") {");
                cw.Indent();

                cw.Write("return new {0}(", boundClass.Name);
                hadOne = false;
                foreach (var parameter in parameters) {
                    if (parameter.SuperArgument != null) {
                        continue;
                    }

                    if (hadOne) {
                        cw.Write(", ");
                    } else {
                        hadOne = true;
                    }
                    cw.Write(GetLocalName(parameter.Property));
                }
                cw.WriteLine(");");

                cw.UnIndent();
                cw.WriteLine("}");
                cw.WriteLine();
                cw.WriteLine("return this;");

                cw.UnIndent();
                cw.WriteLine("}");

                cw.WriteLine();
                cw.WriteLine("@Override");
                cw.WriteLine("public void accept(BoundVisitor visitor) {");
                cw.Indent();
                cw.WriteLine("if (!visitor.isDone()) {");
                cw.Indent();
                cw.WriteLine("visitor.visit{0}(this);", GetStrippedName(boundClass));
                cw.UnIndent();
                cw.WriteLine("}");
                cw.UnIndent();
                cw.WriteLine("}");

                cw.WriteLine();
                cw.WriteLine("@Override");
                cw.WriteLine("public <T> T accept(BoundActionVisitor<T> visitor) {");
                cw.Indent();
                cw.WriteLine("return visitor.visit{0}(this);", GetStrippedName(boundClass));
                cw.UnIndent();
                cw.WriteLine("}");
            }

            cw.UnIndent();
            cw.WriteLine("}");
        }

        private void GenerateBoundKind(BoundSchema schema, CodeWriter cw) {
            cw.WriteLine("public enum BoundKind {");
            cw.Indent();

            foreach (var syntaxClass in schema.Classes) {
                if (!syntaxClass.IsAbstract) {
                    cw.WriteLine("{0},", GetBoundKind(syntaxClass));
                }
            }

            cw.UnIndent();
            cw.WriteLine("}");
        }

        private string GetBoundKind(BoundClass boundClass) {
            string name = GetStrippedName(boundClass);

            var sb = new StringBuilder();

            for (int i = 0; i < name.Length; i++) {
                char c = name[i];

                if (i > 0 && Char.IsUpper(c)) {
                    sb.Append('_');
                }
                sb.Append(Char.ToUpper(c));
            }

            return sb.ToString();
        }

        private void WriteParameters(CodeWriter cw, List<Parameter> parameters) {
            bool hadOne = false;

            foreach (var parameter in parameters) {
                if (parameter.SuperArgument != null) {
                    continue;
                }

                var property = parameter.Property;

                if (hadOne) {
                    cw.Write(", ");
                } else {
                    hadOne = true;
                }

                cw.Write("{0} {1}", GetTypeName(property), GetLocalName(property));
            }
        }

        private string GetMethodName(BoundProperty property) {
            string methodName = property.Name;
            if (methodName.StartsWith("Is") || methodName.StartsWith("Has")) {
                methodName = GetLocalName(property);
            } else {
                methodName = "get" + methodName;
            }
            return methodName;
        }

        private static string GetStrippedName(BoundClass boundClass) {
            string prefix = "Bound";
            Debug.Assert(boundClass.Name.StartsWith(prefix));
            return boundClass.Name.Substring(prefix.Length);
        }

        private static bool IsValueType(string type) {
            switch (type) {
                case "bool":
                    return true;
                default:
                    return false;
            }
        }

        private static string GetLocalName(BoundProperty property) {
            string name = property.Name;

            return name.Substring(0, 1).ToLower() + name.Substring(1);
        }

        private static string GetTypeName(BoundProperty property) {
            if (property.IsList) {
                return "ImmutableArray<" + property.Type + ">";
            }

            return property.Type;
        }

        private List<Parameter> ResolveParameters(BoundClass boundClass, bool resolveSuperArguments) {
            List<Parameter> baseParameters;

            if (boundClass.Base != null) {
                baseParameters = ResolveParameters(classes[boundClass.Base], resolveSuperArguments);
            } else {
                baseParameters = new List<Parameter>();
            }

            var parameters = new List<Parameter>();
            bool hadLast = false;

            foreach (var baseParameter in baseParameters) {
                if (!hadLast && baseParameter.Property.IsLast) {
                    hadLast = true;
                    foreach (var property in boundClass.Properties) {
                        parameters.Add(new Parameter(property));
                    }
                }

                if (!resolveSuperArguments || baseParameter.SuperArgument == null) {
                    parameters.Add(baseParameter);
                }
            }

            if (!hadLast) {
                foreach (var property in boundClass.Properties) {
                    parameters.Add(new Parameter(property));
                }
            }

            if (resolveSuperArguments) {
                foreach (var superArgument in boundClass.SuperArgument) {
                    Parameter parameter = null;

                    foreach (var item in parameters) {
                        if (item.Property.Name == superArgument.Name) {
                            parameter = item;
                            break;
                        }
                    }

                    Debug.Assert(parameter != null);

                    parameter.SuperArgument = superArgument.Value;
                }
            }

            return parameters;
        }

        private static void WriteWhenChanged(string file, string content) {
            if (File.Exists(file)) {
                string current = File.ReadAllText(file);
                if (current == content) {
                    return;
                }
            }

            Directory.CreateDirectory(Path.GetDirectoryName(file));

            File.WriteAllText(file, content);
        }

        private class Parameter {
            public Parameter(BoundProperty property) {
                Property = property;
            }

            public BoundProperty Property { get; private set; }
            public string SuperArgument { get; set; }
        }
    }
}
