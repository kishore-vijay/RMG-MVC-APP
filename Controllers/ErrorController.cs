using Microsoft.AspNetCore.Mvc;

public class ErrorController : Controller 
{
    [Route("Error/{statusCode}")]
    public IActionResult HandleErrorCode(int statusCode)
    {
        switch(statusCode){
            case 404:
                ViewBag.ErrorMessage="Page Not Found";
                ViewBag.Code="404";
                break;
            case 204:
                ViewBag.ErrorMessage="No Content Found";
                ViewBag.Code="204";
                break;
            case 500:
                ViewBag.ErrorMessage="Internal Server Error";
                ViewBag.Code="500";
                break;
        }
        return View("Error");
    }
}
