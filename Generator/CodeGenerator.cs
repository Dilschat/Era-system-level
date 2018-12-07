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

        private SymbolTable symbolTable;
        private CallTable callTable;

        public CodeGenerator(AstNode tree, SymbolTable symbols, CallTable calls)
        {
            root = tree;
            symbolTable = symbols;
            callTable = calls;

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