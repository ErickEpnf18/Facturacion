using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Facturacion.Context;
using Facturacion.DTOs;
using Facturacion.Services.Interfaces;

namespace Facturacion.Controllers
{
    [ApiController]
    [Route("api/listados/EmpresasCreadas")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public class ListadosController : Controller
    {
        DateTime currentTimePacific = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time"));
        private readonly IConfiguration _configuration;
        private readonly AplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IAlmacenadorArchivos almacenadorArchivos;

        public ListadosController( IConfiguration configuration,
            AplicationDbContext context, 
            IMapper mapper,
            IAlmacenadorArchivos almacenadorArchivos)
        {
   
            _configuration = configuration;
            this.context = context;
            this.mapper = mapper;
            this.almacenadorArchivos = almacenadorArchivos;
        }
        /// <summary>
        /// Trae el listado de Tipos de  Compronbantes
        /// </summary>
        /// <remarks>
        /// Tomar el Ejemplo del pie de pagina
        /// http://demo.factura.link/index.php/escritorio/datosconfiguracion
        /// </remarks>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("GetTipoComprobante")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ViewTipoComprobante>>> GetTipoComprobante()
        {
            var TipoComprobamte = await context.Comprobantes.ToListAsync();
            return mapper.Map<List<ViewTipoComprobante>>(TipoComprobamte);
        }

        


    }
}
