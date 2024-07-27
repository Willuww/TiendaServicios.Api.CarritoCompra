using MediatR;
using TiendaServicios.Api.CarritoCompra.Modelo;
using TiendaServicios.Api.CarritoCompra.Persistencia;

namespace TiendaServicios.Api.CarritoCompra.Aplicacion
{
    public class Nuevo
    {
        public class Ejecuta : IRequest { 
        
            public DateTime? FechaCreacionSesion { get; set; }
            public List<string> ProductoLista { get; set; }

        }
        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly CarritoContexto _context;
            public Manejador(CarritoContexto context)
            {
                _context = context;
            }
            public async Task<Unit> Handle(Ejecuta request,
                CancellationToken cancellationToken)
            {
                var carritoSesion = new CarritoSesion
                {
                    FechaCreacion = request.FechaCreacionSesion
                };
                _context.CarritoSesiones.Add(carritoSesion);
                var result = await _context.SaveChangesAsync();
                if (result == 0)
                {
                    throw new Exception("No se pudo agregar al carrito prro");
                }
                int id = carritoSesion.CarritoSesionId;
                //insertamos todos los elementos de la lista del carrito de compras
                foreach (var p in request.ProductoLista)
                {
                    var detalleSesion = new CarritoSesionDetalle
                    {
                        FechaCreacion = DateTime.Now,
                        CarritoSesionId = id,
                        ProductoSeleccionado = p
                    };
                    _context.CarritoSesionDetalle.Add(detalleSesion);
                }
                var value = await _context.SaveChangesAsync();

                if (value > 0)
                {
                    return Unit.Value;
                }
                throw new Exception("No se insertó el detalle del carrito de compras");
            }
        }
    }
}
