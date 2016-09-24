using System;
using System.Runtime.Serialization;


namespace ErrorUnit.Logger_Elmah
{
    [Serializable]
    internal class ErrorUnitException : Exception
    {
        private string testableErrorJson;

        public ErrorUnitException() { }

        public ErrorUnitException(string testableErrorJson, Exception exception) : base(exception.Message, exception)
        {
            this.testableErrorJson = testableErrorJson;          
        }

        public ErrorUnitException(string message) : base(message) { }

      
        protected ErrorUnitException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override string ToString()
        {
            return testableErrorJson == null ? base.ToString() : testableErrorJson;
        }
    }
}