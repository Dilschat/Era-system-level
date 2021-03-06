﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Erasystemlevel.Parser
{
    public class AstNode
    {
        protected object value;
        protected NodeType type;
        protected readonly List<AstNode> childs = new List<AstNode>();

        public enum NodeType
        {
            None,
            Unit,
            Code,
            Data,
            Routine,
            Module,
            Variable,
            Constant,
            AsmComment,
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
            VarDefinition,
            From,
            To,
            Step,
            Format
        }

        public void SetNodeType(NodeType type)
        {
            this.type = type;
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
            return value;
        }

        public void addChild(AstNode node)
        {
            childs.Add(node);
        }

        public List<AstNode> getChilds()
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

            if (thisChilds.Count != objChilds.Count)
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
            hashCode = hashCode * -1521134295 + EqualityComparer<List<AstNode>>.Default.GetHashCode(childs);
            return hashCode;
        }

        public override string ToString()
        {

            var currentName = type.ToString();
            if (value.ToString().Length > 0 && !currentName.Equals(value.ToString()))
            {
                currentName += "(" + value + ")";
            }
            
            var childrenJson = "";
            foreach (var i in childs)
            {
                var childrenList = i.ToString().Split('\n');
                var formattedChildren = childrenList.Aggregate("", (current, j) => current + "  " + j + "\n");

                childrenJson = childrenJson + formattedChildren;
            }

            var currentJson = currentName;
            if (childrenJson.Length > 0)
            {
                currentJson += ": {\n" + childrenJson + "}";
            }

            return currentJson;
        }
    }
}