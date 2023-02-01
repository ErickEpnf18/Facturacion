using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Facturacion.Context
{
    public partial class CajaGlobal
    {
        public int Id { get; set; }
        public int? FkEmpresa { get; set; }
        [ForeignKey(nameof(FkEmpresa))]
        public virtual Empresa FkEmpresaNavigation { get; set; }
        public int? FkLocal { get; set; }
        [ForeignKey(nameof(FkLocal))]
        public virtual Local FkLocalNavigation { get; set; }
        public int? EmpresasCreadas { get; set; }
        [ForeignKey(nameof(EmpresasCreadas))]
        public virtual EmpresasCreada EmpresasCreadasNavigation { get; set; }
        public DateTime? FechaIngreso { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? CajaGlobalEfectivoTotal { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? CajaGlobalTarjetasDebitoTotal { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? CajaGlobalTrasferenciaTotal { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? CajaGlobalCreditoTotal { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? CajaGlobalTotalValores { get; set; }

    }
}
