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

        public override bool Equals(object obj)
        {
            if (obj == null||obj.GetType() != typeof(SymbolTableEntry))
            {
                return false;
            }

            var val = (SymbolTableEntry) obj;
            return type.Equals(val.type) && name.Equals(val.name);
        }
    }
}