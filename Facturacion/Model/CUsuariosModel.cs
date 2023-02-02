
using Facturacion.DTOs;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Facturacion.Model
{
    public class LoginUsario
    {
        [Required(ErrorMessage = "Campo requerido {0}")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Campo requerido {0}")]
        public string Password { get; set; }


    }
    public class RucDto
    {
        [Required(ErrorMessage = "Campo requerido {0}")]
        [StringLength(maximumLength: 13, MinimumLength = 13)]
        [RegularExpression("(^[0-9]+$)", ErrorMessage = "Solo se permiten números")]

        public string Ruc { get; set; }


    }
    public class EmpresaCreadaIdDto
    {
        [Required(ErrorMessage = "Campo requerido {0}")]
        [RegularExpression("(^[0-9]+$)", ErrorMessage = "Solo se permiten números")]
        public int IdEmpresaCreada { get; set; }
    }
 
    public class CorreoEmpresaCreadaIdDto : EmpresaCreadaIdDto
    {
        [Required(ErrorMessage = "Campo requerido {0}")]
        [RegularExpression("(^[0-9]+$)", ErrorMessage = "Solo se permiten números")]
        public int IdCorreo { get; set; }
    }


    public class ExisteSede
    {
        public string Error { get; set; }
        public string IdSede { get; set; }

    }
    public class CrearUsuarioAdministradorModel
    {

        [Required(ErrorMessage = "Campo {0} es necesesario")]
        [StringLength(maximumLength: 13, MinimumLength = 13)]
        public string Ruc { get; set; }

        [Required(ErrorMessage = "Campo Telefono es necesesario")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "Campo {0} es necesesario")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Campo {0} es necesesario")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Campo {0} es necesesario")]
        public string UserName { get; set; }


    }
    public class PostEmpresaCreada : CrearUsuarioAdministradorModel
    {

    }
    public class PostEmpresa : CrearUsuarioAdministradorModel
    {

    }
    public class PutEmpresaDto
    {
        public int Id { get; set; }

        public string EmpresaEmail { get; set; }
        public bool? EmpresaEstado { get; set; }
        public IFormFile EmpresaImagen { get; set; }
        public string EmpresaContrasena { get; set; }
        public string EmpresaTelefono { get; set; }

    }
    public class ViewEmpresaCreada : ViewCrearUsuarioAdministradorModel
    {

        public string EmpresaPropietario { get; set; }
        public string Telefono { get; set; }


    }
    public class ViewEmpresaCreadaInformacionGeneralDTO
    {
        public int Id { get; set; }
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

        public ViewPermisosEmpresaCreadaDTo? PermisosEmpresaCreadadto { get; set; }
        public CorreoEmpresaCreadaDto? CorreoEmpresaCreadadto { get; set; }
        public ICollection<UsuarioEmpresaCreadaDTo>? UsuarioEmpresaCreadadDTos { get; set; }
        public ICollection<LocalEmpresaCreadaDto>? LocalEmpresaCreadadDtos { get; set; }


    }
    public class PermisosEmpresaCreadaDto
    {
        public int Id { get; set; }
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
    public class CorreoEmpresaCreadaDto
    {
        public int IdEmpresaCreada { get; set; }
        public int Port { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool EnableSsl { get; set; }
        public string Host { get; set; }

    }


    public class UpdateCorreoEmpresaCreadaDto : CorreoEmpresaCreadaDto
    {
        public int IdCorreo { get; set; }

    }
    public class UsuarioEmpresaCreadaDTo
    {
        public int Id { get; set; }
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

    }
    public class LocalEmpresaCreadaDto
    {
        public int Id { get; set; }
        public string LocalNombre { get; set; }
        public string LocalTelefono { get; set; }
        public string LocalDireccion { get; set; }
        public string LocalActividad { get; set; }
        public DateTime LocalFechainicioactividad { get; set; }
        public string LocalEstado { get; set; }
        public string LocalNumero { get; set; }


    }


    public class ViewEmpresa
    {
        public int Id { get; set; }
        public string EmpresaRuc { get; set; }
        public string EmpresaPropietario { get; set; }
        public string EmpresaEmail { get; set; }
        public bool? EmpresaEstado { get; set; }
        public string EmpresaContrasena { get; set; }
        public string EmpresaImagen { get; set; }
        public string EmpresaTelefono { get; set; }
        public string EmpresaUsuarioCreador { get; set; }
        public string Mensaje { get; set; }

    }
    public class ViewPermisosEmpresaCreada
    {
        public string EmpresaPropietario { get; set; }
        public string Telefono { get; set; }


    }


    public class ViewCrearUsuarioAdministradorModel
    {
        public int IdEmpresaCreada { get; set; }
        public string Ruc { get; set; }
        public string Email { get; set; }
        public string Mensaje { get; set; }

    }



    public class ViewUsuarioToken
    {
        public string Token { get; set; }
        public DateTime Expiracion { get; set; }
        public string UriImagen { get; set; }
        public string Error { get; set; }


    }
    public class IdEmpresaPermisoAdmin
    {
        [Required(ErrorMessage = "Campo requerido {0}")]
        [RegularExpression("(^[0-9]+$)", ErrorMessage = "Solo se permiten números")]
        public int IdEmpresaAdmin { get; set; }
        public bool? BloquiarEmpresa { get; set; }
        public bool? IngresoEmpresa { get; set; }
        public bool? IngresoPermisoEmpresa { get; set; }

    }
    public class IdPermisosEmpresaPermisos
    {
        public int IdPermisos { get; set; }
    }
    public class IdEmpresaDto
    {
        public int IdEmpresaAdmin { get; set; }
    }



    public class PutEmpresaCreadaDto
    {
        public int Id { get; set; }
        public string EmpresasCreadaEmail { get; set; }
        public bool? EmpresasCreadaEstado { get; set; }
        public bool? EmpresasCreadapruebaproduccion { get; set; }
        public IFormFile? EmpresasCreadaUbicacionarchivop12 { get; set; }
        public string? EmpresasCreadaContrasenaArchivop12 { get; set; }
        public IFormFile? EmpresasCreadaImagen { get; set; }
        public string? EmpresasCreadaDireccion { get; set; }
        public string? EmpresasCreadaTelefono { get; set; }
        public string? EmpresasCreadaObligadoContabiliadad { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal EmpresasPorcentajeIva { get; set; }

    }
}
