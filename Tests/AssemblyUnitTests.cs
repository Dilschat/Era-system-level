using System.Collections;
using System.Runtime.InteropServices;
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
        public void AssemblyReadingTest()
        {
            var node = new AstNode(new Token(Token.TokenType.Operator, ":="));
            node.SetNodeType(AstNode.NodeType.OperationOnRegisters);
            var dereference = new AstNode(new Token(Token.TokenType.Register, "R1"));
            node.addChild(new AstNode(new Token(Token.TokenType.Register, "R0")));
            node.addChild(dereference);
            
            var output = AssemblyBuffer.statementToString(node);
            Assert.AreEqual(output, "R0 := R1;");
        }
        
    }
}