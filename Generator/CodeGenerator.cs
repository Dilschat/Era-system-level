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
            generateCodeRoutine();
            generateModulesRoutines();

            assembly.put(AsmBuilder.label(":end"));
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
            memoryManager.initialize();

            // Execute modules and statements sequentially
            foreach (var ctx in _tree.getChilds())
            {
                if (ctx.GetNodeType() != AstNode.NodeType.Module) continue;
                var module = moduleTable[ctx.getValue().ToString()];
                memoryManager.setCurrentModule(module);

                foreach (var astNode in ctx.getChilds())
                {
                    if (astNode.GetNodeType() != AstNode.NodeType.Variable) continue;

                    foreach (var varDecl in astNode.getChilds())
                    {
                        if (varDecl.GetNodeType() == AstNode.NodeType.Type) continue;

                        // todo: вычисление значения для переменной модулей
                    }
                }
            }
        }

        private void generateCodeRoutine()
        {
            var module = _getMainModule();

            // On completion would jump to the end
            assembly.put(AsmBuilder.setRegister(RegistersManager.RL_REG, ":end"));

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

        private void _generateVarExpr(AstNode node)
        {
            var name = node.getChilds()[0].getValue();
        }

        private string _generateRoutineLabel(string module, string routine)
        {
            return module + "." + routine;
        }

        private Module _getMainModule()
        {
            return moduleTable[SemanticAnalyzer2.basicModuleName];
        }
    }
}