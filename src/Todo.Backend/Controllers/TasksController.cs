using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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
            return Ok(_db.Task.Include(x => x.Children).Where(x => x.Parent == null).ToList());
        }

        // Add new todo task
        [HttpPost]
        public async Task<IActionResult> Post(TodoTask task)
        {
            try
            {
                if (task == null)
                {
                    return BadRequest("Task was empty.");
                }

                task.CreatedAt = DateTime.Now;

                var result = _db.Task.Add(task);
                await _db.SaveChangesAsync();

                // Return result and location of the newly created task
                return CreatedAtAction(nameof(GetTask), new { id = result.Entity.Id }, result.Entity);
            }
            catch (Exception e)
            {
                
                _logger.LogError(e, "Failed to add new task");
                return Problem("Failed to add new task", JsonConvert.SerializeObject(task), StatusCodes.Status500InternalServerError);
            }
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                var task = new TodoTask { Id = id };
                _db.Task.Attach(task);
                var result = _db.Task.Remove(task);
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return NotFound($"Task with id {id} was not found.");
            }
            return Ok();
        }
    }
}
