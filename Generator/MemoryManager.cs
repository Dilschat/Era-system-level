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

        public void appendModuleVariables(Module module, SymbolTableEntry[] symbols)
        {
            module.staticBase = staticPointer;

            var locId = 0;
            foreach (var symbol in symbols)
            {
                symbol.localId = locId++;
            }

            staticPointer += wordSize * symbols.Length;
        }

        public void initialize()
        {
            setRegister(RegistersManager.SB_REG, 0);
            setRegister(RegistersManager.SP_REG, getStaticPointer());
            moveFpToSp();
        }

        public int getStaticPointer()
        {
            return staticPointer;
        }

        public void setRegister(string reg, int index)
        {
            buffer.put(AsmBuilder.setRegister(reg, index));
        }

        public void setCurrentModule(Module module)
        {
            buffer.put(AsmBuilder.setRegister(RegistersManager.SB_REG, module.staticBase));
        }

        public void moveFpToSp()
        {
            buffer.put(AsmBuilder.setRegister(RegistersManager.FP_REG, RegistersManager.SP_REG));
        }
    }

    internal class RegistersManager
    {
        // System registers
        internal const string PC_REG = "R31"; // Program counter
        internal const string SB_REG = "R30"; // Static base
        internal const string SP_REG = "R29"; // Stack pointer
        internal const string FP_REG = "R28"; // Frame pointer

        // Useful registers
        internal const string JL_REG = "R27"; // Jump location register
        internal const string RL_REG = "R26"; // Return location register

        private AsmBuffer buffer;

        internal RegistersManager(AsmBuffer assembly)
        {
            buffer = assembly;
        }
    }
}