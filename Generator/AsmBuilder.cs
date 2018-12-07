using Erasystemlevel.Parser;

namespace Erasystemlevel.Generator
{
    /**
     * Builds nodes with asm code
     */
    public class AsmBuilder
    {
        public static AstNode label(string name)
        {
            var node = new AstNode(name);
            node.SetNodeType(AstNode.NodeType.Label);

            return node;
        }

        public static AstNode condJump(string r1, string r2)
        {
            var ifNode = new AstNode("if");
            ifNode.SetNodeType(AstNode.NodeType.AssemblerStatement);

            var retReg = new AstNode(r1);
            retReg.SetNodeType(AstNode.NodeType.Register);

            var gotoNode = new AstNode("goto");
            gotoNode.SetNodeType(AstNode.NodeType.AssemblerStatement);

            var jumpReg = new AstNode(r2);
            jumpReg.SetNodeType(AstNode.NodeType.Register);

            ifNode.addChild(retReg);
            ifNode.addChild(gotoNode);
            gotoNode.addChild(jumpReg);

            return ifNode;
        }

        public static AstNode setRegister(string reg, int val)
        {
            return opOnRegister(reg, ":=", val.ToString());
        }

        public static AstNode setRegister(string rl, string rr)
        {
            return opOnRegister(rl, ":=", rr);
        }

        public static AstNode opOnRegister(string rl, string op, string rr)
        {
            var node = new AstNode(op);
            node.addChild(new AstNode(rl));
            node.addChild(new AstNode(rr));
            node.SetNodeType(AstNode.NodeType.OperationOnRegisters);

            return node;
        }
    }
}