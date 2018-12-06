using System.Collections.Generic;
using System.Text;
using Erasystemlevel.Parser;

namespace EraSystemLevel.Generator
{
    public class AssemblyBuffer
    {
        private LinkedList<AstNode> statements;

        public AssemblyBuffer()
        {
        }

        public void put(AstNode node)
        {
            if (node.GetNodeType() == AstNode.NodeType.AssemblerStatement ||
                node.GetNodeType() == AstNode.NodeType.OperationOnRegisters)
            {
                statements.AddLast(node);
            }

            // todo: check that this node is as assembly statement and then push it to statements list
        }

        public override string ToString()
        {
            var asmCode = new StringBuilder();

            foreach (var item in statements)
            {
                asmCode.Append(statementToString(item) + "\n");
            }

            return base.ToString();
        }

        private static string statementToString(AstNode item)
        {
            if (item.GetNodeType() == AstNode.NodeType.OperationOnRegisters)
            {
                if (item.getValue().Equals(":="))
                {
                    // todo
                }
                else
                {
                    var childs = item.getChilds();
                    var left = (AstNode) childs[0];
                    var right = (AstNode) childs[1];
                    
                    return left.getValue() + " " + item.getValue() + " " + right.getValue() + ";";
                }
                
            } else if (item.GetNodeType() == AstNode.NodeType.AssemblerStatement)
            {
                // todo
            }

            return "";
        }
    }
}