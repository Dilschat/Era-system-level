using System.Collections.Generic;
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
            // todo: check that this node is as assembly statement and then push it to statements list
        }

        public override string ToString()
        {
            // todo: generate assembly text from statements list
            return base.ToString();
        }
    }
}