﻿using System;
using System.Collections.Generic;
using Erasystemlevel.Exception;
using Erasystemlevel.Tokenizer;

namespace Erasystemlevel.Parser
{
    public class Parser
    {
        private LinkedList<Token> tokens;
        public Parser(LinkedList<Token> tokens)
        {
            this.tokens = tokens;
        }

        private AstNode ParseUnit(){
            try{
                AstNode node = parseCode();
                return node;
            }catch(InvalidOperationException e){

            }
            
            try{
                AstNode node = parseData();
                return node;
            }catch(InvalidOperationException e){

            }
            try
            {
                AstNode node = parseModule();
                return node;
            }
            catch (InvalidOperationException e)
            {

            }
            try
            {
                AstNode node = parseRoutine();
                return node;
            }
            catch (InvalidOperationException e)
            {

            }
            return null;

        }

        private AstNode parseCode(){
            Token nextToken = tokens.First.Value;
            tokens.RemoveFirst();
            AstNode code = new AstNode("Code");
            if(!(nextToken.GetTokenType() == Token.TokenType.Keyword && nextToken.GetValue().Equals("code"))){
                throw new InvalidOperationException("Cant parse code");
            }
            nextToken = tokens.First.Value;

            while (!(nextToken.GetTokenType() == Token.TokenType.Keyword && nextToken.GetValue().Equals("end"))&&tokens.Count>0){
                tokens.RemoveFirst();
                try{
                    code.addChild(parseVariable());
                }catch(InvalidOperationException e){

                }
                try
                {
                    code.addChild(parseConstant());
                }
                catch (InvalidOperationException e)
                {

                }
                try
                {
                    code.addChild(parseStatement());
                }
                catch (InvalidOperationException e)
                {

                }

                nextToken = tokens.First.Value;

            }
            if (!nextToken.GetValue().Equals("end")){
                throw new SyntaxError("Error in code parsing");
            }

        }
        private AstNode parseData(){
            return null;
        }
        private AstNode parseRoutine()
        {
            return null;
        }
        private AstNode parseModule()
        {
            return null;
        }
        private AstNode parseVariable(){
            AstNode variable = new AstNode("Variable");

            try{
                variable.addChild(parseType());
            }catch(SystemException e){

            }
            Token nextToken = tokens.First.Value;

            while (nextToken.GetValue().Equals(";")){

            }
        }
        private AstNode parseConstant()
        {
            return null;
        }


        private AstNode parseIdentifier()
        {
            return null;
        }

        private AstNode parseLiteral()
        {
            return null;
        }

        private AstNode parseDeclaration()
        {
            AstNode variable = new AstNode("Declaration");

            return null;
        }

        private AstNode parseVarDefinition()
        {
            return null;
        }

        private AstNode parseType()
        {
            return null;
        }

        private AstNode parseExpression()
        {
            return null;
        }

        private AstNode parseConstDefinition()
        {
            return null;
        }

        private AstNode parseStatement()
        {
            return null;
        }
        private AstNode parseLabel()
        {
            return null;
        }
        private AstNode parseAssemblerStatement()
        {
            return null;
        }
        private AstNode parseExtensionStatement()
        {
            return null;
        }
        private AstNode parseDirective()
        {
            return null;
        }
        private AstNode parseAttribute()
        {
            return null;
        }
        private AstNode parseParameters()
        {
            return null;
        }
        private AstNode parseResults()
        {
            return null;
        }

        private AstNode parseParameter()
        {
            return null;
        }
        private AstNode parseRegister()
        {
            return null;
        }
        private AstNode parseRoutineBody()
        {
            return null;
        }
        private AstNode parseRoutineBody()
        {
            return null;
        }
        private AstNode parsePrimary()
        {
            return null;
        }
        private AstNode parseVariableReference()
        {
            return null;
        }
        private AstNode parseDereference()
        {
            return null;
        }
        private AstNode parseArrayElement()
        {
            return null;
        }
        private AstNode parseDataElement()
        {
            return null;
        }
        private AstNode parseRegister()
        {
            return null;
        }
        private AstNode parseExplicitAddress()
        {
            return null;
        }

        private AstNode parseOperand()
        {
            return null;
        }
        private AstNode parseOperator()
        {
            return null;
        }
        private AstNode parsePrimitiveOperator()
        {
            return null;
        }
        private AstNode parseAssemblerStatement()
        {
            return null;
        }
        private AstNode parseRegister()
        {
            return null;
        }
        private AstNode parseDirective()
        {
            return null;
        }
        private AstNode parseExtensionStatement()
        {
            return null;
        }
        private AstNode parseFor()
        {
            return null;
        }
        private AstNode parseWhile()
        {
            AstNode whileStatement = new AstNode("While");
            Token nextToken = tokens.First.Value;
            if (nextToken.GetValue().Equals("while"))
            {
                whileStatement.addChild(new AstNode(tokens.First.Value));
                tokens.RemoveFirst();
            }
            else
            {
                throw new SyntaxError("Can't parse while");
            }
            while (true)
            {
                try
                {
                    whileStatement.addChild(parseExpression());
                }
                catch (SyntaxError e)
                {
                    nextToken = tokens.First.Value;
                    whileStatement.addChild(parseLoopBody);
                }
            }

            return whileStatement;
        }

        private AstNode parseLoopBody()
        {
            AstNode loopBody = new AstNode("LoopBody");
            Token nextToken = tokens.First.Value;
            if (nextToken.GetValue().Equals("loop"))
            {
                loopBody.addChild(new AstNode(tokens.First.Value));
                tokens.RemoveFirst();
            }
            else
            {
                throw new SyntaxError("Can't parse loop");
            }

            while(true){
                try{
                    loopBody.addChild(parseStatement());
                }catch(SyntaxError e){
                    nextToken = tokens.First.Value;
                    if (nextToken.GetValue().Equals("end"))
                    {
                        loopBody.addChild(new AstNode(nextToken));
                        tokens.RemoveFirst();
                        return loopBody;
                    }
                    else
                    {
                        throw new SyntaxError("Can't parse loop");
                    }
                }
            }

        }

        private AstNode parseBreak()
        {
            AstNode breakNode = new AstNode("Break");
            Token nextToken = tokens.First.Value;
            if (nextToken.GetValue().Equals("break"))
            {
                breakNode.addChild(new AstNode(tokens.First.Value));
                tokens.RemoveFirst();
            }
            else
            {
                throw new SyntaxError("Can't parse swap");
            }
            return breakNode;
        }

        private AstNode parseSwap()
        {
            AstNode swap = new AstNode("Swap");
            swap.addChild(parsePrimary());
            Token nextToken = tokens.First.Value;
            if (nextToken.GetValue().Equals("<=>"))
            {
                swap.addChild(new AstNode(tokens.First.Value));
                tokens.RemoveFirst();
            }
            else
            {
                throw new SyntaxError("Can't parse swap");
            }
            swap.addChild(parsePrimary());

            return swap;
        }

        private AstNode parseGoto()
        {
            AstNode go_to = new AstNode("GoTo");
            Token nextToken = tokens.First.Value;

            if (nextToken.GetValue().Equals("goto"))
            {
                go_to.addChild(new AstNode(tokens.First.Value));
                tokens.RemoveFirst();

            }
            else
            {
                throw new SyntaxError("Can't parse goto");
            }

            go_to.addChild(parseIdentifier());



            return go_to;
        }
        private AstNode parseAssigment(){
            AstNode assigment = new AstNode("Assigment");
            assigment.addChild(parsePrimary());
            Token nextToken = tokens.First.Value;
            if (nextToken.GetValue().Equals(":="))
            {
                assigment.addChild(new AstNode(tokens.First.Value));
                tokens.RemoveFirst();
            }
            else
            {
                throw new SyntaxError("Can't parse assigment");
            }
            assigment.addChild(parseExpression());
            return assigment;
        }


    }
}
