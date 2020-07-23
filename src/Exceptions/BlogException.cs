using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using hello.restaurant.api.Models;

namespace hello.restaurant.api.Exceptions
{
     public class BlogException : Exception
    {
        public BlogException()
        {
        }

        public BlogException(string message) : base(message)
        {
        }

        public BlogException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BlogException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    public class BlogNotFoundException : BlogException
    {
        public BlogNotFoundException(Blog blog)
            : base($"Blog {blog.Title}, ({blog.Id}) of {blog.Name} was not found.")
        {
        }

        public BlogNotFoundException(string blogKey)
            : base($"Blog {blogKey} of was not found.")
        {
        }
    }
}