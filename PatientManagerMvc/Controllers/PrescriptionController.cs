using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatientManagerClassLibrary;
using PatientManagerClassLibrary.Models;
using PatientManagerMvc.Models;

namespace PatientManagerMvc.Controllers
{
    public class PrescriptionController : Controller
    {
        private readonly PatientManagerContext _context;

        public PrescriptionController(PatientManagerContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var perscriptions = await _context.Prescriptions.ToListAsync();
            return View(perscriptions);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prescription = await _context.Prescriptions.FirstOrDefaultAsync(p => p.Id == id);

            if (prescription == null)
            {
                return NotFound();
            }
            return View(prescription);
        }

        public IActionResult Create(long? patientId)
        {
            var viewModel = new PrescriptionViewModel
            {
                StartDate = DateTime.Now
            };

            if (patientId != null)
            {
                viewModel.PatientId = patientId.Value;
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PrescriptionViewModel prescription)
        {
            if (ModelState.IsValid)
            {
                var newPrescription = new Prescription
                {
                    Medication = prescription.Medication,
                    Dosage = prescription.Dosage,
                    StartDate = prescription.StartDate,
                    PatientId = prescription.PatientId
                };

                _context.Prescriptions.Add(newPrescription);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(prescription);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prescription = await _context.Prescriptions.FirstOrDefaultAsync(p => p.Id == id);
            if (prescription == null)
            {
                return NotFound();
            }

            var viewModel = new PrescriptionViewModel
            {
                Id = prescription.Id,
                Medication = prescription.Medication,
                Dosage = prescription.Dosage,
                StartDate = prescription.StartDate,
                PatientId = prescription.PatientId
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PrescriptionViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var prescription = await _context.Prescriptions.FindAsync(id);
                    if (prescription == null)
                    {
                        return NotFound();
                    }

                    prescription.Medication = viewModel.Medication;
                    prescription.Dosage = viewModel.Dosage;
                    prescription.StartDate = viewModel.StartDate;
                    prescription.PatientId = viewModel.PatientId;

                    _context.Update(prescription);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrescriptionExists(viewModel.Id))
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
            return View(viewModel);
        }

        public async Task<IActionResult> Delete (int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var prescription = await _context.Prescriptions.FirstOrDefaultAsync(p => p.Id == id);
            if (prescription == null)
            {
                return NotFound();
            }
            return View(prescription);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmation(int? id)
        {
            var prescrition = await _context.Prescriptions.FindAsync(id);
            if (prescrition == null)
            {
                return NotFound();
            }

            _context.Prescriptions.Remove(prescrition);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool PrescriptionExists(long id)
        {
            return _context.Prescriptions.Any(p => p.Id == id);
        }
    }
}
