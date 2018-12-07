using System;
using Erasystemlevel.Exception;
using Erasystemlevel;

namespace Erasystemlevel
{
    internal static class Run
    {
        private const string CodeFile = "code.txt";

        private static void Main()
        {
            var compiler = new Compiler(true);

            string eraAsm;
            try
            {
                eraAsm = compiler.compile(CodeFile);
            }
            catch (SyntaxError e)
            {
                printError("Syntax error: ", e);
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

            Console.WriteLine(eraAsm);
        }

        public static void printError(string description, SystemException error)
        {
            Console.Write(description);
            Console.WriteLine(error);
        }
    }
}