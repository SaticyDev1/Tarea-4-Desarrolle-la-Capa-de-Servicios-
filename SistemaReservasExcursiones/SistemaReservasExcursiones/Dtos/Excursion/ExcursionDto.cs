namespace SistemaReservasExcursiones.Dtos.Excursion
{
    public class ExcursionDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public decimal Precio { get; set; }
        public int CupoMaximo { get; set; }
        public int CuposDisponibles { get; set; }
        public int GuiaId { get; set; }
        public bool Activa { get; set; }
    }
}
