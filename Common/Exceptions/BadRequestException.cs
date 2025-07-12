using System;

namespace Common.Exceptions
{
    public class BadRequestException : ApiException
    {
        public BadRequestException(string message) : base(message, 400)
        {
        }

        public BadRequestException(string message, Exception innerException) 
            : base(message, innerException, 400)
        {
        }
    }
} 