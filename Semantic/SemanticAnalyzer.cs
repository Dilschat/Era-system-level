using System.Collections;
using System.Collections.Generic;
using Erasystemlevel.Parser;
using Erasystemlevel.Tokenizer;

namespace EraSystemLevel.Semantic
{
    public class SemanticAnalyzer
    {
        public SymbolTable symbolTable;
        public CallTable callTable;
        
        public SemanticAnalyzer(AstNode tree)
        {
            callTable = new CallTable();
        }

        public void AnalyzeTree(AstNode tree)
        {
            ArrayList childs = tree.getChilds();
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
                        entry.functionType= AnalyzeName(i);

                    }
                    else if (i.GetNodeType().Equals(AstNode.NodeType.Parameters))
                    {
                        entry.functionType= AnalyzeParameters(i);

                    }
                    else if (i.GetNodeType().Equals(AstNode.NodeType.Results))
                    {
                        entry.functionType= AnalyzeResults(i);

                    }
                }
            }
            else
            {
                foreach(AstNode i in childs)
                {
                    AnalyzeTree(i);
                }
            }
        }

        private string AnalyzeName(AstNode astNode)
        {
            return astNode.getValue().ToString();
        }

        private string AnalyzeParameters(AstNode astNode)
        {
            
            ArrayList childs = astNode.getChilds();
            ArrayList parameters = new ArrayList();
            foreach (AstNode i in childs)
            {
                parameters.Add(((Token) ( i.getChilds()[0]).getChilds()[0]).GetValue());
            }

            throw new System.NotImplementedException();
        }

        private string AnalyzeResults(AstNode astNode)
        {
            throw new System.NotImplementedException();
        }

        private string AnalyzeAttribute(AstNode astNode)
        {
            return astNode.getValue().ToString();
        }
    }
}