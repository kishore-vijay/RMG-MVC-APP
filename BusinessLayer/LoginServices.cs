using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ResourceManageGroup.Models;
using ResourceManageGroup.Data;
using Microsoft.EntityFrameworkCore;
using System.Data;
namespace ResourceManageGroup.Services;
public class LoginServices
{
    private readonly ApplicationDbContext _dbContext;
    public LoginServices(ApplicationDbContext dbContext,IWebHostEnvironment hostEnvironment)
    {
        _dbContext = dbContext;
    }

    private string GetemployeePassword(string userEmail)
    {
        return _dbContext.EmployeeDetails?
            .Where(c => c.employeeEmail == userEmail)
            .Select(c => c.employeePassword)
            .FirstOrDefault() ?? string.Empty;
    }
    private string GetmanagerPassword(string userEmail)
    {
        return _dbContext.ManagerDetails?
            .Where(c => c.managerEmail == userEmail)
            .Select(c => c.managerPassword)
            .FirstOrDefault() ?? string.Empty;
    }
    private string GetrecruiterPassword(string userEmail)
    {
        return _dbContext.RecruiterDetails?
            .Where(c => c.recruiterEmail == userEmail)
            .Select(c => c.recruiterPassword)
            .FirstOrDefault() ?? string.Empty;
    }
    private string GetUserRole(string userEmail)
    {
        if (_dbContext.EmployeeDetails?.Any(c => c.employeeEmail == userEmail) ?? false)
        {
            return "Employee";
        }
        if (_dbContext.ManagerDetails?.Any(c => c.managerEmail == userEmail) ?? false)
        {
            return "Manager";
        }
        if (_dbContext.RecruiterDetails?.Any(c => c.recruiterEmail == userEmail) ?? false)
        {
            return "Recruiter";
        }

        return "Unknown";
    }

}