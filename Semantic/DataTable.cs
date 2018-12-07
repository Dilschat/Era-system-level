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
        public string name;

        public DataTableEntry(AstNode node)
        {
            name = (string) ((AstNode) node.getValue()).getValue();
        }
    }
}