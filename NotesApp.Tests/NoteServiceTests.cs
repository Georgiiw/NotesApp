using Microsoft.EntityFrameworkCore;
using NotesApp.Core;
using NotesApp.Data;
using NotesApp.Data.Entities;
using NotesApp.ViewModels.Note;


namespace NotesApp.Tests
{
    [TestFixture]
    public class NoteServiceTests
    {
        private ApplicationDbContext context;
        private NoteService noteService;
        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            context = new ApplicationDbContext(options);
            context.Notes.Add(new Note
            {
                Id = 1,
                Title = "Default",
                Content = "Default test",
                CreatorId = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
            });
            context.SaveChanges();

            noteService = new NoteService(context);
        }
        [TearDown]
        public void TearDown()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
        [Test]
        public async Task AddNoteToDbTest()
        {
            NoteFormModel model = new NoteFormModel
            {
                Title = "Title Test",
                Content = "Testing test",
                CreatorId = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
            };

            var userId = Guid.NewGuid();

            await noteService.AddAsync(model, userId);
            var addedModel = context.Notes.Find(2);

            Assert.NotNull(addedModel);
            Assert.That(addedModel.Title, Is.EqualTo("Title Test"));
        }
        [Test]
        public async Task EditNoteTest()
        {
            NoteViewModel edited = new NoteViewModel
            {
                Id = 1,
                Title = "Edited",
                Content = "Edited Test",
                LastUpdatedAt = DateTime.UtcNow,
            };

            var editedModel = context.Notes.Find(1);

            await noteService.EditAsync(edited);

            Assert.NotNull(editedModel);
            Assert.That(editedModel.Title, Is.EqualTo("Edited"));
        }
        [Test]
        public async Task DeleteNoteTest()
        {
            await noteService.DeleteAsync(1);

            var deletedModel = await context.Notes.FindAsync(1);

            Assert.Null(deletedModel);
        }
    }
}
