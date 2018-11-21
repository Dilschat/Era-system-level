using System;
using System.Collections.Generic;
using Erasystemlevel.Tokenizer;

namespace Erasystemlevel.Parser
{
    public class Program : AstNode 
    {
        public Program (){
            base.value = "Program";
        }

        public static Program ParseProgramm(LinkedList<Token> tokens)
        {
            Program program = new Program();
            program.addChild(Unit.parseUnit());
            return program;
        }
    }


}
