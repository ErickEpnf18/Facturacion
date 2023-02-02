using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Facturacion.DTOs
{
    public class ViewEmpresaCreadaComprobanteCompraDTO
    {
        public string? ClaveAccesoXml { get; set; }
        public string? NumeroFactura { get; set; }
        public string? Proveedor { get; set; }
        public string? Ruc { get; set; }
    }
    public class ViewEmpresaCreadaComprobanteCompraCreadosDTO
    {
        public int  NumeroDeFacturasCompraIngresadas { get; set; }
        
    }

    public class ListViewEmpresaCreadaComprobanteDetalleCompraCreadosDTO
    {
        public DateTime FechaIngreso { get; set; }
        public DateTime FechaEmision { get; set; }
        public string DirEstablecimiento { get; set; }
        public string FormaPago { get; set; }
        public decimal Total { get; set; }
        public decimal BaseImponible { get; set; }
        public string ClaveAccesoXml { get; set; }
        public string NumeroFactura{ get; set; }
        public int Id { get; set; }
        public List<DetalleFacturaCompraDTo> DetalleFacturaCompras { get; set; }
    }

    public class ViewFacturaCompraDTo
    {
        public DateTime FechaIngreso { get; set; }
        public DateTime FechaEmision { get; set; }
        public string DirEstablecimiento { get; set; }
        public string FormaPago { get; set; }
        public decimal Total { get; set; }
        public decimal BaseImponible { get; set; }
        public string ClaveAccesoXml { get; set; }
        public string NumeroFactura { get; set; }
        public int Id { get; set; }
    }



    public class DetalleFacturaCompraDTo
    {
        public int Id { get; set; }
        public int FkFacturaCompra { get; set; }
        public string CodigoPrincipal { get; set; }
        public string Descripcion { get; set; }
        public int Cantidad { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioUnitario { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Descuento { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioTotalSinImpuesto { get; set; }
    }

    public class EmpresaCreadaComprobanteCompraDTO
    {
        [Required(ErrorMessage = "Campo requerido {0}")]
        [StringLength(maximumLength: 49, MinimumLength = 49)]
        [RegularExpression("(^[0-9]+$)", ErrorMessage = "Solo se permiten números")]
        public string ClaveAcceso { get; set; }
    }
    public class ArchivoComprobanteCompraTxtDto
    {
        [Required(ErrorMessage = "Campo requerido {0}")]
        public IFormFile pathArchivoTXT { get; set; }
    }
    public class EmpresaCreadaComprobanteCompraIdDTO
    {
        [Required(ErrorMessage = "Campo requerido {0}")]
        public int Id { get; set; }
    }

}
