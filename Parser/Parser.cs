using System;
using System.Collections.Generic;
using System.Linq;
using Erasystemlevel.Exception;
using Erasystemlevel.Tokenizer;

namespace Erasystemlevel.Parser
{
    public static class Parser
    {
        public static AstNode ParseUnit(TokenReader reader)
        {
            try
            {
                var node = parseCode(reader);
                reader.clear();
                return node;
            }
            catch (SyntaxError e)
            {
                Console.WriteLine(e);
            }

            Console.WriteLine('1');
            try
            {
                var node = parseData(reader);
                reader.clear();
                return node;
            }
            catch (SyntaxError e)
            {
                Console.WriteLine(e);
            }

            Console.WriteLine('2');

            try
            {
                var node = parseModule(reader);
                reader.clear();
                return node;
            }
            catch (SyntaxError e)
            {
                Console.WriteLine(e);
            }

            try
            {
                var node = parseRoutine(reader);
                reader.clear();
                return node;
            }
            catch (SyntaxError e)
            {
                Console.WriteLine(e);
            }

            throw new SyntaxError("Can't parse unit");
        }

        public static AstNode parseCode(TokenReader reader)
        {
            var nextToken = reader.readNextToken();
            var code = new AstNode(AstNode.NodeType.Code);
            if (!nextToken.GetValue().Equals("code"))
            {
                reader.SaveReadTokens();
                throw new SyntaxError("Can't parse code");
            }

            while (true)
            {
                try
                {
                    code.addChild(parseVariable(reader));
                    continue;
                }
                catch (SyntaxError e)
                {
                    Console.WriteLine(e);
                }

                try
                {
                    code.addChild(parseConstant(reader));
                    continue;
                }
                catch (SyntaxError e)
                {
                    Console.WriteLine(e);
                }

                try
                {
                    code.addChild(parseStatement(reader));
                    continue;
                }
                catch (SyntaxError e)
                {
                    Console.WriteLine(e);
                }

                nextToken = reader.readNextToken();

                if (nextToken.GetValue().Equals("end"))
                {
                    reader.clear();
                    code.SetNodeType(AstNode.NodeType.Code);
                    return code;
                }

                reader.SaveReadTokens();
                throw new SyntaxError("Can't parse code");
            }
        }

        public static AstNode parseData(TokenReader reader)
        {
            Token nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("data"))
            {
                reader.SaveReadTokens();
                throw new SyntaxError("Can't parse data");
            }

            reader.clear();
            AstNode node = new AstNode(parseIdentifier(reader));
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
            catch (SyntaxError e)
            {
                Console.WriteLine(e);
            }

            var nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("routine"))
            {
                reader.SaveReadTokens();
                throw new SyntaxError("Can't parse routine");
            }

            node.addChild(parseIdentifier(reader));
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
            catch (SyntaxError e)
            {
                Console.WriteLine(e);
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

            reader.SaveReadTokens();

            nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("do")) throw new SyntaxError("Can't parse routine");
            reader.clear();
            node.addChild(parseRoutineBody(reader));
            nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("end"))
            {
                reader.SaveReadTokens();
                throw new SyntaxError("Can't parse routine");
            }

            reader.clear();
            node.SetNodeType(AstNode.NodeType.Routine);
            return node;
        }

        public static AstNode parseModule(TokenReader reader)
        {
            var nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("module"))
            {
                reader.SaveReadTokens();
                throw new SyntaxError("Can't parse module");
            }

            reader.clear();
            var node = new AstNode(parseIdentifier(reader));
            reader.clear();
            while (true)
            {
                try
                {
                    node.addChild(parseDeclaration(reader));
                }
                catch (SyntaxError)
                {
                    reader.SaveReadTokens();
                    break;
                }
            }

            nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("end"))
            {
                reader.SaveReadTokens();
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
                    reader.SaveReadTokens();
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
                reader.SaveReadTokens();
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
                    reader.SaveReadTokens();
                    throw new SyntaxError("Can't parse constant");
                }
            }
        }

        private static void checkToken(Token nextToken)
        {
            if (nextToken == null) throw new ArgumentNullException(nameof(nextToken));
        }

        public static AstNode parseIdentifier(TokenReader reader)
        {
            var nextToken = reader.readNextToken();
            if (nextToken.GetTokenType() == Token.TokenType.Identifier) return new AstNode(nextToken);
            reader.SaveReadTokens();
            throw new SyntaxError("Can't parse identifier");
        }

        public static AstNode parseLiteral(TokenReader reader) //Literal:  Numeric
        {
            var nextToken = reader.readNextToken();
            if (nextToken.GetTokenType() == Token.TokenType.Number)
            {
                reader.clear();
                return new AstNode(nextToken);
            }

            reader.SaveReadTokens();
            throw new SyntaxError("Can't parse literal");
        }

        public static AstNode parseDeclaration(TokenReader reader)
        {
            var variable = new AstNode("Declaration");
            try
            {
//                variable.addChild(parseVariable(reader));
//                reader.clear();
//                return variable;
                return parseVariable(reader);
            }
            catch (SyntaxError)
            {
                reader.SaveReadTokens();
            }

            try
            {
//                variable.addChild(parseConstant(reader));
//                reader.clear();
//                return variable;
                return parseConstant(reader);
            }
            catch (SyntaxError)
            {
                reader.SaveReadTokens();
            }

            try
            {
//                variable.addChild(parseRoutine(reader));
//                reader.clear();
//                return variable;
                return parseRoutine(reader);
            }
            catch (SyntaxError)
            {
                reader.SaveReadTokens();
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
                node.setValue(new Token(Token.TokenType.Operator, ":="));
                node.addChild(parseExpression(reader));
                nextToken = reader.readNextToken();
                if (!nextToken.GetValue().Equals("]")) throw new SyntaxError("Can't parse varDefinition");
                reader.clear();
                node.SetNodeType(AstNode.NodeType.VarDefinition);
                return node;
            }

            {
                reader.SaveReadTokens();
                node.SetNodeType(AstNode.NodeType.VarDefinition);
                return node;
            }
        }

        public static AstNode parseType(TokenReader reader)
        {
            var nextToken = reader.readNextToken();
            if (nextToken == null)
            {
                throw new SyntaxError("Can't parse type");
            }

            if (nextToken.GetTokenType() != Token.TokenType.Keyword)
            {
                reader.SaveReadTokens();
                throw new SyntaxError("Can't parse type");
            }

            if (nextToken.GetValue().Equals("int"))
            {
                reader.clear();
                return new AstNode(nextToken);
            }

            if (nextToken.GetValue().Equals("short"))
            {
                reader.clear();
                return new AstNode(nextToken);
            }

            if (nextToken.GetValue().Equals("byte"))
            {
                reader.clear();
                return new AstNode(nextToken);
            }

            reader.SaveReadTokens();
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
                    reader.SaveReadTokens();
                    throw new SyntaxError("Can't parse expression");
                }
            }
            catch (SyntaxError)
            {
                reader.SaveReadTokens();
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
                reader.SaveReadTokens();
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
            reader.SaveReadTokens();
            var node = new AstNode(AstNode.NodeType.Statement);
            try
            {
                node.addChild(parseLabel(reader));
            }
            catch (SyntaxError)
            {
                reader.SaveReadTokens();
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
                reader.SaveReadTokens();
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
                reader.SaveReadTokens();
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
                reader.SaveReadTokens();
                throw new SyntaxError("Cant parse statement");
            }
        }

        public static AstNode parseLabel(TokenReader reader)
        {
            var nextToken = reader.readNextToken();
            var node = new AstNode("Label");
            if (!nextToken.GetValue().Equals("<"))
            {
                reader.SaveReadTokens();
                throw new SyntaxError("Can't parse label");
            }

            nextToken = reader.readNextToken();
            if (nextToken.GetTokenType() != Token.TokenType.Identifier)
            {
                reader.SaveReadTokens();
                throw new SyntaxError("Can't parse label");
            }

            node.setValue(nextToken);
            nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals(">"))
            {
                reader.SaveReadTokens();
                throw new SyntaxError("Can't parse label");
            }

            reader.clear();
            node.SetNodeType(AstNode.NodeType.Label);
            return node;
        }

        public static AstNode parseAssemblerStatement(TokenReader reader)
        {
            AstNode node;
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
                    reader.SaveReadTokens();
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
                    reader.SaveReadTokens();
                    reader.clear();
                    node.SetNodeType(AstNode.NodeType.AssemblerStatement);
                    return node;
                }
            }

            var operators = new List<string>(new[]
                {":=", "+=", ">>=", "-=", "<<=", "|=", "&=", "^=", "<=", ">=", "?="});
            try
            {
                reader.SaveReadTokens();
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
                            throw new SyntaxError("Can't parse assembler statement");
                        }

                        var goTo = new AstNode(newToken);
                        goTo.addChild(parseRegister(reader));
                        node.addChild(goTo);
                    }
                    else
                    {
                        reader.SaveReadTokens();
                        throw new SyntaxError("Can't parse assembler statement");
                    }
                }
                catch (SyntaxError)
                {
                    reader.SaveReadTokens();
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
                reader.SaveReadTokens();
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
                reader.SaveReadTokens();
                throw new SyntaxError("Can't parse operation on registers");
            }

            AstNode rightOperand = null;
            try
            {
                rightOperand = parseOperator(reader);
            }
            catch (SyntaxError)
            {
                reader.SaveReadTokens();
            }

            if (rightOperand != null && ((Token) rightOperand.getValue()).GetValue().Equals("*"))
            {
                rightOperand.addChild(parseRegister(reader));
            }
            else
            {
                rightOperand = parseRegister(reader);
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

            reader.SaveReadTokens();
            throw new SyntaxError("Can't parse attribute");
        }

        public static AstNode parseParameters(TokenReader reader)
        {
            var node = new AstNode(AstNode.NodeType.Parameters);
            var nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("("))
            {
                reader.SaveReadTokens();
                throw new SyntaxError("Cant parse parameters");
            }

            try
            {
                node.addChild(parseParameter(reader));
                nextToken = reader.readNextToken();
            }
            catch
            {
                reader.SaveReadTokens();
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
                    reader.SaveReadTokens();
                    break;
                }
            }

            if (!nextToken.GetValue().Equals(")"))
            {
                reader.SaveReadTokens();
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
                    reader.SaveReadTokens();
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
                reader.SaveReadTokens();
            }

            try
            {
                return parseRegister(reader);
            }
            catch (SyntaxError)
            {
                reader.SaveReadTokens();
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
                    reader.SaveReadTokens();
                }

                try
                {
                    node.addChild(parseConstant(reader));
                    continue;
                }
                catch (SyntaxError)
                {
                    reader.SaveReadTokens();
                }

                try
                {
                    node.addChild(parseStatement(reader));
                    continue;
                }
                catch (SyntaxError)
                {
                    reader.SaveReadTokens();
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
                reader.SaveReadTokens();
            }

            try
            {
                reader.clear();
                return parseArrayElement(reader);
            }
            catch (SyntaxError)
            {
                reader.SaveReadTokens();
            }

            try
            {
                reader.clear();
                return parseDataElement(reader);
            }
            catch (SyntaxError)
            {
                reader.SaveReadTokens();
            }

            try
            {
                reader.clear();
                return parseRegister(reader);
            }
            catch (SyntaxError)
            {
                reader.SaveReadTokens();
            }

            try
            {
                reader.clear();
                return parseExplicitAddress(reader);
            }
            catch (SyntaxError)
            {
                reader.SaveReadTokens();
            }

            try
            {
                reader.clear();
                return parseVariableReference(reader);
            }
            catch (SyntaxError)
            {
                reader.SaveReadTokens();
            }

            throw new SyntaxError("Cant parse primary");
        }

        public static AstNode parseVariableReference(TokenReader reader)
        {
            return parseIdentifier(reader);
        }

        public static AstNode parseDereference(TokenReader reader)
        {
            var dereference = new AstNode(null);
            var nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("*"))
            {
                reader.SaveReadTokens();
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
                reader.SaveReadTokens();
            }

            try
            {
                reader.clear();
                dereference.addChild(parseRegister(reader));
                return dereference;
            }
            catch (SyntaxError)
            {
                reader.SaveReadTokens();
            }

            throw new SyntaxError("Cant parse dereference");
        }

        public static AstNode parseArrayElement(TokenReader reader)
        {
            var arrayElement = parseVariableReference(reader);
            var nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("["))
            {
                reader.SaveReadTokens();
                throw new SyntaxError("Can't parse array element");
            }

            arrayElement.addChild(parseExpression(reader));
            nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("]"))
            {
                reader.SaveReadTokens();
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
                reader.SaveReadTokens();
                throw new SyntaxError("Can't parse data element");
            }

            dataElement.addChild(parseExpression(reader));
            nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("]"))
            {
                reader.SaveReadTokens();
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
                reader.SaveReadTokens();
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
                reader.SaveReadTokens();
            }

            try
            {
                reader.clear();
                return parseLiteral(reader);
            }
            catch (SyntaxError)
            {
                reader.SaveReadTokens();
            }

            try
            {
                reader.clear();
                return parseIdentifier(reader);
            }
            catch (SyntaxError)
            {
                reader.SaveReadTokens();
            }

            try
            {
                reader.clear();
                return parseAddress(reader);
            }
            catch (SyntaxError)
            {
                reader.SaveReadTokens();
                throw new SyntaxError("Can't parse operand");
            }
        }

        public static AstNode parseAddress(TokenReader reader)
        {
            var address = new AstNode("Address");
            var nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("&"))
            {
                reader.SaveReadTokens();
                throw new SyntaxError("Can't parse address");
            }

            address.setValue(nextToken);
            address.addChild(parseVariableReference(reader));
            reader.clear();
            return address;
        }

        public static AstNode parseReceiver(TokenReader reader)
        {
            var receiver = new AstNode("Receiver");
            var nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("this"))
            {
                reader.SaveReadTokens();
                throw new SyntaxError("Can't parse receiver");
            }

            receiver.setValue(nextToken);
            reader.clear();
            return receiver;
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

            reader.SaveReadTokens();
            throw new SyntaxError("Can't parse register");
        }

        public static AstNode parseDirective(TokenReader reader)
        {
            var directive = new AstNode("Directive");
            var nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("format"))
            {
                reader.SaveReadTokens();
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
                reader.SaveReadTokens();
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
//                node.addChild(parseAssignment(reader));
//                reader.clear();
//                node.SetNodeType(AstNode.NodeType.ExtensionStatement);
//                return node;
                return parseAssignment(reader);
            }
            catch (SyntaxError)
            {
                reader.SaveReadTokens();
            }

            try
            {
//                node.addChild(parseCall(reader));
//                reader.clear();
//                node.SetNodeType(AstNode.NodeType.ExtensionStatement);
//                return node;
                return parseCall(reader);
            }
            catch (SyntaxError)
            {
                reader.SaveReadTokens();
            }

            try
            {
//                node.addChild(parseIf(reader));
//                reader.clear();
//                node.SetNodeType(AstNode.NodeType.ExtensionStatement);
//                return node;
                return parseIf(reader);
            }
            catch (SyntaxError)
            {
                reader.SaveReadTokens();
            }

            try
            {
//                node.addChild(parseWhile(reader));
//                reader.clear();
//                node.SetNodeType(AstNode.NodeType.ExtensionStatement);
//                return node;
                return parseWhile(reader);
            }
            catch (SyntaxError)
            {
                reader.SaveReadTokens();
            }

            try
            {
//                node.addChild(parseFor(reader));
//                reader.clear();
//                node.SetNodeType(AstNode.NodeType.ExtensionStatement);
//                return node;
                return parseFor(reader);
            }
            catch (SyntaxError)
            {
                reader.SaveReadTokens();
            }

            try
            {
//                node.addChild(parseBreak(reader));
//                reader.clear();
//                node.SetNodeType(AstNode.NodeType.ExtensionStatement);
//                return node;
                return parseBreak(reader);
            }
            catch (SyntaxError)
            {
                reader.SaveReadTokens();
            }

            try
            {
//                node.addChild(parseSwap(reader));
//                reader.clear();
//                node.SetNodeType(AstNode.NodeType.ExtensionStatement);
//                return node;

                return parseSwap(reader);
            }
            catch (SyntaxError)
            {
                reader.SaveReadTokens();
            }

            try
            {
//                node.addChild(parseGoto(reader));
//                reader.clear();
//                node.SetNodeType(AstNode.NodeType.ExtensionStatement);
//                return node;
                return parseGoto(reader);
            }
            catch (SyntaxError)
            {
                reader.SaveReadTokens();
            }

            throw new SyntaxError("Can't parse extension statement");
        }

        public static AstNode parseIf(TokenReader reader)
        {
            var nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("if"))
            {
                reader.SaveReadTokens();
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
                reader.SaveReadTokens();
            }

            reader.clear();
            ifNode.SetNodeType(AstNode.NodeType.If);
            return ifNode;
        }

        private static AstNode parseIfBody(TokenReader reader)
        {
            var nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("do"))
            {
                reader.SaveReadTokens();
                throw new SyntaxError("Can't parse if body");
            }

            reader.clear();
            nextToken = reader.readNextToken();
            if (nextToken.GetValue().Equals("end"))
            {
                return new AstNode("RoutineBody");
            }

            reader.SaveReadTokens();
            var ifBody = parseRoutineBody(reader);
            nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("end"))
            {
                reader.SaveReadTokens();
                throw new SyntaxError("Can't parse if body");
            }

            reader.clear();
            ifBody.SetNodeType(AstNode.NodeType.IfBody);
            return ifBody;
        }

        public static AstNode parseCall(TokenReader reader)
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
                    throw new SyntaxError("Can't parse call");
                }

                call.addChild(primary);
            }

            if (!nextToken.GetValue().Equals("("))
            {
                reader.SaveReadTokens();
                throw new SyntaxError("Can't parse call");
            }

            reader.clear();
            call.addChild(parseCallParameters(reader));
            nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals(";"))
            {
                reader.SaveReadTokens();
                throw new SyntaxError("Can't parse call");
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
                    reader.SaveReadTokens();
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
                reader.SaveReadTokens();
                throw new SyntaxError("Can't parse for");
            }

            if (nextToken.GetValue().Equals("for"))
            {
                var forNode = new AstNode(nextToken);
                forStatement = forNode;
                forNode.addChild(parseIdentifier(reader));
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
                    reader.SaveReadTokens();
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
                reader.SaveReadTokens();
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
                    reader.SaveReadTokens();
                    nextToken = reader.readNextToken();
                    if (!nextToken.GetValue().Equals("end"))
                    {
                        reader.SaveReadTokens();
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
                reader.SaveReadTokens();
                throw new SyntaxError("Can't parse break");
            }

            breakNode.setValue(nextToken);
            reader.clear();
            breakNode.SetNodeType(AstNode.NodeType.Break);
            return breakNode;
        }

        public static AstNode parseSwap(TokenReader reader)
        {
            var swap = new AstNode(null);
            swap.addChild(parsePrimary(reader));
            var nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("<=>"))
            {
                reader.SaveReadTokens();
                throw new SyntaxError("Can't parse swap");
            }

            swap.setValue(nextToken);
            swap.addChild(parsePrimary(reader));
            nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals(";"))
            {
                reader.SaveReadTokens();
                throw new SyntaxError("Can't parse swap");
            }

            reader.clear();
            swap.SetNodeType(AstNode.NodeType.Swap);
            return swap;
        }

        public static AstNode parseGoto(TokenReader reader)
        {
            var goGo = new AstNode("goto");
            var nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("goto"))
            {
                reader.SaveReadTokens();
                throw new SyntaxError("Can't parse goto");
            }

            goGo.setValue(nextToken);
            goGo.addChild(parseIdentifier(reader));
            var delimiter = reader.readNextToken();
            if (!delimiter.GetValue().Equals(";"))
            {
                throw new SyntaxError("Can't parse goto");
            }

            reader.clear();
            goGo.SetNodeType(AstNode.NodeType.Goto);
            return goGo;
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
                reader.SaveReadTokens();
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