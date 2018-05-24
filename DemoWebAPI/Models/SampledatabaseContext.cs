using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DemoWebAPI.Models
{
    public partial class SampledatabaseContext : DbContext
    {
        public virtual DbSet<Bank> Bank { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(@"Data Source=sampleserver-aj.database.windows.net;Initial Catalog=SampleDatabase;User ID=testuser;Password=Azure@1234");
        //}
        public SampledatabaseContext(DbContextOptions<SampledatabaseContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bank>(entity =>
            {
                entity.Property(e => e.BankId)
                    .HasColumnName("BankID");

                entity.Property(e => e.BankName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.EmpId).HasColumnName("EmpID");

                entity.HasOne(d => d.Emp)
                    .WithMany(p => p.Bank)
                    .HasForeignKey(d => d.EmpId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Bank__EmpID__2FCF1A8A");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.EmpId);

                entity.Property(e => e.EmpId)
                    .HasColumnName("EmpID");

                entity.Property(e => e.Address)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.HasSequence<int>("SalesOrderNumber");
        }
    }
}
