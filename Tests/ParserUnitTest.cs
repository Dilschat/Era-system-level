using System;
using System.Collections;
using Erasystemlevel.Exception;
using Erasystemlevel.Parser;
using Erasystemlevel.Tokenizer;
using NUnit.Framework;

namespace Erasystemlevel.Tests
{
    [TestFixture]
    public class ParserUnitTest
    {
        [Test]
        public void parseBreakTest()
        {
            var tokenizer = new Tokenizer.Tokenizer(getTestFilePath("Break.txt"));
            var reader = new TokenReader(tokenizer);
            var breakNode = Parser.Parser.parseBreak(reader);
            Assert.AreEqual(((Token) breakNode.getValue()), new Token(Token.TokenType.Keyword, "break"));
            Assert.AreEqual(new ArrayList(), breakNode.getChilds());
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
        }

        [Test]
        public void parseIdentifierTest()
        {
            var tokenizer = new Tokenizer.Tokenizer(getTestFilePath("Identifier.txt"));
            var reader = new TokenReader(tokenizer);
            var node = Parser.Parser.parseIdentifier(reader);
            Assert.AreEqual((Token) node.getValue(), new Token(Token.TokenType.Identifier, "player"));
            Assert.AreEqual(new ArrayList(), node.getChilds());
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
        }

        [Test]
        public void parseRegisterTest()
        {
            var tokenizer = new Tokenizer.Tokenizer(getTestFilePath("register.txt"));
            var reader = new TokenReader(tokenizer);
            var node = Parser.Parser.parseRegister(reader);
            Assert.AreEqual((Token) node.getValue(), new Token(Token.TokenType.Register, "R1"));
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
        }

        [Test]
        public void parseArrayAndDataTest()
        {
            var tokenizer = new Tokenizer.Tokenizer(getTestFilePath("array_and_data_elements.txt"));
            var reader = new TokenReader(tokenizer);
            var node = Parser.Parser.parseArrayElement(reader);
            Assert.AreEqual((Token) node.getValue(), new Token(Token.TokenType.Identifier, "bla"));
            var childs = new ArrayList {new AstNode(new Token(Token.TokenType.Number, "2"))};
            var curChilds = node.getChilds();
            Assert.AreEqual(childs, curChilds);
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));

            tokenizer = new Tokenizer.Tokenizer(getTestFilePath("array_and_data_elements.txt"));
            reader = new TokenReader(tokenizer);
            node = Parser.Parser.parseDataElement(reader);
            Assert.AreEqual((Token) node.getValue(), new Token(Token.TokenType.Identifier, "bla"));
            childs = new ArrayList {new AstNode(new Token(Token.TokenType.Number, "2"))};
            curChilds = node.getChilds();
            Assert.AreEqual(childs, curChilds);
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
        }

        [Test]
        public void parseExplicitAddressTest()
        {
            var tokenizer = new Tokenizer.Tokenizer(getTestFilePath("explicitAddress.txt"));
            var reader = new TokenReader(tokenizer);
            var node = Parser.Parser.parseExplicitAddress(reader);
            Assert.AreEqual((Token) node.getValue(), new Token(Token.TokenType.Operator, "*"));
            var childs = new ArrayList();
            childs.Add(new AstNode(new Token(Token.TokenType.Number, "323")));
            var curChilds = node.getChilds();
            Assert.AreEqual(childs, curChilds);
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
        }


        [Test]
        public void parsDereferenceTest()
        {
            var tokenizer = new Tokenizer.Tokenizer(getTestFilePath("dereference.txt"));
            var reader = new TokenReader(tokenizer);
            var node = Parser.Parser.parseDereference(reader);
            Assert.AreEqual((Token) node.getValue(), new Token(Token.TokenType.Operator, "*"));
            var childs = new ArrayList {new AstNode(new Token(Token.TokenType.Identifier, "id"))};
            var curChilds = node.getChilds();
            Assert.AreEqual(childs, curChilds);
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));

            node = Parser.Parser.parseDereference(reader);
            Assert.AreEqual((Token) node.getValue(), new Token(Token.TokenType.Operator, "*"));
            childs = new ArrayList {new AstNode(new Token(Token.TokenType.Register, "R0"))};
            curChilds = node.getChilds();
            Assert.AreEqual(childs, curChilds);
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
        }

        [Test]
        public void parseVariableReferenceTest()
        {
            var tokenizer = new Tokenizer.Tokenizer(getTestFilePath("Identifier.txt"));
            var reader = new TokenReader(tokenizer);
            var node = Parser.Parser.parseVariableReference(reader);
            Assert.AreEqual((Token) node.getValue(), new Token(Token.TokenType.Identifier, "player"));
            Assert.AreEqual(new ArrayList(), node.getChilds());
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
        }

        [Test]
        public void parseGoToTest()
        {
            var tokenizer = new Tokenizer.Tokenizer(getTestFilePath("goto.txt"));
            var reader = new TokenReader(tokenizer);
            var node = Parser.Parser.parseGoto(reader);
            Assert.AreEqual((Token) node.getValue(), new Token(Token.TokenType.Keyword, "goto"));
            var a = new AstNode("");
            var b = new AstNode("");
            a.Equals(b);

            var childs = new ArrayList {new AstNode(new Token(Token.TokenType.Identifier, "player"))};
            Assert.AreEqual(childs, node.getChilds());
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
        }

        [Test]
        public void parseReceiverTest()
        {
            var tokenizer = new Tokenizer.Tokenizer(getTestFilePath("receiver.txt"));
            var reader = new TokenReader(tokenizer);
            var node = Parser.Parser.parseReceiver(reader);
            Assert.AreEqual(node, new AstNode(new Token(Token.TokenType.Identifier, "a")));
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
        }

        [Test]
        public void parseLiteralTest()
        {
            var tokenizer = new Tokenizer.Tokenizer(getTestFilePath("Literal.txt"));
            var reader = new TokenReader(tokenizer);
            var node = Parser.Parser.parseLiteral(reader);
            Assert.AreEqual(node, new AstNode(new Token(Token.TokenType.Number, "123")));
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
        }

        [Test]
        public void parseAddressTest()
        {
            var tokenizer = new Tokenizer.Tokenizer(getTestFilePath("Address.txt"));
            var reader = new TokenReader(tokenizer);
            var node = Parser.Parser.parseAddress(reader);
            Assert.AreEqual((Token) node.getValue(), new Token(Token.TokenType.Operator, "&"));
            var childs = new ArrayList {new AstNode(new Token(Token.TokenType.Identifier, "var"))};
            var curChilds = node.getChilds();
            Assert.AreEqual(childs, curChilds);
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
        }

        [Test]
        public void parseOperandTest()
        {
            var tokenizer = new Tokenizer.Tokenizer(getTestFilePath("Literal.txt"));
            var reader = new TokenReader(tokenizer);
            var node = Parser.Parser.parseOperand(reader);
            Assert.AreEqual(node, new AstNode(new Token(Token.TokenType.Number, "123")));
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));

            tokenizer = new Tokenizer.Tokenizer(getTestFilePath("Address.txt"));
            reader = new TokenReader(tokenizer);
            node = Parser.Parser.parseOperand(reader);
            Assert.AreEqual((Token) node.getValue(), new Token(Token.TokenType.Operator, "&"));
            var childs = new ArrayList {new AstNode(new Token(Token.TokenType.Identifier, "var"))};
            var curChilds = node.getChilds();
            Assert.AreEqual(childs, curChilds);
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));

            tokenizer = new Tokenizer.Tokenizer(getTestFilePath("receiver.txt"));
            reader = new TokenReader(tokenizer);
            node = Parser.Parser.parseOperand(reader);
            Assert.AreEqual(node, new AstNode(new Token(Token.TokenType.Identifier, "a")));
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
        }


        [Test]
        public void parseSwapTest()
        {
            var tokenizer = new Tokenizer.Tokenizer(getTestFilePath("swap.txt"));
            var reader = new TokenReader(tokenizer);
            var node = Parser.Parser.parseSwap(reader);
            Assert.AreEqual((Token) node.getValue(), new Token(Token.TokenType.Operator, "<=>"));
            var childs = new ArrayList
            {
                new AstNode(new Token(Token.TokenType.Identifier, "man")),
                new AstNode(new Token(Token.TokenType.Identifier, "woman"))
            };
            var curChilds = node.getChilds();
            Assert.AreEqual(childs, curChilds);
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
        }


        [Test]
        public void parseWhileTest()
        {
            var tokenizer = new Tokenizer.Tokenizer(getTestFilePath("while.txt"));
            var reader = new TokenReader(tokenizer);
            var node = Parser.Parser.parseWhile(reader);
            Assert.AreEqual(node.getValue(), AstNode.NodeType.While);
            var curChilds = node.getChilds();
            Assert.AreEqual(curChilds[0].getValue(), new Token(Token.TokenType.Operator, ">"));
            Assert.AreEqual(curChilds[1].getValue(), AstNode.NodeType.LoopBody);
        }

        [Test]
        public void parseForTest()
        {
            var tokenizer = new Tokenizer.Tokenizer(getTestFilePath("for.txt"));
            var reader = new TokenReader(tokenizer);
            var node = Parser.Parser.parseFor(reader);
            Assert.AreEqual((Token) node.getValue(), new Token(Token.TokenType.Keyword, "for"));
            var childs = node.getChilds();
            Assert.AreEqual(childs[0], new AstNode(new Token(Token.TokenType.Identifier, "i")));
            Assert.AreEqual(childs[1].getValue(), new Token(Token.TokenType.Keyword, "from"));
            Assert.AreEqual(childs[2].getValue(), new Token(Token.TokenType.Keyword, "to"));
            Assert.AreEqual(childs[3].getValue(), new Token(Token.TokenType.Keyword, "step"));
        }

        [Test]
        public void parseExtensionStatementTest()
        {
            var tokenizer = new Tokenizer.Tokenizer(getTestFilePath("extensionStatement.txt"));
            var reader = new TokenReader(tokenizer);
            var node = Parser.Parser.parseExtensionStatement(reader);
            Assert.AreEqual((Token) node.getValue(), new Token(Token.TokenType.Operator, ":="));
            node.getChilds();
            var expectedChilds = new ArrayList
            {
                new AstNode(new Token(Token.TokenType.Identifier, "id")),
                new AstNode(new Token(Token.TokenType.Number, "2"))
            };
            node = Parser.Parser.parseExtensionStatement(reader);
            Assert.AreEqual(node.getValue(), "Call");

            var curChilds = node.getChilds();
            expectedChilds = new ArrayList {new AstNode(new Token(Token.TokenType.Identifier, "func"))};
            var parameters = new AstNode("CallParameters");
            parameters.addChild(new AstNode(new Token(Token.TokenType.Identifier, "a")));
            expectedChilds.Add(parameters);
            Assert.AreEqual(curChilds, expectedChilds);

            node = Parser.Parser.parseExtensionStatement(reader);
            Assert.AreEqual(node.getValue(), "Call");

            curChilds = node.getChilds();
            expectedChilds = new ArrayList();
            var funcCall = new AstNode(new Token(Token.TokenType.Delimiter, "."));
            funcCall.addChild(new AstNode(new Token(Token.TokenType.Identifier, "obj")));
            funcCall.addChild(new AstNode(new Token(Token.TokenType.Identifier, "func")));
            expectedChilds.Add(funcCall);
            parameters = new AstNode("CallParameters");
            expectedChilds.Add(parameters);
            Assert.AreEqual(curChilds, expectedChilds);
        }

        [Test]
        public void parseExpressionTest()
        {
            var tokenizer = new Tokenizer.Tokenizer(getTestFilePath("expression.txt"));
            var reader = new TokenReader(tokenizer);
            var node = Parser.Parser.parseExpression(reader);
            Assert.AreEqual((Token) node.getValue(), new Token(Token.TokenType.Operator, "+"));
            var childs = new ArrayList
            {
                new AstNode(new Token(Token.TokenType.Number, "1")),
                new AstNode(new Token(Token.TokenType.Number, "2"))
            };
            var curChilds = node.getChilds();
            Assert.AreEqual(childs, curChilds);
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));

            node = Parser.Parser.parseExpression(reader);
            Assert.AreEqual((Token) node.getValue(), new Token(Token.TokenType.Operator, "&"));
            childs = new ArrayList {new AstNode(new Token(Token.TokenType.Identifier, "a"))};
            curChilds = node.getChilds();
            Assert.AreEqual(childs, curChilds);
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
        }

        [Test]
        public void parsePrimaryTest()
        {
            var tokenizer = new Tokenizer.Tokenizer(getTestFilePath("primary.txt"));
            var reader = new TokenReader(tokenizer);
            var node = Parser.Parser.parsePrimary(reader);
            Assert.AreEqual((Token) node.getValue(), new Token(Token.TokenType.Identifier, "bla"));
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));

            tokenizer = new Tokenizer.Tokenizer(getTestFilePath("array_and_data_elements.txt"));
            reader = new TokenReader(tokenizer);
            var node1 = Parser.Parser.parsePrimary(reader);
            Assert.AreEqual((Token) node1.getValue(), new Token(Token.TokenType.Identifier, "bla"));
            var childs = new ArrayList {new AstNode(new Token(Token.TokenType.Number, "2"))};
            var curChilds = node1.getChilds();
            Assert.AreEqual(childs, curChilds);
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
        }

        [Test]
        public void parseAssignmentTest()
        {
            var tokenizer = new Tokenizer.Tokenizer(getTestFilePath("assignment.txt"));
            var reader = new TokenReader(tokenizer);
            var node = Parser.Parser.parseAssignment(reader);
            Assert.AreEqual((Token) node.getValue(), new Token(Token.TokenType.Operator, ":="));
            node.getChilds();
            var expectedChilds = new ArrayList
            {
                new AstNode(new Token(Token.TokenType.Identifier, "id")),
                new AstNode(new Token(Token.TokenType.Number, "2"))
            };
        }

        [Test]
        public void parseDirectiveTest()
        {
            var tokenizer = new Tokenizer.Tokenizer(getTestFilePath("directive.txt"));
            var reader = new TokenReader(tokenizer);
            var node = Parser.Parser.parseDirective(reader);
            Assert.AreEqual((Token) node.getValue(), new Token(Token.TokenType.Keyword, "format"));
            node.getChilds();
            var expectedChilds = new ArrayList {new AstNode(new Token(Token.TokenType.Number, "8"))};
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
        }

        [Test]
        public void parseAttributeTest()
        {
            var tokenizer = new Tokenizer.Tokenizer(getTestFilePath("attribute.txt"));
            var reader = new TokenReader(tokenizer);
            var node = Parser.Parser.parseAttribute(reader);
            Assert.AreEqual((Token) node.getValue(), new Token(Token.TokenType.Keyword, "start"));
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
        }

        [Test]
        public void parseTypeTest()
        {
            var tokenizer = new Tokenizer.Tokenizer(getTestFilePath("type.txt"));
            var reader = new TokenReader(tokenizer);
            var node = Parser.Parser.parseType(reader);
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
            var tokenizer = new Tokenizer.Tokenizer(getTestFilePath("label.txt"));
            var reader = new TokenReader(tokenizer);
            var node = Parser.Parser.parseLabel(reader);
            Assert.AreEqual((Token) node.getValue(), new Token(Token.TokenType.Identifier, "id"));
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
        }


        [Test]
        public void parseResultsTest()
        {
            var tokenizer = new Tokenizer.Tokenizer(getTestFilePath("results.txt"));
            var reader = new TokenReader(tokenizer);
            var node = Parser.Parser.parseResults(reader);
            Assert.AreEqual(node.getValue(), AstNode.NodeType.Results);
            node.getChilds();
            var expectedChilds = new ArrayList
            {
                new AstNode(new Token(Token.TokenType.Register, "R1")),
                new AstNode(new Token(Token.TokenType.Register, "R2")),
                new AstNode(new Token(Token.TokenType.Register, "R3"))
            };
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));

            node = Parser.Parser.parseResults(reader);
            Assert.AreEqual(node.getValue(), AstNode.NodeType.Results);
            node.getChilds();
            expectedChilds = new ArrayList {new AstNode(new Token(Token.TokenType.Register, "R1"))};
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));

            node = Parser.Parser.parseResults(reader);
            Assert.AreEqual(node.getValue(), AstNode.NodeType.Results);
            node.getChilds();
            expectedChilds = new ArrayList
            {
                new AstNode(new Token(Token.TokenType.Register, "R1")),
                new AstNode(new Token(Token.TokenType.Register, "R2"))
            };
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
        }

        [Test]
        public void parseParameterTest()
        {
            var tokenizer = new Tokenizer.Tokenizer(getTestFilePath("parameter.txt"));
            var reader = new TokenReader(tokenizer);
            var node = Parser.Parser.parseParameter(reader);
            Assert.AreEqual((Token) node.getValue(), new Token(Token.TokenType.Keyword, "int"));
            node.getChilds();
            var expectedChilds = new ArrayList {new AstNode(new Token(Token.TokenType.Identifier, "parameter"))};
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
            reader.clear();

            node = Parser.Parser.parseParameter(reader);
            Assert.AreEqual((Token) node.getValue(), new Token(Token.TokenType.Register, "R1"));
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
        }

        [Test]
        public void parseParametersTest()
        {
            var tokenizer = new Tokenizer.Tokenizer(getTestFilePath("parameters.txt"));
            var reader = new TokenReader(tokenizer);
            var node = Parser.Parser.parseParameters(reader);
            Assert.AreEqual(node.getValue(), AstNode.NodeType.Parameters);
            node.getChilds();
            var expectedChilds = new ArrayList {new AstNode(new Token(Token.TokenType.Register, "R0"))};
        }

        [Test]
        public void parseConstDefinitionTest()
        {
            var tokenizer = new Tokenizer.Tokenizer(getTestFilePath("constDefinition.txt"));
            var reader = new TokenReader(tokenizer);
            var node = Parser.Parser.parseConstDefinition(reader);
            Assert.AreEqual((Token) node.getValue(), new Token(Token.TokenType.Operator, "="));
            var curChilds = node.getChilds();
            var expectedChilds = new ArrayList {new AstNode(new Token(Token.TokenType.Identifier, "dilchat"))};
            var expression = new AstNode(new Token(Token.TokenType.Operator, "+"));
            expression.addChild(new AstNode(new Token(Token.TokenType.Number, "1")));
            expression.addChild(new AstNode(new Token(Token.TokenType.Number, "1")));
            expectedChilds.Add(expression);
            Assert.AreEqual(expectedChilds, curChilds);
        }

        [Test]
        public void parseVarDefinitionTest()
        {
            var tokenizer = new Tokenizer.Tokenizer(getTestFilePath("varDefinition.txt"));
            var reader = new TokenReader(tokenizer);
            var node = Parser.Parser.parseVarDefinition(reader);
            Assert.AreEqual(node.getValue(), new Token(Token.TokenType.Operator, "[]"));
            var curChilds = node.getChilds();
            var expectedChilds = new ArrayList {new AstNode(new Token(Token.TokenType.Identifier, "dilchat"))};
            var expression = new AstNode(new Token(Token.TokenType.Operator, "+"));
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
            Assert.AreEqual(node.getValue(), AstNode.NodeType.Variable);
            var curChilds = node.getChilds();
            var expectedChilds = new ArrayList
            {
                new AstNode(new Token(Token.TokenType.Keyword, "byte"))
            };
            var expression = new AstNode(new Token(Token.TokenType.Operator, "+"));
            expression.addChild(new AstNode(new Token(Token.TokenType.Number, "1")));
            expression.addChild(new AstNode(new Token(Token.TokenType.Number, "1")));
            var varExpression = new AstNode(new Token(Token.TokenType.Operator, ":="));
            varExpression.addChild(new AstNode(new Token(Token.TokenType.Identifier, "dilchat")));
            varExpression.addChild(expression);
            expectedChilds.Add(varExpression);
            Assert.AreEqual(curChilds, expectedChilds);
        }

        [Test]
        public void ParseRoutinebodyTest()
        {
            var tokenizer = new Tokenizer.Tokenizer(getTestFilePath("routineBody.txt"));
            var reader = new TokenReader(tokenizer);
            var node = Parser.Parser.parseRoutineBody(reader);
            Assert.AreEqual(node.getValue(), AstNode.NodeType.RoutineBody);
            var variable = new AstNode(AstNode.NodeType.Variable);
            var curChilds = node.getChilds();
            variable.addChild(new AstNode(new Token(Token.TokenType.Keyword, "byte")));

            var expression = new AstNode(new Token(Token.TokenType.Operator, "+"));
            expression.addChild(new AstNode(new Token(Token.TokenType.Number, "1")));
            expression.addChild(new AstNode(new Token(Token.TokenType.Number, "1")));
            var varExpression = new AstNode(new Token(Token.TokenType.Operator, ":="));
            varExpression.addChild(new AstNode(new Token(Token.TokenType.Identifier, "dilchat")));
            varExpression.addChild(expression);
            variable.addChild(varExpression);
            Assert.AreEqual(curChilds[0], variable);
            Assert.AreEqual(curChilds[1], variable);
        }

        [Test]
        public void parseRoutineTest()
        {
            var tokenizer = new Tokenizer.Tokenizer(getTestFilePath("routine.txt"));
            var reader = new TokenReader(tokenizer);
            var node = Parser.Parser.parseRoutine(reader);
            Assert.AreEqual(node.getValue(), AstNode.NodeType.Routine);
            var variable = new AstNode(AstNode.NodeType.Variable);
            var curChilds = node.getChilds();
            Assert.AreEqual(curChilds[0], new AstNode(new Token(Token.TokenType.Keyword, "start")));
            Assert.AreEqual(curChilds[1], new AstNode(new Token(Token.TokenType.Identifier, "func")));
            Assert.AreEqual(curChilds[2].getValue(), AstNode.NodeType.Parameters);
            Assert.AreEqual(curChilds[3].getValue(), AstNode.NodeType.Results);
            Assert.AreEqual(curChilds[4].getValue(), AstNode.NodeType.RoutineBody);
        }

        [Test]
        public void parseModuleTest()
        {
            var tokenizer = new Tokenizer.Tokenizer(getTestFilePath("module.txt"));
            var reader = new TokenReader(tokenizer);
            var node = Parser.Parser.parseModule(reader);
            Assert.AreEqual(node.getValue(), new AstNode(new Token(Token.TokenType.Identifier, "id")));
            var curChilds = node.getChilds();
            var expectedChilds = new ArrayList
            {
                new AstNode(new Token(Token.TokenType.Keyword, "byte"))
            };
            var expression = new AstNode(new Token(Token.TokenType.Operator, "+"));
            expression.addChild(new AstNode(new Token(Token.TokenType.Number, "1")));
            expression.addChild(new AstNode(new Token(Token.TokenType.Number, "1")));
            var varExpression = new AstNode(new Token(Token.TokenType.Operator, ":="));
            varExpression.addChild(new AstNode(new Token(Token.TokenType.Identifier, "dilchat")));
            varExpression.addChild(expression);
            expectedChilds.Add(varExpression);
            Assert.AreEqual(curChilds[0].getChilds(), expectedChilds);
        }

        [Test]
        public void parseDataTest()
        {
            var tokenizer = new Tokenizer.Tokenizer(getTestFilePath("data.txt"));
            var reader = new TokenReader(tokenizer);
            var node = Parser.Parser.parseData(reader);
            Assert.AreEqual(node.getValue(), new AstNode(new Token(Token.TokenType.Identifier, "id")));
            var curChilds = node.getChilds();
            var expectedChilds = new ArrayList
            {
                new AstNode(new Token(Token.TokenType.Number, "1")),
                new AstNode(new Token(Token.TokenType.Number, "2"))
            };
            Assert.AreEqual(curChilds, expectedChilds);
        }

        [Test]
        public void parseCodeTest()
        {
            var tokenizer = new Tokenizer.Tokenizer(getTestFilePath("code.txt"));
            var reader = new TokenReader(tokenizer);
            var node = Parser.Parser.parseCode(reader);
            Assert.AreEqual(node.getValue(), AstNode.NodeType.Code);
            var curChilds = node.getChilds();
            var expectedChilds = new ArrayList
            {
                new AstNode(new Token(Token.TokenType.Keyword, "byte"))
            };
            var expression = new AstNode(new Token(Token.TokenType.Operator, "+"));
            expression.addChild(new AstNode(new Token(Token.TokenType.Number, "1")));
            expression.addChild(new AstNode(new Token(Token.TokenType.Number, "1")));
            var varExpression = new AstNode(new Token(Token.TokenType.Operator, ":="));
            varExpression.addChild(new AstNode(new Token(Token.TokenType.Identifier, "dilchat")));
            varExpression.addChild(expression);
            expectedChilds.Add(varExpression);
            Assert.AreEqual(curChilds[0].getChilds(), expectedChilds);
        }

        [Test]
        public void parseUnitTest()
        {
            var tokenizer = new Tokenizer.Tokenizer(getTestFilePath("code.txt"));
            var reader = new TokenReader(tokenizer);
            var node = Parser.Parser.ParseUnit(reader);
            Assert.AreEqual(node.getValue(), AstNode.NodeType.Code);
            var curChilds = node.getChilds();
            var expectedChilds = new ArrayList
            {
                new AstNode(new Token(Token.TokenType.Keyword, "byte"))
            };
            var expression = new AstNode(new Token(Token.TokenType.Operator, "+"));
            expression.addChild(new AstNode(new Token(Token.TokenType.Number, "1")));
            expression.addChild(new AstNode(new Token(Token.TokenType.Number, "1")));
            var varExpression = new AstNode(new Token(Token.TokenType.Operator, ":="));
            varExpression.addChild(new AstNode(new Token(Token.TokenType.Identifier, "dilchat")));
            varExpression.addChild(expression);
            expectedChilds.Add(varExpression);
            Assert.AreEqual(curChilds[0].getChilds(), expectedChilds);
        }

        [Test]
        public void parseDeclarationTest()
        {
            var tokenizer = new Tokenizer.Tokenizer(getTestFilePath("declaration.txt"));
            var reader = new TokenReader(tokenizer);
            var node = Parser.Parser.parseDeclaration(reader);
            Assert.AreEqual(node.getValue(), AstNode.NodeType.Variable);
            var curChilds = node.getChilds();
            var expectedChilds = new ArrayList
            {
                new AstNode(new Token(Token.TokenType.Keyword, "byte"))
            };
            var expression = new AstNode(new Token(Token.TokenType.Operator, "+"));
            expression.addChild(new AstNode(new Token(Token.TokenType.Number, "1")));
            expression.addChild(new AstNode(new Token(Token.TokenType.Number, "1")));
            var varExpression = new AstNode(new Token(Token.TokenType.Operator, ":="));
            varExpression.addChild(new AstNode(new Token(Token.TokenType.Identifier, "dilchat")));
            varExpression.addChild(expression);
            expectedChilds.Add(varExpression);
            Assert.AreEqual(curChilds, expectedChilds);
        }

        [Test]
        public void parseConstantTest()
        {
            var tokenizer = new Tokenizer.Tokenizer(getTestFilePath("constant.txt"));
            var reader = new TokenReader(tokenizer);
            var node = Parser.Parser.parseConstant(reader);
            Assert.AreEqual((Token) node.getValue(), new Token(Token.TokenType.Keyword, "const"));
            var curChilds = node.getChilds();
            var expectedChilds = new ArrayList();
            var constDef = new AstNode(new Token(Token.TokenType.Operator, "="));
            constDef.addChild(new AstNode(new Token(Token.TokenType.Identifier, "a")));
            constDef.addChild(new AstNode(new Token(Token.TokenType.Number, "1")));
            var constDef1 = new AstNode(new Token(Token.TokenType.Operator, "="));
            constDef1.addChild(new AstNode(new Token(Token.TokenType.Identifier, "b")));
            constDef1.addChild(new AstNode(new Token(Token.TokenType.Number, "2")));
            expectedChilds.Add(constDef);
            expectedChilds.Add(constDef1);
            Assert.AreEqual(curChilds, expectedChilds);
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));


            node = Parser.Parser.parseConstant(reader);
            Assert.AreEqual((Token) node.getValue(), new Token(Token.TokenType.Keyword, "const"));
            curChilds = node.getChilds();
            expectedChilds = new ArrayList();
            constDef = new AstNode(new Token(Token.TokenType.Operator, "="));
            constDef.addChild(new AstNode(new Token(Token.TokenType.Identifier, "a")));
            constDef.addChild(new AstNode(new Token(Token.TokenType.Number, "1")));
            expectedChilds.Add(constDef);
            Assert.AreEqual(curChilds, expectedChilds);

            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
        }

        [Test]
        public void parseAssemblerStatementTest()
        {
            var tokenizer = new Tokenizer.Tokenizer(getTestFilePath("assemblerStatement.txt"));
            var reader = new TokenReader(tokenizer);
            var node = Parser.Parser.parseAssemblerStatement(reader);
            Assert.AreEqual((Token) node.getValue(), new Token(Token.TokenType.Keyword, "skip"));
            var curChilds = node.getChilds();
            var expectedChilds = new ArrayList {new AstNode(new Token(Token.TokenType.Number, "1"))};
            Assert.AreEqual(curChilds, expectedChilds);
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
            reader.clear();
            node = Parser.Parser.parseAssemblerStatement(reader);
            Assert.AreEqual((Token) node.getValue(), new Token(Token.TokenType.Keyword, "stop"));
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
            reader.clear();

            node = Parser.Parser.parseAssemblerStatement(reader);
            Assert.AreEqual((Token) node.getValue(), new Token(Token.TokenType.Operator, ":="));
            curChilds = node.getChilds();
            expectedChilds = new ArrayList {new AstNode(new Token(Token.TokenType.Register, "R0"))};
            var dereference = new AstNode(new Token(Token.TokenType.Operator, "*"));
            dereference.addChild(new AstNode(new Token(Token.TokenType.Register, "R1")));
            expectedChilds.Add(dereference);
            Assert.AreEqual(curChilds, expectedChilds);
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
            reader.clear();

            node = Parser.Parser.parseAssemblerStatement(reader);
            Assert.AreEqual((Token) node.getValue(), new Token(Token.TokenType.Operator, ":="));
            curChilds = node.getChilds();
            expectedChilds = new ArrayList {new AstNode(new Token(Token.TokenType.Register, "R0"))};
            dereference = new AstNode(new Token(Token.TokenType.Register, "R1"));
            expectedChilds.Add(dereference);
            Assert.AreEqual(curChilds, expectedChilds);
            Assert.AreEqual(reader.readNextToken(), new Token(Token.TokenType.Delimiter, ";"));
        }

        [Test]
        public void parseCallTest()
        {
            var tokenizer = new Tokenizer.Tokenizer(getTestFilePath("call.txt"));
            var reader = new TokenReader(tokenizer);
            var node = Parser.Parser.parseCall(reader);
            Assert.AreEqual(node.getValue(), "Call");

            var curChilds = node.getChilds();
            var expectedChilds = new ArrayList {new AstNode(new Token(Token.TokenType.Identifier, "func"))};
            var parameters = new AstNode("CallParameters");
            parameters.addChild(new AstNode(new Token(Token.TokenType.Identifier, "a")));
            expectedChilds.Add(parameters);
            Assert.AreEqual(curChilds, expectedChilds);

            node = Parser.Parser.parseCall(reader);
            Assert.AreEqual(node.getValue(), "Call");

            curChilds = node.getChilds();
            expectedChilds = new ArrayList();
            var funcCall = new AstNode(new Token(Token.TokenType.Delimiter, "."));
            funcCall.addChild(new AstNode(new Token(Token.TokenType.Identifier, "obj")));
            funcCall.addChild(new AstNode(new Token(Token.TokenType.Identifier, "func")));
            expectedChilds.Add(funcCall);
            parameters = new AstNode("CallParameters");
            expectedChilds.Add(parameters);
            Assert.AreEqual(curChilds, expectedChilds);
        }

        [Test]
        public void parseSimpleIfTest()
        {
            var tokenizer = new Tokenizer.Tokenizer(getTestFilePath("if.txt"));
            var reader = new TokenReader(tokenizer);
            var node = Parser.Parser.parseIf(reader);
            Assert.AreEqual((Token) node.getValue(), new Token(Token.TokenType.Keyword, "if"));
            var curChilds = node.getChilds();
            var expectedChilds = new ArrayList();
            var expression = new AstNode(new Token(Token.TokenType.Operator, ">"));
            expression.addChild(new AstNode(new Token(Token.TokenType.Identifier, "a")));
            expression.addChild(new AstNode(new Token(Token.TokenType.Number, "2")));
            expectedChilds.Add(expression);
            var ifBody = new AstNode(AstNode.NodeType.IfBody);
            ifBody.SetNodeType(AstNode.NodeType.IfBody);
            
            AstNode statement = new AstNode(AstNode.NodeType.Statement);
            statement.SetNodeType(AstNode.NodeType.Statement);
            AstNode assigment = new AstNode(new Token(Token.TokenType.Operator,":="));
            AstNode id = new AstNode(new Token(Token.TokenType.Identifier,"a"));
            id.SetNodeType(AstNode.NodeType.Identifier);
            AstNode lit = new AstNode(new Token(Token.TokenType.Number,"2"));
            id.SetNodeType(AstNode.NodeType.Literal);
            assigment.addChild(id);
            assigment.addChild(lit);
            assigment.SetNodeType(AstNode.NodeType.Assignment);
            statement.addChild(assigment);
            ifBody.addChild(statement);
            AstNode ifBody2 = new AstNode(AstNode.NodeType.IfBody);
            ifBody2.SetNodeType(AstNode.NodeType.IfBody);
            expectedChilds.Add(ifBody);
            expectedChilds.Add(ifBody2);
            Assert.AreEqual(curChilds, expectedChilds);
        }
        
        [Test]
        public void parseComplexIfTest()
        {
            var tokenizer = new Tokenizer.Tokenizer(getTestFilePath("if.txt"));
            var reader = new TokenReader(tokenizer);
            var node = Parser.Parser.parseIf(reader);
            Console.WriteLine(node.ToString());
            Assert.AreEqual((Token) node.getValue(), new Token(Token.TokenType.Keyword, "if"));
            var curChilds = node.getChilds();
            var expectedChilds = new ArrayList();
            var expression = new AstNode(new Token(Token.TokenType.Operator, ">"));
            expression.addChild(new AstNode(new Token(Token.TokenType.Identifier, "a")));
            expression.addChild(new AstNode(new Token(Token.TokenType.Number, "2")));
            expectedChilds.Add(expression);
            var ifBody = new AstNode(AstNode.NodeType.IfBody);
            ifBody.SetNodeType(AstNode.NodeType.IfBody);
            
            AstNode statement = new AstNode(AstNode.NodeType.Statement);
            statement.SetNodeType(AstNode.NodeType.Statement);
            AstNode assigment = new AstNode(new Token(Token.TokenType.Operator,":="));
            AstNode id = new AstNode(new Token(Token.TokenType.Identifier,"a"));
            id.SetNodeType(AstNode.NodeType.Identifier);
            AstNode lit = new AstNode(new Token(Token.TokenType.Number,"2"));
            id.SetNodeType(AstNode.NodeType.Literal);
            assigment.addChild(id);
            assigment.addChild(lit);
            assigment.SetNodeType(AstNode.NodeType.Assignment);
            statement.addChild(assigment);
            ifBody.addChild(statement);
            AstNode ifBody2 = new AstNode(AstNode.NodeType.IfBody);
            ifBody2.SetNodeType(AstNode.NodeType.IfBody);
            expectedChilds.Add(ifBody);
            expectedChilds.Add(ifBody2);
            Assert.AreEqual(curChilds, expectedChilds);
        }

        [Test]
        public void parseStatementTest()
        {
            var tokenizer = new Tokenizer.Tokenizer(getTestFilePath("statement.txt"));
            var reader = new TokenReader(tokenizer);
            var node = Parser.Parser.parseStatement(reader);
            Assert.AreEqual(node.getValue(), AstNode.NodeType.Statement);

            var curChilds = node.getChilds();
            var expectedChilds = new ArrayList();
            var gotoNode = new AstNode(new Token(Token.TokenType.Keyword, "goto"));
            gotoNode.addChild(new AstNode(new Token(Token.TokenType.Identifier, "a")));
            expectedChilds.Add(gotoNode);
            Assert.AreEqual(curChilds, expectedChilds);

            node = Parser.Parser.parseStatement(reader);
            Assert.AreEqual(node.getValue(), AstNode.NodeType.Statement);

            curChilds = node.getChilds();
            expectedChilds = new ArrayList {new AstNode(new Token(Token.TokenType.Identifier, "id"))};
            gotoNode = new AstNode(new Token(Token.TokenType.Keyword, "goto"));
            gotoNode.addChild(new AstNode(new Token(Token.TokenType.Identifier, "a")));
            expectedChilds.Add(gotoNode);
            Assert.AreEqual(curChilds, expectedChilds);
        }

        [Test]
        public void parseLoopBodyTest()
        {
            var tokenizer = new Tokenizer.Tokenizer(getTestFilePath("loopBody.txt"));
            var reader = new TokenReader(tokenizer);
            var node = Parser.Parser.parseLoopBody(reader);
            Assert.AreEqual(node.GetNodeType(), AstNode.NodeType.LoopBody);

            var curChilds = node.getChilds();

            Assert.AreEqual(curChilds[0].GetNodeType(), AstNode.NodeType.Statement);
        }

        [Test]
        public void negativeLoopVarDefinitionTest()
        {
            var tokenizer = new Tokenizer.Tokenizer(getTestFilePath("varInLoop.txt"));
            var reader = new TokenReader(tokenizer);

            var exLoop = Assert.Throws<SyntaxError>(delegate { Parser.Parser.parseLoopBody(reader); });

            Assert.That(exLoop.Message, Is.EqualTo("Can't parse loop body"));
        }

        [Test]
        public void negativeConditionVarDefinitionTest()
        {
            var tokenizerIf = new Tokenizer.Tokenizer(getTestFilePath("varInIf.txt"));
            var readerIf = new TokenReader(tokenizerIf);

            var tokenizerElse = new Tokenizer.Tokenizer(getTestFilePath("varInElse.txt"));
            var readerElse = new TokenReader(tokenizerElse);

            var exIf = Assert.Throws<SyntaxError>(delegate { Parser.Parser.parseLoopBody(readerIf); });
            var exElse = Assert.Throws<SyntaxError>(delegate { Parser.Parser.parseLoopBody(readerElse); });

            Assert.That(exIf.Message, Is.EqualTo("todo"));
            Assert.That(exElse.Message, Is.EqualTo("todo"));
        }

        private static string getTestFilePath(string fileName)
        {
            return TestUtils.getTestFilePath(fileName);
        }
    }
}