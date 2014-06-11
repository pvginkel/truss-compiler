package truss.compiler.generator.bound.xml;

import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlAttribute;
import javax.xml.bind.annotation.XmlRootElement;

@XmlRootElement(name = "property")
@XmlAccessorType(XmlAccessType.FIELD)
public class BoundProperty {
    @XmlAttribute
    private String name;
    @XmlAttribute
    private String type;
    @XmlAttribute
    private boolean list;
    @XmlAttribute
    private boolean nullable;
    @XmlAttribute
    private boolean last;

    public String getName() {
        return name;
    }

    public String getType() {
        return type;
    }

    public boolean isList() {
        return list;
    }

    public boolean isNullable() {
        return nullable;
    }

    public boolean isLast() {
        return last;
    }
}
