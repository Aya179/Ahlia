using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Ahlia.Models
{
    public partial class AhliahContext : DbContext
    {
        public AhliahContext()
        {
        }

        public AhliahContext(DbContextOptions<AhliahContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BannedClient> BannedClients { get; set; } = null!;
        public virtual DbSet<Banning> Bannings { get; set; } = null!;
        public virtual DbSet<City> Cities { get; set; } = null!;
        public virtual DbSet<Client> Clients { get; set; } = null!;
        public virtual DbSet<ClientStokcksMovement> ClientStokcksMovements { get; set; } = null!;
        public virtual DbSet<ClientType> ClientTypes { get; set; } = null!;
        public virtual DbSet<Employee> Employees { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;
        public virtual DbSet<Penefit> Penefits { get; set; } = null!;
        public virtual DbSet<StockPrice> StockPrices { get; set; } = null!;
        public virtual DbSet<StocksMovement> StocksMovements { get; set; } = null!;
        public virtual DbSet<StoppedClient> StoppedClients { get; set; } = null!;
        public virtual DbSet<Stopping> Stoppings { get; set; } = null!;
        public virtual DbSet<Rassing> Rassing { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-I8KMUEQ;Database=Ahliah;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<BannedClient>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BannedTypeId).HasColumnName("bannedTypeId");

                entity.Property(e => e.ClientId).HasColumnName("clientId");

                entity.Property(e => e.Enddate)
                    .HasColumnType("date")
                    .HasColumnName("enddate");

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.OrderedBy)
                    .HasMaxLength(50)
                    .HasColumnName("orderedBy");

                entity.Property(e => e.Photo)
                 
                     .HasColumnType("image");
                entity.Property(e => e.CancelImage)

                    .HasColumnType("image");


                entity.Property(e => e.Reason)
                    .HasMaxLength(500)
                    .HasColumnName("reason");

                entity.Property(e => e.Startdate)
                    .HasColumnType("date")
                    .HasColumnName("startdate");

                entity.HasOne(d => d.BannedType)
                    .WithMany(p => p.BannedClients)
                    .HasForeignKey(d => d.BannedTypeId)
                    .HasConstraintName("FK_BannedClients_Banning");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.BannedClients)
                    .HasForeignKey(d => d.ClientId)
                    .HasConstraintName("FK_BannedClients_Client");
            });
            modelBuilder.Entity<Rassing>(entity =>
            {
                entity.ToTable("Rassing");

                entity.Property(e => e.id).HasColumnName("id");
                entity.Property(e => e.Amount).HasColumnType("float");
                entity.Property(e => e.ContractImag).HasColumnType("image");
                entity.Property(e => e.RassingType).HasMaxLength(150);
                entity.Property(e => e.RassingDate).HasColumnType("date");




            });

            modelBuilder.Entity<Banning>(entity =>
            {
                entity.ToTable("Banning");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BannedType)
                    .HasMaxLength(50)
                    .HasColumnName("bannedType");
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.ToTable("City");

                entity.Property(e => e.CityId).HasColumnName("cityId");

                entity.Property(e => e.CityName)
                    .HasMaxLength(50)
                    .HasColumnName("cityName");

                entity.Property(e => e.Country)
                    .HasMaxLength(50)
                    .HasColumnName("country");
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("Client");
                //entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ActiveStocks).HasColumnName("activeStocks");
                entity.Property(e => e.clientnumber).HasColumnName("clientnumber");

                entity.Property(e => e.AddressDomicil)
                    .HasMaxLength(150)
                    .HasColumnName("addressDomicil");

                entity.Property(e => e.AddressWork)
                    .HasMaxLength(150)
                    .HasColumnName("addressWork");

                entity.Property(e => e.BankAccount)
                    .HasMaxLength(50)
                    .HasColumnName("bankAccount");

                entity.Property(e => e.Birthcity)
                    .HasMaxLength(50)
                    .HasColumnName("birthcity");

                entity.Property(e => e.Birthdate)
                    .HasColumnType("date")
                    .HasColumnName("birthdate");

                entity.Property(e => e.CityId).HasColumnName("cityId");

                entity.Property(e => e.ClientStatus)
                    .HasMaxLength(50)
                    .HasColumnName("clientStatus");

                entity.Property(e => e.ClientTypeId).HasColumnName("clientTypeId");

                entity.Property(e => e.Fax)
                    .HasMaxLength(50)
                    .HasColumnName("fax");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .HasColumnName("firstName");

                entity.Property(e => e.HomePhone)
                    .HasMaxLength(50)
                    .HasColumnName("homePhone");

                entity.Property(e => e.IdcardPhoto)
                    .HasColumnType("image");

                entity.Property(e => e.Idphoto)
                    .HasColumnType("image");

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.IsAlive).HasColumnName("isAlive");

                entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");

                entity.Property(e => e.Khana)
                    .HasMaxLength(50)
                    .HasColumnName("khana");

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .HasColumnName("lastName");

                entity.Property(e => e.MiddleName)
                    .HasMaxLength(50)
                    .HasColumnName("middleName");

                entity.Property(e => e.Mobile1)
                    .HasMaxLength(50)
                    .HasColumnName("mobile1");

                entity.Property(e => e.Mobile2)
                    .HasMaxLength(50)
                    .HasColumnName("mobile2");

                entity.Property(e => e.Mother)
                    .HasMaxLength(50)
                    .HasColumnName("mother");

                entity.Property(e => e.NationalId)
                    .HasMaxLength(50)
                    .HasColumnName("nationalId");

                entity.Property(e => e.NationalIdType)
                    .HasMaxLength(50)
                    .HasColumnName("nationalIdType");

                entity.Property(e => e.Nationality)
                    .HasMaxLength(50)
                    .HasColumnName("nationality");

                entity.Property(e => e.NotactiveStocks).HasColumnName("notactiveStocks");

                entity.Property(e => e.Notes)
                    .HasMaxLength(150)
                    .HasColumnName("notes");

                entity.Property(e => e.OriginalAddress)
                    .HasMaxLength(150)
                    .HasColumnName("originalAddress");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Clients)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("FK_Client_City");

                entity.HasOne(d => d.ClientType)
                    .WithMany(p => p.Clients)
                    .HasForeignKey(d => d.ClientTypeId)
                    .HasConstraintName("FK_Client_ClientType");
            });

            modelBuilder.Entity<ClientStokcksMovement>(entity =>
            {
                entity.ToTable("ClientStokcksMovement");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.Property(e => e.ClientId).HasColumnName("clientId");

                entity.Property(e => e.ContractImage)
                    .HasMaxLength(150)
                    .HasColumnName("contractImage");

                entity.Property(e => e.IsApproved).HasColumnName("isApproved");

                entity.Property(e => e.MovementDate)
                    .HasColumnType("date")
                    .HasColumnName("movementDate");

                entity.Property(e => e.MovementTypeId).HasColumnName("movementTypeId");

                entity.Property(e => e.NewClientId).HasColumnName("newClientId");

                entity.Property(e => e.Notes)
                    .HasMaxLength(250)
                    .HasColumnName("notes");

                entity.Property(e => e.Reason)
                    .HasMaxLength(200)
                    .HasColumnName("reason");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ClientStokcksMovementClients)
                    .HasForeignKey(d => d.ClientId)
                    .HasConstraintName("FK_ClientStokcksMovement_Client");

                entity.HasOne(d => d.MovementType)
                    .WithMany(p => p.ClientStokcksMovements)
                    .HasForeignKey(d => d.MovementTypeId)
                    .HasConstraintName("FK_ClientStokcksMovement_StocksMovement");

                entity.HasOne(d => d.NewClient)
                    .WithMany(p => p.ClientStokcksMovementNewClients)
                    .HasForeignKey(d => d.NewClientId)
                    .HasConstraintName("FK_ClientStokcksMovement_Client1");
            });

            modelBuilder.Entity<ClientType>(entity =>
            {
                entity.HasKey(e => e.TypeId);

                entity.ToTable("ClientType");

                entity.Property(e => e.TypeId).HasColumnName("typeId");

                entity.Property(e => e.Notes)
                    .HasMaxLength(50)
                    .HasColumnName("notes");

                entity.Property(e => e.TypeName)
                    .HasMaxLength(50)
                    .HasColumnName("typeName");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("Employee");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address)
                    .HasMaxLength(500)
                    .HasColumnName("address");

                entity.Property(e => e.CityId).HasColumnName("cityId");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .HasColumnName("email");

                entity.Property(e => e.EmpId)
                    .HasMaxLength(10)
                    .HasColumnName("empId")
                    .IsFixedLength();

                entity.Property(e => e.EmployeeName)
                    .HasMaxLength(50)
                    .HasColumnName("employeeName");

                entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");

                entity.Property(e => e.Mobile)
                    .HasMaxLength(10)
                    .HasColumnName("mobile")
                    .IsFixedLength();

                entity.Property(e => e.Password)
                    .HasMaxLength(250)
                    .HasColumnName("password");
                entity.Property(e => e.RoleId)
                   .HasMaxLength(450)
                   .HasColumnName("roleId")
                   .IsFixedLength();

              //  entity.Property(e => e.RoleId).HasColumnName("roleId");

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .HasColumnName("userName");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("FK_Employee_City");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("amount");

                entity.Property(e => e.BankAccount)
                    .HasMaxLength(150)
                    .HasColumnName("bankAccount");

                entity.Property(e => e.BranchName)
                    .HasMaxLength(50)
                    .HasColumnName("branchName");

                entity.Property(e => e.CardNumber).HasColumnName("cardNumber");

                entity.Property(e => e.CheckDate)
                    .HasColumnType("date")
                    .HasColumnName("checkDate");

                entity.Property(e => e.CheckNumber)
                    .HasMaxLength(50)
                    .HasColumnName("checkNumber");

                entity.Property(e => e.Checker).HasColumnName("checker");

                entity.Property(e => e.CityId).HasColumnName("cityId");

                entity.Property(e => e.ClientId).HasColumnName("clientId");

                entity.Property(e => e.Editor).HasColumnName("editor");

                entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");

                entity.Property(e => e.IsPayed).HasColumnName("isPayed");

                entity.Property(e => e.Notes)
                    .HasMaxLength(150)
                    .HasColumnName("notes");

                entity.Property(e => e.PayementFor)
                    .HasMaxLength(500)
                    .HasColumnName("payementFor");

                entity.Property(e => e.Paymentdate)
                    .HasColumnType("date")
                    .HasColumnName("paymentdate");

                entity.Property(e => e.ReceiverName)
                    .HasMaxLength(50)
                    .HasColumnName("receiverName");

                entity.Property(e => e.ReceiverNumber)
                    .HasMaxLength(50)
                    .HasColumnName("receiverNumber");

                entity.Property(e => e.ReceiverResidance)
                    .HasMaxLength(150)
                    .HasColumnName("receiverResidance");

                entity.Property(e => e.RecieveDate)
                    .HasColumnType("date")
                    .HasColumnName("recieveDate");

                entity.HasOne(d => d.CheckerNavigation)
                    .WithMany(p => p.PaymentCheckerNavigations)
                    .HasForeignKey(d => d.Checker)
                    .HasConstraintName("FK_Payments_Employee1");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("FK_Payments_City");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.ClientId)
                    .HasConstraintName("FK_Payments_Client");

                entity.HasOne(d => d.EditorNavigation)
                    .WithMany(p => p.PaymentEditorNavigations)
                    .HasForeignKey(d => d.Editor)
                    .HasConstraintName("FK_Payments_Employee");
            });

            modelBuilder.Entity<Penefit>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ClientId).HasColumnName("clientId");

                entity.Property(e => e.CompleteAmount)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("completeAmount");

                entity.Property(e => e.PriceId).HasColumnName("priceId");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.Penefits)
                    .HasForeignKey(d => d.ClientId)
                    .HasConstraintName("FK_Penefits_Client");

                entity.HasOne(d => d.Price)
                    .WithMany(p => p.Penefits)
                    .HasForeignKey(d => d.PriceId)
                    .HasConstraintName("FK_Penefits_StockPrices");
            });

            modelBuilder.Entity<StockPrice>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.ContractImage)
                   .HasColumnType("image");
                entity.Property(e => e.IsApprove).HasColumnName("isApprove");

                entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");

                entity.Property(e => e.Sharedate)
                    .HasColumnType("date")
                    .HasColumnName("sharedate");

                entity.Property(e => e.Shareprice)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("shareprice");

                entity.Property(e => e.Year).HasColumnName("year");
            });

            modelBuilder.Entity<StocksMovement>(entity =>
            {
                entity.ToTable("StocksMovement");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .HasColumnName("description");

                entity.Property(e => e.MovementType)
                    .HasMaxLength(50)
                    .HasColumnName("movementType");
            });

            modelBuilder.Entity<StoppedClient>(entity =>
            {
                //entity.HasNoKey();

                entity.ToTable("StoppedClient");
                entity.Property(e => e.Id).HasColumnName("id");


                entity.Property(e => e.ClientId).HasColumnName("clientId");

                entity.Property(e => e.Enddate)
                    .HasColumnType("date")
                    .HasColumnName("enddate");

                entity.Property(e => e.Photo)

                   .HasColumnType("image");
                entity.Property(e => e.CancelImage)

                    .HasColumnType("image");
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.Reason)
                    .HasMaxLength(500)
                    .HasColumnName("reason");

                entity.Property(e => e.StartDate)
                    .HasColumnType("date")
                    .HasColumnName("startDate");

                entity.Property(e => e.StoppedTypeId).HasColumnName("stoppedTypeId");

                entity.HasOne(d => d.Client)
                    .WithMany()
                    .HasForeignKey(d => d.ClientId)
                    .HasConstraintName("FK_StoppedClient_Client");

                entity.HasOne(d => d.StoppedType)
                    .WithMany()
                    .HasForeignKey(d => d.StoppedTypeId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_StoppedClient_Stopping");
            });

            modelBuilder.Entity<Stopping>(entity =>
            {
                entity.ToTable("Stopping");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.StoppedStatus).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
