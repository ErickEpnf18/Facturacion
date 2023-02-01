using NesFactApiV4.Services;
using System.ComponentModel.DataAnnotations;

namespace NesFactApiV4.Atributos
{
    public class ValidarCedulaRucAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }

            CVealidarCedula verificarDni = new CVealidarCedula();
            var verificar = verificarDni.VerificaIdentificacion(value.ToString());
            if (verificar.Result == false)
            {
                return new ValidationResult("La cedula/ruc  ingresado no es valida");
            }
            return ValidationResult.Success;
        }
    }
}
