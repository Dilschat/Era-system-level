using System;
using System.Collections;
using Erasystemlevel.Parser;
using Erasystemlevel.Tokenizer;
using NUnit.Framework;

namespace Erasystemlevel.Tests
{
    [TestFixture]
    public class Samples
    {
        [Test]
        public void DataTest1Case()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("codeData1.txt"));
            TokenReader reader = new TokenReader(tokenizer);
            AstNode node = Parser.Parser.ParseUnit(reader);
            Console.WriteLine(node.ToString());
        }
        
        [Test]
        public void DataTest2Case()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("codeData2.txt"));
            TokenReader reader = new TokenReader(tokenizer);
            AstNode node = Parser.Parser.ParseUnit(reader);
            Console.WriteLine(node.ToString());
        }
        
        [Test]
        public void ModuleTest1Case()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("codeModule1.txt"));
            TokenReader reader = new TokenReader(tokenizer);
            AstNode node = Parser.Parser.ParseUnit(reader);
            Console.WriteLine(node.ToString());
        }
        
        [Test]
        public void ModuleTest2Case()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("codeModule2.txt"));
            TokenReader reader = new TokenReader(tokenizer);
            AstNode node = Parser.Parser.ParseUnit(reader);
            Console.WriteLine(node.ToString());
        }
        
        [Test]
        public void ModuleTest3Case()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("codeModule3.txt"));
            TokenReader reader = new TokenReader(tokenizer);
            AstNode node = Parser.Parser.ParseUnit(reader);
            Console.WriteLine(node.ToString());
        }
        
        [Test]
        public void codeTest1Case()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("codeCode1.txt"));
            TokenReader reader = new TokenReader(tokenizer);
            AstNode node = Parser.Parser.ParseUnit(reader);
            Console.WriteLine(node.ToString());
        }
        [Test]
        public void codeTest2Case()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("codeCode2.txt"));
            TokenReader reader = new TokenReader(tokenizer);
            AstNode node = Parser.Parser.ParseUnit(reader);
            Console.WriteLine(node.ToString());
        }
        [Test]
        public void codeTest3Case()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("codeCode3.txt"));
            TokenReader reader = new TokenReader(tokenizer);
            AstNode node = Parser.Parser.ParseUnit(reader);
            Console.WriteLine(node.ToString());
        }
        [Test]
        public void codeTest4Case()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("codeCode4.txt"));
            TokenReader reader = new TokenReader(tokenizer);
            AstNode node = Parser.Parser.ParseUnit(reader);
            Console.WriteLine(node.ToString());
        }
        
        [Test]
        public void routineTest1Case()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("codeRoutine1.txt"));
            TokenReader reader = new TokenReader(tokenizer);
            AstNode node = Parser.Parser.ParseUnit(reader);
            Console.WriteLine(node.ToString());
        }
        [Test]
        public void routineTest2Case()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("codeRoutine2.txt"));
            TokenReader reader = new TokenReader(tokenizer);
            AstNode node = Parser.Parser.ParseUnit(reader);
            Console.WriteLine(node.ToString());
        }
        [Test]
        public void routineTest3Case()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("codeRoutine3.txt"));
            TokenReader reader = new TokenReader(tokenizer);
            AstNode node = Parser.Parser.ParseUnit(reader);
            Console.WriteLine(node.ToString());
        }
        [Test]
        public void routineTest4Case()
        {
            Tokenizer.Tokenizer tokenizer = new Tokenizer.Tokenizer(getTestFilePath("codeRoutine4.txt"));
            TokenReader reader = new TokenReader(tokenizer);
            AstNode node = Parser.Parser.ParseUnit(reader);
            Console.WriteLine(node.ToString());
        }
        private static string getTestFilePath(string fileName)
        {
            return "Tests/codeSamples/" + fileName;
        }
    }
}