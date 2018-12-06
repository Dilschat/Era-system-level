using System;
using Erasystemlevel.Generator;
using Erasystemlevel.Parser;
using Erasystemlevel.Semantic;
using Erasystemlevel.Tokenizer;

namespace EraSystemLevel
{
    public class Compiler
    {
        private readonly bool _debug;

        public Compiler(bool debug)
        {
            this._debug = debug;
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
            if (_debug)
            {
                Console.WriteLine(line, obj);
            }
        }

        private void printDebug(string line)
        {
            if (_debug)
            {
                Console.WriteLine(line);
            }
        }
    }
}