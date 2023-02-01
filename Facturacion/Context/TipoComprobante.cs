using System;
using System.Collections.Generic;

#nullable disable

namespace Facturacion.Context
{
    public partial class TipoComprobante
    {
        public int Id { get; set; }
        public string Comprobanteid { get; set; }
        public string Comprobanteombre { get; set; }
        public Secuencial SecuencialNavigation { get; set; }
    }
}
