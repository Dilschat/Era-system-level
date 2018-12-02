using Erasystemlevel.Parser;
using Erasystemlevel.Tokenizer;
using NUnit.Framework;
using System.Collections;
using System;
namespace Erasystemlevel.Tests
{
    [TestFixture()]
    public class NUnitTestClass
    {
        [Test()]
        public void TokenReadingTestCase()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer("/Users/dilsatsalihov/Projects/ERA-system-level/Era-system-level/Tests/test_reading.txt");
            Parser.TokenReader reader = new Parser.TokenReader(tokenizer);
            Assert.AreEqual(reader.readNextToken().ToString(), new Token(Token.TokenType.Number, "1").ToString());
            Assert.AreEqual(reader.readNextToken().ToString(), new Token(Token.TokenType.Number, "2").ToString());
            Assert.AreEqual(reader.readNextToken().ToString(), new Token(Token.TokenType.Number, "3").ToString());


        }

        [Test()]
        public void TokenSavedReadingTestCase()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer("/Users/dilsatsalihov/Projects/ERA-system-level/Era-system-level/Tests/test_reading.txt");
            Parser.TokenReader reader = new Parser.TokenReader(tokenizer);
            Assert.AreEqual(reader.readNextToken().ToString(), new Token(Token.TokenType.Number, "1").ToString());
            Assert.AreEqual(reader.readNextToken().ToString(), new Token(Token.TokenType.Number, "2").ToString());
            Assert.AreEqual(reader.readNextToken().ToString(), new Token(Token.TokenType.Number, "3").ToString());
            reader.saveReadedTokens();
            Assert.AreEqual(reader.readNextToken().ToString(), new Token(Token.TokenType.Number, "1").ToString());
            Assert.AreEqual(reader.readNextToken().ToString(), new Token(Token.TokenType.Number, "2").ToString());
            Assert.AreEqual(reader.readNextToken().ToString(), new Token(Token.TokenType.Number, "3").ToString());


        }

        [Test()]
        public void parseBreakTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer("/Users/dilsatsalihov/Projects/ERA-system-level/Era-system-level/Tests/Break.txt");
            Parser.TokenReader reader = new Parser.TokenReader(tokenizer);
            AstNode breakNode = Parser.Parser.parseBreak(reader);
            Assert.AreEqual(((Token)breakNode.getValue()), new Token(Token.TokenType.Keyword, "break"));
            Assert.AreEqual(new ArrayList(), breakNode.getChilds());
            Assert.AreEqual(reader.readNextToken(),new Token(Token.TokenType.Delimiter, ";"));
        }

        [Test()]
        public void parseIdentifierTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer("/Users/dilsatsalihov/Projects/ERA-system-level/Era-system-level/Tests/Identifier.txt");
            Parser.TokenReader reader = new Parser.TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseIdentifier(reader);
            Assert.AreEqual(((Token)node.getValue()), new Token(Token.TokenType.Identifier, "player"));
            Assert.AreEqual(new ArrayList(), node.getChilds());
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
        }

        [Test()]
        public void parseGoToTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer("/Users/dilsatsalihov/Projects/ERA-system-level/Era-system-level/Tests/goto.txt");
            Parser.TokenReader reader = new Parser.TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseGoto(reader);
            Assert.AreEqual(((Token)node.getValue()), new Token(Token.TokenType.Keyword, "goto"));
            AstNode a = new AstNode("");
            AstNode b = new AstNode("");
            a.Equals(b);

            ArrayList childs = new ArrayList();
            childs.Add(new AstNode(new Token(Token.TokenType.Identifier, "player")));
            Assert.AreEqual(childs, node.getChilds());
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
        }
    }


    
}
