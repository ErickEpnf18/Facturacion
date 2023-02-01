using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Facturacion.Context
{
    public partial class Secuencial
    {
        public Secuencial()
        {
            ComprobanteVenta = new HashSet<ComprobanteVenta>();
        }

        public int Id { get; set; }
        public int FkTipoCompronbante { get; set; }
        [ForeignKey(nameof(FkTipoCompronbante))]
        public TipoComprobante TipoComprobanteNavigation { get; set; }

        public int? FkEmpresa { get; set; }
        [ForeignKey(nameof(FkEmpresa))]
        public virtual Empresa FkEmpresaNavigation { get; set; }
        public int? FkEmpresasCreada { get; set; }
        [ForeignKey(nameof(FkEmpresasCreada))]
        public virtual EmpresasCreada EmpresasCreadaNavigation { get; set; }
        public int? FkLocal { get; set; }
        [ForeignKey(nameof(FkLocal))]
        public virtual Local LocalNavigation { get; set; }

        public string Numestablecimiento { get; set; }
        public string Numpuntoemision { get; set; }
        public int Numcorrelativo { get; set; }
        public bool Estado { get; set; }



        public virtual ICollection<ComprobanteVenta> ComprobanteVenta { get; set; }
    }
}
