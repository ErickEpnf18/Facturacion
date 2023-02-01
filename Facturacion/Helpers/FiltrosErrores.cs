using Microsoft.AspNetCore.Mvc.Filters;

namespace NesFactApiV4.Helpers
{
    public class FiltrosErrores : ExceptionFilterAttribute
    {
        private readonly ILogger<FiltrosErrores> logger;

        public FiltrosErrores(ILogger<FiltrosErrores> logger)
        {
            this.logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            logger.LogError(context.Exception, context.Exception.Message);
            base.OnException(context);
        }
    }
}
