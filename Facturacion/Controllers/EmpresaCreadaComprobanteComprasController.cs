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
using System.Data;
using System.Security.Claims;
using System.Xml.Linq;
using V4Nessoft.Services.FacturacionElectronica.Services;
using V4Nessoft.Services.SriServices;

namespace Facturacion.Controllers
{

    [ApiController]
    [Route("api/EmpresaCreada/ComprobanteCompras")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "UsariosNesEmpresaCreada")]
    public class EmpresaCreadaComprobanteComprasController : Controller
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
        private readonly CRespuestaAutorizacionSRI cRespuestaAutorizacionSRI;
        private readonly CSriws cSriws;

        public EmpresaCreadaComprobanteComprasController(UserManager<NetUserAditional> userManager,
            SignInManager<NetUserAditional> signInManager,
            IConfiguration configuration, AplicationDbContext context, IMapper mapper,
           IAlmacenadorArchivos almacenadorArchivos, IEmailSender emailSender,
            CConsultaSri cConsultaSri, CFuncionCedulas cFuncionCedulas, CSriws cSriws)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            this.context = context;
            this.mapper = mapper;
            this.cSriws = cSriws;
            this.almacenadorArchivos = almacenadorArchivos;
            this.emailSender = emailSender;
            this.cConsultaSri = cConsultaSri;
            this.cFuncionCedulas = cFuncionCedulas;

        }

        [HttpPost("PostComprobanteComprasEmpresaCreada")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ViewEmpresaCreadaComprobanteCompraDTO>> PostComprobanteComprasEmpresaCreada([FromBody] EmpresaCreadaComprobanteCompraDTO empresaCreadaComprobanteCompraDTO)
        {
            try
            {
                var curentUser = HttpContext.User;
                var claims = curentUser.Claims.ToList();
                if (claims.Count == 0)
                    return BadRequest("No tiene permiso para esta seccion");
                if (empresaCreadaComprobanteCompraDTO.ClaveAcceso.Length == 49)
                {
                    var respuesta = cSriws.AutorizacionComprobanteoffline
                        (empresaCreadaComprobanteCompraDTO.ClaveAcceso);


                    if (respuesta.Comprobantes.Count == 0)
                        return BadRequest("No existe Factura en el SRI");

                    DatosXmlServices datosXmlServices = new DatosXmlServices();

                    string nombre = "";
                    var xmlComprobante = XDocument.Parse(respuesta.Comprobantes[0].Comprobante);
                    var DatosXml = datosXmlServices.DatosXml(xmlComprobante);
                    var TbInfotributaria = DatosXml.Tables[0];
                    var TbInfotributaria1 = DatosXml.Tables[1];
                    if (respuesta.Comprobantes[0].Comprobante.ToString().Contains("factura"))
                    {

                        SriRepositorio? sriRepositorio = null;
                        Proveedor? proveedor1 = null;
                        sriRepositorio = await context.SriRepositorios.FirstOrDefaultAsync(x => x.NumeroRuc.Equals(TbInfotributaria.Rows[0]["ruc"].ToString()));

                        if (sriRepositorio == null)
                        {

                            var getsriRepositorio = cConsultaSri.GetRucSri(TbInfotributaria.Rows[0]["ruc"].ToString());
                            if (getsriRepositorio == null)
                                return BadRequest($"No existe Contribuyente con el siguiente Ruc {TbInfotributaria.Rows[0]["ruc"].ToString()}");
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
                        proveedor1 = await context.Proveedores.FirstOrDefaultAsync(x => x.ProveedoresRuc.Equals(TbInfotributaria.Rows[0]["ruc"].ToString())
                           && x.FkEmpresa.Equals(Convert.ToInt32(claims[1].Value)) && x.FkEmpresasCreada.Equals(Convert.ToInt32(claims[2].Value)));
                        if (proveedor1 == null)
                        {

                            proveedor1 = new Proveedor
                            {
                                FkSriRepositorio = sriRepositorio.Id,
                                FkEmpresa = Convert.ToInt32(claims[1].Value),
                                FkEmpresasCreada = Convert.ToInt32(claims[2].Value),
                                ProveedoresRuc = sriRepositorio.NumeroRuc,
                                ProveedoresPropietario = sriRepositorio.RazonSocial,
                                ProveedoresEstado = true
                            };
                            await context.AddAsync(proveedor1);
                            await context.SaveChangesAsync();
                        }

                        var InfoFacturaCompra = await context.ComprobanteCompras.
                                         FirstOrDefaultAsync(x => x.ClaveAccesoXml.Equals(TbInfotributaria.Rows[0]["claveAccesoXML"].ToString()) && x.FkEmpresa.Equals(Convert.ToInt32(claims[1].Value)) && x.FkEmpresasCreada.Equals(Convert.ToInt32(claims[2].Value)));
                        if (InfoFacturaCompra != null)
                            return BadRequest(error: $"Factura ya ingresada:{InfoFacturaCompra.ClaveAccesoXml} ");


                        var TbInfoFactura = DatosXml.Tables[1];
                        var x = TbInfoFactura.Rows[0]["fechaEmision"].ToString().Split("/");
                        var fechaEmision = x[2] + "/" + x[1] + "/" + x[0];
                        var infoFactura = new ComprobanteCompra
                        {
                            FkEmpresa = Convert.ToInt32(claims[1].Value),
                            FkEmpresasCreada = Convert.ToInt32(claims[2].Value),
                            FkProvedor = proveedor1.Id,
                            FechaIngreso = currentTimePacific,
                            FechaEmision = Convert.ToDateTime(fechaEmision),
                            DirEstablecimiento = TbInfoFactura.Rows[0]["dirEstablecimiento"].ToString(),
                            ContribuyenteEspecial = TbInfoFactura.Rows[0]["contribuyenteEspecial"].ToString(),
                            ObligadoContabilidad = TbInfoFactura.Rows[0]["obligadoContabilidad"].ToString(),
                            TipoIdentificacionComprador = TbInfoFactura.Rows[0]["tipoIdentificacionComprador"].ToString(),
                            RazonSocialComprador = TbInfoFactura.Rows[0]["razonSocialComprador"].ToString(),
                            IdentificacionComprador = TbInfoFactura.Rows[0]["identificacionComprador"].ToString(),
                            TotalSinImpuestos = Convert.ToDecimal(TbInfoFactura.Rows[0]["totalSinImpuestos"].ToString()),
                            TotalDescuento = Convert.ToDecimal(TbInfoFactura.Rows[0]["totalDescuento"].ToString()),
                            Codigo = TbInfoFactura.Rows[0]["codigo"].ToString(),
                            CodigoPorcentaje = TbInfoFactura.Rows[0]["codigoPorcentaje"].ToString(),
                            BaseImponible = Convert.ToDecimal(TbInfoFactura.Rows[0]["baseImponible"].ToString()),
                            Valor = Convert.ToDecimal(TbInfoFactura.Rows[0]["valor"].ToString()),
                            Propina = Convert.ToDecimal(TbInfoFactura.Rows[0]["propina"].ToString()),
                            ImporteTotal = Convert.ToDecimal(TbInfoFactura.Rows[0]["importeTotal"].ToString()),
                            Moneda = TbInfoFactura.Rows[0]["moneda"].ToString(),
                            FormaPago = TbInfoFactura.Rows[0]["moneda"].ToString(),
                            Total = Convert.ToDecimal(TbInfoFactura.Rows[0]["total"].ToString()),
                            ClaveAccesoXml = TbInfotributaria.Rows[0]["claveAccesoXML"].ToString(),
                            NumeroFactura = TbInfotributaria.Rows[0]["estab"].ToString() + "-" + TbInfotributaria.Rows[0]["ptoEmi"].ToString() + "-" + TbInfotributaria.Rows[0]["secuencial"].ToString()

                        };

                        await context.AddAsync(infoFactura);
                        try
                        {
                            await context.SaveChangesAsync();
                        }
                        catch (Exception ex)
                        {

                            return BadRequest("Erroe en la  Factura Ingresada en el SRI" + ex.ToString());
                        }

                        DetallesFacturaCompra detalleVenta = null;
                        var TbDetalle = DatosXml.Tables[2];
                        for (int i = 0; i < TbDetalle.Rows.Count; i++)
                        {
                            string[] Scantidad;
                            int cantidad = 0;

                            if (TbDetalle.Rows[i]["cantidad"].ToString().Contains("."))
                            {
                                Scantidad = TbDetalle.Rows[i]["cantidad"].ToString().Split(".");
                                cantidad = Convert.ToInt32(Scantidad[0]);
                            }
                            else
                            {
                                cantidad = Convert.ToInt32(TbDetalle.Rows[i]["cantidad"].ToString());
                            }
                            detalleVenta = new DetallesFacturaCompra
                            {
                                FkFacturaCompra = infoFactura.Id,
                                CodigoPrincipal = TbDetalle.Rows[i]["codigoPrincipal"].ToString(),
                                Cantidad = cantidad,
                                Descripcion = TbDetalle.Rows[i]["descripcion"].ToString(),
                                PrecioUnitario = Convert.ToDecimal(TbDetalle.Rows[i]["precioUnitario"].ToString()),
                                Descuento = Convert.ToDecimal(TbDetalle.Rows[i]["descuento"].ToString()),
                                PrecioTotalSinImpuesto = Convert.ToDecimal(TbDetalle.Rows[i]["precioTotalSinImpuestos"].ToString())

                            };
                            await context.AddAsync(detalleVenta);
                            await context.SaveChangesAsync();

                        }

                        return Created("", new ViewEmpresaCreadaComprobanteCompraDTO
                        {
                            ClaveAccesoXml = infoFactura.ClaveAccesoXml,
                            NumeroFactura = infoFactura.NumeroFactura,
                            Proveedor = proveedor1.ProveedoresPropietario,
                            Ruc = proveedor1.ProveedoresRuc,
                        });
                    }
                    return BadRequest("Erroe en la  clavede Acceso Ingresada en el SRI No es una factura");
                }

                return BadRequest("Erroe en la  clavede Acceso Ingresada en el SRI");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.ToString());
            }

        }

        [HttpPost("PostComprobanteComprasTxtEmpresaCreada")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ViewEmpresaCreadaComprobanteCompraCreadosDTO>> PostComprobanteComprasTxtEmpresaCreada([FromForm] ArchivoComprobanteCompraTxtDto archivoComprobanteCompraTxtDto)
        {
            try
            {
                var curentUser = HttpContext.User;
                var claims = curentUser.Claims.ToList();
                if (claims.Count == 0)
                    return BadRequest("No tiene permiso para esta seccion");

                DataTable dtDatos = new DataTable();
                var fileStream = archivoComprobanteCompraTxtDto.pathArchivoTXT;
                DataSet dsXml = new DataSet();
                using (var reader = new StreamReader(fileStream.OpenReadStream()))
                {
                    var fileContent = reader.ReadToEnd();
                    var resultado = fileContent.Replace("	", ";");
                    var resultadoSplit = resultado.Split(';');
                    dtDatos.Columns.Add("docsri");
                    foreach (var item in resultadoSplit)
                    {
                        if (item.Length == 49)
                        {
                            if (char.IsNumber(item, 2))
                            {
                                dtDatos.Rows.Add(item);

                            }
                        }
                    }

                }
                if (dtDatos.Rows.Count >= 0)
                {
                    int contadorAutorizado = 0;

                    for (int i = 0; i < dtDatos.Rows.Count; i++)
                    {
                        if (dtDatos.Rows[i]["docsri"].ToString().Length == 49)
                        {
                            var respuesta = cSriws.AutorizacionComprobanteoffline(dtDatos.Rows[i]["docsri"].ToString());
                       
                       
                            
                            if (respuesta.Comprobantes.Count == 0)
                                return BadRequest("No existe Factura en el SRI");

                            DatosXmlServices datosXmlServices = new DatosXmlServices();

                            string nombre = "";
                            var xmlComprobante = XDocument.Parse(respuesta.Comprobantes[0].Comprobante);
                            var DatosXml = datosXmlServices.DatosXml(xmlComprobante);
                            var TbInfotributaria = DatosXml.Tables[0];
                            var TbInfotributaria1 = DatosXml.Tables[1];
                            if (respuesta.Comprobantes[0].Comprobante.ToString().Contains("factura"))
                            {
                                contadorAutorizado++;
                                SriRepositorio? sriRepositorio = null;
                                Proveedor? proveedor1 = null;
                                sriRepositorio = await context.SriRepositorios.FirstOrDefaultAsync(x => x.NumeroRuc.Equals(TbInfotributaria.Rows[0]["ruc"].ToString()));

                                if (sriRepositorio == null)
                                {

                                    var getsriRepositorio = cConsultaSri.GetRucSri(TbInfotributaria.Rows[0]["ruc"].ToString());
                                    if (getsriRepositorio == null)
                                        return BadRequest($"No existe Contribuyente con el siguiente Ruc {TbInfotributaria.Rows[0]["ruc"].ToString()}");
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
                                proveedor1 = await context.Proveedores.FirstOrDefaultAsync(x => x.ProveedoresRuc.Equals(TbInfotributaria.Rows[0]["ruc"].ToString())
                                   && x.FkEmpresa.Equals(Convert.ToInt32(claims[1].Value)) && x.FkEmpresasCreada.Equals(Convert.ToInt32(claims[2].Value)));
                                if (proveedor1 == null)
                                {

                                    proveedor1 = new Proveedor
                                    {
                                        FkSriRepositorio = sriRepositorio.Id,
                                        FkEmpresa = Convert.ToInt32(claims[1].Value),
                                        FkEmpresasCreada = Convert.ToInt32(claims[2].Value),
                                        ProveedoresRuc = sriRepositorio.NumeroRuc,
                                        ProveedoresPropietario = sriRepositorio.RazonSocial,
                                        ProveedoresEstado = true
                                    };
                                    await context.AddAsync(proveedor1);
                                    await context.SaveChangesAsync();
                                }

                                var InfoFacturaCompra = await context.ComprobanteCompras.
                                                 FirstOrDefaultAsync(x => x.ClaveAccesoXml.Equals(TbInfotributaria.Rows[0]["claveAccesoXML"].ToString()));
                                if (InfoFacturaCompra != null)
                                    return BadRequest(error: $"Factura ya ingresada:{InfoFacturaCompra.ClaveAccesoXml} ");


                                var TbInfoFactura = DatosXml.Tables[1];
                                var x = TbInfoFactura.Rows[0]["fechaEmision"].ToString().Split("/");
                                var fechaEmision = x[2] + "/" + x[1] + "/" + x[0];
                                var infoFactura = new ComprobanteCompra
                                {
                                    FkEmpresa = Convert.ToInt32(claims[1].Value),
                                    FkEmpresasCreada = Convert.ToInt32(claims[2].Value),
                                    FkProvedor = proveedor1.Id,
                                    FechaIngreso = currentTimePacific,
                                    FechaEmision = Convert.ToDateTime(fechaEmision),
                                    DirEstablecimiento = TbInfoFactura.Rows[0]["dirEstablecimiento"].ToString(),
                                    ContribuyenteEspecial = TbInfoFactura.Rows[0]["contribuyenteEspecial"].ToString(),
                                    ObligadoContabilidad = TbInfoFactura.Rows[0]["obligadoContabilidad"].ToString(),
                                    TipoIdentificacionComprador = TbInfoFactura.Rows[0]["tipoIdentificacionComprador"].ToString(),
                                    RazonSocialComprador = TbInfoFactura.Rows[0]["razonSocialComprador"].ToString(),
                                    IdentificacionComprador = TbInfoFactura.Rows[0]["identificacionComprador"].ToString(),
                                    TotalSinImpuestos = Convert.ToDecimal(TbInfoFactura.Rows[0]["totalSinImpuestos"].ToString()),
                                    TotalDescuento = Convert.ToDecimal(TbInfoFactura.Rows[0]["totalDescuento"].ToString()),
                                    Codigo = TbInfoFactura.Rows[0]["codigo"].ToString(),
                                    CodigoPorcentaje = TbInfoFactura.Rows[0]["codigoPorcentaje"].ToString(),
                                    BaseImponible = Convert.ToDecimal(TbInfoFactura.Rows[0]["baseImponible"].ToString()),
                                    Valor = Convert.ToDecimal(TbInfoFactura.Rows[0]["valor"].ToString()),
                                    Propina = Convert.ToDecimal(TbInfoFactura.Rows[0]["propina"].ToString()),
                                    ImporteTotal = Convert.ToDecimal(TbInfoFactura.Rows[0]["importeTotal"].ToString()),
                                    Moneda = TbInfoFactura.Rows[0]["moneda"].ToString(),
                                    FormaPago = TbInfoFactura.Rows[0]["moneda"].ToString(),
                                    Total = Convert.ToDecimal(TbInfoFactura.Rows[0]["total"].ToString()),
                                    ClaveAccesoXml = TbInfotributaria.Rows[0]["claveAccesoXML"].ToString(),
                                    NumeroFactura = TbInfotributaria.Rows[0]["estab"].ToString() + "-" + TbInfotributaria.Rows[0]["ptoEmi"].ToString() + "-" + TbInfotributaria.Rows[0]["secuencial"].ToString()

                                };

                                await context.AddAsync(infoFactura);
                                try
                                {
                                    await context.SaveChangesAsync();
                                }
                                catch (Exception ex)
                                {

                                    return BadRequest("Erroe en la  Factura Ingresada en el SRI" + ex.ToString());
                                }

                                DetallesFacturaCompra detalleVenta = null;
                                var TbDetalle = DatosXml.Tables[2];
                                for (int y = 0; y < TbDetalle.Rows.Count; y++)
                                {
                                    string[] Scantidad;
                                    int cantidad = 0;

                                    if (TbDetalle.Rows[y]["cantidad"].ToString().Contains("."))
                                    {
                                        Scantidad = TbDetalle.Rows[y]["cantidad"].ToString().Split(".");
                                        cantidad = Convert.ToInt32(Scantidad[0]);
                                    }
                                    else
                                    {
                                        cantidad = Convert.ToInt32(TbDetalle.Rows[y]["cantidad"].ToString());
                                    }
                                    detalleVenta = new DetallesFacturaCompra
                                    {
                                        FkFacturaCompra = infoFactura.Id,
                                        CodigoPrincipal = TbDetalle.Rows[y]["codigoPrincipal"].ToString(),
                                        Cantidad = cantidad,
                                        Descripcion = TbDetalle.Rows[y]["descripcion"].ToString(),
                                        PrecioUnitario = Convert.ToDecimal(TbDetalle.Rows[y]["precioUnitario"].ToString()),
                                        Descuento = Convert.ToDecimal(TbDetalle.Rows[y]["descuento"].ToString()),
                                        PrecioTotalSinImpuesto = Convert.ToDecimal(TbDetalle.Rows[y]["precioTotalSinImpuestos"].ToString())

                                    };
                                    await context.AddAsync(detalleVenta);
                                    await context.SaveChangesAsync();

                                }




                            }


                        }


                    }

                    return Created("", new ViewEmpresaCreadaComprobanteCompraCreadosDTO
                    {
                        NumeroDeFacturasCompraIngresadas = contadorAutorizado
                    });

                }

                return BadRequest("Error en el TXT en el SRI No es una factura");

            }
            catch (Exception ex)
            {


                return BadRequest(ex.ToString());
            }

        }

        [HttpGet("GetListComprobanteCompras")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ViewFacturaCompraDTo>>> GetListComprobanteCompras()
        {
            try
            {
                var curentUser = HttpContext.User;
                var claims = curentUser.Claims.ToList();
                if (claims.Count == 0)
                    return BadRequest("No tiene permiso para esta seccion");
                var ComprobanteComprass = await context.ComprobanteCompras.Where(x => x.FkEmpresa.Equals(Convert.ToInt32(claims[1].Value))
                && x.FkEmpresasCreada.Equals(Convert.ToInt32(claims[2].Value))).ToListAsync();
                if (ComprobanteComprass == null)
                    return BadRequest("No tiene Comprobante Compras");
                var ComprobanteCompras = mapper.Map<List<ComprobanteCompra>, List<ViewFacturaCompraDTo>>(ComprobanteComprass);

                return Ok(ComprobanteCompras);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.ToString());
            }

        }

        [HttpGet("GetComprobanteCompra")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ListViewEmpresaCreadaComprobanteDetalleCompraCreadosDTO>> GetComprobanteCompra([FromQuery] EmpresaCreadaComprobanteCompraIdDTO empresaCreadaComprobanteCompraId)
        {
            try
            {
                var curentUser = HttpContext.User;
                var claims = curentUser.Claims.ToList();
                if (claims.Count == 0)
                    return BadRequest("No tiene permiso para esta seccion");
                var ComprobanteComprass = await context.ComprobanteCompras.Include(x => x.TbDetallesFacturaCompras).FirstOrDefaultAsync
                    (x => x.FkEmpresa.Equals(Convert.ToInt32(claims[1].Value))
                && x.FkEmpresasCreada.Equals(Convert.ToInt32(claims[2].Value)) &&
                    x.Id.Equals(empresaCreadaComprobanteCompraId.Id));
                if (ComprobanteComprass == null)
                    return BadRequest("No existe Comprobante");
                var comprobanteCompraDetalle = mapper.Map<ListViewEmpresaCreadaComprobanteDetalleCompraCreadosDTO>(ComprobanteComprass);
                return comprobanteCompraDetalle;


            }
            catch (Exception ex)
            {

                return BadRequest(ex.ToString());


            }

        }


    }

}

