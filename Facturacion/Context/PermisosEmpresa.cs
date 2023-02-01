using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Facturacion.Context
{
    public class PermisosEmpresa
    {
        public int Id { get; set; }
        public int? FkEmpresa { get; set; }
        [ForeignKey(nameof(FkEmpresa))]
        public virtual Empresa FkEmpresaNavigation { get; set; }
        public int? FkEmpresasCreadas { get; set; }
        [ForeignKey(nameof(FkEmpresasCreadas))]
        public virtual EmpresasCreada FkEmpresasCreadasNavigation { get; set; }
        public int? FkLocal { get; set; }
        [ForeignKey(nameof(FkLocal))]
        public virtual Local FkLocalNavigation { get; set; }
        public int NumeroFacturas { get; set; }
        [DisplayFormat(DataFormatString ="{0:dd/MM/yyyy}",ApplyFormatInEditMode =true)]
        public DateTime InicioDeActividades { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FinDeActividades { get; set; }
        public bool BloquiarEmpresa { get; set; }
        public bool IngresoEmpresa { get; set; }
        public bool IngresoPermisoEmpresa { get; set; }
        public bool IngresoFacturasVentas { get; set; }
        public bool IngresoFacturacionElectronica { get; set; }
        public bool IngresoCrearClientes { get; set; }
        public bool IngresoFacturasCompras { get; set; }
        public bool IngresoProveedores { get; set; }

    }
}
