using System.Collections.Generic;
using Erasystemlevel.Parser;
using Erasystemlevel.Semantic;

namespace Erasystemlevel.Generator
{
    public class MemoryManager
    {
        private const int wordSize = 32;

        private RegistersManager registers;
        private AssemblyBuffer buffer;

        private int staticPointer = 0; // Pointer to the top of the static data

        public MemoryManager(AssemblyBuffer assembly)
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
    }

    class RegistersManager
    {
        private const string PC_REG = "R31"; // Program counter
        private const string SB_REG = "R30"; // Static base
        private const string SP_REG = "R29"; // Stack pointer
        private const string FP_REG = "R28"; // Frame pointer

        internal RegistersManager(AssemblyBuffer assembly)
        {
        }
    }
}