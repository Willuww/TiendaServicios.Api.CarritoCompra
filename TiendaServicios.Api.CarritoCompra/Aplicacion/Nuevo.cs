using MediatR;

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
            pulic Manejador(CarritoContexto context)
            {
                _context = context;
            }
        }
    }
}
