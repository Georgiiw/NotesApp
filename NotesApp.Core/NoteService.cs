﻿using Microsoft.EntityFrameworkCore;
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
            try
            {
                Note note = new Note
                {
                    Title = model.Title,
                    Content = model.Content,
                    CreatedAt = DateTime.Now,
                    CreatorId = userId,

                };
                await this._db.AddAsync(note);
                await this._db.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new ApplicationException("An unexpected error occurred while adding new note.");
            }
           
        }

        public async Task DeleteAsync(int noteId)
        {
            try
            {
                Note note = await this._db.Notes.FirstOrDefaultAsync(n => n.Id == noteId);

                this._db.Notes.Remove(note);
                await this._db.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new ApplicationException("An unexpected error occurred while deleting your note.");
            }
            
        }

        public async Task EditAsync(NoteViewModel model)
        {
            try
            {
                Note noteToEdit = await this._db.Notes
                .FirstOrDefaultAsync(n => n.Id == model.Id);

                noteToEdit.Title = model.Title;
                noteToEdit.Content = model.Content;
                noteToEdit.LastUpdatedAt = DateTime.Now;

                await this._db.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new ApplicationException("An unexpected error occurred while editing your note.");
            }
            
        }

        public async Task<IEnumerable<NoteViewModel>> GetAllAsync(Guid userId)
        {
            IEnumerable<NoteViewModel> notes = await this._db.Notes
                .Where(n => n.CreatorId == userId)
                .Select(n => new NoteViewModel 
                {
                    Id = n.Id,
                    Title = n.Title,
                    Content = n.Content,
                    CreatedAt = n.CreatedAt,
                    LastUpdatedAt = n.LastUpdatedAt,
                }).ToListAsync();

            return notes;
        }

        public async Task<string> GetNoteCreatorId(int noteId)
        {
            Note note = await this._db.Notes.FirstOrDefaultAsync(n => n.Id == noteId);
            string creatorId = note.CreatorId.ToString();
            return creatorId;
        }
    }
}
