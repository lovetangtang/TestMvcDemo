namespace DBModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SysArea")]
    public partial class SysArea
    {
        [Key]
        public int AreaID { get; set; }

        [StringLength(50)]
        public string AreaName { get; set; }

        public int? IsDefault { get; set; }

        public int? IsActive { get; set; }

        public int? Creator { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? Editor { get; set; }

        public DateTime? EditTime { get; set; }
    }
}
