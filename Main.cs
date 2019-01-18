using System;
using Erasystemlevel.Exception;
using Erasystemlevel;
using Erasystemlevel.Utils;

namespace Erasystemlevel
{
    internal static class Run
    {
        private const string CODE_FILE = "code.txt";
        private const bool DEBUG = true;

        private static void Main()
        {
            var compiler = new Compiler(DEBUG);

            string eraAsm;
            try
            {
                eraAsm = compiler.compile(CODE_FILE);
            }
            catch (SyntaxError e)
            {
                e.setProgramText(SourceCodeUtils.getProgramText(CODE_FILE));

                printError(e);
                return;
            }
            catch (SemanticError e)
            {
                printError("Semantic error: ", e);
                return;
            }
            catch (GenerationError e)
            {
                printError("Generation error: ", e);
                return;
            }

            if (!DEBUG)
            {
                Console.WriteLine(eraAsm);
            }
        }

        private static void printError(string description, SystemException error)
        {
            Console.Write(description);
            Console.WriteLine(error);
        }

        private static void printError(SyntaxError error)
        {
            Console.WriteLine(error.verbose());
        }
    }
}