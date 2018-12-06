using System;
using Erasystemlevel.Exception;

namespace EraSystemLevel
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

            Console.WriteLine(eraAsm);
        }
    }
}