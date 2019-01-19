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
            
            //assembly.putLine();
            assembly.putComment("Init static data");
            generateStaticInitializer();
            
            assembly.putComment("Code block");
            generateCodeRoutine();
            
            assembly.putLine();
            generateModulesRoutines();

            assembly.putLine();
            assembly.putComment("End of program");
            assembly.put(AsmBuilder.label(":end"));
        }

        public void reset()
        {
            assembly = new AsmBuffer();
            memoryManager = new MemoryManager(assembly);
        }

        private void allocateStatic()
        {
            assembly.putComment("Data section");
            foreach (var de in dataTable)
            {
                memoryManager.appendData(de.Value);
            }

            foreach (var me in moduleTable)
            {
                var module = me.Value;
                var symbols = new List<SymbolTableEntry>();

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
                var module = moduleTable[Module.getName(ctx)];
                memoryManager.setCurrentModule(module);

                assembly.putComment("Eval static data for `" + module.name + "`");
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

            _generateRoutine(module, module.routines["Code"]);
        }

        private void generateModulesRoutines()
        {
            foreach (var modulePair in moduleTable)
            {
                var module = modulePair.Value;
                
                assembly.putComment("Routines for `" + module.name + "`");
                foreach (var routinePair in module.routines)
                {
                    var routine = routinePair.Value;

                    // we skip `Code` routine
                    if (module.name != SemanticAnalyzer.BasicModuleName || routine.name != "Code")
                    {
                        _generateRoutine(module, routine);
                    }
                }
            }
        }

        private void _generateRoutine(Module module, RoutineTableEntry routine)
        {
            // todo: генерация кода одной функции
            assembly.put(AsmBuilder.condJump(RegistersManager.JL_REG, RegistersManager.RL_REG));
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
            return moduleTable[SemanticAnalyzer.BasicModuleName];
        }
    }
}