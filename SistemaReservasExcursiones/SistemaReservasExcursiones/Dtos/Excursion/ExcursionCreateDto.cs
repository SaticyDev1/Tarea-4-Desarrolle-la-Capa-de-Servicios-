namespace SistemaReservasExcursiones.Dtos.Excursion
{
    public class ExcursionCreateDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public decimal Precio { get; set; }
        public int CupoMaximo { get; set; }
        public int GuiaId { get; set; }
    }
}