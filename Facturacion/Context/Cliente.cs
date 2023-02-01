using System.ComponentModel.DataAnnotations.Schema;

namespace Facturacion.Context
{
    public class Cliente
    {
        public int Id { get; set; }
        public int FkRespositorioCedulas { get; set; }
        [ForeignKey(nameof(FkRespositorioCedulas))]
        public RespositorioCedulas FkRespositorioCedulasNavigation { get; set; }
        public int? FkSrirepositorio { get; set; }
        [ForeignKey(nameof(FkSrirepositorio))]
        public SriRepositorio FkSrirepositorioNavigation { get; set; }
        public int? FkEmpresa { get; set; }
        [ForeignKey(nameof(FkEmpresa))]
        public virtual Empresa FkEmpresaNavigation { get; set; }
        public int? FkLocal { get; set; }
        [ForeignKey(nameof(FkLocal))]
        public virtual Local FkLocalNavigation { get; set; }
        public int? EmpresasCreadas { get; set; }
        [ForeignKey(nameof(EmpresasCreadas))]
        public virtual EmpresasCreada EmpresasCreadasNavigation { get; set; }
        public int? FkUsuario { get; set; }
        [ForeignKey(nameof(FkEmpresa))]
        public virtual Usuario Usuario { get; set; }

        public string? Identificacion { get; set; }
        public string? Razonsocial { get; set; }
        public string? Direccion { get; set; }
        public string? Email { get; set; }
        public string? NumeroTelefon { get; set; }
        public bool? Estado  { get; set; }
    }
}
