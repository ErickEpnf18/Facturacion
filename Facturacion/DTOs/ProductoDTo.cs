using System.ComponentModel.DataAnnotations;

namespace Facturacion.DTOs
{
    public class ProductoIdDTo
    {
        [Required(ErrorMessage = "Campo requerido {0}")]
        public int Id { get; set; }

    }

    public class ProductoDTo
    {
        [Required(ErrorMessage = "Campo requerido {0}")]
        public bool EsProducto { get; set; }
        public string Sku { get; set; }
        [Required(ErrorMessage = "Campo requerido {0}")]
        public string ProductoCodigo { get; set; }
        public string ProductoDescripcion { get; set; }
        [Required(ErrorMessage = "Campo requerido {0}")]
        public decimal ProductoValor { get; set; }
        [Required(ErrorMessage = "Campo requerido {0}")]
        public int ProductoCantidad { get; set; }
        [Required(ErrorMessage = "Campo requerido {0}")]
        public bool ProductoConIva { get; set; }
    }
    public class ProductoUpdateDTo: ProductoDTo
    {
        [Required(ErrorMessage = "Campo requerido {0}")]
        public int Id { get; set; }

    }
    public class ViewProductoDto
    {
        public int Id { get; set; }
        public DateTime FechaRegistroProducto { get; set; }
        public bool? EsProducto { get; set; }
        public string? Sku { get; set; }
        public string? ProductoCodigo { get; set; }
        public string? ProductoDescripcion { get; set; }
        public string? ProductoEstado { get; set; }
        public decimal? ProductoValor { get; set; }
        public int? ProductoCantidad { get; set; }
        public bool? ProductoConIva { get; set; }
    }

  
}
