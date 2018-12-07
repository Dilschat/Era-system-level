using Erasystemlevel.Parser;

namespace Erasystemlevel.Generator
{
    /**
     * Builds nodes with asm code
     */
    public class AsmBuilder
    {
        public static AstNode jumpToRegister(string reg)
        {
            return null;
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