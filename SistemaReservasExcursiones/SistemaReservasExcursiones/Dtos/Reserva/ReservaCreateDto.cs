namespace SistemaReservasExcursiones.Dtos.Reserva
{
    public class ReservaCreateDto
    {
        public int ClienteId { get; set; }
        public int ExcursionId { get; set; }
        public int CantidadParticipantes { get; set; }
        public bool Pagada { get; set; }
        public string Observacion { get; set; } = string.Empty;
    }
}