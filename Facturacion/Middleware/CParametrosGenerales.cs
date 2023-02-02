using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Facturacion.Middleware
{
    public class CParametrosGenerales
    {  

        private static int? fkEmpresa = 0;
        private static decimal? porcentajeIva = 0;
        private static int? fkLocal;
        private static string usuarioRolId = "S/N";
        private static int fkTbSri = 0;
        private static string netUser = "S/N";
        private static bool? producionprueba=false;
        private static string usuarioRolName = "S/N";
        public static decimal? PorcentajeIva
        {
            get { return porcentajeIva; }
            set { porcentajeIva = value; }
        }
        public static int? FkEmpresa
        {
            get { return fkEmpresa; }
            set { fkEmpresa = value; }
        }
        public static int? FkLocal
        {
            get { return fkLocal; }
            set { fkLocal = value; }
        }
        public static string UsuarioRolId
        {
            get { return usuarioRolId; }
            set { usuarioRolId = value; }
        }
        public static string UsuarioRolName
        {
            get { return usuarioRolName; }
            set { usuarioRolName = value; }
        }
        public static int FkTbSri
        {
            get { return fkTbSri; }
            set { fkTbSri = value; }
        }
        public static string NetUser
        {
            get { return netUser; }
            set { netUser = value; }
        }

        public static bool ProduccionPrueba
        {
            get { return (bool)producionprueba; }
            set { producionprueba = value; }
        }

        public class ParametrosGenerales
        {
            public int? fkEmpresa { get; set; }
            public int? fkLocal { get; set; }
            public string usuarioRolId { get; set; }
            public int fkTbSri { get; set; }
            public string netUser { get; set; }
            public bool? produccionPrueba { get; set; }
            public string usuarioRolName { get; set; }


        }
        public static void CargarParametros(ParametrosGenerales parametros)
        {

            FkEmpresa = parametros.fkEmpresa;
            FkLocal = parametros.fkLocal;
            UsuarioRolId = parametros.usuarioRolId;
            FkTbSri= parametros.fkTbSri;
            NetUser = parametros.netUser;
            producionprueba = parametros.produccionPrueba;
            usuarioRolName = parametros.usuarioRolName;
        }

    }
}
