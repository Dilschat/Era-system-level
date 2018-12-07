using System;
using System.Collections;
using System.Collections.Generic;
using Erasystemlevel.Exception;
using Erasystemlevel.Parser;
using Erasystemlevel.Tokenizer;
using EraSystemLevel.Semantic;
using NUnit.Framework.Constraints;

namespace Erasystemlevel.Semantic
{
    public class SemanticAnalyzer2
    {
        public const string basicModuleName = ":b";

        public readonly ModuleTable moduleTable;
        public readonly DataTable dataTable;

        private readonly HashSet<string> reservedNames;

        private readonly AstNode _tree;
        public readonly AastNode annotatedTree;

        public SemanticAnalyzer2(AstNode tree)
        {
            moduleTable = new ModuleTable();
            dataTable = new DataTable();
            reservedNames = new HashSet<string>();

            _tree = tree;

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
            // add to data table
            // add data name to reservedNames
        }

        private void handleModule(AstNode node)
        {
            // check module name in reservedNames
            // add module name to reservedNames
            // add to module table
            // add all variables and functions to this table
        }

        private void handleRoutine(AstNode node)
        {
            // check function name in reservedNames
            // check function parameters in reservedNames
            // add function name to reservedNames
            // add to this function to basic module

            validateRoutine(null, node);
        }

        private void handleCode(AstNode node)
        {
            // add function `code` to basic module, may be wrapper above handleRoutine
        }

        private void validateRoutine(Module module, AstNode node)
        {
            // check symbols and make links
            // check that return registers are used
            // check that return registers are last statements
        }

        private void validate()
        {
            // throw an exceptions if there is no `code` function in basic module
        }
    }
}