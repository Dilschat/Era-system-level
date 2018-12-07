using System.Collections.Generic;
using Erasystemlevel.Parser;
using Erasystemlevel.Semantic;

namespace Erasystemlevel.Generator
{
    public class MemoryManager
    {
        private RegistersManager registers;

        private Dictionary<string, int> moduleBase; // Static base for each module
        private int staticPointer = 0; // Pointer to the top of the static data

        public MemoryManager(AssemblyBuffer assembly)
        {
            registers = new RegistersManager(assembly);
        }

        public void addModuleVariable(Module module, SymbolTableEntry2 node)
        {
            // todo
        }

        public void addData(AstNode node)
        {
            // todo
        }

        public void generateDataAllocation()
        {
            // todo
        }

        public void generateStaticAllocation()
        {
            // todo
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