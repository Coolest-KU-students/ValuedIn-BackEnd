using ValuedInBE.System.Exceptions;

namespace ValuedInBE.System.WebConfigs.Middleware
{
    public class ExceptionTranslationMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (HttpStatusCarryingException ex)
            {
                context.Response.StatusCode = (int) ex.StatusCode;
                context.Response.ContentType = "text/html";
                await context.Response.WriteAsync(ex.Message); //TODO: Dictionary; logger
            } //TODO: catch else
        }
    }
}
