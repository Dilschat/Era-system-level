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

        public CallTable routines = new CallTable();
        public SymbolTable symbols = new SymbolTable();

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
            }
            else if (node.GetNodeType().Equals(AstNode.NodeType.Variable))
            {
                List<AstNode> childs = node.getChilds();
                AstNode type = childs[0];
                for (int i = 1; i < childs.Count; i++)
                {
                    handleVarDefinitions(childs[i], ((Token) type.getValue()).GetValue());
                }
            }
        }

        private void handleConstDefinition(AstNode node)
        {
            AstNode id = node.getChilds()[0];
            SymbolTableEntry entry = new SymbolTableEntry(id);
            entry.type = "int";
            entry.isConst = true;
            entry.isInitialized = true;
            symbols.Add(((Token) id.getValue()).GetValue(), entry);
        }

        private void handleVarDefinitions(AstNode node, string type)
        {
            List<AstNode> childs = node.getChilds();
            AstNode id = childs[0];
            SymbolTableEntry entry = new SymbolTableEntry(id);
            entry.type = type;
            if (childs.Count > 1 && childs[1].GetNodeType().Equals(AstNode.NodeType.Expression))
            {
                entry.isInitialized = true;
            }

            symbols.Add(((Token) id.getValue()).GetValue(), entry);
        }

        public void addRoutine(AstNode node)
        {
            List<AstNode> childs = node.getChilds();
            CallTableEntry entry = new CallTableEntry(node);
            foreach (var i in childs)
            {
                if (i.GetNodeType().Equals(AstNode.NodeType.Identifier))
                {
                    entry.name = ((Token) i.getValue()).GetValue();
                }
                else if (i.GetNodeType().Equals(AstNode.NodeType.Parameters))
                {
                    foreach (var parameter in i.getChilds())
                    {
                        entry.parameters.Add(handleParameters(parameter, entry));
                    }
                }
                else if (i.GetNodeType().Equals(AstNode.NodeType.Results))
                {
                    foreach (var result in i.getChilds())
                    {
                        entry.results.Add(handleResult(result));
                    }
                }
                else if (i.GetNodeType().Equals(AstNode.NodeType.RoutineBody))

                {
                    entry.hasBody = true;
                    entry.symbols = handleRoutineBody(i, new SymbolTable());
                }

                routines.Add(entry.name, entry);
            }
        }

        private SymbolTable handleRoutineBody(AstNode node, SymbolTable table)
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
                        SymbolTableEntry entry = new SymbolTableEntry(id);
                        entry.type = "int";
                        entry.isConst = true;
                        entry.isInitialized = true;
                        table.Add(((Token) id.getValue()).GetValue(), entry);
                        return table;
                    }
                }
                else if (j.GetNodeType().Equals(AstNode.NodeType.Variable))
                {
                    List<AstNode> childs = j.getChilds();
                    string type = ((Token) childs[0].getValue()).GetValue();
                    for (int i = 1; i < childs.Count; i++)
                    {
                        AstNode id = childs[i].getChilds()[0];
                        SymbolTableEntry entry = new SymbolTableEntry(id);
                        entry.type = type;
                        if (childs.Count > 1 && childs[1].GetNodeType().Equals(AstNode.NodeType.Expression))
                        {
                            entry.isInitialized = true;
                        }

                        table.Add(((Token) id.getValue()).GetValue(), entry);
                        return table;
                    }
                }
                else
                {
                    SymbolTable newTable = handleRoutineBody(j, table);
                    foreach (var t in newTable.Values)
                    {
                        table.Add(t.name, t);
                    }

                    return table;
                }
            }

            return new SymbolTable();
        }

        private string handleParameters(AstNode parameter, CallTableEntry entry)
        {
            string type = ((Token) parameter.getValue()).GetValue();
            string name = ((Token) parameter.getChilds()[0].getValue()).GetValue();
            checkSymbol(name);
            SymbolTableEntry symbolTableEntry = new SymbolTableEntry(parameter.getChilds()[0]);
            symbolTableEntry.type = type;
            symbols.Add(name, symbolTableEntry);
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