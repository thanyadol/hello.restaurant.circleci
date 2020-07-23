using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using hello.restaurant.api.Models;
using Microsoft.AspNetCore.Http;
//using ForEvolve.Blog.Samples.NinjaApi.Services;

//service
using hello.restaurant.api.Services;
using Microsoft.AspNetCore.Cors;
using hello.restaurant.api.Exceptions;

namespace hello.restaurant.api.Controllers
{
    [Route("api/[controller]")]
    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;
       // private readonly NorthwindContext _context;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService ?? throw new ArgumentNullException(nameof(blogService));
           // _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        

        [EnableCors("AllowCors")]
        [Route("list")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Blog>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListAsync()
        {
            var blogs = await _blogService.ListAsync();
            return Ok(blogs);
        }

        [EnableCors("AllowCors")]
        [Route("post")]
        [HttpPost]
        [ProducesResponseType(typeof(Blog), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAsync([FromBody]Blog blog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createBlog = await _blogService.CreateAsync(blog);
            
            return CreatedAtAction(
                nameof(CreateAsync),
                createBlog
            );
        }


        [EnableCors("AllowCors")]
        [Route("get/{key}")]
        [HttpGet]
        [ProducesResponseType(typeof(Blog), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync(string key)
        {
            var blog = await _blogService.GetAsync(key);

            if(blog == null)
            {
                return NotFound();
            } 
            else
            {
                return Ok(blog);
            }
        }


        /*  [HttpGet("{clan}")]
        [ProducesResponseType(typeof(IEnumerable<Ninja>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ReadAllInClanAsync(string clan)
        {
            try
            {
                var clanNinja = await _ninjaService.ReadAllInClanAsync(clan);
                return Ok(clanNinja);
            }
            catch (ClanNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(Ninja), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAsync([FromBody]Ninja ninja)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdNinja = await _ninjaService.CreateAsync(ninja);
            return CreatedAtAction(
                nameof(ReadOneAsync),
                new { clan = createdNinja.Clan.Name, key = createdNinja.Key },
                createdNinja
            );
        }

        [HttpPut]
        [ProducesResponseType(typeof(Ninja), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateAsync([FromBody]Ninja ninja)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedNinja = await _ninjaService.UpdateAsync(ninja);
                return Ok(updatedNinja);
            }
            catch (NinjaNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{clan}/{key}")]
        [ProducesResponseType(typeof(Ninja), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync(string clan, string key)
        {
            try
            {
                var deletedNinja = await _ninjaService.DeleteAsync(clan, key);
                return Ok(deletedNinja);
            }
            catch (NinjaNotFoundException)
            {
                return NotFound();
            }
        } */
    }
}



