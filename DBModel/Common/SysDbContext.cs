namespace DBModel
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class SysDbContext : DbContext
    {
        public SysDbContext()
            : base("name=SysDbContext")
        {
        }
        public virtual DbSet<SysDictionary> SysDictionary { get; set; }
        public virtual DbSet<SysUser> SysUser { get; set; }
        public virtual DbSet<SysArea> SysArea { get; set; }
        public virtual DbSet<SysMenu> SysMenu { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<SysDictionary>()
                .Property(e => e.DictCode)
                .IsUnicode(false);

            modelBuilder.Entity<SysDictionary>()
                .HasMany(e => e.SysDictionary1)
                .WithOptional(e => e.SysDictionary2)
                .HasForeignKey(e => e.ParentId);

            modelBuilder.Entity<SysUser>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<SysUser>()
                .Property(e => e.OpenId)
                .IsUnicode(false);

            modelBuilder.Entity<SysUser>()
                .Property(e => e.PersonalAuth)
                .IsUnicode(false);
            modelBuilder.Entity<SysMenu>()
                      .HasMany(e => e.Children)
                      .WithOptional(e => e.Parent)
                      .HasForeignKey(e => e.ParentID);
        }
    }
}
