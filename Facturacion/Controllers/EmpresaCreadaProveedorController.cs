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
    [Route("api/EmpresaCreada/Proveedores")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "UsariosNesEmpresaCreada")]

    public class EmpresaCreadaProveedorController : Controller
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

        public EmpresaCreadaProveedorController(UserManager<NetUserAditional> userManager,
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

        [HttpGet("GetListProveedoresEmpresaCreada")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ViewProveedoresEmpresaCreadaDTo>>> GetListProveedoresEmpresaCreada()
        {
            var curentUser = HttpContext.User;
            var claims = curentUser.Claims.ToList();
            if (claims.Count == 0)
                return BadRequest("No tiene permiso para esta seccion");
            var getListProveedorEmpresa = await context.Proveedores.Include(x => x.SriRepositorioNavigation)
                .Where(x => x.FkEmpresa.Equals(Convert.ToInt32(claims[1].Value))
             && x.FkEmpresasCreada.Equals(Convert.ToInt32(claims[2].Value)))
                          .ToListAsync();

            if (getListProveedorEmpresa != null)
            {

                var viewCorreoEmpresa = mapper.Map<List<Proveedor>, List<ViewProveedoresEmpresaCreadaDTo>>(getListProveedorEmpresa);
                return viewCorreoEmpresa;

            }
            return BadRequest("No se encuentra Permisas Empresa");

        }


        [HttpGet("GetProveedoresEmpresaCreada")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ViewProveedoresEmpresaCreadaDTo>> GetProveedoresEmpresaCreada([FromBody] GetProveedoresEmpresaCreadaDTo getProveedoresEmpresaCreadaDTo)
        {
            var curentUser = HttpContext.User;
            var claims = curentUser.Claims.ToList();
            if (claims.Count == 0)
                return BadRequest("No tiene permiso para esta seccion");
            var getProveedoresEmpresa = await context.Proveedores.Include(x => x.SriRepositorioNavigation).FirstOrDefaultAsync(
                x => x.FkEmpresa.Equals(Convert.ToInt32(claims[1].Value))
             && x.FkEmpresasCreada.Equals(Convert.ToInt32(claims[2].Value)) && x.Id.Equals(getProveedoresEmpresaCreadaDTo.Id));



            if (getProveedoresEmpresa != null)
            {

                var viewProveedorEmpresaCreada = mapper.Map<Proveedor, ViewProveedoresEmpresaCreadaDTo>(getProveedoresEmpresa);
                return viewProveedorEmpresaCreada;

            }
            return BadRequest("No se encuentra Proveedor Empresa ");

        }


        [HttpPost("PostProveedoresEmpresaCreada")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ViewProveedoresEmpresaCreadaDTo>> PostProveedoresEmpresaCreada([FromBody] ProveedoresEmpresaCreadaDTo proveedoresEmpresaCreadaDTo)
        {
            var curentUser = HttpContext.User;
            var claims = curentUser.Claims.ToList();
            if (claims.Count == 0)
                return BadRequest("No tiene permiso para esta seccion");


            SriRepositorio? sriRepositorio = null;
            sriRepositorio = await context.SriRepositorios.FirstOrDefaultAsync(x => x.NumeroRuc.Equals(proveedoresEmpresaCreadaDTo.Ruc));

            if (sriRepositorio == null)
            {

                var getsriRepositorio = cConsultaSri.GetRucSri(proveedoresEmpresaCreadaDTo.Ruc);
                if (getsriRepositorio == null || !string.IsNullOrEmpty(getsriRepositorio.Error))
                    return BadRequest($"No existe Contribuyente con el siguiente Ruc {proveedoresEmpresaCreadaDTo.Ruc}");
                sriRepositorio = mapper.Map<SriRepositorio>(getsriRepositorio);

                Informacionfechascontribuyente informacionfechascontribuyente = new Informacionfechascontribuyente();
                PersonaMiPyme1 personaMiPyme = new PersonaMiPyme1();
                List<PersonaEstablecimientosRuc> personaEstablecimientosRucs = new List<PersonaEstablecimientosRuc>();
                informacionfechascontribuyente = mapper.Map<Informacionfechascontribuyente>(getsriRepositorio.modelInformacionfechascontribuyente);
                personaMiPyme = mapper.Map<PersonaMiPyme1>(getsriRepositorio.modelPersonaMiPyme);
                personaEstablecimientosRucs = mapper.Map<List<PersonaEstablecimientosRuc>>(getsriRepositorio.modelPersonaEstablecimientosRucs);
                try
                {

                    await context.AddAsync(sriRepositorio);
                    await context.SaveChangesAsync();
                    if (personaMiPyme != null)
                    {
                        personaMiPyme.FkSriRepositorio = sriRepositorio.Id;
                        await context.AddAsync(personaMiPyme);
                        await context.SaveChangesAsync();
                    }
                    if (informacionfechascontribuyente != null)
                    {
                        informacionfechascontribuyente.FkSriRepositorio = sriRepositorio.Id;
                        await context.AddAsync(informacionfechascontribuyente);
                        await context.SaveChangesAsync();
                    }
                    if (personaEstablecimientosRucs != null)
                    {
                        foreach (var item in personaEstablecimientosRucs)
                        {
                            item.FkSri = sriRepositorio.Id;
                            await context.AddAsync(item);
                            await context.SaveChangesAsync();

                        };
                    }
                }
                catch (Exception ex)
                {

                    var exasd = ex.ToString();
                }
            }
            var provedores = await context.Proveedores.Include(x => x.SriRepositorioNavigation).FirstOrDefaultAsync(x => x.ProveedoresRuc.Equals(proveedoresEmpresaCreadaDTo.Ruc));
            if (provedores != null)
                return BadRequest($"Ya existe Proveedor con el siguiente Ruc {proveedoresEmpresaCreadaDTo.Ruc}");

            var proveedor = new Proveedor
            {
                FkSriRepositorio = sriRepositorio.Id,
                FkEmpresa = Convert.ToInt32(claims[1].Value),
                FkEmpresasCreada = Convert.ToInt32(claims[2].Value),
                ProveedoresRuc = sriRepositorio.NumeroRuc,
                ProveedoresPropietario = sriRepositorio.RazonSocial,
                ProveedoresEstado = true
            };
            await context.AddAsync(proveedor);
            await context.SaveChangesAsync();

            var ViewProveedores = new
                ViewProveedoresEmpresaCreadaDTo
            {
                ProveedoresRuc = sriRepositorio.NumeroRuc,
                ProveedoresPropietario = sriRepositorio.RazonSocial,
                ProveedoresEstado = true

            };
            return Created("", ViewProveedores);

        }
       
        [HttpPost("PostProveedoresManualEmpresaCreada")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ViewProveedoresEmpresaCreadaDTo>> PostProveedoresManualEmpresaCreada([FromBody] ProveedoresEmpresaCreadaManualDTo proveedoresEmpresaCreadaManualDTo)
        {
            var curentUser = HttpContext.User;
            var claims = curentUser.Claims.ToList();
            if (claims.Count == 0)
                return BadRequest("No tiene permiso para esta seccion");

            var provedores = await context.Proveedores.FirstOrDefaultAsync(x => x.ProveedoresRuc.Equals(proveedoresEmpresaCreadaManualDTo.Cedula));
            if (provedores != null)
                return BadRequest($"Ya existe Proveedor con el siguiente Ruc {proveedoresEmpresaCreadaManualDTo.RazonSocial}");

            var proveedor = new Proveedor
            {
                FkSriRepositorio = null,
                FkEmpresa = Convert.ToInt32(claims[1].Value),
                FkEmpresasCreada = Convert.ToInt32(claims[2].Value),
                ProveedoresRuc = proveedoresEmpresaCreadaManualDTo.Cedula,
                ProveedoresPropietario = proveedoresEmpresaCreadaManualDTo.RazonSocial,
                ProveedoresEstado = true
            };
            await context.AddAsync(proveedor);
            await context.SaveChangesAsync();

            var ViewProveedores = new
                ViewProveedoresEmpresaCreadaDTo
            {
                Id=proveedor.Id,
                ProveedoresRuc = proveedoresEmpresaCreadaManualDTo.Cedula,
                ProveedoresPropietario = proveedoresEmpresaCreadaManualDTo.RazonSocial,
                ProveedoresEstado = true

            };
            return Created("", ViewProveedores);

        }



        [HttpPut("UpdateProveedoresEmpresaCreada")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ViewProveedoresEmpresaCreadaDTo>> UpdateProveedoresEmpresaCreada([FromBody] PutProveedoresEmpresaCreadaDTo putClienteEmpresaCreadaDTo)
        {
            var curentUser = HttpContext.User;
            var claims = curentUser.Claims.ToList();
            if (claims.Count == 0)
                return BadRequest("No tiene permiso para esta seccion");
        
            var Provedores = await context.Proveedores.FirstOrDefaultAsync(x => x.Id.Equals(putClienteEmpresaCreadaDTo.Id)&& x.FkEmpresa.Equals(Convert.ToInt32(claims[1].Value))
             && x.FkEmpresasCreada.Equals(Convert.ToInt32(claims[2].Value)));
            if (Provedores == null)
                return BadRequest("No existe Cliente");
            Provedores.ProveedoresEstado = putClienteEmpresaCreadaDTo.Estado;
            await context.SaveChangesAsync();

            return Ok("Proveedor  esta en estado Desactivo");
        }


        [HttpDelete("DeleteProveedoresEmpresaCreada")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ViewProveedoresEmpresaCreadaDTo>> DeleteClienteEmpresaCreada([FromQuery] DeleteProveedoresEmpresaCreadaDTo deleteClienteEmpresaCreadaDTo)
        {
            var curentUser = HttpContext.User;
            var claims = curentUser.Claims.ToList();
            if (claims.Count == 0)
                return BadRequest("No tiene permiso para esta seccion");
          
            var Provedores = await context.Proveedores.FirstOrDefaultAsync(x => x.Id.Equals(deleteClienteEmpresaCreadaDTo.Id)&&x.FkEmpresa.Equals(Convert.ToInt32(claims[1].Value))
             && x.FkEmpresasCreada.Equals(Convert.ToInt32(claims[2].Value)));
            if (Provedores == null)
                return BadRequest("No existe Cliente");
            Provedores.ProveedoresEstado = false;
            await context.SaveChangesAsync();

            return Ok("Proveedor  esta en estado Desactivo");
        }




    }
}
