using System.Collections.Generic;
using Erasystemlevel.Parser;
using Erasystemlevel.Semantic;

namespace Erasystemlevel.Generator
{
    public class CodeGenerator
    {
        public AsmBuffer assembly;

        private AastNode _tree;
        private MemoryManager memoryManager;

        private ModuleTable moduleTable;
        private DataTable dataTable;

        public CodeGenerator(AastNode tree, ModuleTable modules, DataTable data)
        {
            _tree = tree;
            moduleTable = modules;
            dataTable = data;

            reset();
        }

        public void generate()
        {
            allocateStatic();
            generateStaticInitializer();
            jumpToCode();
            generateModulesRoutines();
            generateCodeRoutine();
        }

        public void reset()
        {
            assembly = new AsmBuffer();
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
            memoryManager.setRegister(RegistersManager.SB_REG, _getMainModule().staticBase);
            memoryManager.setRegister(RegistersManager.SP_REG, memoryManager.getStaticPointer());
            memoryManager.moveFpToSp();

            // todo: вычисление значений для переменных модулей
        }

        private void jumpToCode()
        {
            var label = _generateRoutineLabel(SemanticAnalyzer2.basicModuleName, "code");
            asmJumpToLabel(label);
        }

        private void generateCodeRoutine()
        {
            var module = _getMainModule();

            _generateRoutine(module, module.routines["code"]);
        }

        private void generateModulesRoutines()
        {
            foreach (var modulePair in moduleTable)
            {
                var module = modulePair.Value;
                foreach (var routinePair in module.routines)
                {
                    var routine = routinePair.Value;

                    // we skip `code` routine
                    if (module.name != SemanticAnalyzer2.basicModuleName || routine.name != "code")
                    {
                        _generateRoutine(module, routine);
                    }
                }
            }
        }

        private void _generateRoutine(Module module, CallTableEntry2 routine)
        {
            // todo: генерация кода одной функции
        }

        private string _generateRoutineLabel(string module, string routine)
        {
            return module + "." + routine;
        }

        private void asmJumpToLabel(string name)
        {
            // todo: allocate register
            assembly.put(AsmBuilder.jumpToRegister("R-1"));
        }

        private Module _getMainModule()
        {
            return moduleTable[SemanticAnalyzer2.basicModuleName];
        }
    }
}