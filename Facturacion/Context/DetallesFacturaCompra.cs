using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Facturacion.Context
{
    public partial class DetallesFacturaCompra
    {
        public int Id { get; set; }
        public int FkFacturaCompra { get; set; }
        [ForeignKey(nameof(FkFacturaCompra))]
        public virtual ComprobanteCompra FkFacturaCompraNavigation { get; set; }
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
}
