﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientManagerClassLibrary.Models
{
    [Table("patient")]
    public class Patient
    {
        [Column("id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column("first_name")]
        public string FirstName { get; set; }

        [Column("last_name")]
        public string LastName { get; set; }

        [Column("sex")]
        public char Sex { get; set; }

        [Column("oib")]
        public string Oib { get; set; }

        [Column("date_of_birth")]
        public DateTime DateOfBirth { get; set; }
    }
}
