package truss.compiler.generator.syntax.xml;

import javax.xml.bind.annotation.*;
import java.util.ArrayList;
import java.util.List;

@XmlRootElement(name = "enum")
@XmlAccessorType(XmlAccessType.FIELD)
public class SyntaxEnum {
    @XmlAttribute
    private String name;
    @XmlElement(name = "property")
    private List<SyntaxProperty> properties = new ArrayList<>();

    public String getName() {
        return name;
    }

    public List<SyntaxProperty> getProperties() {
        return properties;
    }
}
