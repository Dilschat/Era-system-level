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
        public AstNode node;
        public readonly string name;

        public int staticBase;

        public CallTable2 routines = new CallTable2();
        public SymbolTable2 symbols = new SymbolTable2();

        public Module(AstNode node)
        {
            this.node = node;
            name = node.getValue().ToString();
        }

        public Module(string name)
        {
            this.name = name;
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