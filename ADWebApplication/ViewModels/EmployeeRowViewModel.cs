namespace ADWebApplication.ViewModels;

public class EmployeeRowViewModel
{
    public int Id { get; set; }
    public string FullName { get; set; } = "";
    public string Username { get; set; } = "";
    public string Email { get; set; } = "";
    public string PhoneNumber { get; set; } = "";
    public bool IsActive { get; set; }
    public string RoleName { get; set; } = "-";
}