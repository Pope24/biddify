using System;

namespace Common.Exceptions
{
    public class ForbiddenException : ApiException
    {
        public ForbiddenException(string message) : base(message, 403)
        {
        }

        public ForbiddenException(string message, Exception innerException) 
            : base(message, innerException, 403)
        {
        }
    }
} 