using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Truss.Compiler.Generate.Xml;
using Enum = Truss.Compiler.Generate.Xml.Enum;

namespace Truss.Compiler.Generate {
    internal class Generator {
        private readonly string _outputDirectory;
        private readonly GeneratorMode _mode;
        private readonly Dictionary<string, Class> _classes = new Dictionary<string, Class>();
        private readonly string _modeLocal;

        public Generator(string outputDirectory, GeneratorMode mode) {
            _outputDirectory = outputDirectory;
            _mode = mode;
            _modeLocal = _mode == GeneratorMode.Syntax ? "syntax" : "node";
        }

        public void Generate() {
            var schema = XmlSerialization.Deserialize<Schema>(
                Path.Combine(_outputDirectory, _mode.ToString(), _mode + "Schema.xml")
            );

            schema.Classes.Sort((a, b) => a.Name.CompareTo(b.Name));

            foreach (var @class in schema.Classes) {
                _classes.Add(@class.Name, @class);
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

            foreach (var @class in schema.Classes) {
                if (!@class.IsIgnore) {
                    GenerateClass(cw, @class, schema);
                    cw.WriteLine();
                }
            }

            foreach (var @enum in schema.Enums) {
                GenerateEnum(cw, @enum);
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
            if (_mode == GeneratorMode.Bound) {
                cw.WriteLine();
                GenerateRewriter(schema, cw);
            }
            cw.WriteLine();
            GenerateKind(schema, cw);

            cw.UnIndent();

            cw.WriteLine("}");

            WriteWhenChanged(Path.Combine(_outputDirectory, _mode.ToString(), _mode + "Schema.Generated.cs"), cw.ToString());
        }

        private void GenerateVisitor(Schema schema, CodeWriter cw) {
            cw.WriteLine("public interface I{0}Visitor {{", _mode);
            cw.Indent();

            cw.WriteLine("bool Done { get; }");

            foreach (var @class in schema.Classes) {
                if (@class.IsAbstract) {
                    continue;
                }

                cw.WriteLine();
                cw.WriteLine("void Visit{0}({1} {2});", GetStrippedName(@class), @class.Name, _modeLocal);
            }

            cw.UnIndent();
            cw.WriteLine("}");
        }

        private void GenerateActionVisitor(Schema schema, CodeWriter cw) {
            cw.WriteLine("public interface I{0}Visitor<T> {{", _mode);
            cw.Indent();

            bool hadOne = false;

            foreach (var @class in schema.Classes) {
                if (@class.IsAbstract) {
                    continue;
                }

                if (hadOne) {
                    cw.WriteLine();
                } else {
                    hadOne = true;
                }
                cw.WriteLine("T Visit{0}({1} {2});", GetStrippedName(@class), @class.Name, _modeLocal);
            }

            cw.UnIndent();
            cw.WriteLine("}");
        }

        private void GenerateAbstractVisitor(Schema schema, CodeWriter cw) {
            cw.WriteLine("public class Abstract{0}Visitor : I{0}Visitor {{", _mode);
            cw.Indent();

            cw.WriteLine("public bool Done { get; set; }");
            cw.WriteLine();
            cw.WriteLine("public void DefaultVisit({0}Node {1}) {{", _mode, _modeLocal);
            cw.WriteLine("}");

            foreach (var @class in schema.Classes) {
                if (@class.IsAbstract) {
                    continue;
                }

                cw.WriteLine();
                cw.WriteLine("public void Visit{0}({1} {2}) {{", GetStrippedName(@class), @class.Name, _modeLocal);
                cw.Indent();
                cw.WriteLine("DefaultVisit({0});", _modeLocal);
                cw.UnIndent();
                cw.WriteLine("}");
            }

            cw.UnIndent();
            cw.WriteLine("}");
        }

        private void GenerateAbstractActionVisitor(Schema schema, CodeWriter cw) {
            cw.WriteLine("public class Abstract{0}Visitor<T> : I{0}Visitor<T> {{", _mode);
            cw.Indent();

            cw.WriteLine("public T DefaultVisit({0}Node {1}) {{", _mode, _modeLocal);
            cw.Indent();
            cw.WriteLine("return default(T);");
            cw.UnIndent();
            cw.WriteLine("}");

            bool hadOne = false;

            foreach (var @class in schema.Classes) {
                if (@class.IsAbstract) {
                    continue;
                }

                if (hadOne) {
                    cw.WriteLine();
                } else {
                    hadOne = true;
                }
                cw.WriteLine("public T Visit{0}({1} {2}) {{", GetStrippedName(@class), @class.Name, _modeLocal);
                cw.Indent();
                cw.WriteLine("return DefaultVisit({0});", _modeLocal);
                cw.UnIndent();
                cw.WriteLine("}");
            }

            cw.UnIndent();
            cw.WriteLine("}");
        }

        private void GenerateWalker(Schema schema, CodeWriter cw) {
            cw.WriteLine("public class {0}TreeWalker : I{0}Visitor {{", _mode);
            cw.Indent();

            cw.WriteLine("public bool Done { get; set; }");
            cw.WriteLine();
            cw.WriteLine("public virtual void VisitList<T>(ImmutableArray<T> list)");
            cw.Indent();
            cw.WriteLine("where T : {0}Node {{", _mode);
            cw.WriteLine("foreach (var node in list) {");
            cw.Indent();
            cw.WriteLine("Visit(node);");
            cw.UnIndent();
            cw.WriteLine("}");
            cw.UnIndent();
            cw.WriteLine("}");
            cw.WriteLine();
            cw.WriteLine("public virtual void Visit({0}Node {1}) {{", _mode, _modeLocal);
            cw.Indent();
            cw.WriteLine("if ({0} != null) {{", _modeLocal);
            cw.Indent();
            cw.WriteLine("{0}.Accept(this);", _modeLocal);
            cw.UnIndent();
            cw.WriteLine("}");
            cw.UnIndent();
            cw.WriteLine("}");

            foreach (var @class in schema.Classes) {
                if (@class.IsAbstract) {
                    continue;
                }

                cw.WriteLine();
                cw.WriteLine("public virtual void Visit{0}({1} {2}) {{", GetStrippedName(@class), @class.Name, _modeLocal);
                cw.Indent();

                foreach (var parameter in ResolveParameters(@class, false)) {
                    if (!IsNode(parameter.Property)) {
                        continue;
                    }

                    if (parameter.Property.IsList) {
                        cw.WriteLine("VisitList({0}.{1});", _modeLocal, parameter.Property.Name);
                    } else {
                        cw.WriteLine("Visit({0}.{1});", _modeLocal, parameter.Property.Name);
                    }
                }

                cw.UnIndent();
                cw.WriteLine("}");
            }

            cw.UnIndent();
            cw.WriteLine("}");
        }

        private void GenerateKind(Schema schema, CodeWriter cw) {
            cw.WriteLine("public enum {0}Kind {{", _mode);
            cw.Indent();

            foreach (var @class in schema.Classes) {
                if (!@class.IsAbstract) {
                    cw.WriteLine("{0},", GetStrippedName(@class));
                }
            }

            cw.UnIndent();
            cw.WriteLine("}");
        }

        private bool IsNode(Property property) {
            if (_mode == GeneratorMode.Syntax) {
                return property.Type.EndsWith(_mode.ToString());
            }

            return property.Type.StartsWith(_mode.ToString());
        }

        private void GenerateClass(CodeWriter cw, Class schemaClass, Schema schema) {
            cw.WriteLine(
                "public {0}class {1} : {2} {{",
                schemaClass.IsAbstract ? "abstract " : "",
                schemaClass.Name,
                schemaClass.Base
                );

            cw.Indent();

            var parameters = ResolveParameters(schemaClass, true);

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
                if (schemaClass.Properties.IndexOf(parameter.Property) != -1) {
                    continue;
                }

                if (hadOne) {
                    cw.Write(", ");
                } else {
                    hadOne = true;
                }

                cw.Write(GetLocalName(parameter.Property));
            }

            cw.WriteLine(") {");

            foreach (var parameter in parameters) {
                var property = parameter.Property;

                if (schemaClass.Properties.IndexOf(property) == -1) {
                    continue;
                }

                if (!property.IsNullable && !IsValueType(property.Type, schema)) {
                    cw.WriteLine("if ({0} == null) {{", GetLocalName(property));
                    cw.Indent();
                    cw.WriteLine("throw new ArgumentNullException(\"{0}\");", GetLocalName(property));
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
                cw.WriteLine("public override {0}Kind Kind {{", _mode);
                cw.Indent();
                cw.WriteLine("get {{ return {0}Kind.{1}; }}", _mode, GetStrippedName(schemaClass));
                cw.UnIndent();
                cw.WriteLine("}");

                if (_mode == GeneratorMode.Bound) {
                    cw.WriteLine();
                    cw.Write("public {0} Update(", schemaClass.Name);
                    WriteParameters(cw, parameters, schema);
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

                        cw.Write(
                            "{0} != {1}",
                            property.Name,
                            GetLocalName(property)
                        );
                    }
                    cw.WriteLine(") {");
                    cw.Indent();

                    cw.Write("return new {0}(", schemaClass.Name);
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
                }

                cw.WriteLine();
                cw.WriteLine("public override void Accept(I{0}Visitor visitor) {{", _mode);
                cw.Indent();
                cw.WriteLine("if (!visitor.Done) {");
                cw.Indent();
                cw.WriteLine("visitor.Visit{0}(this);", GetStrippedName(schemaClass));
                cw.UnIndent();
                cw.WriteLine("}");
                cw.UnIndent();
                cw.WriteLine("}");

                cw.WriteLine();
                cw.WriteLine("public override T Accept<T>(I{0}Visitor<T> visitor) {{", _mode);
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

        private void GenerateEnum(CodeWriter cw, Enum schemaEnum) {
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

        private static void WriteParameters(CodeWriter cw, List<Parameter> parameters, Schema schema) {
            bool hadOne = false;

            foreach (var parameter in parameters) {
                if (hadOne) {
                    cw.Write(", ");
                } else {
                    hadOne = true;
                }

                cw.Write("{0} {1}", GetTypeName(parameter.Property, schema), GetLocalName(parameter.Property));
            }
        }

        private string GetStrippedName(Class @class) {
            string postfix = _mode.ToString();
            if (_mode == GeneratorMode.Syntax) {
                Debug.Assert(@class.Name.EndsWith(postfix));
                return @class.Name.Substring(0, @class.Name.Length - postfix.Length);
            } else {
                Debug.Assert(@class.Name.StartsWith(postfix));
                return @class.Name.Substring(postfix.Length);
            }
        }

        private static bool IsValueType(string type, Schema schema) {
            return type == "bool" || schema.Enums.Any(p => p.Name == type);
        }

        private static string GetLocalName(Property property) {
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

        private static string GetTypeName(Property property, Schema schema) {
            if (property.IsNullable && schema.Enums.Any(p => p.Name == property.Type)) {
                return property.Type + "?";
            }
            if (property.IsList) {
                return "ImmutableArray<" + property.Type + ">";
            }

            return property.Type;
        }

        private List<Parameter> ResolveParameters(Class @class, bool resolveSuperArguments) {
            List<Parameter> baseParameters;

            if (@class.Base != null) {
                baseParameters = ResolveParameters(_classes[@class.Base], resolveSuperArguments);
            } else {
                baseParameters = new List<Parameter>();
            }

            var parameters = new List<Parameter>();
            bool hadLast = false;

            foreach (var baseParameter in baseParameters) {
                if (!hadLast && baseParameter.Property.IsLast) {
                    hadLast = true;
                    foreach (var property in @class.Properties) {
                        parameters.Add(new Parameter(property));
                    }
                }

                if (!resolveSuperArguments || baseParameter.SuperArgument == null) {
                    parameters.Add(baseParameter);
                }
            }

            if (!hadLast) {
                foreach (var property in @class.Properties) {
                    parameters.Add(new Parameter(property));
                }
            }

            if (resolveSuperArguments) {
                foreach (var superArgument in @class.SuperArguments) {
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

        //private List<Property> ResolveParameters(Class @class, bool resolveSuperArguments) {
        //    List<Property> baseParameters;

        //    if (@class.Base != null) {
        //        baseParameters = ResolveParameters(_classes[@class.Base], resolveSuperArguments);
        //    } else {
        //        baseParameters = new List<Property>();
        //    }

        //    var parameters = new List<Property>();
        //    bool hadLast = false;

        //    foreach (var baseParameter in baseParameters) {
        //        if (!hadLast && baseParameter.IsLast) {
        //            hadLast = true;
        //            parameters.AddRange(@class.Properties);
        //        }

        //        parameters.Add(baseParameter);
        //    }

        //    if (!hadLast) {
        //        parameters.AddRange(@class.Properties);
        //    }

        //    return parameters;
        //}

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

        private void GenerateRewriter(Schema schema, CodeWriter cw) {
            cw.WriteLine("public class {0}TreeRewriter : I{0}Visitor<{0}Node> {{", _mode);
            cw.Indent();

            cw.WriteLine("public ImmutableArray<T> VisitList<T>(ImmutableArray<T> nodes)");
            cw.Indent();
            cw.WriteLine("where T : {0}Node {{", _mode);

            cw.WriteLine("if (nodes == null) {");
            cw.Indent();
            cw.WriteLine("throw new ArgumentNullException(\"nodes\");");
            cw.UnIndent();
            cw.WriteLine("}");
            cw.WriteLine();

            cw.WriteLine("if (nodes.Count == 0) {");
            cw.Indent();

            cw.WriteLine("return nodes;");

            cw.UnIndent();
            cw.WriteLine("}");
            cw.WriteLine();
            cw.WriteLine("ImmutableArray<T>.Builder result = null;");
            cw.WriteLine();
            cw.WriteLine("for (int i = 0; i < nodes.Count; i++) {");
            cw.Indent();

            cw.WriteLine("var item = nodes[i];");
            cw.WriteLine("var visited = (T)item.Accept(this);");
            cw.WriteLine();
            cw.WriteLine("if (item != visited && result == null) {");
            cw.Indent();

            cw.WriteLine("result = new ImmutableArray<T>.Builder();");
            cw.WriteLine("for (int j = 0; j < i; j++) {");
            cw.Indent();

            cw.WriteLine("result.Add(nodes[j]);");

            cw.UnIndent();
            cw.WriteLine("}");

            cw.UnIndent();
            cw.WriteLine("}");
            cw.WriteLine();
            cw.WriteLine("if (result != null && visited != null) {");
            cw.Indent();

            cw.WriteLine("result.Add(visited);");

            cw.UnIndent();
            cw.WriteLine("}");

            cw.UnIndent();
            cw.WriteLine("}");
            cw.WriteLine();
            cw.WriteLine("if (result != null) {");
            cw.Indent();

            cw.WriteLine("return result.Build();");

            cw.UnIndent();
            cw.WriteLine("}");
            cw.WriteLine();
            cw.WriteLine("return nodes;");

            cw.UnIndent();
            cw.WriteLine("}");

            cw.WriteLine();
            cw.WriteLine("public T Visit<T>(T {0})", _modeLocal);
            cw.Indent();
            cw.WriteLine("where T : {0}Node {{", _mode);

            cw.WriteLine("if ({0} != null) {{", _modeLocal);
            cw.Indent();
            cw.WriteLine("return (T){0}.Accept(this);", _modeLocal);
            cw.UnIndent();
            cw.WriteLine("}");
            cw.WriteLine();
            cw.WriteLine("return null;");
            cw.UnIndent();
            cw.WriteLine("}");

            foreach (var @class in schema.Classes) {
                if (@class.IsAbstract) {
                    continue;
                }

                cw.WriteLine();
                cw.WriteLine("public {0}Node Visit{1}({2} {3}) {{", _mode, GetStrippedName(@class), @class.Name, _modeLocal);
                cw.Indent();

                var parameters = ResolveParameters(@class, true);

                foreach (var parameter in parameters) {
                    var property = parameter.Property;

                    if (!IsNode(property) || parameter.SuperArgument != null) {
                        continue;
                    }

                    if (property.IsList) {
                        cw.WriteLine(
                            "{0} {1} = VisitList({2}.{3});",
                            GetTypeName(property, schema),
                            GetLocalName(property),
                            _modeLocal,
                            property.Name
                        );
                    } else {
                        cw.WriteLine(
                            "{0} {1} = Visit({2}.{3});",
                            GetTypeName(property, schema),
                            GetLocalName(property),
                            _modeLocal,
                            property.Name
                        );
                    }
                }

                cw.Write("return {0}.Update(", _modeLocal);

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
                    if (IsNode(property)) {
                        cw.Write(GetLocalName(property));
                    } else {
                        cw.Write("{0}.{1}", _modeLocal, property.Name);
                    }
                }

                cw.WriteLine(");");

                cw.UnIndent();
                cw.WriteLine("}");
            }

            cw.UnIndent();
            cw.WriteLine("}");
        }

        private class Parameter {
            public Parameter(Property property) {
                Property = property;
            }

            public Property Property { get; private set; }
            public string SuperArgument { get; set; }
        }
    }
}
