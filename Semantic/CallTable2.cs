using System.Collections;
using System.Collections.Generic;
using Erasystemlevel.Parser;

namespace Erasystemlevel.Semantic
{
    public class CallTable2 : Dictionary<string, CallTableEntry2>
    {
    }

    public class CallTableEntry2
    {
        public AstNode node;
        
        public bool hasBody = true;
        public string name;

        public ArrayList parameters;
        public ArrayList results;

        public SymbolTable2 symbols;

        public CallTableEntry2(AstNode node)
        {
            this.node = node;
            name = node.getValue().ToString();

            parameters = new ArrayList();
            results = new ArrayList();
            symbols = new SymbolTable2();
        }
    }
}