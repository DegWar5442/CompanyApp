using System;
using System.Collections.Generic;
using CompanyApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanyApp.Data;

public partial class AppDbContext : DbContext
{
	public AppDbContext()
	{
	}

	public AppDbContext(DbContextOptions<AppDbContext> options)
		: base(options)
	{
	}

	public virtual DbSet<Address> Addresses { get; set; }

	public virtual DbSet<City> Cities { get; set; }



	public virtual DbSet<Company> Companies { get; set; }

	public virtual DbSet<Department> Departments { get; set; }

	public virtual DbSet<Employee> Employees { get; set; }

	public virtual DbSet<Position> Positions { get; set; }



	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
		=> optionsBuilder.UseSqlServer("Server=DESKTOP-MN3NU9T;Database=CompanyAppDB;User ID=companyTestUser;Password=123;TrustServerCertificate=True;");

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Address>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PK__Addresse__3214EC073758FF64");

			entity.Property(e => e.ApartmentNumber).HasMaxLength(20);
			entity.Property(e => e.BuildingNumber).HasMaxLength(20);
			entity.Property(e => e.StreetName).HasMaxLength(200);

			entity.HasOne(d => d.City).WithMany(p => p.Addresses)
				.HasForeignKey(d => d.CityId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK_Addresses_Cities");


		});

		modelBuilder.Entity<City>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PK__Cities__3214EC070C1159A6");



			entity.Property(e => e.Name).HasMaxLength(100);


		});

		modelBuilder.Entity<Company>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PK__Company__3214EC07962A3C7B");

			entity.ToTable("Company");

			entity.Property(e => e.Name).HasMaxLength(255);
			entity.Property(e => e.Phone).HasMaxLength(50);

			entity.HasOne(d => d.Address).WithMany(p => p.Companies)
				.HasForeignKey(d => d.AddressId)
				.HasConstraintName("FK_Company_Addresses");
		});

		modelBuilder.Entity<Department>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PK__Departme__3214EC0764E8D952");

			entity.ToTable("Department");

			entity.HasIndex(e => e.Name, "UQ__Departme__737584F6077959B9").IsUnique();

			entity.Property(e => e.Name).HasMaxLength(100);
		});

		modelBuilder.Entity<Employee>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC0777FA0287");

			entity.ToTable("Employee");

			entity.Property(e => e.FirstName).HasMaxLength(100);
			entity.Property(e => e.LastName).HasMaxLength(100);
			entity.Property(e => e.MiddleName).HasMaxLength(100);
			entity.Property(e => e.Phone).HasMaxLength(50);
			entity.Property(e => e.Salary).HasColumnType("decimal(18, 2)");

			entity.HasOne(d => d.Address).WithMany(p => p.Employees)
				.HasForeignKey(d => d.AddressId)
				.HasConstraintName("FK_Employee_Addresses");

			entity.HasOne(d => d.Department).WithMany(p => p.Employees)
				.HasForeignKey(d => d.DepartmentId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK_Employee_Department");

			entity.HasOne(d => d.Position).WithMany(p => p.Employees)
				.HasForeignKey(d => d.PositionId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK_Employee_Position");
		});

		modelBuilder.Entity<Position>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PK__Position__3214EC076E271330");

			entity.ToTable("Position");

			entity.HasIndex(e => e.Name, "UQ__Position__737584F651063EEF").IsUnique();

			entity.Property(e => e.Name).HasMaxLength(100);
		});





		OnModelCreatingPartial(modelBuilder);
	}

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
