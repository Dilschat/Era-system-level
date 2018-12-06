using System.Collections;
using System.Collections.Generic;

namespace Erasystemlevel.Parser
{
    public class AstNode
    {
        private object value;
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

        public AstNode(NodeType type, string value)
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

        public AstNode(object value)
        {
            this.value = value;
        }

        public NodeType GetNodeType()
        {
            return type;
        }

        public void setValue(object value)
        {
            this.value = value;
        }

        public object getValue()
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

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var thisChilds = getChilds();
            var objChilds = ((AstNode) obj).getChilds();

            if (thisChilds.Count != (objChilds.Count))
            {
                return false;
            }

            for (var i = 0; i < getChilds().Count; i++)
            {
                if (!thisChilds[i].Equals(objChilds[i]))
                {
                    return false;
                }
            }

            return getValue().Equals(((AstNode) obj).getValue());
        }

        public override int GetHashCode()
        {
            var hashCode = 1252435396;
            hashCode = hashCode * -1521134295 + EqualityComparer<object>.Default.GetHashCode(value);
            hashCode = hashCode * -1521134295 + EqualityComparer<ArrayList>.Default.GetHashCode(childs);
            return hashCode;
        }

        override
            public string ToString()
        {
            var childsJsons = "";
            for (var i = 0; i < childs.Count; i++)
            {
                var childsList = childs[i].ToString().Split('\n');
                var formattedChilds = "";
                for (var j = 0; j < childsList.Length; j++)
                {
                    formattedChilds = formattedChilds + "  " + childsList[j] + "\n";
                }

                childsJsons = childsJsons + formattedChilds;
            }

            return value + ":{\n" + childsJsons + "\n}";
        }
    }
}