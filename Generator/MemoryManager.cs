using System.Collections.Generic;
using Erasystemlevel.Parser;
using Erasystemlevel.Semantic;

namespace Erasystemlevel.Generator
{
    public class MemoryManager
    {
        private const int wordSize = 32;

        private RegistersManager registers;
        private AsmBuffer buffer;

        private int staticPointer = 0; // Pointer to the top of the static data

        public MemoryManager(AsmBuffer assembly)
        {
            buffer = assembly;
            registers = new RegistersManager(assembly);
        }

        public void appendData(DataTableEntry entry)
        {
            entry.baseAddr = staticPointer;

            buffer.put(entry.node);

            staticPointer += wordSize * entry.node.getChilds().Count;
        }

        public void appendModuleVariables(Module module, SymbolTableEntry2[] symbols)
        {
            module.staticBase = staticPointer;

            var locId = 0;
            foreach (var symbol in symbols)
            {
                symbol.localId = locId++;
            }

            staticPointer += wordSize * symbols.Length;
        }

        public int getStaticPointer()
        {
            return staticPointer;
        }

        public void setRegister(string reg, int index)
        {
            buffer.put(asmSetRegister(reg, index));
        }

        public void moveFpToSp()
        {
            buffer.put(asmSetRegister(RegistersManager.FP_REG, RegistersManager.SP_REG));
        }

        private AstNode asmSetRegister(string reg, int val)
        {
            return asmOpOnRegister(reg, ":=", val.ToString());
        }

        private AstNode asmSetRegister(string rl, string rr)
        {
            return asmOpOnRegister(rl, ":=", rr);
        }

        private AstNode asmOpOnRegister(string rl, string op, string rr)
        {
            var node = new AstNode(op);
            node.addChild(new AstNode(rl));
            node.addChild(new AstNode(rr));
            node.SetNodeType(AstNode.NodeType.OperationOnRegisters);

            return node;
        }
    }

    internal class RegistersManager
    {
        internal const string PC_REG = "R31"; // Program counter
        internal const string SB_REG = "R30"; // Static base
        internal const string SP_REG = "R29"; // Stack pointer
        internal const string FP_REG = "R28"; // Frame pointer
        internal const string TR_REG = "R27"; // Register with always true value
        internal const string FL_REG = "R26"; // Register with always false value

        internal RegistersManager(AsmBuffer assembly)
        {
        }
    }
}