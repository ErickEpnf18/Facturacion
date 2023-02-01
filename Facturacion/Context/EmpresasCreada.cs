using System.ComponentModel.DataAnnotations.Schema;

namespace Facturacion.Context
{
    public class EmpresasCreada
    {
        public EmpresasCreada()
        {
            ComprobanteCompras = new HashSet<ComprobanteCompra>();
            ComprobanteVenta = new HashSet<ComprobanteVenta>();
            Locals = new HashSet<Local>();
            Productos = new HashSet<Producto>();
            Secuencials = new HashSet<Secuencial>();
            CajaGlobals = new HashSet<CajaGlobal>(); ;
            Usuarios = new HashSet<Usuario>(); 
            Clientes = new HashSet<Cliente>();


        }
        public int Id { get; set; }
        public int FkEmpresa { get; set; }
        [ForeignKey(nameof(FkEmpresa))]
        public virtual Empresa FkEmpresaNavigation { get; set; }
        public int? FkSriRepositorio { get; set; }
        [ForeignKey(nameof(FkSriRepositorio))]
        public virtual SriRepositorio SriRepositorioNavigation { get; set; }

        public string EmpresasCreadaRuc { get; set; }
        public string EmpresasCreadaPropietario { get; set; }
        public string EmpresasCreadaEmail { get; set; }
        public bool? EmpresasCreadaEstado { get; set; }
        public bool? EmpresasCreadapruebaproduccion { get; set; }
        public string? EmpresasCreadaUbicacionarchivop12 { get; set; }
        public string? EmpresasCreadaContrasenaArchivop12 { get; set; }
        public string? EmpresasCreadaImagen { get; set; }
        public string? EmpresasCreadaDireccion { get; set; }
        public string? EmpresasCreadaTelefono { get; set; }
        public string? EmpresasCreadaObligadoContabiliadad { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal EmpresasPorcentajeIva { get; set; }
        public virtual NetUserAditional FkNetUserNavigation { get; set; }
        public virtual PermisosEmpresa FkPermisosEmpresaNavigation { get; set; }
        public virtual CorreoEmpresa FkCorreoNavigation { get; set; }
        public virtual ICollection<Usuario> Usuarios { get; set; }
        public virtual ICollection<Local> Locals { get; set; }

        public virtual ICollection<Cliente> Clientes { get; set; }
        public virtual ICollection<ComprobanteCompra> ComprobanteCompras { get; set; }
        public virtual ICollection<ComprobanteVenta> ComprobanteVenta { get; set; }

        public virtual ICollection<Producto> Productos { get; set; }
        public virtual ICollection<Secuencial> Secuencials { get; set; }
        public virtual ICollection<CajaGlobal> CajaGlobals { get; set; }
       




    }
}
