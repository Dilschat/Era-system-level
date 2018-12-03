using System;
using System.Collections.Generic;
using Erasystemlevel.Exception;
using Erasystemlevel.Tokenizer;
using Erasystemlevel.Parser;
namespace Erasystemlevel.Parser
{
    public class Parser
    {
        private Tokenizer.Tokenizer tokenizer;
        private Stack<Token> lookaheadBuffer = new Stack<Token>();
        private Stack<Token> savingBuffer = new Stack<Token>();
        public Parser(Tokenizer.Tokenizer tokenizer)
        {
            this.tokenizer = tokenizer;
        }

        public AstNode ParseUnit()
        {
            try
            {
                AstNode node = parseCode();
                lookaheadBuffer.Clear();
                return node;
            }
            catch (SyntaxError e)
            {
                Console.WriteLine(e);
            }
            Console.WriteLine('1');
            try
            {
                AstNode node = parseData();
                lookaheadBuffer.Clear();
                return node;
            }
            catch (SyntaxError e)
            {
                Console.WriteLine(e);
            }
            Console.WriteLine('2');

            try
            {
                AstNode node = parseModule();
                lookaheadBuffer.Clear();
                return node;
            }
            catch (SyntaxError e)
            {
                Console.WriteLine(e);
            }
            try
            {
                AstNode node = parseRoutine();
                lookaheadBuffer.Clear();
                return node;
            }
            catch (SyntaxError e)
            {
                Console.WriteLine(e);
            }
            throw new SyntaxError("");

        }

        private AstNode parseCode()
        {
            Token nextToken = readNextToken();
            AstNode code = new AstNode("Code");
            if (!(nextToken.GetValue().Equals("code")))
            {
                saveReadedTokens();
                throw new SyntaxError("");
            }
            nextToken = null;
            while (true)
            {

                try
                {
                    code.addChild(parseVariable());
                    continue;
                }
                catch (SyntaxError e)
                {

                }
                try
                {
                    code.addChild(parseConstant());
                    continue;
                }
                catch (SyntaxError e)
                {

                }
                try
                {
                    code.addChild(parseStatement());
                    continue;
                }
                catch (SyntaxError e)
                {

                }
                nextToken = readNextToken();

                if (nextToken.GetValue().Equals("end")){
                    lookaheadBuffer.Clear();
                    return code;
                }else{
                    saveReadedTokens();
                    throw new SyntaxError("");
                }

            }

        }
        private AstNode parseData()
        {
            Token nextToken = readNextToken();
            if (!nextToken.GetValue().Equals("data"))
            {
                saveReadedTokens();
                throw new SyntaxError("");
            }
            AstNode node = new AstNode("Data");
            //node.addChild(parseIdentifier());
            try
            {
                node.addChild(parseLiteral());
            }
            catch (SyntaxError e)
            {
                nextToken = readNextToken();
                if (nextToken.GetValue().Equals("end"))
                {
                    lookaheadBuffer.Clear();
                    return node;
                }
                else
                {
                    throw new SyntaxError("");

                }

            }
            while (true)
            {

                nextToken = tokenizer.Tokenize();
                if (nextToken.GetValue().Equals("end"))
                {
                    lookaheadBuffer.Clear();
                    return node;
                }
                else if (nextToken.GetValue().Equals(","))
                {
                    node.addChild(parseLiteral());
                }
            }
        }

        private AstNode parseRoutine()
        {
            AstNode node = new AstNode("Routine");
            try
            {
                node.addChild(parseAttribute());
            }catch(SyntaxError e){}
            Token nextToken = readNextToken();
            if (!nextToken.GetValue().Equals("routine"))
            {
                saveReadedTokens();
                throw new SyntaxError("");
            }
            //node.addChild(parseIdentifier());
            try
            {
                node.addChild(parseParameter());
            }catch(SyntaxError e){}
            nextToken = readNextToken();
            if (nextToken.GetValue().Equals(":"))
            {
                node.addChild(parseResults());
            }
            if (nextToken.GetValue().Equals(";"))
            {
                lookaheadBuffer.Clear();
                return node;
            }
            else if (nextToken.GetValue().Equals("do"))
            {
                node.addChild(parseRoutine());
                nextToken = readNextToken();
                if (!nextToken.GetValue().Equals("end"))
                {
                    saveReadedTokens();
                    throw new SyntaxError("");
                }
                lookaheadBuffer.Clear();
                return node;
            }
            throw new SyntaxError("");
        }

        private AstNode parseModule()
        {
            Token nextToken = readNextToken();
            if (!nextToken.GetValue().Equals("module"))
            {
                saveReadedTokens();
                throw new SyntaxError("");
            }
            AstNode node = new AstNode("Module");
            //node.addChild(parseIdentifier());
            while (true)
            {
                try
                {
                    node.addChild(parseDeclaration());
                }
                catch (SyntaxError e)
                {
                    saveReadedTokens();
                    break;
                }
            }
            if (!nextToken.GetValue().Equals("end"))
            {
                saveReadedTokens();
                throw new SyntaxError("");
            }
            lookaheadBuffer.Clear();
            return node;
        }

        private AstNode parseVariable()
        {
            AstNode node = new AstNode("Variable");
            node.addChild(parseType());
            while (true)
            {
                node.addChild(parseVarDefinition());
                Token nextToken = readNextToken();
                if (nextToken.GetValue().Equals(","))
                {
                    continue;
                }
                else if (nextToken.GetValue().Equals(";"))
                {
                    lookaheadBuffer.Clear();
                    return node;
                }
                else
                {
                    saveReadedTokens();
                    throw new SyntaxError("");
                }
            }
        }

        private AstNode parseConstant()
        {
            Token nextToken = readNextToken();
            if (!(nextToken.GetValue().Equals("const")))
            {
                saveReadedTokens();
                throw new SyntaxError("");
            }
            AstNode node = new AstNode("Constant");
            while (true)
            {
                node.addChild(parseConstDefinition());
                nextToken = readNextToken();
                if (nextToken.GetValue().Equals(","))
                {
                    node.addChild(parseConstDefinition());
                }
                else if (nextToken.GetValue().Equals(";"))
                {
                    lookaheadBuffer.Clear();
                    return node;
                }
                else
                {
                    saveReadedTokens();
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
            reader.clear();
            return new AstNode(nextToken);
        }

        private AstNode parseLiteral() //Literal:  Numeric
        {
            Token nextToken = readNextToken();
            if(nextToken.GetTokenType()==Token.TokenType.Number){
                lookaheadBuffer.Clear();
                return new AstNode(nextToken);
            }
            throw new SyntaxError("");
        }

        private AstNode parseDeclaration()
        {
            AstNode variable = new AstNode("Declaration");
            try
            {
                variable.addChild(parseVariable());
                lookaheadBuffer.Clear();
                return variable;
            }
            catch (SyntaxError e)
            {
                saveReadedTokens();
            }
            try
            {
                variable.addChild(parseConstant());
                lookaheadBuffer.Clear();
                return variable;
            }
            catch (SyntaxError e)
            {
                saveReadedTokens();
            }
            try
            {
                variable.addChild(parseRoutine());
                lookaheadBuffer.Clear();
                return variable;
            }
            catch (SyntaxError e)
            {
                saveReadedTokens();
            }
            throw new SyntaxError("");
        }

        private AstNode parseVarDefinition(){ 
            AstNode node = new AstNode("VarDefinition");
            //node.addChild(parseIdentifier());
            Token nextToken = readNextToken();
            if(nextToken.GetValue().Equals(":=")){
                node.addChild(parseExpression());
                lookaheadBuffer.Clear();
                return node;
            }
            else if(nextToken.GetValue().Equals("[")){
                node.addChild(parseExpression());
                nextToken = readNextToken();
                if (nextToken.GetValue().Equals("]"))
                {
                    lookaheadBuffer.Clear();
                    return node;
                }else{
                    throw new SyntaxError("");
                }

            }else{
                saveReadedTokens();
                return node;
            }
        }

        private AstNode parseType()
        {
            Token nextToken = readNextToken();
            if (!(nextToken.GetTokenType() == Token.TokenType.Keyword))
            {
                saveReadedTokens();
                throw new SyntaxError("");
            }
            if (nextToken.GetValue().Equals("int"))
            {
                lookaheadBuffer.Clear();
                return new AstNode(nextToken);
            }
            if (nextToken.GetValue().Equals("short"))
            {
                lookaheadBuffer.Clear();
                return new AstNode(nextToken);
            }
            if (nextToken.GetValue().Equals("byte"))
            {
                lookaheadBuffer.Clear();
                return new AstNode(nextToken);
            }
            saveReadedTokens();
            throw new SyntaxError("");
        }

        private AstNode parseExpression()
        {
            AstNode node = new AstNode("Expression");
            node.addChild(parseOperand());
            try
            {
                //node.addChild(parseOperator());
                try
                {
                    node.addChild(parseOperand());
                    lookaheadBuffer.Clear();
                    return node;
                }
                catch (SyntaxError e)
                {
                    saveReadedTokens();
                    throw new SyntaxError("");
                }
            }
                catch (SyntaxError e){
                lookaheadBuffer.Clear();
                return node;
            }
        }

        private AstNode parseConstDefinition()
        {
            AstNode node = new AstNode(null);
           //node.addChild(parseIdentifier());
            Token nextToken = readNextToken();
            if (!nextToken.GetValue().Equals("="))
            {
                saveReadedTokens();
                throw new SyntaxError("");
            }
            node.setValue(nextToken);
            node.addChild(parseExpression());
            lookaheadBuffer.Clear();
            return node;
        }

        private AstNode parseStatement()
        {
            AstNode node = new AstNode("Statement");
            try
            {
                while (true)
                {
                    node.addChild(parseLabel());
                }
            }
            catch (SyntaxError e)
            {
                saveReadedTokens();
                throw new SyntaxError("");
            }
            try
           {
                node.addChild(parseAssemblerStatement());
                lookaheadBuffer.Clear();
                return node;
            }
            catch (SyntaxError e)
            {
                saveReadedTokens();
            }
            try
            {
                node.addChild(parseExtensionStatement());
                lookaheadBuffer.Clear();
                return node;
            }
            catch (SyntaxError e)
            {
                saveReadedTokens();
            }
            try
            {
                node.addChild(parseDirective());
                lookaheadBuffer.Clear();
                return node;

            }
            catch (SyntaxError e)
            {
                saveReadedTokens();
                throw new SyntaxError("Cant parse statement");
            }
            throw new SyntaxError("Cant parse statement");
        }

        private AstNode parseLabel(){
            Token nextToken = readNextToken();
            AstNode node = new AstNode("Label");
            if (!nextToken.GetValue().Equals("<"))
            {
                saveReadedTokens();
                throw new SyntaxError("Can't parse error");
            }
            nextToken = readNextToken();
            if (!(nextToken.GetTokenType() == Token.TokenType.Identifier))
            {
                saveReadedTokens();
                throw new SyntaxError("Can't parse error");

            }
            node.addChild(new AstNode(nextToken));
            nextToken = readNextToken();
            if (!nextToken.GetValue().Equals(">"))
            {
                saveReadedTokens();
                throw new SyntaxError("Can't parse error");
            }
            lookaheadBuffer.Clear();
            return node;
        }

        private AstNode parseAssemblerStatement(){
            AstNode node = new AstNode("AssemblerStatement");
            Token nextToken = readNextToken();
            if(nextToken.GetValue().Equals("skip")){
                node.addChild(new AstNode(nextToken));
                try{
                    node.addChild(parseExpression());
                    lookaheadBuffer.Clear();
                    return node;
                }catch(SyntaxError e){
                    lookaheadBuffer.Clear();
                    return node;
                }
            }else if (nextToken.GetValue().Equals("stop"))
            {
                node.addChild(new AstNode(nextToken));
                try
                {
                    node.addChild(parseExpression());
                    lookaheadBuffer.Clear();
                    return node;
                }
                catch (SyntaxError e)
                {
                    lookaheadBuffer.Clear();
                    return node;
                }
            }
            List<string> operators =new List<string>(new string[]{ ":=", "+=", ">>=", "-=", "<<=", "|=", "&=", "^="
            , "<=", ">=", "?="});
            try
            {
                node.addChild(parseOperationOnRegisters(operators));
            }catch(SyntaxError e){
                try
                {
                    if (nextToken.GetValue().Equals("if"))
                    {
                        node.addChild(new AstNode(nextToken));
                        node.addChild(parseRegister());
                        Token newToken = readNextToken();
                        if(!nextToken.GetValue().Equals("goto")){
                            throw new SyntaxError("");
                        }
                        node.addChild(new AstNode(newToken));
                        node.addChild(parseRegister());
                    }else{
                        saveReadedTokens();
                        throw new SyntaxError("");
                    }
                }catch(SyntaxError){
                    saveReadedTokens();
                    throw new SyntaxError("");
                }
            }
            lookaheadBuffer.Clear();
            return node;
        }
        private AstNode parseOperationOnRegisters(List<string> operators){
            AstNode leftOperand = null;
            try
            {
              // leftOperand = parseOperator();
            }catch(SyntaxError e){}
            if(leftOperand!=null && ((Token)leftOperand.getValue()).GetValue().Equals("*")){
                leftOperand.addChild(parseRegister());
            }else{
                leftOperand = parseRegister();
            }
            Token operation =readNextToken();
            bool exists = false;
            for (int i = 0; i < operators.Count; i++){
                if(operators[i].Equals(operation.GetValue())){
                    exists = true;
                }
            }
            if(exists==false){
                saveReadedTokens();
                throw new SyntaxError("");
            }
            AstNode rightOperand = null;

            if (rightOperand != null && ((Token)rightOperand.getValue()).GetValue().Equals("*"))
            {
                rightOperand.addChild(parseRegister());
            }
            else
            {
                rightOperand = parseRegister();
            }
            AstNode node = new AstNode(operation);
            node.addChild(leftOperand);
            node.addChild(rightOperand);
            lookaheadBuffer.Clear();
            return node;
        
        }

        private AstNode parseAttribute()
        {
            Token nextToken = readNextToken();
            if (nextToken.GetValue().Equals("start"))
            {
                lookaheadBuffer.Clear();
                return new AstNode(nextToken);
            }
            if (nextToken.GetValue().Equals("entry"))
            {
                lookaheadBuffer.Clear();
                return new AstNode(nextToken);
            }
            saveReadedTokens();
            throw new SyntaxError("Can't parse attribute");

        }
        private AstNode parseParameters()
        {
            AstNode node = new AstNode("Parameters");
            Token nextToken = readNextToken();
            if (!nextToken.GetValue().Equals("("))
            {
                saveReadedTokens();
                throw new SyntaxError("Cant parse parameters");
            }
            while (true)
            {
                node.addChild(parseParameter());
                nextToken = readNextToken();
                if (nextToken.GetValue().Equals(","))
                {
                    node.addChild(parseParameter());
                }
                else
                {
                    break;
                }
            }
            if (!nextToken.GetValue().Equals(")"))
            {
                saveReadedTokens();
                throw new SyntaxError("Cant parse parameters");
            }
            lookaheadBuffer.Clear();
            return node;
        }

        private AstNode parseResults()
        {
            AstNode node = new AstNode("Results");
            while (true)
            {
                node.addChild(parseRegister());
                Token nextToken = readNextToken();
                if (nextToken.GetValue().Equals(","))
                {
                    node.addChild(parseRegister());
                    continue;
                }
                else
                {
                    lookaheadBuffer.Clear();
                    return node;
                }
            }
        }

        private AstNode parseParameter()
        {
            AstNode node = new AstNode("Parameter");
            try
            {
                node.addChild(parseType());
                //node.addChild(parseIdentifier());
                lookaheadBuffer.Clear();
                return node;
            }
            catch (SyntaxError e)
            {
                saveReadedTokens();
                node.cleanChild();
            }
            try
            {
                node.addChild(parseRegister());
                lookaheadBuffer.Clear();
                return node;
            }
            catch (SyntaxError e)
            {
                saveReadedTokens();
                throw new SyntaxError("Cant parse parameter");
            }
        }

        private AstNode parseRoutineBody()
        {
            AstNode node = new AstNode("RoutineBody");
            while (true)
            {
                try
                {
                    node.addChild(parseVariable());
                    continue;
                }
                    catch (SyntaxError e) { saveReadedTokens(); }
                try
                {
                    node.addChild(parseConstant());
                    continue;
                }
                catch (SyntaxError e) { saveReadedTokens(); }
                try
                {
                    node.addChild(parseStatement());
                    continue;
                }
                catch (SyntaxError e) { saveReadedTokens(); }
                break;
            }
            lookaheadBuffer.Clear();
            return node;
        }

        private AstNode parsePrimary()
        {
            try
            {
                lookaheadBuffer.Clear();
                return parseVariableReference();
            }
            catch (SyntaxError e) { saveReadedTokens(); }
            try
            {
                lookaheadBuffer.Clear();
                return parseDereference();
            }
            catch (SyntaxError e)
            {
                saveReadedTokens();
            }
            try
            {
                lookaheadBuffer.Clear();
                return parseArrayElement();
            }
            catch (SyntaxError e)
            {
                saveReadedTokens();
            }
            try
            {
                lookaheadBuffer.Clear();
                return parseDataElement();
            }
            catch (SyntaxError e)
            {
                saveReadedTokens();
            }
            try
            {
                lookaheadBuffer.Clear();
                return parseRegister();
            }
            catch (SyntaxError e)
            {
                saveReadedTokens();
            }
            try
            {
                lookaheadBuffer.Clear();
                return parseExplicitAddress();
            }
            catch (SyntaxError e)
            {
                saveReadedTokens();
            }
            throw new SyntaxError("Cant parse primary");
        }

        private AstNode parseVariableReference()
        {
            AstNode variableReference = new AstNode(null);
           //variableReference.addChild(parseIdentifier());
            return variableReference;
        }
        private AstNode parseDereference()
        {
            AstNode deference = new AstNode("deference");
            Token nextToken = readNextToken();
            if (!nextToken.GetValue().Equals("*"))
            {
                saveReadedTokens();
                throw new SyntaxError("");
            }
            try
            {
                lookaheadBuffer.Clear();
                return parseVariableReference();
            }
            catch (SyntaxError e)
            {
                saveReadedTokens();
            }
            try
            {
                lookaheadBuffer.Clear();
                return parseRegister();
            }
            catch (SyntaxError e)
            {
                saveReadedTokens();
            }
            return deference;
            throw new SyntaxError("Cant parse operand");
        }
        private AstNode parseArrayElement()//TODO finish node
        {
            AstNode arrayElement = new AstNode("ArrayElement");
            arrayElement.addChild(parseVariableReference());
            Token nextToken = readNextToken();
            if (!nextToken.GetValue().Equals("["))
            {
                saveReadedTokens();
                throw new SyntaxError("");
            }
            arrayElement.addChild(parseExpression());
            if (!nextToken.GetValue().Equals("]"))
            {
                saveReadedTokens();
                throw new SyntaxError("");
            }
            lookaheadBuffer.Clear();
            return arrayElement;
        }
        private AstNode parseDataElement()//TODO finish node
        {
            AstNode dataElement = new AstNode("dataElement");
            //dataElement.addChild(parseIdentifier());
            Token nextToken = readNextToken();
            if (!nextToken.GetValue().Equals("["))
            {
                saveReadedTokens();
                throw new SyntaxError("");
            }
            dataElement.addChild(parseExpression());
            if (!nextToken.GetValue().Equals("]"))
            {
                saveReadedTokens();
                throw new SyntaxError("");
            }
            lookaheadBuffer.Clear();
            return dataElement;
        }

        private AstNode parseExplicitAddress()
        {
            AstNode explicitAddress = new AstNode("explicitAddress");
            Token nextToken = readNextToken();
            if (!nextToken.GetValue().Equals("*"))
            {
                saveReadedTokens();
                throw new SyntaxError("");
            }
            explicitAddress.addChild(parseLiteral());
            lookaheadBuffer.Clear();
            return explicitAddress;
        }

        private AstNode parseOperand()
        {
            try
            {
                lookaheadBuffer.Clear();
                return parseReceiver();
            }
            catch (SyntaxError e)
            {
                saveReadedTokens();
            }
            try
            {
                lookaheadBuffer.Clear();
                return parseLiteral();
            }
            catch (SyntaxError e)
            {
                saveReadedTokens();
            }
            lookaheadBuffer.Clear();
            return parseAddress();
            throw new SyntaxError("Cant parse operand");
        }

        private AstNode parseAddress()//TODO finish node
        {
            AstNode address = new AstNode("Address");
            Token nextToken = readNextToken();
            if (!nextToken.GetValue().Equals("&"))
            {
                saveReadedTokens();
                throw new SyntaxError("");
            }
            address.addChild(parseVariableReference());
            lookaheadBuffer.Clear();
            return address;
        }

        private AstNode parseReceiver()//TODO finish node
        {
            AstNode receiver = new AstNode("Receiver");
            Token nextToken = readNextToken();
            if (!nextToken.GetValue().Equals("this"))
            {
                saveReadedTokens();
                throw new SyntaxError("");
            }
            //receiver.addChild(parseIdentifier());
            lookaheadBuffer.Clear();
            return receiver;
        }

        public static AstNode parseOperator(TokenReader reader)
        {
            Token nextToken = reader.readNextToken();
            Token.TokenType operatorType = Token.TokenType.Operator;
            if (nextToken.GetTokenType().Equals(operatorType))
            {
                reader.clear();
                return new AstNode(nextToken);
            }
            throw new SyntaxError("");
        }

        // Useless node 
        //private AstNode parsePrimitiveOperator()
        //{
        //    AstNode primitiveOperator = new AstNode(null);
        //    Token nextToken = readNextToken();
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



        private AstNode parseRegister()
        {
            AstNode register = new AstNode("Register");
            Token nextToken = readNextToken();
            if (nextToken.GetTokenType().Equals(Token.TokenType.Register))
            {
                register.addChild(new AstNode(nextToken));
            }
            else
            {
                saveReadedTokens();
                throw new SyntaxError("Can't parse register");
            }
            lookaheadBuffer.Clear();
            return register;


        }

        private AstNode parseDirective()
        {
            AstNode directive = new AstNode("Directive");
            Token nextToken = readNextToken();
            if (!nextToken.GetValue().Equals("format"))
            {
                saveReadedTokens();
                throw new SyntaxError("Can't parse directive");
            }
            nextToken = readNextToken();
            if (nextToken.GetValue().Equals("8") || nextToken.GetValue().Equals("16")
                || nextToken.GetValue().Equals("32"))
            {
                directive.addChild(new AstNode(nextToken));
            }
            else
            {
                saveReadedTokens();
                throw new SyntaxError("Can't parse directive");
            }
            lookaheadBuffer.Clear();
            return directive;
        }

        private AstNode parseExtensionStatement()
        {
            AstNode node = new AstNode("ExtensionStatement");
            try
            {
                node.addChild(parseAssigment());
            }
            catch (SyntaxError e) { saveReadedTokens(); }
            try
            {
                node.addChild(parseCall());
                lookaheadBuffer.Clear();
                return node;
            }
            catch (SyntaxError e) { saveReadedTokens(); }
            try
            {
                node.addChild(parseIf());
                lookaheadBuffer.Clear();
                return node;

            }
            catch (SyntaxError e) { saveReadedTokens(); }
            try
            {
                node.addChild(parseWhile());
                lookaheadBuffer.Clear();
                return node;

            }
            catch (SyntaxError e) { saveReadedTokens(); }
            try
            {
                node.addChild(parseFor());
                lookaheadBuffer.Clear();
                return node;

            }
            catch (SyntaxError e) { saveReadedTokens(); }
            try
            {
               // node.addChild(parseBreak());
                lookaheadBuffer.Clear();
                return node;

            }
            catch (SyntaxError e) { saveReadedTokens(); }
            try
            {
                node.addChild(parseSwap());
                lookaheadBuffer.Clear();
                return node;

            }
            catch (SyntaxError e) { saveReadedTokens(); }
            try
            {
                //node.addChild(parseGoto());
                lookaheadBuffer.Clear();
                return node;

            }
            catch (SyntaxError e) { saveReadedTokens(); }
            throw new SyntaxError("");

        }

        private AstNode parseIf() // if Expression do RoutineBody end
        {
            Token nextToken = readNextToken();
            if (!nextToken.GetValue().Equals("if"))
            {
                saveReadedTokens();
                throw new SyntaxError("");
            }
            AstNode ifNode = new AstNode("if");
            ifNode.addChild(parseExpression());
            nextToken = readNextToken();
            if (!nextToken.GetValue().Equals("do"))
            {
                saveReadedTokens();
                throw new SyntaxError("");
            }
            ifNode.addChild(parseRoutineBody());
            if (!nextToken.GetValue().Equals("end"))
            {
                saveReadedTokens();
                throw new SyntaxError("");
            }
            lookaheadBuffer.Clear();
            return ifNode;
        }

        private AstNode parseCall() //Identifier(); // Identifier ( [Identifier {, Identifier}]
        {
            AstNode call = new AstNode("Call");
          // call.addChild(parseIdentifier());
            call.addChild(parseCallParameters());
            lookaheadBuffer.Clear();
            return call;
        }

        private AstNode parseCallParameters()
        {
            AstNode parameters = new AstNode("parameters");
            Token nextToken = readNextToken();
            if (!nextToken.GetValue().Equals("("))
            {
                saveReadedTokens();
                throw new SyntaxError("");
            }
            while (true)
            {
                //    parameters.addChild(parseIdentifier());
                    nextToken = readNextToken();
                    if (nextToken.GetValue().Equals(","))
                    {
                        continue;
                    }
                    if (!nextToken.GetValue().Equals(")"))
                    {
                        saveReadedTokens();
                        throw new SyntaxError("");
                    }
                lookaheadBuffer.Clear();
                return parameters;
                
            }
        }

        private AstNode parseFor() // default: from zero to inf step 1
        {
            AstNode forStatement = new AstNode("For");
            Token nextToken = readNextToken();
            if (nextToken.GetValue().Equals("for"))
            {
                AstNode forNode = new AstNode(nextToken);
                forStatement.addChild(forNode);
              // forNode.addChild(parseIdentifier());
                nextToken = readNextToken();
            }
            if (nextToken.GetValue().Equals("from"))
            {
                AstNode fromStatement = new AstNode(nextToken);
                forStatement.addChild(fromStatement);
                fromStatement.addChild(parseExpression());
                nextToken = readNextToken();
            }
            if (nextToken.GetValue().Equals("to"))
            {
                AstNode toStatement = new AstNode(nextToken);
                forStatement.addChild(toStatement);
                toStatement.addChild(parseExpression());
                nextToken = readNextToken();
            }
            if (nextToken.GetValue().Equals("step"))
            {
                AstNode stepNode = new AstNode(nextToken);
                forStatement.addChild(stepNode);
                stepNode.addChild(parseExpression());
            }
            forStatement.addChild(parseLoopBody());
            lookaheadBuffer.Clear();
            return forStatement;
        }

        private AstNode parseWhile()
        {
            AstNode whileStatement = new AstNode("while");
            Token nextToken = readNextToken();
            if (nextToken.GetValue().Equals("while"))
            {
                try
                {
                    whileStatement.addChild(parseExpression());
                }
                catch (SyntaxError e)
                {
                    saveReadedTokens();
                    whileStatement.addChild(parseLoopBody());
                    lookaheadBuffer.Clear();
                    return whileStatement;
                }
            }
            whileStatement.addChild(parseLoopBody());
            lookaheadBuffer.Clear();
            return whileStatement;
        }

        private AstNode parseLoopBody()
        {
            AstNode loopBody = new AstNode("loopbody");
            Token nextToken = readNextToken();
            if (!nextToken.GetValue().Equals("loop"))
            {
                saveReadedTokens();
                throw new SyntaxError("");
            }
            while (true)
            {
                try
                {
                    loopBody.addChild(parseStatement());
                }
                catch (SyntaxError e)
                {
                    saveReadedTokens();
                    nextToken = readNextToken();
                    if (!nextToken.GetValue().Equals("end"))
                    {
                        saveReadedTokens();
                        throw new SyntaxError("Can't parse loop");

                    }
                    lookaheadBuffer.Clear();
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

        private AstNode parseSwap()
        {
            AstNode swap = new AstNode(null);
            swap.addChild(parsePrimary());
            Token nextToken = readNextToken();
            if (!nextToken.GetValue().Equals("<=>"))
            {
                saveReadedTokens();
                throw new SyntaxError("");
            }
            swap.setValue(nextToken);
            swap.addChild(parsePrimary());
            nextToken = readNextToken();
            if (!nextToken.GetValue().Equals(";"))
            {
                saveReadedTokens();
                throw new SyntaxError("");
            }
            lookaheadBuffer.Clear();
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

        private AstNode parseAssigment()
        {
            AstNode assigment = new AstNode(null);
            assigment.addChild(parsePrimary());
            Token nextToken = readNextToken();
            lookaheadBuffer.Push(nextToken);
            if (nextToken.GetValue().Equals(":="))
            {
                assigment.setValue(":=");
            }
            else
            {
                saveReadedTokens();
                throw new SyntaxError("Can't parse assigment");
            }
            assigment.addChild(parseExpression());
            nextToken = readNextToken();
            if (!nextToken.GetValue().Equals(";"))
            {
                assigment.setValue(":=");
            }
            lookaheadBuffer.Clear();
            return assigment;
        }

        private void saveReadedTokens()
        {
            while (lookaheadBuffer.Count >0)
            {
                savingBuffer.Push(lookaheadBuffer.Pop());
            }
        }

        private Token readNextToken()
        {
            if (savingBuffer.Count >0)
            {
                lookaheadBuffer.Push(savingBuffer.Peek());
                return savingBuffer.Pop();
            }
            Token nextToken = tokenizer.Tokenize();
            lookaheadBuffer.Push(nextToken);
            return nextToken;
        }
    }
}
