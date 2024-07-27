namespace TiendaServicios.Api.CarritoCompra.Aplicacion
{
    public class CarritoDto
    {
        public int CarritoId { get; set; }
        public DateTime? FechaCreacionSesion {get; set;}
        public List<CarritoDetalleDdto> LlistaDeProductos { get; set; }

    }
}
