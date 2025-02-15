using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PatientManagerClassLibrary;
using PatientManagerClassLibrary.Models;
using PatientManagerClassLibrary.Services.Interfaces;
using PatientManagerMvc.Models;

namespace PatientManagerMvc.Controllers
{
    public class MedicalFileController : Controller
    {
        private readonly PatientManagerContext _context;
        private readonly IMinioService _minioService;

        public MedicalFileController(PatientManagerContext context, IMinioService minioService)
        {
            _context = context;
            _minioService = minioService;
        }

        public async Task<IActionResult> Index()
        {
            var medicalFiles = await _context.MedicalFiles
                .Include(m => m.CheckUp)
                .ThenInclude(c => c.Patient)
                .Select(m => new MedicalFileViewModel
                {
                    Id = m.Id,
                    ObjectId = m.ObjectId,
                    CheckUpId = m.CheckUpId,
                    CheckUps = new List<SelectListItem>
                    {
                new SelectListItem
                {
                    Value = m.CheckUp.Id.ToString(),
                    Text = $"{m.CheckUp.Patient.FirstName} {m.CheckUp.Patient.LastName} - {m.CheckUp.Type} - {m.CheckUp.Date.ToShortDateString()}"
                }
                    }
                }).ToListAsync();

            return View(medicalFiles);
        }


        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicalFile = await _context.MedicalFiles
                .Include(m => m.CheckUp)
                .ThenInclude(c => c.Patient)
                .Select(m => new MedicalFileViewModel
                {
                    Id = m.Id,
                    ObjectId = m.ObjectId,
                    CheckUpId = m.CheckUpId,
                    CheckUps = new List<SelectListItem>
                    {
                new SelectListItem
                {
                    Value = m.CheckUp.Id.ToString(),
                    Text = $"{m.CheckUp.Patient.FirstName} {m.CheckUp.Patient.LastName} - {m.CheckUp.Type} - {m.CheckUp.Date.ToShortDateString()}"
                }
                    }
                })
                .FirstOrDefaultAsync(m => m.Id == id);

            if (medicalFile == null)
            {
                return NotFound();
            }

            return View(medicalFile);
        }


        [HttpPost]
        [Route("MedicalFile/UploadFile")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(extension))
            {
                return BadRequest("Invalid file type. Only image files are allowed.");
            }

            string objectId;
            using (var stream = file.OpenReadStream())
            {
                objectId = await _minioService.PutObject(stream, file.FileName, file.ContentType, file.Length);
            }

            return Json(new { objectId, fileName = file.FileName, size = file.Length });
        }

        [HttpGet]
        [Route("MedicalFile/DownloadFile/{id}")]
        public async Task<IActionResult> DownloadFile(long id)
        {
            var medicalFile = await _context.MedicalFiles.FindAsync(id);
            if (medicalFile == null)
            {
                return NotFound();
            }

            var minioObjectResponse = await _minioService.GetObject(medicalFile.ObjectId);
            if (minioObjectResponse == null)
            {
                return NotFound();
            }

            return File(minioObjectResponse.Data, minioObjectResponse.ContentType, Path.GetFileName(medicalFile.ObjectId));
        }

        public async Task<IActionResult> Create()
        {
            var checkUps = await _context.CheckUps
                .Include(c => c.Patient)
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = $"{c.Patient.FirstName} {c.Patient.LastName} - {c.Type} - {c.Date.ToShortDateString()}"
                }).ToListAsync();

            var viewModel = new MedicalFileViewModel
            {
                CheckUps = checkUps
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CheckUpId,ObjectId")] MedicalFileViewModel medicalFile)
        {
            if (ModelState.IsValid)
            {
                var newMedicalFile = new MedicalFile
                {
                    ObjectId = medicalFile.ObjectId,
                    CheckUpId = medicalFile.CheckUpId
                };

                _context.Add(newMedicalFile);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            var checkUps = await _context.CheckUps
                .Include(c => c.Patient)
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = $"{c.Patient.FirstName} {c.Patient.LastName} - {c.Date.ToShortDateString()}"
                }).ToListAsync();

            medicalFile.CheckUps = checkUps;

            return View(medicalFile);
        }

        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicalFile = await _context.MedicalFiles
                .Include(m => m.CheckUp)
                .ThenInclude(c => c.Patient)
                .Select(m => new MedicalFileViewModel
                {
                    Id = m.Id,
                    ObjectId = m.ObjectId,
                    CheckUpId = m.CheckUpId
                })
                .FirstOrDefaultAsync(m => m.Id == id);

            if (medicalFile == null)
            {
                return NotFound();
            }

            var checkUps = await _context.CheckUps
                .Include(c => c.Patient)
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = $"{c.Patient.FirstName} {c.Patient.LastName} - {c.Type} - {c.Date.ToShortDateString()}"
                }).ToListAsync();

            medicalFile.CheckUps = checkUps;

            return View(medicalFile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,CheckUpId,ObjectId")] MedicalFileViewModel medicalFile, IFormFile newFile)
        {
            if (id != medicalFile.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingMedicalFile = await _context.MedicalFiles.FindAsync(id);
                    if (existingMedicalFile == null)
                    {
                        return NotFound();
                    }

                    if (newFile != null && newFile.Length > 0)
                    {
                        await _minioService.DeleteObject(existingMedicalFile.ObjectId);

                        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                        var extension = Path.GetExtension(newFile.FileName).ToLowerInvariant();
                        if (!allowedExtensions.Contains(extension))
                        {
                            return BadRequest("Invalid file type.");
                        }

                        string newObjectId;
                        using (var stream = newFile.OpenReadStream())
                        {
                            newObjectId = await _minioService.PutObject(stream, newFile.FileName, newFile.ContentType, newFile.Length);
                        }

                        existingMedicalFile.ObjectId = newObjectId;
                    }

                    existingMedicalFile.CheckUpId = medicalFile.CheckUpId;

                    _context.Update(existingMedicalFile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicalFileExists(medicalFile.Id))
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

            var checkUps = await _context.CheckUps
                .Include(c => c.Patient)
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = $"{c.Patient.FirstName} {c.Patient.LastName} - {c.Type} - {c.Date.ToShortDateString()}"
                }).ToListAsync();

            medicalFile.CheckUps = checkUps;

            return View(medicalFile);
        }

        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicalFile = await _context.MedicalFiles
                .Include(m => m.CheckUp)
                .ThenInclude(c => c.Patient)
                .Select(m => new MedicalFileViewModel
                {
                    Id = m.Id,
                    ObjectId = m.ObjectId,
                    CheckUpId = m.CheckUpId,
                    CheckUps = new List<SelectListItem>
                    {
                new SelectListItem
                {
                    Value = m.CheckUp.Id.ToString(),
                    Text = $"{m.CheckUp.Patient.FirstName} {m.CheckUp.Patient.LastName} - {m.CheckUp.Type} - {m.CheckUp.Date.ToShortDateString()}"
                }
                    }
                })
                .FirstOrDefaultAsync(m => m.Id == id);

            if (medicalFile == null)
            {
                return NotFound();
            }

            return View(medicalFile);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var medicalFile = await _context.MedicalFiles.FindAsync(id);
            if (medicalFile != null)
            {
                await _minioService.DeleteObject(medicalFile.ObjectId);

                _context.MedicalFiles.Remove(medicalFile);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Route("MedicalFile/GetImage/{id}")]
        public async Task<IActionResult> GetImage(long id)
        {
            var medicalFile = await _context.MedicalFiles.FindAsync(id);
            if (medicalFile == null)
            {
                return NotFound();
            }

            var minioObjectResponse = await _minioService.GetObject(medicalFile.ObjectId);
            if (minioObjectResponse == null)
            {
                return NotFound();
            }

            return File(minioObjectResponse.Data, minioObjectResponse.ContentType);
        }

        private bool MedicalFileExists(long id)
        {
            return _context.MedicalFiles.Any(e => e.Id == id);
        }
    }
}
