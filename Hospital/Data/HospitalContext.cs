using Hospital.Models;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Data;

public class HospitalContext : DbContext
{
    public HospitalContext(DbContextOptions<HospitalContext> options) : base(options) { }

    public DbSet<Patient> Patients { get; set; }
    public DbSet<Doctor> Doctors { get; set; } 
    public DbSet<District> Districts { get; set; }
    public DbSet<Specialization> Specializations { get; set; }
    public DbSet<Cabinet> Cabinets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Doctor>(d =>
        {
            d.HasOne(x => x.Cabinet).WithMany().HasForeignKey(x => x.CabinetId);
            d.HasOne(x => x.Specialization).WithMany().HasForeignKey(x => x.SpecializationId);
            d.HasOne(x => x.District).WithMany().HasForeignKey(x => x.DistrictId);
        });

        modelBuilder.Entity<Patient>(p =>
        {
            p.HasOne(x => x.District).WithMany().HasForeignKey(x => x.DistrictId);
        });
    }
}