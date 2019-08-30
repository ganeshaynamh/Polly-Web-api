using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiDemo.Models
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options): base(options)
        {
            //created by jagdish parmar
        }
        public DbSet<TodoItem> TodoItems { get; set; }
    }
}
