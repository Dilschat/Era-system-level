using Erasystemlevel.Generator;
using Erasystemlevel.Parser;
using Erasystemlevel.Tokenizer;
using NUnit.Framework;

namespace Erasystemlevel.Tests
{
    [TestFixture]
    public class AssemblyUnitTests
    {
        [Test]
        public void AssignmentTest()
        {
            var node = new AstNode(new Token(Token.TokenType.Operator, ":="));
            node.SetNodeType(AstNode.NodeType.OperationOnRegisters);
            var dereference = new AstNode(new Token(Token.TokenType.Register, "R1"));
            node.addChild(new AstNode(new Token(Token.TokenType.Register, "R0")));
            node.addChild(dereference);
            
            var output = AssemblyBuffer.statementToString(node);
            Assert.AreEqual(output, "R0 := R1;");
        }
        
        [Test]
        public void ASRTest()
        {
            var node = new AstNode(new Token(Token.TokenType.Operator, ">>="));
            node.SetNodeType(AstNode.NodeType.OperationOnRegisters);
            var dereference = new AstNode(new Token(Token.TokenType.Register, "R1"));
            node.addChild(new AstNode(new Token(Token.TokenType.Register, "R0")));
            node.addChild(dereference);
            
            var output = AssemblyBuffer.statementToString(node);
            Assert.AreEqual(output, "R0 >>= R1;");
        }
        
        [Test]
        public void StarTest()
        {
            var node = new AstNode(new Token(Token.TokenType.Operator, ":="));
            node.SetNodeType(AstNode.NodeType.OperationOnRegisters);
            var star = new AstNode(new Token(Token.TokenType.Operator, "*"));
            star.addChild(new AstNode(new Token(Token.TokenType.Register, "R1")));
            node.addChild(star);
            node.addChild(new AstNode(new Token(Token.TokenType.Register, "R0")));
            
            var output = AssemblyBuffer.statementToString(node);
            Assert.AreEqual(output, "* R1 := R0;");
        }
        
        [Test]
        public void SkipTest()
        {
            var node = new AstNode(new Token(Token.TokenType.Keyword, "skip"));
            node.SetNodeType(AstNode.NodeType.AssemblerStatement);
            node.addChild(new AstNode(new Token(Token.TokenType.Number, "2")));
            
            var output = AssemblyBuffer.statementToString(node);
            Assert.AreEqual(output, "skip 2;");
        }
        
        [Test]
        public void IfTest()
        {
            var node = new AstNode(new Token(Token.TokenType.Keyword, "if"));
            node.SetNodeType(AstNode.NodeType.AssemblerStatement);
            node.addChild(new AstNode(new Token(Token.TokenType.Register, "R0")));
            var goTo = new AstNode(new Token(Token.TokenType.Keyword, "goto"));
            node.addChild(goTo);
            goTo.addChild(new AstNode(new Token(Token.TokenType.Register, "R1")));
            
            var output = AssemblyBuffer.statementToString(node);
            Assert.AreEqual(output, "if R0 goto R1;");
        }
        
    }
}