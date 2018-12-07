using System.Collections;
using System.Collections.Generic;
using Erasystemlevel.Exception;
using Erasystemlevel.Parser;
using Erasystemlevel.Tokenizer;

namespace Erasystemlevel.Semantic
{
    public class ModuleTable : Dictionary<string, Module>
    {
        public void Add(Module m)
        {
            Add(m.name, m);
        }
    }

    public class Module
    {
        public AstNode node;
        public readonly string name;

        public int staticBase;

        public CallTable2 routines = new CallTable2();
        public SymbolTable2 symbols = new SymbolTable2();

        public Module(AstNode node)
        {
            this.node = node;
            name = node.getValue().ToString();
        }

        public Module(string name)
        {
            this.name = name;
        }

        public void addVariable(AstNode node)
        {
            if (node.GetNodeType().Equals(AstNode.NodeType.Constant))
            {
                List<AstNode> consts = node.getChilds();
                foreach (var i in consts)
                {
                    handleConstDefinition(i);
                }
            }else if(node.GetNodeType().Equals(AstNode.NodeType.Variable))
            {
                List<AstNode> childs = node.getChilds();
                AstNode type = childs[0] ;
                for (int i = 1; i < childs.Count; i++)
                {
                    handleVarDefinitions(childs[i], ((Token)type.getValue()).GetValue());
                }
            }
        }

        private void handleConstDefinition(AstNode node)
        {
            AstNode id = node.getChilds()[0];
            SymbolTableEntry2 entry2 = new SymbolTableEntry2(id);
            entry2.type = "int";
            entry2.isConst = true;
            entry2.isInitialized = true;
            symbols.Add(((Token)id.getValue()).GetValue(),entry2);
        }

        private void handleVarDefinitions(AstNode node, string type)
        {
            List<AstNode> childs = node.getChilds();
            AstNode id = childs[0];
            SymbolTableEntry2 entry2 = new SymbolTableEntry2(id);
            entry2.type = type;
            if (childs.Count > 1 && childs[1].GetNodeType().Equals(AstNode.NodeType.Expression))
            {
                entry2.isInitialized = true;
            }
            symbols.Add(((Token)id.getValue()).GetValue(),entry2);
        }

        public void addRoutine(AstNode node)
        {
            List<AstNode> childs = node.getChilds();
            CallTableEntry2 entry2 = new CallTableEntry2(node);
            foreach (var i in childs)
            {
                if (i.GetNodeType().Equals(AstNode.NodeType.Identifier))
                {
                    entry2.name = ((Token) i.getValue()).GetValue();
                }else if (i.GetNodeType().Equals(AstNode.NodeType.Parameters))
                {
                    foreach (var parameter in i.getChilds())
                    {
                        entry2.parameters.Add(handleParameters(parameter,entry2));
                    }
                }else if (i.GetNodeType().Equals(AstNode.NodeType.Results))
                {
                    foreach (var result in i.getChilds())
                    {
                        entry2.results.Add(handleResult(result));
                    }
                    
                }else if (i.GetNodeType().Equals(AstNode.NodeType.RoutineBody))

                {
                    entry2.hasBody = true;
                    entry2.symbols = handleRoutineBody(i,new SymbolTable2());

                }
                routines.Add(entry2.name,entry2);
                
            }
        }

        private SymbolTable2 handleRoutineBody(AstNode node, SymbolTable2 table)
        {
            List<AstNode> childes = node.getChilds();
            foreach (var j in childes)
            {
                    if (j.GetNodeType().Equals(AstNode.NodeType.Constant))
                    {
                        List<AstNode> consts = j.getChilds();
                        foreach (var constant in consts)
                        {
                            AstNode id = constant.getChilds()[0];
                            SymbolTableEntry2 entry2 = new SymbolTableEntry2(id);
                            entry2.type = "int";
                            entry2.isConst = true;
                            entry2.isInitialized = true;
                            table.Add(((Token)id.getValue()).GetValue(),entry2);
                            return table;
                        }
                                    
                    }
                    else if (j.GetNodeType().Equals(AstNode.NodeType.Variable))
                    {
                        List<AstNode> childs = j.getChilds();
                        string type = ((Token) childs[0].getValue()).GetValue();
                        for (int i = 1; i<childs.Count; i++)
                        {
                            AstNode id = childs[i].getChilds()[0];
                            SymbolTableEntry2 entry2 = new SymbolTableEntry2(id);
                            entry2.type =type;
                            if (childs.Count > 1 && childs[1].GetNodeType().Equals(AstNode.NodeType.Expression))
                            {
                                entry2.isInitialized = true;
                            }
                            table.Add(((Token)id.getValue()).GetValue(),entry2);
                            return table;
                        }
                        
                        
                    }
                    else
                    {
                        SymbolTable2 newTable = handleRoutineBody(j, table);
                        foreach (var t in newTable.Values)
                        {
                            table.Add(t.name, t);
                        }

                        return table;

                    }
                
                
            }
            return new SymbolTable2();

        }

        private string handleParameters(AstNode parameter, CallTableEntry2 entry2)
        {
            string type = ((Token) parameter.getValue()).GetValue();
            string name = ((Token) parameter.getChilds()[0].getValue()).GetValue();
            checkSymbol(name);
            SymbolTableEntry2 symbolTableEntry2 = new SymbolTableEntry2(parameter.getChilds()[0]);
            symbolTableEntry2.type = type;
            symbols.Add(name,symbolTableEntry2);
            return type;



        }

        private string handleResult(AstNode result)
        {
            return ((Token) result.getChilds()[0].getValue()).GetValue();
        }

        private void checkSymbol(string name)
        {
            if (symbols.ContainsKey(name))
            {
                throw new SemanticError("Nonunique key");
            }
        }
    }
}