using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hello.restaurant.api.Models
{
    public class RestaurantParam
    {
        public string Id { get; set; }
        //public Clan Clan { get; set; }
        public string Title { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime Created { get; set; }
        
        public int Level { get; set; }

       
    }
}