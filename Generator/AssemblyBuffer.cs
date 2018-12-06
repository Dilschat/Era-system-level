using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Erasystemlevel.Exception;
using Erasystemlevel.Parser;

namespace Erasystemlevel.Generator
{
    public class AssemblyBuffer
    {
        private LinkedList<AstNode> statements = new LinkedList<AstNode>();

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

            return asmCode.ToString();
        }


        public static string statementToString(AstNode item)
        {
            var val = item.getValue().ToString();
            var childs = item.getChilds();

            if (childs.Count != 2 && item.GetNodeType() == AstNode.NodeType.OperationOnRegisters)
            {
                throw new GenerationError("Invalid node supplied for operation on registers");
            }

            if (item.GetNodeType() == AstNode.NodeType.AssemblerStatement)
            {
                if (val.Equals("skip") || val.Equals("stop"))
                {
                    var asmCode = val;
                    if (childs.Count == 1)
                    {
                        asmCode += " " + childs[0].getValue();
                    }
                    
                    return asmCode + ";";
                }

                if (val.Equals("if"))
                {
                    var gotoNode = item.getChilds()[1];

                    var condRegister = item.getChilds()[0].getValue();
                    var addrRegister = gotoNode.getChilds()[0].getValue();

                    return val + " " + condRegister + " " + gotoNode.getValue() + " " + addrRegister + ";";
                }
            }

            var left = childs[0];
            var right = childs[1];

            var leftRegister = left.getValue().ToString();
            var rightRegister = right.getValue().ToString();
            if (item.GetNodeType() == AstNode.NodeType.OperationOnRegisters)
            {
                if (val.Equals(":="))
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

            throw new GenerationError("Invalid assembly statement supplied");
        }
    }
}