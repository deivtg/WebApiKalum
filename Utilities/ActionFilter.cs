using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApiKalum.Utilities
{
    public class ActionFilter : IActionFilter
    {
        private readonly ILogger<ActionFilter> Logger;
        public ActionFilter(ILogger<ActionFilter> logger)
        {
            Logger = logger;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            Logger.LogInformation("Esto se ejecuta despues de la accion realizada");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            Logger.LogInformation("Esto se ejecuta antes de la accion");
        }
    }
}