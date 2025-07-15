using System;

namespace Common.Exceptions
{
    public class UnauthorizedException : ApiException
    {
        public UnauthorizedException(string message) : base(message, 401)
        {
        }

        public UnauthorizedException(string message, Exception innerException) 
            : base(message, innerException, 401)
        {
        }
    }
} 