using Microsoft.EntityFrameworkCore;
using WebApplication4.Models;

namespace WebApplication4.Context;

using Microsoft.EntityFrameworkCore;
using System;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure primary key for Prescription entity
        modelBuilder.Entity<Prescription>()
            .HasKey(p => p.IdPrescription);

        modelBuilder.Entity<PrescriptionMedicament>()
            .HasKey(pm => new { pm.IdMedicament, pm.IdPrescription });

        modelBuilder.Entity<PrescriptionMedicament>()
            .HasOne(pm => pm.Medicament)
            .WithMany(m => m.PrescriptionMedicaments)
            .HasForeignKey(pm => pm.IdMedicament);

        modelBuilder.Entity<PrescriptionMedicament>()
            .HasOne(pm => pm.Prescription)
            .WithMany(p => p.PrescriptionMedicaments)
            .HasForeignKey(pm => pm.IdPrescription);

        modelBuilder.Entity<Prescription>()
            .HasOne(p => p.Doctor)
            .WithMany(d => d.Prescriptions)
            .HasForeignKey(p => p.IdDoctor);

        modelBuilder.Entity<Prescription>()
            .HasOne(p => p.Patient)
            .WithMany(pt => pt.Prescriptions)
            .HasForeignKey(p => p.IdPatient);

        // Seed data
        modelBuilder.Entity<Doctor>().HasData(
            new Doctor { IdDoctor = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" },
            new Doctor { IdDoctor = 2, FirstName = "Jane", LastName = "Smith", Email = "jane.smith@example.com" }
        );

        modelBuilder.Entity<Patient>().HasData(
            new Patient { IdPatient = 1, FirstName = "Alice", LastName = "Brown", Birthdate = new DateTime(1980, 1, 1) },
            new Patient { IdPatient = 2, FirstName = "Bob", LastName = "Johnson", Birthdate = new DateTime(1990, 2, 2) }
        );

        modelBuilder.Entity<Medicament>().HasData(
            new Medicament { IdMedicament = 1, Name = "Med1", Description = "Desc1", Type = "Type1" },
            new Medicament { IdMedicament = 2, Name = "Med2", Description = "Desc2", Type = "Type2" }
        );

        modelBuilder.Entity<Prescription>().HasData(
            new Prescription { IdPrescription = 1, Date = DateTime.Now, DueDate = DateTime.Now.AddDays(10), IdPatient = 1, IdDoctor = 1 },
            new Prescription { IdPrescription = 2, Date = DateTime.Now, DueDate = DateTime.Now.AddDays(5), IdPatient = 2, IdDoctor = 2 }
        );

        modelBuilder.Entity<PrescriptionMedicament>().HasData(
            new PrescriptionMedicament { IdMedicament = 1, IdPrescription = 1, Dose = 1, Details = "Take once a day" },
            new PrescriptionMedicament { IdMedicament = 2, IdPrescription = 2, Dose = 2, Details = "Take twice a day" }
        );
    }
}