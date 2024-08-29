using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Entity.GoodjobQuery
{
    public partial class Goodjob_QueryContext : DbContext
    {
        public Goodjob_QueryContext()
        {
        }

        public Goodjob_QueryContext(DbContextOptions<Goodjob_QueryContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ArticleRelateJobFunction> ArticleRelateJobFunctions { get; set; } = null!;
        public virtual DbSet<BackEmail> BackEmails { get; set; } = null!;
        public virtual DbSet<BackMemId> BackMemIds { get; set; } = null!;
        public virtual DbSet<Ceshi> Ceshis { get; set; } = null!;
        public virtual DbSet<MemBlackList> MemBlackLists { get; set; } = null!;
        public virtual DbSet<MemPosAiquery> MemPosAiqueries { get; set; } = null!;
        public virtual DbSet<MemPosJobFunction> MemPosJobFunctions { get; set; } = null!;
        public virtual DbSet<MemPosJobFunction1> MemPosJobFunctions1 { get; set; } = null!;
        public virtual DbSet<MemPosJobLocation> MemPosJobLocations { get; set; } = null!;
        public virtual DbSet<MemPosQuery> MemPosQueries { get; set; } = null!;
        public virtual DbSet<MyResumeAll> MyResumeAlls { get; set; } = null!;
        public virtual DbSet<MyResumeQuery> MyResumeQueries { get; set; } = null!;
        public virtual DbSet<MyResumeVice> MyResumeVices { get; set; } = null!;
        public virtual DbSet<PanYuWiredPosQuery> PanYuWiredPosQueries { get; set; } = null!;
        public virtual DbSet<TempCommendPosition> TempCommendPositions { get; set; } = null!;
        public virtual DbSet<TempUpdatedPosition> TempUpdatedPositions { get; set; } = null!;
        public virtual DbSet<TempUpdatedResume> TempUpdatedResumes { get; set; } = null!;
        public virtual DbSet<TvMemInfo> TvMemInfos { get; set; } = null!;
        public virtual DbSet<Xianchan> Xianchans { get; set; } = null!;
        public virtual DbSet<XunHuanMyUserId> XunHuanMyUserIds { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=192.168.3.2;Initial Catalog=Goodjob_Query;User ID=goodjobjishu;Password=juncaiwang/*-;Encrypt=False;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ArticleRelateJobFunction>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Article_RelateJobFunction");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.NewsId).HasColumnName("NewsID");
            });

            modelBuilder.Entity<BackEmail>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Back_Email");

                entity.Property(e => e.Email)
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<BackMemId>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Back_MemID");

                entity.Property(e => e.MemId).HasColumnName("MemID");
            });

            modelBuilder.Entity<Ceshi>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ceshi");

                entity.Property(e => e.MyuserId).HasColumnName("MyuserID");
            });

            modelBuilder.Entity<MemBlackList>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Mem_BlackList");

                entity.Property(e => e.InsertDate).HasColumnType("smalldatetime");

                entity.Property(e => e.MemId).HasColumnName("MemID");
            });

            modelBuilder.Entity<MemPosAiquery>(entity =>
            {
                entity.HasKey(e => e.PosId);

                entity.ToTable("MemPos_AIQuery");

                entity.Property(e => e.PosId)
                    .ValueGeneratedNever()
                    .HasColumnName("PosID");

                entity.Property(e => e.DeptId).HasColumnName("DeptID");

                entity.Property(e => e.Geyn).HasColumnName("GEYN");

                entity.Property(e => e.HasPage).HasComment("是否有专页");

                entity.Property(e => e.Iscommend).HasComment("是否推荐企业");

                entity.Property(e => e.JobFunctionPart)
                    .HasMaxLength(200)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.JobLocation)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Lng).HasColumnName("lng");

                entity.Property(e => e.MemId).HasColumnName("MemID");

                entity.Property(e => e.MemName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MemRegisterDate).HasColumnType("smalldatetime");

                entity.Property(e => e.PosDescription)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.PosName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PosSalary).HasComment("固定薪资 用于存放 千元等 用户手动输入的金额");

                entity.Property(e => e.PostDate).HasColumnType("smalldatetime");

                entity.Property(e => e.RegisterBy).HasComment("0 俊才 1.前程 2.boss 3.58 4.智联 5拉钩");

                entity.Property(e => e.RegularWorker).HasComment("转正机会  1 提供转正  2 不提供转正  3 面议");

                entity.Property(e => e.ReqDegreeId).HasColumnName("ReqDegreeID");

                entity.Property(e => e.SalaryMode).HasComment("0 薪资类型 1 固定薪资 2范围薪资");

                entity.Property(e => e.SalaryRange).HasMaxLength(150);

                entity.Property(e => e.SalaryUnit).HasComment("薪资单位 1 元/每小时  2 元/每天  3 元/每周  4 元/每月  5 元/每次  6 元/每件");

                entity.Property(e => e.SeoAddress)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.WeekRequired).HasComment("0");

                entity.Property(e => e.Welfa).HasMaxLength(150);
            });

            modelBuilder.Entity<MemPosJobFunction>(entity =>
            {
                entity.ToTable("Mem_PosJobFunction");

                entity.HasIndex(e => e.JobfunctionSmall, "IX_Mem_PosJobFunction_JSmall");

                entity.HasIndex(e => e.PosId, "IX_Mem_PosJobFunction_PosID");

                entity.HasIndex(e => e.JobFunctionBig, "IX_Mem_PosJobFunction_jBig");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.JobFunctionBig).HasColumnName("JobFunction_big");

                entity.Property(e => e.JobfunctionSmall).HasColumnName("Jobfunction_small");

                entity.Property(e => e.PosId).HasColumnName("PosID");
            });

            modelBuilder.Entity<MemPosJobFunction1>(entity =>
            {
                entity.ToTable("Mem_PosJobFunction_s");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.JobFunctionBig).HasColumnName("JobFunction_big");

                entity.Property(e => e.JobFunctionId).HasColumnName("JobFunctionID");

                entity.Property(e => e.JobfunctionSmall).HasColumnName("Jobfunction_small");

                entity.Property(e => e.PosId).HasColumnName("PosID");
            });

            modelBuilder.Entity<MemPosJobLocation>(entity =>
            {
                entity.ToTable("Mem_PosJobLocation");

                entity.HasIndex(e => e.JobLocationP, "IX_Mem_PosJobLocation_JP");

                entity.HasIndex(e => e.JobLocationC, "IX_Mem_PosJobLocation_Jc");

                entity.HasIndex(e => e.PosId, "IX_Mem_PosJobLocation_PID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.JobLocationC).HasColumnName("JobLocation_C");

                entity.Property(e => e.JobLocationP).HasColumnName("JobLocation_P");

                entity.Property(e => e.PosId).HasColumnName("PosID");
            });

            modelBuilder.Entity<MemPosQuery>(entity =>
            {
                entity.HasKey(e => e.PosId);

                entity.ToTable("MemPos_Query");

                entity.HasIndex(e => e.Iscommend, "IX_MemPos_Query_Iscom");

                entity.HasIndex(e => e.MemId, "IX_MemPos_Query_MemID");

                entity.HasIndex(e => e.PosType, "IX_MemPos_Query_PosType");

                entity.HasIndex(e => e.PostDate, "IX_MemPos_Query_PostDate");

                entity.HasIndex(e => e.ReqDegreeId, "IX_MemPos_Query_RD");

                entity.HasIndex(e => e.ReqSex, "IX_MemPos_Query_RS");

                entity.HasIndex(e => e.ReqWorkYear, "IX_MemPos_Query_RW");

                entity.HasIndex(e => e.Salary, "IX_MemPos_Query_Salary");

                entity.HasIndex(e => e.ReqAge1, "IX_MemPos_Query_age1");

                entity.HasIndex(e => e.ReqAge2, "IX_MemPos_Query_age2");

                entity.HasIndex(e => e.Calling, "IX_MemPos_Query_calling");

                entity.HasIndex(e => e.MemName, "IX_MemPos_Qusery_MemName");

                entity.HasIndex(e => e.PosName, "MemPos_Query_PosName");

                entity.HasIndex(e => e.Hires, "PK_MemPos_Query_Hires");

                entity.Property(e => e.PosId)
                    .ValueGeneratedNever()
                    .HasColumnName("PosID");

                entity.Property(e => e.DeptId).HasColumnName("DeptID");

                entity.Property(e => e.Geyn).HasColumnName("GEYN");

                entity.Property(e => e.HasPage).HasComment("是否有专页");

                entity.Property(e => e.Iscommend).HasComment("是否推荐企业");

                entity.Property(e => e.JobFunctionPart)
                    .HasMaxLength(200)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.JobLocation)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Lng).HasColumnName("lng");

                entity.Property(e => e.MemId).HasColumnName("MemID");

                entity.Property(e => e.MemName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MemRegisterDate).HasColumnType("smalldatetime");

                entity.Property(e => e.PosDescription)
                    .HasMaxLength(1800)
                    .IsUnicode(false);

                entity.Property(e => e.PosName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PosSalary).HasComment("固定薪资 用于存放 千元等 用户手动输入的金额");

                entity.Property(e => e.PostDate).HasColumnType("smalldatetime");

                entity.Property(e => e.RegisterBy)
                    .HasDefaultValueSql("((0))")
                    .HasComment(" 0 俊才 1.前程 2.boss 3.58 4.智联");

                entity.Property(e => e.RegularWorker).HasComment("转正机会  1 提供转正  2 不提供转正  3 面议");

                entity.Property(e => e.ReqDegreeId).HasColumnName("ReqDegreeID");

                entity.Property(e => e.SalaryMode).HasComment("0 薪资类型 1 固定薪资 2范围薪资");

                entity.Property(e => e.SalaryMonth).HasDefaultValueSql("((12))");

                entity.Property(e => e.SalaryRange).HasMaxLength(150);

                entity.Property(e => e.SalaryUnit).HasComment("薪资单位 1 元/每小时  2 元/每天  3 元/每周  4 元/每月  5 元/每次  6 元/每件");

                entity.Property(e => e.SeoAddress)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.WeekRequired).HasComment("0");

                entity.Property(e => e.Welfa).HasMaxLength(150);
            });

            modelBuilder.Entity<MyResumeAll>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("MyResume_All");

                entity.HasIndex(e => e.MyUserId, "IX_MyResume_All_Text");

                entity.HasIndex(e => e.MyUserId, "PK_MyResume_All")
                    .IsUnique()
                    .IsClustered();

                entity.Property(e => e.AllText).HasColumnType("text");

                entity.Property(e => e.MyUserId).HasColumnName("MyUserID");
            });

            modelBuilder.Entity<MyResumeQuery>(entity =>
            {
                entity.HasKey(e => e.MyUserId);

                entity.ToTable("MyResume_Query");

                entity.HasIndex(e => e.Birthday, "IX_MyResume_Query_Birthday");

                entity.HasIndex(e => e.DegreeId, "IX_MyResume_Query_Degreeid");

                entity.HasIndex(e => e.EngResumeFlag, "IX_MyResume_Query_EngFlag");

                entity.HasIndex(e => e.PerName, "IX_MyResume_Query_PerName");

                entity.HasIndex(e => e.PhotoFlag, "IX_MyResume_Query_Poto");

                entity.HasIndex(e => e.Speciality, "IX_MyResume_Query_SPly");

                entity.HasIndex(e => e.SchoolName, "IX_MyResume_Query_Schname");

                entity.HasIndex(e => e.Sex, "IX_MyResume_Query_Sex");

                entity.HasIndex(e => e.ResumeStatus, "IX_MyResume_Query_Status");

                entity.HasIndex(e => e.UpdateDate, "IX_MyResume_Query_Update");

                entity.HasIndex(e => e.WorkedYear, "IX_MyResume_Query_Wky");

                entity.HasIndex(e => e.BelongType, "PK_MyResume_Query_BelongType");

                entity.HasIndex(e => new { e.HometownP, e.HometownC }, "PK_MyResume_Query_Hometown");

                entity.HasIndex(e => new { e.LocationP, e.LocationC, e.LocationT }, "PK_MyResume_Query_Location");

                entity.Property(e => e.MyUserId)
                    .ValueGeneratedNever()
                    .HasColumnName("MyUserID");

                entity.Property(e => e.BelongType).HasComment("简历所属 0为俊才 1为大岗 2为南沙 4为黄阁");

                entity.Property(e => e.Birthday).HasColumnType("smalldatetime");

                entity.Property(e => e.DegreeId).HasColumnName("DegreeID");

                entity.Property(e => e.Grade)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.HometownC).HasColumnName("Hometown_C");

                entity.Property(e => e.HometownP).HasColumnName("Hometown_P");

                entity.Property(e => e.LastPosName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.LocationC).HasColumnName("Location_C");

                entity.Property(e => e.LocationP).HasColumnName("Location_P");

                entity.Property(e => e.LocationT).HasColumnName("Location_T");

                entity.Property(e => e.PerName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Professional)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ResumeStatus).HasDefaultValueSql("((0))");

                entity.Property(e => e.SchoolName)
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.Speciality)
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<MyResumeVice>(entity =>
            {
                entity.ToTable("MyResumeVice");

                entity.HasIndex(e => new { e.ResumeTitle, e.Contact }, "PK_MyResumeVice_Title");

                entity.Property(e => e.AddTime)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Appendix).HasColumnType("text");

                entity.Property(e => e.Contact).HasMaxLength(50);

                entity.Property(e => e.EplId).HasColumnName("EPL_Id");

                entity.Property(e => e.EplName)
                    .HasMaxLength(50)
                    .HasColumnName("EPL_Name");

                entity.Property(e => e.RdepId).HasColumnName("RDepId");

                entity.Property(e => e.Regions)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ReplId).HasColumnName("REPL_Id");

                entity.Property(e => e.ReplName)
                    .HasMaxLength(50)
                    .HasColumnName("REPL_Name");

                entity.Property(e => e.ResumeTitle).HasMaxLength(100);

                entity.Property(e => e.ShareEplId)
                    .HasMaxLength(100)
                    .HasColumnName("ShareEpl_Id");

                entity.Property(e => e.UpdataTime).HasColumnType("smalldatetime");
            });

            modelBuilder.Entity<PanYuWiredPosQuery>(entity =>
            {
                entity.HasKey(e => e.PosId);

                entity.ToTable("PanYuWiredPos_Query");

                entity.Property(e => e.PosId)
                    .ValueGeneratedNever()
                    .HasColumnName("PosID");

                entity.Property(e => e.JobLocation)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.MemId).HasColumnName("MemID");

                entity.Property(e => e.MemName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PosDescription)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.PosName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PostDate).HasColumnType("smalldatetime");

                entity.Property(e => e.ReqDegreeId).HasColumnName("ReqDegreeID");

                entity.Property(e => e.SalaryRange).HasMaxLength(150);

                entity.Property(e => e.SeoAddress)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Welfa).HasMaxLength(150);
            });

            modelBuilder.Entity<TempCommendPosition>(entity =>
            {
                entity.ToTable("Temp_CommendPosition");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.JobLocationC)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("JobLocation_C");

                entity.Property(e => e.JobLocationP)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("JobLocation_P");

                entity.Property(e => e.MemId).HasColumnName("MemID");

                entity.Property(e => e.MemName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.PosId).HasColumnName("PosID");

                entity.Property(e => e.PosName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<TempUpdatedPosition>(entity =>
            {
                entity.ToTable("Temp_UpdatedPosition");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.JobLocationC)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("JobLocation_C");

                entity.Property(e => e.JobLocationP)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("JobLocation_P");

                entity.Property(e => e.MemId).HasColumnName("MemID");

                entity.Property(e => e.MemName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.PosId).HasColumnName("PosID");

                entity.Property(e => e.PosName)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TempUpdatedResume>(entity =>
            {
                entity.ToTable("Temp_UpdatedResume");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.MyUserId).HasColumnName("MyUserID");
            });

            modelBuilder.Entity<TvMemInfo>(entity =>
            {
                entity.ToTable("Tv_MemInfo");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.MemId).HasColumnName("MemID");

                entity.Property(e => e.MemName)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Xianchan>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("xianchan");

                entity.Property(e => e.MyUserId).HasColumnName("MyUserID");
            });

            modelBuilder.Entity<XunHuanMyUserId>(entity =>
            {
                entity.HasKey(e => e.MyUserId);

                entity.ToTable("XunHuan_MyUserID");

                entity.Property(e => e.MyUserId)
                    .ValueGeneratedNever()
                    .HasColumnName("MyUserID");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
