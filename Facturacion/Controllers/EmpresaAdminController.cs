using AutoMapper;
using Facturacion.Context;
using Facturacion.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Facturacion.Model;
using Facturacion.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Facturacion.DTOs;

namespace Facturacion.Controllers
{
    [ApiController]
    [Route("api/AdminEmpresa")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]

    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "AdminNes")]

    public class EmpresaAdminController : Controller
    {
        DateTime currentTimePacific = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time"));

        private readonly UserManager<NetUserAditional> _userManager;
        private readonly SignInManager<NetUserAditional> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly AplicationDbContext context;
        private readonly IMapper mapper;

        private readonly IAlmacenadorArchivos almacenadorArchivos;
   //     private readonly IEmailSender emailSender;

        private readonly IWebHostEnvironment webHostEnvironment;

        public EmpresaAdminController(UserManager<NetUserAditional> userManager,
            SignInManager<NetUserAditional> signInManager, IConfiguration configuration, AplicationDbContext context,
            IMapper mapper, IAlmacenadorArchivos almacenadorArchivos, 
            IWebHostEnvironment webHostEnvironment)
        {

            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            this.context = context;
            this.mapper = mapper;
            this.almacenadorArchivos = almacenadorArchivos;
         //   this.emailSender = emailSender;

            this.webHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        /// Inmgreso De empresas Administradoras 
        /// </summary>
        /// <remarks>
        /// Se crea con el La empresa que es dueña del sistema o la que compro 
        /// La Empresa Administradora es Una sola que esta en la tabla Empresa campo EmpresaDueno
        /// Es el unico que puede crear Empresas Administradoras.
        /// El puede Crear Empresas Admin Pero no Empresa Dueno
        /// </remarks>
        /// <param name="postEmpresaCreada"></param>
        /// <returns></returns>
        [HttpPost("PostEmpresaAdmin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ViewEmpresa>> PostEmpresaAdmin([FromBody] IngresoRepoSriDto ingresoRepoSriDto)
        {
            var curentUser = HttpContext.User;
            var claims = curentUser.Claims.ToList();
            if (claims.Count == 0)
                return BadRequest("No tiene permiso para esta seccion");
            //  AdminEmpresaDueno

            Random generator = new Random();
            String numeroGenerado = generator.Next(0, 100000000).ToString("D6");
            SriRepositorio? sriRepositorio = null;
            sriRepositorio = await context.SriRepositorios.FirstOrDefaultAsync(x => x.NumeroRuc.Equals(ingresoRepoSriDto.NumeroRuc));

            if (sriRepositorio == null)
            {

                //if (getsriRepositorio == null)
                //    return BadRequest($"No existe Contribuyente con el siguiente Ruc {postEmpresaCreada.Ruc}");
                //sriRepositorio = mapper.Map<SriRepositorio>(getsriRepositorio);


                try
                {
                    var EmpresaMappe = mapper.Map<SriRepositorio>(ingresoRepoSriDto);
                    await context.AddAsync(EmpresaMappe);
                    await context.SaveChangesAsync();

                    return Ok(new ViewEmpresaDto
                    {
                        Id = EmpresaMappe.Id,
                        EmpresaRuc = EmpresaMappe.NumeroRuc,

                        Mensaje = "Usuario Empresa Creada Satisfactoriamente"
                    });
                }
                catch (Exception ex)
                {

                    var exasd = ex.ToString();
                    return BadRequest(exasd);
                }
            

            }
            return BadRequest();
            var identityUser = await _userManager.FindByEmailAsync(ingresoRepoSriDto.Email);
            if (identityUser != null)
                return BadRequest($"Ya existe Empresa con ese  correo {identityUser.Email},Empresa= {identityUser.FkEmpresa}");

            var empresa = new Empresa
            {
                EmpresaRuc = sriRepositorio.NumeroRuc,
                EmpresaPropietario = sriRepositorio.RazonSocial,
                EmpresaEmail = ingresoRepoSriDto.Email,
                EmpresaEstado = true,
                EmpresaUsuarioCreador = claims[0].Value,
                EmpresaTelefono = "",


            };
            await context.AddAsync(empresa);
            await context.SaveChangesAsync();

            string EmpresaCarpeta = Path.Combine(webHostEnvironment.WebRootPath, "DatosEmpresaAdmin-" + sriRepositorio.NumeroRuc);

            var user = new NetUserAditional
            {
                UserName = ingresoRepoSriDto.NumeroRuc,
                Email = ingresoRepoSriDto.Email,
                FechaDeRegistro = currentTimePacific,
                NumeroparaConfirmacion = numeroGenerado,
                FkEmpresa = empresa.Id,
                FkEmpresaCreada = null,
                FkUsuario = null,
                FkNetUserid = claims[0].Value,
                EmailConfirmed = true
               


            };
            try
            {
                var result = await _userManager.CreateAsync(user, ingresoRepoSriDto.Password);
                if (result.Succeeded)
                {
                    //await emailSender.SendEmailAsync(user.Email, numeroGenerado, "");
                    await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "AdminEmpresa"));
                    await _userManager.AddToRoleAsync(user, "AdminEmpresa");
                    ;
                    return Ok(new ViewEmpresaDto
                    {
                        Id = empresa.Id,
                        EmpresaRuc = empresa.EmpresaRuc,
                        EmpresaPropietario = empresa.EmpresaPropietario,
                        EmpresaEmail = empresa.EmpresaEmail,
                        EmpresaEstado = empresa.EmpresaEstado,
                        EmpresaTelefono = empresa.EmpresaTelefono,
                        EmpresaUsuarioCreador = empresa.EmpresaUsuarioCreador,
                        Mensaje = "Usuario Empresa Creada Satisfactoriamente"
                    });


                }
                return BadRequest("No tiene permiso para esta seccion");

            }
            catch (Exception ex)
            {

                return BadRequest(ex.ToString());
            }


        }

        /// <summary>
        /// Actulizar Empresa Creada por la EmpresaDueno
        /// </summary>
        /// <param name="putEmpresaDto"></param>
        /// <returns></returns>
        /// 
        [HttpPut("PutEmpresaAdmin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ViewEmpresaDto>> PutEmpresaAdmin([FromForm] PutEmpresaDto putEmpresaDto)
        {
            var curentUser = HttpContext.User;
            var claims = curentUser.Claims.ToList();
            if (claims.Count == 0)
                return BadRequest("No tiene permiso para esta seccion");
            var PutEmpresa = await context.Empresas.FirstOrDefaultAsync(x => x.Id.Equals(putEmpresaDto.Id));
            if (PutEmpresa == null)
                return BadRequest("No existe Empresa para Actualizar");

            string EmpresaCarpeta = Path.Combine(webHostEnvironment.WebRootPath, "DatosEmpresaAdmin-" + PutEmpresa.EmpresaRuc);
            if (!Directory.Exists(EmpresaCarpeta))
            {
                Directory.CreateDirectory(EmpresaCarpeta);
            }
            if (putEmpresaDto.EmpresaImagen != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await putEmpresaDto.EmpresaImagen.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(putEmpresaDto.EmpresaImagen.FileName);
                    PutEmpresa.EmpresaImagen = await almacenadorArchivos.EditarArchivo(contenido, extension, "DatosEmpresaAdmin-" + PutEmpresa.EmpresaRuc,
                    PutEmpresa.EmpresaImagen, putEmpresaDto.EmpresaImagen.ContentType);
                }

            }

            PutEmpresa.EmpresaEmail = putEmpresaDto.EmpresaEmail.Trim();
            PutEmpresa.EmpresaEstado = putEmpresaDto.EmpresaEstado;
            PutEmpresa.EmpresaTelefono = putEmpresaDto.EmpresaTelefono;
            await context.SaveChangesAsync();
            return Ok(new ViewEmpresa
            {
                Id = PutEmpresa.Id,
                EmpresaRuc = PutEmpresa.EmpresaRuc,
                EmpresaPropietario = PutEmpresa.EmpresaPropietario,
                EmpresaEmail = PutEmpresa.EmpresaEmail,
                EmpresaEstado = PutEmpresa.EmpresaEstado,
                EmpresaTelefono = PutEmpresa.EmpresaTelefono,
                EmpresaUsuarioCreador = PutEmpresa.EmpresaUsuarioCreador,
                Mensaje = "Usuario Empresa Creada Satisfactoriamente"
            });

        }
        /// <summary>
        /// Traer Empresa Admin para poderla administrar  editar
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        [HttpGet("GetEmpresaAdmin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ViewEmpresaDto>> GeEmpresasCreada([FromQuery] IdEmpresaDto idEmpresa)
        {
            var curentUser = HttpContext.User;
            var claims = curentUser.Claims.ToList();
            if (claims.Count == 0)
                return BadRequest("No tiene permiso para esta seccion");
            var GetEmpresa = await context.Empresas.FirstOrDefaultAsync(x => x.Id.Equals(idEmpresa.IdEmpresaAdmin));
            if (GetEmpresa == null)
                return BadRequest("No existe Empresa ");

            return mapper.Map<Empresa, ViewEmpresaDto>(GetEmpresa);

        }
        /// <summary>
        /// Traer todas las empresas Administradoras que se an creado
        /// </summary>
        /// <returns>ViewEmpresaDto</returns>

        [HttpGet("GetListEmpresaAdmin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ViewEmpresaDto>>> GetListEmpresasAdmin()
        {
            var curentUser = HttpContext.User;
            var claims = curentUser.Claims.ToList();
            if (claims.Count == 0)
                return BadRequest("No tiene permiso para esta seccion");
            var GetEmpresa = await context.Empresas.Where(x => x.EmpresaUsuarioCreador.Equals(claims[0].Value)).ToListAsync();
            if (GetEmpresa == null)
                return BadRequest("No existe Empresa ");

            return mapper.Map<List<Empresa>, List<ViewEmpresaDto>>(GetEmpresa);

        }

        //permisos
        /// <summary>
        /// Traer empresa para poderla dar los permisos
        /// </summary>
        /// <param name="idEmpresaPermisoAdmin"></param>
        /// <returns></returns>
        [HttpGet("GetPermisoEmpresa")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ViewPermisosEmpresaDTo>> GetPermisosEmpresa([FromQuery] IdPermisosEmpresaPermisos idEmpresaPermisoAdmin)
        {
            var curentUser = HttpContext.User;
            var claims = curentUser.Claims.ToList();
            if (claims.Count == 0)
                return BadRequest("No tiene permiso para esta seccion");
            var getEmpresaPermisos = await context.PermisosEmpresas.
                FirstOrDefaultAsync(x => x.Id.Equals(idEmpresaPermisoAdmin.IdPermisos));


            if (getEmpresaPermisos != null)
            {

                var viewEmpresaPermisos = mapper.Map<PermisosEmpresa, ViewPermisosEmpresaDTo>(getEmpresaPermisos);
                return viewEmpresaPermisos;

            }
            return BadRequest("No se encuentra Permisas Empresa");

        }

        [HttpGet("GetEmpresaInformacionGeneral")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ViewEmpresaInformacionGeneralDTO>> GetEmpresaInformacionGeneral([FromQuery] EmpresaCreadaIdDto empresaCreadaIdDto)
        {
            try
            {
                var curentUser = HttpContext.User;
                var claims = curentUser.Claims.ToList();
                if (claims.Count == 0)
                    return BadRequest("No tiene permiso para esta seccion");
                var empresa = await context.Empresas.
                    Include(x => x.Usuarios).
                    Include(x => x.FkPermisosEmpresaNavigation).
                    Include(x => x.FkCorreoNavigation).
                    FirstOrDefaultAsync(x => x.Id.Equals(Convert.ToInt32(claims[1].Value))
                    && x.Id.Equals(empresaCreadaIdDto.IdEmpresaCreada));
                var srirepositorio = await context.SriRepositorios

                    .FirstOrDefaultAsync(x => x.Id.Equals(empresa.FkSriRepositorio));


                if (empresa != null)
                {

                    var EmpresaMappe = mapper.Map<ViewEmpresaInformacionGeneralDTO>(empresa);
                    return Ok(EmpresaMappe);


                }
                return BadRequest("No se encuentra Empresa");

            }
            catch (Exception ex)
            {

                return BadRequest(ex.ToString());
            }


        }





        /// <summary>
        /// Traer todas los Permisos de cada una de las Empresas 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetListPermisosEmpresa")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ViewPermisosEmpresaDTo>>> GetListPermisosEmpresa()
        {
            var curentUser = HttpContext.User;
            var claims = curentUser.Claims.ToList();
            if (claims.Count == 0)
                return BadRequest("No tiene permiso para esta seccion");
            var GetEmpresaCreadaPemisos = await context.PermisosEmpresas.Include(x => x.FkEmpresaNavigation).
                Where(x => x.FkEmpresaNavigation.EmpresaUsuarioCreador.Equals(claims[0].Value)).ToListAsync();

            if (GetEmpresaCreadaPemisos != null)
            {

                var viewEmpresaCreadaPermisos = mapper.Map<List<PermisosEmpresa>, List<ViewPermisosEmpresaDTo>>(GetEmpresaCreadaPemisos);
                return viewEmpresaCreadaPermisos;



            }
            return BadRequest("No se encuentra Permisas Empresa");

        }


        /// <summary>
        /// agregar permisos a emopresa
        /// </summary>
        /// <param name="permisosEmpresaDTo"></param>
        /// <returns></returns>
        [HttpPost("PostPermisosEmpresas")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ViewPermisosEmpresaDTo>> PostPermisosEmpresas([FromBody] IdEmpresaPermisoAdmin permisosEmpresaDTo)
        {
            var curentUser = HttpContext.User;
            var claims = curentUser.Claims.ToList();
            if (claims.Count == 0)
                return BadRequest("No tiene permiso para esta seccion");
            var empresaPermisos = await context.PermisosEmpresas.
             FirstOrDefaultAsync(x => x.FkEmpresa.Equals(Convert.ToInt32(permisosEmpresaDTo.IdEmpresaAdmin)));

            if (empresaPermisos == null)
            {
                var postEmpresaPemiso = mapper.Map<PermisosEmpresa>(permisosEmpresaDTo);
                postEmpresaPemiso.FkEmpresa = permisosEmpresaDTo.IdEmpresaAdmin;

                await context.AddAsync(postEmpresaPemiso);
                await context.SaveChangesAsync();
                var vievPermisosCreada = mapper.Map<ViewPermisosEmpresaDTo>(postEmpresaPemiso);
                return Created("", vievPermisosCreada);
            }
            return BadRequest(" Permisas Empresa ya Asignados");

        }
        /// <summary>
        /// upsate permisos empresa
        /// </summary>
        /// <param name="idEmpresaPermisoAdmin"></param>
        /// <returns></returns>
        [HttpPut("PutPermisosEmpresas")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ViewPermisosEmpresaDTo>> PutPermisosEmpresas([FromBody] IdEmpresaPermisoAdmin idEmpresaPermisoAdmin)
        {
            var curentUser = HttpContext.User;
            var claims = curentUser.Claims.ToList();
            if (claims.Count == 0)
                return BadRequest("No tiene permiso para esta seccion");
            var empresaPermisos = await context.PermisosEmpresas.
                  FirstOrDefaultAsync(x => x.FkEmpresa.Equals(idEmpresaPermisoAdmin.IdEmpresaAdmin)
           );

            if (empresaPermisos != null)
            {
                empresaPermisos = mapper.Map(idEmpresaPermisoAdmin, empresaPermisos);

                empresaPermisos.FkEmpresa = idEmpresaPermisoAdmin.IdEmpresaAdmin;
                await context.SaveChangesAsync();
                var vievPermisosCreada = mapper.Map<ViewPermisosEmpresaDTo>(empresaPermisos);
                return Ok(vievPermisosCreada);
            }
            return BadRequest("No se encuentra Permisas Empresa");

        }
        /// <summary>
        /// traer Correo Empresa Admin
        /// </summary>
        /// <param name="correoEmpresaIdDto"></param>
        /// <returns></returns>
        [HttpGet("GetCorreoEmpresas")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ViewCorreoEmpresaDto>> GetCorreoEmpresa([FromBody] CorreoEmpresaIdDto correoEmpresaIdDto)
        {
            var curentUser = HttpContext.User;
            var claims = curentUser.Claims.ToList();


            if (claims.Count == 0)
                return BadRequest("No tiene permiso para esta seccion");
            var getCorreoEmpresa = await context.CorreoEmpresas.Include(x => x.FkEmpresaNavigation)
                 .FirstOrDefaultAsync(x => x.FkEmpresa.Equals(correoEmpresaIdDto.fkempresa)
                  && x.Id.Equals(correoEmpresaIdDto.IdCorreo));
            if (getCorreoEmpresa != null)
            {
                var viewEmpresaPermisos = mapper.Map<CorreoEmpresa, ViewCorreoEmpresaDto>(getCorreoEmpresa);
            }
            return BadRequest("No se encuentra Permisas Empresa");

        }


        /// <summary>
        /// Crear correo empresa Creada
        /// </summary>
        /// <param name="permisosEmpresaCreadaDTo"></param>
        /// <returns></returns>
        [HttpPost("PostCorreoEmpresa")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ViewCorreoEmpresaDto>> PostCorreoEmpresaCreada([FromBody] CorreoEmpresaDto permisosEmpresaCreadaDTo)
        {
            var curentUser = HttpContext.User;
            var claims = curentUser.Claims.ToList();
            if (claims.Count == 0)
                return BadRequest("No tiene permiso para esta seccion");

            var GetEmpresaCoreo = await context.CorreoEmpresas.
                FirstOrDefaultAsync(x => x.FkEmpresasCreadas.Equals(permisosEmpresaCreadaDTo.IdEmpresaCreada)
                && x.FkEmpresa.Equals(claims[1].Value));

            if (GetEmpresaCoreo == null)
            {

                var postEmpresaCorreo = mapper.Map<CorreoEmpresa>(GetEmpresaCoreo);
                postEmpresaCorreo.FkEmpresa = Convert.ToInt32(claims[1].Value);
                postEmpresaCorreo.FkEmpresasCreadas = permisosEmpresaCreadaDTo.IdEmpresaCreada;
                await context.AddAsync(postEmpresaCorreo);
                await context.SaveChangesAsync();
                var vievPermisos = mapper.Map<ViewCorreoEmpresaDto>(postEmpresaCorreo);
                vievPermisos.Mensajes = "Coreo Creado Satisfactoriamente";

                return Created("", vievPermisos);

            }
            return BadRequest(" Se encuentra creado el correo  Empresa");

        }
        /// <summary>
        /// Actualizar Correo
        /// </summary>
        /// <param name="correoEmpresaDto"></param>
        /// <returns></returns>
        [HttpPut("PutCorreoEmpresa")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ViewCorreoEmpresaDto>> PutCorreoEmpresaCreada([FromBody] updateCorreoEmpresaDto correoEmpresaDto)
        {
            var curentUser = HttpContext.User;
            var claims = curentUser.Claims.ToList();
            if (claims.Count == 0)
                return BadRequest("No tiene permiso para esta seccion");
            var GetEmpresaCoreo = await context.CorreoEmpresas.
                     FirstOrDefaultAsync(x => x.FkEmpresasCreadas.Equals(correoEmpresaDto.IdEmpresaCreada)
                     && x.FkEmpresa.Equals(claims[1].Value) && x.Id.Equals(correoEmpresaDto.IdEmpresaCreada));

            if (GetEmpresaCoreo != null)
            {

                var postEmpresaCorreo = mapper.Map<CorreoEmpresa>(correoEmpresaDto);
                context.Update(postEmpresaCorreo);
                await context.SaveChangesAsync();
                var vievPermisos = mapper.Map<ViewCorreoEmpresaDto>(postEmpresaCorreo);
                vievPermisos.Mensajes = "Coreo Actualizado Satisfactoriamente";

                return Ok(vievPermisos);

            }
            return BadRequest("No se encuentra Permisas Empresa");

        }




    }


}