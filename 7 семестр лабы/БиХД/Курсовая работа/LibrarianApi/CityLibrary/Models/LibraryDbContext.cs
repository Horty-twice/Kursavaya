using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CityLibrary.Models;

public partial class LibraryDbContext : DbContext
{
    public LibraryDbContext()
    {
    }

    public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BookCopy> BookCopies { get; set; }

    public virtual DbSet<BookCopyStatus> BookCopyStatuses { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Fine> Fines { get; set; }

    public virtual DbSet<FineReason> FineReasons { get; set; }

    public virtual DbSet<FineStatus> FineStatuses { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Issue> Issues { get; set; }

    public virtual DbSet<IssueItem> IssueItems { get; set; }

    public virtual DbSet<IssueItemStatus> IssueItemStatuses { get; set; }

    public virtual DbSet<IssueStatus> IssueStatuses { get; set; }

    public virtual DbSet<Library> Libraries { get; set; }

    public virtual DbSet<ManagerRequest> ManagerRequests { get; set; }

    public virtual DbSet<ManagerRequestStatus> ManagerRequestStatuses { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<PurchaseRequest> PurchaseRequests { get; set; }

    public virtual DbSet<PurchaseRequestItem> PurchaseRequestItems { get; set; }

    public virtual DbSet<PurchaseRequestItemStatus> PurchaseRequestItemStatuses { get; set; }

    public virtual DbSet<PurchaseRequestStatus> PurchaseRequestStatuses { get; set; }

    public virtual DbSet<Reader> Readers { get; set; }

    public virtual DbSet<ReaderStatus> ReaderStatuses { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<Supply> Supplies { get; set; }

    public virtual DbSet<SupplyItem> SupplyItems { get; set; }

    public virtual DbSet<SupplyItemStatus> SupplyItemStatuses { get; set; }

    public virtual DbSet<SupplyStatus> SupplyStatuses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=city_libraries_db;Username=postgres;Password=1234");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("author_pkey");

            entity.ToTable("author");

            entity.HasIndex(e => e.LastName, "idx_author_last_name");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("last_name");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(100)
                .HasColumnName("middle_name");
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("book_pkey");

            entity.ToTable("book");

            entity.HasIndex(e => e.Title, "book_title_key").IsUnique();

            entity.HasIndex(e => e.Genreid, "idx_book_genre_id");

            entity.HasIndex(e => e.Genreid, "idx_book_genreid");

            entity.HasIndex(e => e.Title, "idx_book_title");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AvailableQuantity).HasColumnName("available_quantity");
            entity.Property(e => e.Genreid).HasColumnName("genreid");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
            entity.Property(e => e.TotalQuantity).HasColumnName("total_quantity");

            entity.HasOne(d => d.Genre).WithMany(p => p.Books)
                .HasForeignKey(d => d.Genreid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fkbook500794");

            entity.HasMany(d => d.Authors).WithMany(p => p.Books)
                .UsingEntity<Dictionary<string, object>>(
                    "BookAuthor",
                    r => r.HasOne<Author>().WithMany()
                        .HasForeignKey("Authorid")
                        .HasConstraintName("fkbook_autho683369"),
                    l => l.HasOne<Book>().WithMany()
                        .HasForeignKey("Bookid")
                        .HasConstraintName("fkbook_autho786924"),
                    j =>
                    {
                        j.HasKey("Bookid", "Authorid").HasName("book_author_pkey");
                        j.ToTable("book_author");
                        j.HasIndex(new[] { "Authorid" }, "idx_book_author_authorid");
                        j.IndexerProperty<int>("Bookid").HasColumnName("bookid");
                        j.IndexerProperty<int>("Authorid").HasColumnName("authorid");
                    });
        });

        modelBuilder.Entity<BookCopy>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("book_copy_pkey");

            entity.ToTable("book_copy");

            entity.HasIndex(e => e.InventoryNumber, "book_copy_inventory_number_key").IsUnique();

            entity.HasIndex(e => e.Bookid, "idx_book_copy_bookid");

            entity.HasIndex(e => e.BookCopyStatusid, "idx_book_copy_statusid");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ArrivalDate)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnName("arrival_date");
            entity.Property(e => e.BookCopyStatusid).HasColumnName("book_copy_statusid");
            entity.Property(e => e.Bookid).HasColumnName("bookid");
            entity.Property(e => e.InventoryNumber)
                .HasMaxLength(50)
                .HasColumnName("inventory_number");

            entity.HasOne(d => d.BookCopyStatus).WithMany(p => p.BookCopies)
                .HasForeignKey(d => d.BookCopyStatusid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fkbook_copy587427");

            entity.HasOne(d => d.Book).WithMany(p => p.BookCopies)
                .HasForeignKey(d => d.Bookid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fkbook_copy961686");
        });

        modelBuilder.Entity<BookCopyStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("book_copy_status_pkey");

            entity.ToTable("book_copy_status");

            entity.HasIndex(e => e.Name, "book_copy_status_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("employee_pkey");

            entity.ToTable("employee");

            entity.HasIndex(e => e.LastName, "idx_employee_last_name");

            entity.HasIndex(e => e.Libraryid, "idx_employee_libraryid");

            entity.HasIndex(e => new { e.LastName, e.FirstName }, "idx_employee_name_search");

            entity.HasIndex(e => e.Positionid, "idx_employee_positionid");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EmploymentDate).HasColumnName("employment_date");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("last_name");
            entity.Property(e => e.Libraryid).HasColumnName("libraryid");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(100)
                .HasColumnName("middle_name");
            entity.Property(e => e.Positionid).HasColumnName("positionid");

            entity.HasOne(d => d.Library).WithMany(p => p.Employees)
                .HasForeignKey(d => d.Libraryid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fkemployee514608");

            entity.HasOne(d => d.Position).WithMany(p => p.Employees)
                .HasForeignKey(d => d.Positionid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fkemployee650198");
        });

        modelBuilder.Entity<Fine>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("fine_pkey");

            entity.ToTable("fine");

            entity.HasIndex(e => new { e.ImpositionDate, e.PaymentDate }, "idx_fine_dates");

            entity.HasIndex(e => e.ImpositionDate, "idx_fine_imposition_date");

            entity.HasIndex(e => e.IssueItemid, "idx_fine_issue_itemid");

            entity.HasIndex(e => e.PaymentDate, "idx_fine_payment_date");

            entity.HasIndex(e => e.Readerid, "idx_fine_reader_payment").HasFilter("(payment_date IS NULL)");

            entity.HasIndex(e => e.Readerid, "idx_fine_readerid");

            entity.HasIndex(e => e.FineReasonid, "idx_fine_reasonid");

            entity.HasIndex(e => e.FineStatusid, "idx_fine_statusid");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Amount)
                .HasPrecision(10, 2)
                .HasColumnName("amount");
            entity.Property(e => e.FineReasonid).HasColumnName("fine_reasonid");
            entity.Property(e => e.FineStatusid).HasColumnName("fine_statusid");
            entity.Property(e => e.ImpositionDate)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnName("imposition_date");
            entity.Property(e => e.IssueItemid).HasColumnName("issue_itemid");
            entity.Property(e => e.PaymentDate).HasColumnName("payment_date");
            entity.Property(e => e.Readerid).HasColumnName("readerid");

            entity.HasOne(d => d.FineReason).WithMany(p => p.Fines)
                .HasForeignKey(d => d.FineReasonid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fkfine244827");

            entity.HasOne(d => d.FineStatus).WithMany(p => p.Fines)
                .HasForeignKey(d => d.FineStatusid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fkfine160352");

            entity.HasOne(d => d.IssueItem).WithMany(p => p.Fines)
                .HasForeignKey(d => d.IssueItemid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fkfine6543");

            entity.HasOne(d => d.Reader).WithMany(p => p.Fines)
                .HasForeignKey(d => d.Readerid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fkfine859344");
        });

        modelBuilder.Entity<FineReason>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("fine_reason_pkey");

            entity.ToTable("fine_reason");

            entity.HasIndex(e => e.Name, "fine_reason_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<FineStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("fine_status_pkey");

            entity.ToTable("fine_status");

            entity.HasIndex(e => e.Name, "fine_status_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("genre_pkey");

            entity.ToTable("genre");

            entity.HasIndex(e => e.Name, "genre_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Issue>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("issue_pkey");

            entity.ToTable("issue");

            entity.HasIndex(e => new { e.RequestDate, e.PlannedReturnDate }, "idx_issue_dates");

            entity.HasIndex(e => e.Employeeid, "idx_issue_employeeid");

            entity.HasIndex(e => e.PlannedReturnDate, "idx_issue_planned_return_date");

            entity.HasIndex(e => e.Readerid, "idx_issue_reader_id");

            entity.HasIndex(e => e.Readerid, "idx_issue_readerid");

            entity.HasIndex(e => e.RequestDate, "idx_issue_request_date");

            entity.HasIndex(e => e.IssueStatusid, "idx_issue_statusid");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Employeeid).HasColumnName("employeeid");
            entity.Property(e => e.IssueStatusid).HasColumnName("issue_statusid");
            entity.Property(e => e.PlannedReturnDate).HasColumnName("planned_return_date");
            entity.Property(e => e.Readerid).HasColumnName("readerid");
            entity.Property(e => e.RequestDate)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnName("request_date");

            entity.HasOne(d => d.Employee).WithMany(p => p.Issues)
                .HasForeignKey(d => d.Employeeid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fkissue18721");

            entity.HasOne(d => d.IssueStatus).WithMany(p => p.Issues)
                .HasForeignKey(d => d.IssueStatusid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fkissue236550");

            entity.HasOne(d => d.Reader).WithMany(p => p.Issues)
                .HasForeignKey(d => d.Readerid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fkissue492432");
        });

        modelBuilder.Entity<IssueItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("issue_item_pkey");

            entity.ToTable("issue_item");

            entity.HasIndex(e => e.ActualReturnDate, "idx_issue_item_actual_return_date");

            entity.HasIndex(e => e.Bookid, "idx_issue_item_book_id");

            entity.HasIndex(e => e.Bookid, "idx_issue_item_bookid");

            entity.HasIndex(e => e.Issueid, "idx_issue_item_issue_id");

            entity.HasIndex(e => e.Issueid, "idx_issue_item_issueid");

            entity.HasIndex(e => e.IssueItemStatusid, "idx_issue_item_statusid");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ActualReturnDate).HasColumnName("actual_return_date");
            entity.Property(e => e.Bookid).HasColumnName("bookid");
            entity.Property(e => e.IssueItemStatusid).HasColumnName("issue_item_statusid");
            entity.Property(e => e.Issueid).HasColumnName("issueid");

            entity.HasOne(d => d.Book).WithMany(p => p.IssueItems)
                .HasForeignKey(d => d.Bookid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fkissue_item694591");

            entity.HasOne(d => d.IssueItemStatus).WithMany(p => p.IssueItems)
                .HasForeignKey(d => d.IssueItemStatusid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fkissue_item139245");

            entity.HasOne(d => d.Issue).WithMany(p => p.IssueItems)
                .HasForeignKey(d => d.Issueid)
                .HasConstraintName("fkissue_item657321");
        });

        modelBuilder.Entity<IssueItemStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("issue_item_status_pkey");

            entity.ToTable("issue_item_status");

            entity.HasIndex(e => e.Name, "issue_item_status_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<IssueStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("issue_status_pkey");

            entity.ToTable("issue_status");

            entity.HasIndex(e => e.Name, "issue_status_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Library>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("library_pkey");

            entity.ToTable("library");

            entity.HasIndex(e => e.Phone, "idx_library_phone");

            entity.HasIndex(e => e.Name, "library_name_key").IsUnique();

            entity.HasIndex(e => e.Phone, "library_phone_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BuildingNumber)
                .HasMaxLength(10)
                .HasColumnName("building_number");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.Street)
                .HasMaxLength(100)
                .HasColumnName("street");
        });

        modelBuilder.Entity<ManagerRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("manager_request_pkey");

            entity.ToTable("manager_request");

            entity.HasIndex(e => e.Bookid, "idx_manager_request_bookid");

            entity.HasIndex(e => e.CreationDate, "idx_manager_request_creation_date");

            entity.HasIndex(e => e.Employeeid, "idx_manager_request_employeeid");

            entity.HasIndex(e => e.ManagerRequestStatusid, "idx_manager_request_statusid");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Bookid).HasColumnName("bookid");
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnName("creation_date");
            entity.Property(e => e.Employeeid).HasColumnName("employeeid");
            entity.Property(e => e.ManagerRequestStatusid).HasColumnName("manager_request_statusid");

            entity.HasOne(d => d.Book).WithMany(p => p.ManagerRequests)
                .HasForeignKey(d => d.Bookid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fkmanager_re167454");

            entity.HasOne(d => d.Employee).WithMany(p => p.ManagerRequests)
                .HasForeignKey(d => d.Employeeid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fkmanager_re940456");

            entity.HasOne(d => d.ManagerRequestStatus).WithMany(p => p.ManagerRequests)
                .HasForeignKey(d => d.ManagerRequestStatusid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fkmanager_re173053");
        });

        modelBuilder.Entity<ManagerRequestStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("manager_request_status_pkey");

            entity.ToTable("manager_request_status");

            entity.HasIndex(e => e.Name, "manager_request_status_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("position_pkey");

            entity.ToTable("position");

            entity.HasIndex(e => e.Name, "position_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<PurchaseRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("purchase_request_pkey");

            entity.ToTable("purchase_request");

            entity.HasIndex(e => e.CreationDate, "idx_purchase_request_creation_date");

            entity.HasIndex(e => e.PurchaseRequestStatusid, "idx_purchase_request_statusid");

            entity.HasIndex(e => e.Supplierid, "idx_purchase_request_supplierid");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnName("creation_date");
            entity.Property(e => e.PurchaseRequestStatusid).HasColumnName("purchase_request_statusid");
            entity.Property(e => e.Supplierid).HasColumnName("supplierid");

            entity.HasOne(d => d.PurchaseRequestStatus).WithMany(p => p.PurchaseRequests)
                .HasForeignKey(d => d.PurchaseRequestStatusid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fkpurchase_r536362");

            entity.HasOne(d => d.Supplier).WithMany(p => p.PurchaseRequests)
                .HasForeignKey(d => d.Supplierid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fkpurchase_r909394");
        });

        modelBuilder.Entity<PurchaseRequestItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("purchase_request_item_pkey");

            entity.ToTable("purchase_request_item");

            entity.HasIndex(e => e.Bookid, "idx_purchase_request_item_bookid");

            entity.HasIndex(e => e.PurchaseRequestid, "idx_purchase_request_item_requestid");

            entity.HasIndex(e => e.PurchaseRequestItemStatusid, "idx_purchase_request_item_statusid");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Bookid).HasColumnName("bookid");
            entity.Property(e => e.PurchaseRequestItemStatusid).HasColumnName("purchase_request_item_statusid");
            entity.Property(e => e.PurchaseRequestid).HasColumnName("purchase_requestid");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Book).WithMany(p => p.PurchaseRequestItems)
                .HasForeignKey(d => d.Bookid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fkpurchase_r396518");

            entity.HasOne(d => d.PurchaseRequestItemStatus).WithMany(p => p.PurchaseRequestItems)
                .HasForeignKey(d => d.PurchaseRequestItemStatusid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fkpurchase_r643340");

            entity.HasOne(d => d.PurchaseRequest).WithMany(p => p.PurchaseRequestItems)
                .HasForeignKey(d => d.PurchaseRequestid)
                .HasConstraintName("fkpurchase_r25238");
        });

        modelBuilder.Entity<PurchaseRequestItemStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("purchase_request_item_status_pkey");

            entity.ToTable("purchase_request_item_status");

            entity.HasIndex(e => e.Name, "purchase_request_item_status_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<PurchaseRequestStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("purchase_request_status_pkey");

            entity.ToTable("purchase_request_status");

            entity.HasIndex(e => e.Name, "purchase_request_status_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Reader>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("reader_pkey");

            entity.ToTable("reader");

            entity.HasIndex(e => e.LastName, "idx_reader_last_name");

            entity.HasIndex(e => e.Libraryid, "idx_reader_libraryid");

            entity.HasIndex(e => new { e.LastName, e.FirstName }, "idx_reader_name_search");

            entity.HasIndex(e => e.Phone, "idx_reader_phone");

            entity.HasIndex(e => e.RegistrationDate, "idx_reader_registration_date");

            entity.HasIndex(e => e.ReaderStatusid, "idx_reader_statusid");

            entity.HasIndex(e => e.Phone, "reader_phone_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("last_name");
            entity.Property(e => e.Libraryid).HasColumnName("libraryid");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(100)
                .HasColumnName("middle_name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.ReaderStatusid).HasColumnName("reader_statusid");
            entity.Property(e => e.RegistrationDate)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnName("registration_date");

            entity.HasOne(d => d.Library).WithMany(p => p.Readers)
                .HasForeignKey(d => d.Libraryid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fkreader5851");

            entity.HasOne(d => d.ReaderStatus).WithMany(p => p.Readers)
                .HasForeignKey(d => d.ReaderStatusid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fkreader264946");
        });

        modelBuilder.Entity<ReaderStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("reader_status_pkey");

            entity.ToTable("reader_status");

            entity.HasIndex(e => e.Name, "reader_status_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("supplier_pkey");

            entity.ToTable("supplier");

            entity.HasIndex(e => e.Name, "idx_supplier_name");

            entity.HasIndex(e => e.Phone, "idx_supplier_phone");

            entity.HasIndex(e => e.Phone, "supplier_phone_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BuildingNumber)
                .HasMaxLength(10)
                .HasColumnName("building_number");
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .HasColumnName("city");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .HasColumnName("name");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.Rating)
                .HasDefaultValue(3)
                .HasColumnName("rating");
            entity.Property(e => e.Street)
                .HasMaxLength(100)
                .HasColumnName("street");
        });

        modelBuilder.Entity<Supply>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("supply_pkey");

            entity.ToTable("supply");

            entity.HasIndex(e => e.DeliveryDate, "idx_supply_delivery_date");

            entity.HasIndex(e => e.OrderDate, "idx_supply_order_date");

            entity.HasIndex(e => e.SupplyStatusid, "idx_supply_statusid");

            entity.HasIndex(e => e.Supplierid, "idx_supply_supplierid");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DeliveryDate).HasColumnName("delivery_date");
            entity.Property(e => e.OrderDate)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnName("order_date");
            entity.Property(e => e.Supplierid).HasColumnName("supplierid");
            entity.Property(e => e.SupplyStatusid).HasColumnName("supply_statusid");
            entity.Property(e => e.TotalCost)
                .HasPrecision(12, 2)
                .HasColumnName("total_cost");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Supplies)
                .HasForeignKey(d => d.Supplierid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fksupply26053");

            entity.HasOne(d => d.SupplyStatus).WithMany(p => p.Supplies)
                .HasForeignKey(d => d.SupplyStatusid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fksupply23249");
        });

        modelBuilder.Entity<SupplyItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("supply_item_pkey");

            entity.ToTable("supply_item");

            entity.HasIndex(e => e.Bookid, "idx_supply_item_bookid");

            entity.HasIndex(e => e.PurchaseRequestItemid, "idx_supply_item_request_itemid");

            entity.HasIndex(e => e.SupplyItemStatusid, "idx_supply_item_statusid");

            entity.HasIndex(e => e.Supplyid, "idx_supply_item_supplyid");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Bookid).HasColumnName("bookid");
            entity.Property(e => e.PricePerUnit)
                .HasPrecision(10, 2)
                .HasColumnName("price_per_unit");
            entity.Property(e => e.PurchaseRequestItemid).HasColumnName("purchase_request_itemid");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.SupplyItemStatusid).HasColumnName("supply_item_statusid");
            entity.Property(e => e.Supplyid).HasColumnName("supplyid");

            entity.HasOne(d => d.Book).WithMany(p => p.SupplyItems)
                .HasForeignKey(d => d.Bookid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fksupply_ite528158");

            entity.HasOne(d => d.PurchaseRequestItem).WithMany(p => p.SupplyItems)
                .HasForeignKey(d => d.PurchaseRequestItemid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fksupply_ite368767");

            entity.HasOne(d => d.SupplyItemStatus).WithMany(p => p.SupplyItems)
                .HasForeignKey(d => d.SupplyItemStatusid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fksupply_ite653301");

            entity.HasOne(d => d.Supply).WithMany(p => p.SupplyItems)
                .HasForeignKey(d => d.Supplyid)
                .HasConstraintName("fksupply_ite394142");
        });

        modelBuilder.Entity<SupplyItemStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("supply_item_status_pkey");

            entity.ToTable("supply_item_status");

            entity.HasIndex(e => e.Name, "supply_item_status_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<SupplyStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("supply_status_pkey");

            entity.ToTable("supply_status");

            entity.HasIndex(e => e.Name, "supply_status_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
