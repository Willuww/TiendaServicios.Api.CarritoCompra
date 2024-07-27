using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Text.Json;
using TiendaServicios.Api.CarritoCompra.RemoteInterface;
using TiendaServicios.Api.CarritoCompra.RemoteModel;

namespace TiendaServicios.Api.CarritoCompra.RemoteServices
{
    public class LibrosService : ILibroService
    {
        private readonly IHttpClientFactory httpClient;
        private readonly ILogger<LibrosService> logger;

        public LibrosService(IHttpClientFactory httpClient, ILogger<LibrosService> logger)
        {
            this.httpClient = httpClient;
            this.logger = logger;
        }


        public async Task<(bool resultado, LibroRemote Libro, string ErrorMessage)> GetLibro(Guid LibroId)
        {
            try
            {
                //creamos nuestro objeto que se comunicará con los endpoint o las api
                var cliente = httpClient.CreateClient("Libros");
                //nos comunicamos con nuestros endpoint que estamos solicitando
                var response = await cliente.GetAsync($"api/LibroMaterial/{LibroId}"); //devuelve un jso tipo response
                if (response.IsSuccessStatusCode)
                {
                    var contenido = await response.Content.ReadAsStringAsync();//leemos el contenido de la respuesta
                                                                               //especificamos que no hay problema por la estructura del json como venga, mayus o minus
                    var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                    var resultado = JsonSerializer.Deserialize<LibroRemote>(contenido, options);
                    return (true, resultado, null);
                }
                return (false, null, response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
    }

}
