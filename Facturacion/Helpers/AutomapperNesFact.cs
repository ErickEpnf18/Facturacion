//using AutoMapper;
//using Facturacion.Context;
//using Facturacion.DTOs;
//using Facturacion.Model;


//namespace Facturacion.Helpers
//{
//    public class AutomapperNesFact : Profile
//    {
//        public AutomapperNesFact()
//        {
//            //SriPersonasMyPyme
//            CreateMap<SriRepositorio, ModelSriRepositorio>().ReverseMap();
//            CreateMap<PersonaMiPyme1, modelPersonaMiPyme>().ReverseMap();
//            CreateMap<PersonaEstablecimientosRuc, modelPersonaEstablecimientosRuc>().ReverseMap();
//            CreateMap<Informacionfechascontribuyente, modelInformacionfechascontribuyente>().ReverseMap();

//            //Empresa
//            CreateMap<Empresa, PutEmpresaDto>().ReverseMap();
//            CreateMap<Empresa, ViewEmpresaDto>().ReverseMap();
//            CreateMap<FormasPago, ViewFormasPagoDto>().ReverseMap();

//            //PermisosEmpresa
//            CreateMap<PermisosEmpresa, IdEmpresaPermisoAdmin>().ReverseMap();
//            CreateMap<PermisosEmpresa, ViewPermisosEmpresaDTo>().ReverseMap();
//            CreateMap<CorreoEmpresa, CorreoEmpresaIdDto>().ReverseMap();

//            //EmpresaCorreo
//            CreateMap<CorreoEmpresa, ViewCorreoEmpresaDto>().ReverseMap();
//            CreateMap<CorreoEmpresa, CorreoEmpresaDto>().ReverseMap();
//            CreateMap<CorreoEmpresa, updateCorreoEmpresaDto>().ReverseMap();

//            //Tipo comprobante
//            CreateMap<TipoComprobante, ViewTipoComprobante>().ReverseMap();
//            //Secuencial
//            CreateMap<Secuencial, Secuencialto>().ReverseMap();
//            //EmpresaCreada
//            //Correo
//            CreateMap<EmpresasCreada, ViewEmpresaCreadaInformacionGeneralDTO>().ReverseMap();

//            CreateMap<Usuario, UsuarioEmpresaCreadaDTo>().ReverseMap();
//            CreateMap<Local, LocalEmpresaCreadaDto>().ReverseMap();
//            CreateMap<CorreoEmpresa, ViewCorreoEmpresaCreadaDto>().ReverseMap();
//            CreateMap<CorreoEmpresa, CorreoEmpresaCreadaDto>().ReverseMap();
//            CreateMap<PermisosEmpresa, ViewPermisosEmpresaCreadaDTo>().ReverseMap();
//            CreateMap<CorreoEmpresa, UpdateCorreoEmpresaCreadaDto>().ReverseMap();


//            //EmpresaGet
//            CreateMap<EmpresasCreada, ViewEmpresaCreadaInformacionGeneralDTO>()
//             .ForMember(x => x.CorreoEmpresaCreadadto, ops => ops.MapFrom(src => src.FkCorreoNavigation))
//             .ForMember(x => x.PermisosEmpresaCreadadto, ops => ops.MapFrom(src => src.FkPermisosEmpresaNavigation))
//             .ForMember(x => x.UsuarioEmpresaCreadadDTos, ops => ops.MapFrom(src => src.Usuarios))
//             .ForMember(x => x.LocalEmpresaCreadadDtos, ops => ops.MapFrom(src => src.Locals));
//            //ViewPermisosEmpresaCreadaDTo

//            CreateMap<PermisosEmpresa, ViewPermisosEmpresaCreadaDTo>()
//               .ForMember(x => x.empreesaCreadaDTo, ops => ops.MapFrom(src => src.FkEmpresaNavigation));



//            CreateMap<PermisosEmpresa, ViewPermisosEmpresaCreadaDTo>().ReverseMap();
//            CreateMap<PermisosEmpresa, PermisosEmpresaCreadaDTo>().ReverseMap();

//            CreateMap<CorreoEmpresa, ViewCorreoEmpresaDto>()
//                 .ForMember(x => x.EmpresaDto, ops => ops.MapFrom(src => src.FkEmpresasCreadas));

//            //Secuenciales
//            CreateMap<Secuencial, SecuencialDto>().ReverseMap();
//            CreateMap<Secuencial, ViewSecuencialto>().ReverseMap();
//            CreateMap<Secuencial, PostSecuencialto>().ReverseMap();



//            //Comprpnate de venta
//            CreateMap<Cliente, ClieteComprobanteventaDto>().ReverseMap();
//            CreateMap<Producto, ProductoComprobanteVentaDto>().ReverseMap();

//            CreateMap<DetalleVenta, DetalleComprobanteVentaDto>()
//             .ForMember(x => x.ProductoVentaDTo, ops => ops.MapFrom(src => src.FkProductoNavigation));

//            CreateMap<ComprobanteVenta, ComprobanteVentaECDto>()
//                .ForMember(x => x.ClieteComprobanteventaDto, ops => ops.MapFrom(src => src.FkClienteNavigation));

//            CreateMap<ComprobanteVenta, ComprobanteVentaCompletoECDto>()
//                .ForMember(x => x.ClieteComprobanteventaDto, ops => ops.MapFrom(src => src.FkClienteNavigation))
//                .ForMember(x => x.detalleComprobanteVentasDto, ops => ops.MapFrom(src => src.DetalleVenta));

//            CreateMap<ComprobanteVenta, PostComprobanteVentaECDto>()
//              .ForMember(x => x.detalleComprobanteVentaDtos, ops => ops.MapFrom(src => src.Tbdocumentosfacturacionelectronicas));

//            CreateMap<DetalleVenta, PostDetalleComprobanteVentaDto>().ReverseMap();
//            CreateMap<ComprobanteVenta, PostComprobanteVentaECDto>().ReverseMap();

//            //Cliente

//            CreateMap<Cliente, PostClienteEmpresaCreadaDTo>().ReverseMap();
//            CreateMap<Cliente, GetListClienteEmpresaCreadaDto>().ReverseMap();
//            CreateMap<Cliente, PutClienteEmpresaCreadaDTo>().ReverseMap();
//            //producto
//            CreateMap<Producto, ProductoDTo>().ReverseMap();
//            CreateMap<Producto, ViewProductoDto>().ReverseMap();
//            CreateMap<Producto, ProductoIdDTo>().ReverseMap();

//            //Proveedor

//            CreateMap<Proveedor, ViewProveedoresEmpresaCreadaDTo>().ReverseMap();

//            CreateMap<Proveedor, GetProveedoresEmpresaCreadaDTo>().ReverseMap();
//            CreateMap<Proveedor, ProveedoresEmpresaCreadaDTo>().ReverseMap();


//            ///comprovante comprao    
//            CreateMap<ComprobanteCompra, ViewEmpresaCreadaComprobanteCompraDTO>().ReverseMap();
//            CreateMap<ComprobanteCompra, EmpresaCreadaComprobanteCompraDTO>().ReverseMap();
//            CreateMap<ComprobanteCompra, ViewFacturaCompraDTo>().ReverseMap();
//            CreateMap<DetallesFacturaCompra, DetalleFacturaCompraDTo>().ReverseMap();
//            CreateMap<ComprobanteCompra, ListViewEmpresaCreadaComprobanteDetalleCompraCreadosDTO>()
//            .ForMember(x => x.DetalleFacturaCompras, ops => ops.MapFrom(src => src.TbDetallesFacturaCompras));

//        }
//    }
//}
