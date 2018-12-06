using System.Collections.Generic;

namespace Erasystemlevel.Semantic
{
    public class SymbolTable: Dictionary<string, SymbolTableEntry>
    {
        
    }
    
    public class SymbolTableEntry
    {
        public string type;
        public string name;
    }
}