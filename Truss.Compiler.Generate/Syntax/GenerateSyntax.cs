using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Truss.Compiler.Generate.Syntax.Xml;

namespace Truss.Compiler.Generate.Syntax {
    public class GenerateSyntax {
        private readonly Dictionary<string, SyntaxClass> _classes = new Dictionary<string, SyntaxClass>();

        public void Generate(string outputDirectory) {
            var schema = XmlSerialization.Deserialize<SyntaxSchema>(
                Path.Combine(outputDirectory, "Syntax", "SyntaxSchema.xml")
                );

            schema.Classes.Sort((a, b) => a.Name.CompareTo(b.Name));

            foreach (var syntaxClass in schema.Classes) {
                _classes.Add(syntaxClass.Name, syntaxClass);
            }

            var cw = new CodeWriter();

            cw.WriteLine("using System;");
            cw.WriteLine("using System.Collections.Generic;");
            cw.WriteLine("using System.Linq;");
            cw.WriteLine("using System.Text;");
            cw.WriteLine("using Truss.Compiler.Support;");

            cw.WriteLine();
            cw.WriteLine("namespace {0} {{", schema.PackageName);

            cw.Indent();

            foreach (var syntaxClass in schema.Classes) {
                if (!syntaxClass.IsIgnore) {
                    GenerateClass(cw, syntaxClass, schema);
                    cw.WriteLine();
                }
            }

            foreach (var syntaxEnum in schema.Enums) {
                GenerateEnum(cw, syntaxEnum);
                cw.WriteLine();
            }

            GenerateVisitor(schema, cw);
            cw.WriteLine();
            GenerateActionVisitor(schema, cw);
            cw.WriteLine();
            GenerateAbstractVisitor(schema, cw);
            cw.WriteLine();
            GenerateAbstractActionVisitor(schema, cw);
            cw.WriteLine();
            GenerateWalker(schema, cw);
            cw.WriteLine();
            GenerateSyntaxKind(schema, cw);

            cw.UnIndent();

            cw.WriteLine("}");

            WriteWhenChanged(Path.Combine(outputDirectory, "Syntax", "SyntaxSchema.Generated.cs"), cw.ToString());
        }

        private void GenerateVisitor(SyntaxSchema schema, CodeWriter cw) {
            cw.WriteLine("public interface ISyntaxVisitor {");
            cw.Indent();

            cw.WriteLine("bool Done { get; }");

            foreach (var syntaxClass in schema.Classes) {
                if (syntaxClass.IsAbstract) {
                    continue;
                }

                cw.WriteLine();
                cw.WriteLine("void Visit{0}({1} syntax);", GetStrippedName(syntaxClass), syntaxClass.Name);
            }

            cw.UnIndent();
            cw.WriteLine("}");
        }

        private void GenerateActionVisitor(SyntaxSchema schema, CodeWriter cw) {
            cw.WriteLine("public interface ISyntaxVisitor<T> {");
            cw.Indent();

            bool hadOne = false;

            foreach (var syntaxClass in schema.Classes) {
                if (syntaxClass.IsAbstract) {
                    continue;
                }

                if (hadOne) {
                    cw.WriteLine();
                } else {
                    hadOne = true;
                }
                cw.WriteLine("T Visit{0}({1} syntax);", GetStrippedName(syntaxClass), syntaxClass.Name);
            }

            cw.UnIndent();
            cw.WriteLine("}");
        }

        private void GenerateAbstractVisitor(SyntaxSchema schema, CodeWriter cw) {
            cw.WriteLine("public class AbstractSyntaxVisitor : ISyntaxVisitor {");
            cw.Indent();

            cw.WriteLine("public bool Done { get; set; }");
            cw.WriteLine();
            cw.WriteLine("public void DefaultVisit(SyntaxNode syntax) {");
            cw.WriteLine("}");

            foreach (var syntaxClass in schema.Classes) {
                if (syntaxClass.IsAbstract) {
                    continue;
                }

                cw.WriteLine();
                cw.WriteLine("public void Visit{0}({1} syntax) {{", GetStrippedName(syntaxClass), syntaxClass.Name);
                cw.Indent();
                cw.WriteLine("DefaultVisit(syntax);");
                cw.UnIndent();
                cw.WriteLine("}");
            }

            cw.UnIndent();
            cw.WriteLine("}");
        }

        private void GenerateAbstractActionVisitor(SyntaxSchema schema, CodeWriter cw) {
            cw.WriteLine("public class AbstractSyntaxVisitor<T> : ISyntaxVisitor<T> {");
            cw.Indent();

            cw.WriteLine("public T DefaultVisit(SyntaxNode syntax) {");
            cw.Indent();
            cw.WriteLine("return default(T);");
            cw.UnIndent();
            cw.WriteLine("}");

            bool hadOne = false;

            foreach (var syntaxClass in schema.Classes) {
                if (syntaxClass.IsAbstract) {
                    continue;
                }

                if (hadOne) {
                    cw.WriteLine();
                } else {
                    hadOne = true;
                }
                cw.WriteLine("public T Visit{0}({1} syntax) {{", GetStrippedName(syntaxClass), syntaxClass.Name);
                cw.Indent();
                cw.WriteLine("return DefaultVisit(syntax);");
                cw.UnIndent();
                cw.WriteLine("}");
            }

            cw.UnIndent();
            cw.WriteLine("}");
        }

        private void GenerateWalker(SyntaxSchema schema, CodeWriter cw) {
            cw.WriteLine("public class SyntaxTreeWalker : ISyntaxVisitor {");
            cw.Indent();

            cw.WriteLine("public bool Done { get; set; }");
            cw.WriteLine();
            cw.WriteLine("public virtual void VisitList<T>(ImmutableArray<T> list)");
            cw.Indent();
            cw.WriteLine("where T : SyntaxNode {");
            cw.WriteLine("foreach (var node in list) {");
            cw.Indent();
            cw.WriteLine("Visit(node);");
            cw.UnIndent();
            cw.WriteLine("}");
            cw.UnIndent();
            cw.WriteLine("}");
            cw.WriteLine();
            cw.WriteLine("public virtual void Visit(SyntaxNode syntax) {");
            cw.Indent();
            cw.WriteLine("if (syntax != null) {");
            cw.Indent();
            cw.WriteLine("syntax.Accept(this);");
            cw.UnIndent();
            cw.WriteLine("}");
            cw.UnIndent();
            cw.WriteLine("}");

            foreach (var syntaxClass in schema.Classes) {
                if (syntaxClass.IsAbstract) {
                    continue;
                }

                cw.WriteLine();
                cw.WriteLine("public virtual void Visit{0}({1} syntax) {{", GetStrippedName(syntaxClass), syntaxClass.Name);
                cw.Indent();

                foreach (var property in ResolveParameters(syntaxClass)) {
                    if (!IsSyntaxNode(property)) {
                        continue;
                    }

                    if (property.IsList) {
                        cw.WriteLine("VisitList(syntax.{0});", property.Name);
                    } else {
                        cw.WriteLine("Visit(syntax.{0});", property.Name);
                    }
                }

                cw.UnIndent();
                cw.WriteLine("}");
            }

            cw.UnIndent();
            cw.WriteLine("}");
        }

        private void GenerateSyntaxKind(SyntaxSchema schema, CodeWriter cw) {
            cw.WriteLine("public enum SyntaxKind {");
            cw.Indent();

            foreach (var syntaxClass in schema.Classes) {
                if (!syntaxClass.IsAbstract) {
                    cw.WriteLine("{0},", GetStrippedName(syntaxClass));
                }
            }

            cw.UnIndent();
            cw.WriteLine("}");
        }

        private static bool IsSyntaxNode(SyntaxProperty property) {
            return property.Type.EndsWith("Syntax");
        }

        private void GenerateClass(CodeWriter cw, SyntaxClass schemaClass, SyntaxSchema schema) {
            cw.WriteLine(
                "public {0}class {1} : {2} {{",
                schemaClass.IsAbstract ? "abstract " : "",
                schemaClass.Name,
                schemaClass.Base
                );

            cw.Indent();

            var parameters = ResolveParameters(schemaClass);

            cw.Write(
                "{0} {1}(",
                schemaClass.IsAbstract ? "protected" : "public",
                schemaClass.Name
            );

            WriteParameters(cw, parameters, schema);

            cw.WriteLine(")");

            cw.Indent();

            cw.Write(": base(");

            bool hadOne = false;
            foreach (var parameter in parameters) {
                if (schemaClass.Properties.IndexOf(parameter) != -1) {
                    continue;
                }

                if (hadOne) {
                    cw.Write(", ");
                } else {
                    hadOne = true;
                }

                cw.Write(GetLocalName(parameter));
            }

            cw.WriteLine(") {");

            foreach (var parameter in parameters) {
                if (schemaClass.Properties.IndexOf(parameter) == -1) {
                    continue;
                }

                if (!parameter.IsNullable && !IsValueType(parameter.Type, schema)) {
                    cw.WriteLine("if ({0} == null) {{", GetLocalName(parameter));
                    cw.Indent();
                    cw.WriteLine("throw new ArgumentNullException(\"{0}\");", GetLocalName(parameter));
                    cw.UnIndent();
                    cw.WriteLine("}");
                }
            }

            if (schemaClass.Validation != null) {
                cw.WriteRaw(schemaClass.Validation);
                cw.WriteRaw(Environment.NewLine);
            }

            cw.WriteLine();

            foreach (var property in schemaClass.Properties) {
                cw.WriteLine("{0} = {1};", property.Name, GetLocalName(property));
            }

            cw.UnIndent();
            cw.WriteLine("}");

            foreach (var property in schemaClass.Properties) {
                cw.WriteLine();
                cw.WriteLine("public {0} {1} {{ get; private set; }}", GetTypeName(property, schema), property.Name);
            }

            if (!schemaClass.IsAbstract) {
                cw.WriteLine();
                cw.WriteLine("public override SyntaxKind Kind {");
                cw.Indent();
                cw.WriteLine("get {{ return SyntaxKind.{0}; }}", GetStrippedName(schemaClass));
                cw.UnIndent();
                cw.WriteLine("}");

                cw.WriteLine();
                cw.WriteLine("public override void Accept(ISyntaxVisitor visitor) {");
                cw.Indent();
                cw.WriteLine("if (!visitor.Done) {");
                cw.Indent();
                cw.WriteLine("visitor.Visit{0}(this);", GetStrippedName(schemaClass));
                cw.UnIndent();
                cw.WriteLine("}");
                cw.UnIndent();
                cw.WriteLine("}");

                cw.WriteLine();
                cw.WriteLine("public override T Accept<T>(ISyntaxVisitor<T> visitor) {");
                cw.Indent();
                cw.WriteLine("return visitor.Visit{0}(this);", GetStrippedName(schemaClass));
                cw.UnIndent();
                cw.WriteLine("}");
            }

            if (schemaClass.Members != null) {
                cw.WriteRaw(schemaClass.Members);
                cw.WriteRaw(Environment.NewLine);
            }

            cw.UnIndent();
            cw.WriteLine("}");
        }

        private void GenerateEnum(CodeWriter cw, SyntaxEnum schemaEnum) {
            cw.WriteLine(
                "public enum {0} {{",
                schemaEnum.Name
                );

            cw.Indent();

            foreach (var property in schemaEnum.Properties) {
                cw.WriteLine("{0},", property.Name);
            }

            cw.UnIndent();
            cw.WriteLine("}");
        }

        private static void WriteParameters(CodeWriter cw, List<SyntaxProperty> parameters, SyntaxSchema schema) {
            bool hadOne = false;

            foreach (var parameter in parameters) {
                if (hadOne) {
                    cw.Write(", ");
                } else {
                    hadOne = true;
                }

                cw.Write("{0} {1}", GetTypeName(parameter, schema), GetLocalName(parameter));
            }
        }

        private static string GetStrippedName(SyntaxClass syntaxClass) {
            string postfix = "Syntax";
            Debug.Assert(syntaxClass.Name.EndsWith(postfix));
            return syntaxClass.Name.Substring(0, syntaxClass.Name.Length - postfix.Length);
        }

        private static bool IsValueType(string type, SyntaxSchema schema) {
            return type == "bool" || schema.Enums.Any(p => p.Name == type);
        }

        private static string GetLocalName(SyntaxProperty property) {
            string name = property.Name;

            name = name.Substring(0, 1).ToLower() + name.Substring(1);

            switch (name) {
                case "finally":
                case "operator":
                case "else":
                case "default":
                    name = name + "_";
                    break;
            }

            return name;
        }

        private static string GetTypeName(SyntaxProperty property, SyntaxSchema schema) {
            if (property.IsNullable && schema.Enums.Any(p => p.Name == property.Type)) {
                return property.Type + "?";
            }
            if (property.IsList) {
                return "ImmutableArray<" + property.Type + ">";
            }

            return property.Type;
        }

        private List<SyntaxProperty> ResolveParameters(SyntaxClass syntaxClass) {
            List<SyntaxProperty> baseParameters;

            if (syntaxClass.Base != null) {
                baseParameters = ResolveParameters(_classes[syntaxClass.Base]);
            } else {
                baseParameters = new List<SyntaxProperty>();
            }

            var parameters = new List<SyntaxProperty>();
            bool hadLast = false;

            foreach (var baseParameter in baseParameters) {
                if (!hadLast && baseParameter.IsLast) {
                    hadLast = true;
                    parameters.AddRange(syntaxClass.Properties);
                }

                parameters.Add(baseParameter);
            }

            if (!hadLast) {
                parameters.AddRange(syntaxClass.Properties);
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
    }
}
