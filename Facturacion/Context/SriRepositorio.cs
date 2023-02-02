using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Facturacion.Context
{
    public partial class SriRepositorio
    {
        public int Id { get; set; }
        public string RepresentanteLegal { get; set; }
        public string Obligado { get; set; }
        public string NombreComercial { get; set; }
        public string NumeroRuc { get; set; }
        public string AgenteRepresentante { get; set; }
        public string Error { get; set; }
        public string RazonSocial { get; set; }


        public virtual ICollection<Empresa> Empresas { get; set; }
        public virtual ICollection<EmpresasCreada> EmpresasCreadas { get; set; }
        public virtual ICollection<Local> Local { get; set; }
        public virtual ICollection<Proveedor> Proveedores { get; set; }
    }


 


}
