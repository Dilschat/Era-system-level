using System;

namespace Erasystemlevel.Exception
{
    public class GenerationError: SystemException
    {
        public GenerationError(string message) : base(message)
        {
        }
    }
}