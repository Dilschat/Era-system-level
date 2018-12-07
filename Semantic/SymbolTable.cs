using System.Collections.Generic;
using Erasystemlevel.Parser;

namespace Erasystemlevel.Semantic
{
    public class SymbolTable : Dictionary<string, SymbolTableEntry>
    {
    }

    public class SymbolTableEntry
    {
        public int localId;
        public AstNode node;

        public readonly string name;
        public string type;

        public bool isConst = false;
        public bool isArray = false;

        public bool isInitialized = false;

        public SymbolTableEntry(AstNode node)
        {
            // todo: extend to more data
            this.node = node;
            name = node.getChilds()[0].getValue().ToString();
        }

        public override string ToString()
        {
            return name;
        }
    }
}