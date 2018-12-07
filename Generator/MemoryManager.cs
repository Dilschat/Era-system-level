namespace Erasystemlevel.Generator
{
    public class MemoryManager
    {
        private RegistersManager registers;
        
        public MemoryManager(AssemblyBuffer assembly)
        {
            registers = new RegistersManager(assembly);
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