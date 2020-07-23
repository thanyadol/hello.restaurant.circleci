using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using hello.restaurant.api.Models;

namespace hello.restaurant.api.Exceptions
{
    public class RestaurantException : Exception
    {
        public RestaurantException()
        {
        }

        public RestaurantException(string message) : base(message)
        {
        }

        public RestaurantException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RestaurantException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    public class RestaurantNotFoundException : RestaurantException
    {
        public RestaurantNotFoundException(Restaurant restaurant)
            : base($"Restaurant {restaurant.Name}, ({restaurant.Id}) of {restaurant.Name} was not found.")
        {
        }

        public RestaurantNotFoundException(string restaurantKey)
            : base($"Restaurant {restaurantKey} of was not found.")
        {
        }
    }
}