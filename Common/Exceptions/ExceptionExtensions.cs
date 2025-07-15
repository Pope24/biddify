using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Exceptions
{
    public static class ExceptionExtensions
    {
        public static void ThrowIfNull<T>(this T? value, string message = "Entity not found")
        {
            if (value == null)
            {
                throw new NotFoundException(message);
            }
        }

        public static void ThrowIfNullOrEmpty<T>(this IEnumerable<T>? collection, string message = "No items found")
        {
            if (collection == null || !collection.Any())
            {
                throw new NotFoundException(message);
            }
        }

        public static void ThrowIfFalse(this bool condition, string message = "Invalid operation")
        {
            if (!condition)
            {
                throw new BadRequestException(message);
            }
        }

        public static void ThrowIfTrue(this bool condition, string message = "Invalid operation")
        {
            if (condition)
            {
                throw new BadRequestException(message);
            }
        }
    }
} 