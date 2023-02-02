using System.ComponentModel.DataAnnotations.Schema;

namespace Facturacion.DTOs
{
    public class PostComprobanteVentaECDto
    {
        public int? FkSecuencial { get; set; }
        public int? FkCliente { get; set; }
        public List<PostDetalleComprobanteVentaDto> detalleComprobanteVentaDtos { get; set; }
        public List<IdFormasPagoDto> iListFormasPagoDtos { get; set; }

    }
    public class PostDetalleComprobanteVentaDto
    {
        public int? FkProducto { get; set; }
        public int? DetallevCantidad { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? DetallevDescuento { get; set; }


    }
    public class ViewComprobanteVentaECDto
    {

        public int Id { get; set; }
        public int FkCliente { get; set; }
        public string Mensaje { get; set; }

    }

    public class ComprobanteVentaECDto
    {

        public int Id { get; set; }
        public string CedulaRuc { get; set; }
        public string Nombre { get; set; }
        public DateTime ComprobantevFecha { get; set; }
        public string ComprobanteNumero { get; set; }
        public string ComprobanteFormapago { get; set; }
        public string ComprobanteEstado { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal ComprobanteSubtotal0 { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal ComprobanteSubtotal12 { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal ComprobanteDescuento { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal ComprobanteSubtotal { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal ComprobanteIvatotal { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal ComprobantevTotal { get; set; }
        public string Docsri { get; set; }
        public ClieteComprobanteventaDto ClieteComprobanteventaDto { get; set; }
    }
    public class ClieteComprobanteventaDto
    {
        public string Identificacion { get; set; }
        public string Razonsocial { get; set; }
        public string Direccion { get; set; }
        public string Email { get; set; }
        public string NumeroTelefon { get; set; }
        public string Estado { get; set; }
    }
    public class IdComprobanteVentaDto
    {
        public int IdComprobanteventa { get; set; }
    }

    public class ComprobanteVentaCompletoECDto : ComprobanteVentaECDto
    {
        public ClieteComprobanteventaDto clientecomprobanteventaDto { get; set; }

        public List<DetalleComprobanteVentaDto> detalleComprobanteVentasDto { get; set; }
    }

    public class DetalleComprobanteVentaDto
    {
        public int DetallevCantidad { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal DetallevValor { get; set; }
        public string DetallevEstado { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal DetallevDescuento { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal DetallevTotal { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal DetallevDescuentoporc { get; set; }
        public string SkuProducto { get; set; }
        public string ProductoDescripcion { get; set; }
        public bool ProductoConIva { get; set; }
        public ProductoComprobanteVentaDto ProductoVentaDTo { get; set; }

    }
    public class ProductoComprobanteVentaDto
    {
        public bool EsProducto { get; set; }
        public string Sku { get; set; }
        public string ProductoCodigo { get; set; }
        public string ProductoDescripcion { get; set; }
        public bool ProductoEstado { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal ProductoValor { get; set; }
        public int ProductoCantidad { get; set; }
        public bool ProductoConIva { get; set; }

    }


}
