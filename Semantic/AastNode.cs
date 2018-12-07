using Erasystemlevel.Parser;

namespace Erasystemlevel.Semantic
{
    /**
     * Annotated ast node
     */
    public class AastNode : AstNode
    {
        public DataTableEntry dataTableEntry;
        public CallTableEntry callTableEntry;
        public SymbolTableEntry symbolTableEntry;

        public AastNode(object value) : base(value)
        {
        }
    }
}