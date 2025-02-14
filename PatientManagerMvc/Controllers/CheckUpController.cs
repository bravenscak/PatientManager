using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PatientManagerClassLibrary;
using PatientManagerClassLibrary.Dictionaries;
using PatientManagerClassLibrary.Enums;
using PatientManagerClassLibrary.Models;
using PatientManagerMvc.Models;

namespace PatientManagerMvc.Controllers
{
    public class CheckUpController : Controller
    {
        private readonly PatientManagerContext _context;

        public CheckUpController(PatientManagerContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var checkUps = await _context.CheckUps.ToListAsync();
            return View(checkUps);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var checkUp = await _context.CheckUps
                .Include(c => c.Patient)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (checkUp == null)
            {
                return NotFound();
            }

            return View(checkUp);
        }


        public IActionResult Create(long? patientId)
        {
            var viewModel = new CheckUpViewModel
            {
                Date = DateTime.Now,
                Patients = _context.Patients.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = $"{p.FirstName} {p.LastName}"
                }).ToList()
            };

            if (patientId != null)
            {
                viewModel.PatientId = patientId.Value;
            }

            ViewBag.CheckUpTypes = CheckUpTypeDictionary.Descriptions;
            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CheckUpViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var checkUp = new CheckUp
                {
                    Date = viewModel.Date,
                    Type = viewModel.Type.ToString(),
                    PatientId = viewModel.PatientId
                };

                _context.CheckUps.Add(checkUp);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.CheckUpTypes = CheckUpTypeDictionary.Descriptions;
            return View(viewModel);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var checkUp = await _context.CheckUps.FindAsync(id);
            if (checkUp == null)
            {
                return NotFound();
            }

            var viewModel = new CheckUpViewModel
            {
                Id = checkUp.Id,
                Date = checkUp.Date,
                Type = Enum.Parse<CheckUpType>(checkUp.Type),
                PatientId = checkUp.PatientId
            };

            ViewBag.CheckUpTypes = CheckUpTypeDictionary.Descriptions;
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CheckUpViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var checkUp = await _context.CheckUps.FindAsync(id);
                    if (checkUp == null)
                    {
                        return NotFound();
                    }

                    viewModel.Date = DateTime.SpecifyKind(viewModel.Date, DateTimeKind.Utc);

                    checkUp.Date = viewModel.Date;
                    checkUp.Type = viewModel.Type.ToString();
                    checkUp.PatientId = viewModel.PatientId;

                    _context.Update(checkUp);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CheckUpExists(viewModel.Id))
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

            ViewBag.CheckUpTypes = CheckUpTypeDictionary.Descriptions;
            return View(viewModel);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var checkUp = await _context.CheckUps.FirstOrDefaultAsync(c => c.Id == id);
            if (checkUp == null)
            {
                return NotFound();
            }

            return View(checkUp);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmation(int? id)
        {
            var checkUp = await _context.CheckUps.FindAsync(id);
            if (checkUp == null)
            {
                return NotFound();
            }

            _context.CheckUps.Remove(checkUp);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CheckUpExists(long id)
        {
            return _context.CheckUps.Any(c => c.Id == id);
        }
    }
}
