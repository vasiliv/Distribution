using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Distribution.Data;
using Distribution.Models;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Distribution.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "Admin, User")]
    public class RecordController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;
        public RecordController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            webHostEnvironment = hostEnvironment;
        }

        // GET: User/Record
        public async Task<IActionResult> Index()
        {
            return View(await _context.Record.ToListAsync());
        }

        // GET: User/Record/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @record = await _context.Record
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@record == null)
            {
                return NotFound();
            }

            return View(@record);
        }

        // GET: User/Record/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: User/Record/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,SecondName,PiradiNomeri,ContractPicture")] Record @record)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = ProcessUploadedFile(@record);
                @record.ContractPictureName = uniqueFileName;
                

                _context.Add(@record);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@record);
        }

        // GET: User/Record/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @record = await _context.Record.FindAsync(id);
            if (@record == null)
            {
                return NotFound();
            }
            return View(@record);
        }

        // POST: User/Record/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,SecondName,PiradiNomeri")] Record @record)
        {
            if (id != @record.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@record);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecordExists(@record.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(@record);
        }

        // GET: User/Record/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @record = await _context.Record
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@record == null)
            {
                return NotFound();
            }

            return View(@record);
        }

        // POST: User/Record/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @record = await _context.Record.FindAsync(id);
            _context.Record.Remove(@record);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecordExists(int id)
        {
            return _context.Record.Any(e => e.Id == id);
        }
        private string ProcessUploadedFile(Record @record)
        {
            string uniqueFileName = null;

            if (@record.ContractPicture != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "Uploads");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + @record.ContractPicture.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    @record.ContractPicture.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
    }
}
