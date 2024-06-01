using Microsoft.AspNetCore.Identity;

namespace NotesApp.Data.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public ApplicationUser()
        {
            Id = Guid.NewGuid();
        }
    }
}
