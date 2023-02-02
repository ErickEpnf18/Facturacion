namespace Facturacion.Services.Interfaces
{
    public interface IFacturacionElectronica
    {
        Task<string> EditarArchivo(byte[] contenido, string extension, string contenedor, string ruta,
            string contentType, string claveAcceso);
        Task BorrarArchivo(string ruta, string contenedor);
        Task<string> GuardarArchivo(byte[] contenido, string extension, string contenedor, string claveAcceso);
        Task<string> GuardarP12(byte[] contenido, string extension, string contenedor, string contentType);
    }
}
