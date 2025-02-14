﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatientManagerClassLibrary;
using PatientManagerClassLibrary.Models;
using PatientManagerMvc.Models;

namespace PatientManagerMvc.Controllers
{
    public class MedicalRecordController : Controller
    {
        private readonly PatientManagerContext _context;

        public MedicalRecordController(PatientManagerContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var medicalRecords = await _context.MedicalRecords.ToListAsync();
            return View(medicalRecords);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicalRecord = await _context.MedicalRecords.FirstOrDefaultAsync(m => m.Id == id);
            if (medicalRecord == null)
            {
                return NotFound();
            }

            return View(medicalRecord);
        }

        public IActionResult Create(int patientId)
        {
            var medicalRecord = new MedicalRecordViewModel
            {
                PatientId = patientId,
                StartDate = DateTime.Now
            };
            return View(medicalRecord);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MedicalRecordViewModel medicalRecord)
        {
            if (ModelState.IsValid)
            {
                var newMedicalRecord = new MedicalRecord
                {
                    Diagnosis = medicalRecord.Diagnosis,
                    StartDate = medicalRecord.StartDate,
                    EndDate = medicalRecord.EndDate,
                    PatientId = medicalRecord.PatientId
                };

                _context.MedicalRecords.Add(newMedicalRecord);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(medicalRecord);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicalRecord = await _context.MedicalRecords.FindAsync(id);
            if (medicalRecord == null)
            {
                return NotFound();
            }

            var medicalRecordViewModel = new MedicalRecordViewModel
            {
                Id = medicalRecord.Id,
                Diagnosis = medicalRecord.Diagnosis,
                StartDate = medicalRecord.StartDate,
                EndDate = medicalRecord.EndDate,
                PatientId = medicalRecord.PatientId
            };

            return View(medicalRecordViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MedicalRecordViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var medicalRecord = await _context.MedicalRecords.FindAsync(id);
                    if (medicalRecord == null)
                    {
                        return NotFound();
                    }

                    medicalRecord.Diagnosis = viewModel.Diagnosis;
                    medicalRecord.StartDate = viewModel.StartDate;
                    medicalRecord.EndDate = viewModel.EndDate;
                    medicalRecord.PatientId = viewModel.PatientId;

                    _context.Update(medicalRecord);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicalRecordExists(viewModel.Id))
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

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicalRecord = await _context.MedicalRecords.FirstOrDefaultAsync(mr => mr.Id == id);
            if (medicalRecord == null)
            {
                return NotFound();
            }

            return View(medicalRecord);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medicalRecord = await _context.MedicalRecords.FindAsync(id);
            if (medicalRecord == null)
            {
                return NotFound();
            }   

            _context.MedicalRecords.Remove(medicalRecord);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MedicalRecordExists(long id)
        {
            return _context.MedicalRecords.Any(mr => mr.Id == id);
        }
    }
}
