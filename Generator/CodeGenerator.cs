using System.Collections.Generic;
using Erasystemlevel.Semantic;

namespace Erasystemlevel.Generator
{
    public class CodeGenerator
    {
        public AssemblyBuffer assembly;

        private AastNode root;
        private MemoryManager memoryManager;

        private ModuleTable moduleTable;
        private DataTable dataTable;

        public CodeGenerator(AastNode tree, ModuleTable modules, DataTable data)
        {
            root = tree;
            moduleTable = modules;
            dataTable = data;

            reset();
        }

        public void generate()
        {
            allocateStatic();
            generateStaticInitializer();
            generateModulesRoutines();
            generateCodeRoutine();
        }

        public void reset()
        {
            assembly = new AssemblyBuffer();
            memoryManager = new MemoryManager(assembly);
        }

        private void allocateStatic()
        {
            foreach (var de in dataTable)
            {
                memoryManager.appendData(de.Value);
            }

            foreach (var me in moduleTable)
            {
                var module = me.Value;
                var symbols = new List<SymbolTableEntry2>();

                foreach (var se in module.symbols)
                {
                    symbols.Add(se.Value);
                }

                memoryManager.appendModuleVariables(module, symbols.ToArray());
            }
        }

        private void generateStaticInitializer()
        {
            // todo
        }

        private void generateModulesRoutines()
        {
            // todo
        }

        private void generateCodeRoutine()
        {
            // todo
        }
    }
}