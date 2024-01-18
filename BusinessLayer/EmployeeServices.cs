using ResourceManageGroup.Models;
using ResourceManageGroup.Data;
using Microsoft.EntityFrameworkCore;
using System.Data;
namespace ResourceManageGroup.Services;
public class EmployeeServices
{
    private readonly ApplicationDbContext _dbContext;
    public EmployeeServices(ApplicationDbContext dbContext,IWebHostEnvironment hostEnvironment)
    {
        _dbContext = dbContext;
    }
    
    public async Task<string> registerUser(Employee employee,Validation validation){
        try{
            int emailcount = 0;
            int numcount = 0;
            if(_dbContext.EmployeeDetails!=null){
                emailcount = _dbContext.EmployeeDetails.Where(c => c.employeeEmail == employee.employeeEmail).Count();
                numcount = _dbContext.EmployeeDetails.Where(c => c.employeeNumber == employee.employeeNumber).Count();
                if(employee.employeeEmail!=null &&  employee.employeeNumber!=null && employee.employeePassword!=null){
                    if(emailcount>0){
                        return "The User Already Exists !!!!!";
                    }
                    else if(numcount>0){
                        return "The User Already Exists !!!!!";
                    }
                    else{
                        employee.employeeTechnology = "Not Assigned";
                        employee.employeeProject = "Not Assigned";
                        employee.employeeWorkingStatus = "Not Assigned" ;
                        employee.employeeTrainingStartTime = "Not Assigned" ;                            
                        employee.employeeTrainingEndTime = "Not Assigned" ;
                        employee.employeeTrainerName = "Not Assigned" ;
                        employee.employeeVacationStartTime = "Not Assigned" ;
                        employee.employeeVacationEndTime = "Not Assigned" ;
                        employee.employeeVacationStatus =  "Not Assigned" ;
                        employee.employeeImage = null;  
                        employee.employeeVacationReason = "Not Assigned";  
                        _dbContext.Add(employee);
                        await _dbContext.SaveChangesAsync(); 
                        return "Succesfully Registered , Log in to Work";                     
                    }
                }           
            }
            return "Invalid Data Provided";
        }
        catch(Exception exception){
            Console.WriteLine(exception.Message);
            return "Invalid Data Provided";
        }
    }

    public async Task<Employee> getEmployeeById(string employeeId)
    {
        if (_dbContext.EmployeeDetails != null)
        {
            var employee = await _dbContext.EmployeeDetails.FirstOrDefaultAsync(x => x.employeeId == employeeId);
            if (employee != null)
            {
                return employee;
            }
        }
        throw new InvalidOperationException($"Employee with ID {employeeId} not found.");
    }

    public async Task<Employee> editEmployee(Employee employee, IFormFile image1)
    {
        try
        {
            if (_dbContext.EmployeeDetails != null)
            {
                var employees = await _dbContext.EmployeeDetails.FindAsync(employee.employeeId);
                if (employees != null)
                {
                    employees.employeeName = employee.employeeName;
                    employees.employeeNumber = employee.employeeNumber;
                    employees.employeeEmail = employee.employeeEmail;
                    employees.employeeAge = employee.employeeAge;
                    if (image1 != null && image1.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await image1.CopyToAsync(memoryStream);
                            employees.employeeImage = memoryStream.ToArray();
                        }
                    }
                    await _dbContext.SaveChangesAsync();
                    return employees;
                }
                else
                {
                    throw new InvalidOperationException($"Employee not found");
                }
            }
            else
            {
                throw new InvalidOperationException($"Database context is null");
            }
        }
        catch 
        {
            throw new InvalidOperationException();
        }
    }
    public async Task<Employee> requestVacation(Employee employee, Validation validation)
    {
        try
        {
            if (_dbContext.EmployeeDetails != null)
            {
                var employees = await _dbContext.EmployeeDetails.FindAsync(employee.employeeId);
                if (employees != null)
                {
                    employees.employeeVacationStatus = "UnAssigned";
                    employees.employeeVacationEndTime = employee.employeeVacationEndTime;
                    employees.employeeVacationStartTime = employee.employeeVacationStartTime;
                    employees.employeeVacationReason = employee.employeeVacationReason;
                    await _dbContext.SaveChangesAsync();
                    return employees;
                }
                else
                {
                    throw new InvalidOperationException($"Employee not found");
                }
            }
            else
            {
                throw new InvalidOperationException($"Database context is null");
            }
        }
        catch
        {
            throw new InvalidOperationException();
        }
    }

    public async Task<byte[]> getEmployeeImage(string id)
    {
        if (_dbContext.EmployeeDetails != null)
        {
            var employee = await _dbContext.EmployeeDetails.FirstOrDefaultAsync(e => e.employeeId == id);
            if (employee != null && employee.employeeImage != null)
            {
                return employee.employeeImage;
            }
        }
        return null; 
    }
}