using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using NesFactApiV4.Context;
using NesFactApiV4.Services.Interfaces;
using NesFactApiV4.Helpers;

namespace NesFactApiV4.Services
{
//    public class InicializadorDB : IInicializadorDB
//    {
//        private readonly AplicationDbContext _db;
//        private readonly UserManager<NetUserAditional> _userManager;
//        private readonly RoleManager<IdentityRole> _roleManager;
//        private readonly CConsultaSri cConsultaSri;
//        public InicializadorDB(Context.AplicationDbContext db, UserManager<NetUserAditional> userManager, RoleManager<IdentityRole> roleManager, CConsultaSri cConsultaSri)
//        {
//            _db = db;
//            _userManager = userManager;
//            _roleManager = roleManager;
//            this.cConsultaSri = cConsultaSri;
//        }


//        public async void Inicializar()
//        {
//            DateTime currentTimePacific = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time"));
//            CPassword cPassword = new CPassword();
//            try
//            {
//                if (_db.Database.GetPendingMigrations().Count() > 0)
//                {
//                    _db.Database.Migrate();
//                }
//            }
//            catch (Exception ex)
//            {
//                var mer = ex.ToString();
//            }


//            //if (_db.Roles.Any(ro => ro.Name == Roles.AdminNessoft)) return;

//            //_roleManager.CreateAsync(new IdentityRole(Roles.AdminNessoft)).GetAwaiter().GetResult();
//            //_roleManager.CreateAsync(new IdentityRole(Roles.AdminEmpresaDueno)).GetAwaiter().GetResult();
//            //_roleManager.CreateAsync(new IdentityRole(Roles.AdminEmpresaDuenoUsuario)).GetAwaiter().GetResult();
//            //_roleManager.CreateAsync(new IdentityRole(Roles.LocalAdminEmpresaDueno)).GetAwaiter().GetResult();
//            //_roleManager.CreateAsync(new IdentityRole(Roles.LocalAdminEmpresaDuenoUsuario)).GetAwaiter().GetResult();


//            //_roleManager.CreateAsync(new IdentityRole(Roles.AdminEmpresa)).GetAwaiter().GetResult();
//            //_roleManager.CreateAsync(new IdentityRole(Roles.EmpresaUsuario)).GetAwaiter().GetResult();
//            //_roleManager.CreateAsync(new IdentityRole(Roles.AdminEmpresaCreada)).GetAwaiter().GetResult();
//            //_roleManager.CreateAsync(new IdentityRole(Roles.UsuarioEmpresaCreada)).GetAwaiter().GetResult();


//            //_roleManager.CreateAsync(new IdentityRole(Roles.LocalEmpresaCreada)).GetAwaiter().GetResult();
//            //_roleManager.CreateAsync(new IdentityRole(Roles.LocalUsuarioEmpresaCreada)).GetAwaiter().GetResult();


//            //byte[] ValidadorHash, ValidadorSalt;



//            //var tipoF = new TipoComprobante
//            //{
//            //    Comprobanteid = "01",
//            //    Comprobanteombre = "FACTURA",

//            //};
//            //_db.Add(tipoF);
//            //_db.SaveChanges();

//            //var tipoL = new TipoComprobante
//            //{
//            //    Comprobanteid = "03",
//            //    Comprobanteombre = "LIQUIDACIÓN DE COMPRA DE  BIENES Y PRESTACIÓN DE SERVICIOS",




//            //};
//            //_db.Add(tipoL);
//            //_db.SaveChanges();
//            //var tipoN = new TipoComprobante
//            //{
//            //    Comprobanteid = "04",
//            //    Comprobanteombre = "NOTA DE CRÉDITO",

//            //};
//            //_db.Add(tipoN);
//            //_db.SaveChanges();
//            //var tipoNo = new TipoComprobante
//            //{
//            //    Comprobanteid = "05",
//            //    Comprobanteombre = "NOTA DE DÉBITO",

//            //};
//            //_db.Add(tipoNo);
//            //_db.SaveChanges();
//            //var tipoG = new TipoComprobante
//            //{
//            //    Comprobanteid = "06",
//            //    Comprobanteombre = "GUÍA DE REMISIÓN",

//            //};
//            //_db.Add(tipoG);
//            //_db.SaveChanges();
//            //var tipoR = new TipoComprobante
//            //{
//            //    Comprobanteid = "07",
//            //    Comprobanteombre = "COMPROBANTE DE RETENCIÓN",

//            //};

//            //_db.Add(tipoR);
//            //_db.SaveChanges();

//            //var sriRepositorio = cConsultaSri.GetRucSri("1723640130001");
//            //var Sri = new SriRepositorio();
//            //Sri = sriRepositorio;
//            //_db.Add(Sri);
//            //_db.SaveChanges();

//            //var empresa = new Empresa
//            //{
//            //    EmpresaRuc = sriRepositorio.NumeroRuc,
//            //    EmpresaPropietario = sriRepositorio.RazonSocial,
//            //    EmpresaEmail = "edwinwla13@hotmail.com",
//            //    EmpresaEstado = true,
//            //    Emprepruebaproduccion = false,
//            //    EmpresaDueno = true
//            //};
//            //_db.Add(empresa);
//            //_db.SaveChanges();
//            //var user = new NetUserAditional
//            //{
//            //    UserName = sriRepositorio.NumeroRuc,
//            //    Email = "edwinwla13@hotmail.com",
//            //    FechaDeRegistro = currentTimePacific,
//            //    FkEmpresa = empresa.Id,
//            //    FkEmpresaCreada = null,
//            //    FkUsuario = null,
//            //    EmailConfirmed = true


//            //};

//            //var result = _userManager.CreateAsync(user, "NesSoft120424");
//            //if (result.Result.Succeeded)
//            //{
//            //      _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "AdminNessoft")).GetAwaiter().GetResult();
//            //     _userManager.AddToRoleAsync(user, "AdminNessoft").GetAwaiter().GetResult();

//            //}
//        }
//    }
//}
public static class InicializarmMiddleware1
{
    public static void InicializarmMiddleware(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<IInicializadorDB>();
    }
}