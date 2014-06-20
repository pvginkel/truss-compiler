using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Truss.Compiler.Symbols {
    public abstract class ContainerSymbol : Symbol {
        private static readonly IList<Symbol> EmptyMembers = new Symbol[0];

        private readonly List<Symbol> _members = new List<Symbol>();
        private readonly Dictionary<string, List<Symbol>> _membersByName = new Dictionary<string, List<Symbol>>();
        private readonly Dictionary<string, List<Symbol>> _membersByMetadataName = new Dictionary<string, List<Symbol>>();

        protected ContainerSymbol(ContainerSymbol parent) {
            if (!(this is GlobalSymbol)) {
                if (parent == null) {
                    throw new ArgumentNullException("parent");
                }
            }

            Members = new ReadOnlyCollection<Symbol>(_members);
            Parent = parent;
        }

        public ContainerSymbol Parent { get; private set; }
        
        public IList<Symbol> Members { get; private set; }

        public void AddMember(Symbol member) {
            if (member == null) {
                throw new ArgumentNullException("member");
            }

            Debug.Assert(!_members.Contains(member));

            _members.Add(member);

            AddMemberByName(member, _membersByName, member.Name);
            AddMemberByName(member, _membersByMetadataName, member.MetadataName);
        }

        private static void AddMemberByName(Symbol member, Dictionary<string, List<Symbol>> membersByName, string name) {
            Debug.Assert(name != null);

            List<Symbol> members;
            if (!membersByName.TryGetValue(name, out members)) {
                members = new List<Symbol>();
                membersByName.Add(name, members);
            }

            members.Add(member);
        }

        public IList<Symbol> GetMemberByName(string name) {
            return GetMemberByName(_membersByName, name);
        }

        public IList<Symbol> GetMemberByMetadataName(string metadataName) {
            return GetMemberByName(_membersByMetadataName, metadataName);
        }

        private static IList<Symbol> GetMemberByName(Dictionary<string, List<Symbol>> membersByName, string name) {
            if (name == null) {
                throw new ArgumentNullException("name");
            }

            List<Symbol> members;
            if (!membersByName.TryGetValue(name, out members)) {
                return EmptyMembers;
            }

            return new ReadOnlyCollection<Symbol>(members);
        }
    }
}
