using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Facturacion.Context;
using Facturacion.DTOs;
using Facturacion.Middleware;
using Facturacion.Model;
using Facturacion.Services;
using Facturacion.Services.FacturacionElectronica.Services;
using Facturacion.Services.Interfaces;
using System.Security.Claims;
namespace Facturacion.Controllers
{
    [ApiController]
    [Route("api/EmpresaCreada/ComprovanteVenta")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "UsariosNesEmpresaCreada")]

    public class EmpresaCreadaComprobanteVentaController : Controller
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
        public EmpresaCreadaComprobanteVentaController(UserManager<NetUserAditional> userManager,
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
        /// Listod de Comprobantes de cada uno de las Empresas Creadas
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetListComprobantesVentaEmpresaCreada")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ComprobanteVentaECDto>>> GetListComprobantesVentaEmpresaCreada()
        {
            var curentUser = HttpContext.User;
            var claims = curentUser.Claims.ToList();
            if (claims.Count == 0)
                return BadRequest("No tiene permiso para esta seccion");
            var comprobanteVenta = await context.ComprobanteVentas.Where(x => x.FkEmpresa.Equals(Convert.ToInt32(claims[1].Value)) &&
            x.FkEmpresasCreada.Equals(Convert.ToInt32(claims[2].Value))).ToListAsync();
            if (comprobanteVenta == null)
                return BadRequest("No tiene Comprobantes de Venta");

            var comprobantesVenta = mapper.Map<List<ComprobanteVenta>, List<ComprobanteVentaECDto>>(comprobanteVenta);
            return Ok(comprobantesVenta);
        }
        /// <summary>
        /// Traer toda le informacion del comprobante de venta
        /// </summary>
        /// <param name="idComprobanteVentaDto"></param>
        /// <returns></returns>
        [HttpGet("GetComprobanteVentaEmpresaCreada")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ComprobanteVentaCompletoECDto>> GeTComprobantesVentaEmpresaCreada([FromQuery] IdComprobanteVentaDto idComprobanteVentaDto)
        {
            var curentUser = HttpContext.User;
            var claims = curentUser.Claims.ToList();
            if (claims.Count == 0)
                return BadRequest("No tiene permiso para esta seccion");
            var getcomprobanteVenta = await context.ComprobanteVentas.FirstOrDefaultAsync(x => x.FkEmpresa.Equals(Convert.ToInt32(claims[1].Value)) &&
            x.FkEmpresasCreada.Equals(Convert.ToInt32(claims[2].Value)) && x.Id.Equals(idComprobanteVentaDto.IdComprobanteventa) && x.ComprobanteEstado.Equals(true));
            if (getcomprobanteVenta == null)
                return BadRequest("No Existe Comprobantes de Venta");

            var comprobanteVenta = mapper.Map<ComprobanteVenta, ComprobanteVentaCompletoECDto>(getcomprobanteVenta);
            return Ok(comprobanteVenta);
        }


        /// <summary>
        /// Traer secuencial para la comprobanteVenta
        /// </summary>
        /// <remarks>
        ///  http://demo.factura.link/index.php/escritorio/venta 
        /// <returns></returns>


        [HttpGet("GetSeucencialComprobanteEmpresaCreada")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SecuencialDto>> GetSeucencialComprobanteEmpresaCreada()
        {
            var curentUser = HttpContext.User;
            var claims = curentUser.Claims.ToList();
            if (claims.Count == 0)
                return BadRequest("No tiene permiso para esta seccion");
            var secuencial = await context.Secuencials.FirstOrDefaultAsync(x => x.FkEmpresa.Equals(Convert.ToInt32(claims[1].Value)) &&
            x.FkEmpresasCreada.Equals(Convert.ToInt32(claims[2].Value)) && x.Estado.Equals(true));
            if (secuencial == null)
                return BadRequest("No tiene Secuencial Registrado para esta seccion");

            var viewSecuencial = mapper.Map<SecuencialDto>(secuencial);
            string strSecuencial = "000000000";
            int num1 = strSecuencial.Length;
            int numerador = Convert.ToInt32(secuencial.Numcorrelativo);
            int num2 = numerador.ToString().Length;
            string secuencial1 = strSecuencial.Insert(num1 - num2, numerador.ToString());
            viewSecuencial.NumComprobante = secuencial.Numpuntoemision + "-" + secuencial.Numestablecimiento + secuencial1.Substring(0, 9).Trim();
            return Ok(viewSecuencial);



        }

        /// <summary>
        /// Trae los Clientes de cada una de las Empresas Creadas para la factura de Venta
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetListClientesComprobanteVentaEmpresaCreada")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<GetListClienteEmpresaCreadaDto>>> GetListClientesComprobanteVentaEmpresaCreada()
        {
            var curentUser = HttpContext.User;
            var claims = curentUser.Claims.ToList();
            if (claims.Count == 0)
                return BadRequest("No tiene permiso para esta seccion");
            var getListClienteEmpresa = await context.Clientes.
                Where(x => x.FkEmpresa.Equals(Convert.ToInt32(claims[1].Value)) &&
                x.EmpresasCreadas.Equals(Convert.ToInt32(claims[2].Value)))
                          .ToListAsync();

            if (getListClienteEmpresa != null)
            {

                var viewCorreoEmpresa = mapper.Map<List<Cliente>, List<GetListClienteEmpresaCreadaDto>>(getListClienteEmpresa);
                return viewCorreoEmpresa;

            }
            return BadRequest("No se encuentra Permisas Empresa");

        }


        /// <summary>
        /// Lista de formas de pago
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetListFormasPago")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ViewFormasPagoDto>>> GetListFormasPago()
        {
            var formasPagos = await context.FormasPagos.ToListAsync();
            return mapper.Map<List<ViewFormasPagoDto>>(formasPagos);
        }


        /// <summary>
        /// Trae los Productos o servicios 
        /// </summary>
        /// <returns></returns>

        [HttpGet("GetListProductoComprobanteVentaEmpresaCreada")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ViewProductoDto>> GetListProductoComprobanteVentaEmpresaCreada()
        {
            var curentUser = HttpContext.User;
            var claims = curentUser.Claims.ToList();
            if (claims.Count == 0)
                return BadRequest("No tiene permiso para esta seccion");
            var getProducto = await context.Productos.
                Where(x => x.FkEmpresa.Equals(Convert.ToInt32(claims[1].Value))
                && x.FkEmpresasCreada.Equals(Convert.ToInt32(claims[2].Value)) && x.ProductoEstado.Equals(true)).ToListAsync();
            if (getProducto != null)
            {
                var Prodcuto = mapper.Map<List<ViewProductoDto>>(getProducto);

                return Ok(Prodcuto);

            }
            return BadRequest("No existe Producto");

        }

        /// <summary>
        /// Ingreso de compribante de venta 
        /// </summary>
        /// <param name="postComprobanteVentaECDto"></param>
        /// <returns></returns>
        [HttpPost("PostComprobanteVentaEmpresaCreada")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ViewEmpresaCreadaComprobanteCompraDTO>> PostComprobanteComprasEmpresaCreada([FromBody] PostComprobanteVentaECDto postComprobanteVentaECDto)
        {
            try
            {
                var curentUser = HttpContext.User;
                var claims = curentUser.Claims.ToList();
                if (claims.Count == 0)
                    return BadRequest("No tiene permiso para esta seccion");
                if (postComprobanteVentaECDto.detalleComprobanteVentaDtos == null)
                    return BadRequest("No tiene detalles para esta seccion");
                if (postComprobanteVentaECDto == null)
                    return BadRequest("No tiene comprobante para esta seccion");

                var empresaCreada = await context.EmpresasCreadas.FirstOrDefaultAsync(x => x.FkEmpresa.Equals(Convert.ToInt32(claims[1].Value))
                && x.Id.Equals(Convert.ToInt32(claims[2].Value)));
                if (empresaCreada == null)
                    return BadRequest("No tiene existe Empresa");

                var PoscomprobanteVenta = mapper.Map<ComprobanteVenta>(postComprobanteVentaECDto);
                PoscomprobanteVenta.FkEmpresa = Convert.ToInt32(claims[1].Value);
                PoscomprobanteVenta.FkEmpresasCreada = Convert.ToInt32(claims[2].Value);
                PoscomprobanteVenta.FkUsuario = Convert.ToInt32(claims[4].Value);
                await context.AddAsync(PoscomprobanteVenta);
                await context.SaveChangesAsync();

                foreach (var item in postComprobanteVentaECDto.detalleComprobanteVentaDtos)
                {
                    var producto = await context.Productos.FirstOrDefaultAsync(x => x.Id.Equals(item.FkProducto));
                    var PoscomprobanteDetalleVenta = mapper.Map<DetalleVenta>(item);
                    PoscomprobanteDetalleVenta.FkComprobanteVenta = PoscomprobanteVenta.Id;
                    PoscomprobanteDetalleVenta.DetallevValor = producto.ProductoValor;
                    PoscomprobanteDetalleVenta.DetallevEstado = "ACT";
                    PoscomprobanteDetalleVenta.DetallevTotal = (decimal)(item.DetallevCantidad * producto.ProductoValor);
                    if (producto.ProductoConIva == true)
                        PoscomprobanteDetalleVenta.DetalleTotalIva = (decimal)(item.DetallevCantidad * producto.ProductoValor) * (empresaCreada.EmpresasPorcentajeIva / 100m);
                    else
                        PoscomprobanteDetalleVenta.DetalleTotalIva = 0.00m;

                    await context.AddAsync(PoscomprobanteDetalleVenta);
                    await context.SaveChangesAsync();
                }

                var VentaDetalle = await context.DetalleVentas.Where(x => x.FkComprobanteVenta.Equals(PoscomprobanteVenta.Id)).ToListAsync();

                List<decimal> Subtotal12 = new List<decimal>();
                List<decimal> Subtotal0 = new List<decimal>();
                foreach (var item in VentaDetalle)
                {
                    if (item.DetalleTotalIva > 0.00m)
                        Subtotal12.Add(item.DetallevTotal);
                    else
                        Subtotal0.Add(item.DetallevTotal);


                }
                var sumasSubtotal = Subtotal12.Sum() + Subtotal0.Sum();
                var ivaSubtotal = (Subtotal12.Sum() * (empresaCreada.EmpresasPorcentajeIva / 100));
                PoscomprobanteVenta.ComprobanteSubtotal0 = Subtotal0.Sum();
                PoscomprobanteVenta.ComprobanteSubtotal12 = Subtotal12.Sum();
                PoscomprobanteVenta.ComprobanteSubtotal = sumasSubtotal;
                PoscomprobanteVenta.ComprobanteIvatotal = ivaSubtotal;
                PoscomprobanteVenta.ComprobantevTotal = sumasSubtotal + ivaSubtotal;
                PoscomprobanteVenta.ComprobantevFecha = currentTimePacific;
                PoscomprobanteVenta.ComprobanteEstado = "GEN";
                PoscomprobanteVenta.ComprobanteFormapago = "";
                foreach (var item in postComprobanteVentaECDto.iListFormasPagoDtos)
                {
                    PoscomprobanteVenta.ComprobanteFormapago = PoscomprobanteVenta.ComprobanteFormapago + item.Codigo +";";

                }
                var secuencialupdate = await context.Secuencials.FirstOrDefaultAsync(x => x.Id.Equals(PoscomprobanteVenta.FkSecuencial));
                string strSecuencial = "000000000";
                int num1 = strSecuencial.Length;
                int numerador = Convert.ToInt32(secuencialupdate.Numcorrelativo);
                int num2 = numerador.ToString().Length;
                string secuencial1 = strSecuencial.Insert(num1 - num2, numerador.ToString());
                PoscomprobanteVenta.ComprobanteNumero = secuencialupdate.Numpuntoemision + "-" + secuencialupdate.Numestablecimiento + "-" + secuencial1.Substring(0, 9).Trim();
                secuencialupdate.Numcorrelativo = secuencialupdate.Numcorrelativo + 1;
                await context.SaveChangesAsync();

                string IdSFormapago = string.Empty;
                var FechaParacajaGlobal = PoscomprobanteVenta.ComprobantevFecha.ToString("yyyy-MM-dd");
                var CajaGlobalDiara = await context.CajaGlobals.

                 FirstOrDefaultAsync(x => x.FkEmpresa.Equals(claims[1].Value)
              && x.EmpresasCreadas.Equals(claims[2].Value) && x.FechaIngreso.ToString().Equals(FechaParacajaGlobal));
                foreach (var item in postComprobanteVentaECDto.iListFormasPagoDtos)
                {

                    IdSFormapago = item.Codigo + "," + IdSFormapago;
                    if (item.Equals("01") || item.Equals("15") || item.Equals("17") || item.Equals("21") || item.Equals("20"))
                    {
                        PoscomprobanteVenta.ComprobantevTotal = sumasSubtotal + ivaSubtotal;
                        //comprobanteVenta.Formapago = formaDepagoDTO.FormadePago;
                        if (CajaGlobalDiara != null)
                        {
                            CajaGlobalDiara.CajaGlobalEfectivoTotal = CajaGlobalDiara.CajaGlobalEfectivoTotal + (sumasSubtotal + ivaSubtotal);

                            await context.SaveChangesAsync();
                        }
                        else
                        {
                            var cajaDiaria = new CajaGlobal
                            {
                                FkEmpresa = Convert.ToInt32(claims[1].Value),
                                EmpresasCreadas = Convert.ToInt32(claims[2].Value),
                                FechaIngreso = currentTimePacific,
                                CajaGlobalEfectivoTotal = sumasSubtotal + ivaSubtotal,
                                CajaGlobalTarjetasDebitoTotal = 0.00m,
                                CajaGlobalTrasferenciaTotal = 0.00m,
                                CajaGlobalCreditoTotal = 0.00m,
                                CajaGlobalTotalValores = sumasSubtotal + ivaSubtotal,

                            };

                            await context.AddAsync(cajaDiaria);
                            await context.SaveChangesAsync();

                        }

                    }
                    if (item.Equals("16") || item.Equals("19") || item.Equals("18"))
                    {
                        PoscomprobanteVenta.ComprobantevTotal = sumasSubtotal + ivaSubtotal;
                        //comprobanteVenta.Formapago = formaDepagoDTO.FormadePago;
                        if (CajaGlobalDiara != null)
                        {
                            CajaGlobalDiara.CajaGlobalTarjetasDebitoTotal = CajaGlobalDiara.CajaGlobalTarjetasDebitoTotal + (sumasSubtotal + ivaSubtotal);
                            await context.SaveChangesAsync();
                        }
                        else
                        {
                            var cajaDiaria = new CajaGlobal
                            {
                                FkEmpresa = Convert.ToInt32(claims[1].Value),
                                EmpresasCreadas = Convert.ToInt32(claims[2].Value),
                                FechaIngreso = currentTimePacific,
                                CajaGlobalEfectivoTotal = 0.00m,
                                CajaGlobalTarjetasDebitoTotal = sumasSubtotal + ivaSubtotal,
                                CajaGlobalTrasferenciaTotal = 0.00m,
                                CajaGlobalCreditoTotal = 0.00m,
                                CajaGlobalTotalValores = sumasSubtotal + ivaSubtotal,

                            };

                            await context.AddAsync(cajaDiaria);
                            await context.SaveChangesAsync();

                        }

                    }
                }
                PoscomprobanteVenta.ComprobanteFormapago = IdSFormapago;

                var ViewComprobante = new ViewComprobanteVentaECDto
                {
                    Id = PoscomprobanteVenta.Id,
                    FkCliente = PoscomprobanteVenta.Id,
                    Mensaje = "Comprobante Creado Corectamente"

                };
                return Created("", ViewComprobante);


            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }



        }


        ///put oara actulizar facturas
    }

}