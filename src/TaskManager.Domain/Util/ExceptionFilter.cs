using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TaskManager.Domain.Util;

namespace TaskManager.Domain.Util
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is DomainException domainException)
            {
                context.Result = new JsonResult(new { message = domainException.Message })
                {
                    StatusCode = 400 
                };
            }
            else
            {
                context.Result = new JsonResult(new { message = "Ocorreu um erro inesperado. Tente novamente mais tarde." })
                {
                    StatusCode = 500
                };
            }

            context.ExceptionHandled = true;
        }

    }
}
