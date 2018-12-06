using System.Collections;
using System.Collections.Generic;

namespace Erasystemlevel.Semantic
{
    public class CallTable: Dictionary<string, CallTableEntry>
    {
        
    }
    
    public class CallTableEntry
    {
        public string functionName;
        public string functionType;
        public ArrayList parameters;
        public ArrayList results;
    }
}