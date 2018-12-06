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
                node.SetNodeType(AstNode.NodeType.Unit);
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
                node.SetNodeType(AstNode.NodeType.Unit);
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
                node.SetNodeType(AstNode.NodeType.Unit);
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
                node.SetNodeType(AstNode.NodeType.Unit);
                return node;
            }
            catch (SyntaxError e)
            {
                Console.WriteLine(e);
            }

            throw new SyntaxError("Cannot parse Unit");
        }

        private static AstNode parseCode(TokenReader reader)
        {
            var nextToken = reader.readNextToken();
            var code = new AstNode("Code");
            if (!nextToken.GetValue().Equals("code"))
            {
                reader.SaveReadedTokens();
                throw new SyntaxError("Cannot parse Code");
            }

            while (true)
            {
                try
                {
                    code.addChild(parseVariable(reader));
                    continue;
                }
                catch (SyntaxError)
                {
                }

                try
                {
                    code.addChild(parseConstant(reader));
                    continue;
                }
                catch (SyntaxError)
                {
                }

                try
                {
                    code.addChild(parseStatement(reader));
                    continue;
                }
                catch (SyntaxError)
                {
                }

                nextToken = reader.readNextToken();

                if (nextToken.GetValue().Equals("end"))
                {
                    reader.clear();
                    return code;
                }

                reader.SaveReadedTokens();
                throw new SyntaxError("Cannot parse Code");
            }
        }

        private static AstNode parseData(TokenReader reader)
        {
            var nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("data"))
            {
                reader.SaveReadedTokens();
                throw new SyntaxError("Cannot parse Data");
            }

            var node = new AstNode("Data");
            node.addChild(parseIdentifier(reader));
            try
            {
                node.addChild(parseLiteral(reader));
            }
            catch (SyntaxError)
            {
                nextToken = reader.readNextToken();
                if (!nextToken.GetValue().Equals("end")) throw new SyntaxError("Cannot parse Data");
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

        private static AstNode parseRoutine(TokenReader reader)
        {
            var node = new AstNode("Routine");
            try
            {
                node.addChild(parseAttribute(reader));
            }
            catch (SyntaxError)
            {
            }

            var nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("routine"))
            {
                reader.SaveReadedTokens();
                throw new SyntaxError("Cannot parse Routine");
            }

            node.addChild(parseIdentifier(reader));
            try
            {
                node.addChild(parseParameter(reader));
            }
            catch (SyntaxError)
            {
            }

            nextToken = reader.readNextToken();
            if (nextToken.GetValue().Equals(":"))
            {
                node.addChild(parseResults(reader));
            }

            if (nextToken.GetValue().Equals(";"))
            {
                reader.clear();
                node.SetNodeType(AstNode.NodeType.Routine);
                return node;
            }

            if (!nextToken.GetValue().Equals("do")) throw new SyntaxError("Cannot parse Routine");
            node.addChild(parseRoutine(reader));
            nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("end"))
            {
                reader.SaveReadedTokens();
                throw new SyntaxError("Cannot parse Routine");
            }

            reader.clear();
            node.SetNodeType(AstNode.NodeType.Routine);
            return node;
        }

        private static AstNode parseModule(TokenReader reader)
        {
            var nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("module"))
            {
                reader.SaveReadedTokens();
                throw new SyntaxError("Cannot parse Module");
            }

            var node = new AstNode("Module");
            node.addChild(parseIdentifier(reader));
            while (true)
            {
                try
                {
                    node.addChild(parseDeclaration(reader));
                }
                catch (SyntaxError)
                {
                    reader.SaveReadedTokens();
                    break;
                }
            }

            if (!nextToken.GetValue().Equals("end"))
            {
                reader.SaveReadedTokens();
                throw new SyntaxError("Cannot parse Module");
            }

            reader.clear();
            node.SetNodeType(AstNode.NodeType.Module);
            return node;
        }

        public static AstNode parseVariable(TokenReader reader)
        {
            var node = new AstNode("Variable");
            node.addChild(parseType(reader));
            while (true)
            {
                node.addChild(parseVarDefinition(reader));
                var nextToken = reader.readNextToken();
                if (nextToken.GetValue().Equals(","))
                {
                }
                else if (nextToken.GetValue().Equals(";"))
                {
                    reader.clear();
                    node.SetNodeType(AstNode.NodeType.Variable);
                    return node;
                }
                else
                {
                    reader.SaveReadedTokens();
                    throw new SyntaxError("Cannot parse Variable");
                }
            }
        }

        public static AstNode parseConstant(TokenReader reader)
        {
            var nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("const"))
            {
                reader.SaveReadedTokens();
                throw new SyntaxError("Cannot parse Constant");
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
                    reader.SaveReadedTokens();
                    throw new SyntaxError("Cannot parse Constant");
                }
            }
        }


        public static AstNode parseIdentifier(TokenReader reader)
        {
            var nextToken = reader.readNextToken();
            if (nextToken.GetTokenType() == Token.TokenType.Identifier) return new AstNode(nextToken);
            reader.SaveReadedTokens();
            throw new SyntaxError("Cannot parse Identifier");
        }

        public static AstNode parseLiteral(TokenReader reader)
        {
            var nextToken = reader.readNextToken();
            if (nextToken.GetTokenType() != Token.TokenType.Number) throw new SyntaxError("Cannot parse Literal");
            reader.clear();
            return new AstNode(nextToken);
        }

        public static AstNode parseDeclaration(TokenReader reader)
        {
            var variable = new AstNode("Declaration");
            try
            {
                variable.addChild(parseVariable(reader));
                reader.clear();
                return variable;
            }
            catch (SyntaxError)
            {
                reader.SaveReadedTokens();
            }

            try
            {
                variable.addChild(parseConstant(reader));
                reader.clear();
                return variable;
            }
            catch (SyntaxError)
            {
                reader.SaveReadedTokens();
            }

            try
            {
                variable.addChild(parseRoutine(reader));
                reader.clear();
                return variable;
            }
            catch (SyntaxError)
            {
                reader.SaveReadedTokens();
            }

            throw new SyntaxError("Cannot parse Declaration");
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
                if (!nextToken.GetValue().Equals("]")) throw new SyntaxError("Cannot parse VarDefinition");
                reader.clear();
                node.SetNodeType(AstNode.NodeType.VarDefinition);
                return node;
            }

            {
                reader.SaveReadedTokens();
                node.SetNodeType(AstNode.NodeType.VarDefinition);
                return node;
            }
        }

        public static AstNode parseType(TokenReader reader)
        {
            var nextToken = reader.readNextToken();
            if (nextToken.GetTokenType() != Token.TokenType.Keyword)
            {
                reader.SaveReadedTokens();
                throw new SyntaxError("Cannot parse Type");
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

            reader.SaveReadedTokens();
            throw new SyntaxError("Cannot parse Type");
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
                    reader.SaveReadedTokens();
                    throw new SyntaxError("Cannot parse Expression");
                }
            }
            catch (SyntaxError)
            {
                reader.SaveReadedTokens();
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
                reader.SaveReadedTokens();
                throw new SyntaxError("Cannot parse ConstDefinition");
            }

            node.setValue(nextToken);
            node.addChild(parseExpression(reader));
            reader.clear();
            node.SetNodeType(AstNode.NodeType.ConstDefinition);
            return node;
        }

        private static AstNode parseStatement(TokenReader reader)
        {
            var node = new AstNode("Statement");
            try
            {
                while (true)
                {
                    node.addChild(parseLabel(reader));
                }
            }
            catch (SyntaxError)
            {
                reader.SaveReadedTokens();
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
                reader.SaveReadedTokens();
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
                reader.SaveReadedTokens();
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
                reader.SaveReadedTokens();
                throw new SyntaxError("Cannot parse Statement");
            }
        }

        public static AstNode parseLabel(TokenReader reader)
        {
            var nextToken = reader.readNextToken();
            var node = new AstNode("Label");
            if (!nextToken.GetValue().Equals("<"))
            {
                reader.SaveReadedTokens();
                throw new SyntaxError("Can't parse Label");
            }

            nextToken = reader.readNextToken();
            if (nextToken.GetTokenType() != Token.TokenType.Identifier)
            {
                reader.SaveReadedTokens();
                throw new SyntaxError("Can't parse Label");
            }

            node.setValue(nextToken);
            nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals(">"))
            {
                reader.SaveReadedTokens();
                throw new SyntaxError("Can't parse Label");
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
                    reader.SaveReadedTokens();
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
                    reader.SaveReadedTokens();
                    reader.clear();
                    node.SetNodeType(AstNode.NodeType.AssemblerStatement);
                    return node;
                }
            }

            var operators = new List<string>(new[]
                {":=", "+=", ">>=", "-=", "<<=", "|=", "&=", "^=", "<=", ">=", "?="});
            try
            {
                reader.SaveReadedTokens();
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
                            throw new SyntaxError("Cannot parse AssemblerStatement");
                        }

                        var goTo = new AstNode(newToken);
                        goTo.addChild(parseRegister(reader));
                        node.addChild(goTo);
                    }
                    else
                    {
                        reader.SaveReadedTokens();
                        throw new SyntaxError("Cannot parse AssemblerStatement");
                    }
                }
                catch (SyntaxError)
                {
                    reader.SaveReadedTokens();
                    throw new SyntaxError("Cannot parse AssemblerStatement");
                }
            }

            reader.clear();
            node.SetNodeType(AstNode.NodeType.AssemblerStatement);
            return node;
        }

        private static AstNode parseOperationOnRegisters(IReadOnlyList<string> operators, TokenReader reader)
        {
            AstNode leftOperand = null;
            try
            {
                leftOperand = parseOperator(reader);
            }
            catch (SyntaxError e)
            {
                reader.SaveReadedTokens();
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
                reader.SaveReadedTokens();
                throw new SyntaxError("Cannot parse OperationOnRegisters");
            }

            AstNode rightOperand = null;
            try
            {
                rightOperand = parseOperator(reader);
            }
            catch (SyntaxError)
            {
                reader.SaveReadedTokens();
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

            reader.SaveReadedTokens();
            throw new SyntaxError("Can't parse attribute");
        }

        public static AstNode parseParameters(TokenReader reader)
        {
            var node = new AstNode("Parameters");
            var nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("("))
            {
                reader.SaveReadedTokens();
                throw new SyntaxError("Cant parse parameters");
            }

            try
            {
                node.addChild(parseParameter(reader));
                nextToken = reader.readNextToken();
            }
            catch
            {
                reader.SaveReadedTokens();
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
                    reader.SaveReadedTokens();
                    break;
                }
            }

            if (!nextToken.GetValue().Equals(")"))
            {
                reader.SaveReadedTokens();
                throw new SyntaxError("Cant parse parameters");
            }

            reader.clear();
            node.SetNodeType(AstNode.NodeType.Parameters);
            return node;
        }

        public static AstNode parseResults(TokenReader reader)
        {
            var node = new AstNode("Results");
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
                    reader.SaveReadedTokens();
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
                reader.SaveReadedTokens();
            }

            try
            {
                return parseRegister(reader);
            }
            catch (SyntaxError)
            {
                reader.SaveReadedTokens();
                throw new SyntaxError("Cant parse parameter");
            }
        }

        private static AstNode parseRoutineBody(TokenReader reader)
        {
            var node = new AstNode("RoutineBody");
            while (true)
            {
                try
                {
                    node.addChild(parseVariable(reader));
                    continue;
                }
                catch (SyntaxError)
                {
                    reader.SaveReadedTokens();
                }

                try
                {
                    node.addChild(parseConstant(reader));
                    continue;
                }
                catch (SyntaxError)
                {
                    reader.SaveReadedTokens();
                }

                try
                {
                    node.addChild(parseStatement(reader));
                    continue;
                }
                catch (SyntaxError)
                {
                    reader.SaveReadedTokens();
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
                reader.SaveReadedTokens();
            }

            try
            {
                reader.clear();
                return parseArrayElement(reader);
            }
            catch (SyntaxError)
            {
                reader.SaveReadedTokens();
            }

            try
            {
                reader.clear();
                return parseDataElement(reader);
            }
            catch (SyntaxError)
            {
                reader.SaveReadedTokens();
            }

            try
            {
                reader.clear();
                return parseRegister(reader);
            }
            catch (SyntaxError)
            {
                reader.SaveReadedTokens();
            }

            try
            {
                reader.clear();
                return parseExplicitAddress(reader);
            }
            catch (SyntaxError)
            {
                reader.SaveReadedTokens();
            }

            try
            {
                reader.clear();
                return parseVariableReference(reader);
            }
            catch (SyntaxError)
            {
                reader.SaveReadedTokens();
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
                reader.SaveReadedTokens();
                throw new SyntaxError("Cannot parse Deference");
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
                reader.SaveReadedTokens();
            }

            try
            {
                reader.clear();
                dereference.addChild(parseRegister(reader));
                return dereference;
            }
            catch (SyntaxError)
            {
                reader.SaveReadedTokens();
            }

            throw new SyntaxError("Cant parse dereference");
        }

        public static AstNode parseArrayElement(TokenReader reader)
        {
            var arrayElement = parseVariableReference(reader);
            var nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("["))
            {
                reader.SaveReadedTokens();
                throw new SyntaxError("Cannot parse array element");
            }

            arrayElement.addChild(parseExpression(reader));
            nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("]"))
            {
                reader.SaveReadedTokens();
                throw new SyntaxError("Cannot parse array element");
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
                reader.SaveReadedTokens();
                throw new SyntaxError("Cannot parse data element");
            }

            dataElement.addChild(parseExpression(reader));
            nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("]"))
            {
                reader.SaveReadedTokens();
                throw new SyntaxError("Cannot parse data element");
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
                reader.SaveReadedTokens();
                throw new SyntaxError("Cannot parse explicit address");
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
                reader.SaveReadedTokens();
            }

            try
            {
                reader.clear();
                return parseLiteral(reader);
            }
            catch (SyntaxError)
            {
                reader.SaveReadedTokens();
            }

            try
            {
                reader.clear();
                return parseIdentifier(reader);
            }
            catch (SyntaxError)
            {
                reader.SaveReadedTokens();
            }

            try
            {
                reader.clear();
                return parseAddress(reader);
            }
            catch (SyntaxError)
            {
                reader.SaveReadedTokens();
                throw new SyntaxError("Cannot parse operand");
            }
        }

        public static AstNode parseAddress(TokenReader reader)
        {
            var address = new AstNode("Address");
            var nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("&"))
            {
                reader.SaveReadedTokens();
                throw new SyntaxError("Cannot parse address");
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
                reader.SaveReadedTokens();
                throw new SyntaxError("Cannot parse receiver");
            }

            receiver.setValue(nextToken);
            reader.clear();
            return receiver;
        }

        private static AstNode parseOperator(TokenReader reader)
        {
            var nextToken = reader.readNextToken();
            const Token.TokenType operatorType = Token.TokenType.Operator;
            if (!nextToken.GetTokenType().Equals(operatorType)) throw new SyntaxError("Cannot parse operator");
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

            reader.SaveReadedTokens();
            throw new SyntaxError("Can't parse register");
        }

        public static AstNode parseDirective(TokenReader reader)
        {
            var directive = new AstNode("Directive");
            var nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("format"))
            {
                reader.SaveReadedTokens();
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
                reader.SaveReadedTokens();
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
                node.addChild(parseAssignment(reader));
            }
            catch (SyntaxError)
            {
                reader.SaveReadedTokens();
            }

            try
            {
                node.addChild(parseCall(reader));
                reader.clear();
                node.SetNodeType(AstNode.NodeType.ExtensionStatement);
                return node;
            }
            catch (SyntaxError)
            {
                reader.SaveReadedTokens();
            }

            try
            {
                node.addChild(parseIf(reader));
                reader.clear();
                node.SetNodeType(AstNode.NodeType.ExtensionStatement);
                return node;
            }
            catch (SyntaxError)
            {
                reader.SaveReadedTokens();
            }

            try
            {
                node.addChild(parseWhile(reader));
                reader.clear();
                node.SetNodeType(AstNode.NodeType.ExtensionStatement);
                return node;
            }
            catch (SyntaxError)
            {
                reader.SaveReadedTokens();
            }

            try
            {
                node.addChild(parseFor(reader));
                reader.clear();
                node.SetNodeType(AstNode.NodeType.ExtensionStatement);
                return node;
            }
            catch (SyntaxError)
            {
                reader.SaveReadedTokens();
            }

            try
            {
                node.addChild(parseBreak(reader));
                reader.clear();
                node.SetNodeType(AstNode.NodeType.ExtensionStatement);
                return node;
            }
            catch (SyntaxError)
            {
                reader.SaveReadedTokens();
            }

            try
            {
                node.addChild(parseSwap(reader));
                reader.clear();
                node.SetNodeType(AstNode.NodeType.ExtensionStatement);
                return node;
            }
            catch (SyntaxError)
            {
                reader.SaveReadedTokens();
            }

            try
            {
                node.addChild(parseGoto(reader));
                reader.clear();
                node.SetNodeType(AstNode.NodeType.ExtensionStatement);
                return node;
            }
            catch (SyntaxError)
            {
                reader.SaveReadedTokens();
            }

            throw new SyntaxError("Cannot parse extension statement");
        }

        public static AstNode parseIf(TokenReader reader)
        {
            var nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("if"))
            {
                reader.SaveReadedTokens();
                throw new SyntaxError("Cannot parse if");
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
                reader.SaveReadedTokens();
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
                reader.SaveReadedTokens();
                throw new SyntaxError("Cannot parse if body");
            }

            reader.clear();
            nextToken = reader.readNextToken();
            if (nextToken.GetValue().Equals("end"))
            {
                return new AstNode("RoutineBody");
            }

            reader.SaveReadedTokens();

            AstNode ifBody = parseRoutineBody(reader);
            nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("end"))
            {
                reader.SaveReadedTokens();
                throw new SyntaxError("Cannot parse if body");
            }

            reader.clear();
            ifBody.SetNodeType(AstNode.NodeType.IfBody);
            return ifBody;
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
                    throw new SyntaxError("Cannot parse call");
                }

                call.addChild(primary);
            }

            if (!nextToken.GetValue().Equals("("))
            {
                reader.SaveReadedTokens();
                throw new SyntaxError("Cannot parse call");
            }

            reader.clear();
            call.addChild(parseCallParameters(reader));
            nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals(";"))
            {
                reader.SaveReadedTokens();
                throw new SyntaxError("Cannot parse call");
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
                Token nextToken = reader.readNextToken();
                if (!nextToken.GetValue().Equals(")"))
                {
                    throw new SyntaxError("Cannot parse call parameters");
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
                    reader.SaveReadedTokens();
                    throw new SyntaxError("Cannot parse call parameters");
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
            if (nextToken.GetValue().Equals("for"))
            {
                var forNode = new AstNode(nextToken);
                forStatement.addChild(forNode);
                forNode.addChild(parseIdentifier(reader));
                nextToken = reader.readNextToken();
            }

            if (nextToken.GetValue().Equals("from"))
            {
                var fromStatement = new AstNode(nextToken);
                forStatement.addChild(fromStatement);
                fromStatement.addChild(parseExpression(reader));
                nextToken = reader.readNextToken();
            }

            if (nextToken.GetValue().Equals("to"))
            {
                var toStatement = new AstNode(nextToken);
                forStatement.addChild(toStatement);
                toStatement.addChild(parseExpression(reader));
                nextToken = reader.readNextToken();
            }

            if (nextToken.GetValue().Equals("step"))
            {
                var stepNode = new AstNode(nextToken);
                forStatement.addChild(stepNode);
                stepNode.addChild(parseExpression(reader));
            }

            forStatement.addChild(parseLoopBody(reader));
            reader.clear();
            forStatement.SetNodeType(AstNode.NodeType.For);
            return forStatement;
        }

        public static AstNode parseWhile(TokenReader reader)
        {
            var whileStatement = new AstNode("while");
            var nextToken = reader.readNextToken();
            if (nextToken.GetValue().Equals("while"))
            {
                try
                {
                    whileStatement.addChild(parseExpression(reader));
                }
                catch (SyntaxError)
                {
                    reader.SaveReadedTokens();
                    whileStatement.addChild(parseLoopBody(reader));
                    reader.clear();
                    whileStatement.SetNodeType(AstNode.NodeType.While);
                    return whileStatement;
                }
            }

            whileStatement.addChild(parseLoopBody(reader));
            reader.clear();
            whileStatement.SetNodeType(AstNode.NodeType.While);
            return whileStatement;
        }

        public static AstNode parseLoopBody(TokenReader reader)
        {
            var loopBody = new AstNode("loopBody");
            var nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("loop"))
            {
                reader.SaveReadedTokens();
                throw new SyntaxError("Cannot parse loop body");
            }

            while (true)
            {
                try
                {
                    loopBody.addChild(parseStatement(reader));
                }
                catch (SyntaxError)
                {
                    reader.SaveReadedTokens();
                    nextToken = reader.readNextToken();
                    if (!nextToken.GetValue().Equals("end"))
                    {
                        reader.SaveReadedTokens();
                        throw new SyntaxError("Can't parse loop");
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
                reader.SaveReadedTokens();
                throw new SyntaxError("Cannot parse break");
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
                reader.SaveReadedTokens();
                throw new SyntaxError("Cannot parse swap");
            }

            swap.setValue(nextToken);
            swap.addChild(parsePrimary(reader));
            nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals(";"))
            {
                reader.SaveReadedTokens();
                throw new SyntaxError("Cannot parse swap");
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
                reader.SaveReadedTokens();
                throw new SyntaxError("Cannot parse goto");
            }

            goTo.setValue(nextToken);
            goTo.addChild(parseIdentifier(reader));
            var delimiter = reader.readNextToken();
            if (!delimiter.GetValue().Equals(";"))
            {
                throw new SyntaxError("Cannot parse goto");
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
                reader.SaveReadedTokens();
                throw new SyntaxError("Can't parse assignment");
            }

            assignment.addChild(parseExpression(reader));
            nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals(";"))
            {
                assignment.setValue(":=");
            }

            reader.clear();
            assignment.SetNodeType(AstNode.NodeType.Assignment);
            return assignment;
        }
    }
}