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
            _debug = debug;
        }

        public string compile(string filepath)
        {
            var tokenizer = new Tokenizer(filepath);
            var tokenReader = new TokenReader(tokenizer);

            Parser._debug = false;
            var tree = Parser.ParseUnit(tokenReader);
            printDebug("Parse tree:\n" + tree + "\n");

            var semantic = new SemanticAnalyzer(tree);
            //printDebug("Semantic tree:\n" + tree + "\n");
            
            var codeGen = new CodeGenerator(tree, semantic.symbolTable, semantic.callTable);
            codeGen.generate();

            var asmCode = codeGen.assembly.ToString();
            printDebug("Generated assembly:\n" + asmCode + "\n");

            return asmCode;
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