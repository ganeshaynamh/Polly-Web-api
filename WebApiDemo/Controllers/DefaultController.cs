using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApiDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DefaultController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;

        public DefaultController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        // GET api/values  
        [HttpGet]
        public async Task<string> GetAsync()
        {
            // define a 404 page  
            var url = $"https://www.c-sharpcorner.com/mytestpagefor404";

            var client = _clientFactory.CreateClient("csharpcorner");
            var response = await client.GetAsync(url);
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }
    }
}