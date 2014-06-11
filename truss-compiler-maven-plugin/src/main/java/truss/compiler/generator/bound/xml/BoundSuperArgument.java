package truss.compiler.generator.bound.xml;

import javax.xml.bind.annotation.*;

@XmlRootElement(name = "superArgument")
@XmlAccessorType(XmlAccessType.FIELD)
public class BoundSuperArgument {
    @XmlAttribute
    private String name;
    @XmlValue
    private String value;

    public String getName() {
        return name;
    }

    public String getValue() {
        return value;
    }
}
