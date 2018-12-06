using System;

namespace Erasystemlevel.Exception
{
    public class SyntaxError : SystemException
    {
        public SyntaxError(string message)
            : base(message)
        {
        }
    }
}