using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Entity.GoodjobResum
{
    public partial class Goodjob_resumeContext : DbContext
    {
        public Goodjob_resumeContext()
        {
        }

        public Goodjob_resumeContext(DbContextOptions<Goodjob_resumeContext> options)
            : base(options)
        {
        }

        public virtual DbSet<MemPageUrlPath> MemPageUrlPaths { get; set; } = null!;
        public virtual DbSet<MemPaymentOrder> MemPaymentOrders { get; set; } = null!;
        public virtual DbSet<MemPhoto> MemPhotos { get; set; } = null!;
        public virtual DbSet<MemPosJobFunction> MemPosJobFunctions { get; set; } = null!;
        public virtual DbSet<MemPosJobLocation> MemPosJobLocations { get; set; } = null!;
        public virtual DbSet<MemPosUpdateLog> MemPosUpdateLogs { get; set; } = null!;
        public virtual DbSet<MemPosition> MemPositions { get; set; } = null!;
        public virtual DbSet<MemProduct> MemProducts { get; set; } = null!;
        public virtual DbSet<MemProductCatalog> MemProductCatalogs { get; set; } = null!;
        public virtual DbSet<MemProductUser> MemProductUsers { get; set; } = null!;
        public virtual DbSet<MemProductUserLog> MemProductUserLogs { get; set; } = null!;
        public virtual DbSet<MemRemarkList> MemRemarkLists { get; set; } = null!;
        public virtual DbSet<MemSearcher> MemSearchers { get; set; } = null!;
        public virtual DbSet<MemServiceOrder> MemServiceOrders { get; set; } = null!;
        public virtual DbSet<MemUser> MemUsers { get; set; } = null!;
        public virtual DbSet<MngAd> MngAds { get; set; } = null!;
        public virtual DbSet<MngAdClass> MngAdClasses { get; set; } = null!;
        public virtual DbSet<MngAreaSite> MngAreaSites { get; set; } = null!;
        public virtual DbSet<MngCheckingResume> MngCheckingResumes { get; set; } = null!;
        public virtual DbSet<MngHtmlLabel> MngHtmlLabels { get; set; } = null!;
        public virtual DbSet<MngHtmlTemplate> MngHtmlTemplates { get; set; } = null!;
        public virtual DbSet<MngPartnerContent> MngPartnerContents { get; set; } = null!;
        public virtual DbSet<MngPartnerPayCompany> MngPartnerPayCompanies { get; set; } = null!;
        public virtual DbSet<MngPartnerUser> MngPartnerUsers { get; set; } = null!;
        public virtual DbSet<MngPopSite> MngPopSites { get; set; } = null!;
        public virtual DbSet<MyEducation> MyEducations { get; set; } = null!;
        public virtual DbSet<MyEducationEng> MyEducationEngs { get; set; } = null!;
        public virtual DbSet<MyFavouriate> MyFavouriates { get; set; } = null!;
        public virtual DbSet<MyFulfil> MyFulfils { get; set; } = null!;
        public virtual DbSet<MyFulfilEng> MyFulfilEngs { get; set; } = null!;
        public virtual DbSet<MyJobFunction> MyJobFunctions { get; set; } = null!;
        public virtual DbSet<MyJobLocation> MyJobLocations { get; set; } = null!;
        public virtual DbSet<MyLanguage> MyLanguages { get; set; } = null!;
        public virtual DbSet<MyLanguageEng> MyLanguageEngs { get; set; } = null!;
        public virtual DbSet<MyLetter> MyLetters { get; set; } = null!;
        public virtual DbSet<MyProject> MyProjects { get; set; } = null!;
        public virtual DbSet<MyProjectEng> MyProjectEngs { get; set; } = null!;
        public virtual DbSet<MyResume> MyResumes { get; set; } = null!;
        public virtual DbSet<MyResumeEng> MyResumeEngs { get; set; } = null!;
        public virtual DbSet<MyResumeOldText> MyResumeOldTexts { get; set; } = null!;
        public virtual DbSet<MySearcher> MySearchers { get; set; } = null!;
        public virtual DbSet<MySendPhoneCount> MySendPhoneCounts { get; set; } = null!;
        public virtual DbSet<MySendResumeHistory> MySendResumeHistories { get; set; } = null!;
        public virtual DbSet<MyTraining> MyTrainings { get; set; } = null!;
        public virtual DbSet<MyTrainingEng> MyTrainingEngs { get; set; } = null!;
        public virtual DbSet<MyUploadFile> MyUploadFiles { get; set; } = null!;
        public virtual DbSet<MyUser> MyUsers { get; set; } = null!;
        public virtual DbSet<MyUserIdBack> MyUserIdBacks { get; set; } = null!;
        public virtual DbSet<MyWorkExp> MyWorkExps { get; set; } = null!;
        public virtual DbSet<MyWorkExpEng> MyWorkExpEngs { get; set; } = null!;
        public virtual DbSet<PubHelpItem> PubHelpItems { get; set; } = null!;
        public virtual DbSet<PubHelpTopic> PubHelpTopics { get; set; } = null!;
        public virtual DbSet<PubInformation> PubInformations { get; set; } = null!;
        public virtual DbSet<PubInformationMy> PubInformationMies { get; set; } = null!;
        public virtual DbSet<PubInterviewing> PubInterviewings { get; set; } = null!;
        public virtual DbSet<PubPerApplyPo> PubPerApplyPos { get; set; } = null!;
        public virtual DbSet<PubResumeViewLog> PubResumeViewLogs { get; set; } = null!;
        public virtual DbSet<PubServiceHelp> PubServiceHelps { get; set; } = null!;
        public virtual DbSet<RecomMyuserId> RecomMyuserIds { get; set; } = null!;
        public virtual DbSet<SetArea> SetAreas { get; set; } = null!;
        public virtual DbSet<SetLevel> SetLevels { get; set; } = null!;
        public virtual DbSet<SiteType> SiteTypes { get; set; } = null!;
        public virtual DbSet<SysExcludeIncompleteMyId> SysExcludeIncompleteMyIds { get; set; } = null!;
        public virtual DbSet<SysExcludeMyId> SysExcludeMyIds { get; set; } = null!;
        public virtual DbSet<SysGetIncompleteName> SysGetIncompleteNames { get; set; } = null!;
        public virtual DbSet<SysGetName> SysGetNames { get; set; } = null!;
        public virtual DbSet<SysGetPosName> SysGetPosNames { get; set; } = null!;
        public virtual DbSet<SysGetPosNameAll> SysGetPosNameAlls { get; set; } = null!;
        public virtual DbSet<SysKeyWord> SysKeyWords { get; set; } = null!;
        public virtual DbSet<SysRamdomExcludeMyId> SysRamdomExcludeMyIds { get; set; } = null!;
        public virtual DbSet<SysRandomGetName> SysRandomGetNames { get; set; } = null!;
        public virtual DbSet<TableMaxId> TableMaxIds { get; set; } = null!;
        public virtual DbSet<TempId> TempIds { get; set; } = null!;
        public virtual DbSet<TemplateSet> TemplateSets { get; set; } = null!;
        public virtual DbSet<TemplateType> TemplateTypes { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=192.168.3.2;Initial Catalog=Goodjob_resume;User ID=goodjobjishu;Password=juncaiwang/*-;Encrypt=False;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MemPageUrlPath>(entity =>
            {
                entity.HasKey(e => e.MemId);

                entity.ToTable("Mem_PageUrlPath");

                entity.Property(e => e.MemId)
                    .ValueGeneratedNever()
                    .HasColumnName("MemID");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UrlPath)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MemPaymentOrder>(entity =>
            {
                entity.HasKey(e => e.OrderId);

                entity.ToTable("Mem_PaymentOrder");

                entity.HasIndex(e => e.MemId, "IX_Mem_PaymentOrder_MemID");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.Description)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.DoDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DoUserId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("DoUserID")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.MemId).HasColumnName("MemID");

                entity.Property(e => e.PayDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PayPerson)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MemPhoto>(entity =>
            {
                entity.ToTable("Mem_Photo");

                entity.HasIndex(e => new { e.MemId, e.Display }, "IX_Mem_Photo_memdis");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.FilePath)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.InsertDate)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.MemId).HasColumnName("MemID");

                entity.Property(e => e.Memo)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Type).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<MemPosJobFunction>(entity =>
            {
                entity.ToTable("Mem_PosJobFunction");

                entity.HasIndex(e => e.PosId, "IX_Mem_PosJobFunction_PosID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.JobFunctionBig).HasColumnName("JobFunction_big");

                entity.Property(e => e.JobFunctionSmall).HasColumnName("JobFunction_small");

                entity.Property(e => e.PosId).HasColumnName("PosID");
            });

            modelBuilder.Entity<MemPosJobLocation>(entity =>
            {
                entity.ToTable("Mem_PosJobLocation");

                entity.HasIndex(e => e.PosId, "IX_Mem_PosJobLocation_PosID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.JobLocationC).HasColumnName("JobLocation_C");

                entity.Property(e => e.JobLocationP).HasColumnName("JobLocation_P");

                entity.Property(e => e.PosId).HasColumnName("PosID");
            });

            modelBuilder.Entity<MemPosUpdateLog>(entity =>
            {
                entity.ToTable("Mem_PosUpdateLog");

                entity.HasIndex(e => e.PosId, "IX_Mem_PosUpdateLog_PosID");

                entity.HasIndex(e => e.Updatetime, "IX_Mem_PosUpdateLog_Updatetime");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.PosId).HasColumnName("PosID");

                entity.Property(e => e.Updatetime)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<MemPosition>(entity =>
            {
                entity.HasKey(e => e.PosId);

                entity.ToTable("Mem_Position");

                entity.HasIndex(e => e.EndValidDate, "IX_Mem_Position_Endvalidate");

                entity.HasIndex(e => e.MemId, "IX_Mem_Position_MemID");

                entity.HasIndex(e => e.PosState, "IX_Mem_Position_PosState");

                entity.HasIndex(e => e.PostDate, "IX_Mem_Position_PostDate");

                entity.Property(e => e.PosId)
                    .ValueGeneratedNever()
                    .HasColumnName("PosID");

                entity.Property(e => e.Address)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.AddressC).HasColumnName("Address_C");

                entity.Property(e => e.AddressP).HasColumnName("Address_P");

                entity.Property(e => e.ContactFax)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ContactFaxE)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ContactFax_E")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ContactFaxZ)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ContactFax_Z")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ContactPerson)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ContactTel)
                    .HasMaxLength(30)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ContactTelE)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ContactTel_E")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ContactTelZ)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ContactTel_Z")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.DeptId).HasColumnName("DeptID");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.EndValidDate)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ExamAddress)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ExamNotice)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Geyn).HasColumnName("GEYN");

                entity.Property(e => e.JobLocation)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.MemId).HasColumnName("MemID");

                entity.Property(e => e.MemName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.PosName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Posdecription)
                    .HasMaxLength(1800)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.PostDate)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RegHometownC).HasColumnName("RegHometown_C");

                entity.Property(e => e.RegHometownP).HasColumnName("RegHometown_P");

                entity.Property(e => e.RegisterDate)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ReqDegreeId).HasColumnName("ReqDegreeID");

                entity.Property(e => e.TagLib)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ZipCode)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<MemProduct>(entity =>
            {
                entity.HasKey(e => e.ProductId);

                entity.ToTable("Mem_Product");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.CatalogId).HasColumnName("CatalogID");

                entity.Property(e => e.Description)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ImageUrl)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.IssueDate)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.MemId).HasColumnName("MemID");

                entity.Property(e => e.Price).HasColumnType("money");

                entity.Property(e => e.ProductName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ProductTypeId).HasColumnName("ProductTypeID");
            });

            modelBuilder.Entity<MemProductCatalog>(entity =>
            {
                entity.HasKey(e => e.CatalogId);

                entity.ToTable("Mem_ProductCatalog");

                entity.Property(e => e.CatalogId).HasColumnName("CatalogID");

                entity.Property(e => e.CatalogName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MemId).HasColumnName("MemID");
            });

            modelBuilder.Entity<MemProductUser>(entity =>
            {
                entity.HasKey(e => e.MemId);

                entity.ToTable("Mem_ProductUser");

                entity.Property(e => e.MemId)
                    .ValueGeneratedNever()
                    .HasColumnName("MemID");

                entity.Property(e => e.BeginDate).HasColumnType("smalldatetime");

                entity.Property(e => e.EndDate).HasColumnType("smalldatetime");

                entity.Property(e => e.OpenDate)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<MemProductUserLog>(entity =>
            {
                entity.ToTable("Mem_ProductUserLog");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BeginDate).HasColumnType("smalldatetime");

                entity.Property(e => e.EndDate).HasColumnType("smalldatetime");

                entity.Property(e => e.MemId).HasColumnName("MemID");

                entity.Property(e => e.OpenDate)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.UserIp)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("UserIP")
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<MemRemarkList>(entity =>
            {
                entity.ToTable("Mem_Remark_List");

                entity.HasIndex(e => new { e.MemId, e.MyUserId }, "IX_Mem_Remark_List_MemIDUserID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.MarkMemo)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.MarkTitle)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.MarkUser)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.MemId).HasColumnName("MemID");

                entity.Property(e => e.MyUserId).HasColumnName("MyUserID");
            });

            modelBuilder.Entity<MemSearcher>(entity =>
            {
                entity.ToTable("Mem_Searcher");

                entity.HasIndex(e => e.MemId, "IX_Mem_Searcher_MemID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.HomeTownC).HasColumnName("HomeTown_C");

                entity.Property(e => e.HomeTownP).HasColumnName("HomeTown_P");

                entity.Property(e => e.JobFunction)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.JobLocation)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.KeyWord)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.LanguageId)
                    .HasColumnName("LanguageID")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.LanguageLevel).HasDefaultValueSql("('')");

                entity.Property(e => e.LastPosName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.LocationC).HasColumnName("Location_C");

                entity.Property(e => e.LocationP).HasColumnName("Location_P");

                entity.Property(e => e.MemId).HasColumnName("MemID");

                entity.Property(e => e.ReqDegreeId1).HasColumnName("ReqDegreeID1");

                entity.Property(e => e.ReqDegreeId2).HasColumnName("ReqDegreeID2");

                entity.Property(e => e.SchoolName)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.SearcherName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Speciality)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<MemServiceOrder>(entity =>
            {
                entity.HasKey(e => e.OrderId);

                entity.ToTable("Mem_ServiceOrder");

                entity.HasIndex(e => e.MemId, "IX_Mem_ServiceOrder_MemID");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.ApplyDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ApplyPerson)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Detail)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.DoDate).HasColumnType("datetime");

                entity.Property(e => e.DoUserId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("DoUserID")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ItemId).HasColumnName("ItemID");

                entity.Property(e => e.MemId).HasColumnName("MemID");
            });

            modelBuilder.Entity<MemUser>(entity =>
            {
                entity.HasKey(e => e.MemId);

                entity.ToTable("Mem_Users");

                entity.HasIndex(e => e.CheckFlag, "IX_Mem_Users_CheckFlag");

                entity.HasIndex(e => e.EndValidDate, "IX_Mem_Users_EndDate");

                entity.HasIndex(e => e.MemberClass, "IX_Mem_Users_MemberClass");

                entity.HasIndex(e => e.RegisterBy, "IX_Mem_Users_RegisterBy");

                entity.HasIndex(e => e.RegisterDate, "IX_Mem_Users_Registerdate");

                entity.HasIndex(e => new { e.UpdateDate, e.PassWord }, "IX_Mem_Users_USPass");

                entity.Property(e => e.MemId)
                    .ValueGeneratedNever()
                    .HasColumnName("MemID");

                entity.Property(e => e.BeginValidDate)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CheckDate)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CheckUserId).HasColumnName("CheckUserID");

                entity.Property(e => e.DateMaxViewNum).HasDefaultValueSql("((300))");

                entity.Property(e => e.EndValidDate)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastLoginDate)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastLoginIp)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("LastLoginIP")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.MaxPosNum).HasDefaultValueSql("((5))");

                entity.Property(e => e.PassWord)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.RegisterDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.SalerEmail)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.SalerTel)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.SalerUserId).HasColumnName("SalerUserID");

                entity.Property(e => e.SalerUserName)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<MngAd>(entity =>
            {
                entity.ToTable("Mng_Ad");

                entity.HasIndex(e => e.AddressC, "IX_Mng_Ad_AddressC");

                entity.HasIndex(e => e.AddressP, "IX_Mng_Ad_AddressP");

                entity.HasIndex(e => e.AreaSite, "IX_Mng_Ad_Areasite");

                entity.HasIndex(e => e.ClassId, "IX_Mng_Ad_ClassID");

                entity.HasIndex(e => e.Disabled, "IX_Mng_Ad_Disabled");

                entity.HasIndex(e => e.MemId, "IX_Mng_Ad_MemID");

                entity.HasIndex(e => e.PartnerSite, "IX_Mng_Ad_PartnerSite");

                entity.HasIndex(e => e.PopSite, "IX_Mng_Ad_PopSite");

                entity.HasIndex(e => e.TopValue, "IX_Mng_Ad_TopValue");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AddTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.AddressC).HasColumnName("Address_C");

                entity.Property(e => e.AddressP).HasColumnName("Address_P");

                entity.Property(e => e.AreaSite)
                    .HasMaxLength(400)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.BeginDate)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ClassId).HasColumnName("ClassID");

                entity.Property(e => e.DevelopSite)
                    .HasMaxLength(400)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Logo)
                    .HasMaxLength(200)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.MemId)
                    .HasMaxLength(50)
                    .HasColumnName("MemID")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.PartnerSite)
                    .HasMaxLength(400)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.PopSite)
                    .HasMaxLength(400)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Remark)
                    .HasMaxLength(200)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.SpaceCount).HasDefaultValueSql("((1))");

                entity.Property(e => e.Url)
                    .HasMaxLength(200)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<MngAdClass>(entity =>
            {
                entity.ToTable("Mng_AdClass");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.ClassCode)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ClassInfo)
                    .HasMaxLength(200)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ClassName)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<MngAreaSite>(entity =>
            {
                entity.ToTable("Mng_AreaSite");

                entity.HasIndex(e => e.CityId, "IX_Mng_AreaSite_CityID");

                entity.HasIndex(e => e.Disabled, "IX_Mng_AreaSite_Disabled");

                entity.HasIndex(e => e.ProvinceId, "IX_Mng_AreaSite_ProvID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CityId).HasColumnName("CityID");

                entity.Property(e => e.Ename)
                    .HasMaxLength(50)
                    .HasColumnName("EName")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Pname)
                    .HasMaxLength(50)
                    .HasColumnName("PName")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ProvinceId).HasColumnName("ProvinceID");
            });

            modelBuilder.Entity<MngCheckingResume>(entity =>
            {
                entity.ToTable("Mng_CheckingResume");

                entity.HasIndex(e => e.MyUserId, "IX_Mng_CheckingResume_MyUserID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.LocationC).HasColumnName("Location_C");

                entity.Property(e => e.LocationP).HasColumnName("Location_P");

                entity.Property(e => e.MyUserId).HasColumnName("MyUserID");

                entity.Property(e => e.PerName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.RegisterDate)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<MngHtmlLabel>(entity =>
            {
                entity.ToTable("Mng_HtmlLabel");

                entity.HasIndex(e => e.AutoRefresh, "IX_Mng_HtmlLabel_AutoRef");

                entity.HasIndex(e => e.Disabled, "IX_Mng_HtmlLabel_Ds");

                entity.HasIndex(e => e.TemplateId, "IX_Mng_HtmlLabel_Tl");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.AddTime)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.AreaSiteConditionField)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CityConditionField)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ClassNameField)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CountField)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.DataCondition)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.DataGroup)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.DataOrder)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.DataTable)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.DevelopSiteConditionField)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.IdField)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ImageField)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.LabelName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.OtherField)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ParentId).HasColumnName("ParentID");

                entity.Property(e => e.PartnerSiteConditionField)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.PopSiteConditionField)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ProvinceConditionField)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.TemplateId).HasColumnName("TemplateID");

                entity.Property(e => e.TimeField)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.TitleField)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<MngHtmlTemplate>(entity =>
            {
                entity.ToTable("Mng_HtmlTemplate");

                entity.HasIndex(e => e.CreateType, "IX_Mng_HtmlTemplate_CT");

                entity.HasIndex(e => e.Disabled, "IX_Mng_HtmlTemplate_DS");

                entity.HasIndex(e => e.TemplateType, "IX_Mng_HtmlTemplate_TP");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.AddTime)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Keyword)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.MasterPagePath)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.SaveEncoding)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.SaveFilePath)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.TemplateName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.TemplatePath)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Title)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<MngPartnerContent>(entity =>
            {
                entity.ToTable("Mng_PartnerContent");

                entity.HasIndex(e => new { e.FunctionBig, e.FunctionSmall }, "IX_Mng_PartnerContent_Function");

                entity.HasIndex(e => e.IndustryId, "IX_Mng_PartnerContent_IndustryID");

                entity.HasIndex(e => new { e.LocationP, e.LocationC }, "IX_Mng_PartnerContent_location");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.FunctionBig).HasColumnName("Function_Big");

                entity.Property(e => e.FunctionSmall).HasColumnName("Function_Small");

                entity.Property(e => e.IndustryId).HasColumnName("IndustryID");

                entity.Property(e => e.LocationC).HasColumnName("Location_C");

                entity.Property(e => e.LocationP).HasColumnName("Location_P");

                entity.Property(e => e.ProductTypeId).HasColumnName("ProductTypeID");

                entity.Property(e => e.SiteId).HasColumnName("SiteID");
            });

            modelBuilder.Entity<MngPartnerPayCompany>(entity =>
            {
                entity.ToTable("Mng_PartnerPayCompany");

                entity.HasIndex(e => e.IsDelete, "IX_Mng_PartnerPayCompany_ISD");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BeginDate).HasColumnType("smalldatetime");

                entity.Property(e => e.EndDate).HasColumnType("smalldatetime");

                entity.Property(e => e.MemId).HasColumnName("MemID");

                entity.Property(e => e.PartnerId).HasColumnName("PartnerID");

                entity.Property(e => e.PayDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.SiteId).HasColumnName("SiteID");
            });

            modelBuilder.Entity<MngPartnerUser>(entity =>
            {
                entity.HasKey(e => e.PartnerId);

                entity.ToTable("Mng_PartnerUser");

                entity.HasIndex(e => e.Disabled, "IX_Mng_PartnerUser_DiB");

                entity.HasIndex(e => e.SiteId, "IX_Mng_PartnerUser_SID");

                entity.Property(e => e.PartnerId)
                    .ValueGeneratedNever()
                    .HasColumnName("PartnerID");

                entity.Property(e => e.DisableDate).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.FolderName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Keyword)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.LastLoginDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastLoginIp)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("LastLoginIP")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.LocationC).HasColumnName("Location_C");

                entity.Property(e => e.LocationP).HasColumnName("Location_P");

                entity.Property(e => e.Opened)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.PartnerName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.RegisterDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.SiteDomain)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.SiteId).HasColumnName("SiteID");

                entity.Property(e => e.SiteName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<MngPopSite>(entity =>
            {
                entity.ToTable("Mng_PopSite");

                entity.HasIndex(e => e.Disabled, "IX_Mng_PopSite_Disable");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.Domain)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FolderName)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.SiteName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MyEducation>(entity =>
            {
                entity.ToTable("My_Education");

                entity.HasIndex(e => new { e.MyuserId, e.BeginDate }, "IX_My_Education_MyUserID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BeginDate)
                    .HasMaxLength(7)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Certificate)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.DegreeId).HasColumnName("DegreeID");

                entity.Property(e => e.EndDate)
                    .HasMaxLength(7)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.MyuserId).HasColumnName("MyuserID");

                entity.Property(e => e.SchoolName)
                    .HasMaxLength(30)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Speciality)
                    .HasMaxLength(30)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.SpecialityMemo)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<MyEducationEng>(entity =>
            {
                entity.ToTable("My_Education_Eng");

                entity.HasIndex(e => new { e.MyuserId, e.BeginDate }, "IX_My_Education_Eng_MyUserID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BeginDate)
                    .HasMaxLength(7)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Certificate)
                    .HasMaxLength(100)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.DegreeId).HasColumnName("DegreeID");

                entity.Property(e => e.EndDate)
                    .HasMaxLength(7)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.MyuserId).HasColumnName("MyuserID");

                entity.Property(e => e.SchoolName)
                    .HasMaxLength(100)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Speciality)
                    .HasMaxLength(100)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.SpecialityMemo)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<MyFavouriate>(entity =>
            {
                entity.ToTable("My_Favouriate");

                entity.HasIndex(e => e.MyUserId, "IX_My_Favouriate_MyUserID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.InsertDate)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.MyUserId).HasColumnName("MyUserID");

                entity.Property(e => e.Note)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.PosId).HasColumnName("PosID");
            });

            modelBuilder.Entity<MyFulfil>(entity =>
            {
                entity.ToTable("My_Fulfil");

                entity.HasIndex(e => e.MyUserId, "IX_My_Fulfil_MyUserID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BeginDate)
                    .HasMaxLength(7)
                    .IsUnicode(false);

                entity.Property(e => e.ComName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasColumnType("text")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.EndDate)
                    .HasMaxLength(7)
                    .IsUnicode(false);

                entity.Property(e => e.MyUserId).HasColumnName("MyUserID");
            });

            modelBuilder.Entity<MyFulfilEng>(entity =>
            {
                entity.ToTable("My_Fulfil_Eng");

                entity.HasIndex(e => e.MyUserId, "IX_My_Fulfil_Eng_MyUserID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BeginDate)
                    .HasMaxLength(7)
                    .IsUnicode(false);

                entity.Property(e => e.ComName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasColumnType("text")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.EndDate)
                    .HasMaxLength(7)
                    .IsUnicode(false);

                entity.Property(e => e.MyUserId).HasColumnName("MyUserID");
            });

            modelBuilder.Entity<MyJobFunction>(entity =>
            {
                entity.ToTable("My_JobFunction");

                entity.HasIndex(e => e.JobFunctionBig, "IX_My_JobFunction_JFB");

                entity.HasIndex(e => e.JobFunctionSmall, "IX_My_JobFunction_JFS");

                entity.HasIndex(e => e.MyUserId, "IX_My_JobFunction_MyUserID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.JobFunctionBig).HasColumnName("JobFunction_big");

                entity.Property(e => e.JobFunctionSmall).HasColumnName("JobFunction_small");

                entity.Property(e => e.MyUserId).HasColumnName("MyUserID");
            });

            modelBuilder.Entity<MyJobLocation>(entity =>
            {
                entity.ToTable("My_JobLocation");

                entity.HasIndex(e => e.JobLocationP, "IX_My_JobLocation_JobLoca");

                entity.HasIndex(e => e.JobLocationC, "IX_My_JobLocation_JobLoca_C");

                entity.HasIndex(e => e.MyUserId, "IX_My_JobLocation_MyUserID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.JobLocationC).HasColumnName("JobLocation_C");

                entity.Property(e => e.JobLocationP).HasColumnName("JobLocation_P");

                entity.Property(e => e.MyUserId).HasColumnName("MyUserID");
            });

            modelBuilder.Entity<MyLanguage>(entity =>
            {
                entity.ToTable("My_Language");

                entity.HasIndex(e => e.LanguageId, "IX_My_Language_LanID");

                entity.HasIndex(e => e.LanguageLevel, "IX_My_Language_LanLevel");

                entity.HasIndex(e => e.MyUserId, "IX_My_Language_MyUserID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.LanguageDescription)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.LanguageId).HasColumnName("LanguageID");

                entity.Property(e => e.MyUserId).HasColumnName("MyUserID");
            });

            modelBuilder.Entity<MyLanguageEng>(entity =>
            {
                entity.ToTable("My_Language_Eng");

                entity.HasIndex(e => e.MyUserId, "IX_My_Language_Eng_MyUID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.LanguageDescription)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.LanguageId).HasColumnName("LanguageID");

                entity.Property(e => e.MyUserId).HasColumnName("MyUserID");
            });

            modelBuilder.Entity<MyLetter>(entity =>
            {
                entity.ToTable("My_Letter");

                entity.HasIndex(e => e.MyUserId, "IX_My_Letter_MyUID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.MyLetter1)
                    .HasColumnType("text")
                    .HasColumnName("MyLetter");

                entity.Property(e => e.MyUserId).HasColumnName("MyUserID");

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MyProject>(entity =>
            {
                entity.ToTable("My_Project");

                entity.HasIndex(e => e.MyUserId, "IX_My_Project_MyUID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BeginDate).HasMaxLength(7);

                entity.Property(e => e.EndDate).HasMaxLength(7);

                entity.Property(e => e.MyUserId).HasColumnName("MyUserID");

                entity.Property(e => e.ProjectDuty).HasColumnType("text");

                entity.Property(e => e.ProjectMemo).HasColumnType("text");

                entity.Property(e => e.ProjectName).HasMaxLength(30);
            });

            modelBuilder.Entity<MyProjectEng>(entity =>
            {
                entity.ToTable("My_Project_Eng");

                entity.HasIndex(e => e.MyUserId, "IX_My_Project_Eng_MyUID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BeginDate).HasMaxLength(7);

                entity.Property(e => e.EndDate).HasMaxLength(7);

                entity.Property(e => e.MyUserId).HasColumnName("MyUserID");

                entity.Property(e => e.ProjectDuty).HasColumnType("text");

                entity.Property(e => e.ProjectMemo).HasColumnType("text");

                entity.Property(e => e.ProjectName).HasMaxLength(30);
            });

            modelBuilder.Entity<MyResume>(entity =>
            {
                entity.HasKey(e => e.MyUserId);

                entity.ToTable("My_Resume");

                entity.HasIndex(e => e.CheckFlag, "IX_My_Resume_CheckFlag");

                entity.HasIndex(e => e.ResumeStatus, "IX_My_Resume_Status");

                entity.HasIndex(e => e.UpdateDate, "IX_UpdateDate");

                entity.HasIndex(e => e.PerName, "Ix_My_Resume_PerName");

                entity.Property(e => e.MyUserId)
                    .ValueGeneratedNever()
                    .HasColumnName("MyUserID")
                    .HasComment("个人账号编号");

                entity.Property(e => e.Address)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasComment("通讯地址");

                entity.Property(e => e.AllCount).HasComment("诚信分");

                entity.Property(e => e.AllWrite).HasComment("整体完整性\r\n0未填写完成，1基本完整，2很完整\r\n基本完整：除项目、证书、肖像photodisplay、语言以外的都填写\r\n很完整就要全部填写");

                entity.Property(e => e.BaseWrite).HasComment("基本信息完整\r\n0未填写，1已填写");

                entity.Property(e => e.Birthday)
                    .HasColumnType("smalldatetime")
                    .HasComment("出生日期");

                entity.Property(e => e.CardNum)
                    .HasMaxLength(18)
                    .HasDefaultValueSql("('')")
                    .HasComment("证件号码");

                entity.Property(e => e.CardType).HasComment("证件类型");

                entity.Property(e => e.CareerDirection)
                    .HasColumnType("text")
                    .HasDefaultValueSql("('')")
                    .HasComment("发展方向");

                entity.Property(e => e.CertWrite).HasComment("证书有上传\r\n0未填写，1已填写");

                entity.Property(e => e.CheckDate)
                    .HasColumnType("smalldatetime")
                    .HasComment("审核日期");

                entity.Property(e => e.CheckFlag).HasComment("审核标志\r\n1=待审核，2=审核通过，3=审核不通过，4=更新后待复审");

                entity.Property(e => e.CheckinDate).HasComment("到职日期");

                entity.Property(e => e.ComputerLevel)
                    .HasDefaultValueSql("('')")
                    .HasComment("计算机能力");

                entity.Property(e => e.ComputerSkills)
                    .HasColumnType("text")
                    .HasDefaultValueSql("('')")
                    .HasComment("计算机技能描述");

                entity.Property(e => e.ContactPhone)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('')")
                    .HasComment("联系电话");

                entity.Property(e => e.DegreeId)
                    .HasColumnName("DegreeID")
                    .HasComment("学历\r\n0=无，70=博士，60=硕士，50=本科，40=大专，30=中专，20=高中");

                entity.Property(e => e.EatHouseNeeded)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('')")
                    .HasComment("食宿要求");

                entity.Property(e => e.EduWrite).HasComment("教育经验有填写\r\n0未填写，1已填写");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('')")
                    .HasComment("电子邮箱(和帐号表一样！要同步更新)");

                entity.Property(e => e.Height).HasComment("身高");

                entity.Property(e => e.Homepage)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasComment("个人主页");

                entity.Property(e => e.HometownC)
                    .HasColumnName("Hometown_C")
                    .HasComment("户籍（市）");

                entity.Property(e => e.HometownP)
                    .HasColumnName("Hometown_P")
                    .HasComment("户籍（省）");

                entity.Property(e => e.HopeWrite).HasComment("意向填写完整\r\n0未填写，1已填写");

                entity.Property(e => e.Imnum)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("IMNum")
                    .HasDefaultValueSql("('')")
                    .HasComment("即时通讯号码");

                entity.Property(e => e.Interesting)
                    .HasColumnType("text")
                    .HasDefaultValueSql("('')")
                    .HasComment("兴趣爱好(老系统无，新加、显示时注意为空就不显)");

                entity.Property(e => e.JobSeeking)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasComment("寻求职位描述(就是以前填写的三项加一起)");

                entity.Property(e => e.JobType).HasComment("求职类型");

                entity.Property(e => e.LanguageWrite).HasComment("语言有填写\r\n0未填写，1已填写");

                entity.Property(e => e.LocationC)
                    .HasColumnName("Location_C")
                    .HasComment("现所在地（市）");

                entity.Property(e => e.LocationP)
                    .HasColumnName("Location_P")
                    .HasComment("现所在地（省）");

                entity.Property(e => e.LocationT).HasColumnName("Location_T");

                entity.Property(e => e.MaritalStatus)
                    .HasDefaultValueSql("((1))")
                    .HasComment("婚姻状况\r\n0=未婚，1=已婚，2=离异，3=保密");

                entity.Property(e => e.MobileNum)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('')")
                    .HasComment("移动电话");

                entity.Property(e => e.Nationality).HasComment("民族");

                entity.Property(e => e.OtherNeeded)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasComment("其它要求");

                entity.Property(e => e.OtherSkills)
                    .HasColumnType("text")
                    .HasDefaultValueSql("('')")
                    .HasComment("其它技能");

                entity.Property(e => e.PartComName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.PerName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasComment("姓名");

                entity.Property(e => e.PhotoDisplay).HasComment("照片显示\r\n0不存在，1不显示，2显示");

                entity.Property(e => e.PhotoName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasComment("肖像照片的名称");

                entity.Property(e => e.PositionName)
                    .HasMaxLength(15)
                    .HasDefaultValueSql("('')")
                    .HasComment("职称");

                entity.Property(e => e.ProjectWrite).HasComment("项目有填写\r\n0未填写，1已填写");

                entity.Property(e => e.RegisterDate)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("注册日期（简历的注册日期，不同于帐号表中的）");

                entity.Property(e => e.ResumeStatus).HasComment("简历的状态\r\n0=正常，1=隐藏简历，2=对指定的企业隐藏，3=待删除（独立功能操作）");

                entity.Property(e => e.Salary).HasComment("薪金要求");

                entity.Property(e => e.SelfDescription)
                    .HasColumnType("text")
                    .HasDefaultValueSql("('')")
                    .HasComment("自我评价");

                entity.Property(e => e.Sex).HasComment("性别\r\n0=不限，1=男，2=女 （增加“不限”的目的是与其它表相应字段进行配合）");

                entity.Property(e => e.SkillWrite).HasComment("技能专长完整");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("更新日期（同时更新帐号表中的更新日期）");

                entity.Property(e => e.WorkWrite).HasComment("工作经验有填写\r\n0未填写，1已填写");

                entity.Property(e => e.WorkedMonth).HasComment("曾工作的月份");

                entity.Property(e => e.WorkedYear).HasComment("曾工作的年份");

                entity.Property(e => e.ZipCode)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('')")
                    .HasComment("邮政编码");
            });

            modelBuilder.Entity<MyResumeEng>(entity =>
            {
                entity.HasKey(e => e.MyUserId);

                entity.ToTable("My_Resume_Eng");

                entity.Property(e => e.MyUserId)
                    .ValueGeneratedNever()
                    .HasColumnName("MyUserID")
                    .HasComment("个人账号编号");

                entity.Property(e => e.Address)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Birthday).HasColumnType("smalldatetime");

                entity.Property(e => e.CardNum)
                    .HasMaxLength(18)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CareerDirection)
                    .HasColumnType("text")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CheckDate).HasColumnType("smalldatetime");

                entity.Property(e => e.ComputerLevel).HasDefaultValueSql("('')");

                entity.Property(e => e.ComputerSkills)
                    .HasColumnType("text")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ContactPhone)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.DegreeId).HasColumnName("DegreeID");

                entity.Property(e => e.EatHouseNeeded)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Email)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Homepage)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.HometownC).HasColumnName("Hometown_C");

                entity.Property(e => e.HometownP).HasColumnName("Hometown_P");

                entity.Property(e => e.Imnum)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("IMNum")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Interesting)
                    .HasColumnType("text")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.JobSeeking)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.LocationC).HasColumnName("Location_C");

                entity.Property(e => e.LocationP).HasColumnName("Location_P");

                entity.Property(e => e.MobileNum)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.OtherNeeded)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.OtherSkills)
                    .HasColumnType("text")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.PartComName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.PerName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.PhotoName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.PositionName)
                    .HasMaxLength(15)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.RegisterDate)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.SelfDescription)
                    .HasColumnType("text")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ZipCode)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<MyResumeOldText>(entity =>
            {
                entity.HasKey(e => e.MyUserId);

                entity.ToTable("My_Resume_OldText");

                entity.Property(e => e.MyUserId)
                    .ValueGeneratedNever()
                    .HasColumnName("MyUserID");

                entity.Property(e => e.EduText)
                    .HasMaxLength(2000)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.WorkText)
                    .HasMaxLength(2000)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<MySearcher>(entity =>
            {
                entity.ToTable("My_Searcher");

                entity.HasIndex(e => e.MyUserId, "IX_My_Searcher_MyUserID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Calling)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Email)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.IncludeDegree)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.IncludeSalary)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.IncludeWorkedYear)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.JobFunction)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.JobLocation)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.KeyWord)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.MyUserId).HasColumnName("MyUserID");

                entity.Property(e => e.PosType).HasDefaultValueSql("((3))");

                entity.Property(e => e.SearcherName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<MySendPhoneCount>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("My_SendPhoneCount");

                entity.Property(e => e.FirstSendTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsSendPhone)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.LastSendTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.MyUserId).HasColumnName("MyUserID");
            });

            modelBuilder.Entity<MySendResumeHistory>(entity =>
            {
                entity.ToTable("My_SendResumeHistory");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CompanyName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Email)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.LetterContent)
                    .HasColumnType("text")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.MyUserId).HasColumnName("MyUserID");

                entity.Property(e => e.OtherRequest)
                    .HasColumnType("text")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.PositionName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.SendTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<MyTraining>(entity =>
            {
                entity.ToTable("My_Training");

                entity.HasIndex(e => new { e.MyUserId, e.BeginDate }, "IX_My_Training_MyUserID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BeginDate).HasMaxLength(7);

                entity.Property(e => e.Certificate)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Course)
                    .HasMaxLength(30)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.EduMemo)
                    .HasMaxLength(1000)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.EndDate)
                    .HasMaxLength(7)
                    .HasDefaultValueSql("((9999)-(99))");

                entity.Property(e => e.MyUserId).HasColumnName("MyUserID");

                entity.Property(e => e.Organization)
                    .HasMaxLength(30)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<MyTrainingEng>(entity =>
            {
                entity.ToTable("My_Training_Eng");

                entity.HasIndex(e => new { e.MyUserId, e.BeginDate }, "IX_My_Training_Eng_MyUserID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BeginDate).HasMaxLength(7);

                entity.Property(e => e.Certificate)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Course)
                    .HasMaxLength(30)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.EduMemo)
                    .HasMaxLength(1000)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.EndDate)
                    .HasMaxLength(7)
                    .HasDefaultValueSql("((9999)-(99))");

                entity.Property(e => e.MyUserId).HasColumnName("MyUserID");

                entity.Property(e => e.Organization)
                    .HasMaxLength(30)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<MyUploadFile>(entity =>
            {
                entity.ToTable("My_UploadFile");

                entity.HasIndex(e => e.MyuserId, "IX_My_UploadFile_MyUserID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.FilePath).HasMaxLength(50);

                entity.Property(e => e.InsertDate)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Memo).HasMaxLength(500);

                entity.Property(e => e.MyuserId).HasColumnName("MyuserID");

                entity.Property(e => e.Title).HasMaxLength(100);
            });

            modelBuilder.Entity<MyUser>(entity =>
            {
                entity.ToTable("My_Users");

                entity.HasIndex(e => e.CheckFlag, "IX_My_Users_CheckFlag");

                entity.HasIndex(e => e.Email, "IX_My_Users_Email");

                entity.HasIndex(e => e.ResumeStatus, "IX_My_Users_ResumeStatus");

                entity.HasIndex(e => e.PhoneNum, "PK_MyUser_PhoneNum");

                entity.HasIndex(e => e.UserName, "PK_My_Users_UserName");

                entity.HasIndex(e => new { e.UserName, e.Password }, "ix_index_My_Users_UserPass");

                entity.Property(e => e.MyUserId)
                    .ValueGeneratedNever()
                    .HasColumnName("MyUserID")
                    .HasComment("个人帐号编号");

                entity.Property(e => e.CheckFlag).HasComment("0=未完成注册，1=待审核，2=审核通过，3=审核不通过，4=更新后待复审");

                entity.Property(e => e.ContactStatus).HasComment("0=公开，1=保密联系方式");

                entity.Property(e => e.DisplayFlag).HasComment("0=正试注册的(培训和教育及工作经历按表显示)，1=copy来的，按字段显示			");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('')")
                    .HasComment("电子邮件地址");

                entity.Property(e => e.EngResumeFlag).HasComment("0=无英文简历，1=显示英文简历，2=隐藏英文简历");

                entity.Property(e => e.IsAuEmail).HasColumnName("isAuEmail");

                entity.Property(e => e.IsAuPhone).HasColumnName("isAuPhone");

                entity.Property(e => e.LastLoginDate)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("最后登录日期");

                entity.Property(e => e.LoginCount).HasComment("登录次数");

                entity.Property(e => e.Password)
                    .HasMaxLength(20)
                    .HasDefaultValueSql("('')")
                    .HasComment("密码");

                entity.Property(e => e.PhoneNum)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ReceiveMailType).HasComment("0=简体内码，1=繁体内码			");

                entity.Property(e => e.RegisterBy).HasComment("注册来源");

                entity.Property(e => e.RegisterDate)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("账号注册日期");

                entity.Property(e => e.ResumeStatus).HasComment("0=正常，1=隐藏简历，2=对指定的企业隐藏，3=待删除");

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasComment("用户名");
            });

            modelBuilder.Entity<MyUserIdBack>(entity =>
            {
                entity.HasKey(e => e.MyUserId);

                entity.ToTable("MyUserID_Back");

                entity.Property(e => e.MyUserId)
                    .ValueGeneratedNever()
                    .HasColumnName("MyUserID");
            });

            modelBuilder.Entity<MyWorkExp>(entity =>
            {
                entity.ToTable("My_WorkExp");

                entity.HasIndex(e => new { e.MyUserId, e.BeginDate }, "IX_My_WorkExp_MyUserID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BeginDate)
                    .HasMaxLength(7)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ComIntroduction)
                    .HasMaxLength(200)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ComName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ComSize).HasDefaultValueSql("('')");

                entity.Property(e => e.ComType).HasDefaultValueSql("('')");

                entity.Property(e => e.Description)
                    .HasColumnType("text")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.EndDate)
                    .HasMaxLength(7)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.LeftReason)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.MyUserId).HasColumnName("MyUserID");

                entity.Property(e => e.PosName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<MyWorkExpEng>(entity =>
            {
                entity.ToTable("My_WorkExp_Eng");

                entity.HasIndex(e => new { e.MyUserId, e.BeginDate }, "IX_My_WorkExp_Eng_MyUserID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BeginDate)
                    .HasMaxLength(7)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ComIntroduction)
                    .HasMaxLength(200)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ComName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ComSize).HasDefaultValueSql("('')");

                entity.Property(e => e.ComType).HasDefaultValueSql("('')");

                entity.Property(e => e.Description)
                    .HasColumnType("text")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.EndDate)
                    .HasMaxLength(7)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.LeftReason)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.MyUserId).HasColumnName("MyUserID");

                entity.Property(e => e.PosName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<PubHelpItem>(entity =>
            {
                entity.HasKey(e => e.ItemId);

                entity.ToTable("Pub_HelpItem");

                entity.Property(e => e.ItemId)
                    .ValueGeneratedNever()
                    .HasColumnName("ItemID");

                entity.Property(e => e.FileName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ItemName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PubHelpTopic>(entity =>
            {
                entity.HasKey(e => e.TopicId);

                entity.ToTable("Pub_HelpTopic");

                entity.Property(e => e.TopicId).HasColumnName("TopicID");

                entity.Property(e => e.AddDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Answer)
                    .HasColumnType("text")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.IssuerId).HasColumnName("IssuerID");

                entity.Property(e => e.IssuerName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ItemId).HasColumnName("ItemID");

                entity.Property(e => e.Question)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PubInformation>(entity =>
            {
                entity.HasKey(e => e.InfoId);

                entity.ToTable("Pub_Information");

                entity.HasIndex(e => e.MemId, "IX_Pub_Information_Memid");

                entity.HasIndex(e => e.IsDelete, "IX_Pub_Information_isdel");

                entity.HasIndex(e => e.IssuedFrom, "IX_Pub_Information_issuedfrom");

                entity.HasIndex(e => e.ReadDate, "IX_Pub_Information_read");

                entity.HasIndex(e => e.SalerId, "IX_Pub_Information_salerid");

                entity.Property(e => e.InfoId).HasColumnName("InfoID");

                entity.Property(e => e.Content).HasColumnType("text");

                entity.Property(e => e.IssuedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Issuer)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.MemId).HasColumnName("MemID");

                entity.Property(e => e.ReadDate).HasColumnType("datetime");

                entity.Property(e => e.Reader)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SalerId).HasColumnName("SalerID");

                entity.Property(e => e.Subject)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PubInformationMy>(entity =>
            {
                entity.HasKey(e => e.InfoId)
                    .HasName("PK_Pub_Message");

                entity.ToTable("Pub_Information_My");

                entity.HasIndex(e => e.IsPublic, "IX_Pub_Information_My_Ispublic");

                entity.HasIndex(e => e.IssuedDate, "IX_Pub_Information_My_Isuserdate");

                entity.HasIndex(e => e.MyUserId, "IX_Pub_Information_My_MyUserID");

                entity.Property(e => e.InfoId).HasColumnName("InfoID");

                entity.Property(e => e.ClerkId).HasColumnName("ClerkID");

                entity.Property(e => e.Content).HasColumnType("text");

                entity.Property(e => e.IsPublic).HasDefaultValueSql("((0))");

                entity.Property(e => e.IssuedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Issuer)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.MyUserId).HasColumnName("MyUserID");

                entity.Property(e => e.ReadDate).HasColumnType("datetime");

                entity.Property(e => e.Subject)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PubInterviewing>(entity =>
            {
                entity.ToTable("Pub_Interviewing");

                entity.HasIndex(e => e.MemId, "IX_Pub_Interviewing_MemID");

                entity.HasIndex(e => e.MyUserId, "IX_Pub_Interviewing_MyUserID");

                entity.HasIndex(e => e.PosId, "IX_Pub_Interviewing_PosID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.InviteCount).HasDefaultValueSql("((1))");

                entity.Property(e => e.InviteDate)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.InviteMemo)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.MemId).HasColumnName("MemID");

                entity.Property(e => e.MemTime)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.MyUserId).HasColumnName("MyUserID");

                entity.Property(e => e.PerTime)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PosId).HasColumnName("PosID");
            });

            modelBuilder.Entity<PubPerApplyPo>(entity =>
            {
                entity.ToTable("Pub_PerApplyPos");

                entity.HasIndex(e => new { e.MemId, e.MemFlag }, "IX_Pub_PerApplyPos_MemID");

                entity.HasIndex(e => new { e.MyUserId, e.PerFlag }, "IX_Pub_PerApplyPos_MyUserID");

                entity.HasIndex(e => e.PosId, "IX_Pub_PerApplyPos_PosID");

                entity.HasIndex(e => e.ReceiveDate, "IX_Pub_PerApplyPos_ReceiveDate");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.MemId).HasColumnName("MemID");

                entity.Property(e => e.MemTime)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.MyUserId).HasColumnName("MyUserID");

                entity.Property(e => e.PerTime)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PosId).HasColumnName("PosID");

                entity.Property(e => e.ReceiveCount).HasDefaultValueSql("((1))");

                entity.Property(e => e.ReceiveDate)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<PubResumeViewLog>(entity =>
            {
                entity.ToTable("pub_ResumeViewLog");

                entity.HasIndex(e => e.MemId, "IX_pub_ResumeViewLog_Memid");

                entity.HasIndex(e => e.MyUserId, "IX_pub_ResumeViewLog_MyUserID");

                entity.HasIndex(e => e.OperateDate, "IX_pub_ResumeViewLog_OperateDate");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.MemId).HasColumnName("MemID");

                entity.Property(e => e.MyUserId).HasColumnName("MyUserID");

                entity.Property(e => e.OperateDate)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.OperateIp)
                    .HasMaxLength(15)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PubServiceHelp>(entity =>
            {
                entity.ToTable("Pub_Service_Help");

                entity.HasIndex(e => e.UserId, "IX_Pub_Service_Help_UserID");

                entity.HasIndex(e => e.UserType, "IX_Pub_Service_Help_UserType");

                entity.HasIndex(e => e.IsReply, "IX_Pub_Service_Help_isreplay");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Email)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.HelpMemo).HasColumnType("text");

                entity.Property(e => e.HelpTitle)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.InsertDate)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ReplyMemo)
                    .HasColumnType("text")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Tel)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .HasComment("用户ID");

                entity.Property(e => e.UserType).HasComment("用户类型,0不是用户,1企业用户,2个人用户");
            });

            modelBuilder.Entity<RecomMyuserId>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("RecomMyuserID");

                entity.Property(e => e.ComDate).HasColumnType("datetime");

                entity.Property(e => e.LocationC).HasColumnName("Location_C");

                entity.Property(e => e.LocationP).HasColumnName("Location_P");

                entity.Property(e => e.MyuserId).HasColumnName("MyuserID");

                entity.Property(e => e.PerName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RegisterDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<SetArea>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SetArea");

                entity.Property(e => e.AddressC).HasColumnName("Address_C");

                entity.Property(e => e.AddressP).HasColumnName("Address_P");

                entity.Property(e => e.AreaEnName).HasMaxLength(50);

                entity.Property(e => e.AreaName).HasMaxLength(50);

                entity.Property(e => e.Comprehensive)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.Focus)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Keywords).HasMaxLength(1000);

                entity.Property(e => e.PosidAll)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.Title).HasMaxLength(1000);

                entity.Property(e => e.TitleC)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasColumnName("Title_C");

                entity.Property(e => e.TitleD)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasColumnName("Title_D");

                entity.Property(e => e.TitleL)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasColumnName("Title_L");

                entity.Property(e => e.TitleR)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasColumnName("Title_R");

                entity.Property(e => e.TitleT)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasColumnName("Title_T");

                entity.Property(e => e.Yngeneration)
                    .HasMaxLength(10)
                    .HasColumnName("YNGeneration")
                    .IsFixedLength();
            });

            modelBuilder.Entity<SetLevel>(entity =>
            {
                entity.ToTable("SetLevel");

                entity.HasIndex(e => e.MemId, "SetLevel_memID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CAboutO)
                    .HasMaxLength(8000)
                    .IsUnicode(false)
                    .HasColumnName("C_about_o");

                entity.Property(e => e.CAboutS)
                    .HasMaxLength(8000)
                    .IsUnicode(false)
                    .HasColumnName("C_about_s");

                entity.Property(e => e.CAboutT)
                    .HasMaxLength(8000)
                    .IsUnicode(false)
                    .HasColumnName("C_about_t");

                entity.Property(e => e.CBanner)
                    .HasMaxLength(8000)
                    .IsUnicode(false)
                    .HasColumnName("C_banner");

                entity.Property(e => e.CBottom)
                    .HasMaxLength(8000)
                    .IsUnicode(false)
                    .HasColumnName("C_bottom");

                entity.Property(e => e.CButton)
                    .HasMaxLength(8000)
                    .IsUnicode(false)
                    .HasColumnName("C_button");

                entity.Property(e => e.CLogo)
                    .HasMaxLength(8000)
                    .IsUnicode(false)
                    .HasColumnName("C_logo");

                entity.Property(e => e.CMiddle)
                    .HasMaxLength(8000)
                    .IsUnicode(false)
                    .HasColumnName("C_middle");

                entity.Property(e => e.CName)
                    .HasMaxLength(8000)
                    .IsUnicode(false)
                    .HasColumnName("C_Name");

                entity.Property(e => e.CProduct)
                    .HasMaxLength(8000)
                    .IsUnicode(false)
                    .HasColumnName("C_product");

                entity.Property(e => e.CTop)
                    .HasMaxLength(8000)
                    .IsUnicode(false)
                    .HasColumnName("C_top");

                entity.Property(e => e.CnLogo)
                    .HasMaxLength(8000)
                    .IsUnicode(false)
                    .HasColumnName("CN_logo");

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.Keywords).HasMaxLength(1000);

                entity.Property(e => e.MemId).HasColumnName("MemID");

                entity.Property(e => e.MemName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PosIdall)
                    .HasMaxLength(8000)
                    .IsUnicode(false)
                    .HasColumnName("PosIDAll");

                entity.Property(e => e.Title).HasMaxLength(1000);

                entity.Property(e => e.Yncreate)
                    .HasMaxLength(10)
                    .HasColumnName("YNCreate")
                    .IsFixedLength();
            });

            modelBuilder.Entity<SiteType>(entity =>
            {
                entity.ToTable("SiteType");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.SiteName).HasMaxLength(100);

                entity.Property(e => e.SiteType1)
                    .HasMaxLength(100)
                    .HasColumnName("SiteType");
            });

            modelBuilder.Entity<SysExcludeIncompleteMyId>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Sys_ExcludeIncompleteMyID");

                entity.Property(e => e.AddTime).HasColumnType("datetime");

                entity.Property(e => e.MyuserId).HasColumnName("MyuserID");
            });

            modelBuilder.Entity<SysExcludeMyId>(entity =>
            {
                entity.HasKey(e => e.MyUserId);

                entity.ToTable("Sys_ExcludeMyID");

                entity.Property(e => e.MyUserId)
                    .ValueGeneratedNever()
                    .HasColumnName("MyUserID");
            });

            modelBuilder.Entity<SysGetIncompleteName>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Sys_GetIncompleteName");

                entity.Property(e => e.AddTime).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MyName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MyuserId).HasColumnName("MyuserID");

                entity.Property(e => e.PassWord)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.WorkWrite).HasColumnName("workWrite");
            });

            modelBuilder.Entity<SysGetName>(entity =>
            {
                entity.HasKey(e => e.MyUserId);

                entity.ToTable("Sys_GetName");

                entity.Property(e => e.MyUserId)
                    .ValueGeneratedNever()
                    .HasColumnName("MyUserID");

                entity.Property(e => e.AddTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MyName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PassWord)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SysGetPosName>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Sys_GetPosName");

                entity.Property(e => e.AddTime).HasColumnType("datetime");

                entity.Property(e => e.JobFunction)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MemId).HasColumnName("MemID");

                entity.Property(e => e.MemName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MyUserId).HasColumnName("MyUserID");

                entity.Property(e => e.PosId).HasColumnName("PosID");

                entity.Property(e => e.PosName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SysGetPosNameAll>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Sys_GetPosNameAll");

                entity.Property(e => e.AddTime).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.JobFunction)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.JobFunction1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MemId).HasColumnName("MemID");

                entity.Property(e => e.MemId1).HasColumnName("MemID1");

                entity.Property(e => e.MemName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MemName1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MyName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MyUserId).HasColumnName("MyUserID");

                entity.Property(e => e.PassWord)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PosId).HasColumnName("PosID");

                entity.Property(e => e.PosId1).HasColumnName("PosID1");

                entity.Property(e => e.PosName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PosName1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SysKeyWord>(entity =>
            {
                entity.ToTable("Sys_KeyWord");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AddTime).HasColumnType("datetime");

                entity.Property(e => e.KeyHome)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.KeyWords)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.PosTypeId).HasColumnName("PosTypeID");
            });

            modelBuilder.Entity<SysRamdomExcludeMyId>(entity =>
            {
                entity.HasKey(e => e.MyuserId);

                entity.ToTable("Sys_RamdomExcludeMyID");

                entity.HasIndex(e => e.MyuserId, "Sys_RamdomExcludeMyIDMyuserID");

                entity.Property(e => e.MyuserId)
                    .ValueGeneratedNever()
                    .HasColumnName("MyuserID");

                entity.Property(e => e.AddTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<SysRandomGetName>(entity =>
            {
                entity.HasKey(e => e.MyUserId);

                entity.ToTable("Sys_RandomGetName");

                entity.Property(e => e.MyUserId)
                    .ValueGeneratedNever()
                    .HasColumnName("MyUserID");

                entity.Property(e => e.AddTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MobileNum)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MyName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PassWord)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TableMaxId>(entity =>
            {
                entity.HasKey(e => e.TableName);

                entity.ToTable("TableMaxID");

                entity.Property(e => e.TableName)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.MaxId).HasColumnName("MaxID");
            });

            modelBuilder.Entity<TempId>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TempID");

                entity.Property(e => e.Myuserid).HasColumnName("myuserid");
            });

            modelBuilder.Entity<TemplateSet>(entity =>
            {
                entity.ToTable("TemplateSet");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.TemplateCoding).HasMaxLength(100);

                entity.Property(e => e.TemplateConnten).HasColumnType("text");

                entity.Property(e => e.TemplateFormat).HasMaxLength(100);

                entity.Property(e => e.TemplateName).HasMaxLength(100);

                entity.Property(e => e.TemplateSiteName).HasMaxLength(100);

                entity.Property(e => e.TemplateType).HasMaxLength(100);
            });

            modelBuilder.Entity<TemplateType>(entity =>
            {
                entity.ToTable("TemplateType");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.SiteType).HasMaxLength(100);

                entity.Property(e => e.TemplateType1)
                    .HasMaxLength(100)
                    .HasColumnName("TemplateType");

                entity.Property(e => e.TemplateTypeEn)
                    .HasMaxLength(100)
                    .HasColumnName("TemplateTypeEN");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
