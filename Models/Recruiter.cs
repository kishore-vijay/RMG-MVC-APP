namespace ResourceManageGroup.Models;
using System.ComponentModel.DataAnnotations;

public class Recruiter{
    [Key]
    public string? recruiterId { get; set; }
    [RegularExpression(@"^[A-Z][a-zA-Z]*(?:\s[A-Z][a-zA-Z]*)*$", ErrorMessage = "Hr - Manager name must contain only letters and spaces.")]
    public string? recruiterName { get; set; }
    [RegularExpression(@"^[a-z0-9]+@[a-zA-Z0-9]+(\.[a-zA-Z]{2,})+$", ErrorMessage = "Invalid email format.")]
    public string? recruiterEmail { get; set; }
    [RegularExpression(@"^\+?[6-9][0-9]{9,11}$", ErrorMessage = "Invalid phone number format.")]
    public string? recruiterNumber { get; set; }
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&.])[A-Za-z\d@$!%*?&.]{8,}$", ErrorMessage = "Password must contain at least 8 characters, one uppercase letter, one lowercase letter, one digit, and one special character.")]
    public string? recruiterPassword { get; set; }
}