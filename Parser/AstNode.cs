using System;
using System.Collections;
using System.Collections.Generic;

namespace Erasystemlevel.Parser
{
    public class AstNode
    {
        private Object value;
        private NodeType type;
        private ArrayList childs = new ArrayList();

        public enum NodeType
        {
            Unit,
            Code,
            Data,
            Routine,
            Module,
            Variable,
            Constant,
            Identifier,
            Literal,
            Declaration,
            Definition,
            Type,
            Expression,
            ConstDefinition,
            Statement,
            Label,
            AssemblerStatement,
            OperationOnRegisters,
            Attribute,
            Parameters,
            Results,
            Parameter,
            RoutineBody,
            Primary,
            VariableReference,
            Deference,
            ArrayElement,
            DataElement,
            ExplicitAddress,
            Operand,
            Address,
            Receiver,
            Operator,
            Register,
            Directive,
            ExtensionStatement,
            If,
            IfBody,
            Call,
            CallParameters,
            For,
            While,
            LoopBody,
            Break,
            Swap,
            Goto,
            Assignment,
            VarDefinition
        }

        public AstNode(NodeType type, String value)
        {
            this.type = type;
            this.value = value;
        }

        public void SetNodeType(NodeType type)
        {
            this.type = type;
        }

        public AstNode()
        {
        }

        public AstNode(Object value)
        {
            this.value = value;
        }

        public NodeType GetNodeType()
        {
            return type;
        }

        public void setValue(Object value)
        {
            this.value = value;
        }

        public Object getValue()
        {
            return this.value;
        }

        public void addChild(AstNode node)
        {
            childs.Add(node);
        }

        public void cleanChild()
        {
            childs = new ArrayList();
        }

        public ArrayList getChilds()
        {
            return childs;
        }

        public override bool Equals(Object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                ArrayList thisChilds = this.getChilds();
                ArrayList objChilds = ((AstNode) obj).getChilds();

                if (thisChilds.Count != (objChilds.Count))
                {
                    return false;
                }

                for (int i = 0; i < this.getChilds().Count; i++)
                {
                    if (!thisChilds[i].Equals(objChilds[i]))
                    {
                        return false;
                    }
                }
            }

            if (!this.getValue().Equals(((AstNode) obj).getValue()))
            {
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            var hashCode = 1252435396;
            hashCode = hashCode * -1521134295 + EqualityComparer<object>.Default.GetHashCode(value);
            hashCode = hashCode * -1521134295 + EqualityComparer<ArrayList>.Default.GetHashCode(childs);
            return hashCode;
        }

        override
            public String ToString()
        {
            string childsJsons = "";
            for (int i = 0; i < childs.Count; i++)
            {
                string[] childsList = childs[i].ToString().Split('\n');
                string formattedChilds = "";
                for (int j = 0; j < childsList.Length; j++)
                {
                    formattedChilds = formattedChilds + "  " + childsList[j] + "\n";
                }

                childsJsons = childsJsons + formattedChilds;
            }

            return value.ToString() + ":{\n" + childsJsons + "\n}";
        }
    }
}