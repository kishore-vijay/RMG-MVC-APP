namespace ResourceManageGroup.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
public class Employee
{   
    [Key]
    public string? employeeId { get; set; }
    [StringLength(maximumLength: 20, ErrorMessage = "Employee name must be between {2} and {1} characters.", MinimumLength = 2)]
    [RegularExpression(@"^[A-Z][a-zA-Z]*(?:\s[A-Z][a-zA-Z]*)*$", ErrorMessage = "Employee name must contain only letters and spaces.")]
    public string? employeeName { get; set; }
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email format.")]
    public string? employeeEmail { get; set; }
    [RegularExpression(@"^\+?[6-9][0-9]{9,11}$", ErrorMessage = "Invalid phone number format.")]
    public string? employeeNumber { get; set; }
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[a-zA-Z\d@$!%*?&]{8,}$", ErrorMessage = "Password must contain at least 8 characters, one uppercase letter, one lowercase letter, one digit, and one special character.")]
    public string? employeePassword { get; set; }
    [Compare("employeePassword", ErrorMessage = "The password and confirmation password do not match.")]
    public string? employeeConfirmPassword { get; set; }
    [Range(18, 60, ErrorMessage = "Employee age must be between {1} and {2}.")]
    public int employeeAge { get; set; }
    public string? employeeVacationStartTime { get; set; }
    [VacationDate(ErrorMessage = "Invalid vacation dates.")]
    public string? employeeVacationEndTime { get; set; }
    public string? employeeVacationReason { get; set; }
    public byte[]? employeeImage { get; set; }
    // The Values Will Be Assigned from HR- Manager Part
    public string? employeeTrainingStartTime { get; set; }
    [TrainingDate(ErrorMessage = "Invalid Training dates.")]
    public string? employeeTrainingEndTime { get; set; } 
    public string? employeeWorkingStatus { get; set; }
    public string? employeeVacationStatus { get; set; }
    public string? employeeTechnology { get; set; }
    public string? employeeTrainerName { get; set; }
    // The Values Will Be Assigned from Project - Manager Part
    public string? employeeProject { get; set; }
}
