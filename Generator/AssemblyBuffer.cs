using System.Collections.Generic;
using System.Text;
using Erasystemlevel.Exception;
using Erasystemlevel.Parser;

namespace Erasystemlevel.Generator
{
    public class AssemblyBuffer
    {
        private LinkedList<AstNode> statements;

        public void put(AstNode node)
        {
            if (node.GetNodeType() == AstNode.NodeType.AssemblerStatement ||
                node.GetNodeType() == AstNode.NodeType.OperationOnRegisters)
            {
                statements.AddLast(node);
            }
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
            var childs = item.getChilds();
            var left = childs[0];
            var right = childs[1];

            var leftRegister = (string) left.getValue();
            var rightRegister = (string) right.getValue();

            if (item.GetNodeType() == AstNode.NodeType.OperationOnRegisters)
            {
                if (item.getValue().Equals(":="))
                {
                    if (leftRegister.Equals("*"))
                    {
                        leftRegister += " " + left.getChilds()[0].getValue();
                    }

                    if (rightRegister.Equals("*"))
                    {
                        rightRegister += " " + right.getChilds()[0].getValue();
                    }
                }

                return leftRegister + " " + item.getValue() + " " + rightRegister + ";";
            }

            if (item.GetNodeType() == AstNode.NodeType.AssemblerStatement)
            {
                var val = item.getValue().ToString();

                if (val.Equals("skip") || val.Equals("stop"))
                {
                    return val;
                }

                if (val.Equals("if"))
                {
                    var gotoNode = item.getChilds()[1];

                    var condRegister = item.getChilds()[0].getValue();
                    var addrRegister = gotoNode.getChilds()[0].getValue();

                    return val + " " + condRegister + " " + gotoNode.getValue() + " " + addrRegister;
                }
            }

            throw new GenerationError("Invalid assembly statement supplied");
        }
    }
}