using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace NesFactApiV4.Atributos
{
    [AttributeUsage(validOn: AttributeTargets.Class)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EMPRESA")]
    public class Cm4PermisoAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var currentUser = context.HttpContext.User;
            var claims = currentUser.Claims.ToList();
            if (claims.Count != 0)
            {
                if (claims[3].Value != "CM4")
                {
                    context.Result = new ContentResult()
                    {
                        StatusCode = 401,
                        Content = "CM4 no autorizado"
                    };

                }
            }


            var appSettings = context.HttpContext.RequestServices.GetRequiredService<AplicationDbContext>();

            var empresaBloquiada = await appSettings.PermisosEmpresas.
                Include(x=>x.FkEmpresaNavigation).
                FirstOrDefaultAsync(idempresa => idempresa.Id.ToString()== claims[6].Value.ToString());


            if (empresaBloquiada != null)
            {
                if (empresaBloquiada.BloquiarEmpresa.Equals(true))
                {
                    context.Result = new ContentResult()
                    {
                        StatusCode = 401,
                        Content = "CM4 no autorizado"
                    };
                    return;

                }

            }
            await next();
        }




    }


}
