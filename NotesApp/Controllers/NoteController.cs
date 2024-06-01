using Microsoft.AspNetCore.Mvc;
using NotesApp.Core;
using NotesApp.Core.Interfaces;
using NotesApp.ViewModels.Note;
using System.Security.Claims;

namespace NotesApp.Controllers
{
    public class NoteController : Controller
    {
        private readonly INoteService _noteService;
        public NoteController(INoteService service)
        {
            _noteService = service;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(NoteFormModel model)
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            await this._noteService.AddAsync(model,userId);
            return View("Index");
        }
    }
}
