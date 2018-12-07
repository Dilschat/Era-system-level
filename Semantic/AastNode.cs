using Erasystemlevel.Parser;
using Erasystemlevel.Semantic;

namespace EraSystemLevel.Semantic
{
    /**
     * Annotated ast node
     */
    public class AastNode: AstNode
    {
        public DataTableEntry dataTableEntry;
        public CallTableEntry2 callTableEntry;
        public SymbolTableEntry2 symbolTableEntry;
        
        public AastNode(object value) : base(value)
        {
        }
    }
}