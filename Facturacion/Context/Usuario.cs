using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Facturacion.Context
{
    public partial class Usuario
    {
        public Usuario()
        {
            ComprobanteCompras = new HashSet<ComprobanteCompra>();
            ComprobanteVenta = new HashSet<ComprobanteVenta>();
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
        public DateTime FechaIngreso { get; set; }
        public string UsuarioNombre { get; set; }
        public string Email { get; set; }
        public string UsuarioTipo { get; set; }
        public bool UsuarioEstado { get; set; }
        public bool IngresoFacturasVentas { get; set; }
        public bool IngresoFacturacionElectronica { get; set; }
        public bool IngresoCrearClientes { get; set; }
        public bool IngresoFacturasCompras { get; set; }
        public bool IngresoEmpresa { get; set; }
        public bool IngresoProveedores { get; set; }

        public virtual NetUserAditional FkNetUserNavigation { get; set; }

        public virtual ICollection<ComprobanteCompra> ComprobanteCompras { get; set; }
        public virtual ICollection<ComprobanteVenta> ComprobanteVenta { get; set; }
    }
}
