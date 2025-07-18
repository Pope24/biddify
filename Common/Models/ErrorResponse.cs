using System;
using System.Collections.Generic;

namespace Common.Models
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string TraceId { get; set; }
        public DateTime Timestamp { get; set; }
        public Dictionary<string, List<string>> Errors { get; set; }

        public ErrorResponse()
        {
            Timestamp = DateTime.UtcNow;
            Errors = new Dictionary<string, List<string>>();
        }

        public ErrorResponse(int statusCode, string message) : this()
        {
            StatusCode = statusCode;
            Message = message;
        }
    }
} 