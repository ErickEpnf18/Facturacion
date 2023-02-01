using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Facturacion.Context
{
    public partial class RutasXml
    {
        public int Id { get; set; }
        public int FkComprobanteVenta { get; set; }
        [ForeignKey(nameof(FkComprobanteVenta))]
        public virtual ComprobanteVenta FkComprobanteVentaNavigation { get; set; }
        public string RutaGenerado { get; set; }
        public string RutaFirmado { get; set; }
        public string RutaAutorizado { get; set; }
        public string RutaPdf { get; set; }
        public string RutaEstaRecepcion { get; set; }
        public string RutaEstaAturizacion { get; set; }

    }
}
