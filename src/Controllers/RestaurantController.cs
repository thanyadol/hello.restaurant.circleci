using System;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authentication;

//logg
using Serilog;

//services
using System.IO;
using System.Net.Http;
using hello.restaurant.api.Models;
using hello.restaurant.api.Services;

namespace cvx.lct.vot.api.Controllers
{
    //[Authorize]
    //[ServiceFilter(typeof(EnsureUserAuthorizeInAsync))]
    [ApiVersion("1.0")]
    [Route("api/{version:apiVersion}/[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;
        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService ?? throw new ArgumentNullException(nameof(restaurantService));
        }

        [EnableCors("AllowCores")]
        [Route("list")]
        [HttpGet]
        [ProducesResponseType(typeof(Restaurant), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Restaurant), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ListAsync(string keyword)
        {

            var pppp = await _restaurantService.ListAsync(keyword);
            return Ok(pppp);
        }


    }
}