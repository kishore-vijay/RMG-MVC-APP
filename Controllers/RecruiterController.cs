using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ResourceManageGroup.Models;
using ResourceManageGroup.Data;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net;
using System.Net.Mail;
namespace ResourceManageGroup.Controllers;

public class RecruiterController : Controller
{
    private readonly ApplicationDbContext _dbContext;
    public RecruiterController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    [HttpGet]
    public IActionResult RegisterR()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> RegisterR(Recruiter recruiter)
    {
        if (ModelState.IsValid)
        {
            try
            {
                int emailcount = 0;
                int numcount = 0;
                if (_dbContext.RecruiterDetails != null)
                {
                    emailcount = _dbContext.RecruiterDetails.Where(c => c.recruiterEmail == recruiter.recruiterEmail).Count();
                    numcount = _dbContext.RecruiterDetails.Where(c => c.recruiterNumber == recruiter.recruiterNumber).Count();
                    if (recruiter.recruiterEmail != null && recruiter.recruiterNumber != null && recruiter.recruiterPassword != null){
                        if (emailcount > 0)
                        {
                            ViewData["Message"] = "The User Already Exists !!!!!";
                        }
                        else if (numcount > 0)
                        {
                            ViewData["Message"] = "The User Already Exists !!!!!";
                        }
                        else
                        {
                            _dbContext.Add(recruiter);
                            await _dbContext.SaveChangesAsync();
                            ViewData["Message"] = "Succesfully Registered , Log in to Work";
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(String.Empty, $"Something Went Wrong{exception.Message}");
            }
        }
        ModelState.AddModelError(String.Empty, $"Something Went Wrong Invalid Model");
        return View("HandleError","Error");
    }  
    
    [HttpGet]
    [CustomAuthorize]
    public async Task<IActionResult> EmployeeListR()
    {
        if (_dbContext.EmployeeDetails != null)
        {
            var employees = await _dbContext.EmployeeDetails.ToListAsync();
            return View(employees);
        }
        return View();
    }
    [HttpGet]
    [CustomAuthorize]
    public async Task<IActionResult> VacationReqListR()
    {
        if (_dbContext.EmployeeDetails != null)
        {
            var employees = await _dbContext.EmployeeDetails.Where(e => e.employeeVacationStatus == "UnAssigned" || e.employeeVacationStatus == "Approved").ToListAsync();
            return View(employees);
        }
        return View();
    }
    public IActionResult GetImage(string id)
    {
        if (_dbContext.EmployeeDetails != null)
        {
            var employee = _dbContext.EmployeeDetails.FirstOrDefault(e => e.employeeId == id);
            if (employee != null && employee.employeeImage != null)
            {
                return File(employee.employeeImage, "image/jpeg"); 
            }
        }
        return NotFound();
    }
    [HttpGet]
    [CustomAuthorize]
    public async Task<IActionResult> ManageEmployeeR(string id)
    {
        if (_dbContext.EmployeeDetails != null)
        {
            var employees = await _dbContext.EmployeeDetails.Where(x => x.employeeId == id).FirstOrDefaultAsync();
            return View(employees);
        }
        return View();
    }
    [HttpGet]
    [CustomAuthorize]
    public async Task<IActionResult> ManageRequestR(string id, Employee employee)
    {

        if (_dbContext.EmployeeDetails != null)
        {
            var employees = await _dbContext.EmployeeDetails.Where(x => x.employeeId == id).FirstOrDefaultAsync();
            if (employees != null)
            {
                TempData["reqemail"] = employees.employeeEmail;
            }
            TempData.Keep();
            return View(employees);
        }
        return View();
    }
    [HttpGet]
    [CustomAuthorize]
    public async Task<IActionResult> ApproveReqR(string id, Employee employee)
    {
        if (_dbContext.EmployeeDetails != null)
        {
            var employees = await _dbContext.EmployeeDetails.Where(x => x.employeeId == id).FirstOrDefaultAsync();
            {
                return View(employees);
            }
        }
        return View();
    }
    [HttpPost]
    [CustomAuthorize]
    public async Task<IActionResult> ApproveReqR(Employee employee)
    {
        ModelState.Remove("employeeVacationEndTime");
        ModelState.Remove("employeeTrainingEndTime");
        ModelState.Remove("employeeAge");
        ModelState.Remove("employeeConfirmPassword");
        if (ModelState.IsValid)
        {
            try
            {
                string? myData = TempData["reqemail"] as string;
                if (myData != null)
                {
                    string temp_email = myData;
                    string from, pass, messageBody;
                    MailMessage message = new MailMessage();
                    from = "kishorevijaykumar26@gmail.com";
                    pass = "uqdjacbibysqmufp";
                    messageBody = "Your Vacation have been accepted by Human Resource Manager";
                    if (temp_email != null)
                    {
                        message.To.Add(new MailAddress(temp_email));
                    }
                    else
                    {
                        throw new Exception("User email is null or invalid.");
                    }
                    message.From = new MailAddress(from);
                    message.Body = messageBody;
                    message.Subject = "Vacation Details ";
                    SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                    smtp.EnableSsl = true;
                    smtp.Port = 587;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.UseDefaultCredentials = false;
                    smtp.EnableSsl = true;
                    smtp.Credentials = new NetworkCredential(from, pass);
                    smtp.Send(message);
                }
                if (_dbContext.EmployeeDetails != null)
                {
                    var employees = _dbContext.EmployeeDetails.FirstOrDefault(p => p.employeeId == employee.employeeId);
                    if (employees != null)
                    {
                        employees.employeeVacationStatus = "Approved";
                    }
                    await _dbContext.SaveChangesAsync();
                }
                var url = Url.Action("VacationReqListR", "Recruiter", new { id = employee.employeeId });
                if (url != null)
                {
                    return Redirect(url);
                }
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(String.Empty, $"Something Went Wrong{exception.Message}");
            }
        }
        ModelState.AddModelError(String.Empty, $"Something Went Wrong Invalid Model");
        return View(employee);
    }

    [HttpGet]
    [CustomAuthorize]
    public async Task<IActionResult> RejectReqR(string id, Employee employee)
    {
        if (_dbContext.EmployeeDetails != null)
        {
            var employees = await _dbContext.EmployeeDetails.Where(x => x.employeeId == id).FirstOrDefaultAsync();
            return View(employees);
        }
        return View();
    }
    [HttpPost]
    [CustomAuthorize]
    public async Task<IActionResult> RejectReqR(Employee employee)
    {
        ModelState.Remove("employeeVacationEndTime");
        ModelState.Remove("employeeTrainingEndTime");
        ModelState.Remove("employeeAge");
        ModelState.Remove("employeeConfirmPassword");
        if (ModelState.IsValid)
        {
            try
            {
                string? myData = TempData["reqemail"] as string;
                if (myData != null)
                {
                    string temp_email = myData;
                    string from, pass, messageBody;
                    MailMessage message = new MailMessage();
                    from = "kishorevijaykumar26@gmail.com";
                    pass = "uqdjacbibysqmufp";
                    messageBody = "Your Vacation have been Rejected by Humar Resource Manager";
                    if (temp_email != null)
                    {
                        message.To.Add(new MailAddress(temp_email));
                    }
                    else
                    {
                        throw new Exception("User email is null or invalid.");
                    }
                    message.From = new MailAddress(from);
                    message.Body = messageBody;
                    message.Subject = "Vacation Details ";
                    SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                    smtp.EnableSsl = true;
                    smtp.Port = 587;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.UseDefaultCredentials = false;
                    smtp.EnableSsl = true;
                    smtp.Credentials = new NetworkCredential(from, pass);
                    smtp.Send(message);
                }
                if (_dbContext.EmployeeDetails != null)
                {
                    var employees = _dbContext.EmployeeDetails.FirstOrDefault(p => p.employeeId == employee.employeeId);
                    if (employees != null)
                    {
                        employees.employeeVacationReason = "Not Assigned";
                        employees.employeeVacationStatus = "Not Assigned";
                        employees.employeeVacationStartTime = "Not Assigned";
                        employees.employeeVacationEndTime = "Not Assigned";
                    }
                    await _dbContext.SaveChangesAsync();
                    var url = Url.Action("VacationReqListR", "Recruiter", new { id = employee.employeeId });
                    if (url != null)
                    {
                        return Redirect(url);
                    }
                }
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(String.Empty, $"Something Went Wrong{exception.Message}");
            }
        }
        ModelState.AddModelError(String.Empty, $"Something Went Wrong Invalid Model");
        return View(employee);
    }
    [HttpGet]
    [CustomAuthorize]
    public async Task<IActionResult> AssignDomainR(string id, Employee employee,Validation Validation)
    {
        employee.employeeTechnology = null;
        Validation.List.AddRange(new string[] {"Java","C#","Angular","React","Microsoft Azure","Sql","Amazon Web Service","Python","Machine Learning","Unity"});
        ViewBag.list = Validation.List;
        if (_dbContext.EmployeeDetails != null)
        {
            var employees = await _dbContext.EmployeeDetails.Where(x => x.employeeId == id).FirstOrDefaultAsync();
            return View(employees);
        }
        return View();
    }
    [HttpPost]
    [CustomAuthorize]
    public async Task<IActionResult> AssignDomainR(Employee employee)
    {
        ModelState.Remove("employeeVacationEndTime");
        ModelState.Remove("employeeTrainingEndTime");
        ModelState.Remove("employeeAge");
        ModelState.Remove("employeeConfirmPassword");
        if (ModelState.IsValid)
        {
            try
            {
                if (_dbContext.EmployeeDetails != null)
                {
                    var employees = _dbContext.EmployeeDetails.FirstOrDefault(p => p.employeeId == employee.employeeId);
                    if (employees != null)
                    {
                        employees.employeeTechnology = employee.employeeTechnology;
                    }
                    await _dbContext.SaveChangesAsync();
                    var url = Url.Action("ManageEmployeeR", "Recruiter", new { id = employee.employeeId });
                    if (url != null)
                    {
                        return Redirect(url);
                    }
                }
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(String.Empty, $"Something Went Wrong{exception.Message}");
            }
        }
        ModelState.AddModelError(String.Empty, $"Something Went Wrong Invalid Model");
        return View(employee);
    }
    [HttpGet]
    [CustomAuthorize]
    public async Task<IActionResult> AssignWorkR(string id, Employee employee)
    {
        if (_dbContext.EmployeeDetails != null)
        {
            var employees = await _dbContext.EmployeeDetails.Where(x => x.employeeId == id).FirstOrDefaultAsync();
            return View(employees);
        }
        return View();
    }
    [HttpPost]
    [CustomAuthorize]
    public async Task<IActionResult> AssignWorkR(Employee employee)
    {
        ModelState.Remove("employeeVacationEndTime");
        ModelState.Remove("employeeTrainingEndTime");
        ModelState.Remove("employeeAge");
        ModelState.Remove("employeeConfirmPassword");
        if (ModelState.IsValid)
        {
            try
            {
                if (_dbContext.EmployeeDetails != null)
                {
                    var employees = await _dbContext.EmployeeDetails.Where(x => x.employeeId == employee.employeeId).FirstOrDefaultAsync();
                    if (employees != null)
                    {
                        employees.employeeWorkingStatus = "Bench";
                        employees.employeeTrainerName = "Not Assigned";
                        employees.employeeTrainingStartTime = "Not Assigned";
                        employees.employeeTrainingEndTime = "Not Assigned";
                    }
                    await _dbContext.SaveChangesAsync();
                    var url = Url.Action("ManageEmployeeR", "Recruiter", new { id = employee.employeeId });
                    if (url != null)
                    {
                        return Redirect(url);
                    }
                }
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(String.Empty, $"Something Went Wrong{exception.Message}");
            }
        }
        ModelState.AddModelError(String.Empty, $"Something Went Wrong Invalid Model");
        return View(employee);
    }
    [HttpGet]
    [CustomAuthorize]
    public async Task<IActionResult> AssignTrainingR(string id, Employee employee,Validation validation)
    {
        employee.employeeTrainerName = null;
        validation.List.AddRange(new string[] { "Sabapathi Shanmugam","Savitha Ragunathan","Silpa Madhusoodanan","Anitha Manogaran","Saraswathi Sathiah","Jaya Ethiraj","Merlin Paul","Naveen Subramaniam"});
        ViewBag.list = validation.List;
        if (_dbContext.EmployeeDetails != null)
        {
            var employees = await _dbContext.EmployeeDetails.Where(x => x.employeeId == id).FirstOrDefaultAsync();
            return View(employees);
        }
        return View();
    }
    [HttpPost]
    [CustomAuthorize]
    public async Task<IActionResult> AssignTrainingR(Employee employee,Validation validation)
    {
        ModelState.Remove("employeeVacationEndTime");
        ModelState.Remove("employeeAge");
        ModelState.Remove("employeeConfirmPassword");
        validation.List.AddRange(new string[] { "Sabapathi Shanmugam","Savitha Ragunathan","Silpa Madhusoodanan","Anitha Manogaran","Saraswathi Sathiah","Jaya Ethiraj","Merlin Paul","Naveen Subramaniam"});
        ViewBag.list = validation.List;
        if (ModelState.IsValid)
        {
            try
            {
                if (_dbContext.EmployeeDetails != null)
                {
                    var employees = await _dbContext.EmployeeDetails.Where(x => x.employeeId == employee.employeeId).FirstOrDefaultAsync();
                    if (employees != null)
                    {
                        employees.employeeWorkingStatus = "Training";
                        employees.employeeTrainerName = employee.employeeTrainerName;
                        employees.employeeTrainingStartTime = employee.employeeTrainingStartTime;
                        employees.employeeTrainingEndTime = employee.employeeTrainingEndTime;
                    }
                    await _dbContext.SaveChangesAsync();
                    var url = Url.Action("ManageEmployeeR", "Recruiter", new { id = employee.employeeId });
                    if (url != null)
                    {
                        return Redirect(url);
                    }
                }
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(String.Empty, $"Something Went Wrong{exception.Message}");
            }
        }
        ModelState.AddModelError(String.Empty, $"Something Went Wrong Invalid Model");
        return View(employee);
    }
    [HttpGet]
    [CustomAuthorize]
    public async Task<IActionResult> RemoveEmployeeR(string id, Employee employee)
    {
        if (_dbContext.EmployeeDetails != null)
        {
            var employees = await _dbContext.EmployeeDetails.Where(x => x.employeeId == id).FirstOrDefaultAsync();
            return View(employees);
        }
        return View();
    }
    [HttpPost]
    [CustomAuthorize]
    public async Task<IActionResult> RemoveEmployeeR(Employee employee)
    {
        ModelState.Remove("employeeVacationEndTime");
        ModelState.Remove("employeeTrainingEndTime");
        ModelState.Remove("employeeAge");
        ModelState.Remove("employeeConfirmPassword");
        if (ModelState.IsValid)
        {
            try
            {
                if (_dbContext.EmployeeDetails != null)
                {
                    var employees = _dbContext.EmployeeDetails.FirstOrDefault(p => p.employeeId == employee.employeeId);
                    if (employees != null)
                    {
                        _dbContext.Remove(employees);
                        await _dbContext.SaveChangesAsync();
                        return RedirectToAction("EmployeeListR");
                    }
                }
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(String.Empty, $"Something Went Wrong{exception.Message}");
            }
        }
        ModelState.AddModelError(String.Empty, $"Something Went Wrong Invalid Model");
        return View(employee);
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

