using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Facturacion.Context;

#nullable disable

namespace Facturacion.Context
{
    public partial class AplicationDbContext : IdentityDbContext<NetUserAditional>
    {

        public AplicationDbContext(DbContextOptions<AplicationDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<RespositorioCedulas> RespositorioCedulas { get; set; }
        public virtual DbSet<SriRepositorio> SriRepositorios { get; set; }
        public virtual DbSet<PermisosEmpresa> PermisosEmpresas { get; set; }
        public virtual DbSet<Empresa> Empresas { get; set; }
        public virtual DbSet<EmpresasCreada> EmpresasCreadas { get; set; }
        public virtual DbSet<CorreoEmpresa> CorreoEmpresas { get; set; }
        public virtual DbSet<Secuencial> Secuencials { get; set; }
        public virtual DbSet<TipoComprobante> Comprobantes { get; set; }
        public virtual DbSet<Local> Locals { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<FormasPago> FormasPagos { get; set; }
        public virtual DbSet<CajaGlobal> CajaGlobals { get; set; }
        public virtual DbSet<Cliente> Clientes { get; set; }
        public virtual DbSet<ComprobanteCompra> ComprobanteCompras { get; set; }
        public virtual DbSet<ComprobanteVenta> ComprobanteVentas { get; set; }
        public virtual DbSet<DetallesFacturaCompra> DetallesFacturaCompras { get; set; }
        public virtual DbSet<DetalleVenta> DetalleVentas { get; set; }
        public virtual DbSet<ErroresFacturasElectronicas> ErroresFacturasElectronicas { get; set; }
        public virtual DbSet<Producto> Productos { get; set; }
        public virtual DbSet<Proveedor> Proveedores { get; set; }
        public virtual DbSet<RutasXml> RutasXmls { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseSqlServer("Data Source=SQL8002.site4now.net;Initial Catalog=db_a87104_synergypower;User Id=db_a87104_synergypower_admin; Password=Klave.123; ");
                //optionsBuilder.UseSqlServer("Data Source=SQL8002.site4now.net;Initial Catalog=db_a88d85_facturaec;User Id=db_a88d85_facturaec_admin;Password=NELSON1887;");
                //optionsBuilder.UseSqlServer("Data Source=SQL8003.site4now.net;Initial Catalog=db_a89665_idealesfe;User Id=db_a89665_idealesfe_admin;Password=TEFA5860;");
                optionsBuilder.UseSqlServer("Data Source=SQL8004.site4now.net;Initial Catalog=db_a8ae41_nessoftfact;User Id=db_a8ae41_nessoftfact_admin;Password=Siara120424;");

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");
            OnModelCreatingPartial(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);



    }
    public class NetUserAditional : IdentityUser
    {
        public DateTime FechaDeRegistro { get; set; }
        public string NumeroparaConfirmacion { get; set; }
        public int? FkEmpresa { get; set; }
        [ForeignKey(nameof(FkEmpresa))]
        public virtual Empresa Empresa { get; set; }

        public int? FkEmpresaCreada { get; set; }
        [ForeignKey(nameof(FkEmpresa))]
        public virtual EmpresasCreada EmpresasCreadas { get; set; }

        public int? FkLocal { get; set; }
        [ForeignKey(nameof(FkEmpresa))]
        public virtual Local Local { get; set; }

        public int? FkUsuario { get; set; }
        [ForeignKey(nameof(FkEmpresa))]
        public virtual Usuario Usuario { get; set; }

        public string? FkNetUserid { get; set; }



    }
}
