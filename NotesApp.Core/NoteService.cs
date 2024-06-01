using NotesApp.Core.Interfaces;
using NotesApp.Data;
using NotesApp.Data.Entities;
using NotesApp.ViewModels.Note;

namespace NotesApp.Core
{
    public class NoteService : INoteService
    {
        private readonly ApplicationDbContext _db;
        public NoteService(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }
        public async Task AddAsync(NoteFormModel model, Guid userId)
        {
            Note note = new Note
            {
                Content = model.Content,
                CreatedAt = DateTime.Now,
                CreatorId = userId,

            };
            await this._db.AddAsync(note);
            await this._db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int noteId)
        {
            Note note = await this._db.Notes.FindAsync(noteId);

            this._db.Notes.Remove(note);
            await this._db.SaveChangesAsync();
        }
    }
}
