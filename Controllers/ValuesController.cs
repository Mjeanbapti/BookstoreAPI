using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookstoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]

        public IEnumerable<string> Get()
        {
            return new string[] { "Values 1", "Values2", "Values3", "values4", "values5" };
        }

        [HttpGet("{id}")]

        public string Get(int id)
        {
            return "The values is " + id;
        }
    }
}
