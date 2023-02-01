using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Facturacion.Context
{
    public partial class Local
    {
        public Local()
        {
            ComprobanteCompras = new HashSet<ComprobanteCompra>();
            ComprobanteVenta = new HashSet<ComprobanteVenta>();
            Productos = new HashSet<Producto>();
            Secuencials = new HashSet<Secuencial>();
            CajaGlobals = new HashSet<CajaGlobal>();
            Usuarios = new HashSet<Usuario>();
            Clientes = new HashSet<Cliente>();
        }

        public int Id { get; set; }
        public int? FkSriRepositorio { get; set; }
        [ForeignKey(nameof(FkSriRepositorio))]
        public virtual SriRepositorio SriRepositorioNavigation { get; set; }
        public int? FkEmpresa { get; set; }
        [ForeignKey(nameof(FkEmpresa))]
        public virtual Empresa FkEmpresaNavigation { get; set; }
        public int? FkEmpresasCreadas { get; set; }
        [ForeignKey(nameof(FkEmpresasCreadas))]
        public virtual EmpresasCreada FkEmpresasCreadaNavigation { get; set; }
    
        public string LocalNombre { get; set; }
        public string LocalTelefono { get; set; }
        public string LocalDireccion { get; set; }
        public string LocalActividad { get; set; }
        public DateTime LocalFechainicioactividad { get; set; }
        public string LocalEstado { get; set; }
        public string LocalNumero { get; set; }

        public virtual NetUserAditional FkNetUserNavigation { get; set; }
        public virtual PermisosEmpresa FkPermisosEmpresaNavigation { get; set; }

        public virtual ICollection<Cliente> Clientes { get; set; }
        public virtual ICollection<ComprobanteCompra> ComprobanteCompras { get; set; }
        public virtual ICollection<ComprobanteVenta> ComprobanteVenta { get; set; }
        public virtual ICollection<Producto> Productos { get; set; }
        public virtual ICollection<Secuencial> Secuencials { get; set; }
        public virtual ICollection<CajaGlobal> CajaGlobals { get; set; }
        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
