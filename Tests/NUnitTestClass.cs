using System;
using System.Collections;
using Erasystemlevel.Parser;
using Erasystemlevel.Tokenizer;
using NUnit.Framework;

namespace Erasystemlevel.Tests
{
    [TestFixture]
    public class NUnitTestClass
    {
        [Test]
        public void TokenReadingTestCase()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("test_reading.txt"));
            TokenReader reader = new TokenReader(tokenizer);
            Assert.AreEqual(reader.readNextToken().ToString(), new Token(Token.TokenType.Number, "1").ToString());
            Assert.AreEqual(reader.readNextToken().ToString(), new Token(Token.TokenType.Number, "2").ToString());
            Assert.AreEqual(reader.readNextToken().ToString(), new Token(Token.TokenType.Number, "3").ToString());
        }

        [Test]
        public void TokenSavedReadingTestCase()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("test_reading.txt"));
            TokenReader reader = new TokenReader(tokenizer);
            Assert.AreEqual(reader.readNextToken().ToString(), new Token(Token.TokenType.Number, "1").ToString());
            Assert.AreEqual(reader.readNextToken().ToString(), new Token(Token.TokenType.Number, "2").ToString());
            Assert.AreEqual(reader.readNextToken().ToString(), new Token(Token.TokenType.Number, "3").ToString());
            reader.saveReadedTokens();
            Assert.AreEqual(reader.readNextToken().ToString(), new Token(Token.TokenType.Number, "1").ToString());
            Assert.AreEqual(reader.readNextToken().ToString(), new Token(Token.TokenType.Number, "2").ToString());
            Assert.AreEqual(reader.readNextToken().ToString(), new Token(Token.TokenType.Number, "3").ToString());
        }

        [Test]
        public void parseBreakTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("Break.txt"));
            TokenReader reader = new TokenReader(tokenizer);
            AstNode breakNode = Parser.Parser.parseBreak(reader);
            Assert.AreEqual(((Token) breakNode.getValue()), new Token(Token.TokenType.Keyword, "break"));
            Assert.AreEqual(new ArrayList(), breakNode.getChilds());
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
        }

        [Test]
        public void parseIdentifierTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("Identifier.txt"));
            TokenReader reader = new TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseIdentifier(reader);
            Assert.AreEqual((Token) node.getValue(), new Token(Token.TokenType.Identifier, "player"));
            Assert.AreEqual(new ArrayList(), node.getChilds());
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
        }

        [Test]
        public void parseRegisterTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("register.txt"));
            TokenReader reader = new TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseRegister(reader);
            Assert.AreEqual((Token) node.getValue(), new Token(Token.TokenType.Register, "R1"));
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
        }

        [Test]
        public void parseArrayAndDataTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("array_and_data_elements.txt"));
            TokenReader reader = new TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseArrayElement(reader);
            Assert.AreEqual((Token) node.getValue(), new Token(Token.TokenType.Identifier, "bla"));
            ArrayList childs = new ArrayList();
            childs.Add(new AstNode(new Token(Token.TokenType.Number, "2")));
            ArrayList curChilds = node.getChilds();
            Assert.AreEqual(childs, curChilds);
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));

            tokenizer = new Tokenizer.Tokenizer(getTestFilePath("array_and_data_elements.txt"));
            reader = new TokenReader(tokenizer);
            node = Parser.Parser.parseDataElement(reader);
            Assert.AreEqual((Token) node.getValue(), new Token(Token.TokenType.Identifier, "bla"));
            childs = new ArrayList();
            childs.Add(new AstNode(new Token(Token.TokenType.Number, "2")));
            curChilds = node.getChilds();
            Assert.AreEqual(childs, curChilds);
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
        }

        [Test]
        public void parseExplicitAddressTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("explicitAddress.txt"));
            TokenReader reader = new TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseExplicitAddress(reader);
            Assert.AreEqual((Token) node.getValue(), new Token(Token.TokenType.Operator, "*"));
            ArrayList childs = new ArrayList();
            childs.Add(new AstNode(new Token(Token.TokenType.Number, "323")));
            ArrayList curChilds = node.getChilds();
            Assert.AreEqual(childs, curChilds);
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
        }


        [Test]
        public void parsDereferenceTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("dereference.txt"));
            TokenReader reader = new TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseDereference(reader);
            Assert.AreEqual((Token) node.getValue(), new Token(Token.TokenType.Operator, "*"));
            ArrayList childs = new ArrayList();
            childs.Add(new AstNode(new Token(Token.TokenType.Identifier, "id")));
            ArrayList curChilds = node.getChilds();
            Assert.AreEqual(childs, curChilds);
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));

            node = Parser.Parser.parseDereference(reader);
            Assert.AreEqual((Token) node.getValue(), new Token(Token.TokenType.Operator, "*"));
            childs = new ArrayList();
            childs.Add(new AstNode(new Token(Token.TokenType.Register, "R0")));
            curChilds = node.getChilds();
            Assert.AreEqual(childs, curChilds);
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
        }

        [Test]
        public void parseVariableReferenceTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("Identifier.txt"));
            TokenReader reader = new TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseVariableReference(reader);
            Assert.AreEqual((Token) node.getValue(), new Token(Token.TokenType.Identifier, "player"));
            Assert.AreEqual(new ArrayList(), node.getChilds());
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
        }

        [Test]
        public void parseGoToTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("goto.txt"));
            TokenReader reader = new TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseGoto(reader);
            Assert.AreEqual((Token) node.getValue(), new Token(Token.TokenType.Keyword, "goto"));
            AstNode a = new AstNode("");
            AstNode b = new AstNode("");
            a.Equals(b);

            ArrayList childs = new ArrayList();
            childs.Add(new AstNode(new Token(Token.TokenType.Identifier, "player")));
            Assert.AreEqual(childs, node.getChilds());
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
        }

        [Test]
        public void parseReceiverTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("receiver.txt"));
            TokenReader reader = new TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseReceiver(reader);
            Assert.AreEqual(node, new AstNode(new Token(Token.TokenType.Keyword, "this")));
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
        }

        [Test]
        public void parseLiteralTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("Literal.txt"));
            TokenReader reader = new TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseLiteral(reader);
            Assert.AreEqual(node, new AstNode(new Token(Token.TokenType.Number, "123")));
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
        }

        [Test]
        public void parseAddressTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("Address.txt"));
            TokenReader reader = new TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseAddress(reader);
            Assert.AreEqual((Token) node.getValue(), new Token(Token.TokenType.Operator, "&"));
            ArrayList childs = new ArrayList();
            childs.Add(new AstNode(new Token(Token.TokenType.Identifier, "var")));
            ArrayList curChilds = node.getChilds();
            Assert.AreEqual(childs, curChilds);
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
        }

        [Test]
        public void parseOperandTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("Literal.txt"));
            TokenReader reader = new TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseOperand(reader);
            Assert.AreEqual(node, new AstNode(new Token(Token.TokenType.Number, "123")));
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));

            tokenizer = new Tokenizer.Tokenizer(getTestFilePath("Address.txt"));
            reader = new TokenReader(tokenizer);
            node = Parser.Parser.parseOperand(reader);
            Assert.AreEqual((Token) node.getValue(), new Token(Token.TokenType.Operator, "&"));
            ArrayList childs = new ArrayList();
            childs.Add(new AstNode(new Token(Token.TokenType.Identifier, "var")));
            ArrayList curChilds = node.getChilds();
            Assert.AreEqual(childs, curChilds);
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));

            tokenizer = new Tokenizer.Tokenizer(getTestFilePath("receiver.txt"));
            reader = new TokenReader(tokenizer);
            node = Parser.Parser.parseOperand(reader);
            Assert.AreEqual(node, new AstNode(new Token(Token.TokenType.Keyword, "this")));
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
        }


        [Test]
        public void parseSwapTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("swap.txt"));
            TokenReader reader = new TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseSwap(reader);
            Assert.AreEqual((Token) node.getValue(), new Token(Token.TokenType.Operator, "<=>"));
            ArrayList childs = new ArrayList();
            childs.Add(new AstNode(new Token(Token.TokenType.Identifier, "man")));
            childs.Add(new AstNode(new Token(Token.TokenType.Identifier, "woman")));
            ArrayList curChilds = node.getChilds();
            Assert.AreEqual(childs, curChilds);
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
        }

        [Test]
        public void parseLoopBodyTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("loopBody.txt"));
            TokenReader reader = new TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseLoopBody(reader);
            Assert.AreEqual((Token) node.getValue(), new Token(Token.TokenType.Keyword, "loop"));
            ArrayList childs = new ArrayList();
            childs.Add(new AstNode(new Token(Token.TokenType.Keyword, "if")));
            Assert.AreEqual(childs, node.getChilds());
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Keyword, "end"));
        }

        [Test]
        public void parseWhileTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("while.txt"));
            TokenReader reader = new TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseWhile(reader);
            Assert.AreEqual((Token) node.getValue(), new Token(Token.TokenType.Keyword, "while"));
            ArrayList childs = new ArrayList();
            childs.Add(new AstNode(new Token(Token.TokenType.Operator, ">")));
            childs.Add(new AstNode(new Token(Token.TokenType.Keyword, "loop")));
            Assert.AreEqual(childs, node.getChilds());
        }

        [Test]
        public void parseForTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("for.txt"));
            TokenReader reader = new TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseFor(reader);
            Assert.AreEqual((Token) node.getValue(), new Token(Token.TokenType.Keyword, "for"));
            ArrayList childs = new ArrayList();
            childs.Add(new AstNode(new Token(Token.TokenType.Identifier, "a")));
            childs.Add(new AstNode(new Token(Token.TokenType.Keyword, "loop")));
            Assert.AreEqual(childs, node.getChilds());
        }

        [Test]
        public void parseExtensionStatementTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("extensionStatement.txt"));
            TokenReader reader = new TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseExtensionStatement(reader);
            Assert.AreEqual((Token) node.getValue(), new Token(Token.TokenType.Keyword, "if"));
            ArrayList childs = new ArrayList();
            childs.Add(new AstNode(new Token(Token.TokenType.Keyword, ">")));
            childs.Add(new AstNode(new Token(Token.TokenType.Keyword, "break")));
            Assert.AreEqual(childs, node.getChilds());
        }

        [Test]
        public void parseExpressionTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("expression.txt"));
            TokenReader reader = new TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseExpression(reader);
            Assert.AreEqual((Token) node.getValue(), new Token(Token.TokenType.Operator, "+"));
            ArrayList childs = new ArrayList();
            childs.Add(new AstNode(new Token(Token.TokenType.Number, "1")));
            childs.Add(new AstNode(new Token(Token.TokenType.Number, "2")));
            ArrayList curChilds = node.getChilds();
            Assert.AreEqual(childs, curChilds);
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));

            node = Parser.Parser.parseExpression(reader);
            Assert.AreEqual((Token) node.getValue(), new Token(Token.TokenType.Operator, "&"));
            childs = new ArrayList();
            childs.Add(new AstNode(new Token(Token.TokenType.Identifier, "a")));
            curChilds = node.getChilds();
            Assert.AreEqual(childs, curChilds);
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
        }

        [Test]
        public void parsePrimaryTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("primary.txt"));
            TokenReader reader = new TokenReader(tokenizer);
            AstNode node = Parser.Parser.parsePrimary(reader);
            Assert.AreEqual((Token) node.getValue(), new Token(Token.TokenType.Identifier, "bla"));
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));

            tokenizer = new Tokenizer.Tokenizer(getTestFilePath("array_and_data_elements.txt"));
            reader = new TokenReader(tokenizer);
            AstNode node1 = Parser.Parser.parsePrimary(reader);
            Assert.AreEqual((Token) node1.getValue(), new Token(Token.TokenType.Identifier, "bla"));
            ArrayList childs = new ArrayList();
            childs.Add(new AstNode(new Token(Token.TokenType.Number, "2")));
            ArrayList curChilds = node1.getChilds();
            Assert.AreEqual(childs, curChilds);
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
        }

        [Test]
        public void parseAssigmentTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("assigment.txt"));
            TokenReader reader = new TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseAssignment(reader);
            Assert.AreEqual((Token) node.getValue(), new Token(Token.TokenType.Operator, ":="));
            node.getChilds();
            ArrayList expectedChilds = new ArrayList();
            expectedChilds.Add(new AstNode(new Token(Token.TokenType.Identifier, "id")));
            expectedChilds.Add(new AstNode(new Token(Token.TokenType.Number, "2")));
        }

        [Test]
        public void parseDirectiveTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("directive.txt"));
            TokenReader reader = new TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseDirective(reader);
            Assert.AreEqual((Token) node.getValue(), new Token(Token.TokenType.Keyword, "format"));
            node.getChilds();
            ArrayList expectedChilds = new ArrayList();
            expectedChilds.Add(new AstNode(new Token(Token.TokenType.Number, "8")));
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
        }

        [Test]
        public void parseAttributeTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("attribute.txt"));
            TokenReader reader = new TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseAttribute(reader);
            Assert.AreEqual((Token) node.getValue(), new Token(Token.TokenType.Keyword, "start"));
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
        }

        [Test]
        public void parseTypeTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("type.txt"));
            TokenReader reader = new TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseType(reader);
            Assert.AreEqual((Token) node.getValue(), new Token(Token.TokenType.Keyword, "int"));
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
            node = Parser.Parser.parseType(reader);
            Assert.AreEqual((Token) node.getValue(), new Token(Token.TokenType.Keyword, "short"));
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
            node = Parser.Parser.parseType(reader);
            Assert.AreEqual((Token) node.getValue(), new Token(Token.TokenType.Keyword, "byte"));
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
        }

        [Test]
        public void parseLabelTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("label.txt"));
            TokenReader reader = new TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseLabel(reader);
            Assert.AreEqual((Token) node.getValue(), new Token(Token.TokenType.Identifier, "id"));
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
        }


        [Test]
        public void parseResultsTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("results.txt"));
            TokenReader reader = new TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseResults(reader);
            Assert.AreEqual(node.getValue(), "Results");
            node.getChilds();
            ArrayList expectedChilds = new ArrayList();
            expectedChilds.Add(new AstNode(new Token(Token.TokenType.Register, "R1")));
            expectedChilds.Add(new AstNode(new Token(Token.TokenType.Register, "R2")));
            expectedChilds.Add(new AstNode(new Token(Token.TokenType.Register, "R3")));
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));

            node = Parser.Parser.parseResults(reader);
            Assert.AreEqual(node.getValue(), "Results");
            node.getChilds();
            expectedChilds = new ArrayList();
            expectedChilds.Add(new AstNode(new Token(Token.TokenType.Register, "R1")));
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));

            node = Parser.Parser.parseResults(reader);
            Assert.AreEqual(node.getValue(), "Results");
            node.getChilds();
            expectedChilds = new ArrayList();
            expectedChilds.Add(new AstNode(new Token(Token.TokenType.Register, "R1")));
            expectedChilds.Add(new AstNode(new Token(Token.TokenType.Register, "R2")));
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));

        }

        [Test]
        public void parseParameterTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("parameter.txt"));
            TokenReader reader = new TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseParameter(reader);
            Assert.AreEqual((Token) node.getValue(), new Token(Token.TokenType.Keyword, "int"));
            node.getChilds();
            ArrayList expectedChilds = new ArrayList();
            expectedChilds.Add(new AstNode(new Token(Token.TokenType.Identifier, "parameter")));
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
            reader.clear();

            node = Parser.Parser.parseParameter(reader);
            Assert.AreEqual((Token) node.getValue(), new Token(Token.TokenType.Register, "R1"));
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
        }

        [Test]
        public void parseParametersTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("parameters.txt"));
            TokenReader reader = new TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseParameters(reader);
            Assert.AreEqual(node.getValue(), "Parameters");
            node.getChilds();
            ArrayList expectedChilds = new ArrayList();
            expectedChilds.Add(new AstNode(new Token(Token.TokenType.Register, "R0")));
        }

        [Test]
        public void parseConstDefinitionTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("constDefinition.txt"));
            TokenReader reader = new TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseConstDefinition(reader);
            Assert.AreEqual((Token)node.getValue(),new Token(Token.TokenType.Operator,"="));
            ArrayList curChilds = node.getChilds();
            ArrayList expectedChilds = new ArrayList();
            expectedChilds.Add(new AstNode(new Token(Token.TokenType.Identifier,"dilchat")));
            AstNode expression = new AstNode(new Token(Token.TokenType.Operator, "+"));
            expression.addChild(new AstNode(new Token(Token.TokenType.Number, "1")));
            expression.addChild(new AstNode(new Token(Token.TokenType.Number, "1")));
            expectedChilds.Add(expression);
            Assert.AreEqual(expectedChilds, curChilds);
            

        }

        [Test]
        public void parseVarDefinitionTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("varDefinition.txt"));
            TokenReader reader = new TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseVarDefinition(reader);
            Assert.AreEqual(node.getValue(),new Token(Token.TokenType.Operator, ":="));
            ArrayList curChilds = node.getChilds();
            ArrayList expectedChilds = new ArrayList();
            expectedChilds.Add(new AstNode(new Token(Token.TokenType.Identifier, "dilchat")));
            AstNode expression = new AstNode(new Token(Token.TokenType.Operator, "+"));
            expression.addChild(new AstNode(new Token(Token.TokenType.Number, "1")));
            expression.addChild(new AstNode(new Token(Token.TokenType.Number, "1")));
            expectedChilds.Add(expression);
            Assert.AreEqual(curChilds, expectedChilds);
        }

        [Test]
        public void parseVariableTest()
        {
            var tokenizer = new Tokenizer.Tokenizer(getTestFilePath("variable.txt"));
            var reader = new TokenReader(tokenizer);
            var node = Parser.Parser.parseVariable(reader);
            Assert.AreEqual(node.getValue(), "Variable");
            var curChilds = node.getChilds();
            var expectedChilds = new ArrayList
            {
                new AstNode(new Token(Token.TokenType.Keyword, "byte")),
                new AstNode(new Token(Token.TokenType.Identifier, "dilchat"))
            };
            var expression = new AstNode(new Token(Token.TokenType.Operator, "+"));
            expression.addChild(new AstNode(new Token(Token.TokenType.Number, "1")));
            expression.addChild(new AstNode(new Token(Token.TokenType.Number, "1")));
            expectedChilds.Add(expression);
            Assert.AreEqual(curChilds, expectedChilds);
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
        }

        [Test]
        public void parseDeclarationTest()
        {
            var tokenizer = new Tokenizer.Tokenizer(getTestFilePath("declaration.txt"));
            var reader = new TokenReader(tokenizer);
            var node = Parser.Parser.parseDeclaration(reader);
            Assert.AreEqual(node.getValue(), "Declaration");
            
            Assert.AreEqual((Token) node.getValue(), new Token(Token.TokenType.Keyword, "int"));
            node.getChilds();
            var expectedChilds = new ArrayList
            {
                new AstNode(new Token(Token.TokenType.Identifier, "dilchat"))
            };
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
            reader.clear();

            node = Parser.Parser.parseParameter(reader);
            Assert.AreEqual((Token) node.getValue(), new Token(Token.TokenType.Keyword, "const"));
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, "="));
            reader.clear();

            node = Parser.Parser.parseParameter(reader);
            Assert.AreEqual((Token) node.getValue(), new Token(Token.TokenType.Keyword, "const"));
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, "="));
        }
        
        [Test]
        public void parseConstantTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("constant.txt"));
            TokenReader reader = new TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseConstant(reader);
            Assert.AreEqual((Token)node.getValue(), new Token(Token.TokenType.Keyword, "const"));
            ArrayList curChilds = node.getChilds();
            ArrayList expectedChilds = new ArrayList();
            AstNode constDef = new AstNode(new Token(Token.TokenType.Operator,"="));
            constDef.addChild(new AstNode(new Token(Token.TokenType.Identifier,"a")));
            constDef.addChild(new AstNode(new Token(Token.TokenType.Number,"1")));
            AstNode constDef1 = new AstNode(new Token(Token.TokenType.Operator,"="));
            constDef1.addChild(new AstNode(new Token(Token.TokenType.Identifier,"b")));
            constDef1.addChild(new AstNode(new Token(Token.TokenType.Number,"2")));
            expectedChilds.Add(constDef);
            expectedChilds.Add(constDef1);
            Assert.AreEqual(curChilds, expectedChilds);
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));

            
            node = Parser.Parser.parseConstant(reader);
            Assert.AreEqual((Token)node.getValue(), new Token(Token.TokenType.Keyword, "const"));
            curChilds = node.getChilds();
            expectedChilds = new ArrayList();
            constDef = new AstNode(new Token(Token.TokenType.Operator,"="));
            constDef.addChild(new AstNode(new Token(Token.TokenType.Identifier,"a")));
            constDef.addChild(new AstNode(new Token(Token.TokenType.Number,"1")));
            expectedChilds.Add(constDef);
            Assert.AreEqual(curChilds, expectedChilds);

            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));

        }
        
        [Test]
        public void parseAssemblerStatementTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("assemblerStatement.txt"));
            TokenReader reader = new TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseAssemblerStatement(reader);
            Assert.AreEqual((Token)node.getValue(), new Token(Token.TokenType.Keyword, "skip"));
            ArrayList curChilds = node.getChilds();
            ArrayList expectedChilds = new ArrayList();
            expectedChilds.Add(new AstNode(new Token(Token.TokenType.Number, "1")));
            Assert.AreEqual(curChilds, expectedChilds);
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
            reader.clear();
            node = Parser.Parser.parseAssemblerStatement(reader);
            Assert.AreEqual((Token)node.getValue(), new Token(Token.TokenType.Keyword, "stop"));
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
            reader.clear();

            node = Parser.Parser.parseAssemblerStatement(reader);
            Assert.AreEqual((Token)node.getValue(), new Token(Token.TokenType.Operator, ":="));
            curChilds = node.getChilds();
            expectedChilds = new ArrayList();
            expectedChilds.Add(new AstNode(new Token(Token.TokenType.Register, "R0")));
            AstNode dereference = new AstNode(new Token(Token.TokenType.Operator,"*"));
            dereference.addChild(new AstNode(new Token(Token.TokenType.Register,"R1")));
            expectedChilds.Add(dereference);
            Assert.AreEqual(curChilds, expectedChilds);
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
            reader.clear();

            node = Parser.Parser.parseAssemblerStatement(reader);
            Assert.AreEqual((Token)node.getValue(), new Token(Token.TokenType.Operator, ":="));
            curChilds = node.getChilds();
            expectedChilds = new ArrayList();
            expectedChilds.Add(new AstNode(new Token(Token.TokenType.Register, "R0")));
            dereference = new AstNode(new Token(Token.TokenType.Register,"R1"));
            expectedChilds.Add(dereference);
            Assert.AreEqual(curChilds, expectedChilds);
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));

        }
        
        [Test]
        public void parseCallTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("call.txt"));
            TokenReader reader = new TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseCall(reader);
            Assert.AreEqual(node.getValue(), "Call");
            
            ArrayList curChilds = node.getChilds();
            ArrayList expectedChilds = new ArrayList();
            expectedChilds.Add(new AstNode(new Token(Token.TokenType.Identifier, "func")));
            AstNode parameters = new AstNode("CallParameters");
            parameters.addChild(new AstNode(new Token(Token.TokenType.Identifier, "a")));
            expectedChilds.Add(parameters);
            Assert.AreEqual(curChilds, expectedChilds);
            
            node = Parser.Parser.parseCall(reader);
            Assert.AreEqual(node.getValue(), "Call");
            
            curChilds = node.getChilds();
            expectedChilds = new ArrayList();
            AstNode funcCall = new AstNode(new Token(Token.TokenType.Delimiter,"."));
            funcCall.addChild(new AstNode(new Token(Token.TokenType.Identifier, "obj")));
            funcCall.addChild(new AstNode(new Token(Token.TokenType.Identifier, "func")));
            expectedChilds.Add(funcCall);
            parameters = new AstNode("CallParameters");
            expectedChilds.Add(parameters);
            Assert.AreEqual(curChilds, expectedChilds);
            
            
           

        }
        
        [Test]
        public void parseIfTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("if.txt"));
            TokenReader reader = new TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseIf(reader);
            Assert.AreEqual((Token)node.getValue(), new Token(Token.TokenType.Keyword,"if"));
            
            ArrayList curChilds = node.getChilds();
            ArrayList expectedChilds = new ArrayList();
            AstNode expression = new AstNode(new Token(Token.TokenType.Operator,">"));
            expression.addChild(new AstNode(new Token(Token.TokenType.Identifier, "a")));
            expression.addChild(new AstNode(new Token(Token.TokenType.Number, "2")));
            AstNode ifBody = new AstNode("RoutineBody");
            expectedChilds.Add(expression);
            expectedChilds.Add(ifBody);
            expectedChilds.Add(ifBody);
            Assert.AreEqual(curChilds, expectedChilds);
        }


        private static string getTestFilePath(string fileName)
        {
            return "Tests/testFiles/" + fileName;
        }
    }
}