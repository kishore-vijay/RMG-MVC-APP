using Microsoft.EntityFrameworkCore;
using ResourceManageGroup.Models;
namespace ResourceManageGroup.Data
{
    public class ApplicationDbContext:DbContext{
        public virtual DbSet<Recruiter> ? RecruiterDetails {get;set;}
        public virtual DbSet<Manager> ? ManagerDetails {get;set;}
        public virtual DbSet<Employee> ? EmployeeDetails {get;set;}
        public virtual DbSet<Project> ? ProjectDetails {get;set;}
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options){
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder){

            modelBuilder.Entity<Recruiter>()
            .Property(m => m.recruiterId)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("CONCAT('23HR', RIGHT('00' + CAST(NEXT VALUE FOR RecruiterSequence AS VARCHAR(2)), 2))");

            modelBuilder.HasSequence<int>("RecruiterSequence", schema: "dbo")
            .StartsAt(1)
            .IncrementsBy(1);
            
            modelBuilder.Entity<Manager>()
            .Property(m => m.managerId)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("CONCAT('23PM', RIGHT('00' + CAST(NEXT VALUE FOR ManagerSequence AS VARCHAR(2)), 2))");

            modelBuilder.HasSequence<int>("ManagerSequence", schema: "dbo")
            .StartsAt(1)
            .IncrementsBy(1);

             modelBuilder.Entity<Employee>()
            .Property(m => m.employeeId)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("CONCAT('23EM', RIGHT('00' + CAST(NEXT VALUE FOR EmployeeSequence AS VARCHAR(2)), 2))");

            modelBuilder.HasSequence<int>("EmployeeSequence", schema: "dbo")
            .StartsAt(1)
            .IncrementsBy(1);

            modelBuilder.Entity<Project>()
            .HasKey(u => u.projectId);
        }
    }
}