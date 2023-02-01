using System;
using System.Collections.Generic;

#nullable disable

namespace Facturacion.Context
{
    public partial class RespositorioCedulas
    {
      
        public int Id { get; set; }
        public string Identificacion { get; set; }
        public string Razonsocial { get; set; }
        public string Estadocivil { get; set; }
        public DateTime Fechanacimiento { get; set; }
        public string Mensaje { get; set; }

    }
}
