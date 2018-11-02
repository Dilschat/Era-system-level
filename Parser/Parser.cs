using System;
using System.Collections.Generic;
using Erasystemlevel.Exception;
using Erasystemlevel.Tokenizer;

namespace Erasystemlevel.Parser
{
    public class Parser
    {
        private LinkedList<Token> tokens;
        public Parser(LinkedList<Token> tokens)
        {
            this.tokens = tokens;
        }

        private AstNode ParseUnit(){
            try{
                AstNode node = parseCode();
                return node;
            }catch(SyntaxError e){

            }
            try{
                AstNode node = parseData();
                return node;
            }catch(SyntaxError e){

            }
            try
            {
                AstNode node = parseModule();
                return node;
            }
            catch (SyntaxError e)
            {

            }
            try
            {
                AstNode node = parseRoutine();
                return node;
            }
            catch (SyntaxError e)
            {

            }

        }

        private AstNode parseCode(){
            Token nextToken = tokens.First.Value;
            tokens.RemoveFirst();
            if(!(nextToken.GetTokenType() == Token.TokenType.Keyword && nextToken.GetValue().Equals("code"))){
                throw new SyntaxError("Cant parse code");
            }
            nextToken = tokens.First.Value;

            while (nextToken.GetTokenType() == Token.TokenType.Keyword && nextToken.GetValue().Equals("end")){
                
            }

        }



    }
}
