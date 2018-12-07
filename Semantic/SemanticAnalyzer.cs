using System;
using System.Collections;
using System.Collections.Generic;
using Erasystemlevel.Exception;
using Erasystemlevel.Parser;
using Erasystemlevel.Tokenizer;
using NUnit.Framework.Constraints;

namespace Erasystemlevel.Semantic
{
    public class SemanticAnalyzer
    {
        public readonly SymbolTable symbolTable;
        public readonly CallTable callTable;
        private readonly AstNode tree;
        public readonly string  unitName;
        private static Dictionary<string, SemanticAnalyzer> units = new Dictionary<string, SemanticAnalyzer>();
        
        public SemanticAnalyzer(AstNode tree)
        {
            callTable = new CallTable();
            symbolTable = new SymbolTable();
            this.tree = tree;
            if (!tree.GetNodeType().Equals(AstNode.NodeType.Code))
            {
                unitName = ((Token) tree.getChilds()[0].getValue()).GetValue();
            }

        }

        public void generateTables()
        {
            AnalyzeTree(tree);
        }
        private void AnalyzeTree(AstNode tree)
        {
            var childes = tree.getChilds();
            if(tree.GetNodeType().Equals(AstNode.NodeType.Call))
            {
                
            }
            else if (tree.GetNodeType().Equals(AstNode.NodeType.Routine))
            {
                var entry = new CallTableEntry();
                foreach(var i in childes)
                {
                    if (i.GetNodeType().Equals(AstNode.NodeType.Attribute))
                    {
                        entry.functionType= AnalyzeAttribute(i);
                    }else if (i.GetNodeType().Equals(AstNode.NodeType.Identifier))
                    {
                        entry.functionName= AnalyzeName(i);

                    }
                    else if (i.GetNodeType().Equals(AstNode.NodeType.Parameters))
                    {
                        entry.parameters= AnalyzeParameters(i);

                    }
                    else if (i.GetNodeType().Equals(AstNode.NodeType.Results))
                    {
                        entry.results= AnalyzeResults(i);

                    }
                    else if (i.GetNodeType().Equals(AstNode.NodeType.RoutineBody))
                    {
                        entry.hasBody = true;
                        AnalyzeSymbols(i);

                    }
                    checkCallEntry(entry);
                    callTable.Add(entry.functionName, entry);
                }
            }
            else
            {
                AnalyzeSymbols(tree);
                foreach(var i in childes)
                {
                    AnalyzeTree(i);
                }
            }
        }

        private void AnalyzeSymbols(AstNode tree)
        {
            if (tree.GetNodeType().Equals(AstNode.NodeType.ConstDefinition))
            {
                AstNode curNode = tree.getChilds()[0];
                SymbolTableEntry entry = new SymbolTableEntry
                {
                    type = "int", name = ((Token) curNode.getValue()).GetValue()
                };
                if (curNode.GetNodeType().Equals(AstNode.NodeType.Literal))
                {
                    checkNumberType(curNode, entry);
                }
                checkSymbolEntry(entry);
                symbolTable.Add(entry.name, entry);


            }else if(tree.GetNodeType().Equals(AstNode.NodeType.Variable))
            {
                var childes = tree.getChilds();
                var type = ((Token) childes[0].getValue()).GetValue();
                for (var i = 1; i < childes.Count; i++)
                {
                    var entry = new SymbolTableEntry
                    {
                        type = type, name = ((Token) childes[i].getChilds()[0].getValue()).GetValue()

                    };
                    checkNumberType(childes[i].getChilds()[1],entry);
                    checkSymbolEntry(entry);
                    symbolTable.Add(entry.name, entry);
                    
                }
                

            
            }
            else if (tree.GetNodeType().Equals(AstNode.NodeType.Assignment))
            {
                List<AstNode> childes = tree.getChilds();
                AstNode literal = childes[1];
                string name = ((Token) childes[0].getValue()).GetValue();
                if (literal.GetNodeType().Equals(AstNode.NodeType.Literal))
                {
                    checkNumberType(literal,symbolTable[name]);
                }
            }
            else if (tree.GetNodeType().Equals(AstNode.NodeType.VariableReference))
            {
                checkSymbolDeclaration(tree);
            }
        }



        private static string AnalyzeName(AstNode astNode)
        {
            return astNode.getValue().ToString();
        }

        private ArrayList AnalyzeParameters(AstNode astNode)
        {
            
            var childes = astNode.getChilds();
            var parameters = new ArrayList();
            foreach (var i in childes)
            {
                parameters.Add(((Token)i.getChilds()[0].getChilds()[0].getValue()).GetValue());
            }
             
            return parameters;
        }

        private static ArrayList AnalyzeResults(AstNode astNode)
        {
            var childes = astNode.getChilds();
            var results = new ArrayList();
            foreach (var i in childes)
            {
                results.Add(((Token)i.getValue()).GetValue());
            }
            return results;
        }

        private static string AnalyzeAttribute(AstNode astNode)
        {
            return astNode.getValue().ToString();
        }

        private void checkSymbolEntry(SymbolTableEntry entry)
        {

            if (!entry.Equals(symbolTable[entry.name]))
            {
                throw new SemanticError("Type error for:"+ entry.ToString());
            }
        }
        
        private void checkCallEntry(CallTableEntry entry)
        {
            if (entry.functionType.Equals("start")&& callTable.ContainsKey(entry.functionName))
            {
                throw new SemanticError("Routine error for:"+ entry.ToString());
            }

            if (entry.functionType.Equals("start") && entry.hasBody)
            {
                throw new SemanticError("Routine error for:"+ entry.ToString());
            }
            if (entry.functionType.Equals("entry") && !entry.hasBody)
            {
                throw new SemanticError("Routine error for:"+ entry.ToString());
            }
            

        }
        private void checkSymbolDeclaration(AstNode astNode)
        {
            if (symbolTable.ContainsKey(((Token) tree.getValue()).GetValue()))
            {
                throw new SemanticError("Variable is not declared:"+ ((Token) tree.getValue()).GetValue());
            }
        }

        private void checkNumberType( AstNode curNode, SymbolTableEntry entry)
        {
            string number = ((Token) curNode.getValue()).GetValue();
            int a;
            if (entry.type.Equals("int")&&!int.TryParse(number,out a))
            {
                throw new SemanticError(entry.name+" is not int");
            }
            short b;
            if (entry.type.Equals("short")&&!short.TryParse(number,out b))
            {
                throw new SemanticError(entry.name+" is not short");
            }
            byte c;
            if (entry.type.Equals("byte")&&!byte.TryParse(number,out c));
            {
                throw new SemanticError(entry.name+" is not byte");
            }
        }
    }
}