namespace SistemaReservasExcursiones.Dtos.Reserva
{
    public class ReservaDto
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public int ExcursionId { get; set; }
        public int CantidadParticipantes { get; set; }
        public decimal MontoTotal { get; set; }
        public bool Pagada { get; set; }
        public bool Cancelada { get; set; }
        public DateTime FechaReserva { get; set; }
        public DateTime? FechaPago { get; set; }
        public string Observacion { get; set; } = string.Empty;
    }
}