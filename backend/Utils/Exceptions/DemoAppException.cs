using System;
using System.Collections.Generic;

namespace Utils.Exceptions
{
    [Serializable]
    public class DemoAppException : Exception
    {
        public Dictionary<string, string> FieldSpecificMessages { get; } = new Dictionary<string, string>();
        public string OverallMessage { get; }
        
        public DemoAppException()
        {
        }

        public DemoAppException(string message, Dictionary<string, string> fieldSpecificMessages = null) : base(message)
        {
            OverallMessage = message;
            
            if (fieldSpecificMessages != null)
            {
                FieldSpecificMessages = fieldSpecificMessages;
            }
        }
    }
}