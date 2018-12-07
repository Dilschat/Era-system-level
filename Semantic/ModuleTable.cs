using System.Collections.Generic;
using Erasystemlevel.Parser;

namespace Erasystemlevel.Semantic
{
    public class ModuleTable : Dictionary<string, Module>
    {
        public void Add(Module m)
        {
            Add(m.name, m);
        }
    }

    public class Module
    {
        public readonly string name;

        private int staticBase;
        
        private CallTable2 routines;
        private SymbolTable2 symbols;

        public Module(string name)
        {
            this.name = name;
            
            routines = new CallTable2();
            symbols = new SymbolTable2();
        }

        public void addVariable(AstNode node)
        {
            // todo
        }

        public void addRoutine(AstNode node)
        {
            // todo
        }
    }
}