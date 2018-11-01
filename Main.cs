using System;
using System.Collections.Generic;
using Erasystemlevel.Tokenizer;

namespace HelloWorld
{
    class Hello
    {
        static void Main()
        {
            Tokenizer tok = new Tokenizer("/Users/dilsatsalihov/Projects/ERA-system-level/Era-system-level/Tokenizer/text.txt");
            LinkedList<Token> list = tok.Tokenize();
            while(list.Count!=0){
                Console.WriteLine(list.First.Value.ToString());
                list.RemoveFirst();
            }
        }
    }
}