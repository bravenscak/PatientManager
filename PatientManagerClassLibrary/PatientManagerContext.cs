using Microsoft.EntityFrameworkCore;
using PatientManagerClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientManagerClassLibrary
{
    internal class PatientManagerContext : DbContext
    {
        public const string CONNECTION_STRING = @"
            Host=absently-cerebral-tody.data-1.euc1.tembo.io;
            Port=5432;
            Username=postgres;
            Password=tdLhCLTTl1p7E1eg;
            Database=postgres
        ";

        public DbSet<Patient> Patients { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<CheckUp> CheckUps { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(CONNECTION_STRING);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
