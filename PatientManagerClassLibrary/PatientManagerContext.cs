using Microsoft.EntityFrameworkCore;
using PatientManagerClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Proxies;

namespace PatientManagerClassLibrary
{
    public class PatientManagerContext : DbContext
    {
        public PatientManagerContext(DbContextOptions<PatientManagerContext> options) : base(options) { }

        public PatientManagerContext()
        {
        }

        public const string CONNECTION_STRING = @"
            Host=yourHost;
            Port=5432;
            Username=postgres;
            Password=yourPassword;
            Database=postgres
        ";

        public DbSet<Patient> Patients { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<CheckUp> CheckUps { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<MedicalFile> MedicalFiles { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(CONNECTION_STRING)
                              .UseLazyLoadingProxies();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CheckUp>()
                .HasOne(e => e.Patient)
                .WithMany(p => p.CheckUps)
                .HasForeignKey(e => e.PatientId);

            modelBuilder.Entity<MedicalRecord>()
                .HasOne(mr => mr.Patient)
                .WithMany(p => p.MedicalRecords)
                .HasForeignKey(mr => mr.PatientId);

            modelBuilder.Entity<Prescription>()
                .HasOne(p => p.Patient)
                .WithMany(p => p.Prescriptions)
                .HasForeignKey(p => p.PatientId);

            modelBuilder.Entity<MedicalFile>()
                .HasOne(mf => mf.CheckUp)
                .WithMany(e => e.MedicalFiles)
                .HasForeignKey(mf => mf.CheckUpId);
        }
    }
}
