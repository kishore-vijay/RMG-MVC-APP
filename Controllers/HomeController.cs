using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ResourceManageGroup.Models;
using ResourceManageGroup.Data;

namespace ResourceManageGroup.Controllers;
public class HomeController : Controller
{ 
    private readonly ApplicationDbContext _dbContext;
    public HomeController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    [Route("")]//Empty string in route for action means it is the default route for the HomeController
    //[CustomActionFilter]
    public IActionResult Index()
    {
        //throw new Exception("Deliberately throwing an Exception");
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
