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
            var tokenizer = new Tokenizer.Tokenizer(getTestFilePath("codeData1.txt"));
            var reader = new TokenReader(tokenizer);
            var node = Parser.Parser.ParseUnit(reader);
            Console.WriteLine(node.ToString());
        }
        
        [Test]
        public void DataTest2Case()
        {
            var tokenizer = new Tokenizer.Tokenizer(getTestFilePath("codeData2.txt"));
            var reader = new TokenReader(tokenizer);
            var node = Parser.Parser.ParseUnit(reader);
            Console.WriteLine(node.ToString());
        }
        
        [Test]
        public void ModuleTest1Case()
        {
            var tokenizer = new Tokenizer.Tokenizer(getTestFilePath("codeModule1.txt"));
            var reader = new TokenReader(tokenizer);
            var node = Parser.Parser.ParseUnit(reader);
            Console.WriteLine(node.ToString());
        }
        
        [Test]
        public void ModuleTest2Case()
        {
            var tokenizer = new Tokenizer.Tokenizer(getTestFilePath("codeModule2.txt"));
            var reader = new TokenReader(tokenizer);
            var node = Parser.Parser.ParseUnit(reader);
            Console.WriteLine(node.ToString());
        }
        
        [Test]
        public void ModuleTest3Case()
        {
            var tokenizer = new Tokenizer.Tokenizer(getTestFilePath("codeModule3.txt"));
            var reader = new TokenReader(tokenizer);
            var node = Parser.Parser.ParseUnit(reader);
            Console.WriteLine(node.ToString());
        }
        
        [Test]
        public void codeTest1Case()
        {
            var tokenizer = new Tokenizer.Tokenizer(getTestFilePath("codeCode1.txt"));
            var reader = new TokenReader(tokenizer);
            var node = Parser.Parser.ParseUnit(reader);
            Console.WriteLine(node.ToString());
        }
        [Test]
        public void codeTest2Case()
        {
            var tokenizer = new Tokenizer.Tokenizer(getTestFilePath("codeCode2.txt"));
            var reader = new TokenReader(tokenizer);
            var node = Parser.Parser.ParseUnit(reader);
            Console.WriteLine(node.ToString());
        }
        [Test]
        public void codeTest3Case()
        {
            var tokenizer = new Tokenizer.Tokenizer(getTestFilePath("codeCode3.txt"));
            var reader = new TokenReader(tokenizer);
            var node = Parser.Parser.ParseUnit(reader);
            Console.WriteLine(node.ToString());
        }
        [Test]
        public void codeTest4Case()
        {
            var tokenizer = new Tokenizer.Tokenizer(getTestFilePath("codeCode4.txt"));
            var reader = new TokenReader(tokenizer);
            var node = Parser.Parser.ParseUnit(reader);
            Console.WriteLine(node.ToString());
        }
        
        [Test]
        public void routineTest1Case()
        {
            var tokenizer = new Tokenizer.Tokenizer(getTestFilePath("codeRoutine1.txt"));
            var reader = new TokenReader(tokenizer);
            var node = Parser.Parser.ParseUnit(reader);
            Console.WriteLine(node.ToString());
        }
        [Test]
        public void routineTest2Case()
        {
            var tokenizer = new Tokenizer.Tokenizer(getTestFilePath("codeRoutine2.txt"));
            var reader = new TokenReader(tokenizer);
            var node = Parser.Parser.ParseUnit(reader);
            Console.WriteLine(node.ToString());
        }
        [Test]
        public void routineTest3Case()
        {
            var tokenizer = new Tokenizer.Tokenizer(getTestFilePath("codeRoutine3.txt"));
            var reader = new TokenReader(tokenizer);
            var node = Parser.Parser.ParseUnit(reader);
            Console.WriteLine(node.ToString());
        }
        [Test]
        public void routineTest4Case()
        {
            var tokenizer = new Tokenizer.Tokenizer(getTestFilePath("codeRoutine4.txt"));
            var reader = new TokenReader(tokenizer);
            var node = Parser.Parser.ParseUnit(reader);
            Console.WriteLine(node.ToString());
        }
        private static string getTestFilePath(string fileName)
        {
            return "Tests/codeSamples/" + fileName;
        }
    }
}