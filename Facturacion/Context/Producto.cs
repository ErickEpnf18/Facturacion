using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Facturacion.Context
{
    public partial class Producto
    {
        public Producto()
        {
            DetalleVenta = new HashSet<DetalleVenta>();
        }

        public int Id { get; set; }
        public int? FkEmpresa { get; set; }
        [ForeignKey(nameof(FkEmpresa))]
        public virtual Empresa FkEmpresaNavigation { get; set; }
        public int? FkEmpresasCreada { get; set; }
        [ForeignKey(nameof(FkEmpresasCreada))]
        public virtual EmpresasCreada EmpresasCreadaNavigation { get; set; }
        public int? FkLocal { get; set; }
        [ForeignKey(nameof(FkLocal))]
        public virtual Local LocalNavigation { get; set; }
        public int? FkUsuario { get; set; }
        [ForeignKey(nameof(FkUsuario))]
        public virtual Usuario Usuario { get; set; }
        public DateTime FechaRegistroProducto { get; set; }
        public bool? EsProducto { get; set; }
        public string? Sku { get; set; }
        public string? ProductoCodigo { get; set; }
        public string? ProductoDescripcion { get; set; }
        public bool? ProductoEstado { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ProductoValor { get; set; }
        public int? ProductoCantidad { get; set; }
        public bool? ProductoConIva { get; set; }
 

        public virtual ICollection<DetalleVenta> DetalleVenta { get; set; }
    }
}
