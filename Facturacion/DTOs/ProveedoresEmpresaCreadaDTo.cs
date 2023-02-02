using System.ComponentModel.DataAnnotations;

namespace Facturacion.DTOs
{
    public class ProveedoresEmpresaCreadaDTo
    {
        [Required(ErrorMessage = "Campo {0} es necesesario")]
        [StringLength(maximumLength: 13, MinimumLength = 13)]
        public string Ruc { get; set; }
    }

    public class ProveedoresEmpresaCreadaManualDTo
    {
        [Required(ErrorMessage = "Campo {0} es necesesario")]
        [StringLength(maximumLength: 10, MinimumLength = 10)]
        public string Cedula { get; set; }

        public string RazonSocial { get; set; }
    }



    public class ViewProveedoresEmpresaCreadaDTo
    {
        public int Id { get; set; }
        public string ProveedoresRuc { get; set; }
        public string ProveedoresPropietario { get; set; }
        public bool? ProveedoresEstado { get; set; }

    }
    public class ViewClientesRucEmpresaCreadaDTo
    {
        public int Id { get; set; }
        public string ClientesRuc { get; set; }
        public string ClientesPropietario { get; set; }
        public bool? ClientesEstado { get; set; }

    }

    public class ClientesRucEmpresaCreadaDTo: ProveedoresEmpresaCreadaDTo
    {
        [EmailAddress]
        [Required(ErrorMessage = "El campo {0} es Requerido")]
        public string Email { get; set; }
        [StringLength(maximumLength: 10, MinimumLength = 10)]
        [RegularExpression("(^[0-9]+$)", ErrorMessage = "Solo se permiten números")]
        public string Telefono { get; set; }
    }

    public class GetProveedoresEmpresaCreadaDTo
    {
        [Required(ErrorMessage = "El campo Id es Requerido")]
        public int Id { get; set; }

    }

    public class DeleteProveedoresEmpresaCreadaDTo
    {
        [Required(ErrorMessage = "El campo Id es Requerido")]
        public int Id { get; set; }

    }

    public class PutProveedoresEmpresaCreadaDTo
    {
        [Required(ErrorMessage = "El campo Id es Requerido")]
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es Requerido")]
        public bool Estado { get; set; }
    }

}