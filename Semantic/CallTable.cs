using System.Collections;
using System.Collections.Generic;
using Erasystemlevel.Parser;

namespace Erasystemlevel.Semantic
{
    public class CallTable : Dictionary<string, CallTableEntry>
    {
    }

    public class CallTableEntry
    {
        public AstNode node;

        public bool hasBody = true;
        public string name;

        public ArrayList parameters;
        public ArrayList results;

        public SymbolTable symbols;

        public CallTableEntry(AstNode node)
        {
            this.node = node;
            name = node.getValue().ToString();

            parameters = new ArrayList();
            results = new ArrayList();
            symbols = new SymbolTable();
        }
    }
}