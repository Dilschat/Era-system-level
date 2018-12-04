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
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("test_reading.txt"));
            Parser.TokenReader reader = new Parser.TokenReader(tokenizer);
            Assert.AreEqual(reader.readNextToken().ToString(), new Token(Token.TokenType.Number, "1").ToString());
            Assert.AreEqual(reader.readNextToken().ToString(), new Token(Token.TokenType.Number, "2").ToString());
            Assert.AreEqual(reader.readNextToken().ToString(), new Token(Token.TokenType.Number, "3").ToString());


        }

        [Test()]
        public void TokenSavedReadingTestCase()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("test_reading.txt"));
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
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("Break.txt"));
            Parser.TokenReader reader = new Parser.TokenReader(tokenizer);
            AstNode breakNode = Parser.Parser.parseBreak(reader);
            Assert.AreEqual(((Token)breakNode.getValue()), new Token(Token.TokenType.Keyword, "break"));
            Assert.AreEqual(new ArrayList(), breakNode.getChilds());
            Assert.AreEqual(reader.readNextToken(),new Token(Token.TokenType.Delimiter, ";"));
        }

        [Test()]
        public void parseIdentifierTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("Identifier.txt"));
            Parser.TokenReader reader = new Parser.TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseIdentifier(reader);
            Assert.AreEqual(((Token)node.getValue()), new Token(Token.TokenType.Identifier, "player"));
            Assert.AreEqual(new ArrayList(), node.getChilds());
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
        }

        [Test()]
        public void parseRegisterTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("register.txt"));
            Parser.TokenReader reader = new Parser.TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseRegister(reader);
            Assert.AreEqual(((Token)node.getValue()), new Token(Token.TokenType.Register, "R1"));
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
        }

        [Test()]
        public void parseArrayAndDataTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("array_and_data_elements.txt"));
            Parser.TokenReader reader = new Parser.TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseArrayElement(reader);
            Assert.AreEqual(((Token)node.getValue()), new Token(Token.TokenType.Identifier, "bla"));
            ArrayList childs = new ArrayList();
            childs.Add(new AstNode(new Token(Token.TokenType.Number, "2")));
            ArrayList curChilds = node.getChilds();
            Assert.AreEqual(childs, curChilds);
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));

            tokenizer = new Tokenizer.Tokenizer(getTestFilePath("array_and_data_elements.txt"));
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
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("explicitAddress.txt"));
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
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("dereference.txt"));
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
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("Identifier.txt"));
            Parser.TokenReader reader = new Parser.TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseVariableReference(reader);
            Assert.AreEqual(((Token)node.getValue()), new Token(Token.TokenType.Identifier, "player"));
            Assert.AreEqual(new ArrayList(), node.getChilds());
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
        }

        [Test()]
        public void parseGoToTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("goto.txt"));
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
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("receiver.txt"));
            Parser.TokenReader reader = new Parser.TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseReceiver(reader);
            Assert.AreEqual(node, new AstNode(new Token(Token.TokenType.Keyword, "this")));
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
        }

        [Test()]
        public void parseLiteralTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("Literal.txt"));
            Parser.TokenReader reader = new Parser.TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseLiteral(reader);
            Assert.AreEqual(node, new AstNode(new Token(Token.TokenType.Number, "123")));
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
        }

        [Test()]
        public void parseAddressTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("Address.txt"));
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
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("Literal.txt"));
            Parser.TokenReader reader = new Parser.TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseOperand(reader);
            Assert.AreEqual(node, new AstNode(new Token(Token.TokenType.Number, "123")));
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));

            tokenizer = new Tokenizer.Tokenizer(getTestFilePath("Address.txt"));
            reader = new Parser.TokenReader(tokenizer);
            node = Parser.Parser.parseOperand(reader);
            Assert.AreEqual(((Token)node.getValue()), new Token(Token.TokenType.Operator, "&"));
            ArrayList childs = new ArrayList();
            childs.Add(new AstNode(new Token(Token.TokenType.Identifier, "var")));
            ArrayList curChilds = node.getChilds();
            Assert.AreEqual(childs, curChilds);
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));

            tokenizer = new Tokenizer.Tokenizer(getTestFilePath("receiver.txt"));
            reader = new Parser.TokenReader(tokenizer);
            node = Parser.Parser.parseOperand(reader);
            Assert.AreEqual(node, new AstNode(new Token(Token.TokenType.Keyword, "this")));
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
        }


        [Test()]
        public void parseSwapTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("swap.txt"));
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
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("loopBody.txt"));
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
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("while.txt"));
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
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("for.txt"));
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
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("extensionStatement.txt"));
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
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("expression.txt"));
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
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("primary.txt"));
            Parser.TokenReader reader = new Parser.TokenReader(tokenizer);
            AstNode node = Parser.Parser.parsePrimary(reader);
            Assert.AreEqual(((Token)node.getValue()), new Token(Token.TokenType.Identifier, "bla"));
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));

            tokenizer = new Tokenizer.Tokenizer(getTestFilePath("array_and_data_elements.txt"));
            reader = new Parser.TokenReader(tokenizer);
            AstNode node1 = Parser.Parser.parsePrimary(reader);
            Assert.AreEqual(((Token)node1.getValue()), new Token(Token.TokenType.Identifier, "bla"));
            ArrayList childs = new ArrayList();
            childs.Add(new AstNode(new Token(Token.TokenType.Number, "2")));
            ArrayList curChilds = node1.getChilds();
            Assert.AreEqual(childs, curChilds);
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
        }
        
        [Test()]
        public void parseAssigmentTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("assigment.txt"));
            Parser.TokenReader reader = new Parser.TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseAssigment(reader);
            Assert.AreEqual(((Token)node.getValue()), new Token(Token.TokenType.Operator, ":="));
            ArrayList curChilds = node.getChilds();
            ArrayList expectedChilds = new ArrayList();
            expectedChilds.Add(new AstNode(new Token(Token.TokenType.Identifier,"id")));
            expectedChilds.Add(new AstNode(new Token(Token.TokenType.Number,"2")));
        }
        
        [Test()]
        public void parseDirectiveTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("directive.txt"));
            Parser.TokenReader reader = new Parser.TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseDirective(reader);
            Assert.AreEqual(((Token)node.getValue()), new Token(Token.TokenType.Keyword, "format"));
            ArrayList curChilds = node.getChilds();
            ArrayList expectedChilds = new ArrayList();
            expectedChilds.Add(new AstNode(new Token(Token.TokenType.Number,"8")));
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));

        }
        
        [Test()]
        public void parseAttributeTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("attribute.txt"));
            Parser.TokenReader reader = new Parser.TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseAttribute(reader);
            Assert.AreEqual(((Token)node.getValue()), new Token(Token.TokenType.Keyword, "start"));
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));

        }
        
        [Test()]
        public void parseTypeTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("type.txt"));
            Parser.TokenReader reader = new Parser.TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseType(reader);
            Assert.AreEqual(((Token)node.getValue()), new Token(Token.TokenType.Keyword, "int"));
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
            node = Parser.Parser.parseType(reader);
            Assert.AreEqual(((Token)node.getValue()), new Token(Token.TokenType.Keyword, "short"));
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
            node = Parser.Parser.parseType(reader);
            Assert.AreEqual(((Token)node.getValue()), new Token(Token.TokenType.Keyword, "byte"));
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));

        }
        
        [Test()]
        public void parseLabelTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("label.txt"));
            Parser.TokenReader reader = new Parser.TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseLabel(reader);
            Assert.AreEqual(((Token)node.getValue()), new Token(Token.TokenType.Identifier, "id"));
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
        }
        
                
        [Test()]
        public void parseResultsTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("results.txt"));
            Parser.TokenReader reader = new Parser.TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseResults(reader);
            Assert.AreEqual((node.getValue()),"Results");
            ArrayList curChilds = node.getChilds();
            ArrayList expectedChilds = new ArrayList();
            expectedChilds.Add(new AstNode(new Token(Token.TokenType.Register,"R1")));
            expectedChilds.Add(new AstNode(new Token(Token.TokenType.Register,"R2")));
            expectedChilds.Add(new AstNode(new Token(Token.TokenType.Register,"R3")));
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
            
            node = Parser.Parser.parseResults(reader);
            Assert.AreEqual((node.getValue()),"Results");
            curChilds = node.getChilds();
            expectedChilds = new ArrayList();
            expectedChilds.Add(new AstNode(new Token(Token.TokenType.Register,"R1")));
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
            
            node = Parser.Parser.parseResults(reader);
            Assert.AreEqual((node.getValue()),"Results");
            curChilds = node.getChilds();
            expectedChilds = new ArrayList();
            expectedChilds.Add(new AstNode(new Token(Token.TokenType.Register,"R1")));
            expectedChilds.Add(new AstNode(new Token(Token.TokenType.Register,"R2")));
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));

        }
        
        [Test()]
        public void parseParameterTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("parameter.txt"));
            Parser.TokenReader reader = new Parser.TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseParameter(reader);
            Assert.AreEqual(((Token)node.getValue()),new Token(Token.TokenType.Keyword,"int"));
            ArrayList curChilds = node.getChilds();
            ArrayList expectedChilds = new ArrayList();
            expectedChilds.Add(new AstNode(new Token(Token.TokenType.Identifier,"parameter")));
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
            reader.clear();
            
            node = Parser.Parser.parseParameter(reader);
            Assert.AreEqual(((Token)node.getValue()),new Token(Token.TokenType.Register,"R1"));
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));

        }
        
        [Test()]
        public void parseParametersTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("parameters.txt"));
            Parser.TokenReader reader = new Parser.TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseParameters(reader);
            Assert.AreEqual(node.getValue(),"Parameters");
            ArrayList curChilds = node.getChilds();
            ArrayList expectedChilds = new ArrayList();
            expectedChilds.Add(new AstNode(new Token(Token.TokenType.Register,"R0")));            
        }

        [Test()]
        public void parseConstDefinitionTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("constDefinition.txt"));
            Parser.TokenReader reader = new Parser.TokenReader(tokenizer);
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
        
        [Test()]
        public void parseVarDefinitionTest()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("varDefinition.txt"));
            Parser.TokenReader reader = new Parser.TokenReader(tokenizer);
            AstNode node = Parser.Parser.parseVarDefinition(reader);
            Assert.AreEqual((Token)node.getValue(),new Token(Token.TokenType.Identifier,"dilchat"));
            ArrayList curChilds = node.getChilds();
            ArrayList expectedChilds = new ArrayList();
            expectedChilds.Add(new AstNode(new Token(Token.TokenType.Number,"1")));

        }

        public static string getTestFilePath(string fileName)
        {
            return "Tests/testFiles/" + fileName;
        }


    }



}
