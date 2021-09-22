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
using Distribution.Areas.User.ViewModels;
using System.Security.Claims;

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
            var recordLoggedInUser = from r in _context.Record
                                     where r.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier)
                                     select r;
            return View(recordLoggedInUser);
            //return View(await _context.Record.ToListAsync());
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
        public async Task<IActionResult> Create(RecordViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = ProcessUploadedFile(model);
                Record @record = new Record
                {
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    FirstName = model.FirstName,
                    SecondName = model.SecondName,
                    PiradiNomeri = model.PiradiNomeri,
                    PhoneNumber = model.PhoneNumber,
                    ContractPictureName = uniqueFileName
                };  
                _context.Add(@record);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: User/Record/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @record = await _context.Record.FindAsync(id);
            //var @record = await _context.Record.Include(x => x.ContractPictureName)
            //    .FirstOrDefaultAsync(x => x.Id == id);
            var recordViewModel = new RecordViewModel()
            {
                Id = @record.Id,
                FirstName = @record.FirstName,
                SecondName = @record.SecondName,
                PiradiNomeri = @record.PiradiNomeri,
                PhoneNumber = @record.PhoneNumber,
                ExistingContractPicture = @record.ContractPictureName
            };
            if (@record == null)
            {
                return NotFound();
            }
            return View(recordViewModel);
        }

        // POST: User/Record/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RecordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var @record = await _context.Record.FindAsync(model.Id);
                @record.FirstName = model.FirstName;
                @record.SecondName = model.SecondName;
                @record.PiradiNomeri = model.PiradiNomeri;
                @record.PhoneNumber = model.PhoneNumber;

                if (model.ContractPicture != null)
                {
                    if (model.ExistingContractPicture != null)
                    {
                        string filePath = Path.Combine(webHostEnvironment.WebRootPath, "Uploads", model.ExistingContractPicture);
                        System.IO.File.Delete(filePath);
                    }
                    @record.ContractPictureName = ProcessUploadedFile(model);
                }                   
                _context.Update(@record);
                await _context.SaveChangesAsync();                
                return RedirectToAction(nameof(Index));
            }
            return View();
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

            var recordViewModel = new RecordViewModel()
            {
                Id = @record.Id,
                FirstName = @record.FirstName,
                SecondName = @record.SecondName,
                PiradiNomeri = @record.PiradiNomeri,
                PhoneNumber = @record.PhoneNumber,
                ExistingContractPicture = @record.ContractPictureName
            };
            if (@record == null)
            {
                return NotFound();
            }
            return View(recordViewModel);
        }

        // POST: User/Record/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @record = await _context.Record.FindAsync(id);
            var CurrentContractImage = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", @record.ContractPictureName);
            _context.Record.Remove(@record);
            if (await _context.SaveChangesAsync() > 0)
            {
                if (System.IO.File.Exists(CurrentContractImage))
                {
                    System.IO.File.Delete(CurrentContractImage);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        private bool RecordExists(int id)
        {
            return _context.Record.Any(e => e.Id == id);
        }
        private string ProcessUploadedFile(RecordViewModel model)
        {
            string uniqueFileName = null;

            if (model.ContractPicture != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "Uploads");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ContractPicture.FileName;
                //uniqueFileName = @record.Id + "_" + @record.ContractPicture.FileName  ;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.ContractPicture.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
    }
}
