using System.Collections.Generic;
using Erasystemlevel.Exception;
using Erasystemlevel.Tokenizer;
namespace Erasystemlevel.Parser
{
    public class Parser
    {
        public static AstNode ParseUnit(TokenReader reader){
            try
            {
                AstNode node = Parser.parseCode(reader);
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
                AstNode node = Parser.parseData(reader);
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
                AstNode node = Parser.parseModule(reader);
                reader.clear();
                return node;
            }
            catch (SyntaxError e)
            {
                Console.WriteLine(e);
            }
            try
            {
                AstNode node = Parser.parseRoutine(reader);
                reader.clear();
                        return node;
            }
            catch (SyntaxError e)
            {
                Console.WriteLine(e);
            }
            throw new SyntaxError("");

        }

        public static AstNode parseCode(TokenReader reader){
            Token nextToken = reader.readNextToken();
            AstNode code = new AstNode("Code");
            if (!(nextToken.GetValue().Equals("code")))
            {
                reader.saveReadedTokens();
                throw new SyntaxError("");
            }
            nextToken = null;
            while (true)
            {

                try
                {
                    code.addChild(Parser.parseVariable(reader));
                    continue;
                }
                catch (SyntaxError e)
                {

                }
                try
                {
                    code.addChild(Parser.parseConstant(reader));
                    continue;
                }
                catch (SyntaxError e)
                {

                }
                try
                {
                    code.addChild(Parser.parseStatement(reader));
                    continue;
                }
                catch (SyntaxError e)
                {

                }
                nextToken = reader.readNextToken();

                if (nextToken.GetValue().Equals("end")){
                    reader.clear();
                            return code;
                }else{
                    reader.saveReadedTokens();
                    throw new SyntaxError("");
                }

            }

        }
        public static AstNode parseData(TokenReader reader){
            Token nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("data"))
            {
                reader.saveReadedTokens();
                throw new SyntaxError("");
            }
            AstNode node = new AstNode("Data");
            node.addChild(Parser.parseIdentifier(reader));
            try
            {
                node.addChild(Parser.parseLiteral(reader));
            }
            catch (SyntaxError e)
            {
                nextToken = reader.readNextToken();
                if (nextToken.GetValue().Equals("end"))
                {
                    reader.clear();
                    return node;
                }
                else
                {
                    throw new SyntaxError("");

                }

            }
            while (true)
            {

                nextToken = reader.readNextToken();
                if (nextToken.GetValue().Equals("end"))
                {
                    reader.clear();
                            return node;
                }
                else if (nextToken.GetValue().Equals(","))
                {
                    node.addChild(Parser.parseLiteral(reader));
                }
            }
        }

        public static AstNode parseRoutine(TokenReader reader){
            AstNode node = new AstNode("Routine");
            try
            {
                node.addChild(Parser.parseAttribute(reader));
            }catch(SyntaxError e){}
            Token nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("routine"))
            {
                reader.saveReadedTokens();
                throw new SyntaxError("");
            }
            node.addChild(Parser.parseIdentifier(reader));
            try
            {
                node.addChild(Parser.parseParameter(reader));
            }catch(SyntaxError e){}
            nextToken = reader.readNextToken();
            if (nextToken.GetValue().Equals(":"))
            {
                node.addChild(Parser.parseResults(reader));
            }
            if (nextToken.GetValue().Equals(";"))
            {
                reader.clear();
                        return node;
            }
            else if (nextToken.GetValue().Equals("do"))
            {
                node.addChild(Parser.parseRoutine(reader));
                nextToken = reader.readNextToken();
                if (!nextToken.GetValue().Equals("end"))
                {
                    reader.saveReadedTokens();
                    throw new SyntaxError("");
                }
                reader.clear();
                return node;
            }
            throw new SyntaxError("");
        }

        public static AstNode parseModule(TokenReader reader){
            Token nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("module"))
            {
                reader.saveReadedTokens();
                throw new SyntaxError("");
            }
            AstNode node = new AstNode("Module");
            node.addChild(Parser.parseIdentifier(reader));
            while (true)
            {
                try
                {
                    node.addChild(Parser.parseDeclaration(reader));
                }
                catch (SyntaxError e)
                {
                    reader.saveReadedTokens();
                    break;
                }
            }
            if (!nextToken.GetValue().Equals("end"))
            {
                reader.saveReadedTokens();
                throw new SyntaxError("");
            }
            reader.clear();
            return node;
        }

        public static AstNode parseVariable(TokenReader reader){
            AstNode node = new AstNode("Variable");
            node.addChild(Parser.parseType(reader));
            while (true)
            {
                node.addChild(Parser.parseVarDefinition(reader));
                Token nextToken = reader.readNextToken();
                if (nextToken.GetValue().Equals(","))
                {
                    continue;
                }
                else if (nextToken.GetValue().Equals(";"))
                {
                    reader.clear();
                            return node;
                }
                else
                {
                    reader.saveReadedTokens();
                    throw new SyntaxError("");
                }
            }
        }

        public static AstNode parseConstant(TokenReader reader){
            Token nextToken = reader.readNextToken();
            if (!(nextToken.GetValue().Equals("const")))
            {
                reader.saveReadedTokens();
                throw new SyntaxError("");
            }
            AstNode node = new AstNode("Constant");
            while (true)
            {
                node.addChild(Parser.parseConstDefinition(reader));
                nextToken = reader.readNextToken();
                if (nextToken.GetValue().Equals(","))
                {
                    node.addChild(Parser.parseConstDefinition(reader));
                }
                else if (nextToken.GetValue().Equals(";"))
                {
                    reader.clear();
                            return node;
                }
                else
                {
                    reader.saveReadedTokens();
                    throw new SyntaxError("");
                }
            }
        }


        public static AstNode parseIdentifier(TokenReader reader)
        {
            Token nextToken = reader.readNextToken();
            if (!(nextToken.GetTokenType() == Token.TokenType.Identifier))
            {
                reader.saveReadedTokens();
                throw new SyntaxError("");
            }
            //reader.clear();
            return new AstNode(nextToken);
        }

        public static AstNode parseLiteral(TokenReader reader) //Literal:  Numeric
        {
            Token nextToken = reader.readNextToken();
            if(nextToken.GetTokenType()==Token.TokenType.Number){
                reader.clear();
                return new AstNode(nextToken);
            }
            throw new SyntaxError("");
        }

        public static AstNode parseDeclaration(TokenReader reader){
            AstNode variable = new AstNode("Declaration");
            try
            {
                variable.addChild(Parser.parseVariable(reader));
                reader.clear();
                        return variable;
            }
            catch (SyntaxError e)
            {
                reader.saveReadedTokens();
            }
            try
            {
                variable.addChild(Parser.parseConstant(reader));
                reader.clear();
                        return variable;
            }
            catch (SyntaxError e)
            {
                reader.saveReadedTokens();
            }
            try
            {
                variable.addChild(Parser.parseRoutine(reader));
                reader.clear();
                        return variable;
            }
            catch (SyntaxError e)
            {
                reader.saveReadedTokens();
            }
            throw new SyntaxError("");
        }

        public static AstNode parseVarDefinition(TokenReader reader){ 
            AstNode node = new AstNode("VarDefinition");
            node.addChild(Parser.parseIdentifier(reader));
            Token nextToken = reader.readNextToken();
            if(nextToken.GetValue().Equals(":=")){
                node.addChild(Parser.parseExpression(reader));
                reader.clear();
                        return node;
            }
            else if(nextToken.GetValue().Equals("[")){
                node.addChild(Parser.parseExpression(reader));
                nextToken = reader.readNextToken();
                if (nextToken.GetValue().Equals("]"))
                {
                    reader.clear();
                            return node;
                }else{
                    throw new SyntaxError("");
                }

            }else{
                reader.saveReadedTokens();
                return node;
            }
        }

        public static AstNode parseType(TokenReader reader){
            Token nextToken = reader.readNextToken();
            if (!(nextToken.GetTokenType() == Token.TokenType.Keyword))
            {
                reader.saveReadedTokens();
                throw new SyntaxError("");
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
            reader.saveReadedTokens();
            throw new SyntaxError("");
        }

        public static AstNode parseExpression(TokenReader reader){
            AstNode firstOPerand = Parser.parseOperand(reader);

            reader.clear();
            try
            {
                AstNode node = Parser.parseOperator(reader);
                try
                {
                    node.addChild(firstOPerand);
                    node.addChild(Parser.parseOperand(reader));
                    reader.clear();
                    return node;
                }
                catch (SyntaxError)
                {
                    reader.saveReadedTokens();
                    throw new SyntaxError("");
                }
            }catch (SyntaxError){
                reader.saveReadedTokens();
                return firstOPerand;
            }
        }

        public static AstNode parseConstDefinition(TokenReader reader){
            AstNode node = new AstNode(null);
            node.addChild(Parser.parseIdentifier(reader));
            Token nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("="))
            {
                reader.saveReadedTokens();
                throw new SyntaxError("");
            }
            node.setValue(nextToken);
            node.addChild(Parser.parseExpression(reader));
            reader.clear();
                    return node;
        }

        public static AstNode parseStatement(TokenReader reader){
            AstNode node = new AstNode("Statement");
            try
            {
                while (true)
                {
                    node.addChild(Parser.parseLabel(reader));
                }
            }
            catch (SyntaxError e)
            {
                reader.saveReadedTokens();
            }
            try
           {
                node.addChild(Parser.parseAssemblerStatement(reader));
                reader.clear();
                        return node;
            }
            catch (SyntaxError e)
            {
                reader.saveReadedTokens();
            }
            try
            {
                node.addChild(Parser.parseExtensionStatement(reader));
                reader.clear();
                        return node;
            }
            catch (SyntaxError e)
            {
                reader.saveReadedTokens();
            }
            try
            {
                node.addChild(Parser.parseDirective(reader));
                reader.clear();
                        return node;

            }
            catch (SyntaxError e)
            {
                reader.saveReadedTokens();
                throw new SyntaxError("Cant parse statement");
            }
            throw new SyntaxError("Cant parse statement");
        }

        public static AstNode parseLabel(TokenReader reader){
            Token nextToken = reader.readNextToken();
            AstNode node = new AstNode("Label");
            if (!nextToken.GetValue().Equals("<"))
            {
                reader.saveReadedTokens();
                throw new SyntaxError("Can't parse error");
            }
            nextToken = reader.readNextToken();
            if (!(nextToken.GetTokenType() == Token.TokenType.Identifier))
            {
                reader.saveReadedTokens();
                throw new SyntaxError("Can't parse error");

            }
            node.addChild(new AstNode(nextToken));
            nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals(">"))
            {
                reader.saveReadedTokens();
                throw new SyntaxError("Can't parse error");
            }
            reader.clear();
            return node;
        }

        public static AstNode parseAssemblerStatement(TokenReader reader){
            AstNode node = new AstNode("AssemblerStatement");
            Token nextToken = reader.readNextToken();
            if(nextToken.GetValue().Equals("skip")){
                node.addChild(new AstNode(nextToken));
                try{
                    node.addChild(Parser.parseExpression(reader));
                    reader.clear();
                            return node;
                }catch(SyntaxError e){
                    reader.clear();
                            return node;
                }
            }else if (nextToken.GetValue().Equals("stop"))
            {
                node.addChild(new AstNode(nextToken));
                try
                {
                    node.addChild(Parser.parseExpression(reader));
                    reader.clear();
                            return node;
                }
                catch (SyntaxError e)
                {
                    reader.clear();
                            return node;
                }
            }
            List<string> operators =new List<string>(new string[]{ ":=", "+=", ">>=", "-=", "<<=", "|=", "&=", "^="
            , "<=", ">=", "?="});
            try
            {
                node.addChild(Parser.parseOperationOnRegisters(operators, reader));
            }catch(SyntaxError e){
                try
                {
                    if (nextToken.GetValue().Equals("if"))
                    {
                        node.addChild(new AstNode(nextToken));
                        node.addChild(Parser.parseRegister(reader));
                        Token newToken = reader.readNextToken();
                        if(!nextToken.GetValue().Equals("goto")){
                            throw new SyntaxError("");
                        }
                        node.addChild(new AstNode(newToken));
                        node.addChild(Parser.parseRegister(reader));
                    }else{
                        reader.saveReadedTokens();
                        throw new SyntaxError("");
                    }
                }catch(SyntaxError){
                    reader.saveReadedTokens();
                    throw new SyntaxError("");
                }
            }
            reader.clear();
            return node;
        }
        public static AstNode parseOperationOnRegisters(List<string> operators, TokenReader reader){
            AstNode leftOperand = null;
            try
            {
              leftOperand = Parser.parseOperator(reader);
            }catch(SyntaxError e){}
            if(leftOperand!=null && ((Token)leftOperand.getValue()).GetValue().Equals("*")){
                leftOperand.addChild(Parser.parseRegister(reader));
            }else{
                leftOperand = Parser.parseRegister(reader);
            }
            Token operation =reader.readNextToken();
            bool exists = false;
            for (int i = 0; i < operators.Count; i++){
                if(operators[i].Equals(operation.GetValue())){
                    exists = true;
                }
            }
            if(exists==false){
                reader.saveReadedTokens();
                throw new SyntaxError("");
            }
            AstNode rightOperand = null;

            if (rightOperand != null && ((Token)rightOperand.getValue()).GetValue().Equals("*"))
            {
                rightOperand.addChild(Parser.parseRegister(reader));
            }
            else
            {
                rightOperand = Parser.parseRegister(reader);
            }
            AstNode node = new AstNode(operation);
            node.addChild(leftOperand);
            node.addChild(rightOperand);
            reader.clear();
            return node;
        
        }

        public static AstNode parseAttribute(TokenReader reader){
            Token nextToken = reader.readNextToken();
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
            reader.saveReadedTokens();
            throw new SyntaxError("Can't parse attribute");

        }
        public static AstNode parseParameters(TokenReader reader){
            AstNode node = new AstNode("Parameters");
            Token nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("("))
            {
                reader.saveReadedTokens();
                throw new SyntaxError("Cant parse parameters");
            }
            while (true)
            {
                node.addChild(Parser.parseParameter(reader));
                nextToken = reader.readNextToken();
                if (nextToken.GetValue().Equals(","))
                {
                    node.addChild(Parser.parseParameter(reader));
                }
                else
                {
                    break;
                }
            }
            if (!nextToken.GetValue().Equals(")"))
            {
                reader.saveReadedTokens();
                throw new SyntaxError("Cant parse parameters");
            }
            reader.clear();
                    return node;
        }

        public static AstNode parseResults(TokenReader reader){
            AstNode node = new AstNode("Results");
            while (true)
            {
                node.addChild(Parser.parseRegister(reader));
                Token nextToken = reader.readNextToken();
                if (nextToken.GetValue().Equals(","))
                {
                    node.addChild(Parser.parseRegister(reader));
                    continue;
                }
                else
                {
                    reader.clear();
                    return node;
                }
            }
        }

        public static AstNode parseParameter(TokenReader reader){
            AstNode node = new AstNode("Parameter");
            try
            {
                node.addChild(Parser.parseType(reader));
                node.addChild(Parser.parseIdentifier(reader));
                reader.clear();
                return node;
            }
            catch (SyntaxError e)
            {
                reader.saveReadedTokens();
                node.cleanChild();
            }
            try
            {
                node.addChild(Parser.parseRegister(reader));
                reader.clear();
                return node;
            }
            catch (SyntaxError e)
            {
                reader.saveReadedTokens();
                throw new SyntaxError("Cant parse parameter");
            }
        }

        public static AstNode parseRoutineBody(TokenReader reader){
            AstNode node = new AstNode("RoutineBody");
            while (true)
            {
                try
                {
                    node.addChild(Parser.parseVariable(reader));
                    continue;
                }
                    catch (SyntaxError e) { reader.saveReadedTokens(); }
                try
                {
                    node.addChild(Parser.parseConstant(reader));
                    continue;
                }
                catch (SyntaxError e) { reader.saveReadedTokens(); }
                try
                {
                    node.addChild(Parser.parseStatement(reader));
                    continue;
                }
                catch (SyntaxError e) { reader.saveReadedTokens(); }
                break;
            }
            reader.clear();
            return node;
        }

        public static AstNode parsePrimary(TokenReader reader){

            try
            {
                //reader.clear();
                return Parser.parseDereference(reader);
            }
            catch (SyntaxError)
            {
                reader.saveReadedTokens();
            }
            try
            {
                //reader.clear();
                return Parser.parseArrayElement(reader);
            }
            catch (SyntaxError)
            {
                reader.saveReadedTokens();
            }
            try
            {
                //reader.clear();
                return Parser.parseDataElement(reader);
            }
            catch (SyntaxError)
            {
                reader.saveReadedTokens();
            }
            try
            {
                //reader.clear();
                return Parser.parseRegister(reader);
            }
            catch (SyntaxError)
            {
                reader.saveReadedTokens();
            }
            try
            {
                //reader.clear();
                return Parser.parseExplicitAddress(reader);
            }
            catch (SyntaxError)
            {
                reader.saveReadedTokens();
            }
            try
            {
                // reader.clear();
                return Parser.parseVariableReference(reader);
            }
            catch (SyntaxError) { reader.saveReadedTokens(); }
            throw new SyntaxError("Cant parse primary");
        }

        public static AstNode parseVariableReference(TokenReader reader){
            return Parser.parseIdentifier(reader);
           
        }
        public static AstNode parseDereference(TokenReader reader){
            AstNode dereference = new AstNode(null);
            Token nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("*"))
            {
                reader.saveReadedTokens();
                throw new SyntaxError("");
            }
            dereference.setValue(nextToken);
            try
            {
                reader.clear();
                dereference.addChild(Parser.parseVariableReference(reader));
                return dereference;
            }
            catch (SyntaxError)
            {
                reader.saveReadedTokens();
            }
            try
            {
                reader.clear();
                dereference.addChild(Parser.parseRegister(reader));
                return dereference;
            }
            catch (SyntaxError)
            {
                reader.saveReadedTokens();
            }
            throw new SyntaxError("Cant parse dereference");
        }
       public static AstNode parseArrayElement(TokenReader reader)
        {
            AstNode arrayElement = Parser.parseVariableReference(reader);
            Token nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("["))
            {
                reader.saveReadedTokens();
                throw new SyntaxError("");
            }
            arrayElement.addChild(Parser.parseExpression(reader));
            nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("]"))
            {
                reader.saveReadedTokens();
                throw new SyntaxError("");
            }
            reader.clear();
            return arrayElement;
        }

        public static AstNode parseDataElement(TokenReader reader)
        {
            AstNode dataElement = Parser.parseIdentifier(reader);
            Token nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("["))
            {
                reader.saveReadedTokens();
                throw new SyntaxError("");
            }
            dataElement.addChild(Parser.parseExpression(reader));
            nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("]"))
            {
                reader.saveReadedTokens();
                throw new SyntaxError("");
            }
            reader.clear();
            return dataElement;
        }

        public static AstNode parseExplicitAddress(TokenReader reader){
            Token nextToken = reader.readNextToken();
            AstNode explicitAddress = new AstNode(nextToken);
            if (!nextToken.GetValue().Equals("*"))
            {
                reader.saveReadedTokens();
                throw new SyntaxError("");
            }
            explicitAddress.addChild(Parser.parseLiteral(reader));
            reader.clear();
            return explicitAddress;
        }

        public static AstNode parseOperand(TokenReader reader){
            try
            {
                reader.clear();
                return Parser.parseReceiver(reader);
            }
            catch (SyntaxError)
            {
                reader.saveReadedTokens();
            }
            try
            {
                reader.clear();
                return Parser.parseLiteral(reader);
            }
            catch (SyntaxError)
            {
                reader.saveReadedTokens();
            }
            try
            {
                reader.clear();
                return Parser.parseAddress(reader);
            }
            catch (SyntaxError)
            {
                reader.saveReadedTokens();
                throw new SyntaxError("");
            }

        }

        public static AstNode parseAddress(TokenReader reader)
        {
            AstNode address = new AstNode("Address");
            Token nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("&"))
            {
                reader.saveReadedTokens();
                throw new SyntaxError("");
            }
            address.setValue(nextToken);
            address.addChild(Parser.parseVariableReference(reader));
            reader.clear();
            return address;
        }

        public static AstNode parseReceiver(TokenReader reader)
        {
            AstNode receiver = new AstNode("Receiver");
            Token nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("this"))
            {
                reader.saveReadedTokens();
                throw new SyntaxError("");
            }
            receiver.setValue(nextToken);
            reader.clear();
            return receiver;
        }

        public static AstNode parseOperator(TokenReader reader)
        {
            Token nextToken = reader.readNextToken();
            Token.TokenType operatorType = Token.TokenType.Operator;
            if (nextToken.GetTokenType().Equals(operatorType))
            {
                reader.clear();
        ;
                return new AstNode(nextToken);
            }
            throw new SyntaxError("");
        }

        // Useless node 
        //public static AstNode parsePrimitiveOperator()
        //{
        //    AstNode primitiveOperator = new AstNode(null);
        //    Token nextToken = reader.readNextToken();
        //    if (nextToken.GetValue().Equals("=") || nextToken.GetValue().Equals("/=")
        //       || nextToken.GetValue().Equals("<") || nextToken.GetValue().Equals(">"))
        //    {
        //        primitiveOperator.setValue(nextToken.GetValue());
        //    }
        //    else
        //    {
        //        throw new SyntaxError("Can't parse register");
        //    }
        //    return primitiveOperator;
        //}



        public static AstNode parseRegister(TokenReader reader){
            Token nextToken = reader.readNextToken();
            if (nextToken.GetTokenType().Equals(Token.TokenType.Register))
            {
                reader.clear();
                return new AstNode(nextToken);
            }
            else
            {
                reader.saveReadedTokens();
                throw new SyntaxError("Can't parse register");
            }



        }

       public static AstNode parseDirective(TokenReader reader){
            AstNode directive = new AstNode("Directive");
            Token nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("format"))
            {
                reader.saveReadedTokens();
                throw new SyntaxError("Can't parse directive");
            }
            nextToken = reader.readNextToken();
            if (nextToken.GetValue().Equals("8") || nextToken.GetValue().Equals("16")
                || nextToken.GetValue().Equals("32"))
            {
                directive.addChild(new AstNode(nextToken));
            }
            else
            {
                reader.saveReadedTokens();
                throw new SyntaxError("Can't parse directive");
            }
            reader.clear();
            return directive;
        }

        public static AstNode parseExtensionStatement(TokenReader reader){
            AstNode node = new AstNode("ExtensionStatement");
            try
            {
                node.addChild(Parser.parseAssigment(reader));
            }
            catch (SyntaxError e) { reader.saveReadedTokens(); }
            try
            {
                node.addChild(Parser.parseCall(reader));
                reader.clear();
                        return node;
            }
            catch (SyntaxError e) { reader.saveReadedTokens(); }
            try
            {
                node.addChild(Parser.parseIf(reader));
                reader.clear();
                        return node;

            }
            catch (SyntaxError e) { reader.saveReadedTokens(); }
            try
            {
                node.addChild(Parser.parseWhile(reader));
                reader.clear();
                        return node;

            }
            catch (SyntaxError e) { reader.saveReadedTokens(); }
            try
            {
                node.addChild(Parser.parseFor(reader));
                reader.clear();
                return node;

            }
            catch (SyntaxError e) { reader.saveReadedTokens(); }
            try
            {
                node.addChild(Parser.parseBreak(reader));
                reader.clear();
                return node;

            }
            catch (SyntaxError e) { reader.saveReadedTokens(); }
            try
            {
                node.addChild(Parser.parseSwap(reader));
                reader.clear();
                return node;

            }
            catch (SyntaxError e) { reader.saveReadedTokens(); }
            try
            {
                node.addChild(Parser.parseGoto(reader));
                reader.clear();
                return node;

            }
            catch (SyntaxError e) { reader.saveReadedTokens(); }
            throw new SyntaxError("");

        }

        public static AstNode parseIf(TokenReader reader) // if Expression do RoutineBody end
        {
            Token nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("if"))
            {
                reader.saveReadedTokens();
                throw new SyntaxError("");
            }
            AstNode ifNode = new AstNode("if");
            ifNode.addChild(Parser.parseExpression(reader));
            nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("do"))
            {
                reader.saveReadedTokens();
                throw new SyntaxError("");
            }
            ifNode.addChild(Parser.parseRoutineBody(reader));
            if (!nextToken.GetValue().Equals("end"))
            {
                reader.saveReadedTokens();
                throw new SyntaxError("");
            }
            reader.clear();
            return ifNode;
        }

        public static AstNode parseCall(TokenReader reader) //Identifier(); // Identifier ( [Identifier {, Identifier}]
        {
            AstNode call = new AstNode("Call");
            call.addChild(Parser.parseIdentifier(reader));
            call.addChild(Parser.parseCallParameters(reader));
            reader.clear(); 
            return call;
        }

        public static AstNode parseCallParameters(TokenReader reader){
            AstNode parameters = new AstNode("parameters");
            Token nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("("))
            {
                reader.saveReadedTokens();
                throw new SyntaxError("");
            }
            while (true)
            {
                    parameters.addChild(Parser.parseIdentifier(reader));
                    nextToken = reader.readNextToken();
                    if (nextToken.GetValue().Equals(","))
                    {
                        continue;
                    }
                    if (!nextToken.GetValue().Equals(")"))
                    {
                        reader.saveReadedTokens();
                        throw new SyntaxError("");
                    }
                reader.clear();
                        return parameters;
                
            }
        }

        public static AstNode parseFor(TokenReader reader) // default: from zero to inf step 1
        {
            AstNode forStatement = new AstNode("For");
            Token nextToken = reader.readNextToken();
            if (nextToken.GetValue().Equals("for"))
            {
                AstNode forNode = new AstNode(nextToken);
                forStatement.addChild(forNode);
                forNode.addChild(Parser.parseIdentifier(reader));
                nextToken = reader.readNextToken();
            }
            if (nextToken.GetValue().Equals("from"))
            {
                AstNode fromStatement = new AstNode(nextToken);
                forStatement.addChild(fromStatement);
                fromStatement.addChild(Parser.parseExpression(reader));
                nextToken = reader.readNextToken();
            }
            if (nextToken.GetValue().Equals("to"))
            {
                AstNode toStatement = new AstNode(nextToken);
                forStatement.addChild(toStatement);
                toStatement.addChild(Parser.parseExpression(reader));
                nextToken = reader.readNextToken();
            }
            if (nextToken.GetValue().Equals("step"))
            {
                AstNode stepNode = new AstNode(nextToken);
                forStatement.addChild(stepNode);
                stepNode.addChild(Parser.parseExpression(reader));
            }
            forStatement.addChild(Parser.parseLoopBody(reader));
            reader.clear();
            return forStatement;
        }

        public static AstNode parseWhile(TokenReader reader){
            AstNode whileStatement = new AstNode("while");
            Token nextToken = reader.readNextToken();
            if (nextToken.GetValue().Equals("while"))
            {
                try
                {
                    whileStatement.addChild(Parser.parseExpression(reader));
                }
                catch (SyntaxError e)
                {
                    reader.saveReadedTokens();
                    whileStatement.addChild(Parser.parseLoopBody(reader));
                    reader.clear();
                            return whileStatement;
                }
            }
            whileStatement.addChild(Parser.parseLoopBody(reader));
            reader.clear();
            return whileStatement;
        }

        public static AstNode parseLoopBody(TokenReader reader){
            AstNode loopBody = new AstNode("loopbody");
            Token nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("loop"))
            {
                reader.saveReadedTokens();
                throw new SyntaxError("");
            }
            while (true)
            {
                try
                {
                    loopBody.addChild(Parser.parseStatement(reader));
                }
                catch (SyntaxError e)
                {
                    reader.saveReadedTokens();
                    nextToken = reader.readNextToken();
                    if (!nextToken.GetValue().Equals("end"))
                    {
                        reader.saveReadedTokens();
                        throw new SyntaxError("Can't parse loop");

                    }
                    reader.clear();
                    return loopBody;

                }
            }

        }

        public static AstNode parseBreak(TokenReader reader)
        {
            AstNode breakNode = new AstNode(null);
            Token nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("break"))
            {
                reader.saveReadedTokens();
                throw new SyntaxError("");
            }
            breakNode.setValue(nextToken);
            reader.clear();
            return breakNode;
        }

         public static AstNode parseSwap(TokenReader reader){
            AstNode swap = new AstNode(null);
            swap.addChild(Parser.parsePrimary(reader));
            Token nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("<=>"))
            {
                reader.saveReadedTokens();
                throw new SyntaxError("");
            }
            swap.setValue(nextToken);
            swap.addChild(Parser.parsePrimary(reader));
            nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals(";"))
            {
                reader.saveReadedTokens();
                throw new SyntaxError("");
            }
            reader.clear();
            return swap;
        }

        public static AstNode parseGoto(TokenReader reader)
        {
            AstNode go_to = new AstNode("goto");
            Token nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals("goto"))
            {
                reader.saveReadedTokens();
                throw new SyntaxError("");
            }
            go_to.setValue(nextToken);
            go_to.addChild(Parser.parseIdentifier(reader));
            Token delim = reader.readNextToken();
            if (!delim.GetValue().Equals(";"))
            {
                throw new SyntaxError("");
            }
            reader.clear();
            return go_to;
        }

        public static AstNode parseAssigment(TokenReader reader)
        {
            AstNode assigment = new AstNode(null);
            assigment.addChild(Parser.parsePrimary(reader));
            Token nextToken = reader.readNextToken();
            if (nextToken.GetValue().Equals(":="))
            {
                assigment.setValue(":=");
            }
            else
            {
                reader.saveReadedTokens();
                throw new SyntaxError("Can't parse assigment");
            }
            assigment.addChild(Parser.parseExpression(reader));
            nextToken = reader.readNextToken();
            if (!nextToken.GetValue().Equals(";"))
            {
                assigment.setValue(":=");
            }
            reader.clear();
            return assigment;
        }


    }
}
