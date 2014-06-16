package truss.compiler.symbols;

import org.apache.commons.lang.Validate;

import java.util.*;

public abstract class ContainerSymbol extends Symbol {
    private final ContainerSymbol parent;
    private final List<Symbol> members = new ArrayList<>();
    private final List<Symbol> unmodifiableMembers = Collections.unmodifiableList(members);
    private final Map<String, List<Symbol>> membersByName = new HashMap<>();
    private final Map<String, List<Symbol>> membersByMetadataName = new HashMap<>();

    protected ContainerSymbol(ContainerSymbol parent) {
        if (!(this instanceof GlobalSymbol)) {
            Validate.notNull(parent, "parent");
        }

        this.parent = parent;
    }

    public ContainerSymbol getParent() {
        return parent;
    }

    public void addMember(Symbol member) {
        Validate.notNull(member, "member");

        assert !members.contains(member);

        members.add(member);

        addMemberByName(member, membersByName, member.getName());
        addMemberByName(member, membersByMetadataName, member.getMetadataName());
    }

    private void addMemberByName(Symbol member, Map<String, List<Symbol>> membersByName, String name) {
        assert name != null;

        List<Symbol> members = membersByName.get(name);
        if (members == null) {
            members = new ArrayList<>();
            membersByName.put(name, members);
        }

        members.add(member);
    }

    public List<Symbol> getMemberByName(String name) {
        return getMemberByName(membersByName, name);
    }

    public List<Symbol> getMemberByMetadataName(String metadataName) {
        return getMemberByName(membersByMetadataName, metadataName);
    }

    private List<Symbol> getMemberByName(Map<String, List<Symbol>> membersByName, String name) {
        Validate.notNull(name, "name");

        List<Symbol> members = membersByName.get(name);
        if (members == null) {
            return Collections.emptyList();
        }

        return Collections.unmodifiableList(members);
    }

    public List<Symbol> getMembers() {
        return unmodifiableMembers;
    }
}
