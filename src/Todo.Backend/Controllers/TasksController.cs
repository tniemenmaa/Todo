using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo.Backend.Data;
using Todo.Backend.Models;

namespace Todo.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly TodoDbContext _db;
        private readonly ILogger<TasksController> _logger;

        public TasksController(TodoDbContext db, ILogger<TasksController> logger)
        {
            _db = db;
            _logger = logger;
        }
        

        // Get all todo baseline tasks
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_db.Task.Where(x => x.Parent == null).ToList());
        }

        // Add new todo task
        [HttpPost]
        public async Task<IActionResult> Post(TodoTask task)
        {
            var result = _db.Task.Add(task);
            await _db.SaveChangesAsync();

            // Return result and location of the newly created task
            return CreatedAtAction(nameof(GetTask), new { id = result.Entity.Id }, result.Entity);
        }

        // Get todo task with id
        [HttpGet("{id}")]
        public IActionResult GetTask(int id)
        {
            var task = _db.Task.Include(x => x.Children).FirstOrDefault(x => x.Id == id);

            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }

        // Add or update task with id
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTask(int id, [FromBody] TodoTask task)
        {
            task.Id = id;
            var result = _db.Task.Update(task);
            await _db.SaveChangesAsync();
            return Ok(task);
        }

        // Partial update for task wtih id
        [HttpPatch("{id}")]
        public TodoTask PatchTask(int id, [FromBody] TodoTask task)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = new TodoTask { Id = id };
            _db.Task.Attach(task);
            _db.Task.Remove(task);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}
