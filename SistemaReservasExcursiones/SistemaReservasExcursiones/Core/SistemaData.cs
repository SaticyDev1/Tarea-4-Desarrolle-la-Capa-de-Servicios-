using SistemaReservasExcursiones.Models;

namespace SistemaReservasExcursiones.Core
{
    public static class SistemaData
    {
        public static List<Cliente> Clientes { get; } = new();
        public static List<Guia> Guias { get; } = new();
        public static List<Excursion> Excursiones { get; } = new();
        public static List<Reserva> Reservas { get; } = new();

        public static int SiguienteClienteId { get; set; } = 1;
        public static int SiguienteGuiaId { get; set; } = 1;
        public static int SiguienteExcursionId { get; set; } = 1;
        public static int SiguienteReservaId { get; set; } = 1;
    }
}