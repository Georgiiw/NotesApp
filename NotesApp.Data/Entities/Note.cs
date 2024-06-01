using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp.Data.Entities
{
    public class Note
    {
        public int Id { get; set; }
        public Guid CreatorId { get; set; }
        public required string Content { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
