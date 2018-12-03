using System;
using System.Collections.Generic;
using Erasystemlevel.Tokenizer;
using System.Text.RegularExpressions;
using System.IO;
using System.Text;
using Erasystemlevel.Parser;
namespace HelloWorld
{
    class Hello
    {
        static void Main()
        {

            Tokenizer t = new Tokenizer("text1.txt");
            //Parser parser = new Parser(t);
            //AstNode n = parser.ParseUnit();
            //Console.WriteLine(n.ToString());
                         //Tokenizer tok = new Tokenizer("/Users/dilsatsalihov/Projects/ERA-system-level/Era-system-level/Tokenizer/text.txt");
            //LinkedList<Token> list = tok.Tokenize();
            //while(list.Count!=0){
            //Console.WriteLine(list.First.Value.ToString());
            //list.RemoveFirst();
            // }
        }
    }
}