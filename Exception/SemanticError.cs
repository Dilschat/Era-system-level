using System;

namespace Erasystemlevel.Exception
{
    public class SemanticError : SystemException
    {
        public SemanticError(string message) : base(message)
        {
        }
    }
}