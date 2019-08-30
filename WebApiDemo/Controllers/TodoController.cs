using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiDemo.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        //created by jagdish parmar
        private readonly TodoContext _Context;
        public TodoController(TodoContext Context)
        {
            _Context = Context;
            if (_Context.TodoItems.Count() == 0)
            {
                _Context.TodoItems.Add(new TodoItem { Name = "jagdish parmar" });
                _Context.SaveChanges();
            }
        }
        // GET: api/<controller>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
            // return await _Context.TodoItems.ToList
            return await _Context.TodoItems.ToListAsync();
        }

        // GET api/<controller>/5   
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
        {
            var todoitem = await _Context.TodoItems.FindAsync(id);
            if (todoitem == null)
            {
                return NotFound();
            }
            return todoitem;
            
        }


        
        // POST api/<controller>
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem item)
        {

            //adddition in inmemmory
            _Context.TodoItems.Add(item);
            await _Context.SaveChangesAsync();

            // update id in GetToDoItem
            // return the status code
            return CreatedAtAction(nameof(GetTodoItem), new { id = item.Id }, item);
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]

        public async Task<IActionResult> PutTodoItem(long id, TodoItem item)
        {

            // if id not found the return the badrequest
            if (id != item.Id)
            {
                return BadRequest();
            }
            //update entry in Inmememroy database
            _Context.Entry(item).State = EntityState.Modified;
            await _Context.SaveChangesAsync();

            // return no Content status code 204
            return NoContent();
        }
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            var todoitem = await _Context.TodoItems.FindAsync(id);
            if (todoitem == null)
            {
                return NotFound();
            }
            _Context.TodoItems.Remove(todoitem);
            await _Context.SaveChangesAsync();

            return NoContent();

        }
        //public void Delete(int id)
        //{
        //}
    }
}
