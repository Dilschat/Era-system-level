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
    /**
     * Converts nodes into asm code
     */
    public class AsmBuffer
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

        public void putLine()
        {
            statements.AddLast(new AstNode(null));
        }

        public void putComment(string text)
        {
            var node = new AstNode(text);
            node.SetNodeType(AstNode.NodeType.AsmComment);

            statements.AddLast(node);
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
            var children = item.getChilds();
            if (children.Count != 2 && item.GetNodeType() == AstNode.NodeType.OperationOnRegisters)
            {
                throw new GenerationError("Invalid node supplied for operation on registers");
            }

            if (item.getValue() == null)
            {
                return "";
            }

            if (item.GetNodeType() == AstNode.NodeType.AsmComment)
            {
                return "// " + item.getValue();
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
            var children = new List<AstNode>(item.getChilds());
            foreach (var t in children)
            {
                val.Add(((Token) t.getValue()).GetValue());
            }

            var literals = string.Join(", ", val.Select(x => x.ToString()).ToArray());

            return "DATA " + literals;
        }

        private static string generateLabel(AstNode item)
        {
            var val = item.getValue().ToString();

            return "<" + val + ">";
        }

        private static string generateStatement(AstNode item)
        {
            var val = item.getValue().ToString();
            var children = item.getChilds();

            var asmCode = "";
            if (val.Equals("skip") || val.Equals("stop"))
            {
                asmCode = val;
                if (children.Count == 1)
                {
                    asmCode += " " + children[0].getValue();
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
            var children = item.getChilds();

            var left = children[0];
            var right = children[1];

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