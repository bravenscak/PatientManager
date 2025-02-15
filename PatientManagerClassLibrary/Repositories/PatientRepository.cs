using Microsoft.EntityFrameworkCore;
using PatientManagerClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientManagerClassLibrary.Repositories
{
    public class PatientRepository : IRepository<Patient>
    {
        private readonly PatientManagerContext _context;

        public PatientRepository(PatientManagerContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Patient entity)
        {
            await _context.Patients.AddAsync(entity);
            await _context.SaveChangesAsync();  
        }

        public async Task DeleteAsync(long id)
        {
            var entity = await _context.Patients.FindAsync(id);
            if (entity != null)
            {
                _context.Patients.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Patient>> GetAllAsync()
        {
            return await _context.Patients.ToListAsync();
        }

        public async Task<Patient?> GetByIdAsync(long id)
        {
            return await _context.Patients
                .Include(p => p.MedicalRecords)
                .Include(p => p.CheckUps)
                .Include(p => p.Prescriptions)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task UpdateAsync(Patient entity)
        {
            _context.Patients.Update(entity);
           await _context.SaveChangesAsync();
        }
    }
}
