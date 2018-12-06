using System;
using Erasystemlevel.Exception;
using EraSystemLevel;

namespace Erasystemlevel
{
    class Run
    {
        private const string CodeFile = "code.txt";

        static void Main()
        {
            var compiler = new Compiler(true);

            string eraAsm;
            try
            {
                eraAsm = compiler.compile(CodeFile);
            }
            catch (SyntaxError e)
            {
                Console.WriteLine("Syntax error:", e);
                return;
            }
            catch (SemanticError e)
            {
                Console.WriteLine("Semantic error:", e);
                return;
            }
            catch (GenerationError e)
            {
                Console.WriteLine("Generation error:", e);
                return;
            }

            Console.WriteLine(eraAsm);
        }
    }
}