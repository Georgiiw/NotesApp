using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotesApp.Core.Interfaces;
using NotesApp.ViewModels.Note;
using System.Security.Claims;

namespace NotesApp.Controllers
{
    [Authorize]
    public class NoteController : Controller
    {
        private readonly INoteService _noteService;
        public NoteController(INoteService service)
        {
            _noteService = service;
        }
        public async Task<IActionResult> Index()
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            IEnumerable<NoteViewModel> notes = await this._noteService.GetAllAsync(userId);

            return View( notes);
        }
        [HttpPost]
        public async Task<IActionResult> Add(NoteFormModel model)
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            await this._noteService.AddAsync(model,userId);
            return RedirectToAction("Index", "Note");
        }
        [HttpPost] 
        public async Task<IActionResult> Edit(NoteViewModel model)
        {
            await this._noteService.EditAsync(model);
            return RedirectToAction("Index", "Note");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var creatorId = this._noteService.GetNoteCreatorId(id).Result;

            if (creatorId != userId)
            {
                return Forbid();
            }
            await this._noteService.DeleteAsync(id);
            return RedirectToAction("Index", "Note");
        }
    }
}
