using System;
namespace Erasystemlevel.Tokenizer
{
    public class Token
    {
        private TokenType type;
        private String value;
        public enum TokenType { Number, Identifier, Delimiter, Operator, Keyword, Register };
        public Token(TokenType type, String value )
        {
            this.type = type;
            this.value = value;
        }

        public TokenType GetTokenType(){
            return type;
        }

        public String GetValue(){
            return value;
        }

        override
        public String ToString(){
            return "{ " + type + ":" + value + "}";
        }
    }
}
