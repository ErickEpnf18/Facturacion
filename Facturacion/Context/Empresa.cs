using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Facturacion.Context
{
    public partial class Empresa
    {
        public Empresa()
        {
            ComprobanteCompras = new HashSet<ComprobanteCompra>();
            ComprobanteVenta = new HashSet<ComprobanteVenta>();
            Locals = new HashSet<Local>();
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
        public string EmpresaRuc { get; set; }
        public string EmpresaPropietario { get; set; }
        public string EmpresaEmail { get; set; }
        public bool? EmpresaEstado { get; set; }
        public string EmpresaContrasena { get; set; }
        public string EmpresaImagen { get; set; }
        public string EmpresaTelefono { get; set; }
        public string EmpresaUsuarioCreador { get; set; }




        public virtual NetUserAditional FkNetUserNavigation { get; set; }
        public virtual PermisosEmpresa FkPermisosEmpresaNavigation { get; set; }
        public virtual CorreoEmpresa FkCorreoNavigation { get; set; }
        public virtual ICollection<Cliente> Clientes { get; set; }
        public virtual ICollection<EmpresasCreada> EmpresasCreadas { get; set; }
        public virtual ICollection<ComprobanteCompra> ComprobanteCompras { get; set; }
        public virtual ICollection<ComprobanteVenta> ComprobanteVenta { get; set; }
        public virtual ICollection<Local> Locals { get; set; }
        public virtual ICollection<Producto> Productos { get; set; }
        public virtual ICollection<Secuencial> Secuencials { get; set; }
        public virtual ICollection<CajaGlobal> CajaGlobals { get; set; }
        public virtual ICollection<Usuario> Usuarios { get; set; }
       
    }
}
