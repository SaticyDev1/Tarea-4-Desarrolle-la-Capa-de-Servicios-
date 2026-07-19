using SistemaReservasExcursiones.Core;
using SistemaReservasExcursiones.Dtos.Reserva;

namespace SistemaReservasExcursiones.Contract
{
    public interface IReservaService
    {
        ServiceResult CrearReserva(ReservaCreateDto dto);
        ServiceResult ActualizarReserva(int id, ReservaUpdateDto dto);
        ServiceResult CancelarReserva(int id);
        ServiceResult MarcarReservaComoPagada(int id);
        ServiceResult ObtenerReservaPorId(int id);
        ServiceResult ObtenerReservas();
    }
}