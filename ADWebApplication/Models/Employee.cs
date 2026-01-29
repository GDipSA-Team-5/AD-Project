using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ADWebApplication.Models
{
    [Table("employee")]
    public class Employee
    {
        [Key]
        [Column("employeeId")]
        public int EmployeeId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("username")]
        public string Username { get; set; } = string.Empty;

        [Required]
        [MaxLength(45)]
        [Column("fullname")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(150)]
        [Column("email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        [Column("passwordHash")]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [Column("roleId")]
        public int RoleId { get; set; }
        public Role Role { get; set; } = null!;

        [Column("isActive")]
        public bool IsActive { get; set; }
    }

    [Table("role")]
    public class Role
    {
        [Key]
        [Column("roleId")]
        public int RoleId { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("name")]
        public string Name { get; set; } = string.Empty;
    }
}
