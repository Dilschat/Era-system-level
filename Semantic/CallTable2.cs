using System.Collections;
using System.Collections.Generic;

namespace Erasystemlevel.Semantic
{
    public class CallTable2 : Dictionary<string, CallTableEntry2>
    {
    }

    public class CallTableEntry2
    {
        public bool hasBody = true;
        public string name;

        public ArrayList parameters;
        public ArrayList results;

        public SymbolTable2 symbols;

        public CallTableEntry2(string name)
        {
            this.name = name;

            parameters = new ArrayList();
            results = new ArrayList();
            symbols = new SymbolTable2();
        }
    }
}