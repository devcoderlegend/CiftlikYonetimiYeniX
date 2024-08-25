using System;
using System.Collections.Generic;
using CiftlikYonetimiYeni.Models;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace CiftlikYonetimiYeni.Data;

public partial class CiftlikYonetimiDbContext : DbContext
{
    public CiftlikYonetimiDbContext()
    {
    }

    public CiftlikYonetimiDbContext(DbContextOptions<CiftlikYonetimiDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CommunicationProtocol> CommunicationProtocols { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<CompanyDetail> CompanyDetails { get; set; }

    public virtual DbSet<DataType> DataTypes { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Device> Devices { get; set; }

    public virtual DbSet<DeviceDepartmentMapping> DeviceDepartmentMappings { get; set; }

    public virtual DbSet<DeviceProfile> DeviceProfiles { get; set; }

    public virtual DbSet<DeviceProfileAttribute> DeviceProfileAttributes { get; set; }

    public virtual DbSet<DeviceValueReceive> DeviceValueReceives { get; set; }

    public virtual DbSet<Farm> Farms { get; set; }

    public virtual DbSet<Rfid> Rfids { get; set; }

    public virtual DbSet<Section> Sections { get; set; }

    public virtual DbSet<Stable> Stables { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserDevice> UserDevices { get; set; }

    public virtual DbSet<UserDeviceType> UserDeviceTypes { get; set; }

    public virtual DbSet<UserProfile> UserProfiles { get; set; }

    public virtual DbSet<UserSectionMapping> UserSectionMappings { get; set; }

    public virtual DbSet<UserSession> UserSessions { get; set; }

    public virtual DbSet<Weight> Weights { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=185.106.20.137;database=CiftlikYonetimiYeni;user=abulu;password=Merlab.2642", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.39-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("latin5_turkish_ci")
            .HasCharSet("latin5");

        modelBuilder.Entity<CommunicationProtocol>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("CommunicationProtocol");

            entity.Property(e => e.Active).HasMaxLength(255);
            entity.Property(e => e.PrimitiveDataType).HasMaxLength(255);
            entity.Property(e => e.ProtocolDescription).HasColumnType("text");
            entity.Property(e => e.ProtocolName).HasMaxLength(255);
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Company");

            entity.Property(e => e.Address).HasColumnType("text");
            entity.Property(e => e.CompanyDescription).HasColumnType("text");
            entity.Property(e => e.CompanyName).HasMaxLength(255);
            entity.Property(e => e.LastUpdate).HasColumnType("datetime");
            entity.Property(e => e.Logo).HasColumnType("blob");
        });

        modelBuilder.Entity<CompanyDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("CompanyDetail");

            entity.HasIndex(e => e.CompanyId, "FK_Company_CompanyDetails");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");

            entity.HasOne(d => d.Company).WithMany(p => p.CompanyDetails)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Company_CompanyDetails");
        });

        modelBuilder.Entity<DataType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("DataType");

            entity.Property(e => e.DataTypeDescription).HasColumnType("text");
            entity.Property(e => e.DataTypeName).HasMaxLength(255);
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Department");

            entity.HasIndex(e => e.CompanyId, "FK_Department_Company");

            entity.Property(e => e.DepartmentDescription).HasColumnType("text");
            entity.Property(e => e.DepartmentName).HasMaxLength(255);
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");

            entity.HasOne(d => d.Company).WithMany(p => p.Departments)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Department_Company");
        });

        modelBuilder.Entity<Device>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Device");

            entity.Property(e => e.DeviceDescription).HasMaxLength(255);
            entity.Property(e => e.DeviceName).HasMaxLength(255);
            entity.Property(e => e.Guid).HasMaxLength(255);
            entity.Property(e => e.UniqueId).HasMaxLength(255);
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");
        });

        modelBuilder.Entity<DeviceDepartmentMapping>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("DeviceDepartmentMapping");

            entity.HasIndex(e => e.DeviceId, "FK_Department_Mapping");

            entity.HasIndex(e => e.DepartmentId, "FK_Device_Department");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");

            entity.HasOne(d => d.Department).WithMany(p => p.DeviceDepartmentMappings)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Device_Department");

            entity.HasOne(d => d.Device).WithMany(p => p.DeviceDepartmentMappings)
                .HasForeignKey(d => d.DeviceId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Department_Mapping");
        });

        modelBuilder.Entity<DeviceProfile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("DeviceProfile");

            entity.HasIndex(e => e.DeviceId, "FK_Device_DeviceProfile");

            entity.Property(e => e.EndByte)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.StartByte)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");

            entity.HasOne(d => d.Device).WithMany(p => p.DeviceProfiles)
                .HasForeignKey(d => d.DeviceId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Device_DeviceProfile");
        });

        modelBuilder.Entity<DeviceProfileAttribute>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("DeviceProfileAttribute");

            entity.HasIndex(e => e.DataTypeId, "FK_DataType_DPA");

            entity.HasIndex(e => e.DeviceProfileId, "FK_DeviceProfile_DeviceProfileAttribute");

            entity.Property(e => e.UpdateTime).HasColumnType("datetime");

            entity.HasOne(d => d.DataType).WithMany(p => p.DeviceProfileAttributes)
                .HasForeignKey(d => d.DataTypeId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_DataType_DPA");

            entity.HasOne(d => d.DeviceProfile).WithMany(p => p.DeviceProfileAttributes)
                .HasForeignKey(d => d.DeviceProfileId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_DeviceProfile_DeviceProfileAttribute");
        });

        modelBuilder.Entity<DeviceValueReceive>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("DeviceValueReceive");

            entity.HasIndex(e => e.DeviceDepartmentMappingId, "FK_DeviceMap_Received");

            entity.Property(e => e.InsertTime).HasColumnType("datetime");
            entity.Property(e => e.ReceivedInformation).HasColumnType("tinyblob");
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");

            entity.HasOne(d => d.DeviceDepartmentMapping).WithMany(p => p.DeviceValueReceives)
                .HasForeignKey(d => d.DeviceDepartmentMappingId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_DeviceMap_Received");
        });

        modelBuilder.Entity<Farm>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Farm");

            entity.HasIndex(e => e.DepartmentId, "FK_Department_Farm");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Department).WithMany(p => p.Farms)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Department_Farm");
        });

        modelBuilder.Entity<Rfid>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("RFIDS");

            entity.HasIndex(e => e.DeviceValueReceiveId, "FK_RFID_DeviveValue");

            entity.HasIndex(e => e.UserId, "FK_User_RFIDUser");

            entity.Property(e => e.DataValue).HasColumnType("tinyblob");
            entity.Property(e => e.InsertTime).HasColumnType("datetime");
            entity.Property(e => e.Rfid1)
                .HasMaxLength(255)
                .HasColumnName("RFID");
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");

            entity.HasOne(d => d.DeviceValueReceive).WithMany(p => p.Rfids)
                .HasForeignKey(d => d.DeviceValueReceiveId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_RFID_DeviveValue");

            entity.HasOne(d => d.User).WithMany(p => p.Rfids)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_User_RFIDUser");
        });

        modelBuilder.Entity<Section>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Section");

            entity.Property(e => e.SectionDescription).HasColumnType("text");
            entity.Property(e => e.SectionName).HasMaxLength(255);
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");
        });

        modelBuilder.Entity<Stable>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Stable");

            entity.HasIndex(e => e.FarmId, "FK_Farm_Stable");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.FarmDescription).HasColumnType("text");
            entity.Property(e => e.FarmName).HasMaxLength(255);
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");

            entity.HasOne(d => d.Farm).WithMany(p => p.Stables)
                .HasForeignKey(d => d.FarmId)
                .HasConstraintName("FK_Farm_Stable");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("User");

            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            entity.Property(e => e.UserName).HasMaxLength(255);
        });

        modelBuilder.Entity<UserDevice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("UserDevice");

            entity.HasIndex(e => e.UserDeviceTypeId, "FK_UserDeviceType_UserDevice");

            entity.Property(e => e.BrandName).HasMaxLength(1000);
            entity.Property(e => e.DeviceId).HasMaxLength(1000);
            entity.Property(e => e.Model).HasMaxLength(1000);
            entity.Property(e => e.RegistrationDate).HasColumnType("datetime");
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            entity.Property(e => e.UserAgent).HasMaxLength(1000);

            entity.HasOne(d => d.UserDeviceType).WithMany(p => p.UserDevices)
                .HasForeignKey(d => d.UserDeviceTypeId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_UserDeviceType_UserDevice");
        });

        modelBuilder.Entity<UserDeviceType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("UserDeviceType");

            entity.Property(e => e.Active).HasMaxLength(255);
            entity.Property(e => e.DeviceTypeName).HasMaxLength(255);
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");
        });

        modelBuilder.Entity<UserProfile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("UserProfile");

            entity.HasIndex(e => e.UserId, "FK_User_UserProfile");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FullName).HasMaxLength(255);
            entity.Property(e => e.NationalId).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber).HasMaxLength(255);
            entity.Property(e => e.Photo).HasColumnType("blob");
            entity.Property(e => e.StaffId).HasMaxLength(255);

            entity.HasOne(d => d.User).WithMany(p => p.UserProfiles)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_User_UserProfile");
        });

        modelBuilder.Entity<UserSectionMapping>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("UserSectionMapping");

            entity.HasIndex(e => e.SectionId, "FK_User_Section");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");

            entity.HasOne(d => d.Section).WithMany(p => p.UserSectionMappings)
                .HasForeignKey(d => d.SectionId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_User_Section");
        });

        modelBuilder.Entity<UserSession>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("UserSession");

            entity.HasIndex(e => e.DeviceId, "FK_Device_UserSession");

            entity.HasIndex(e => e.UserId, "FK_User_UserSession");

            entity.Property(e => e.ExpireTime).HasColumnType("datetime");
            entity.Property(e => e.GeneratedKey).HasMaxLength(255);
            entity.Property(e => e.IpAddress).HasMaxLength(255);
            entity.Property(e => e.LoginTime).HasColumnType("datetime");
            entity.Property(e => e.Updatetime).HasColumnType("datetime");

            entity.HasOne(d => d.Device).WithMany(p => p.UserSessions)
                .HasForeignKey(d => d.DeviceId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Device_UserSession");

            entity.HasOne(d => d.User).WithMany(p => p.UserSessions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_User_UserSession");
        });

        modelBuilder.Entity<Weight>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasIndex(e => e.DeviceValueReceiveId, "FK_DeviceValue_Weight");

            entity.HasIndex(e => e.UserId, "FK_User_Weight");

            entity.Property(e => e.DataValue).HasColumnType("tinyblob");
            entity.Property(e => e.InsertTime).HasColumnType("datetime");
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            entity.Property(e => e.Weight1).HasColumnName("Weight");

            entity.HasOne(d => d.DeviceValueReceive).WithMany(p => p.Weights)
                .HasForeignKey(d => d.DeviceValueReceiveId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_DeviceValue_Weight");

            entity.HasOne(d => d.User).WithMany(p => p.Weights)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_User_Weight");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}