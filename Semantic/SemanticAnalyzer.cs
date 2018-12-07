using System.Collections.Generic;
using Erasystemlevel.Exception;
using Erasystemlevel.Parser;
using Erasystemlevel.Tokenizer;

namespace Erasystemlevel.Semantic
{
    public class SemanticAnalyzer
    {
        public const string basicModuleName = ":b";

        public readonly ModuleTable moduleTable;
        public readonly DataTable dataTable;

        private readonly HashSet<string> reservedNames;

        private readonly AstNode _tree;
        public readonly AastNode annotatedTree;

        public SemanticAnalyzer(AstNode tree)
        {
            moduleTable = new ModuleTable();
            dataTable = new DataTable();
            reservedNames = new HashSet<string>();

            _tree = tree;
            annotatedTree = new AastNode(tree); // fixme

            var basicModule = new Module(basicModuleName);
            moduleTable.Add(basicModule);
        }

        public void analyze()
        {
            foreach (var ctx in _tree.getChilds())
            {
                if (ctx.GetNodeType() == AstNode.NodeType.Module)
                {
                    handleModule(ctx);
                }
                else if (ctx.GetNodeType() == AstNode.NodeType.Data)
                {
                    handleData(ctx);
                }
                else if (ctx.GetNodeType() == AstNode.NodeType.Routine)
                {
                    handleRoutine(ctx);
                }
                else if (ctx.GetNodeType() == AstNode.NodeType.Code)
                {
                    handleCode(ctx);
                }
            }

            validate();
        }

        private void handleData(AstNode node)
        {
            // check module name in reservedNames
            var dataName = ((Token) node.getValue()).GetValue();
            if (reservedNames.Contains(dataName))
            {
                throw new SemanticError("Data name is not unique: " + dataName);
            }

            // add to data table
            dataTable.Add(node);

            // add data name to reservedNames
            reservedNames.Add(dataName);
        }

        private void handleModule(AstNode node)
        {
            // check module name in reservedNames
            var moduleName = ((Token) node.getValue()).GetValue();
            if (reservedNames.Contains(moduleName))
            {
                throw new SemanticError("Module name is not unique: " + moduleName);
            }

            // add module name to reservedNames
            reservedNames.Add(moduleName);

            // add to module table
            var module = new Module(node);

            // add all variables and functions to this module
            foreach (var i in node.getChilds())
            {
                if (i.GetNodeType().Equals(AstNode.NodeType.Variable) ||
                    i.GetNodeType().Equals(AstNode.NodeType.Constant))
                {
                    module.addVariable(i);
                }
                else if (i.GetNodeType().Equals(AstNode.NodeType.Routine))
                {
                    module.addRoutine(i);
                }
            }

            moduleTable.Add(module);
        }

        private void handleRoutine(AstNode node)
        {
            var name = "";
            var parameters = new List<string>();
            foreach (var i in node.getChilds())
            {
                if (i.GetNodeType().Equals(AstNode.NodeType.Identifier))
                {
                    name = ((Token) i.getValue()).GetValue();
                }
                else if (i.GetNodeType().Equals(AstNode.NodeType.Parameters))
                {
                    foreach (var parameter in i.getChilds())
                    {
                        var parameterChilds = parameter.getChilds();
                        parameters.Add(((Token) parameterChilds[1].getValue()).GetValue());
                    }
                }
            }

            // check function name in reservedNames
            if (reservedNames.Contains(name))
            {
                throw new SemanticError("Routine name is not unique: " + name);
            }

            // add function name to reservedNames
            reservedNames.Add(name);

            // check function parameters in reservedNames
            foreach (var i in parameters)
            {
                if (reservedNames.Contains(i))
                {
                    throw new SemanticError("Routine parameter name is not unique: " + i);
                }

                reservedNames.Add(name);
            }

            // add to this function to basic module
            var module = moduleTable[basicModuleName];
            module.addRoutine(node);
            moduleTable.Add(name, module);

            validateRoutine(module, node);
        }

        // add function `code` to basic module, may be wrapper above handleRoutine
        private void handleCode(AstNode node)
        {
            var basicModule = moduleTable[basicModuleName];
            basicModule.addRoutine(node);
        }

        private void validateRoutine(Module module, AstNode node)
        {
            // check symbols and make links
            // check that return registers are used
            // check that return registers are last statements
        }

        // throw an exceptions if there is no `code` function in basic module
        private void validate()
        {
            if (!moduleTable[basicModuleName].routines.ContainsKey("Code"))
            {
                throw new SemanticError("No code provided");
            }
        }
    }
}