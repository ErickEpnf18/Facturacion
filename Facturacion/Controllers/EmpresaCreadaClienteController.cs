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
    [Route("api/EmpresaCreada/Clientes")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "UsariosNesEmpresaCreada")]

    public class EmpresaCreadaClienteController : Controller
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
        public EmpresaCreadaClienteController(UserManager<NetUserAditional> userManager,
            SignInManager<NetUserAditional> signInManager,
            IConfiguration configuration, AplicationDbContext context,
            IMapper mapper, IAlmacenadorArchivos almacenadorArchivos,
            IEmailSender emailSender, CConsultaSri cConsultaSri, CFuncionCedulas cFuncionCedulas)
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
        /// Listado Clientes Emrepsa Creada
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetListClientesEmpresaCreada")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<GetListClienteEmpresaCreadaDto>>> GetListClientesEmpresaCreada()
        {
            var curentUser = HttpContext.User;
            var claims = curentUser.Claims.ToList();
            if (claims.Count == 0)
                return BadRequest("No tiene permiso para esta seccion");
            var getListClienteEmpresa = await context.Clientes.
                Where(x => x.FkEmpresa.Equals(Convert.ToInt32(claims[1].Value))
                && x.EmpresasCreadas.Equals(Convert.ToInt32(claims[2].Value)) && x.Estado == true)
                          .ToListAsync();

            if (getListClienteEmpresa != null)
            {

                var viewClienteEmpresaCreada = mapper.Map<List<Cliente>, List<GetListClienteEmpresaCreadaDto>>(getListClienteEmpresa);
                return viewClienteEmpresaCreada;

            }
            return BadRequest("No se encuentra Permisas Empresa");

        }
        /// <summary>
        /// Traer cliente por Id
        /// </summary>
        /// <param name="getClienteDto"></param>
        /// <returns></returns>
        [HttpGet("GetClientesEmpresaCreada")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetListClienteEmpresaCreadaDto>> GetClientesEmpresaCreada([FromQuery] GetCliendteDto getClienteDto)
        {
            var curentUser = HttpContext.User;
            var claims = curentUser.Claims.ToList();
            if (claims.Count == 0)
                return BadRequest("No tiene permiso para esta seccion");
            var getListClienteEmpresa = await context.Clientes.FirstOrDefaultAsync(
                x => x.FkEmpresa.Equals(Convert.ToInt32(claims[1].Value))
             && x.EmpresasCreadas.Equals(Convert.ToInt32(claims[2].Value)) && x.Id.Equals(getClienteDto.Id));



            if (getListClienteEmpresa != null)
            {

                var viewCorreoEmpresa = mapper.Map<Cliente, GetListClienteEmpresaCreadaDto>(getListClienteEmpresa);
                return viewCorreoEmpresa;

            }
            return BadRequest("No se encuentra Permisas Empresa");

        }
        /// <summary>
        /// Agrear Cliente a travez del Web Service de la cedula
        /// </summary>
        /// <param name="cedulasRepositorioDto"></param>
        /// <returns></returns>
        [HttpPost("PostRespositorioCedulas")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ViewClienteEmpresaCreadaDTo>> PostRespositorioCedulas([FromBody] CedulasRepositorioDto cedulasRepositorioDto)
        {
            try
            {
                var curentUser = HttpContext.User;
                var claims = curentUser.Claims.ToList();
                if (claims.Count == 0)
                    return BadRequest("No tiene permiso para esta seccion");
                var empresaCreaada = await context.EmpresasCreadas.
                    FirstOrDefaultAsync(x => x.FkEmpresa.Equals(Convert.ToInt32(claims[1].Value))
                 && x.Id.Equals(Convert.ToInt32(claims[2].Value)));
                if (empresaCreaada == null)
                    return BadRequest("No existe Empresa");


                var Repositoriocedulas = await context.
                    RespositorioCedulas.
                    FirstOrDefaultAsync(x => x.Identificacion.Equals(cedulasRepositorioDto.Cedula));
                if (Repositoriocedulas == null)
                {
                    var cedulareturn = cFuncionCedulas.ConsultaCedula(cedulasRepositorioDto.Cedula);
                    if (cedulareturn != null)
                    {
                        var cedulacreate = new RespositorioCedulas()
                        {
                            Identificacion = cedulareturn.identificacion,
                            Razonsocial = cedulareturn.razonsocial,
                            Tipo = cedulareturn.tipo,
                            Sexo = cedulareturn.sexo,
                            Estadocivil = cedulareturn.estadocivil,
                            Fechanacimiento = cedulareturn.fechanacimiento,
                            Nacionalidad = cedulareturn.nacionalidad,
                            Ciudad = cedulareturn.ciudad,
                            Direccion = cedulareturn.direccion,
                            Nrocalle = cedulareturn.nrocalle,
                            Response = cedulareturn.response,
                            Mensaje = cedulareturn.mensaje,

                        };

                        var ClientePostEmpresaCreada = new PostClienteEmpresaCreadaDTo
                        {
                            FkRespositorioCedulas = cedulacreate.Id,
                            FkEmpresa = empresaCreaada.FkEmpresa,
                            EmpresasCreadas = empresaCreaada.Id,
                            FkUsuario = Convert.ToInt32(claims[3].Value),
                            Identificacion = cedulacreate.Identificacion,
                            Razonsocial = cedulacreate.Razonsocial,
                            Direccion = cedulacreate.Direccion,
                            Email = cedulasRepositorioDto.CorreoCliente
                        };
                        await context.AddAsync(cedulacreate);
                        await context.SaveChangesAsync();
                        var empresaCreadaCliente = mapper.Map<Cliente>(ClientePostEmpresaCreada);
                        empresaCreadaCliente.Estado = true;
                        empresaCreadaCliente.FkUsuario = null;
                        await context.AddAsync(empresaCreadaCliente);
                        await context.SaveChangesAsync();

                        var ClienteEmpresaCreada = new
                             ViewClienteEmpresaCreadaDTo()
                        {
                            EmpresasCreadas = ClientePostEmpresaCreada.EmpresasCreadas,
                            Identificacion = ClientePostEmpresaCreada.Identificacion,
                            Razonsocial = ClientePostEmpresaCreada.Razonsocial

                        };
                        return Created("", ClienteEmpresaCreada);


                    }

                }
                else
                {

                    var ClienteEmpresaCreadaget = await context.
                        Clientes.FirstOrDefaultAsync
                        (x => x.Identificacion.Equals(Repositoriocedulas.Identificacion)
                      && x.FkEmpresa.Equals(Convert.ToInt32(claims[1].Value))
                      && x.EmpresasCreadas.Equals(Convert.ToInt32(claims[2].Value)));
                    if (ClienteEmpresaCreadaget != null)
                    {
                        return BadRequest("Ya  existe Cliente- " +
                            ClienteEmpresaCreadaget.Identificacion + "-"
                            + ClienteEmpresaCreadaget.Id);
                    }


                    var ClientePostEmpresaCreada = new PostClienteEmpresaCreadaDTo
                    {
                        FkRespositorioCedulas = Repositoriocedulas.Id,
                        FkEmpresa = empresaCreaada.FkEmpresa,
                        EmpresasCreadas = empresaCreaada.Id,
                        FkUsuario = Convert.ToInt32(claims[3].Value),
                        Identificacion = Repositoriocedulas.Identificacion,
                        Razonsocial = Repositoriocedulas.Razonsocial,
                        Direccion = Repositoriocedulas.Direccion,
                        Email = cedulasRepositorioDto.CorreoCliente

                    };

                    var empresaCreadaCliente = mapper.Map<Cliente>(ClientePostEmpresaCreada);
                    empresaCreadaCliente.Estado = true;
                    empresaCreadaCliente.FkUsuario = null;
                    await context.AddAsync(empresaCreadaCliente);
                    await context.SaveChangesAsync();

                    var ClienteEmpresaCreada = new
                         ViewClienteEmpresaCreadaDTo()
                    {
                        EmpresasCreadas = ClientePostEmpresaCreada.EmpresasCreadas,
                        Identificacion = ClientePostEmpresaCreada.Identificacion,
                        Razonsocial = ClientePostEmpresaCreada.Razonsocial

                    };
                    return Created("", ClienteEmpresaCreada);

                }
                return BadRequest("Intenlo Nuevamente Ecurrio error al Crear Cliente");


            }
            catch (Exception ex)
            {

                return BadRequest(ex.ToString());
            }

        }
        /// <summary>
        /// Agrear Cliente a Manual Cedula
        /// </summary>
        /// <param name="cedulasRepositorioDto"></param>
        /// <returns></returns>
        [HttpPost("PostRespositorioCedulasManual")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ViewClienteEmpresaCreadaDTo>> PostRespositorioCedulasManual([FromBody] CedulasRepositoriomanualDto cedulasRepositorioDto)
        {
            try
            {
                var curentUser = HttpContext.User;
                var claims = curentUser.Claims.ToList();
                if (claims.Count == 0)
                    return BadRequest("No tiene permiso para esta seccion");
                var empresaCreaada = await context.EmpresasCreadas.
                    FirstOrDefaultAsync(x => x.FkEmpresa.Equals(Convert.ToInt32(claims[1].Value))
                 && x.Id.Equals(Convert.ToInt32(claims[2].Value)));
                if (empresaCreaada == null)
                    return BadRequest("No existe Empresa");


                var Repositoriocedulas = await context.
                    RespositorioCedulas.
                    FirstOrDefaultAsync(x => x.Identificacion.Equals(cedulasRepositorioDto.Cedula));
                if (Repositoriocedulas == null)
                {


                    var cedulacreate = new RespositorioCedulas()
                    {
                        Identificacion = cedulasRepositorioDto.Cedula,
                        Razonsocial = cedulasRepositorioDto.Razonsocial.Normalize(),
                        Direccion = cedulasRepositorioDto.Direccion.Normalize(),
                        Mensaje = "Creado",

                    };

                    var ClientePostEmpresaCreada = new PostClienteEmpresaCreadaDTo
                    {
                        FkRespositorioCedulas = cedulacreate.Id,
                        FkEmpresa = empresaCreaada.FkEmpresa,
                        EmpresasCreadas = empresaCreaada.Id,
                        FkUsuario = Convert.ToInt32(claims[3].Value),
                        Identificacion = cedulacreate.Identificacion,
                        Razonsocial = cedulacreate.Razonsocial,
                        Direccion = cedulacreate.Direccion,
                        Email = cedulasRepositorioDto.CorreoCliente
                    };
                    await context.AddAsync(cedulacreate);
                    await context.SaveChangesAsync();
                    var empresaCreadaCliente = mapper.Map<Cliente>(ClientePostEmpresaCreada);
                    empresaCreadaCliente.Estado = true;
                    empresaCreadaCliente.FkUsuario = null;
                    await context.AddAsync(empresaCreadaCliente);
                    await context.SaveChangesAsync();

                    var ClienteEmpresaCreada = new
                         ViewClienteEmpresaCreadaDTo()
                    {
                        EmpresasCreadas = ClientePostEmpresaCreada.EmpresasCreadas,
                        Identificacion = ClientePostEmpresaCreada.Identificacion,
                        Razonsocial = ClientePostEmpresaCreada.Razonsocial

                    };
                    return Created("", ClienteEmpresaCreada);
                }

            }
            catch (Exception ex)
            {
                return BadRequest("Intenlo Nuevamente Ecurrio error al Crear Cliente" + ex.ToString());
            }

            return BadRequest("Intenlo Nuevamente Ecurrio error al Crear Cliente");

        }
        /// <summary>
        /// Agregar Cliente Por Ruc
        /// </summary>
        /// <param name="clientesRucEmpresaCreadaDTo"></param>
        /// <returns></returns>


        [HttpPost("PostClientesRucEmpresaCreada")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ViewProveedoresEmpresaCreadaDTo>> PostClientesRucEmpresaCreada([FromBody] ClientesRucEmpresaCreadaDTo clientesRucEmpresaCreadaDTo)
        {
            var curentUser = HttpContext.User;
            var claims = curentUser.Claims.ToList();
            if (claims.Count == 0)
                return BadRequest("No tiene permiso para esta seccion");


            SriRepositorio? sriRepositorio = null;
            sriRepositorio = await context.SriRepositorios.FirstOrDefaultAsync(x => x.NumeroRuc.Equals(clientesRucEmpresaCreadaDTo.Ruc));

            if (sriRepositorio == null)
            {

                var getsriRepositorio = cConsultaSri.GetRucSri(clientesRucEmpresaCreadaDTo.Ruc);
                if (getsriRepositorio == null || !string.IsNullOrEmpty(getsriRepositorio.Error))
                    return BadRequest($"No existe Contribuyente con el siguiente Ruc {clientesRucEmpresaCreadaDTo.Ruc}");
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

                var clientesEmrpsaCreada = await context.Clientes.Include(x => x.FkSrirepositorioNavigation).FirstOrDefaultAsync(x => x.Identificacion.Equals(clientesRucEmpresaCreadaDTo.Ruc));
                if (clientesEmrpsaCreada != null)
                    return BadRequest($"Ya existe Cliente con el siguiente Ruc {clientesRucEmpresaCreadaDTo.Ruc}");
                var direcionclienteRuc = personaEstablecimientosRucs.FirstOrDefault(x => x.FkSri.Equals(sriRepositorio.Id));
                string DireccionCliente = string.Empty;
                if (direcionclienteRuc != null)
                    DireccionCliente = direcionclienteRuc.DireccionCompleta;


                var clienteEmpresaCreada = new Cliente
                {
                    FkSrirepositorio = sriRepositorio.Id,
                    FkEmpresa = Convert.ToInt32(claims[1].Value),
                    EmpresasCreadas = Convert.ToInt32(claims[2].Value),
                    Identificacion = sriRepositorio.NumeroRuc,
                    Razonsocial = sriRepositorio.RazonSocial,
                    Direccion = DireccionCliente,
                    Email = clientesRucEmpresaCreadaDTo.Email,
                    NumeroTelefon = clientesRucEmpresaCreadaDTo.Telefono,
                    Estado = true


                };
                await context.AddAsync(clienteEmpresaCreada);
                await context.SaveChangesAsync();

                var ViewClientesRucEmpresaCreada = new
                    ViewClientesRucEmpresaCreadaDTo
                {
                    Id = clienteEmpresaCreada.Id,
                    ClientesRuc = sriRepositorio.NumeroRuc,
                    ClientesPropietario = sriRepositorio.RazonSocial,
                    ClientesEstado = true

                };
                return Created("", ViewClientesRucEmpresaCreada);
            }
            return BadRequest($"No existe Cliente Ruc  {clientesRucEmpresaCreadaDTo.Ruc} ");

        }

        /// <summary>
        /// Actulizar Cliente cedulao ruc 
        /// </summary>
        /// <param name="putClienteEmpresaCreadaDTo"></param>
        /// <returns></returns>

        [HttpPut("PutClienteEmpresaCreada")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ViewClienteEmpresaCreadaDTo>> PutClienteEmpresaCreada([FromBody] PutClienteEmpresaCreadaDTo putClienteEmpresaCreadaDTo)
        {
            var curentUser = HttpContext.User;
            var claims = curentUser.Claims.ToList();
            if (claims.Count == 0)
                return BadRequest("No tiene permiso para esta seccion");

            var clienteUpdateEmpresaCreada = await context.
                           Clientes.FirstOrDefaultAsync
                           (x => x.FkEmpresa.Equals(Convert.ToInt32(claims[1].Value))
                 && x.EmpresasCreadas.Equals(Convert.ToInt32(claims[2].Value)) && x.Id.Equals(putClienteEmpresaCreadaDTo.Id));
            if (clienteUpdateEmpresaCreada == null)
                return BadRequest("No existe Cliente");

            clienteUpdateEmpresaCreada = mapper.Map(putClienteEmpresaCreadaDTo, clienteUpdateEmpresaCreada);

            await context.SaveChangesAsync();
            var ClienteEmpresaCreada = new
                   ViewClienteEmpresaCreadaDTo()
            {
                EmpresasCreadas = clienteUpdateEmpresaCreada.EmpresasCreadas,
                Identificacion = clienteUpdateEmpresaCreada.Identificacion,
                Razonsocial = clienteUpdateEmpresaCreada.Razonsocial

            };
            return Ok(ClienteEmpresaCreada);
        }
        /// <summary>
        /// Poner en estado desactioco el clientre
        /// </summary>
        /// <param name="deleteClienteEmpresaCreadaDTo"></param>
        /// <returns></returns>

        [HttpDelete("DeleteClienteEmpresaCreada")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ViewClienteEmpresaCreadaDTo>> DeleteClienteEmpresaCreada([FromQuery] DeleteClienteEmpresaCreadaDTo deleteClienteEmpresaCreadaDTo)
        {
            var curentUser = HttpContext.User;
            var claims = curentUser.Claims.ToList();
            if (claims.Count == 0)
                return BadRequest("No tiene permiso para esta seccion");
            var empresaCreaada = await context.EmpresasCreadas.FirstOrDefaultAsync(x => x.FkEmpresa.Equals(Convert.ToInt32(claims[1].Value))
                 && x.Id.Equals(Convert.ToInt32(claims[2].Value)));
            if (empresaCreaada == null)
                return BadRequest("No existe Empresa");
            var clienteUpdate = await context.Clientes.FirstOrDefaultAsync(x => x.Id.Equals(deleteClienteEmpresaCreadaDTo.Id));
            if (clienteUpdate == null)
                return BadRequest("No existe Cliente");
            clienteUpdate.Estado = false;
            await context.SaveChangesAsync();

            return Ok("Cliente en esta Desactivo");
        }
    }
}