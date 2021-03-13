namespace DBModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SysMenu")]
    public partial class SysMenu
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SysMenu()
        {
            Children = new HashSet<SysMenu>();
        }

        [Key]
        public int MenuID { get; set; }

        public int? ParentID { get; set; }

        [StringLength(50)]
        public string MenuName { get; set; }

        [StringLength(50)]
        public string MenuCode { get; set; }

        [StringLength(50)]
        public string Icon { get; set; }

        public int? Status { get; set; }

        [StringLength(50)]
        public string Url { get; set; }

        public string Operation { get; set; }

        public int? SortNo { get; set; }

        public int? IsActive { get; set; }

        public int? Creator { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? Editor { get; set; }

        public DateTime? EditTime { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SysMenu> Children { get; set; }

        public virtual SysMenu Parent { get; set; }
    }
}
