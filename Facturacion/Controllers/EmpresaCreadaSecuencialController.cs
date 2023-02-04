using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Facturacion.Context;
using Facturacion.DTOs;
using Facturacion.Model;
using Facturacion.Services;
using Facturacion.Services.FacturacionElectronica.Services;
using Facturacion.Services.Interfaces;
using System.Security.Claims;
namespace Facturacion.Controllers
{
    public class EmpresaCreadSecuencialController: Controller
    {
        DateTime currentTimePacific = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time"));

        private readonly UserManager<NetUserAditional> _userManager;
        private readonly SignInManager<NetUserAditional> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly AplicationDbContext context;
        private readonly IMapper mapper;
        private readonly EmailServices emailService;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly IEmailSender emailSender;
        private readonly CConsultaSri cConsultaSri;
        private readonly CFuncionCedulas cFuncionCedulas;
        public EmpresaCreadSecuencialController(UserManager<NetUserAditional> userManager,
           SignInManager<NetUserAditional> signInManager, IConfiguration configuration,
           AplicationDbContext context, IMapper mapper, EmailServices emailService,
           IAlmacenadorArchivos almacenadorArchivos, IEmailSender emailSender, CConsultaSri cConsultaSri, CFuncionCedulas cFuncionCedulas)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            this.context = context;
            this.mapper = mapper;
            this.emailService = emailService;
            this.almacenadorArchivos = almacenadorArchivos;
            this.emailSender = emailSender;
            this.cConsultaSri = cConsultaSri;
            this.cFuncionCedulas = cFuncionCedulas;
        }

        //[HttpGet("GetCabezeraFacturaEmpresaCreada")]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<ActionResult<ViewProductoDto>> GetCabezeraFacturaEmpresaCreada([FromBody] ProductoIdDTo productoDTo)
        //{
        //    var curentUser = HttpContext.User;
        //    var claims = curentUser.Claims.ToList();
        //    if (claims.Count == 0)
        //        return BadRequest("No tiene permiso para esta seccion");
        //    var getProducto = await context.Productos.FirstOrDefaultAsync(x => x.ProductoCodigo.Equals(productoDTo.Id)
        //    && x.FkEmpresa.Equals(Convert.ToInt32(claims[1].Value)) && x.FkEmpresasCreada.Equals(Convert.ToInt32(claims[2].Value)));
        //    if (getProducto != null)
        //    {
        //        var Prodcuto = mapper.Map<ViewProductoDto>(getProducto);

        //        return Ok(Prodcuto);

        //    }
        //    return BadRequest("No existe Producto");

        //}




    }
}
