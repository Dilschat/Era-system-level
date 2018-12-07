using System;
using Erasystemlevel.Generator;
using Erasystemlevel.Parser;
using Erasystemlevel.Semantic;

namespace Erasystemlevel
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
            var tokenizer = new Tokenizer.Tokenizer(filepath);
            var tokenReader = new TokenReader(tokenizer);

            Parser.Parser._debug = false;
            var tree = Parser.Parser.ParseUnit(tokenReader);
            printDebug("Parse tree:\n" + tree + "\n");

            var semantic = new SemanticAnalyzer2(tree);
            semantic.analyze();

            var aTree = semantic.annotatedTree;
            printDebug("Semantic tree:\n" + aTree + "\n");

            //var semantic = new SemanticAnalyzer(tree);
            //semantic.generateTables();
            //semantic.analyze();
            //printDebug("Semantic tree:\n" + tree + "\n");

            var codeGen = new CodeGenerator(aTree, semantic.moduleTable, semantic.dataTable);
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