using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Facturacion.Migrations
{
    public partial class Db2Ecommerce2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Comprobantes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Comprobanteid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comprobanteombre = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comprobantes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FormasPagos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormasPagos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RespositorioCedulas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Identificacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Razonsocial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Estadocivil = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fechanacimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Mensaje = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RespositorioCedulas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SriRepositorios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonaSociedad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RepresentanteLegal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Obligado = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NombreComercial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumeroRuc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AgenteRepresentante = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Error = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RazonSocial = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SriRepositorios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Empresas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FkSriRepositorio = table.Column<int>(type: "int", nullable: true),
                    EmpresaRuc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmpresaPropietario = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmpresaEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmpresaEstado = table.Column<bool>(type: "bit", nullable: true),
                    EmpresaContrasena = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmpresaImagen = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmpresaTelefono = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmpresaUsuarioCreador = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empresas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Empresas_SriRepositorios_FkSriRepositorio",
                        column: x => x.FkSriRepositorio,
                        principalTable: "SriRepositorios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EmpresasCreadas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FkEmpresa = table.Column<int>(type: "int", nullable: false),
                    FkSriRepositorio = table.Column<int>(type: "int", nullable: true),
                    EmpresasCreadaRuc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmpresasCreadaPropietario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmpresasCreadaEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmpresasCreadaEstado = table.Column<bool>(type: "bit", nullable: true),
                    EmpresasCreadapruebaproduccion = table.Column<bool>(type: "bit", nullable: true),
                    EmpresasCreadaUbicacionarchivop12 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmpresasCreadaContrasenaArchivop12 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmpresasCreadaImagen = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmpresasCreadaDireccion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmpresasCreadaTelefono = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmpresasCreadaObligadoContabiliadad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmpresasPorcentajeIva = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmpresasCreadas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmpresasCreadas_Empresas_FkEmpresa",
                        column: x => x.FkEmpresa,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmpresasCreadas_SriRepositorios_FkSriRepositorio",
                        column: x => x.FkSriRepositorio,
                        principalTable: "SriRepositorios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Locals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FkSriRepositorio = table.Column<int>(type: "int", nullable: true),
                    FkEmpresa = table.Column<int>(type: "int", nullable: true),
                    FkEmpresasCreadas = table.Column<int>(type: "int", nullable: true),
                    LocalNombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocalTelefono = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocalDireccion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocalActividad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocalFechainicioactividad = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LocalEstado = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocalNumero = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Locals_Empresas_FkEmpresa",
                        column: x => x.FkEmpresa,
                        principalTable: "Empresas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Locals_EmpresasCreadas_FkEmpresasCreadas",
                        column: x => x.FkEmpresasCreadas,
                        principalTable: "EmpresasCreadas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Locals_SriRepositorios_FkSriRepositorio",
                        column: x => x.FkSriRepositorio,
                        principalTable: "SriRepositorios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CajaGlobals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FkEmpresa = table.Column<int>(type: "int", nullable: true),
                    FkLocal = table.Column<int>(type: "int", nullable: true),
                    EmpresasCreadas = table.Column<int>(type: "int", nullable: true),
                    FechaIngreso = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CajaGlobalEfectivoTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CajaGlobalTarjetasDebitoTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CajaGlobalTrasferenciaTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CajaGlobalCreditoTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CajaGlobalTotalValores = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CajaGlobals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CajaGlobals_Empresas_FkEmpresa",
                        column: x => x.FkEmpresa,
                        principalTable: "Empresas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CajaGlobals_EmpresasCreadas_EmpresasCreadas",
                        column: x => x.EmpresasCreadas,
                        principalTable: "EmpresasCreadas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CajaGlobals_Locals_FkLocal",
                        column: x => x.FkLocal,
                        principalTable: "Locals",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CorreoEmpresas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FkEmpresa = table.Column<int>(type: "int", nullable: true),
                    FkLocal = table.Column<int>(type: "int", nullable: true),
                    FkEmpresasCreadas = table.Column<int>(type: "int", nullable: true),
                    Port = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EnableSsl = table.Column<bool>(type: "bit", nullable: false),
                    Host = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CorreoEmpresas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CorreoEmpresas_Empresas_FkEmpresa",
                        column: x => x.FkEmpresa,
                        principalTable: "Empresas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CorreoEmpresas_EmpresasCreadas_FkEmpresasCreadas",
                        column: x => x.FkEmpresasCreadas,
                        principalTable: "EmpresasCreadas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CorreoEmpresas_Locals_FkLocal",
                        column: x => x.FkLocal,
                        principalTable: "Locals",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PermisosEmpresas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FkEmpresa = table.Column<int>(type: "int", nullable: true),
                    FkEmpresasCreadas = table.Column<int>(type: "int", nullable: true),
                    FkLocal = table.Column<int>(type: "int", nullable: true),
                    NumeroFacturas = table.Column<int>(type: "int", nullable: false),
                    InicioDeActividades = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FinDeActividades = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BloquiarEmpresa = table.Column<bool>(type: "bit", nullable: false),
                    IngresoEmpresa = table.Column<bool>(type: "bit", nullable: false),
                    IngresoPermisoEmpresa = table.Column<bool>(type: "bit", nullable: false),
                    IngresoFacturasVentas = table.Column<bool>(type: "bit", nullable: false),
                    IngresoFacturacionElectronica = table.Column<bool>(type: "bit", nullable: false),
                    IngresoCrearClientes = table.Column<bool>(type: "bit", nullable: false),
                    IngresoFacturasCompras = table.Column<bool>(type: "bit", nullable: false),
                    IngresoProveedores = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermisosEmpresas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PermisosEmpresas_Empresas_FkEmpresa",
                        column: x => x.FkEmpresa,
                        principalTable: "Empresas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PermisosEmpresas_EmpresasCreadas_FkEmpresasCreadas",
                        column: x => x.FkEmpresasCreadas,
                        principalTable: "EmpresasCreadas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PermisosEmpresas_Locals_FkLocal",
                        column: x => x.FkLocal,
                        principalTable: "Locals",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Proveedores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FkSriRepositorio = table.Column<int>(type: "int", nullable: true),
                    FkEmpresa = table.Column<int>(type: "int", nullable: true),
                    FkEmpresasCreada = table.Column<int>(type: "int", nullable: true),
                    FkLocal = table.Column<int>(type: "int", nullable: true),
                    ProveedoresRuc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProveedoresPropietario = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProveedoresEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProveedoresEstado = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proveedores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Proveedores_Empresas_FkEmpresa",
                        column: x => x.FkEmpresa,
                        principalTable: "Empresas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Proveedores_EmpresasCreadas_FkEmpresasCreada",
                        column: x => x.FkEmpresasCreada,
                        principalTable: "EmpresasCreadas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Proveedores_Locals_FkLocal",
                        column: x => x.FkLocal,
                        principalTable: "Locals",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Proveedores_SriRepositorios_FkSriRepositorio",
                        column: x => x.FkSriRepositorio,
                        principalTable: "SriRepositorios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Secuencials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FkTipoCompronbante = table.Column<int>(type: "int", nullable: false),
                    FkEmpresa = table.Column<int>(type: "int", nullable: true),
                    FkEmpresasCreada = table.Column<int>(type: "int", nullable: true),
                    FkLocal = table.Column<int>(type: "int", nullable: true),
                    Numestablecimiento = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Numpuntoemision = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Numcorrelativo = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Secuencials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Secuencials_Comprobantes_FkTipoCompronbante",
                        column: x => x.FkTipoCompronbante,
                        principalTable: "Comprobantes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Secuencials_Empresas_FkEmpresa",
                        column: x => x.FkEmpresa,
                        principalTable: "Empresas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Secuencials_EmpresasCreadas_FkEmpresasCreada",
                        column: x => x.FkEmpresasCreada,
                        principalTable: "EmpresasCreadas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Secuencials_Locals_FkLocal",
                        column: x => x.FkLocal,
                        principalTable: "Locals",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FkEmpresa = table.Column<int>(type: "int", nullable: true),
                    FkEmpresasCreada = table.Column<int>(type: "int", nullable: true),
                    FkLocal = table.Column<int>(type: "int", nullable: true),
                    FechaIngreso = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioNombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UsuarioTipo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UsuarioEstado = table.Column<bool>(type: "bit", nullable: false),
                    IngresoFacturasVentas = table.Column<bool>(type: "bit", nullable: false),
                    IngresoFacturacionElectronica = table.Column<bool>(type: "bit", nullable: false),
                    IngresoCrearClientes = table.Column<bool>(type: "bit", nullable: false),
                    IngresoFacturasCompras = table.Column<bool>(type: "bit", nullable: false),
                    IngresoEmpresa = table.Column<bool>(type: "bit", nullable: false),
                    IngresoProveedores = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuarios_Empresas_FkEmpresa",
                        column: x => x.FkEmpresa,
                        principalTable: "Empresas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Usuarios_EmpresasCreadas_FkEmpresasCreada",
                        column: x => x.FkEmpresasCreada,
                        principalTable: "EmpresasCreadas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Usuarios_Locals_FkLocal",
                        column: x => x.FkLocal,
                        principalTable: "Locals",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FechaDeRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NumeroparaConfirmacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FkEmpresa = table.Column<int>(type: "int", nullable: true),
                    FkEmpresaCreada = table.Column<int>(type: "int", nullable: true),
                    FkLocal = table.Column<int>(type: "int", nullable: true),
                    FkUsuario = table.Column<int>(type: "int", nullable: true),
                    FkNetUserid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Empresas_FkEmpresa",
                        column: x => x.FkEmpresa,
                        principalTable: "Empresas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AspNetUsers_EmpresasCreadas_FkEmpresa",
                        column: x => x.FkEmpresa,
                        principalTable: "EmpresasCreadas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Locals_FkEmpresa",
                        column: x => x.FkEmpresa,
                        principalTable: "Locals",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Usuarios_FkEmpresa",
                        column: x => x.FkEmpresa,
                        principalTable: "Usuarios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FkRespositorioCedulas = table.Column<int>(type: "int", nullable: false),
                    FkSrirepositorio = table.Column<int>(type: "int", nullable: true),
                    FkEmpresa = table.Column<int>(type: "int", nullable: true),
                    FkLocal = table.Column<int>(type: "int", nullable: true),
                    EmpresasCreadas = table.Column<int>(type: "int", nullable: true),
                    FkUsuario = table.Column<int>(type: "int", nullable: true),
                    Identificacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Razonsocial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumeroTelefon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Estado = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clientes_Empresas_FkEmpresa",
                        column: x => x.FkEmpresa,
                        principalTable: "Empresas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Clientes_EmpresasCreadas_EmpresasCreadas",
                        column: x => x.EmpresasCreadas,
                        principalTable: "EmpresasCreadas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Clientes_Locals_FkLocal",
                        column: x => x.FkLocal,
                        principalTable: "Locals",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Clientes_RespositorioCedulas_FkRespositorioCedulas",
                        column: x => x.FkRespositorioCedulas,
                        principalTable: "RespositorioCedulas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Clientes_SriRepositorios_FkSrirepositorio",
                        column: x => x.FkSrirepositorio,
                        principalTable: "SriRepositorios",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Clientes_Usuarios_FkEmpresa",
                        column: x => x.FkEmpresa,
                        principalTable: "Usuarios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ComprobanteCompras",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FkEmpresa = table.Column<int>(type: "int", nullable: true),
                    FkEmpresasCreada = table.Column<int>(type: "int", nullable: true),
                    FkLocal = table.Column<int>(type: "int", nullable: true),
                    FkUsuario = table.Column<int>(type: "int", nullable: true),
                    FkProvedor = table.Column<int>(type: "int", nullable: false),
                    FechaIngreso = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaEmision = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DirEstablecimiento = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContribuyenteEspecial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ObligadoContabilidad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TipoIdentificacionComprador = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RazonSocialComprador = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentificacionComprador = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalSinImpuestos = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalDescuento = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodigoPorcentaje = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BaseImponible = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Propina = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ImporteTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Moneda = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FormaPago = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ClaveAccesoXml = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumeroFactura = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComprobanteCompras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComprobanteCompras_Empresas_FkEmpresa",
                        column: x => x.FkEmpresa,
                        principalTable: "Empresas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ComprobanteCompras_EmpresasCreadas_FkEmpresasCreada",
                        column: x => x.FkEmpresasCreada,
                        principalTable: "EmpresasCreadas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ComprobanteCompras_Locals_FkLocal",
                        column: x => x.FkLocal,
                        principalTable: "Locals",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ComprobanteCompras_Proveedores_FkProvedor",
                        column: x => x.FkProvedor,
                        principalTable: "Proveedores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComprobanteCompras_Usuarios_FkEmpresa",
                        column: x => x.FkEmpresa,
                        principalTable: "Usuarios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Productos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FkEmpresa = table.Column<int>(type: "int", nullable: true),
                    FkEmpresasCreada = table.Column<int>(type: "int", nullable: true),
                    FkLocal = table.Column<int>(type: "int", nullable: true),
                    FkUsuario = table.Column<int>(type: "int", nullable: true),
                    FechaRegistroProducto = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EsProducto = table.Column<bool>(type: "bit", nullable: true),
                    Sku = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductoCodigo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductoDescripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductoEstado = table.Column<bool>(type: "bit", nullable: true),
                    ProductoValor = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ProductoCantidad = table.Column<int>(type: "int", nullable: true),
                    ProductoConIva = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Productos_Empresas_FkEmpresa",
                        column: x => x.FkEmpresa,
                        principalTable: "Empresas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Productos_EmpresasCreadas_FkEmpresasCreada",
                        column: x => x.FkEmpresasCreada,
                        principalTable: "EmpresasCreadas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Productos_Locals_FkLocal",
                        column: x => x.FkLocal,
                        principalTable: "Locals",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Productos_Usuarios_FkUsuario",
                        column: x => x.FkUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ComprobanteVentas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FkEmpresa = table.Column<int>(type: "int", nullable: true),
                    FkEmpresasCreada = table.Column<int>(type: "int", nullable: true),
                    FkLocal = table.Column<int>(type: "int", nullable: true),
                    FkUsuario = table.Column<int>(type: "int", nullable: true),
                    FkSecuencial = table.Column<int>(type: "int", nullable: true),
                    FkCliente = table.Column<int>(type: "int", nullable: true),
                    FkTipoComprobante = table.Column<int>(type: "int", nullable: true),
                    ComprobanteFormapago = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ComprobantevFecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ComprobanteNumero = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ComprobanteEstado = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ComprobanteSubtotal0 = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ComprobanteSubtotal12 = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ComprobanteDescuento = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ComprobanteSubtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ComprobanteIvatotal = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ComprobantevTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Docsri = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComprobanteVentas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComprobanteVentas_Clientes_FkCliente",
                        column: x => x.FkCliente,
                        principalTable: "Clientes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ComprobanteVentas_Comprobantes_FkTipoComprobante",
                        column: x => x.FkTipoComprobante,
                        principalTable: "Comprobantes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ComprobanteVentas_Empresas_FkEmpresa",
                        column: x => x.FkEmpresa,
                        principalTable: "Empresas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ComprobanteVentas_EmpresasCreadas_FkEmpresasCreada",
                        column: x => x.FkEmpresasCreada,
                        principalTable: "EmpresasCreadas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ComprobanteVentas_Locals_FkLocal",
                        column: x => x.FkLocal,
                        principalTable: "Locals",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ComprobanteVentas_Secuencials_FkSecuencial",
                        column: x => x.FkSecuencial,
                        principalTable: "Secuencials",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ComprobanteVentas_Usuarios_FkUsuario",
                        column: x => x.FkUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DetallesFacturaCompras",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FkFacturaCompra = table.Column<int>(type: "int", nullable: false),
                    CodigoPrincipal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Descuento = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrecioTotalSinImpuesto = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetallesFacturaCompras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetallesFacturaCompras_ComprobanteCompras_FkFacturaCompra",
                        column: x => x.FkFacturaCompra,
                        principalTable: "ComprobanteCompras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DetalleVentas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FkProducto = table.Column<int>(type: "int", nullable: false),
                    FkComprobanteVenta = table.Column<int>(type: "int", nullable: false),
                    DetallevCantidad = table.Column<int>(type: "int", nullable: false),
                    DetallevValor = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DetallevEstado = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DetallevDescuento = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DetallevTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DetalleTotalIva = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetalleVentas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetalleVentas_ComprobanteVentas_FkComprobanteVenta",
                        column: x => x.FkComprobanteVenta,
                        principalTable: "ComprobanteVentas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetalleVentas_Productos_FkProducto",
                        column: x => x.FkProducto,
                        principalTable: "Productos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ErroresFacturasElectronicas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FkComprobanteVenta = table.Column<int>(type: "int", nullable: false),
                    Empidfk = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FelectEstado = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FelectAmbiente = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FelectNumeroautorizacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FelectFechaautorizacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FelectIdentificador = table.Column<int>(type: "int", nullable: true),
                    FelectMensaje = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FelectInformacionadicional = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FelectTipo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FelectComprobantexml = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FkComprobanteVentaNavigationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErroresFacturasElectronicas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ErroresFacturasElectronicas_ComprobanteVentas_FkComprobanteVentaNavigationId",
                        column: x => x.FkComprobanteVentaNavigationId,
                        principalTable: "ComprobanteVentas",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RutasXmls",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FkComprobanteVenta = table.Column<int>(type: "int", nullable: false),
                    RutaGenerado = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RutaFirmado = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RutaAutorizado = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RutaPdf = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RutaEstaRecepcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RutaEstaAturizacion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RutasXmls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RutasXmls_ComprobanteVentas_FkComprobanteVenta",
                        column: x => x.FkComprobanteVenta,
                        principalTable: "ComprobanteVentas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_FkEmpresa",
                table: "AspNetUsers",
                column: "FkEmpresa",
                unique: true,
                filter: "[FkEmpresa] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CajaGlobals_EmpresasCreadas",
                table: "CajaGlobals",
                column: "EmpresasCreadas");

            migrationBuilder.CreateIndex(
                name: "IX_CajaGlobals_FkEmpresa",
                table: "CajaGlobals",
                column: "FkEmpresa");

            migrationBuilder.CreateIndex(
                name: "IX_CajaGlobals_FkLocal",
                table: "CajaGlobals",
                column: "FkLocal");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_EmpresasCreadas",
                table: "Clientes",
                column: "EmpresasCreadas");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_FkEmpresa",
                table: "Clientes",
                column: "FkEmpresa");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_FkLocal",
                table: "Clientes",
                column: "FkLocal");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_FkRespositorioCedulas",
                table: "Clientes",
                column: "FkRespositorioCedulas");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_FkSrirepositorio",
                table: "Clientes",
                column: "FkSrirepositorio");

            migrationBuilder.CreateIndex(
                name: "IX_ComprobanteCompras_FkEmpresa",
                table: "ComprobanteCompras",
                column: "FkEmpresa");

            migrationBuilder.CreateIndex(
                name: "IX_ComprobanteCompras_FkEmpresasCreada",
                table: "ComprobanteCompras",
                column: "FkEmpresasCreada");

            migrationBuilder.CreateIndex(
                name: "IX_ComprobanteCompras_FkLocal",
                table: "ComprobanteCompras",
                column: "FkLocal");

            migrationBuilder.CreateIndex(
                name: "IX_ComprobanteCompras_FkProvedor",
                table: "ComprobanteCompras",
                column: "FkProvedor");

            migrationBuilder.CreateIndex(
                name: "IX_ComprobanteVentas_FkCliente",
                table: "ComprobanteVentas",
                column: "FkCliente");

            migrationBuilder.CreateIndex(
                name: "IX_ComprobanteVentas_FkEmpresa",
                table: "ComprobanteVentas",
                column: "FkEmpresa");

            migrationBuilder.CreateIndex(
                name: "IX_ComprobanteVentas_FkEmpresasCreada",
                table: "ComprobanteVentas",
                column: "FkEmpresasCreada");

            migrationBuilder.CreateIndex(
                name: "IX_ComprobanteVentas_FkLocal",
                table: "ComprobanteVentas",
                column: "FkLocal");

            migrationBuilder.CreateIndex(
                name: "IX_ComprobanteVentas_FkSecuencial",
                table: "ComprobanteVentas",
                column: "FkSecuencial");

            migrationBuilder.CreateIndex(
                name: "IX_ComprobanteVentas_FkTipoComprobante",
                table: "ComprobanteVentas",
                column: "FkTipoComprobante");

            migrationBuilder.CreateIndex(
                name: "IX_ComprobanteVentas_FkUsuario",
                table: "ComprobanteVentas",
                column: "FkUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_CorreoEmpresas_FkEmpresa",
                table: "CorreoEmpresas",
                column: "FkEmpresa",
                unique: true,
                filter: "[FkEmpresa] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CorreoEmpresas_FkEmpresasCreadas",
                table: "CorreoEmpresas",
                column: "FkEmpresasCreadas",
                unique: true,
                filter: "[FkEmpresasCreadas] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CorreoEmpresas_FkLocal",
                table: "CorreoEmpresas",
                column: "FkLocal");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesFacturaCompras_FkFacturaCompra",
                table: "DetallesFacturaCompras",
                column: "FkFacturaCompra");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleVentas_FkComprobanteVenta",
                table: "DetalleVentas",
                column: "FkComprobanteVenta");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleVentas_FkProducto",
                table: "DetalleVentas",
                column: "FkProducto");

            migrationBuilder.CreateIndex(
                name: "IX_Empresas_FkSriRepositorio",
                table: "Empresas",
                column: "FkSriRepositorio");

            migrationBuilder.CreateIndex(
                name: "IX_EmpresasCreadas_FkEmpresa",
                table: "EmpresasCreadas",
                column: "FkEmpresa");

            migrationBuilder.CreateIndex(
                name: "IX_EmpresasCreadas_FkSriRepositorio",
                table: "EmpresasCreadas",
                column: "FkSriRepositorio");

            migrationBuilder.CreateIndex(
                name: "IX_ErroresFacturasElectronicas_FkComprobanteVentaNavigationId",
                table: "ErroresFacturasElectronicas",
                column: "FkComprobanteVentaNavigationId");

            migrationBuilder.CreateIndex(
                name: "IX_Locals_FkEmpresa",
                table: "Locals",
                column: "FkEmpresa");

            migrationBuilder.CreateIndex(
                name: "IX_Locals_FkEmpresasCreadas",
                table: "Locals",
                column: "FkEmpresasCreadas");

            migrationBuilder.CreateIndex(
                name: "IX_Locals_FkSriRepositorio",
                table: "Locals",
                column: "FkSriRepositorio");

            migrationBuilder.CreateIndex(
                name: "IX_PermisosEmpresas_FkEmpresa",
                table: "PermisosEmpresas",
                column: "FkEmpresa",
                unique: true,
                filter: "[FkEmpresa] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PermisosEmpresas_FkEmpresasCreadas",
                table: "PermisosEmpresas",
                column: "FkEmpresasCreadas",
                unique: true,
                filter: "[FkEmpresasCreadas] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PermisosEmpresas_FkLocal",
                table: "PermisosEmpresas",
                column: "FkLocal",
                unique: true,
                filter: "[FkLocal] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_FkEmpresa",
                table: "Productos",
                column: "FkEmpresa");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_FkEmpresasCreada",
                table: "Productos",
                column: "FkEmpresasCreada");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_FkLocal",
                table: "Productos",
                column: "FkLocal");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_FkUsuario",
                table: "Productos",
                column: "FkUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Proveedores_FkEmpresa",
                table: "Proveedores",
                column: "FkEmpresa");

            migrationBuilder.CreateIndex(
                name: "IX_Proveedores_FkEmpresasCreada",
                table: "Proveedores",
                column: "FkEmpresasCreada");

            migrationBuilder.CreateIndex(
                name: "IX_Proveedores_FkLocal",
                table: "Proveedores",
                column: "FkLocal");

            migrationBuilder.CreateIndex(
                name: "IX_Proveedores_FkSriRepositorio",
                table: "Proveedores",
                column: "FkSriRepositorio");

            migrationBuilder.CreateIndex(
                name: "IX_RutasXmls_FkComprobanteVenta",
                table: "RutasXmls",
                column: "FkComprobanteVenta");

            migrationBuilder.CreateIndex(
                name: "IX_Secuencials_FkEmpresa",
                table: "Secuencials",
                column: "FkEmpresa");

            migrationBuilder.CreateIndex(
                name: "IX_Secuencials_FkEmpresasCreada",
                table: "Secuencials",
                column: "FkEmpresasCreada");

            migrationBuilder.CreateIndex(
                name: "IX_Secuencials_FkLocal",
                table: "Secuencials",
                column: "FkLocal");

            migrationBuilder.CreateIndex(
                name: "IX_Secuencials_FkTipoCompronbante",
                table: "Secuencials",
                column: "FkTipoCompronbante",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_FkEmpresa",
                table: "Usuarios",
                column: "FkEmpresa");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_FkEmpresasCreada",
                table: "Usuarios",
                column: "FkEmpresasCreada");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_FkLocal",
                table: "Usuarios",
                column: "FkLocal");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CajaGlobals");

            migrationBuilder.DropTable(
                name: "CorreoEmpresas");

            migrationBuilder.DropTable(
                name: "DetallesFacturaCompras");

            migrationBuilder.DropTable(
                name: "DetalleVentas");

            migrationBuilder.DropTable(
                name: "ErroresFacturasElectronicas");

            migrationBuilder.DropTable(
                name: "FormasPagos");

            migrationBuilder.DropTable(
                name: "PermisosEmpresas");

            migrationBuilder.DropTable(
                name: "RutasXmls");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "ComprobanteCompras");

            migrationBuilder.DropTable(
                name: "Productos");

            migrationBuilder.DropTable(
                name: "ComprobanteVentas");

            migrationBuilder.DropTable(
                name: "Proveedores");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Secuencials");

            migrationBuilder.DropTable(
                name: "RespositorioCedulas");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Comprobantes");

            migrationBuilder.DropTable(
                name: "Locals");

            migrationBuilder.DropTable(
                name: "EmpresasCreadas");

            migrationBuilder.DropTable(
                name: "Empresas");

            migrationBuilder.DropTable(
                name: "SriRepositorios");
        }
    }
}
