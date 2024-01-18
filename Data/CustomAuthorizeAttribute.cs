using ResourceManageGroup.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
namespace ResourceManageGroup.Data
{
public class CustomAuthorizeAttribute : Attribute ,IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.Session.GetObjectFromJson<Login>("users");
        if (user == null)
        {
            Console.WriteLine("Error: users is null");
            context.Result = new RedirectToActionResult("Logins", "Login", null);
            return;
        }
        Console.WriteLine($"users: {JsonConvert.SerializeObject(user)}");
    }
}
}