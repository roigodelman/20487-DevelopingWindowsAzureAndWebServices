using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;


namespace BlueYonder.Companion.Controllers.ActionFilters
{
    public class ModelValidationAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, new HttpError(actionContext.ModelState, true));
        }
    }
}
