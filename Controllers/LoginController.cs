using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ResourceManageGroup.Models;
using ResourceManageGroup.Data;
using ResourceManageGroup.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net;
using System.Net.Mail;
using AliasConsole=System.Console;
namespace ResourceManageGroup.Controllers;
public class LoginController : Controller
{
    private readonly ApplicationDbContext _dbContext;
    public LoginController(ApplicationDbContext dbContext,IWebHostEnvironment hostEnvironment)
    {
        _dbContext = dbContext;
    } 
    [HttpGet]
    public IActionResult Logins(Login login)
    {
        var cookieValue = Request.Cookies["MyAuthCookie"];
        if (cookieValue != null)
        {
            var decodedCookie = cookieValue.Split(":");
            login.userEmail = decodedCookie[0];
            login.userPassword = decodedCookie[1];
            ViewBag.email=login.userEmail;
            ViewBag.password=login.userPassword;
        }
        return View();
    }
    [HttpPost]
    public IActionResult Logins(Login login,Validation validation)
    {
        if (ModelState.IsValid)
        {
            try
            {
                if(login.userEmail!=null){
                var dbpassworde = GetemployeePassword(login.userEmail);
                var dbpasswordm = GetmanagerPassword(login.userEmail);
                var dbpasswordh = GetrecruiterPassword(login.userEmail);
                if (!string.IsNullOrEmpty(dbpassworde) && dbpassworde.Equals(login.userPassword))
                {
                    //Storing values of employee model in json file named "users" in session
                    HttpContext.Session.SetObjectAsJson("users", login);
                            //Cookies 
                    var cookieValue = $"{login.userEmail}:{login.userPassword}";
                    var cookieOptions = new CookieOptions
                    {
                        Expires = DateTimeOffset.Now.AddDays(30),
                        HttpOnly = true
                    };
                    Response.Cookies.Append("MyAuthCookie", cookieValue, cookieOptions);
                    string ? empId;
                    if(_dbContext.EmployeeDetails!=null){
                        empId = _dbContext.EmployeeDetails.Where(c => c.employeeEmail == login.userEmail).Select(c => c.employeeId).FirstOrDefault();
                        TempData["EmpId"]=Convert.ToString(empId);
                        return RedirectToAction("EmployeeViewE", "employee", new { employeeId = empId });
                    }
                }
                else if (!string.IsNullOrEmpty(dbpasswordm) && dbpasswordm.Equals(login.userPassword))
                {
                    //Storing values of employee model in json file named "users" in session
                    HttpContext.Session.SetObjectAsJson("users", login);
                    //Cookies 
                    var cookieValue = $"{login.userEmail}:{login.userPassword}";
                    var cookieOptions = new CookieOptions
                    {
                        Expires = DateTimeOffset.Now.AddDays(30),
                        HttpOnly = true
                    };
                    Response.Cookies.Append("MyAuthCookie", cookieValue, cookieOptions);
                    return RedirectToAction("ProjectListM", "Manager");
                }
                else if (!string.IsNullOrEmpty(dbpasswordh) && dbpasswordh.Equals(login.userPassword))
                {
                    //Storing values of employee model in json file named "users" in session
                    HttpContext.Session.SetObjectAsJson("users", login);
                    //Cookies 
                    var cookieValue = $"{login.userEmail}:{login.userPassword}";
                    var cookieOptions = new CookieOptions
                    {
                        Expires = DateTimeOffset.Now.AddDays(30),
                        HttpOnly = true
                    };
                    Response.Cookies.Append("MyAuthCookie", cookieValue, cookieOptions);
                    return RedirectToAction("EmployeeListR", "Recruiter"); 
                }
            }
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(String.Empty, $"Something Went Wrong: {exception.Message}");
            }
        }
        return View(login);
    }
    
    [HttpGet]
    public IActionResult VerifyUser()
    {
        return View();
    }
    [HttpPost]
    public IActionResult VerifyUser(Validation validation,Login login)
    {
        string ? myData = TempData["verificationcode"] as String;
        string ? email = TempData["email"] as String;
        if(myData==validation.verifyCode){
            login.userEmail=email;
            HttpContext.Session.SetObjectAsJson("users", login);
            if(login.userEmail!=null){
                string userRole = GetUserRole(login.userEmail);
                if (userRole == "Manager")
                {
                    // Redirect to Manager's page
                    return RedirectToAction("ProjectListM", "Manager");
                }
                else if (userRole == "Recruiter")
                {
                    // Redirect to Recruiter's page
                    return RedirectToAction("EmployeeListR", "Recruiter");
                }
                else if (userRole == "Employee")
                {
                    // Redirect to Employee's page
                    string ? empId;
                    if(_dbContext.EmployeeDetails!=null){
                        empId = _dbContext.EmployeeDetails.Where(c => c.employeeEmail == login.userEmail).Select(c => c.employeeId).FirstOrDefault();
                        TempData["EmpId"]=Convert.ToString(empId);
                        return RedirectToAction("EmployeeViewE", "employee", new { employeeId = empId });
                    }
                }
            }
        }
        else{
            ViewBag.message="OTP is Invalid";
        }
        return View();
    }

    [HttpGet]
    public IActionResult forgotPassword()
    {
        return View();
    }

    [HttpPost]
    public IActionResult forgotPassword(Login login)
    {
        if(ModelState.IsValid){
            try{
                int count = 0;
                count = _dbContext.EmployeeDetails?.Count(c => c.employeeEmail == login.userEmail) ?? 0;
                if (count == 0)
                {
                    count = _dbContext.ManagerDetails?.Count(c => c.managerEmail == login.userEmail) ?? 0;
                }
                if (count == 0)
                {
                    count = _dbContext.RecruiterDetails?.Count(c => c.recruiterEmail == login.userEmail) ?? 0;
                }
                if(login.userEmail!=null){
                    if(count>0){
                        try
                        {
                            Random random = new Random();
                            string code = random.Next(100000, 999999).ToString();
                            TempData["verificationcode"] = code;
                            TempData["email"] = login.userEmail;
                            string from, pass, messageBody;
                            MailMessage message = new MailMessage();
                            from = "kishorevijaykumar26@gmail.com";
                            pass = "uqdjacbibysqmufp";
                            messageBody = "Your Verification Code is " + code;
                            if(login.userEmail != null)
                            {
                                message.To.Add(new MailAddress(login.userEmail));
                            }
                            else
                            {
                                throw new Exception("User email is null or invalid.");
                            }
                            message.From = new MailAddress(from);
                            message.Body = messageBody;
                            message.Subject = "Otp To Login ";
                            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                            smtp.EnableSsl = true;
                            smtp.Port = 587;
                            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                            smtp.UseDefaultCredentials = false;
                            smtp.EnableSsl = true;
                            smtp.Credentials = new NetworkCredential(from, pass);                  
                            smtp.Send(message);
                            ViewData["Message"] = "Verification Code sent successfully";
                            return RedirectToAction("VerifyUser", "Login");
                        }
                        catch (Exception ex)
                        {
                            ViewData["Message"] = "Error sending verification code: " + ex.Message;
                        }
                    }
                }
                else{
                    ViewData["Message"]="Check Your E-Mail";
                }
            }
            catch(Exception exception){
                ModelState.AddModelError(String.Empty,$"Something Went Wrong{exception.Message}"); 
            }
        }
        return View(login);
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
    [CustomAuthorize]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Logins","Login");
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}