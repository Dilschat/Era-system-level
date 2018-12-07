using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Erasystemlevel.Exception;
using Erasystemlevel.Parser;
using Erasystemlevel.Tokenizer;

namespace Erasystemlevel.Generator
{
    public class AssemblyBuffer
    {
        private readonly LinkedList<AstNode> statements = new LinkedList<AstNode>();

        public void put(AstNode node)
        {
            if (node.GetNodeType() == AstNode.NodeType.AssemblerStatement ||
                node.GetNodeType() == AstNode.NodeType.OperationOnRegisters ||
                node.GetNodeType() == AstNode.NodeType.Label ||
                node.GetNodeType() == AstNode.NodeType.Data ||
                node.GetNodeType() == AstNode.NodeType.Directive)
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
            var childs = item.getChilds();
            if (childs.Count != 2 && item.GetNodeType() == AstNode.NodeType.OperationOnRegisters)
            {
                throw new GenerationError("Invalid node supplied for operation on registers");
            }

            if (item.GetNodeType() == AstNode.NodeType.Label)
            {
                return generateLabel(item);
            }

            if (item.GetNodeType() == AstNode.NodeType.Data)
            {
                return generateData(item);
            }

            if (item.GetNodeType() == AstNode.NodeType.AssemblerStatement)
            {
                return generateStatement(item);
            }

            if (item.GetNodeType() == AstNode.NodeType.OperationOnRegisters)
            {
                return generateRegisterOperation(item);
            }

            throw new GenerationError("Invalid assembly statement supplied");
        }

        private static string generateData(AstNode item)
        {
            var val = new List<string>();
            var childs = new List<AstNode>(item.getChilds());
            for (var i = 1; i < childs.Count; i++)
            {
                val.Add(((Token) childs[i].getValue()).GetValue());
            }
            var literals = string.Join(",", val.Select(x => x.ToString()).ToArray());

            return "DATA" + literals;
        }

        private static string generateLabel(AstNode item)
        {
            var val = item.getValue().ToString();

            return "<" + val + ">";
        }

        private static string generateStatement(AstNode item)
        {
            var val = item.getValue().ToString();
            var childs = item.getChilds();

            var asmCode = "";
            if (val.Equals("skip") || val.Equals("stop"))
            {
                asmCode = val;
                if (childs.Count == 1)
                {
                    asmCode += " " + childs[0].getValue();
                }
            }
            else if (val.Equals("if"))
            {
                var gotoNode = item.getChilds()[1];

                var condRegister = item.getChilds()[0].getValue();
                var addrRegister = gotoNode.getChilds()[0].getValue();

                asmCode = val + " " + condRegister + " " + gotoNode.getValue() + " " + addrRegister;
            }

            return asmCode + ";";
        }

        private static string generateRegisterOperation(AstNode item)
        {
            var val = item.getValue().ToString();
            var childs = item.getChilds();

            var left = childs[0];
            var right = childs[1];

            var leftRegister = left.getValue().ToString();
            var rightRegister = right.getValue().ToString();

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
    }
}