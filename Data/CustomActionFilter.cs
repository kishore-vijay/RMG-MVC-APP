using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
namespace ResourceManageGroup.Data
{
public class CustomActionFilter : Attribute, IActionFilter,IResultFilter
{
    public void OnActionExecuting(ActionExecutingContext context){
        Console.WriteLine("Action Name : "+context.ActionDescriptor.DisplayName);
        Console.WriteLine("OnActionExecuting");
    } 
    public void OnActionExecuted(ActionExecutedContext context){
        Console.WriteLine("Action Name : "+context.ActionDescriptor.DisplayName);
        Console.WriteLine("OnActionExecuted");
    }
    public void OnResultExecuting(ResultExecutingContext context){
        Console.WriteLine("Action Name : "+context.ActionDescriptor.DisplayName);
        Console.WriteLine("OnResultExecuting");
    } 
    public void OnResultExecuted(ResultExecutedContext context){
        Console.WriteLine("Action Name : "+context.ActionDescriptor.DisplayName);
        Console.WriteLine("OnResultExecuted");
    }
}
}



