using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using TiendaServicios.Api.CarritoCompra.Persistencia;
using TiendaServicios.Api.CarritoCompra.RemoteInterface;
using TiendaServicios.Api.CarritoCompra.RemoteModel;

namespace TiendaServicios.Api.CarritoCompra.Aplicacion
{
    public class Consulta
    {
        public class Ejecuta : IRequest<CarritoDto> { 
        ///recibe este valor como parametro
        public int CarritoSessionId { get; set; }
        }

        public class Manejador : IRequestHandler<Ejecuta, CarritoDto>
        {
            private readonly CarritoContexto carritoContexto;
            private readonly ILibroService libroService;
            public Manejador(CarritoContexto _carritoContexto,
                ILibroService _libroService)
            { 
                carritoContexto = _carritoContexto;
                libroService = _libroService;
            }

            public async Task<CarritoDto> Handle(Ejecuta request,
                CancellationToken cancellationToken)
            {
                //Ontenemos el carrito almacenado en la base de datos pasando el 
                var carritoSesion = await carritoContexto.CarritoSesiones.FirstOrDefaultAsync(x => x.CarritoSesionId ==
                    request.CarritoSessionId);
                //Devuelce la lista de producto detalle solo para conocer el
                var carritoSessionDetalle = await carritoContexto.
                    CarritoSesionDetalle.Where(x => x.CarritoSesionId ==
                    request.CarritoSessionId).ToListAsync();

                var listaCarritoDto = new List<CarritoDetalleDdto>();

                foreach (var libro in carritoSessionDetalle)
                {
                    //invocamos a la microservice externa
                    var response = await libroService.
                        GetLibro(new System.Guid(libro.ProductoSeleccionado));
                    if (response.resultado)
                    {
                        //se accede si se encuentra algo en la base de datos
                        var objetoLibro = response.Libro; //retorno un libroRemo
                        var carritoDetalle = new CarritoDetalleDdto
                        {
                            TituloLibro = objetoLibro.Titulo,
                            FechaPublicacion = objetoLibro.FechaPublicacion,
                            LibroId = objetoLibro.LibreriaMateriaId
                        };
                        listaCarritoDto.Add(carritoDetalle);
                    }
                }
                //llenamos el objeto que realmente es necesario retornar
                var carritoSessionDto = new CarritoDto
                {
                    CarritoId = carritoSesion.CarritoSesionId,
                    FechaCreacionSesion = carritoSesion.FechaCreacion,
                    LlistaDeProductos = listaCarritoDto
                };
                return carritoSessionDto;
            }
        }
    }
}
