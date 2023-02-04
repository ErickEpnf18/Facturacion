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
    [ApiController]
    [Route("api/EmpresaCreada/Producto")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "UsariosNesEmpresaCreada")]

    public class EmpresaCreadaProductosController : Controller
    {
        DateTime currentTimePacific = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time"));

        private readonly UserManager<NetUserAditional> _userManager;
        private readonly SignInManager<NetUserAditional> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly AplicationDbContext context;
        private readonly IMapper mapper;

        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly IEmailSender emailSender;
        private readonly CConsultaSri cConsultaSri;
        private readonly CFuncionCedulas cFuncionCedulas;
        public EmpresaCreadaProductosController(UserManager<NetUserAditional> userManager,
           SignInManager<NetUserAditional> signInManager, IConfiguration configuration,
           AplicationDbContext context, IMapper mapper,
           IAlmacenadorArchivos almacenadorArchivos, IEmailSender emailSender, CConsultaSri cConsultaSri, CFuncionCedulas cFuncionCedulas)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            this.context = context;
            this.mapper = mapper;

            this.almacenadorArchivos = almacenadorArchivos;
            this.emailSender = emailSender;
            this.cConsultaSri = cConsultaSri;
            this.cFuncionCedulas = cFuncionCedulas;
        }
       /// <summary>
       /// Traer Producto Emrpresa Creada
       /// </summary>
       /// <param name="productoDTo"></param>
       /// <returns></returns>
        [HttpGet("GetProductoEmpresaCreada")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ViewProductoDto>> GetProductoEmpresaCreada([FromQuery] ProductoIdDTo productoDTo)
        {
            try
            {
                var curentUser = HttpContext.User;
                var claims = curentUser.Claims.ToList();
                if (claims.Count == 0)
                    return BadRequest("No tiene permiso para esta seccion");
                var getProducto = await context.Productos.FirstOrDefaultAsync(x => x.Id.Equals(productoDTo.Id)
                && x.FkEmpresa.Equals(Convert.ToInt32(claims[1].Value)) && x.FkEmpresasCreada.Equals(Convert.ToInt32(claims[2].Value)));
                if (getProducto != null)
                {
                    var Prodcuto = mapper.Map<ViewProductoDto>(getProducto);

                    return Ok(Prodcuto);

                }
                return BadRequest("No existe Producto");

            }
            catch (Exception ex)
            {

                return BadRequest(ex.ToString());

            }


        }
        /// <summary>
        /// lista de Producotos Empresa creada
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetListProductoEmpresaCreada")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ViewProductoDto>> GetListProductoEmpresaCreada()
        {
            try
            {
                var curentUser = HttpContext.User;
                var claims = curentUser.Claims.ToList();
                if (claims.Count == 0)
                    return BadRequest("No tiene permiso para esta seccion");
                var getProducto = await context.Productos.
                    Where(x => x.FkEmpresa.Equals(Convert.ToInt32(claims[1].Value)) && x.FkEmpresasCreada.Equals(Convert.ToInt32(claims[2].Value))).ToListAsync();
                if (getProducto != null)
                {
                    var Prodcuto = mapper.Map<List<ViewProductoDto>>(getProducto);

                    return Ok(Prodcuto);

                }
                return BadRequest("No existe Producto");


            }
            catch (Exception ex)
            {

                return BadRequest(ex.ToString());

            }

        }

        [HttpPost("PostProductoEmpresaCreada")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ViewProductoDto>> PostProductoEmpresaCreada([FromBody] ProductoDTo productoDTo)
        {
            try
            {
                var curentUser = HttpContext.User;
                var claims = curentUser.Claims.ToList();
                if (claims.Count == 0)
                    return BadRequest("No tiene permiso para esta seccion");
                var getProducto = await context.Productos.FirstOrDefaultAsync(x => x.ProductoCodigo.Equals(productoDTo.ProductoCodigo)
                && x.FkEmpresa.Equals(Convert.ToInt32(claims[1].Value)) && x.FkEmpresasCreada.Equals(Convert.ToInt32(claims[2].Value)));
                if (getProducto != null)
                    return BadRequest($"Ya existe el Prodcucto {getProducto.ProductoCodigo}");

                var ProductoEmpresaCreada = mapper.Map<Producto>(productoDTo);
                ProductoEmpresaCreada.FkEmpresa = Convert.ToInt32(claims[1].Value);
                ProductoEmpresaCreada.FkEmpresasCreada = Convert.ToInt32(claims[2].Value);
                ProductoEmpresaCreada.FechaRegistroProducto = currentTimePacific;
                await context.AddAsync(ProductoEmpresaCreada);
                await context.SaveChangesAsync();
                var Prodcuto = mapper.Map<ViewProductoDto>(ProductoEmpresaCreada);

                return Created("", Prodcuto);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.ToString());
            }


        }


        [HttpPost("PostProductoEmpresaCreadaCSV")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ViewProductoDto>> PostProductoEmpresaCreadaCSV([FromBody] ProductoDTo productoDTo)
        {
            try
            {
                var curentUser = HttpContext.User;
                var claims = curentUser.Claims.ToList();
                if (claims.Count == 0)
                    return BadRequest("No tiene permiso para esta seccion");
                var getProducto = await context.Productos.FirstOrDefaultAsync(x => x.ProductoCodigo.Equals(productoDTo.ProductoCodigo)
                && x.FkEmpresa.Equals(Convert.ToInt32(claims[1].Value)) && x.FkEmpresasCreada.Equals(Convert.ToInt32(claims[2].Value)));
                if (getProducto != null)
                    return BadRequest($"Ya existe el Prodcucto {getProducto.ProductoCodigo}");

                var ProductoEmpresaCreada = mapper.Map<Producto>(productoDTo);
                ProductoEmpresaCreada.FkEmpresa = Convert.ToInt32(claims[1].Value);
                ProductoEmpresaCreada.FkEmpresasCreada = Convert.ToInt32(claims[2].Value);
                ProductoEmpresaCreada.FechaRegistroProducto = currentTimePacific;
                await context.AddAsync(ProductoEmpresaCreada);
                await context.SaveChangesAsync();
                var Prodcuto = mapper.Map<ViewProductoDto>(ProductoEmpresaCreada);

                return Created("", Prodcuto);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.ToString());
            }


        }

        [HttpPut("PutProductoEmpresaCreada")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ViewProductoDto>> PutProductoEmpresaCreada([FromBody] ProductoUpdateDTo productoDTo)
        {
            var curentUser = HttpContext.User;
            var claims = curentUser.Claims.ToList();
            if (claims.Count == 0)
                return BadRequest("No tiene permiso para esta seccion");
            var getProducto = await context.Productos.FirstOrDefaultAsync(x => x.Id.Equals(productoDTo.Id)
            && x.FkEmpresa.Equals(Convert.ToInt32(claims[1].Value)) && x.FkEmpresasCreada.Equals(Convert.ToInt32(claims[2].Value)));
            if (getProducto == null)
                return BadRequest("No existe el producto");

            getProducto = mapper.Map(productoDTo, getProducto);

            await context.SaveChangesAsync();
            var Prodcuto = mapper.Map<ViewProductoDto>(getProducto);

            return Ok(Prodcuto);

        }

        [HttpDelete("DeleteProductoEmpresaCreada")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ViewProductoDto>> DeleteProductoEmpresaCreada([FromQuery] ProductoIdDTo productoDTo)
        {
            var curentUser = HttpContext.User;
            var claims = curentUser.Claims.ToList();
            if (claims.Count == 0)
                return BadRequest("No tiene permiso para esta seccion");
            var getProducto = await context.Productos.FirstOrDefaultAsync(x => x.Id.Equals(productoDTo.Id)
            && x.FkEmpresa.Equals(Convert.ToInt32(claims[1].Value)) && x.FkEmpresasCreada.Equals(Convert.ToInt32(claims[2].Value)));
            if (getProducto == null)
                return BadRequest("No existe el producto");
            getProducto.ProductoEstado = false;
            await context.SaveChangesAsync();
            var Prodcuto = mapper.Map<ViewProductoDto>(getProducto);

            return Created("Producto  esta Desactivo", Prodcuto);
        }



    }
}
