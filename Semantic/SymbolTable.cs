using System.Collections.Generic;
using Erasystemlevel.Parser;
using Erasystemlevel.Tokenizer;

namespace Erasystemlevel.Semantic
{
    public class SymbolTable : Dictionary<string, SymbolTableEntry>
    {
    }

    public class SymbolTableEntry
    {
        public int localId;
        public AstNode node;

        public readonly string name;
        public string type;

        public bool isConst = false;
        public bool isArray = false;

        public bool isInitialized = false;

        public SymbolTableEntry(AstNode node)
        {
            // todo: extend to more data
            this.node = node;
            name = node.getChilds()[0].getValue().ToString();

            resolveType();
        }

        public SymbolTableEntry(AstNode node, int id) : this(node)
        {
            localId = id;
        }

        private void resolveType()
        {
            if (node.GetNodeType().Equals(AstNode.NodeType.Constant))
            {
                type = "int";
            }
            else if (node.GetNodeType().Equals(AstNode.NodeType.Variable))
            {
                var childs = node.getChilds();

                type = ((Token) childs[0].getValue()).GetValue();
            }
        }

        private void resolveFlags()
        {
            if (node.GetNodeType().Equals(AstNode.NodeType.Constant))
            {
                isConst = true;
                isInitialized = true;
            }
            else if (node.GetNodeType().Equals(AstNode.NodeType.Variable))
            {
                if (node.getChilds().Count > 1)
                {
                    isInitialized = true;
                }
            }
        }

        public override string ToString()
        {
            return name;
        }
    }
}