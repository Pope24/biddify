using System;

namespace Common.Exceptions
{
    public class NotFoundException : ApiException
    {
        public NotFoundException(string message) : base(message, 404)
        {
        }

        public NotFoundException(string message, Exception innerException) 
            : base(message, innerException, 404)
        {
        }
    }
} 