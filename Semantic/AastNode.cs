using Erasystemlevel.Parser;

namespace Erasystemlevel.Semantic
{
    /**
     * Annotated ast node
     */
    public class AastNode : AstNode
    {
        public DataTableEntry dataTableEntry;
        public RoutineTableEntry routineTableEntry;
        public SymbolTableEntry symbolTableEntry;

        public AastNode(AstNode node) : base(node.getValue())
        {
            type = node.GetNodeType();

            foreach (var astNode in node.getChilds())
            {
                childs.Add(astNode);
            }
        }
    }
}