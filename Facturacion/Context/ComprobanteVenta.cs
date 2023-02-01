using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Facturacion.Context
{
    public partial class ComprobanteVenta
    {
        public ComprobanteVenta()
        {
            Tbdocumentosfacturacionelectronicas = new HashSet<ErroresFacturasElectronicas>();
            TbrutasXmls = new HashSet<RutasXml>();
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
        public int? FkSecuencial { get; set; }
        [ForeignKey(nameof(FkSecuencial))]
        public virtual Secuencial FkSecuencialNavigation { get; set; }
        public int? FkCliente { get; set; }
        [ForeignKey(nameof(FkCliente))]

        public virtual Cliente FkClienteNavigation { get; set; }
        public int? FkTipoComprobante { get; set; }
        [ForeignKey(nameof(FkTipoComprobante))]

        public virtual TipoComprobante FkTipoComprobanteNavigation { get; set; }
        public string ComprobanteFormapago { get; set; }
        public DateTime ComprobantevFecha { get; set; }

        public string ComprobanteNumero { get; set; }
  
        public string ComprobanteEstado { get; set; } = "ACT";
        [Column(TypeName = "decimal(18,2)")]
        public decimal ComprobanteSubtotal0 { get; set; } = 0.00m;
        [Column(TypeName = "decimal(18,2)")]
        public decimal ComprobanteSubtotal12 { get; set; } = 0.00m;
        [Column(TypeName = "decimal(18,2)")]
        public decimal ComprobanteDescuento { get; set; } = 0.00m;
        [Column(TypeName = "decimal(18,2)")]
        public decimal ComprobanteSubtotal { get; set; } = 0.00m;
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ComprobanteIvatotal { get; set; } = 0.00m;
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ComprobantevTotal { get; set; } = 0.00m;
        public string Docsri { get; set; }


        public virtual ICollection<ErroresFacturasElectronicas> Tbdocumentosfacturacionelectronicas { get; set; }
        public virtual ICollection<RutasXml> TbrutasXmls { get; set; }
        public virtual ICollection<DetalleVenta> DetalleVenta { get; set; }
    }
}
