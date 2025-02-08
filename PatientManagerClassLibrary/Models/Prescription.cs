using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientManagerClassLibrary.Models
{
    [Table("perscription")]
    public class Prescription
    {
        [Column("id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column("medication")]
        public string Medication { get; set; }

        [Column("dosage")]
        public string Dosage { get; set; }

        [Column("start_date")]
        public DateTime StartDate { get; set; }

        [Column("end_date")]
        public DateTime EndDate { get; set; }

        [Column("patient_id")]
        public long PatientId { get; set; }

        public Patient Patient { get; set; }
    }
}
