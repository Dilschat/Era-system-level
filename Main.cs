using System;
using Erasystemlevel.Tokenizer;
using Erasystemlevel.Exception;
using Erasystemlevel.Parser;
using Erasystemlevel.Generator;
using Erasystemlevel.Semantic;

namespace Erasystemlevel
{
    internal static class Run
    {
        private const string CodeFile = "code.txt";

        private static void Main()
        {
            var tokenizer = new Tokenizer.Tokenizer(CodeFile);
            var tokenReader = new TokenReader(tokenizer);

            AstNode tree;
            try
            {
                tree = Parser.Parser.ParseUnit(tokenReader);
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