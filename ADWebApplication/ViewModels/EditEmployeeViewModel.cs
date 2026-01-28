using System.ComponentModel.DataAnnotations;

namespace ADWebApplication.ViewModels;

public class EditEmployeeViewModel
{
    public int Id { get; set; }

    [Required]
    public string FullName { get; set; } = "";

    [Required]
    public string Username { get; set; } = "";

    [Required, EmailAddress]
    public string Email { get; set; } = "";

    [Required]
    public string PhoneNumber { get; set; } = "";

    public bool IsActive { get; set; }

    [Required]
    public string RoleName { get; set; } = "";

    // optional reset password
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
    [DataType(DataType.Password)]
    public string? NewPassword { get; set; }

    [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
    [DataType(DataType.Password)]
    public string? ConfirmNewPassword { get; set; }
}