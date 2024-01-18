namespace ResourceManageGroup.Models;
using System.ComponentModel.DataAnnotations;

public class Project{
    public int projectId { get; set; }
    [RegularExpression(@"^[0-9a-zA-Z\s]+$", ErrorMessage = "Project name must contain only letters and spaces and numbers.")]
    public string? projectName { get; set; }
    public string? projectDescription { get; set; }
    public string? projectStartTime{ get; set;}
    [ProjectDate(ErrorMessage = "Invalid Project dates.")]
    public string? projectEndTime{ get; set;}
    public string? projectLead{ get; set;}
    public string? projectType { get; set; }
}