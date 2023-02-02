using System.ComponentModel.DataAnnotations;

namespace Facturacion.DTOs
{

    public class PermisosEmpresaCreadaDTo
    {
        [Required(ErrorMessage = "Campo requerido {0}")]
        public int IdEmpresaCreada { get; set; }

        [Required(ErrorMessage = "Campo requerido {0}")]
        public int NumeroFacturas { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime InicioDeActividades { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FinDeActividades { get; set; }
        public bool BloquiarEmpresa { get; set; }
        public bool IngresoFacturasVentas { get; set; }
        public bool IngresoFacturacionElectronica { get; set; }
        public bool IngresoCrearClientes { get; set; }
        public bool IngresoFacturasCompras { get; set; }
        public bool IngresoEmpresa { get; set; }
        public bool IngresoProveedores { get; set; }
    }


    public class PermisoEmpresaDto : PermisosEmpresaCreadaDTo
    {


    }

    public class ViewPermisosEmpresaCreadaDTo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo requerido {0}")]
        public int NumeroFacturas { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime InicioDeActividades { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public int? FkEmpresasCreadas { get; set; }
        public DateTime FinDeActividades { get; set; }
        public bool BloquiarEmpresa { get; set; }
        public bool IngresoEmpresa { get; set; }
        public bool IngresoPermisoEmpresa { get; set; }
        public bool IngresoFacturasVentas { get; set; }
        public bool IngresoFacturacionElectronica { get; set; }
        public bool IngresoCrearClientes { get; set; }
        public bool IngresoFacturasCompras { get; set; }
        public bool IngresoProveedores { get; set; }
        public EmpreesaCreadaDTo empreesaCreadaDTo { get; set; }

    }
    public class EmpreesaCreadaDTo
    {
        public string EmpresasCreadaRuc { get; set; }
        public string EmpresasCreadaPropietario { get; set; }

    }



    public class ViewPermisosEmpresaDTo : ViewPermisosEmpresaCreadaDTo
    {


    }

    public class CorreoEmpresaIdDto
    {
        [Required(ErrorMessage = "Campo requerido {0}")]
        [RegularExpression("(^[0-9]+$)", ErrorMessage = "Solo se permiten números")]
        public int IdCorreo { get; set; }
        public int fkempresa { get; set; }
    }


}
