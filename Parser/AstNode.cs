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

        public void setValue(Object value){
            this.value = value;
        }

        public Object getValue(){
            return this.value;
        }

        public void addChild(AstNode node){
            childs.Add(node);
        }

        public void cleanChild(){
            childs = new ArrayList();
        }


    }
}
