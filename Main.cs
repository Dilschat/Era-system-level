using System;
using Erasystemlevel.Tokenizer;
using Erasystemlevel.Exception;
using Erasystemlevel.Parser;
using EraSystemLevel.Generator;
using EraSystemLevel.Semantic;

namespace EraSystemLevel
{
    class Run
    {
        private const string CodeFile = "code.txt";

        static void Main()
        {
            var tokenizer = new Tokenizer(CodeFile);
            var tokenReader = new TokenReader(tokenizer);

            AstNode tree;
            try
            {
                tree = Parser.ParseUnit(tokenReader);
            }
            catch (SyntaxError e)
            {
                Console.WriteLine("Syntax error:", e);
                return;
            }

            Console.WriteLine("Parse tree:", tree.ToString());

            SemanticAnalyzer semantic;
            try
            {
                semantic = new SemanticAnalyzer(tree);
            }
            catch (SemanticError e)
            {
                Console.WriteLine("Syntax error:", e);
                return;
            }

            var codeGen = new CodeGenerator(tree, semantic.symbolTable, semantic.callTable);
            var assembly = codeGen.assembly;

            Console.WriteLine("Generated assembly:");
            Console.WriteLine(assembly.ToString());
        }
    }
}