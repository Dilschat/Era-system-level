using System.Collections.Generic;
using Erasystemlevel.Exception;
using Erasystemlevel.Parser;

namespace Erasystemlevel.Semantic
{
    public class DataTable : Dictionary<string, DataTableEntry>
    {
        public void Add(AstNode node)
        {
            if (node.GetNodeType() != AstNode.NodeType.Data)
            {
                throw new SemanticError("Data node should be supplied");
            }

            var entry = new DataTableEntry(node);

            Add(entry.name, entry);
        }

        public HashSet<string> getNames()
        {
            return new HashSet<string>(Keys);
        }
    }

    public class DataTableEntry
    {
        public AstNode node;
        public readonly string name;

        public int baseAddr;

        public DataTableEntry(AstNode node)
        {
            this.node = node;
            name = node.getValue().ToString();
        }
    }
}