using NotesApp.ViewModels.Note;


namespace NotesApp.Core.Interfaces
{
    public interface INoteService
    {
        Task AddAsync(NoteFormModel model, Guid userId);
        Task DeleteAsync(int noteId);
    }
}
