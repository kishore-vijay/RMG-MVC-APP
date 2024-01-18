using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ResourceManageGroup.Models;
using ResourceManageGroup.Data;
using ResourceManageGroup.Services;

namespace ResourceManageGroup.Controllers;
public class EmployeeController : Controller
{
    private readonly ApplicationDbContext _dbContext;
    private readonly EmployeeServices _employeeServices;
    public EmployeeController(ApplicationDbContext dbContext,EmployeeServices employeeServices,IWebHostEnvironment hostEnvironment)
    {
        _dbContext = dbContext;
        _employeeServices = employeeServices;
    } 
    [HttpGet]
    public IActionResult RegisterE()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> RegisterE(Employee employee,Validation validation)
    {
        ModelState.Remove("employeeVacationEndTime");
        ModelState.Remove("employeeTrainingEndTime");
        ModelState.Remove("employeeVacationReason");
        if(ModelState.IsValid){
            try{
                var message = await _employeeServices.registerUser(employee, validation);
                ViewData["Message"] = message;
            }
            catch(Exception exception){
                ModelState.AddModelError(String.Empty,$"Something Went Wrong{exception.Message}"); 
            }
        }
        ModelState.AddModelError(String.Empty,$"Something Went Wrong Invalid Model"); 
        return View(employee);
    }
    
    [HttpGet("Employee/EmployeeViewE/{employeeId:regex(^\\d{{2}}EM\\d{{2,3}}$)}")]
    //[HttpGet("Employee/EmployeeViewE/{employeeId:length(8,9)}")]
    [CustomAuthorize]
    public async Task<IActionResult> EmployeeViewE(string employeeId)
    {
        var employee = await _employeeServices.getEmployeeById(employeeId);
        return View(employee);
    }
    
    
    [HttpGet]
    [CustomAuthorize]
    public async Task<IActionResult> EditEmployeeE(Employee employee)
    {
        string ? myData = TempData["EmpId"] as String;
        TempData.Keep("EmpId");
        employee.employeeId=myData;
        if(_dbContext.EmployeeDetails!=null){
            var employees = await _employeeServices.getEmployeeById(employee.employeeId);
            return View(employees);
        }
        return View();
    }
    [HttpPost]
    [CustomAuthorize]
    public async Task<IActionResult> EditEmployeeE(Employee employee, IFormFile image1)
    {
        string? myData = TempData["EmpId"] as string;
        TempData.Keep("EmpId");
        employee.employeeId = myData;
        ModelState.Remove("employeeAge");
        ModelState.Remove("employeePassword");
        ModelState.Remove("employeeConfirmPassword");
        ModelState.Remove("employeeVacationEndTime");
        ModelState.Remove("employeeTrainingEndTime");
        ModelState.Remove("employeeConfirmPassword");
        if (ModelState.IsValid)
        {
            try
            {
                await _employeeServices.editEmployee(employee,image1);
                var url = Url.Action("EmployeeViewE", "Employee", new { id = employee.employeeId });
                if (url != null)
                {
                    return Redirect(url);
                }
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, $"Something went wrong: {exception.Message}");
            }
        }
        return View(employee);
    }
    [HttpGet]
    [CustomAuthorize]
    public async Task<IActionResult> VacationRequestE(Employee employee)
    {
        string ? myData = TempData["EmpId"] as String;
        TempData.Keep("EmpId");
        employee.employeeId=myData;
        if(_dbContext.EmployeeDetails!=null){
            var employees = await _employeeServices.getEmployeeById(employee.employeeId);
            return View(employees);
        }
        return View();
    }
    [HttpPost]
    [CustomAuthorize]
    public async Task<IActionResult> VacationRequestE(Employee employee,Validation validation)
    {
        string ? myData = TempData["EmpId"] as string;
        TempData.Keep("EmpId");
        employee.employeeId = myData;
        ModelState.Remove("employeeTrainingEndTime");
        ModelState.Remove("employeeAge");
        ModelState.Remove("employeeConfirmPassword");
        if (ModelState.IsValid)
        {
            try
            {
                await _employeeServices.requestVacation(employee,validation);
                var url = Url.Action("EmployeeViewE", "Employee", new { id = employee.employeeId });
                if (url != null)
                {
                    return Redirect(url);
                }
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, $"Something went wrong: {exception.Message}");
            }
        }
        return View(employee);
    }

    public async Task<IActionResult> GetImage(string id)
    {
        var imageBytes = await _employeeServices.getEmployeeImage(id);
        if (imageBytes != null)
        {
            return File(imageBytes, "image/jpeg");
        }
        return NotFound();
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}