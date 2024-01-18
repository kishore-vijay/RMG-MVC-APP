namespace ResourceManageGroup.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Validation{
    public string ? verifyCode { get; set; }
    public List<string> List { get; set; } = new List<string>();
}



















public class VacationDateAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object ?value, ValidationContext validationContext)
    {
        try{
            
            var startTimeProperty = validationContext.ObjectType.GetProperty("employeeVacationStartTime");
            var endTimeProperty = validationContext.ObjectType.GetProperty("employeeVacationEndTime");
            if (startTimeProperty == null || endTimeProperty == null)
                throw new ArgumentException("Invalid property names");
            var startTimeValue = startTimeProperty.GetValue(validationContext.ObjectInstance) as string;
            var endTimeValue = endTimeProperty.GetValue(validationContext.ObjectInstance) as string;
            if (!DateTime.TryParse(startTimeValue, out DateTime startTime) || !DateTime.TryParse(endTimeValue, out DateTime endTime))
            {
                return new ValidationResult("Select Vacation Start & End Period");
            }
            if (startTime > endTime)
            {
                return new ValidationResult("Start date should be earlier than the end date.");
            }
            if(endTime.AddMonths(-3) > startTime)
            {
                return new ValidationResult("Vacation is valid only for 3 months.");
            }
            return ValidationResult.Success;
        }
        catch (Exception exception)
        {
            Console.WriteLine("An exception occurred: " + exception.Message);
            return new ValidationResult("An error occurred during validation.");
        }
    }
}

public class TrainingDateAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object ?value, ValidationContext validationContext)
    {
        try{
            
            var startTimeProperty = validationContext.ObjectType.GetProperty("employeeTrainingStartTime");
            var endTimeProperty = validationContext.ObjectType.GetProperty("employeeTrainingEndTime");
            if (startTimeProperty == null || endTimeProperty == null)
                throw new ArgumentException("Invalid property names");
            var startTimeValue = startTimeProperty.GetValue(validationContext.ObjectInstance) as string;
            var endTimeValue = endTimeProperty.GetValue(validationContext.ObjectInstance) as string;
            if (!DateTime.TryParse(startTimeValue, out DateTime startTime) || !DateTime.TryParse(endTimeValue, out DateTime endTime))
            {
                return new ValidationResult("Select Training Start & End Period");
            }
            if (startTime > endTime)
            {
                return new ValidationResult("Start date should be earlier than the end date.");
            }
            if(endTime.AddMonths(-12) > startTime)
            {
                return new ValidationResult("Training can be alloted maximum for a Year");
            }
            return ValidationResult.Success;
        }
        catch (Exception exception)
        {
            Console.WriteLine("An exception occurred: " + exception.Message);
            return new ValidationResult("An error occurred during validation.");
        }
    }
}
public class ProjectDateAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object ?value, ValidationContext validationContext)
    {
        try
        {
            var startTimeProperty = validationContext.ObjectType.GetProperty("projectStartTime");
            var endTimeProperty = validationContext.ObjectType.GetProperty("projectEndTime");
            if (startTimeProperty == null || endTimeProperty == null)
                throw new ArgumentException("Invalid property names");
            var startTimeValue = startTimeProperty.GetValue(validationContext.ObjectInstance) as string;
            var endTimeValue = endTimeProperty.GetValue(validationContext.ObjectInstance) as string;
            if (!DateTime.TryParse(startTimeValue, out DateTime startTime) || !DateTime.TryParse(endTimeValue, out DateTime endTime))
            {
                return new ValidationResult("Select Project Start & End Period");
            }
            if (startTime > endTime)
            {
                return new ValidationResult("Start date should be earlier than the end date.");
            }
            return ValidationResult.Success;
        }
        catch (Exception exception)
        {
            Console.WriteLine("An exception occurred: " + exception.Message);
            return new ValidationResult("An error occurred during validation.");
        }
    }
}

