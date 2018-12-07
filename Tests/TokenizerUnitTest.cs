using Erasystemlevel.Parser;
using Erasystemlevel.Tokenizer;
using NUnit.Framework;

namespace Erasystemlevel.Tests
{
    public class TokenizerUnitTest
    {
        [Test]
        public void TokenReadingTestCase()
        {
            var tokenizer = new Tokenizer.Tokenizer(getTestFilePath("test_reading.txt"));
            var reader = new TokenReader(tokenizer);

            Assert.AreEqual(reader.readNextToken().ToString(), new Token(Token.TokenType.Number, "1").ToString());
            Assert.AreEqual(reader.readNextToken().ToString(), new Token(Token.TokenType.Number, "2").ToString());
            Assert.AreEqual(reader.readNextToken().ToString(), new Token(Token.TokenType.Number, "3").ToString());
        }

        [Test]
        public void TokenSavedReadingTestCase()
        {
            var tokenizer = new Tokenizer.Tokenizer(getTestFilePath("test_reading.txt"));
            var reader = new TokenReader(tokenizer);
            Assert.AreEqual(reader.readNextToken().ToString(), new Token(Token.TokenType.Number, "1").ToString());
            Assert.AreEqual(reader.readNextToken().ToString(), new Token(Token.TokenType.Number, "2").ToString());
            Assert.AreEqual(reader.readNextToken().ToString(), new Token(Token.TokenType.Number, "3").ToString());
            reader.saveReadTokens();
            Assert.AreEqual(reader.readNextToken().ToString(), new Token(Token.TokenType.Number, "1").ToString());
            Assert.AreEqual(reader.readNextToken().ToString(), new Token(Token.TokenType.Number, "2").ToString());
            Assert.AreEqual(reader.readNextToken().ToString(), new Token(Token.TokenType.Number, "3").ToString());
        }

        [Test]
        public void CommentsIgnored()
        {
            var tokenizer = new Tokenizer.Tokenizer(getTestFilePath("comments.txt"));
            var reader = new TokenReader(tokenizer);

            var firstToken = reader.readNextToken();
            Assert.NotNull(firstToken);
            Assert.AreEqual(new Token(Token.TokenType.Keyword, "code").ToString(), firstToken.ToString());
            Assert.Null(reader.readNextToken());
        }

        private static string getTestFilePath(string fileName)
        {
            return TestUtils.getTestFilePath(fileName);
        }
    }
}