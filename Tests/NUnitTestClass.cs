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
            Console.WriteLine(System.IO.Directory.GetCurrentDirectory());
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer("Tests/test_reading.txt");
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
        public void parseRegisterTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer("/Users/dilsatsalihov/Projects/ERA-system-level/Era-system-level/Tests/register.txt");
            Parser.TokenReader reader = new Parser.TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseRegister(reader);
            Assert.AreEqual(((Token)node.getValue()), new Token(Token.TokenType.Register, "R1"));
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
        }

        [Test()]
        public void parseArrayAndDataTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer("/Users/dilsatsalihov/Projects/ERA-system-level/Era-system-level/Tests/array_and_data_elements.txt");
            Parser.TokenReader reader = new Parser.TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseArrayElement(reader);
            Assert.AreEqual(((Token)node.getValue()), new Token(Token.TokenType.Identifier, "bla"));
            ArrayList childs = new ArrayList();
            childs.Add(new AstNode(new Token(Token.TokenType.Number, "2")));
            ArrayList curChilds = node.getChilds();
            Assert.AreEqual(childs, curChilds);
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));

            tokenizer = new Tokenizer.Tokenizer("/Users/dilsatsalihov/Projects/ERA-system-level/Era-system-level/Tests/array_and_data_elements.txt");
            reader = new Parser.TokenReader(tokenizer);
            node = Parser.Parser.parseDataElement(reader);
            Assert.AreEqual(((Token)node.getValue()), new Token(Token.TokenType.Identifier, "bla"));
            childs = new ArrayList();
            childs.Add(new AstNode(new Token(Token.TokenType.Number, "2")));
            curChilds = node.getChilds();
            Assert.AreEqual(childs, curChilds);
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
        }

        [Test()]
        public void parseExplicitAddressTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer("/Users/dilsatsalihov/Projects/ERA-system-level/Era-system-level/Tests/explicitAddress.txt");
            Parser.TokenReader reader = new Parser.TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseExplicitAddress(reader);
            Assert.AreEqual(((Token)node.getValue()), new Token(Token.TokenType.Operator, "*"));
            ArrayList childs = new ArrayList();
            childs.Add(new AstNode(new Token(Token.TokenType.Number, "323")));
            ArrayList curChilds = node.getChilds();
            Assert.AreEqual(childs, curChilds);
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
        }


        [Test()]
        public void parsDereferenceTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer("/Users/dilsatsalihov/Projects/ERA-system-level/Era-system-level/Tests/dereference.txt");
            Parser.TokenReader reader = new Parser.TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseDereference(reader);
            Assert.AreEqual(((Token)node.getValue()), new Token(Token.TokenType.Operator, "*"));
            ArrayList childs = new ArrayList();
            childs.Add(new AstNode(new Token(Token.TokenType.Identifier, "id")));
            ArrayList curChilds = node.getChilds();
            Assert.AreEqual(childs, curChilds);
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));

            node = Parser.Parser.parseDereference(reader);
            Assert.AreEqual(((Token)node.getValue()), new Token(Token.TokenType.Operator, "*"));
            childs = new ArrayList();
            childs.Add(new AstNode(new Token(Token.TokenType.Register, "R0")));
            curChilds = node.getChilds();
            Assert.AreEqual(childs, curChilds);
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
        }

        [Test()]
        public void parseVariableReferenceTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer("/Users/dilsatsalihov/Projects/ERA-system-level/Era-system-level/Tests/Identifier.txt");
            Parser.TokenReader reader = new Parser.TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseVariableReference(reader);
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
        public void parseReceiverTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer("/Users/dilsatsalihov/Projects/ERA-system-level/Era-system-level/Tests/receiver.txt");
            Parser.TokenReader reader = new Parser.TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseReceiver(reader);
            Assert.AreEqual(node, new AstNode(new Token(Token.TokenType.Keyword, "this")));
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
        }

        [Test()]
        public void parseLiteralTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer("/Users/dilsatsalihov/Projects/ERA-system-level/Era-system-level/Tests/Literal.txt");
            Parser.TokenReader reader = new Parser.TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseLiteral(reader);
            Assert.AreEqual(node, new AstNode(new Token(Token.TokenType.Number, "123")));
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
        }

        [Test()]
        public void parseAddressTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer("/Users/dilsatsalihov/Projects/ERA-system-level/Era-system-level/Tests/Address.txt");
            Parser.TokenReader reader = new Parser.TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseAddress(reader);
            Assert.AreEqual(((Token)node.getValue()), new Token(Token.TokenType.Operator, "&"));
            ArrayList childs = new ArrayList();
            childs.Add(new AstNode(new Token(Token.TokenType.Identifier, "var")));
            ArrayList curChilds = node.getChilds();
            Assert.AreEqual(childs, curChilds);
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
        }
        [Test()]
        public void parseOperandTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer("/Users/dilsatsalihov/Projects/ERA-system-level/Era-system-level/Tests/Literal.txt");
            Parser.TokenReader reader = new Parser.TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseOperand(reader);
            Assert.AreEqual(node, new AstNode(new Token(Token.TokenType.Number, "123")));
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));

            tokenizer = new Tokenizer.Tokenizer("/Users/dilsatsalihov/Projects/ERA-system-level/Era-system-level/Tests/Address.txt");
            reader = new Parser.TokenReader(tokenizer);
            node = Parser.Parser.parseOperand(reader);
            Assert.AreEqual(((Token)node.getValue()), new Token(Token.TokenType.Operator, "&"));
            ArrayList childs = new ArrayList();
            childs.Add(new AstNode(new Token(Token.TokenType.Identifier, "var")));
            ArrayList curChilds = node.getChilds();
            Assert.AreEqual(childs, curChilds);
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));

            tokenizer = new Tokenizer.Tokenizer("/Users/dilsatsalihov/Projects/ERA-system-level/Era-system-level/Tests/receiver.txt");
            reader = new Parser.TokenReader(tokenizer);
            node = Parser.Parser.parseOperand(reader);
            Assert.AreEqual(node, new AstNode(new Token(Token.TokenType.Keyword, "this")));
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
        }


        [Test()]
        public void parseSwapTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer("/Users/dilsatsalihov/Projects/ERA-system-level/Era-system-level/Tests/swap.txt");
            Parser.TokenReader reader = new Parser.TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseSwap(reader);
            Assert.AreEqual(((Token)node.getValue()), new Token(Token.TokenType.Operator, "<=>"));
            ArrayList childs = new ArrayList();
            childs.Add(new AstNode(new Token(Token.TokenType.Identifier, "man")));
            childs.Add(new AstNode(new Token(Token.TokenType.Identifier, "woman")));
            ArrayList curChilds = node.getChilds();
            Assert.AreEqual(childs, curChilds);
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
            Assert.AreEqual(((Token)node.getValue()), new Token(Token.TokenType.Operator, "+"));
            ArrayList childs = new ArrayList();
            childs.Add(new AstNode(new Token(Token.TokenType.Number, "2")));
            childs.Add(new AstNode(new Token(Token.TokenType.Number, "2")));
            ArrayList curChilds = node.getChilds();
            Assert.AreEqual(childs, curChilds);
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));

            node = Parser.Parser.parseExpression(reader);
            Assert.AreEqual(((Token)node.getValue()), new Token(Token.TokenType.Operator, "&"));
            childs = new ArrayList();
            childs.Add(new AstNode(new Token(Token.TokenType.Identifier, "a")));
            curChilds = node.getChilds();
            Assert.AreEqual(childs, curChilds);
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
        }

        [Test()]
        public void parsePrimaryTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer("/Users/dilsatsalihov/Projects/ERA-system-level/Era-system-level/Tests/primary.txt");
            Parser.TokenReader reader = new Parser.TokenReader(tokenizer);
            AstNode node = Parser.Parser.parsePrimary(reader);
            Assert.AreEqual(((Token)node.getValue()), new Token(Token.TokenType.Identifier, "bla"));
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));

            tokenizer = new Tokenizer.Tokenizer("Tests/array_and_data_elements.txt");
            reader = new Parser.TokenReader(tokenizer);
            AstNode node1 = Parser.Parser.parsePrimary(reader);
            Assert.AreEqual(((Token)node1.getValue()), new Token(Token.TokenType.Identifier, "bla"));
            ArrayList childs = new ArrayList();
            childs.Add(new AstNode(new Token(Token.TokenType.Number, "2")));
            ArrayList curChilds = node1.getChilds();
            Assert.AreEqual(childs, curChilds);
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
        }


    }



}
