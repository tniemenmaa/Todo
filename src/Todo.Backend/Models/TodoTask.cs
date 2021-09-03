using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Todo.Backend.Models
{
    public class TodoTask
    {
        public int Id { get; set; }

        [MaxLength(128)]
        public string Summary { get; set; }

        [MaxLength(1024)]
        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? DeadlineAt { get; set; }

        public int Priority { get; set; }

        public TodoTaskStatus Status { get; set; }

        public int? ParentId { get; set; }

        /// <summary>
        /// Navigational property for parent task
        /// </summary>
        [JsonIgnore]
        public TodoTask Parent { get; set; }

        /// <summary>
        /// Navigational property for child tasks
        /// </summary>
        public ICollection<TodoTask> Children { get; set; }


    }
}
