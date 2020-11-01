using System;
using System.Collections.Generic;

namespace Utils.Exceptions
{
    [Serializable]
    public class BadRequestException : DemoAppException
    {
        public BadRequestException(string message, Dictionary<string, string> fieldSpecificMessages = null) : base(
            message, fieldSpecificMessages)
        {
        }
    }
}