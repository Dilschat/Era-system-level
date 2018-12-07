using System.Collections.Generic;

namespace Erasystemlevel.Semantic
{
    public class SymbolTable2 : Dictionary<string, SymbolTableEntry2>
    {
    }

    public class SymbolTableEntry2
    {
        public int localId;

        public string name;
        public string type;

        public bool isConst = false;
        public bool isArray = false;

        public bool isInitialized = false;

        public SymbolTableEntry2(string name)
        {
            this.name = name;
        }

        public override string ToString()
        {
            return name;
        }
    }
}