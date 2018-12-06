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

        
        public string ToJsonString(){
            return "{ " + type + ":" + value + "}";
        }

        public override string ToString()
        {
            return value;
        }

        public override bool Equals(object obj)
        {
            return obj != null && ToJsonString().Equals(((Token) obj).ToJsonString());
        }

    }
}
