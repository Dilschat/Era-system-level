using System;
using Erasystemlevel.Parser;
using Erasystemlevel.Tokenizer;
using EraSystemLevel.Generator;
using EraSystemLevel.Semantic;

namespace EraSystemLevel
{
    public class Compiler
    {
        private bool debug;

        public Compiler(bool debug)
        {
            this.debug = debug;
        }

        public string compile(string filepath)
        {
            var tokenizer = new Tokenizer(filepath);
            var tokenReader = new TokenReader(tokenizer);

            var tree = Parser.ParseUnit(tokenReader);
            printDebug("Parse tree:", tree.ToString());

            var semantic = new SemanticAnalyzer(tree);
            var codeGen = new CodeGenerator(tree, semantic.symbolTable, semantic.callTable);

            var assembly = codeGen.assembly;
            printDebug("Generated assembly:\n", assembly);

            return assembly.ToString();
        }

        private void printDebug(string line, object obj)
        {
            if (debug)
            {
                Console.WriteLine(line, obj);
            }
        }

        private void printDebug(string line)
        {
            if (debug)
            {
                Console.WriteLine(line);
            }
        }
    }
}