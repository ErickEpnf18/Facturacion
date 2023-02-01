using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Facturacion.Context
{
    public partial class Proveedor
    {
        public int Id { get; set; }
        public int? FkSriRepositorio { get; set; }
        [ForeignKey(nameof(FkSriRepositorio))]
        public virtual SriRepositorio SriRepositorioNavigation  { get; set; }
        public int? FkEmpresa { get; set; }
        [ForeignKey(nameof(FkEmpresa))]
        public virtual Empresa FkEmpresaNavigation { get; set; }
        public int? FkEmpresasCreada { get; set; }
        [ForeignKey(nameof(FkEmpresasCreada))]
        public virtual EmpresasCreada EmpresasCreadaNavigation { get; set; }
        public int? FkLocal { get; set; }
        [ForeignKey(nameof(FkLocal))]
        public virtual Local LocalNavigation { get; set; }

        public string ProveedoresRuc { get; set; }
        public string ProveedoresPropietario { get; set; }
        public string ProveedoresEmail { get; set; }
        public bool? ProveedoresEstado { get; set; }

        public virtual ICollection<ComprobanteCompra> ComprobanteCompras { get; set; }

    }
}
