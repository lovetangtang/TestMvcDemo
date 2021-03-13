namespace DBModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SysUser")]
    public partial class SysUser
    {
        [Key]
        public int UserId { get; set; }

        [StringLength(10)]
        public string UserName { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }

        [StringLength(100)]
        public string OpenId { get; set; }

        public int? OUId { get; set; }

        public int? RoleId { get; set; }

        public int? AreaID { get; set; }

        public string PersonalAuth { get; set; }

        public int? IsActive { get; set; }

        public int? Creator { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? Editor { get; set; }

        public DateTime? EditTime { get; set; }
    }
}
