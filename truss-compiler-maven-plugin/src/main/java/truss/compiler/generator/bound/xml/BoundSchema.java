package truss.compiler.generator.bound.xml;

import javax.xml.bind.annotation.*;
import java.util.ArrayList;
import java.util.List;

@XmlRootElement(name = "schema")
@XmlAccessorType(XmlAccessType.FIELD)
public class BoundSchema {
    @XmlAttribute
    private String packageName;
    @XmlElement(name = "class")
    private List<BoundClass> classes = new ArrayList<>();

    public String getPackageName() {
        return packageName;
    }

    public List<BoundClass> getClasses() {
        return classes;
    }
}
