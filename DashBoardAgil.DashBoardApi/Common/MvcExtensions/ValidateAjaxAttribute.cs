using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace ComprasNet.Common.MvcExtensions
{
    /// <summary>
    /// Validação Ajax para Actions.
    /// </summary>
    public class ValidateAjaxAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.HttpContext.Request.IsAjaxRequest())
                return;

            var modelState = filterContext.Controller.ViewData.ModelState;
            
            if (modelState.IsValid) 
                return;

            var errorModel =
                from x in modelState.Keys
                where modelState[x].Errors.Count > 0
                select new
                {
                    key = x,
                    msg = modelState[x].Errors[0].ErrorMessage.Contains("is not valid for") ? "Formato Inválido." : modelState[x].Errors[0].ErrorMessage
                };
            filterContext.Result = new JsonResult
            {
                Data = errorModel
            };
            filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
        }
    }
}