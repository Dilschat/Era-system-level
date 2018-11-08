using System;
using System.Collections;

namespace Erasystemlevel.Parser
{
    public class AstNode
    {
        private Object value;
        private ArrayList childs = new ArrayList();
        public AstNode(Object value)
        {
            this.value = value;
        }

        public void addChild(AstNode node){
            childs.Add(node);
        }
    }
}
