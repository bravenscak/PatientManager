using PatientManagerClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientManagerClassLibrary.Utilities
{
    public class CsvExporter
    {
        public string ExportPatientsToCsv(IEnumerable<Patient> patients)
        {
            var csv = new StringBuilder();
            csv.AppendLine("Id,FirstName,LastName,Sex,Oib,DateOfBirth,MedicalRecords,CheckUps,Perscriptions");
            foreach (var patient in patients)
            {
                var medicalRecords = string.Join(";", patient.MedicalRecords.Select(mr => $"{mr.Diagnosis} ({mr.StartDate.ToShortDateString()} - {mr.EndDate?.ToShortDateString() ?? "Present"})"));
                var checkUps = string.Join(";", patient.CheckUps.Select(cu => $"{cu.Type} ({cu.Date.ToShortDateString()})"));
                var prescriptions = string.Join(";", patient.Prescriptions.Select(p => $"{p.Medication} ({p.Dosage}, {p.StartDate.ToShortDateString()})"));

                csv.AppendLine($"{patient.Id},{patient.FirstName},{patient.LastName},{patient.Sex},{patient.Oib},{patient.DateOfBirth.ToShortDateString()},{medicalRecords},{checkUps},{prescriptions}");
            }

            return csv.ToString();
        }
    }
}
