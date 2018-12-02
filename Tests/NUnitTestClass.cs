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

        [Test()]
        public void parseSwapTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer("/Users/dilsatsalihov/Projects/ERA-system-level/Era-system-level/Tests/swap.txt");
            Parser.TokenReader reader = new Parser.TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseSwap(reader);
            Assert.AreEqual(((Token)node.getValue()), new Token(Token.TokenType.Operator, "man"));
            ArrayList childs = new ArrayList();
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, "<=>"));
            childs.Add(new AstNode(new Token(Token.TokenType.Operator, "woman")));
            Assert.AreEqual(childs, node.getChilds());
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
        }

        [Test()]
        public void parseLoopBodyTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer("/Users/dilsatsalihov/Projects/ERA-system-level/Era-system-level/Tests/loopBody.txt");
            Parser.TokenReader reader = new Parser.TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseLoopBody(reader);
            Assert.AreEqual(((Token)node.getValue()), new Token(Token.TokenType.Keyword, "loop"));
            ArrayList childs = new ArrayList();
            childs.Add(new AstNode(new Token(Token.TokenType.Keyword, "if")));
            Assert.AreEqual(childs, node.getChilds());
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Keyword, "end"));

        }

        [Test()]
        public void parseWhileTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer("/Users/dilsatsalihov/Projects/ERA-system-level/Era-system-level/Tests/while.txt");
            Parser.TokenReader reader = new Parser.TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseWhile(reader);
            Assert.AreEqual(((Token)node.getValue()), new Token(Token.TokenType.Keyword, "while"));
            ArrayList childs = new ArrayList();
            childs.Add(new AstNode(new Token(Token.TokenType.Operator, ">")));
            childs.Add(new AstNode(new Token(Token.TokenType.Keyword, "loop")));
            Assert.AreEqual(childs, node.getChilds());
        }

        [Test()]
        public void parseForTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer("/Users/dilsatsalihov/Projects/ERA-system-level/Era-system-level/Tests/for.txt");
            Parser.TokenReader reader = new Parser.TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseFor(reader);
            Assert.AreEqual(((Token)node.getValue()), new Token(Token.TokenType.Keyword, "for"));
            ArrayList childs = new ArrayList();
            childs.Add(new AstNode(new Token(Token.TokenType.Identifier, "a")));
            childs.Add(new AstNode(new Token(Token.TokenType.Keyword, "loop")));
            Assert.AreEqual(childs, node.getChilds());
        }

        [Test()]
        public void parseExtensionStatementTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer("/Users/dilsatsalihov/Projects/ERA-system-level/Era-system-level/Tests/extensionStatement.txt");
            Parser.TokenReader reader = new Parser.TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseExtensionStatement(reader);
            Assert.AreEqual(((Token)node.getValue()), new Token(Token.TokenType.Keyword, "if"));
            ArrayList childs = new ArrayList();
            childs.Add(new AstNode(new Token(Token.TokenType.Keyword, ">")));
            childs.Add(new AstNode(new Token(Token.TokenType.Keyword, "break")));
            Assert.AreEqual(childs, node.getChilds());
        }

        [Test()]
        public void parseExpressionTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer("/Users/dilsatsalihov/Projects/ERA-system-level/Era-system-level/Tests/expression.txt");
            Parser.TokenReader reader = new Parser.TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseExpression(reader);
            Assert.AreEqual(((Token)node.getValue()), new Token(Token.TokenType.Keyword, "this"));
            ArrayList childs = new ArrayList();
            childs.Add(new AstNode(new Token(Token.TokenType.Number, "12")));
            Assert.AreEqual(childs, node.getChilds());
        }

        [Test()]
        public void parsePrimaryTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer("/Users/dilsatsalihov/Projects/ERA-system-level/Era-system-level/Tests/expression.txt");
            Parser.TokenReader reader = new Parser.TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseExpression(reader);
            Assert.AreEqual(((Token)node.getValue()), new Token(Token.TokenType.Keyword, "this"));
            ArrayList childs = new ArrayList();
            childs.Add(new AstNode(new Token(Token.TokenType.Number, "12")));
            Assert.AreEqual(childs, node.getChilds());
        }


    }



}
