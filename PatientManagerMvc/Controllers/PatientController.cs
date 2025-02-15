using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatientManagerClassLibrary;
using PatientManagerClassLibrary.Dictionaries;
using PatientManagerClassLibrary.Enums;
using PatientManagerClassLibrary.Models;
using PatientManagerClassLibrary.Utilities;
using PatientManagerMvc.Models;

namespace PatientManagerMvc.Controllers
{
    public class PatientController : Controller
    {
        private readonly PatientManagerContext _context;
        private readonly CsvExporter _csvExporter;

        public PatientController(PatientManagerContext context, CsvExporter csvExporter)
        {
            _context = context;
            _csvExporter = csvExporter;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            var patients = from p in _context.Patients
                           select p;

            if (!String.IsNullOrEmpty(searchString))
            {
                patients = patients.Where(p => p.LastName.Contains(searchString) || p.Oib.Contains(searchString));
            }

            var patientList = await patients.ToListAsync();

            var patientViewModel = patientList
                .Select(p => new PatientViewModel
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Sex = p.Sex,
                    Oib = p.Oib,
                    DateOfBirth = p.DateOfBirth
                }).ToList();

            return View(patientViewModel);
        }



        public IActionResult Details(long id)
        {
            var patient = _context.Patients
                .Where(p => p.Id == id)
                .Select(p => new PatientViewModel
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Sex = p.Sex,
                    Oib = p.Oib,
                    DateOfBirth = p.DateOfBirth,
                    MedicalRecords = p.MedicalRecords.Select(mr => new MedicalRecordViewModel
                    {
                        Diagnosis = mr.Diagnosis,
                        StartDate = mr.StartDate,
                        EndDate = mr.EndDate
                    }).ToList(),
                    Prescriptions = p.Prescriptions.Select(p => new PrescriptionViewModel
                    {
                        Medication = p.Medication,
                        Dosage = p.Dosage,
                        StartDate = p.StartDate
                    }).ToList(),
                    CheckUps = p.CheckUps.Select(cu => new CheckUpViewModel
                    {
                        Type = Enum.Parse<CheckUpType>(cu.Type),
                        Date = cu.Date
                    }).ToList()
                }).FirstOrDefault();
            if (patient == null)
            {
                return NotFound();
            }

            ViewBag.CheckUpTypes = CheckUpTypeDictionary.Descriptions;
            return View(patient);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PatientViewModel patient)
        {
            if (ModelState.IsValid)
            {
                var newPatient = new Patient
                {
                    FirstName = patient.FirstName,
                    LastName = patient.LastName,
                    Sex = patient.Sex,
                    Oib = patient.Oib,
                    DateOfBirth = DateTime.SpecifyKind(patient.DateOfBirth, DateTimeKind.Utc)
                };

                _context.Patients.Add(newPatient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(patient);
        }

        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .Select(p => new PatientViewModel
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Sex = p.Sex,
                    Oib = p.Oib,
                    DateOfBirth = p.DateOfBirth
                }).FirstOrDefaultAsync(p => p.Id == id);

            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, PatientViewModel patient)
        {
            if (id != patient.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingPatient = await _context.Patients.FindAsync(id);
                    if (existingPatient == null)
                    {
                        return NotFound();
                    }

                    existingPatient.FirstName = patient.FirstName;
                    existingPatient.LastName = patient.LastName;
                    existingPatient.Sex = patient.Sex;
                    existingPatient.Oib = patient.Oib;
                    existingPatient.DateOfBirth = DateTime.SpecifyKind(patient.DateOfBirth, DateTimeKind.Utc);

                    _context.Update(existingPatient);
                    await _context.SaveChangesAsync();
                }
                catch
                {
                    if (!PatientExists(patient.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(patient);
        }

        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .Select(p => new PatientViewModel
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Sex = p.Sex,
                    Oib = p.Oib,
                    DateOfBirth = p.DateOfBirth
                }).FirstOrDefaultAsync(p => p.Id == id);

            if (patient == null)
            {
                return NotFound();
            }
            return View(patient);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmation(long? id)
        {
            var patient = await _context.Patients.FindAsync(id);

            if (patient == null)
            {
                return NotFound();
            }

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatientExists(long id)
        {
            return _context.Patients.Any(p => p.Id == id);
        }

        public async Task<IActionResult> ExportToCsv(long id)
        {
            var patients = await _context.Patients
                .Include(p=>p.MedicalRecords)
                .Include(p=>p.CheckUps)
                .Include(p => p.Prescriptions)
                .FirstOrDefaultAsync(p=> p.Id==id);

            if (patients == null)
            {
                return NotFound();
            }

            var csv = _csvExporter.ExportPatientsToCsv(new List<Patient> { patients });
            var bytes = System.Text.Encoding.UTF8.GetBytes(csv);
            return File(bytes, "text/csv", $"Patient-{patients.LastName}-{patients.LastName}.csv");
        }
    }
}
