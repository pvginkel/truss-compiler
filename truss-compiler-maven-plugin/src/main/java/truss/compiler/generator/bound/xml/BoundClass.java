package truss.compiler.generator.bound.xml;

import javax.xml.bind.annotation.*;
import java.util.ArrayList;
import java.util.List;

@XmlRootElement(name = "class")
@XmlAccessorType(XmlAccessType.FIELD)
public class BoundClass {
    @XmlAttribute
    private String name;
    @XmlAttribute
    private String base;
    @XmlAttribute(name = "abstract")
    private boolean abstract_;
    @XmlAttribute
    private boolean ignore;
    @XmlElement(name = "property")
    private List<BoundProperty> properties = new ArrayList<>();
    @XmlElement(name = "superArgument")
    private List<BoundSuperArgument> superArguments = new ArrayList<>();
    private String validation;

    public String getName() {
        return name;
    }

    public String getBase() {
        return base;
    }

    public boolean isAbstract() {
        return abstract_;
    }

    public boolean isIgnore() {
        return ignore;
    }

    public List<BoundProperty> getProperties() {
        return properties;
    }

    public List<BoundSuperArgument> getSuperArguments() {
        return superArguments;
    }

    public String getValidation() {
        return validation;
    }
}
