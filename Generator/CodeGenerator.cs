using System.Collections.Generic;
using Erasystemlevel.Parser;
using Erasystemlevel.Semantic;

namespace Erasystemlevel.Generator
{
    public class CodeGenerator
    {
        public AssemblyBuffer assembly;

        private AstNode root;
        private MemoryManager memoryManager;

        private ModuleTable moduleTable;
        private DataTable dataTable;

        public CodeGenerator(AstNode tree, ModuleTable modules, DataTable data)
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
            // todo
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