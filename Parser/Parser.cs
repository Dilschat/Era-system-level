using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Erasystemlevel.Exception;
using Erasystemlevel.Tokenizer;

namespace Erasystemlevel.Parser
{
    public static class Parser
    {
        public static bool _debug = false;

        public static AstNode ParseUnit(TokenReader reader)
        {
            AstNode unit = new AstNode(AstNode.NodeType.Unit);
            unit.SetNodeType(AstNode.NodeType.Unit);
            while (true)
            {
                if (reader.isEmpty())
                {
                    break;
                }
                try
                {
                    var node = parseCode(reader);
                    reader.clear();
                    unit.addChild(node);      
                    continue;
                }
                catch (SyntaxError e)
                {
                    if (_debug) Console.WriteLine(e.Message);
                }

                try
                {
                    var node = parseData(reader);
                    reader.clear();
                    unit.addChild(node);
                    continue;
                }
                catch (SyntaxError e)
                {
                    if (_debug) Console.WriteLine(e.Message);
                }

                try
                {
                    var node = parseModule(reader);
                    reader.clear();
                    unit.addChild(node);
                    continue;
                }
                catch (SyntaxError e)
                {
                    if (_debug) Console.WriteLine(e.Message);
                }

                try
                {
                    var node = parseRoutine(reader);
                    reader.clear();
                    unit.addChild(node);
                    continue;
                }
                catch (SyntaxError e)
                {
                    if (_debug) Console.WriteLine(e.Message);
                }

                throw new SyntaxError("Can't parse unit");
            }

            return unit;

        }
        
        public static AstNode parseCode(TokenReader reader)
        {
            var nextToken = reader.readNextToken();
            var code = new AstNode(AstNode.NodeType.Code);
            checkToken(nextToken);
            if (!nextToken.GetValue().Equals("code"))
            {
                reader.saveReadTokens();
                throw new SyntaxError("Can't parse code");
            }

            reader.clear();
            while (true)
            {
                try
                {
                    code.addChild(parseVariable(reader));
                    continue;
                }
                catch (SyntaxError e)
                {
                    if (_debug) Console.WriteLine(e.Message);
                }

                try
                {
                    code.addChild(parseConstant(reader));
                    continue;
                }
                catch (SyntaxError e)
                {
                    if (_debug) Console.WriteLine(e.Message);
                }

                try
                {
                    code.addChild(parseStatement(reader));
                    continue;
                }
                catch (SyntaxError e)
                {
                    if (_debug) Console.WriteLine(e.Message);
                }

                nextToken = reader.readNextToken();

                if (nextToken.GetValue().Equals("end"))
                {
                    reader.clear();
                    code.SetNodeType(AstNode.NodeType.Code);
                    return code;
                }

                reader.saveReadTokens();
                throw new SyntaxError("Can't parse code");
            }
        }

        public static AstNode parseData(TokenReader reader)
        {
            var nextToken = reader.readNextToken();
            checkToken(nextToken);
            if (!nextToken.GetValue().Equals("data"))
            {
                reader.saveReadTokens();
                throw new SyntaxError("Can't parse data");
            }

            reader.clear();
            var node = new AstNode(parseIdentifier(reader));
            reader.clear();
            try
            {
                node.addChild(parseLiteral(reader));
                reader.clear();
            }

            catch (SyntaxError)
            {
                nextToken = reader.readNextToken();
                if (!nextToken.GetValue().Equals("end")) throw new SyntaxError("Can't parse data");
                reader.clear();
                node.SetNodeType(AstNode.NodeType.Data);
                return node;
            }

            while (true)
            {
                nextToken = reader.readNextToken();
                if (nextToken.GetValue().Equals("end"))
                {
                    reader.clear();
                    node.SetNodeType(AstNode.NodeType.Data);
                    return node;
                }

                if (nextToken.GetValue().Equals(","))
                {
                    node.addChild(parseLiteral(reader));
                }
            }
        }

        public static AstNode parseRoutine(TokenReader reader)
        {
            var node = new AstNode(AstNode.NodeType.Routine);
            try
            {
                node.addChild(parseAttribute(reader));
            }
            catch (SyntaxError)
            {
            }

            var nextToken = reader.readNextToken();
            checkToken(nextToken);
            if (!nextToken.GetValue().Equals("routine"))
            {
                reader.saveReadTokens();
                throw new SyntaxError("Can't parse routine");
            }

            node.addChild(parseIdentifier(reader));
            reader.clear();
            try
            {
                node.addChild(parseParameters(reader));
                nextToken = reader.readNextToken();
                if (!nextToken.GetValue().Equals(")"))
                {
                    throw new SyntaxError("Can't parse routine");
                }

                reader.clear();
            }
            catch (SyntaxError)
            {
                reader.saveReadTokens();
            }

            nextToken = reader.readNextToken();


            if (nextToken.GetValue().Equals(":>"))
            {
                node.addChild(parseResults(reader));
                nextToken = reader.readNextToken();
            }


            if (nextToken.GetValue().Equals(";"))
            {
                reader.clear();
                node.SetNodeType(AstNode.NodeType.Routine);
                return node;
            }
            else
            {
                reader.saveReadTokens();
            }

            nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("do")) throw new SyntaxError("Can't parse routine");
            reader.clear();
            node.addChild(parseRoutineBody(reader));
            nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("end"))
            {
                reader.saveReadTokens();
                throw new SyntaxError("");
            }

            reader.clear();
            node.SetNodeType(AstNode.NodeType.Routine);
            return node;
        }

        public static AstNode parseModule(TokenReader reader)
        {
            var nextToken = reader.readNextToken();
            checkToken(nextToken);
            if (!nextToken.GetValue().Equals("module"))
            {
                reader.saveReadTokens();
                throw new SyntaxError("Can't parse module");
            }

            reader.clear();
            var node = new AstNode(parseIdentifier(reader));
            reader.clear();
            while (true)
            {
                try
                {
                    nextToken = reader.readNextToken();
                    if (nextToken == null)
                    {
                        return node;
                    }

                    reader.saveReadTokens();
                    node.addChild(parseDeclaration(reader));
                }
                catch (SyntaxError)
                {
                    reader.saveReadTokens();
                    break;
                }
            }

            nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("end"))
            {
                reader.saveReadTokens();
                throw new SyntaxError("Can't parse module");
            }

            reader.clear();
            node.SetNodeType(AstNode.NodeType.Module);
            return node;
        }

        public static AstNode parseVariable(TokenReader reader)
        {
            var node = new AstNode(AstNode.NodeType.Variable);
            node.addChild(parseType(reader));
            node.addChild(parseVarDefinition(reader));
            var nextToken = reader.readNextToken();
            while (true)
            {
                if (nextToken.GetValue().Equals(","))
                {
                    node.addChild(parseVarDefinition(reader));
                    nextToken = reader.readNextToken();
                }
                else if (nextToken.GetValue().Equals(";"))
                {
                    reader.clear();
                    node.SetNodeType(AstNode.NodeType.Variable);
                    return node;
                }
                else
                {
                    reader.saveReadTokens();
                    throw new SyntaxError("Can't parse variable");
                }
            }
        }

        public static AstNode parseConstant(TokenReader reader)
        {
            var nextToken = reader.readNextToken();
            checkToken(nextToken);
            if (!nextToken.GetValue().Equals("const"))
            {
                reader.saveReadTokens();
                throw new SyntaxError("Can't parse constant");
            }

            var node = new AstNode(nextToken);
            node.addChild(parseConstDefinition(reader));
            while (true)
            {
                nextToken = reader.readNextToken();
                if (nextToken.GetValue().Equals(","))
                {
                    node.addChild(parseConstDefinition(reader));
                }
                else if (nextToken.GetValue().Equals(";"))
                {
                    reader.clear();
                    node.SetNodeType(AstNode.NodeType.Constant);
                    return node;
                }
                else
                {
                    reader.saveReadTokens();
                    throw new SyntaxError("Can't parse constant");
                }
            }
        }

        private static void checkToken(Token nextToken)
        {
            if (nextToken == null)
            {
                throw new SyntaxError("Can't check token");
            }
        }

        public static AstNode parseIdentifier(TokenReader reader)
        {
            var nextToken = reader.readNextToken();
            if (nextToken.GetTokenType() == Token.TokenType.Identifier)
            {
                AstNode node = new AstNode(nextToken);
                node.SetNodeType(AstNode.NodeType.Identifier);
                return node;
            }
            reader.saveReadTokens();
            throw new SyntaxError("Can't parse identifier");
        }

        public static AstNode parseLiteral(TokenReader reader)
        {
            var nextToken = reader.readNextToken();
            if (nextToken.GetTokenType() == Token.TokenType.Number)
            {
                reader.clear();
                AstNode node = new AstNode(nextToken);
                node.SetNodeType(AstNode.NodeType.Literal);
                return node;
            }

            reader.saveReadTokens();
            throw new SyntaxError("Can't parse literal");
        }

        public static AstNode parseDeclaration(TokenReader reader)
        {
            var variable = new AstNode("Declaration");
            try
            {
                return parseVariable(reader);
            }
            catch (SyntaxError)
            {
                reader.saveReadTokens();
            }

            try
            {
                return parseConstant(reader);
            }
            catch (SyntaxError)
            {
                reader.saveReadTokens();
            }

            try
            {
                return parseRoutine(reader);
            }
            catch (SyntaxError)
            {
                reader.saveReadTokens();
            }

            throw new SyntaxError("Can't parse declaration");
        }

        public static AstNode parseVarDefinition(TokenReader reader)
        {
            var node = new AstNode("VarDefinition");
            node.addChild(parseIdentifier(reader));
            var nextToken = reader.readNextToken();

            if (nextToken.GetValue().Equals(":="))
            {
                node.setValue(nextToken);
                node.addChild(parseExpression(reader));
                reader.clear();
                node.SetNodeType(AstNode.NodeType.VarDefinition);
                return node;
            }

            if (nextToken.GetValue().Equals("["))
            {
                node.setValue(new Token(Token.TokenType.Operator, "[]"));
                node.addChild(parseExpression(reader));
                nextToken = reader.readNextToken();
                if (!nextToken.GetValue().Equals("]")) throw new SyntaxError("Can't parse varDefinition");
                reader.clear();
                node.SetNodeType(AstNode.NodeType.ArrayElement);
                return node;
            }

            {
                reader.saveReadTokens();
                node.SetNodeType(AstNode.NodeType.VarDefinition);
                return node;
            }
        }

        public static AstNode parseType(TokenReader reader)
        {
            var nextToken = reader.readNextToken();
            if (nextToken == null)
            {
                throw new SyntaxError("");
            }

            if (nextToken.GetTokenType() != Token.TokenType.Keyword)
            {
                reader.saveReadTokens();
                throw new SyntaxError("Can't parse type (token: " + nextToken.ToJsonString() + ")");
            }

            if (nextToken.GetValue().Equals("int"))
            {
                reader.clear();
                AstNode node = new AstNode(nextToken);
                node.SetNodeType(AstNode.NodeType.Type);
                return node;
            }

            if (nextToken.GetValue().Equals("short"))
            {
                reader.clear();
                AstNode node = new AstNode(nextToken);
                node.SetNodeType(AstNode.NodeType.Type);
                return node;
            }

            if (nextToken.GetValue().Equals("byte"))
            {
                reader.clear();
                AstNode node = new AstNode(nextToken);
                node.SetNodeType(AstNode.NodeType.Type);
                return node;
            }

            reader.saveReadTokens();
            throw new SyntaxError("Can't parse type");
        }

        public static AstNode parseExpression(TokenReader reader)
        {
            var firstOperand = parseOperand(reader);

            reader.clear();
            try
            {
                var node = parseOperator(reader);
                try
                {
                    node.addChild(firstOperand);
                    node.addChild(parseOperand(reader));
                    reader.clear();
                    node.SetNodeType(AstNode.NodeType.Expression);
                    return node;
                }
                catch (SyntaxError)
                {
                    reader.saveReadTokens();
                    throw new SyntaxError("Can't parse expression");
                }
            }
            catch (SyntaxError)
            {
                reader.saveReadTokens();
                return firstOperand;
            }
        }

        public static AstNode parseConstDefinition(TokenReader reader)
        {
            var node = new AstNode("ConstDefinition");
            node.addChild(parseIdentifier(reader));
            var nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("="))
            {
                reader.saveReadTokens();
                throw new SyntaxError("Can't parse constDefinition");
            }

            node.setValue(nextToken);
            node.addChild(parseExpression(reader));
            reader.clear();
            node.SetNodeType(AstNode.NodeType.ConstDefinition);
            return node;
        }

        public static AstNode parseStatement(TokenReader reader)
        {
            var nextToken = reader.readNextToken();
            checkToken(nextToken);
            reader.saveReadTokens();
            var node = new AstNode(AstNode.NodeType.Statement);
            try
            {
                node.addChild(parseLabel(reader));
            }
            catch (SyntaxError)
            {
                reader.saveReadTokens();
            }

            try
            {
                node.addChild(parseAssemblerStatement(reader));
                reader.clear();
                node.SetNodeType(AstNode.NodeType.Statement);
                return node;
            }
            catch (SyntaxError)
            {
                reader.saveReadTokens();
            }

            try
            {
                node.addChild(parseExtensionStatement(reader));
                reader.clear();
                node.SetNodeType(AstNode.NodeType.Statement);
                return node;
            }
            catch (SyntaxError)
            {
                reader.saveReadTokens();
            }

            try
            {
                node.addChild(parseDirective(reader));
                reader.clear();
                node.SetNodeType(AstNode.NodeType.Statement);
                return node;
            }
            catch (SyntaxError)
            {
                reader.saveReadTokens();
                throw new SyntaxError("Cant parse statement");
            }
        }

        public static AstNode parseLabel(TokenReader reader)
        {
            var nextToken = reader.readNextToken();
            var node = new AstNode("Label");
            checkToken(nextToken);
            if (!nextToken.GetValue().Equals("<"))
            {
                reader.saveReadTokens();
                throw new SyntaxError("Can't parse label");
            }

            nextToken = reader.readNextToken();
            if (nextToken.GetTokenType() != Token.TokenType.Identifier)
            {
                reader.saveReadTokens();
                throw new SyntaxError("Can't parse label");
            }

            node.setValue(nextToken);
            nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals(">"))
            {
                reader.saveReadTokens();
                throw new SyntaxError("Can't parse label");
            }

            reader.clear();
            node.SetNodeType(AstNode.NodeType.Label);
            return node;
        }

        public static AstNode parseAssemblerStatement(TokenReader reader)
        {
            var node = new AstNode("AssemblerStatement");
            var nextToken = reader.readNextToken();
            if (nextToken.GetValue().Equals("skip"))
            {
                node = new AstNode(nextToken);
                try
                {
                    node.addChild(parseExpression(reader));
                    reader.clear();
                    node.SetNodeType(AstNode.NodeType.AssemblerStatement);
                    return node;
                }
                catch (SyntaxError)
                {
                    reader.saveReadTokens();
                    reader.clear();
                    node.SetNodeType(AstNode.NodeType.AssemblerStatement);
                    return node;
                }
            }

            if (nextToken.GetValue().Equals("stop"))
            {
                node = new AstNode(nextToken);
                try
                {
                    node.addChild(parseExpression(reader));
                    reader.clear();
                    node.SetNodeType(AstNode.NodeType.AssemblerStatement);
                    return node;
                }
                catch (SyntaxError)
                {
                    reader.saveReadTokens();
                    reader.clear();
                    node.SetNodeType(AstNode.NodeType.AssemblerStatement);
                    return node;
                }
            }

            var operators = new List<string>(new[]
                {":=", "+=", ">>=", "-=", "<<=", "|=", "&=", "^=", "<=", ">=", "?="});
            try
            {
                reader.saveReadTokens();
                node = parseOperationOnRegisters(operators, reader);
            }
            catch (SyntaxError)
            {
                try
                {
                    if (nextToken.GetValue().Equals("if"))
                    {
                        node = new AstNode(nextToken);
                        node.addChild(parseRegister(reader));
                        var newToken = reader.readNextToken();
                        if (!nextToken.GetValue().Equals("goto"))
                        {
                            throw new SyntaxError("Goto expected");
                        }

                        var goTo = new AstNode(newToken);
                        goTo.addChild(parseRegister(reader));
                        node.addChild(goTo);
                    }
                    else
                    {
                        reader.saveReadTokens();
                        throw new SyntaxError("Can't parse assembler statement");
                    }
                }
                catch (SyntaxError)
                {
                    reader.saveReadTokens();
                    throw new SyntaxError("Can't parse assembler statement");
                }
            }

            reader.clear();
            node.SetNodeType(AstNode.NodeType.AssemblerStatement);
            return node;
        }

        private static AstNode parseOperationOnRegisters(IEnumerable<string> operators, TokenReader reader)
        {
            AstNode leftOperand = null;
            try
            {
                leftOperand = parseOperator(reader);
            }
            catch (SyntaxError)
            {
                reader.saveReadTokens();
            }

            if (leftOperand != null && ((Token) leftOperand.getValue()).GetValue().Equals("*"))
            {
                leftOperand.addChild(parseRegister(reader));
            }
            else
            {
                leftOperand = parseRegister(reader);
            }

            var operation = reader.readNextToken();
            var exists = operators.Any(i => i.Equals(operation.GetValue()));

            reader.clear();
            if (exists == false)
            {
                reader.saveReadTokens();
                throw new SyntaxError("Can't parse operation on registers");
            }

            AstNode rightOperand = null;
            try
            {
                rightOperand = parseOperator(reader);
            }
            catch (SyntaxError)
            {
                reader.saveReadTokens();
            }

            try
            {
                if (rightOperand != null && ((Token) rightOperand.getValue()).GetValue().Equals("*"))
                {
                    rightOperand.addChild(parseRegister(reader));
                }
                else
                {
                    rightOperand = parseRegister(reader);
                }
            }
            catch (SyntaxError)
            {
                rightOperand = parseExpression(reader);
            }

            var node = new AstNode(operation);
            node.addChild(leftOperand);
            node.addChild(rightOperand);
            reader.clear();
            node.SetNodeType(AstNode.NodeType.OperationOnRegisters);
            return node;
        }

        public static AstNode parseAttribute(TokenReader reader)
        {
            var nextToken = reader.readNextToken();
            checkToken(nextToken);
            if (nextToken.GetValue().Equals("start"))
            {
                reader.clear();
                return new AstNode(nextToken);
            }

            if (nextToken.GetValue().Equals("entry"))
            {
                reader.clear();
                return new AstNode(nextToken);
            }

            reader.saveReadTokens();
            throw new SyntaxError("Can't parse attribute");
        }

        public static AstNode parseParameters(TokenReader reader)
        {
            var node = new AstNode(AstNode.NodeType.Parameters);
            var nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("("))
            {
                reader.saveReadTokens();
                throw new SyntaxError("Cant parse parameters");
            }

            reader.clear();
            try
            {
                node.addChild(parseParameter(reader));
                nextToken = reader.readNextToken();
            }
            catch
            {
                reader.saveReadTokens();
                node.SetNodeType(AstNode.NodeType.Parameters);
                return node;
            }

            while (true)
            {
                if (nextToken.GetValue().Equals(","))
                {
                    node.addChild(parseParameter(reader));
                    nextToken = reader.readNextToken();
                }
                else
                {
                    reader.saveReadTokens();
                    break;
                }
            }

            if (!nextToken.GetValue().Equals(")"))
            {
                reader.saveReadTokens();
                throw new SyntaxError("Cant parse parameters");
            }

            reader.clear();
            node.SetNodeType(AstNode.NodeType.Parameters);
            return node;
        }

        public static AstNode parseResults(TokenReader reader)
        {
            var node = new AstNode(AstNode.NodeType.Results);
            node.addChild(parseRegister(reader));
            var nextToken = reader.readNextToken();
            while (true)
            {
                if (nextToken.GetValue().Equals(","))
                {
                    node.addChild(parseRegister(reader));
                    nextToken = reader.readNextToken();
                }
                else
                {
                    reader.saveReadTokens();
                    reader.clear();
                    node.SetNodeType(AstNode.NodeType.Results);
                    return node;
                }
            }
        }

        public static AstNode parseParameter(TokenReader reader)
        {
            try
            {
                var node = parseType(reader);
                node.addChild(parseIdentifier(reader));
                reader.clear();
                node.SetNodeType(AstNode.NodeType.Parameter);
                return node;
            }
            catch (SyntaxError)
            {
                reader.saveReadTokens();
            }

            try
            {
                return parseRegister(reader);
            }
            catch (SyntaxError)
            {
                reader.saveReadTokens();
                throw new SyntaxError("Cant parse parameter");
            }
        }

        public static AstNode parseRoutineBody(TokenReader reader)
        {
            var node = new AstNode(AstNode.NodeType.RoutineBody);
            while (true)
            {
                try
                {
                    node.addChild(parseVariable(reader));
                    continue;
                }
                catch (SyntaxError)
                {
                    reader.saveReadTokens();
                }

                try
                {
                    node.addChild(parseConstant(reader));
                    continue;
                }
                catch (SyntaxError)
                {
                    reader.saveReadTokens();
                }

                try
                {
                    node.addChild(parseStatement(reader));
                    continue;
                }
                catch (SyntaxError)
                {
                    reader.saveReadTokens();
                }

                break;
            }

            reader.clear();
            node.SetNodeType(AstNode.NodeType.RoutineBody);
            return node;
        }

        public static AstNode parsePrimary(TokenReader reader)
        {
            try
            {
                reader.clear();
                return parseDereference(reader);
            }
            catch (SyntaxError)
            {
                reader.saveReadTokens();
            }

            try
            {
                reader.clear();
                return parseArrayElement(reader);
            }
            catch (SyntaxError)
            {
                reader.saveReadTokens();
            }

            try
            {
                reader.clear();
                return parseDataElement(reader);
            }
            catch (SyntaxError)
            {
                reader.saveReadTokens();
            }

            try
            {
                reader.clear();
                return parseRegister(reader);
            }
            catch (SyntaxError)
            {
                reader.saveReadTokens();
            }

            try
            {
                reader.clear();
                return parseExplicitAddress(reader);
            }
            catch (SyntaxError)
            {
                reader.saveReadTokens();
            }

            try
            {
                reader.clear();
                return parseVariableReference(reader);
            }
            catch (SyntaxError)
            {
                reader.saveReadTokens();
            }

            throw new SyntaxError("Cant parse primary");
        }

        public static AstNode parseVariableReference(TokenReader reader)
        {
            AstNode node = parseIdentifier(reader);
            node.SetNodeType(AstNode.NodeType.VariableReference);
            return node;
           
        }

        public static AstNode parseDereference(TokenReader reader)
        {
            var dereference = new AstNode(null);
            var nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("*"))
            {
                reader.saveReadTokens();
                throw new SyntaxError("Can't parse deference");
            }

            dereference.setValue(nextToken);
            try
            {
                reader.clear();
                dereference.addChild(parseVariableReference(reader));
                return dereference;
            }
            catch (SyntaxError)
            {
                reader.saveReadTokens();
            }

            try
            {
                reader.clear();
                dereference.addChild(parseRegister(reader));
                return dereference;
            }
            catch (SyntaxError)
            {
                reader.saveReadTokens();
            }

            throw new SyntaxError("Cant parse dereference");
        }

        public static AstNode parseArrayElement(TokenReader reader)
        {
            var arrayElement = parseVariableReference(reader);
            var nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("["))
            {
                reader.saveReadTokens();
                throw new SyntaxError("Can't parse array element");
            }

            arrayElement.addChild(parseExpression(reader));
            nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("]"))
            {
                reader.saveReadTokens();
                throw new SyntaxError("Can't parse array element");
            }

            reader.clear();
            return arrayElement;
        }

        public static AstNode parseDataElement(TokenReader reader)
        {
            var dataElement = parseIdentifier(reader);
            var nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("["))
            {
                reader.saveReadTokens();
                throw new SyntaxError("Can't parse data element");
            }

            dataElement.addChild(parseExpression(reader));
            nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("]"))
            {
                reader.saveReadTokens();
                throw new SyntaxError("Can't parse data element");
            }

            reader.clear();
            return dataElement;
        }

        public static AstNode parseExplicitAddress(TokenReader reader)
        {
            var nextToken = reader.readNextToken();
            var explicitAddress = new AstNode(nextToken);
            if (!nextToken.GetValue().Equals("*"))
            {
                reader.saveReadTokens();
                throw new SyntaxError("Can't parse explicit address");
            }

            explicitAddress.addChild(parseLiteral(reader));
            reader.clear();
            return explicitAddress;
        }

        public static AstNode parseOperand(TokenReader reader)
        {
            try
            {
                reader.clear();
                return parseReceiver(reader);
            }
            catch (SyntaxError)
            {
                reader.saveReadTokens();
            }

            try
            {
                reader.clear();
                return parseLiteral(reader);
            }
            catch (SyntaxError)
            {
                reader.saveReadTokens();
            }

            try
            {
                reader.clear();
                return parseIdentifier(reader);
            }
            catch (SyntaxError)
            {
                reader.saveReadTokens();
            }

            try
            {
                reader.clear();
                return parseAddress(reader);
            }
            catch (SyntaxError)
            {
                reader.saveReadTokens();
                throw new SyntaxError("Can't parse operand");
            }
        }

        public static AstNode parseAddress(TokenReader reader)
        {
            var address = new AstNode("Address");
            var nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("&"))
            {
                reader.saveReadTokens();
                throw new SyntaxError("Can't parse address");
            }

            address.setValue(nextToken);
            address.addChild(parseVariableReference(reader));
            reader.clear();
            return address;
        }

        public static AstNode parseReceiver(TokenReader reader)
        {
            return parsePrimary(reader);
        }

        private static AstNode parseOperator(TokenReader reader)
        {
            var nextToken = reader.readNextToken();
            const Token.TokenType operatorType = Token.TokenType.Operator;
            if (!nextToken.GetTokenType().Equals(operatorType)) throw new SyntaxError("Can't parse operator");
            reader.clear();
            return new AstNode(nextToken);
        }


        public static AstNode parseRegister(TokenReader reader)
        {
            var nextToken = reader.readNextToken();
            if (nextToken.GetTokenType().Equals(Token.TokenType.Register))
            {
                reader.clear();
                return new AstNode(nextToken);
            }

            reader.saveReadTokens();
            throw new SyntaxError("Can't parse register");
        }

        public static AstNode parseDirective(TokenReader reader)
        {
            var directive = new AstNode("Directive");
            var nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("format"))
            {
                reader.saveReadTokens();
                throw new SyntaxError("Can't parse directive");
            }

            directive.setValue(nextToken);
            nextToken = reader.readNextToken();
            if (nextToken.GetValue().Equals("8") || nextToken.GetValue().Equals("16")
                                                 || nextToken.GetValue().Equals("32"))
            {
                directive.addChild(new AstNode(nextToken));
            }
            else
            {
                reader.saveReadTokens();
                throw new SyntaxError("Can't parse directive");
            }

            reader.clear();
            return directive;
        }

        public static AstNode parseExtensionStatement(TokenReader reader)
        {
            var node = new AstNode("ExtensionStatement");
            try
            {
                return parseAssignment(reader);
            }
            catch (SyntaxError)
            {
                reader.saveReadTokens();
            }

            try
            {
                return parseCall(reader);
            }
            catch (SyntaxError)
            {
                reader.saveReadTokens();
            }

            try
            {
                return parseIf(reader);
            }
            catch (SyntaxError)
            {
                reader.saveReadTokens();
            }

            try
            {
                return parseWhile(reader);
            }
            catch (SyntaxError)
            {
                reader.saveReadTokens();
            }

            try
            {
                return parseFor(reader);
            }
            catch (SyntaxError)
            {
                reader.saveReadTokens();
            }

            try
            {
                return parseBreak(reader);
            }
            catch (SyntaxError)
            {
                reader.saveReadTokens();
            }

            try
            {
                return parseSwap(reader);
            }
            catch (SyntaxError)
            {
                reader.saveReadTokens();
            }

            try
            {
                return parseGoto(reader);
            }
            catch (SyntaxError)
            {
                reader.saveReadTokens();
            }

            throw new SyntaxError("Can't parse extension statement");
        }

        public static AstNode parseIf(TokenReader reader)
        {
            var nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("if"))
            {
                reader.saveReadTokens();
                throw new SyntaxError("Can't parse if");
            }

            var ifNode = new AstNode(nextToken);
            ifNode.addChild(parseExpression(reader));
            ifNode.addChild(parseIfBody(reader));
            nextToken = reader.readNextToken();
            if (nextToken.GetValue().Equals("else"))
            {
                ifNode.addChild(parseIfBody(reader));
            }
            else
            {
                reader.saveReadTokens();
            }

            reader.clear();
            ifNode.SetNodeType(AstNode.NodeType.If);
            return ifNode;
        }

        private static AstNode parseIfBody(TokenReader reader)
        {
            var ifBody = new AstNode(AstNode.NodeType.IfBody);
            var nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("do"))
            {
                reader.saveReadTokens();
                throw new SyntaxError("Can't parse if body");
            }

            reader.clear();
            while (true)
            {
                try
                {
                    ifBody.addChild(parseStatement(reader));
                }
                catch (SyntaxError)
                {
                    reader.saveReadTokens();
                    nextToken = reader.readNextToken();
                    if (!nextToken.GetValue().Equals("end"))
                    {
                        reader.saveReadTokens();
                        throw new SyntaxError("Can't parse if body");
                    }

                    reader.clear();
                    ifBody.SetNodeType(AstNode.NodeType.IfBody);
                    return ifBody;
                }
            }
        }

        public static AstNode
            parseCall(TokenReader reader)
        {
            var call = new AstNode("Call");
            var primary = parsePrimary(reader);
            var nextToken = reader.readNextToken();
            if (nextToken.GetValue().Equals("."))
            {
                var identifier = parseIdentifier(reader);
                var node = new AstNode(nextToken);
                node.addChild(primary);
                node.addChild(identifier);
                call.addChild(node);
                nextToken = reader.readNextToken();
            }
            else
            {
                var value = (Token) primary.getValue();
                if (!value.GetTokenType().Equals(Token.TokenType.Identifier))
                {
                    throw new SyntaxError("Can't parse if body");
                }

                call.addChild(primary);
            }

            if (!nextToken.GetValue().Equals("("))
            {
                reader.saveReadTokens();
                throw new SyntaxError("Can't parse if body");
            }

            reader.clear();
            call.addChild(parseCallParameters(reader));
            nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals(";"))
            {
                reader.saveReadTokens();
                throw new SyntaxError("Can't parse if body");
            }

            reader.clear();
            call.SetNodeType(AstNode.NodeType.Call);
            return call;
        }

        private static AstNode parseCallParameters(TokenReader reader)
        {
            var parameters = new AstNode("CallParameters");
            try
            {
                parameters.addChild(parseIdentifier(reader));
            }
            catch (SyntaxError)
            {
                var nextToken = reader.readNextToken();
                if (!nextToken.GetValue().Equals(")"))
                {
                    throw new SyntaxError("Can't parse call parameters");
                }

                reader.clear();
                parameters.SetNodeType(AstNode.NodeType.CallParameters);
                return parameters;
            }

            while (true)
            {
                var nextToken = reader.readNextToken();
                if (nextToken.GetValue().Equals(","))
                {
                    parameters.addChild(parseIdentifier(reader));
                    continue;
                }

                if (!nextToken.GetValue().Equals(")"))
                {
                    reader.saveReadTokens();
                    throw new SyntaxError("Can't parse call parameters");
                }

                reader.clear();
                parameters.SetNodeType(AstNode.NodeType.CallParameters);
                return parameters;
            }
        }

        public static AstNode parseFor(TokenReader reader)
        {
            var forStatement = new AstNode("For");
            var nextToken = reader.readNextToken();
            if (nextToken.GetValue().Equals("end"))
            {
                reader.saveReadTokens();
                throw new SyntaxError("Can't parse for");
            }

            if (nextToken.GetValue().Equals("for"))
            {
                var forNode = new AstNode(nextToken);
                forStatement = forNode;
                forNode.addChild(parseIdentifier(reader));
                reader.clear();
                nextToken = reader.readNextToken();
            }

            if (nextToken.GetValue().Equals("from"))
            {
                var fromStatement = new AstNode(nextToken);
                fromStatement.addChild(parseExpression(reader));
                fromStatement.SetNodeType(AstNode.NodeType.From);
                forStatement.addChild(fromStatement);
                nextToken = reader.readNextToken();
            }
            else
            {
                var fromStatement = new AstNode(new Token(Token.TokenType.Keyword, "from"));
                var expression = new AstNode(new Token(Token.TokenType.Number, "0"));
                expression.SetNodeType(AstNode.NodeType.Expression);
                fromStatement.addChild(expression);
                fromStatement.SetNodeType(AstNode.NodeType.From);
                forStatement.addChild(fromStatement);
            }

            if (nextToken.GetValue().Equals("to"))
            {
                var toStatement = new AstNode(nextToken);
                toStatement.addChild(parseExpression(reader));
                toStatement.SetNodeType(AstNode.NodeType.To);
                forStatement.addChild(toStatement);
                nextToken = reader.readNextToken();
            }
            else
            {
                var toStatement = new AstNode(new Token(Token.TokenType.Keyword, "to"));
                toStatement.addChild(new AstNode(new Token(Token.TokenType.Number, "0")));
                toStatement.SetNodeType(AstNode.NodeType.To);
                forStatement.addChild(toStatement);
            }

            if (nextToken.GetValue().Equals("step"))
            {
                var stepNode = new AstNode(nextToken);
                stepNode.addChild(parseExpression(reader));
                stepNode.SetNodeType(AstNode.NodeType.Step);
                forStatement.addChild(stepNode);
            }
            else
            {
                var stepNode = new AstNode(new Token(Token.TokenType.Keyword, "step"));
                stepNode.addChild(new AstNode(new Token(Token.TokenType.Number, "1")));
                stepNode.SetNodeType(AstNode.NodeType.Step);
                forStatement.addChild(stepNode);
            }

            reader.saveReadTokens();
            forStatement.addChild(parseLoopBody(reader));
            reader.clear();
            forStatement.SetNodeType(AstNode.NodeType.For);
            return forStatement;
        }

        public static AstNode parseWhile(TokenReader reader)
        {
            var whileStatement = new AstNode(AstNode.NodeType.While);
            var nextToken = reader.readNextToken();
            if (nextToken.GetValue().Equals("end"))
            {
                throw new SyntaxError("Can't parse while");
            }

            if (nextToken.GetValue().Equals("while"))
            {
                try
                {
                    whileStatement.addChild(parseExpression(reader));
                }
                catch (SyntaxError)
                {
                    reader.saveReadTokens();
                }
            }

            whileStatement.addChild(parseLoopBody(reader));
            reader.clear();
            whileStatement.SetNodeType(AstNode.NodeType.While);
            return whileStatement;
        }

        public static AstNode parseLoopBody(TokenReader reader)
        {
            var loopBody = new AstNode(AstNode.NodeType.LoopBody);
            var nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("loop"))
            {
                reader.saveReadTokens();
                throw new SyntaxError("Can't parse loop body");
            }

            reader.clear();
            while (true)
            {
                try
                {
                    loopBody.addChild(parseStatement(reader));
                }
                catch (SyntaxError)
                {
                    reader.saveReadTokens();
                    nextToken = reader.readNextToken();
                    if (!nextToken.GetValue().Equals("end"))
                    {
                        reader.saveReadTokens();
                        throw new SyntaxError("Can't parse loop body");
                    }

                    reader.clear();
                    loopBody.SetNodeType(AstNode.NodeType.LoopBody);
                    return loopBody;
                }
            }
        }

        public static AstNode parseBreak(TokenReader reader)
        {
            var breakNode = new AstNode(null);
            var nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("break"))
            {
                reader.saveReadTokens();
                throw new SyntaxError("Can't parse break");
            }

            breakNode.setValue(nextToken);
            reader.clear();
            breakNode.SetNodeType(AstNode.NodeType.Break);
            nextToken = reader.readNextToken();
            checkToken(nextToken);
            if (!nextToken.GetValue().Equals(";"))
            {
                throw new SyntaxError("Can't parse break");
            }

            return breakNode;
        }

        public static AstNode parseSwap(TokenReader reader)
        {
            var swap = new AstNode(null);
            swap.addChild(parsePrimary(reader));
            var nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("<=>"))
            {
                reader.saveReadTokens();
                throw new SyntaxError("Can't parse swap");
            }

            swap.setValue(nextToken);
            swap.addChild(parsePrimary(reader));
            nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals(";"))
            {
                reader.saveReadTokens();
                throw new SyntaxError("Can't parse swap");
            }

            reader.clear();
            swap.SetNodeType(AstNode.NodeType.Swap);
            return swap;
        }

        public static AstNode parseGoto(TokenReader reader)
        {
            var goTo = new AstNode("goto");
            var nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("goto"))
            {
                reader.saveReadTokens();
                throw new SyntaxError("Can't parse goto");
            }

            goTo.setValue(nextToken);
            goTo.addChild(parseIdentifier(reader));
            var delimiter = reader.readNextToken();
            if (!delimiter.GetValue().Equals(";"))
            {
                throw new SyntaxError("Can't parse goto");
            }

            reader.clear();
            goTo.SetNodeType(AstNode.NodeType.Goto);
            return goTo;
        }

        public static AstNode parseAssignment(TokenReader reader)
        {
            var assignment = new AstNode(null);
            assignment.addChild(parsePrimary(reader));
            var nextToken = reader.readNextToken();
            if (nextToken.GetValue().Equals(":="))
            {
                assignment.setValue(nextToken);
            }
            else
            {
                reader.saveReadTokens();
                throw new SyntaxError("Can't parse assignment");
            }

            assignment.addChild(parseExpression(reader));
            nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals(";"))
            {
                throw new SyntaxError("Can't parse assignment");
            }

            reader.clear();
            assignment.SetNodeType(AstNode.NodeType.Assignment);
            return assignment;
        }
        
       
    }
}