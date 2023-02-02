using Facturacion.Model;
using System.ComponentModel.DataAnnotations;


namespace Facturacion.DTOs
{
    public class EmpresaDto
    {

    }
    public class ViewEmpresaDto
    {
        public int Id { get; set; }
        public string EmpresaRuc { get; set; }
        public string EmpresaPropietario { get; set; }
        public string EmpresaEmail { get; set; }
        public bool? EmpresaEstado { get; set; }
        public string EmpresaImagen { get; set; }
        public string EmpresaUsuarioCreador { get; set; }
        public string EmpresaTelefono { get; set; }
        public string Mensaje { get; set; }


    }
    public class CorreoEmpresaDto
    {
        [Required(ErrorMessage = "Campo {0} es necesesario")]
        public int IdEmpresaCreada { get; set; }
        [Required(ErrorMessage = "Campo {0} es necesesario")]
        public int Port { get; set; }
        [Required(ErrorMessage = "Campo {0} es necesesario")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Campo {0} es necesesario")]

        public string Password { get; set; }
        [Required(ErrorMessage = "Campo {0} es necesesario")]
        public bool EnableSsl { get; set; }
        [Required(ErrorMessage = "Campo {0} es necesesario")]
        public string Host { get; set; }


    }
    public class updateCorreoEmpresaDto: CorreoEmpresaDto
    {
        [Required(ErrorMessage = "Campo {0} es necesesario")]
        public int IdCorreoEmpresaCreada { get; set; }



    }

    public class ViewCorreoEmpresaCreadaDto
    {
        public int Id { get; set; }
        public int FkEmpresasCreadas { get; set; }
        public int Port { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool EnableSsl { get; set; }
        public string Host { get; set; }
        public string Mensajes { get; set; }
    }

    public class ViewCorreoEmpresaDto
    {
        public int Id { get; set; }
        public int Port { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool EnableSsl { get; set; }
        public string Host { get; set; }
        public string Mensajes { get; set; }
        public ViewEmpresaDto EmpresaDto { get; set; }
        

    }
    public class ViewTipoComprobante
    {
        public int Id { get; set; }
        public string Comprobanteid { get; set; }
        public string Comprobanteombre { get; set; }

    }
    public class Secuencialto
    {
        [Required(ErrorMessage = "Campo {0} es necesesario")]

        public int? FkEmpresasCreada { get; set; }
      
    }
    public class IdSecuencialto
    {
        [Required(ErrorMessage = "Campo {0} es necesesario")]
     

        public int IdSecuencial { get; set; }

    }
    public class ViewSecuencialto
    {
        public int Id { get; set; }
        public string Comprobante { get; set; }
        public string? EmpresaCreada { get; set; }
        public string Numestablecimiento { get; set; }
        public string Numpuntoemision { get; set; }
        public string Numcorrelativo { get; set; }
        public bool Estado { get; set; }
    }
    public class PostSecuencialto
    {
        public int FkTipoCompronbante { get; set; }
        public int? FkEmpresasCreada { get; set; }
        [Required(ErrorMessage = "Campo requerido {0}")]
        [StringLength(maximumLength: 3, MinimumLength = 3)]
        [RegularExpression("(^[0-9]+$)", ErrorMessage = "Solo se permiten números")]
        public string Numestablecimiento { get; set; }
        [Required(ErrorMessage = "Campo requerido {0}")]
        [StringLength(maximumLength: 3, MinimumLength = 3)]
        [RegularExpression("(^[0-9]+$)", ErrorMessage = "Solo se permiten números")]
        public string Numpuntoemision { get; set; }
        [Required(ErrorMessage = "Campo requerido {0}")]
        [StringLength(maximumLength: 9, MinimumLength = 1)]
        [RegularExpression("(^[0-9]+$)", ErrorMessage = "Solo se permiten números")]
        public string Numcorrelativo { get; set; }
        public bool Estado { get; set; }
    }
    public class UpdateSecuencialDto
    {
        public int IdSecuencial { get; set; }

        [StringLength(maximumLength: 9, MinimumLength = 1)]
        [RegularExpression("(^[0-9]+$)", ErrorMessage = "Solo se permiten números")]
        public int Numcorrelativo { get; set; }
        public bool Estado { get; set; }
    }
    public class ViewSecuencial
    {
        public string NombreComprobante { get; set; }
        public string NumeroSecuencial { get; set; }

        public string Mensaje { get; set; }

    }
    public class ViewEmpresaInformacionGeneralDTO
    {
        public int Id { get; set; }
        public string EmpresaRuc { get; set; }
        public string EmpresaPropietario { get; set; }
        public string EmpresaEmail { get; set; }
        public bool? EmpresaEstado { get; set; }
        public string EmpresaContrasena { get; set; }
        public string EmpresaImagen { get; set; }
        public string EmpresaUsuarioCreador { get; set; }
        public string EmpresaTelefono { get; set; }

        public ViewPermisosEmpresa? viewPermisosEmpresa { get; set; }
        public CorreoEmpresaDto? CorreoEmpresaCreadadto { get; set; }
        public ICollection<UsuarioEmpresaDTo>? UsuarioEmpresaCreadadDTos { get; set; }


    }


    public class ViewPermisosEmpresa
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
        public EmpreesaCreadaDTo empreesaCreadaDTo { get; set; }

    }
    public class UsuarioEmpresaDTo: UsuarioEmpresaCreadaDTo
    { }
    
}
