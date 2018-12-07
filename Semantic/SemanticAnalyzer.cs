using System.Collections;
using System.Collections.Generic;
using Erasystemlevel.Exception;
using Erasystemlevel.Parser;
using Erasystemlevel.Tokenizer;

namespace Erasystemlevel.Semantic
{
    public class SemanticAnalyzer
    {
        public readonly SymbolTable symbolTable;
        public readonly CallTable callTable;
        private readonly AstNode tree;
        
        public SemanticAnalyzer(AstNode tree)
        {
            callTable = new CallTable();
            symbolTable = new SymbolTable();
            this.tree = tree;
        }

        public void generateTables()
        {
            AnalyzeTree(tree);
        }
        private void AnalyzeTree(AstNode tree)
        {
            var childes = tree.getChilds();
            if (tree.GetNodeType().Equals(AstNode.NodeType.Routine))
            {
                var entry = new CallTableEntry();
                foreach(var i in childes)
                {
                    if (i.GetNodeType().Equals(AstNode.NodeType.Attribute))
                    {
                        entry.functionType= AnalyzeAttribute(i);
                    }else if (i.GetNodeType().Equals(AstNode.NodeType.Identifier))
                    {
                        entry.functionName= AnalyzeName(i);

                    }
                    else if (i.GetNodeType().Equals(AstNode.NodeType.Parameters))
                    {
                        entry.parameters= AnalyzeParameters(i);

                    }
                    else if (i.GetNodeType().Equals(AstNode.NodeType.Results))
                    {
                        entry.results= AnalyzeResults(i);

                    }
                    else if (i.GetNodeType().Equals(AstNode.NodeType.RoutineBody))
                    {
                        AnalyzeSymbols(i);

                    }
                }
            }
            else
            {
                AnalyzeSymbols(tree);
                foreach(var i in childes)
                {
                    AnalyzeTree(i);
                }
            }
        }

        private void AnalyzeSymbols(AstNode tree)
        {
            if (tree.GetNodeType().Equals(AstNode.NodeType.ConstDefinition))
            {
                SymbolTableEntry entry = new SymbolTableEntry
                {
                    type = "int", name = ((Token) tree.getChilds()[0].getValue()).GetValue()
                };
                checkSymbolEntry(entry);
                symbolTable.Add(entry.name, entry);


            }else if(tree.GetNodeType().Equals(AstNode.NodeType.Variable))
            {
                var childes = tree.getChilds();
                var type = ((Token) childes[0].getValue()).GetValue();
                for (var i = 1; i < childes.Count; i++)
                {
                    var entry = new SymbolTableEntry
                    {

                        type = type, name = ((Token) childes[i].getChilds()[0].getValue()).GetValue()

                    };
                    checkSymbolEntry(entry);
                    symbolTable.Add(entry.name, entry);
                    
                }
                


            }
        }
        
        private static string AnalyzeName(AstNode astNode)
        {
            return astNode.getValue().ToString();
        }

        private ArrayList AnalyzeParameters(AstNode astNode)
        {
            
            var childes = astNode.getChilds();
            var parameters = new ArrayList();
            foreach (var i in childes)
            {
                parameters.Add(((Token)i.getChilds()[0].getChilds()[0].getValue()).GetValue());
            }
             
            return parameters;
        }

        private static ArrayList AnalyzeResults(AstNode astNode)
        {
            var childes = astNode.getChilds();
            var results = new ArrayList();
            foreach (var i in childes)
            {
                results.Add(((Token)i.getValue()).GetValue());
            }
            return results;
        }

        private static string AnalyzeAttribute(AstNode astNode)
        {
            return astNode.getValue().ToString();
        }

        private void checkSymbolEntry(SymbolTableEntry entry)
        {

            if (!entry.Equals(symbolTable[entry.name]))
            {
                throw new SemanticError("Type error for:"+ entry.ToString());

            }
        }
        
        private void checkCallEntry(CallTableEntry entry)
        {


        }
    }
}