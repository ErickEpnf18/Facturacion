using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Facturacion.Context
{
    public partial class ComprobanteCompra
    {
        public ComprobanteCompra()
        {
            TbDetallesFacturaCompras = new HashSet<DetallesFacturaCompra>();
        }

        public int Id { get; set; }

        public int? FkEmpresa { get; set; }
        [ForeignKey(nameof(FkEmpresa))]
        public virtual Empresa FkEmpresaNavigation { get; set; }
        public int? FkEmpresasCreada { get; set; }
        [ForeignKey(nameof(FkEmpresasCreada))]
        public virtual EmpresasCreada FkEmpresasCreadaNavigation { get; set; }
        public int? FkLocal { get; set; }
        [ForeignKey(nameof(FkLocal))]
        public virtual Local LocalNavigation { get; set; }
        public int? FkUsuario { get; set; }
        [ForeignKey(nameof(FkEmpresa))]
        public virtual Usuario Usuario { get; set; }

        public int FkProvedor { get; set; }
        [ForeignKey(nameof(FkProvedor))]
        public virtual Proveedor FkProvedorNavigation { get; set; }


        public DateTime FechaIngreso { get; set; }
        public DateTime FechaEmision { get; set; }
        public string DirEstablecimiento { get; set; }
        public string ContribuyenteEspecial { get; set; }
        public string ObligadoContabilidad { get; set; }
        public string TipoIdentificacionComprador { get; set; }
        public string RazonSocialComprador { get; set; }
        public string IdentificacionComprador { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalSinImpuestos { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalDescuento { get; set; }
        public string Codigo { get; set; }
        public string CodigoPorcentaje { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal BaseImponible { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Valor { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Propina { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal ImporteTotal { get; set; }
        public string Moneda { get; set; }
        public string FormaPago { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }
        public string ClaveAccesoXml { get; set; }
        public string NumeroFactura { get; set; }


        public virtual ICollection<DetallesFacturaCompra> TbDetallesFacturaCompras { get; set; }
    }
}
