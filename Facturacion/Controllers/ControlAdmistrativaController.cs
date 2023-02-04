using AutoMapper;
using Facturacion.Context;
using Facturacion.Model;
using Facturacion.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Facturacion.Controllers
{
    [ApiController]
    [Route("api/controlAdministrativo/cuentas")]
    public class ControlAdmistrativaController : Controller
    {
        DateTime currentTimePacific = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time"));

        private readonly UserManager<NetUserAditional> _userManager;
        private readonly SignInManager<NetUserAditional> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly AplicationDbContext context;
        private readonly IMapper mapper;

        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly IEmailSender emailSender;

        public ControlAdmistrativaController(UserManager<NetUserAditional> userManager,
            SignInManager<NetUserAditional> signInManager, IConfiguration configuration,
            AplicationDbContext context, IMapper mapper,


            IAlmacenadorArchivos almacenadorArchivos, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            this.context = context;
            this.mapper = mapper;

            this.almacenadorArchivos = almacenadorArchivos;
            this.emailSender = emailSender;
        
        }
        /// <summary>
        /// Login para todos los tipos de Usuarios del Sistema
        /// </summary>
        /// <param name="loginUsario"></param>
        /// <returns></returns>

        [AllowAnonymous]
        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ViewUsuarioToken>> Login([FromBody] LoginUsario loginUsario)
        {

            var identityUser = await _userManager.FindByEmailAsync(loginUsario.Email);
            if (identityUser == null)
                identityUser = await _userManager.FindByNameAsync(loginUsario.Email);


            if (identityUser != null)
            {
                var confirmMail = await _userManager.IsEmailConfirmedAsync(identityUser);
                if (confirmMail == true)
                {
                    var resultado = await _signInManager.PasswordSignInAsync(identityUser,
                        loginUsario.Password, isPersistent: true, lockoutOnFailure: false);
                    if (resultado.Succeeded)
                    {
                        var datosRol = await context.UserRoles.FirstOrDefaultAsync(x => x.UserId.Equals(identityUser.Id));

                        var rol = await context.Roles.FirstOrDefaultAsync(x => x.Id.Equals(datosRol.RoleId));

                        if (rol.Name.Equals("AdminEmpresaDueno"))
                            return await ConstruirToken(identityUser.Id);
                        if (rol.Name.Equals("UsuarioEmpresaCreada"))
                            return await ConstruirToken(identityUser.Id);
                        if (rol.Name.Equals("AdminEmpresa"))
                            return await ConstruirToken(identityUser.Id);
                        if (rol.Name.Equals("UsuarioEmpresa"))
                            return await ConstruirToken(identityUser.Id);
                        if (rol.Name.Equals("LocalUsuarioEmpresaCreada"))
                            return await ConstruirToken(identityUser.Id);
                        if (rol.Name.Equals("AdminEmpresaDuenoUsuario"))
                            return await ConstruirToken(identityUser.Id);
                        if (rol.Name.Equals("AdminNessoft"))
                            return await ConstruirToken(identityUser.Id);
                        if (rol.Name.Equals("LocalEmpresaCreada"))
                            return await ConstruirToken(identityUser.Id);
                        if (rol.Name.Equals("LocalAdminEmpresaDuenoUsuario"))
                            return await ConstruirToken(identityUser.Id);
                        if (rol.Name.Equals("AdminEmpresaCreada"))
                            return await ConstruirToken(identityUser.Id);
                        


                    }
                    else
                    {
                        return BadRequest("Intento de inicio de sesión no válido");
                    }
                }
                else
                {
                    return BadRequest("Email No confirmado");

                }
            }
            else
            {
                return BadRequest("Usuario No existe");
            }
            return BadRequest("Usuario No existe");
        }



        private async Task<ViewUsuarioToken> ConstruirToken(string idNetUser)
        {
            var identityUser = await _userManager.FindByIdAsync(idNetUser);
            int identityUserFkEmpresa = 0, identityUserFkEmpresaCreada = 0, identityUserFkLocal = 0, identityUserFkUsuario = 0;
            if (identityUser.FkEmpresa != null)
            {
                identityUserFkEmpresa = (int)identityUser.FkEmpresa;
            }
            if (identityUser.FkEmpresaCreada != null)
            {
                identityUserFkEmpresaCreada = (int)identityUser.FkEmpresaCreada;
            }
            if (identityUser.FkLocal != null)
            {
                identityUserFkLocal = (int)identityUser.FkLocal;
            }
            if (identityUser.FkUsuario != null)
            {
                identityUserFkUsuario = (int)identityUser.FkUsuario;
            }

            var datosRol = await context.UserRoles.FirstOrDefaultAsync(x => x.UserId.Equals(idNetUser));

            var rol = await context.Roles.FirstOrDefaultAsync(x => x.Id.Equals(datosRol.RoleId));



            switch (rol.Name)
            {            // public const string AdminNessoft = "AdminNessoft";
                case "AdminNessoft":

                    var claims = new List<Claim>() {
                                   new Claim(ClaimTypes.NameIdentifier,identityUser.Id),
                                    new Claim(ClaimTypes.NameIdentifier, identityUserFkEmpresa.ToString()),
                                    new Claim(ClaimTypes.NameIdentifier, identityUserFkEmpresaCreada.ToString()),
                                    new Claim(ClaimTypes.NameIdentifier, identityUserFkLocal.ToString()),
                                    new Claim(ClaimTypes.NameIdentifier, identityUserFkUsuario.ToString()),
                                    new Claim(ClaimTypes.Role,rol.Name),
                                    new Claim(ClaimTypes.Email, identityUser.Email),

                                };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwt:key"]));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var expiracion = DateTime.UtcNow.AddMonths(6);

                    JwtSecurityToken token = new JwtSecurityToken(
                            issuer: null,
                            audience: null,
                            claims: claims,
                        expires: expiracion,
                        signingCredentials: creds);

                    return new ViewUsuarioToken()
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(token),
                        Expiracion = expiracion,
                        UriImagen = ""

                    };

                case "AdminEmpresaDueno":
                    if (identityUserFkEmpresa == 0 && identityUserFkUsuario == 0)
                    {
                        return new ViewUsuarioToken()
                        {
                            Error = "No existe Empresa"

                        };
                    }
                    var empresa = await context.Empresas.FirstOrDefaultAsync(x => x.Id.Equals(identityUserFkEmpresa));
                    var claimsEmpresa = new List<Claim>() {
                                    new Claim(ClaimTypes.NameIdentifier,identityUser.Id),
                                    new Claim(ClaimTypes.NameIdentifier, identityUserFkEmpresa.ToString()),
                                    new Claim(ClaimTypes.NameIdentifier, identityUserFkEmpresaCreada.ToString()),
                                    new Claim(ClaimTypes.NameIdentifier, identityUserFkLocal.ToString()),
                                    new Claim(ClaimTypes.NameIdentifier, identityUserFkUsuario.ToString()),
                                    new Claim(ClaimTypes.Role,rol.Name),
                                    new Claim(ClaimTypes.Email, identityUser.Email),

                                };

                    var keyEmpresa = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwt:key"]));
                    var credsEmpresa = new SigningCredentials(keyEmpresa, SecurityAlgorithms.HmacSha256);

                    var expiracionEmpresa = DateTime.UtcNow.AddMonths(6);

                    JwtSecurityToken tokenEmpresa = new JwtSecurityToken(
                            issuer: null,
                            audience: null,
                            claims: claimsEmpresa,
                        expires: expiracionEmpresa,
                        signingCredentials: credsEmpresa);


                    return new ViewUsuarioToken()
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(tokenEmpresa),
                        Expiracion = expiracionEmpresa,
                        UriImagen = empresa.EmpresaImagen

                    };
                case "AdminEmpresaDuenoUsuario":

                    if (identityUserFkEmpresa == 0 && identityUserFkUsuario == 0)
                    {
                        return new ViewUsuarioToken()
                        {
                            Error = "No existe Empresa"

                        };
                    }
                    var empresaAE = await context.Empresas.FirstOrDefaultAsync(x => x.Id.Equals(identityUserFkEmpresa));
                    var claimsempresaCreadaa = new List<Claim>() {
                                    new Claim(ClaimTypes.NameIdentifier,identityUser.Id),
                                    new Claim(ClaimTypes.NameIdentifier, identityUserFkEmpresa.ToString()),
                                    new Claim(ClaimTypes.NameIdentifier, identityUserFkEmpresaCreada.ToString()),
                                    new Claim(ClaimTypes.NameIdentifier, identityUserFkLocal.ToString()),
                                    new Claim(ClaimTypes.NameIdentifier, identityUserFkUsuario.ToString()),
                                    new Claim(ClaimTypes.Role,rol.Name),
                                    new Claim(ClaimTypes.Email, identityUser.Email),

                                };

                    var keyempresaCreadaa = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwt:key"]));
                    var credsempresaCreadaa = new SigningCredentials(keyempresaCreadaa, SecurityAlgorithms.HmacSha256);

                    var expiracionempresaCreadaa = DateTime.UtcNow.AddMonths(6);

                    JwtSecurityToken tokenempresaCreadaa = new JwtSecurityToken(
                            issuer: null,
                            audience: null,
                            claims: claimsempresaCreadaa,
                        expires: expiracionempresaCreadaa,
                        signingCredentials: credsempresaCreadaa);


                    return new ViewUsuarioToken()
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(tokenempresaCreadaa),
                        Expiracion = expiracionempresaCreadaa,
                        UriImagen = empresaAE.EmpresaImagen
                    };
                case "LocalAdminEmpresaDueno":
                    //public const string AdminEmpresaDueno = "AdminEmpresaDueno";


                    if (identityUserFkLocal == 0 && identityUserFkEmpresa == 0 && identityUserFkUsuario == 0)
                    {
                        return new ViewUsuarioToken()
                        {
                            Error = "No existe Empresa"

                        };
                    }
                    var empresaL = await context.Empresas.FirstOrDefaultAsync(x => x.Id.Equals(identityUserFkEmpresa));
                    var claimsLocalAdminEmpresaDueno = new List<Claim>() {
                                    new Claim(ClaimTypes.NameIdentifier,identityUser.Id),
                                    new Claim(ClaimTypes.NameIdentifier, identityUserFkEmpresa.ToString()),
                                    new Claim(ClaimTypes.NameIdentifier, identityUserFkEmpresaCreada.ToString()),
                                    new Claim(ClaimTypes.NameIdentifier, identityUserFkLocal.ToString()),
                                    new Claim(ClaimTypes.NameIdentifier, identityUserFkUsuario.ToString()),
                                    new Claim(ClaimTypes.Role,rol.Name),
                                    new Claim(ClaimTypes.Email, identityUser.Email),

                                };

                    var keyEmpresaLocalAdminEmpresaDueno = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwt:key"]));
                    var credsEmpresaLocalAdminEmpresaDueno = new SigningCredentials(keyEmpresaLocalAdminEmpresaDueno, SecurityAlgorithms.HmacSha256);

                    var expiracionEmpresaLocalAdmin = DateTime.UtcNow.AddMonths(6);

                    JwtSecurityToken tokenEmpresaLocalAdmin = new JwtSecurityToken(
                            issuer: null,
                            audience: null,
                            claims: claimsLocalAdminEmpresaDueno,
                        expires: expiracionEmpresaLocalAdmin,
                        signingCredentials: credsEmpresaLocalAdminEmpresaDueno);


                    return new ViewUsuarioToken()
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(tokenEmpresaLocalAdmin),
                        Expiracion = expiracionEmpresaLocalAdmin,
                        UriImagen = empresaL.EmpresaImagen
                    };


                case "LocalAdminEmpresaDuenoUsuario":


                    if (identityUserFkLocal == 0)
                    {
                        return new ViewUsuarioToken()
                        {
                            Error = "No existe Local"

                        };
                    }
                    var empresaLDU = await context.Empresas.FirstOrDefaultAsync(x => x.Id.Equals(identityUserFkEmpresa));
                    var claimsLocalAdminEmpresaDuenoUsuario = new List<Claim>() {
                                    new Claim(ClaimTypes.NameIdentifier,identityUser.Id),
                                    new Claim(ClaimTypes.NameIdentifier, identityUserFkEmpresa.ToString()),
                                    new Claim(ClaimTypes.NameIdentifier, identityUserFkEmpresaCreada.ToString()),
                                    new Claim(ClaimTypes.NameIdentifier, identityUserFkLocal.ToString()),
                                    new Claim(ClaimTypes.NameIdentifier, identityUserFkUsuario.ToString()),
                                    new Claim(ClaimTypes.Role,rol.Name),
                                    new Claim(ClaimTypes.Email, identityUser.Email),

                                };


                    var keylocal = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwt:key"]));
                    var credslocal = new SigningCredentials(keylocal, SecurityAlgorithms.HmacSha256);

                    var expiracionlocal = DateTime.UtcNow.AddMonths(6);

                    JwtSecurityToken tokenLocal = new JwtSecurityToken(
                            issuer: null,
                            audience: null,
                            claims: claimsLocalAdminEmpresaDuenoUsuario,
                        expires: expiracionlocal,
                        signingCredentials: credslocal);


                    return new ViewUsuarioToken()
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(tokenLocal),
                        Expiracion = expiracionlocal,
                        UriImagen = empresaLDU.EmpresaImagen
                    };

                case "AdminEmpresa":


                    if (identityUserFkEmpresa == 0 && identityUserFkUsuario == 0)
                    {
                        return new ViewUsuarioToken()
                        {
                            Error = "No existe AdminEmpresa"

                        };
                    }
                    var empresaAdmin = await context.Empresas.FirstOrDefaultAsync(x => x.Id.Equals(identityUserFkEmpresa));
                    var claimsEmpresaUsuario = new List<Claim>() {
                                    new Claim(ClaimTypes.NameIdentifier,identityUser.Id),
                                    new Claim(ClaimTypes.NameIdentifier, identityUserFkEmpresa.ToString()),
                                    new Claim(ClaimTypes.NameIdentifier, identityUserFkEmpresaCreada.ToString()),
                                    new Claim(ClaimTypes.NameIdentifier, identityUserFkLocal.ToString()),
                                    new Claim(ClaimTypes.NameIdentifier, identityUserFkUsuario.ToString()),
                                    new Claim(ClaimTypes.Role,rol.Name),
                                    new Claim(ClaimTypes.Email, identityUser.Email),

                                };

                    var keyEmpresaUsuario = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwt:key"]));
                    var credsEmpresaUsuario = new SigningCredentials(keyEmpresaUsuario, SecurityAlgorithms.HmacSha256);

                    var expiracionkeyEmpresaUsuario = DateTime.UtcNow.AddMonths(6);

                    JwtSecurityToken tokenkeyEmpresaUsuario = new JwtSecurityToken(
                            issuer: null,
                            audience: null,
                            claims: claimsEmpresaUsuario,
                        expires: expiracionkeyEmpresaUsuario,
                        signingCredentials: credsEmpresaUsuario);


                    return new ViewUsuarioToken()
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(tokenkeyEmpresaUsuario),
                        Expiracion = expiracionkeyEmpresaUsuario,
                        UriImagen = empresaAdmin.EmpresaImagen,
                    };

                case "UsuarioEmpresa":
                    if (identityUserFkEmpresa == 0 && identityUserFkUsuario == 0)
                    {
                        return new ViewUsuarioToken()
                        {
                            Error = "No existe AdminEmpresa"

                        };
                    }
                    var empresaAdminu = await context.Empresas.FirstOrDefaultAsync(x => x.Id.Equals(identityUserFkEmpresa));
                    var claimsLocalUsuario = new List<Claim>() {
                                    new Claim(ClaimTypes.NameIdentifier,identityUser.Id),
                                    new Claim(ClaimTypes.NameIdentifier, identityUserFkEmpresa.ToString()),
                                    new Claim(ClaimTypes.NameIdentifier, identityUserFkEmpresaCreada.ToString()),
                                    new Claim(ClaimTypes.NameIdentifier, identityUserFkLocal.ToString()),
                                    new Claim(ClaimTypes.NameIdentifier, identityUserFkUsuario.ToString()),
                                    new Claim(ClaimTypes.Role,rol.Name),
                                    new Claim(ClaimTypes.Email, identityUser.Email),

                                };

                    var keyLocalUsuario = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwt:key"]));
                    var credsLocalUsuario = new SigningCredentials(keyLocalUsuario, SecurityAlgorithms.HmacSha256);

                    var expiracionkeyLocalUsuario = DateTime.UtcNow.AddMonths(6);

                    JwtSecurityToken tokenkeyLocalUsuario = new JwtSecurityToken(
                            issuer: null,
                            audience: null,
                            claims: claimsLocalUsuario,
                        expires: expiracionkeyLocalUsuario,
                        signingCredentials: credsLocalUsuario);


                    return new ViewUsuarioToken()
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(tokenkeyLocalUsuario),
                        Expiracion = expiracionkeyLocalUsuario,
                        UriImagen = empresaAdminu.EmpresaImagen,

                    };

                case "AdminEmpresaCreada":

                    if (identityUserFkEmpresaCreada == 0 && identityUserFkUsuario == 0)
                    {
                        return new ViewUsuarioToken()
                        {
                            Error = "No existe AdminEmpresa"

                        };
                    }
                    var AdminEmpresaCreada = await context.EmpresasCreadas.FirstOrDefaultAsync(x => x.Id.Equals(identityUserFkEmpresaCreada));
                    var claimsAdminEmpresaCreada = new List<Claim>() {
                                    new Claim(ClaimTypes.NameIdentifier,identityUser.Id),
                                    new Claim(ClaimTypes.NameIdentifier, identityUserFkEmpresa.ToString()),
                                    new Claim(ClaimTypes.NameIdentifier, identityUserFkEmpresaCreada.ToString()),
                                    new Claim(ClaimTypes.NameIdentifier, identityUserFkLocal.ToString()),
                                    new Claim(ClaimTypes.NameIdentifier, identityUserFkUsuario.ToString()),
                                    new Claim(ClaimTypes.Role,rol.Name),
                                    new Claim(ClaimTypes.Email, identityUser.Email),

                                };

                    var keyAdminEmpresaCreada = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwt:key"]));
                    var credAdminEmpresaCreada = new SigningCredentials(keyAdminEmpresaCreada, SecurityAlgorithms.HmacSha256);

                    var expiracionkeyAdminEmpresaCreada = DateTime.UtcNow.AddMonths(6);

                    JwtSecurityToken tokenAdminEmpresaCreada = new JwtSecurityToken(
                            issuer: null,
                            audience: null,
                            claims: claimsAdminEmpresaCreada,
                        expires: expiracionkeyAdminEmpresaCreada,
                        signingCredentials: credAdminEmpresaCreada);


                    return new ViewUsuarioToken()
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(tokenAdminEmpresaCreada),
                        Expiracion = expiracionkeyAdminEmpresaCreada,
                        UriImagen = AdminEmpresaCreada.EmpresasCreadaImagen


                    };

                case "UsuarioEmpresaCreada":
                    if (identityUserFkEmpresaCreada == 0 && identityUserFkUsuario == 0)
                    {
                        return new ViewUsuarioToken()
                        {
                            Error = "No existe AdminEmpresa"

                        };
                    }
                    var empresaUsuarioEmpresaCreada = await context.EmpresasCreadas.FirstOrDefaultAsync(x => x.Id.Equals(identityUserFkEmpresaCreada));
                    var claimsempresaUsuario = new List<Claim>() {
                                    new Claim(ClaimTypes.NameIdentifier,identityUser.Id),
                                    new Claim(ClaimTypes.NameIdentifier, identityUserFkEmpresa.ToString()),
                                    new Claim(ClaimTypes.NameIdentifier, identityUserFkEmpresaCreada.ToString()),
                                    new Claim(ClaimTypes.NameIdentifier, identityUserFkLocal.ToString()),
                                    new Claim(ClaimTypes.NameIdentifier, identityUserFkUsuario.ToString()),
                                    new Claim(ClaimTypes.Role,rol.Name),
                                    new Claim(ClaimTypes.Email, identityUser.Email),

                                };

                    var keyUsuarioEmpresaCreada = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwt:key"]));
                    var credsUsuarioEmpresaCreada = new SigningCredentials(keyUsuarioEmpresaCreada, SecurityAlgorithms.HmacSha256);

                    var expiracionUsuarioEmpresaCreada = DateTime.UtcNow.AddMonths(6);

                    JwtSecurityToken tokenkeyUsuarioEmpresaCreada = new JwtSecurityToken(
                            issuer: null,
                            audience: null,
                            claims: claimsempresaUsuario,
                        expires: expiracionUsuarioEmpresaCreada,
                        signingCredentials: credsUsuarioEmpresaCreada);


                    return new ViewUsuarioToken()
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(tokenkeyUsuarioEmpresaCreada),
                        Expiracion = expiracionUsuarioEmpresaCreada,
                        UriImagen = empresaUsuarioEmpresaCreada.EmpresasCreadaImagen,

                    };

                case "LocalEmpresaCreada":
                    if (identityUserFkLocal == 0 && identityUserFkEmpresa == 0 && identityUserFkUsuario == 0)
                    {
                        return new ViewUsuarioToken()
                        {
                            Error = "No existe AdminEmpresa"

                        };
                    }
                    var LocalEmpresaCreada = await context.EmpresasCreadas.FirstOrDefaultAsync(x => x.Id.Equals(identityUserFkEmpresaCreada));
                    var claimsLocalEmpresaCreada = new List<Claim>() {
                                    new Claim(ClaimTypes.NameIdentifier,identityUser.Id),
                                    new Claim(ClaimTypes.NameIdentifier, identityUserFkEmpresa.ToString()),
                                    new Claim(ClaimTypes.NameIdentifier, identityUserFkEmpresaCreada.ToString()),
                                    new Claim(ClaimTypes.NameIdentifier, identityUserFkLocal.ToString()),
                                    new Claim(ClaimTypes.NameIdentifier, identityUserFkUsuario.ToString()),
                                    new Claim(ClaimTypes.Role,rol.Name),
                                    new Claim(ClaimTypes.Email, identityUser.Email),

                                };

                    var keyLocalEmpresaCreada = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwt:key"]));
                    var credsLocalEmpresaCreada = new SigningCredentials(keyLocalEmpresaCreada, SecurityAlgorithms.HmacSha256);

                    var expiracionLocalEmpresaCreada = DateTime.UtcNow.AddMonths(6);

                    JwtSecurityToken tokenkeyLocalEmpresaCreada = new JwtSecurityToken(
                            issuer: null,
                            audience: null,
                            claims: claimsLocalEmpresaCreada,
                        expires: expiracionLocalEmpresaCreada,
                        signingCredentials: credsLocalEmpresaCreada);


                    return new ViewUsuarioToken()
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(tokenkeyLocalEmpresaCreada),
                        Expiracion = expiracionLocalEmpresaCreada,
                        UriImagen = LocalEmpresaCreada.EmpresasCreadaImagen,

                    };

                case "LocalUsuarioEmpresaCreada":
                    if (identityUserFkEmpresaCreada == 0 && identityUserFkUsuario == 0)
                    {
                        return new ViewUsuarioToken()
                        {
                            Error = "No existe AdminEmpresa"

                        };
                    }
                    var LocalUsuarioEmpresaCreada = await context.EmpresasCreadas.FirstOrDefaultAsync(x => x.Id.Equals(identityUserFkEmpresaCreada));
                    var claimsLocalUsuarioEmpresaCreada = new List<Claim>() {
                                    new Claim(ClaimTypes.NameIdentifier,identityUser.Id),
                                    new Claim(ClaimTypes.NameIdentifier, identityUserFkEmpresa.ToString()),
                                    new Claim(ClaimTypes.NameIdentifier, identityUserFkEmpresaCreada.ToString()),
                                    new Claim(ClaimTypes.NameIdentifier, identityUserFkLocal.ToString()),
                                    new Claim(ClaimTypes.NameIdentifier, identityUserFkUsuario.ToString()),
                                    new Claim(ClaimTypes.Role,rol.Name),
                                    new Claim(ClaimTypes.Email, identityUser.Email),

                                };

                    var keyLocalUsuarioEmpresaCreada = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwt:key"]));
                    var credsLocalUsuarioEmpresaCreada = new SigningCredentials(keyLocalUsuarioEmpresaCreada, SecurityAlgorithms.HmacSha256);

                    var expiracionLocalUsuarioEmpresaCreada = DateTime.UtcNow.AddMonths(6);

                    JwtSecurityToken tokenkeyLocalUsuarioEmpresaCreada = new JwtSecurityToken(
                            issuer: null,
                            audience: null,
                            claims: claimsLocalUsuarioEmpresaCreada,
                        expires: expiracionLocalUsuarioEmpresaCreada,
                        signingCredentials: credsLocalUsuarioEmpresaCreada);


                    return new ViewUsuarioToken()
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(tokenkeyLocalUsuarioEmpresaCreada),
                        Expiracion = expiracionLocalUsuarioEmpresaCreada,
                        UriImagen = LocalUsuarioEmpresaCreada.EmpresasCreadaImagen,

                    };
              
              
                    
            }

            return new ViewUsuarioToken()
            {
                Error = "No existe usuario"

            };

        }

    }
}
