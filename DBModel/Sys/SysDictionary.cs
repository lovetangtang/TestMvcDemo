namespace DBModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SysDictionary")]
    public partial class SysDictionary
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SysDictionary()
        {
            SysDictionary1 = new HashSet<SysDictionary>();
        }

        [Key]
        public int DictId { get; set; }

        public int? ParentId { get; set; }

        [StringLength(50)]
        public string DictName { get; set; }

        [StringLength(50)]
        public string DictCode { get; set; }

        public int? IsActive { get; set; }

        public int? DictValue { get; set; }

        public int? SortNo { get; set; }

        public int? Creator { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? Editor { get; set; }

        public DateTime? EditTime { get; set; }

        [StringLength(500)]
        public string Remark { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SysDictionary> SysDictionary1 { get; set; }

        public virtual SysDictionary SysDictionary2 { get; set; }
    }
}
