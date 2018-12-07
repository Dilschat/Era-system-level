using System.Collections.Generic;
using Erasystemlevel.Parser;
using Erasystemlevel.Semantic;
using EraSystemLevel.Semantic;

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
                memoryManager.addData(de.Value.node);
            }

            foreach (var me in moduleTable)
            {
                var module = me.Value;
               
                foreach (var se in module.symbols)
                {
                    var symbol = se.Value;
                    
                    memoryManager.addModuleVariable(module, symbol);
                }
            }
            
            memoryManager.generateDataAllocation();
            memoryManager.generateStaticAllocation();
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