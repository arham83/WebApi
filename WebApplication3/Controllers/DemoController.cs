using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;
using WebApplication3.GlobalExceptionHandler;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication3.Controllers
{
    [Authorize] // Apply authorization to all endpoints in the controller

    [Route("api/[controller]")]
    [ApiController]
    public class DemoController : ControllerBase
    {
        List<string> names = new List<string>()
        {
            "Arham",
            "Abdul",
            "John",
            "Dimtry"
        };
        public DemoController(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger<DemoController>();
        }

        public ILogger Logger { get; }

        // GET: api/<DemoController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return names;
        }

        // GET api/<DemoController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            var name = names[id]; // Accessing the list with the provided index
            return name;
        }

        // POST api/<DemoController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<DemoController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<DemoController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
