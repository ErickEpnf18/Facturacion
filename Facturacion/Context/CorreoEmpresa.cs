using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Facturacion.Context
{
    public class CorreoEmpresa
    {
        public int Id { get; set; }
        public int? FkEmpresa { get; set; }
        [ForeignKey(nameof(FkEmpresa))]
        public virtual Empresa FkEmpresaNavigation { get; set; }
        public int? FkLocal { get; set; }
        [ForeignKey(nameof(FkLocal))]
        public virtual Local FkLocalNavigation { get; set; }
        public int? FkEmpresasCreadas { get; set; }
        [ForeignKey(nameof(FkEmpresasCreadas))]
        public virtual EmpresasCreada EmpresasCreadasNavigation { get; set; }

        public int Port { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool EnableSsl { get; set; }
        public string Host { get; set; }
    }
}
