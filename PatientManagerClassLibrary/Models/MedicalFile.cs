using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientManagerClassLibrary.Models
{
    public class MedicalFile
    {
        [Column("id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column("object_id")]
        public string ObjectId { get; set; }

        [Column("checkup_id")]
        public long CheckUpId { get; set; }

        public virtual CheckUp CheckUp { get; set; }
    }
}
