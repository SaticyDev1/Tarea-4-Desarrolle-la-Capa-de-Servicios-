namespace SistemaReservasExcursiones.Dtos.Reserva
{
    public class ReservaUpdateDto
    {
        public int CantidadParticipantes { get; set; }
        public bool Pagada { get; set; }
        public bool Cancelada { get; set; }
        public string Observacion { get; set; } = string.Empty;
    }
}