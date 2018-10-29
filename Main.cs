using System;
using Erasystemlevel.Tokenizer;

namespace HelloWorld
{
    class Hello
    {
        static void Main()
        {
            Tokenizer tok = new Tokenizer("/Users/dilsatsalihov/Projects/ERA-system-level/Era-system-level/Tokenizer/text.txt");
            tok.tokenize();
        }
    }
}