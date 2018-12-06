using System.Collections;
using System.Collections.Generic;
using Erasystemlevel.Parser;
using Erasystemlevel.Tokenizer;

namespace Erasystemlevel.Semantic
{
    public class SemanticAnalyzer
    {
        public SymbolTable symbolTable;
        public CallTable callTable;
        private AstNode tree;
        
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
            List<AstNode> childs = tree.getChilds();
            if (tree.GetNodeType().Equals(AstNode.NodeType.Routine))
            {
                CallTableEntry entry = new CallTableEntry();
                foreach(AstNode i in childs)
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
                foreach(AstNode i in childs)
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
            }else if(tree.GetNodeType().Equals(AstNode.NodeType.Variable))
            {
                List<AstNode> childs = tree.getChilds();
                var type = ((Token) childs[0].getValue()).GetValue();
                for (int i = 1; i < childs.Count; i++)
                {
                    SymbolTableEntry entry = new SymbolTableEntry
                    {

                        type = type, name = ((Token) childs[i].getChilds()[0].getValue()).GetValue()

                    };
                }
                


            }
        }
        
        private string AnalyzeName(AstNode astNode)
        {
            return astNode.getValue().ToString();
        }

        private ArrayList AnalyzeParameters(AstNode astNode)
        {
            
            List<AstNode> childs = astNode.getChilds();
            ArrayList parameters = new ArrayList();
            foreach (AstNode i in childs)
            {
                parameters.Add(((Token)i.getChilds()[0].getChilds()[0].getValue()).GetValue());
            }

            return parameters;
        }

        private ArrayList AnalyzeResults(AstNode astNode)
        {
            List<AstNode> childs = astNode.getChilds();
            ArrayList results = new ArrayList();
            foreach (AstNode i in childs)
            {
                results.Add(((Token)i.getValue()).GetValue());
            }

            return results;
        }

        private string AnalyzeAttribute(AstNode astNode)
        {
            return astNode.getValue().ToString();
        }
    }
}