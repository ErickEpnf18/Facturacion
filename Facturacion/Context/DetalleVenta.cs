using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Facturacion.Context
{
    public partial class DetalleVenta
    {
     

        public int Id { get; set; }

        public int FkProducto { get; set; }
        [ForeignKey(nameof(FkProducto))]
        public virtual Producto FkProductoNavigation { get; set; }
        public int FkComprobanteVenta { get; set; }
        [ForeignKey(nameof(FkComprobanteVenta))]
        public virtual ComprobanteVenta ComprobanteVentaNavigation { get; set; }
        public int DetallevCantidad { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? DetallevValor { get; set; }
        public string DetallevEstado { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal DetallevDescuento { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal DetallevTotal { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? DetalleTotalIva { get; set; }
 
    }
}
