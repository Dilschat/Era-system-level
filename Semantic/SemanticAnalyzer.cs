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
        
        public SemanticAnalyzer(AstNode tree)
        {
            callTable = new CallTable();
        }

        public void AnalyzeTree(AstNode tree)
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
            ArrayList parameters = new ArrayList();
            foreach (AstNode i in childs)
            {
                parameters.Add(((Token)i.getChilds()[0].getChilds()[0].getValue()).GetValue());
            }

            return parameters;
        }

        private string AnalyzeAttribute(AstNode astNode)
        {
            return astNode.getValue().ToString();
        }
    }
}