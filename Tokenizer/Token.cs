namespace Erasystemlevel.Tokenizer
{
    public class Token
    {
        private readonly TokenType type;
        private readonly string value;
        public enum TokenType { Number, Identifier, Delimiter, Operator, Keyword, Register }
        public Token(TokenType type, string value)
        {
            this.type = type;
            this.value = value;
        }

        public TokenType GetTokenType(){
            return type;
        }

        public string GetValue(){
            return value;
        }

        override
        public string ToString(){
            return "{ " + type + ":" + value + "}";
        }

        public override bool Equals(object obj)
        {
            return obj != null && ToString().Equals(((Token) obj).ToString());
        }

    }
}
