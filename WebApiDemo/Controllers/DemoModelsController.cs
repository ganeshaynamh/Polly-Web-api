using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Polly;
using WebApiDemo.Models;

namespace WebApiDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemoModelsController : ControllerBase
    {
        private readonly DemoDbContext _context;
        private readonly IHttpClientFactory _ClientFactory;

        public DemoModelsController(DemoDbContext context, IHttpClientFactory _clientFactory)
        {
            _context = context;
            _ClientFactory = _clientFactory;
        }

       
        // GET: api/DemoModels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DemoModel>>> GetModelItem()
        {
            var message = Encoding.UTF8.GetBytes("hello, retry pattern");

            var retry = Policy
            .Handle<Exception>()
            .WaitAndRetry(2, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
            try
            {
                retry.Execute(() =>
                {
                    Console.WriteLine($"begin at RetryAttemp", retry);
                });
                //return Ok("retryAttempt");

                Console.WriteLine("hello retry count");

            }
            catch (Exception)
            {
                return Ok("Exception Generates");
            }

            return await _context.ModelItem.ToListAsync();





            //return await _context.ModelItem.ToListAsync();
        }

        // GET: api/DemoModels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DemoModel>> GetDemoModel(int id)
        {
            var demoModel = await _context.ModelItem.FindAsync(id);

            if (demoModel == null)
            {
                return NotFound();
            }

            return demoModel;
        }

        // PUT: api/DemoModels/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDemoModel(int id, DemoModel demoModel)
        {
            if (id != demoModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(demoModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DemoModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/DemoModels
        [HttpPost]
        public async Task<ActionResult<DemoModel>> PostDemoModel(DemoModel demoModel)
        {
            _context.ModelItem.Add(demoModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDemoModel", new { id = demoModel.Id }, demoModel);
        }

        // DELETE: api/DemoModels/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<DemoModel>> DeleteDemoModel(int id)
        {
            var demoModel = await _context.ModelItem.FindAsync(id);
            if (demoModel == null)
            {
                return NotFound();
            }

            _context.ModelItem.Remove(demoModel);
            await _context.SaveChangesAsync();

            return demoModel;
        }

        private bool DemoModelExists(int id)
        {
            return _context.ModelItem.Any(e => e.Id == id);
        }
    }
}
