using System;
using Erasystemlevel.Generator;
using Erasystemlevel.Parser;
using Erasystemlevel.Semantic;

namespace Erasystemlevel
{
    public class Compiler
    {
        private readonly bool _debug;

        public Tokenizer.Tokenizer tokenizer;
        public TokenReader tokenReader;
        public AstNode astTree;

        public Compiler(bool debug)
        {
            _debug = debug;
        }

        public string compile(string filepath)
        {
            tokenizer = new Tokenizer.Tokenizer(filepath);
            tokenReader = new TokenReader(tokenizer);

            Parser.Parser._debug = false;
            astTree = Parser.Parser.ParseUnit(tokenReader);
            printDebug("Parse tree:\n" + astTree + "\n");

            var semantic = new SemanticAnalyzer(astTree);
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
            printDebug("Generated assembly:\n" + asmCode);

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