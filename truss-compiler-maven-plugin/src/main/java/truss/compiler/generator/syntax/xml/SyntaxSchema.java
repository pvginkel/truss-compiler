package truss.compiler.generator.syntax.xml;

import javax.xml.bind.annotation.*;
import java.util.ArrayList;
import java.util.List;

@XmlRootElement(name = "schema")
@XmlAccessorType(XmlAccessType.FIELD)
public class SyntaxSchema {
    @XmlAttribute
    private String packageName;
    @XmlElement(name = "class")
    private List<SyntaxClass> classes = new ArrayList<>();
    @XmlElement(name = "enum")
    private List<SyntaxEnum> enums = new ArrayList<>();

    public String getPackageName() {
        return packageName;
    }

    public List<SyntaxClass> getClasses() {
        return classes;
    }

    public List<SyntaxEnum> getEnums() {
        return enums;
    }
}
